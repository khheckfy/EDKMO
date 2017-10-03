using EDKMO.BusinessLogic.BusinessModels;
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

        public EventService(IUnitOfWork db)
        {
            DB = db;
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
                        e.StartDate >= dFrom && e.EndDate <= dTo
                        select new
                        {
                            e.AccountId,
                            EndDate = DbFunctions.AddHours(e.EndDate, e.User.Territory.UTCHours),
                            StartDate = DbFunctions.AddHours(e.StartDate, e.User.Territory.UTCHours),
                            e.EventId,
                            e.EventName,
                            e.LongDescription,
                            e.ShortDescription,
                            e.ReportMoId,
                            e.TerritoryId,
                            e.UserId,
                            e.User.Territory.UTCHours
                        };

            return query.ToList();
        }

        public async Task<SchedulerDataObject> ScheduleObject()
        {
            await Task.FromResult(0);

            return new SchedulerDataObject()
            {
                Resources = DB.UserRepository.Query().ToList(),
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
                    UserId = evnt.UserId
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
                        EventName = road.Name,
                        StartDate = roadStart,
                        EndDate = roadEnd,
                        EventTypeId = road.EventTypeId,
                        TerritoryId = evnt.TerritoryId,
                        UserId = evnt.UserId
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
                        EventName = road.Name,
                        StartDate = roadStart,
                        EndDate = roadEnd,
                        EventTypeId = road.EventTypeId,
                        TerritoryId = evnt.TerritoryId,
                        UserId = evnt.UserId
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
    }
}
