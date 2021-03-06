﻿using EDKMO.BusinessLogic.BusinessModels;
using EDKMO.BusinessLogic.DTO;
using EDKMO.BusinessLogic.Interfaces;
using EDKMO.Domain;
using EDKMO.Domain.Entities;
using System;
using EDKMO.Core.Extensions;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using DevExpress.XtraScheduler;

namespace EDKMO.BusinessLogic.Services
{
    public class EventService : IEventService
    {
        IUnitOfWork DB;
        private bool IsClient { set; get; }

        public EventService(IUnitOfWork db)
        {
            DB = db;
        }

        public EventDTO Get(object id)
        {
            EventDTO dto = new EventDTO();

            dto = (from e in DB.EventRepository.Query()
                   where
                   e.EventId == (int)id
                   select new EventDTO()
                   {
                       IsMainEvent = e.IsMainEvent,
                       AccountId = e.AccountId,
                       EndDate = e.EndDate,
                       EventColor = e.EventType.Color,
                       EventId = e.EventId,
                       EventName = e.EventName,
                       EventTypeId = e.EventTypeId,
                       EventTypeName = e.EventType.Name,
                       FaIcon = e.EventType.FaIcon,
                       LongDescription = e.LongDescription,
                       ReportMoId = e.ReportMoId,
                       ShortDescription = e.ShortDescription,
                       StartDate = e.StartDate,
                       UserId = e.UserId,
                       TerritoryId = e.TerritoryId,
                       ROUrl = e.Territory.ServerPath,
                       UTCHours = e.User.Territory.UTCHours,
                       RoId = e.RoId,
                   }).FirstOrDefault();

            if (dto.RoId.HasValue)
            {
                dto.RoIdName = DB.TerritoryRepository.FindById(dto.RoId.Value)?.Name;
            }

            return dto;
        }

        public async Task Remove(int id)
        {
            var obj = await DB.EventRepository.FindByIdAsync(id);
            if (obj != null)
            {
                if (obj.ReportMoId.HasValue)
                {
                    var list = await DB.EventRepository.Query().Where(n => n.ReportMoId == obj.ReportMoId.Value).ToListAsync();
                    foreach (var l in list)
                        DB.EventRepository.Remove(l);
                }
                else
                    DB.EventRepository.Remove(obj);
                await DB.SaveChangesAsync();
            }
        }


        private object FetchAppointmentsHelperMethod(FetchAppointmentsEventArgs args)
        {
            args.ForceReloadAppointments = true;

            DateTime dFrom = args.Interval.Start.Date.AddDays(-7);
            DateTime dTo = args.Interval.End.Date.AddDays(1).AddSeconds(-1).AddDays(7);

            var query = from e in DB.EventRepository.Query()
                        where
                        IsClient == false && e.StartDate >= dFrom && e.EndDate <= dTo ||
                        IsClient == true && e.ClientStartDate >= dFrom && e.ClientEndDate <= dTo
                        select new
                        {
                            e.AccountId,
                            EndDate = DbFunctions.AddHours(IsClient == false ? e.EndDate : e.ClientEndDate, e.User.Territory.UTCHours),
                            StartDate = DbFunctions.AddHours(IsClient == false ? e.StartDate : e.ClientStartDate, e.User.Territory.UTCHours),
                            e.EventId,
                            e.EventName,
                            e.LongDescription,
                            e.ShortDescription,
                            e.ReportMoId,
                            e.TerritoryId,
                            e.UserId,
                            e.User.Territory.UTCHours,
                            e.RoId,
                        };

            return query.ToList();
        }

        public async Task<SchedulerDataObject> ScheduleObject(List<byte> resources, bool isClientTime)
        {
            var resList = await DB
                .UserRepository
                .Query()
                .ToListAsync();

            if (resources.Count > 0)
                resList = resList.Where(n => resources.Contains(n.UserId)).ToList();

            IsClient = isClientTime;
            return new SchedulerDataObject()
            {
                Resources = resList,
                FetchAppointments = FetchAppointmentsHelperMethod
            };
        }

        public async Task<List<EventDTO>> ListByDate(DateTime date, byte? territoryId = null, byte? userId = null)
        {
            var data = await DB.EventRepository.ListByDate(date, territoryId, userId);
            var eventTypes = await DB.EventTypeRepository.GetAllAsync();
            var result = Mapper.Map<List<Event>, List<EventDTO>>(data);

            result.ForEach(e =>
            {
                var temp = eventTypes.Single(n => n.EventTypeId == e.EventTypeId);
                e.RefEventName = temp.Name;
                e.FaIcon = temp.FaIcon;
                e.EventColor = temp.Color;
            });

            return result;
        }

        public async Task UpdateEvent(EventDTO evnt)
        {
            try
            {
                Event obj = await DB.EventRepository.FindByIdAsync(evnt.EventId);

                obj.EventName = evnt.EventName;
                obj.LongDescription = evnt.LongDescription;
                obj.ShortDescription = evnt.ShortDescription;

                await DB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EventCreateResult> CreateEvent(EventDTO evnt)
        {
            EventCreateResult result = new EventCreateResult();

            try
            {
                Territory territory = await DB.TerritoryRepository.FindByIdAsync(evnt.TerritoryId);
                EventType road = null;
                if (evnt.RoadTo.Hours > 0 || evnt.RoadTo.Minutes > 0 || evnt.RoadAfter.Hours > 0 || evnt.RoadAfter.Minutes > 0)
                {
                    road = DB.EventTypeRepository.Query().FirstOrDefault(n => n.Name == "дорога");
                    if (road == null)
                        throw new Exception("Событие \"Дорога\" не найдено");
                }


                evnt.EndDate = evnt.StartDate.Add(evnt.EventDuration);
                Event obj = new Event()
                {
                    AccountId = evnt.AccountId,
                    ReportMoId = evnt.ReportMoId,
                    CreatedOn = DateTime.Now,
                    EventName = evnt.EventName,
                    EndDate = evnt.EndDate,
                    EventTypeId = evnt.EventTypeId,
                    LongDescription = evnt.LongDescription,
                    ShortDescription = evnt.ShortDescription,
                    StartDate = evnt.StartDate,
                    TerritoryId = evnt.TerritoryId,
                    UserId = evnt.UserId,
                    IsMainEvent = true,
                    RoId = evnt.RoId,
                };

                //Приводим все к гринвичу
                obj.StartDate = obj.StartDate.AddHours((-1) * territory.UTCHours);
                obj.EndDate = obj.EndDate.AddHours((-1) * territory.UTCHours);

                TimeSpan tsFrom = obj.StartDate.TimeOfDay;
                TimeSpan tsTo = obj.EndDate.TimeOfDay;
                var userEvents = await ListByDate(evnt.StartDate, null, evnt.UserId);
                //Получили все события этого чувака на день и надо проверить попадание периодов
                var e = userEvents
                        .FirstOrDefault(n => n.StartDate.TimeOfDay >= tsFrom && n.EndDate.TimeOfDay <= tsTo);
                if (e == null)
                    e = userEvents
                          .FirstOrDefault(n => n.StartDate.TimeOfDay <= tsFrom && n.EndDate.TimeOfDay <= tsTo && tsTo > n.StartDate.TimeOfDay && n.EndDate.TimeOfDay > tsFrom);
                if (e == null)
                    e = userEvents
                          .FirstOrDefault(n => n.StartDate.TimeOfDay >= tsFrom && n.EndDate.TimeOfDay >= tsTo && n.StartDate.TimeOfDay < tsTo);
                if (e == null)
                    e = userEvents
                          .FirstOrDefault(n => n.StartDate.TimeOfDay < tsFrom && n.EndDate.TimeOfDay >= tsTo);
                if (e != null)
                    throw new Exception(string.Format("На этого менеджера уже есть задание, входящее в выбранный период события. {0}-{1}", e.StartDate, e.EndDate));

                DB.EventRepository.Add(obj);

                if (evnt.RoadTo.Hours > 0 || evnt.RoadTo.Minutes > 0)
                {
                    DateTime roadStart = obj.StartDate - evnt.RoadTo;
                    DateTime roadEnd = obj.StartDate - TimeSpan.FromMinutes(1);

                    DB.EventRepository.Add(new Event()
                    {
                        AccountId = evnt.AccountId,
                        ReportMoId = evnt.ReportMoId,
                        CreatedOn = DateTime.Now,
                        StartDate = roadStart,
                        EndDate = roadEnd,
                        EventTypeId = road.EventTypeId,
                        TerritoryId = evnt.TerritoryId,
                        UserId = evnt.UserId,
                        IsMainEvent = false,
                        RoId = evnt.RoId,
                    });

                    tsFrom = roadStart.TimeOfDay;
                    tsTo = roadEnd.TimeOfDay;
                    e = userEvents
                        .FirstOrDefault(n => n.StartDate.TimeOfDay >= tsFrom && n.EndDate.TimeOfDay <= tsTo);
                    if (e == null)
                        e = userEvents
                              .FirstOrDefault(n => n.StartDate.TimeOfDay <= tsFrom && n.EndDate.TimeOfDay <= tsTo && tsTo > n.StartDate.TimeOfDay && n.EndDate.TimeOfDay > tsFrom);
                    if (e == null)
                        e = userEvents
                              .FirstOrDefault(n => n.StartDate.TimeOfDay >= tsFrom && n.EndDate.TimeOfDay >= tsTo && n.StartDate.TimeOfDay < tsTo);
                    if (e == null)
                        e = userEvents
                              .FirstOrDefault(n => n.StartDate.TimeOfDay < tsFrom && n.EndDate.TimeOfDay >= tsTo);
                    if (e != null)
                        throw new Exception(string.Format("Невозможно назначить дорогу до. На этого менеджера уже есть задание, входящее в выбранный период события. {0}-{1}", e.StartDate, e.EndDate));
                }

                if (evnt.RoadAfter.Hours > 0 || evnt.RoadAfter.Minutes > 0)
                {
                    DateTime roadStart = obj.EndDate + TimeSpan.FromMinutes(1);
                    DateTime roadEnd = obj.EndDate + evnt.RoadAfter;

                    DB.EventRepository.Add(new Event()
                    {
                        AccountId = evnt.AccountId,
                        ReportMoId = evnt.ReportMoId,
                        CreatedOn = DateTime.Now,
                        StartDate = roadStart,
                        EndDate = roadEnd,
                        EventTypeId = road.EventTypeId,
                        TerritoryId = evnt.TerritoryId,
                        UserId = evnt.UserId,
                        IsMainEvent = false,
                        RoId = evnt.RoId,
                    });

                    tsFrom = roadStart.TimeOfDay;
                    tsTo = roadEnd.TimeOfDay;
                    e = userEvents
                        .FirstOrDefault(n => n.StartDate.TimeOfDay >= tsFrom && n.EndDate.TimeOfDay <= tsTo);
                    if (e == null)
                        e = userEvents
                              .FirstOrDefault(n => n.StartDate.TimeOfDay <= tsFrom && n.EndDate.TimeOfDay <= tsTo && tsTo > n.StartDate.TimeOfDay && n.EndDate.TimeOfDay > tsFrom);
                    if (e == null)
                        e = userEvents
                              .FirstOrDefault(n => n.StartDate.TimeOfDay >= tsFrom && n.EndDate.TimeOfDay >= tsTo && n.StartDate.TimeOfDay < tsTo);
                    if (e == null)
                        e = userEvents
                              .FirstOrDefault(n => n.StartDate.TimeOfDay < tsFrom && n.EndDate.TimeOfDay >= tsTo);
                    if (e != null)
                        throw new Exception(string.Format("Невозможно назначить дорогу до. На этого менеджера уже есть задание, входящее в выбранный период события. {0}-{1}", e.StartDate, e.EndDate));
                }



                await DB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ErrorDump = ex.CreateExceptionDump();
            }

            return result;
        }


        public async Task<EventCreateResult> CreateBlockEvent(EventDTO evnt)
        {
            EventCreateResult result = new EventCreateResult();

            try
            {
                var user = await DB.UserRepository.FindByIdAsync(evnt.UserId);
                var territory = await DB.TerritoryRepository.FindByIdAsync(user.TerritoryId);

                //Надо перевести время во врмея пользователя по его территории
                evnt.StartDate = evnt.StartDate.AddHours((-1) * territory.UTCHours);
                evnt.EndDate = evnt.EndDate.AddHours((-1) * territory.UTCHours);

                evnt.ClientStartDate = evnt.ClientStartDate.AddHours((-1) * territory.UTCHours);
                evnt.ClientEndDate = evnt.ClientEndDate.AddHours((-1) * territory.UTCHours);

                var date = evnt.StartDate.Date;
                while (date <= evnt.EndDate.Date)
                {
                    var obj = new Event()
                    {
                        CreatedOn = DateTime.Now,

                        EndDate = date.Add(evnt.EndDate.TimeOfDay),
                        StartDate = date.Add(evnt.StartDate.TimeOfDay),

                        ClientEndDate = date.Add(evnt.ClientEndDate.TimeOfDay),
                        ClientStartDate = date.Add(evnt.ClientStartDate.TimeOfDay),

                        EventTypeId = evnt.EventTypeId,
                        LongDescription = evnt.LongDescription,
                        ShortDescription = evnt.ShortDescription,
                        TerritoryId = user.TerritoryId,
                        UserId = evnt.UserId,
                        IsMainEvent = true,
                        RoId = evnt.RoId,
                    };

                    DB.EventRepository.Add(obj);

                    date = date.AddDays(1);
                }
                await DB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ErrorDump = ex.CreateExceptionDump();
            }

            return result;
        }
    }
}
