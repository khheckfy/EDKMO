namespace EDKMO.Data.EntityFramework
{
    using Domain.Entities;
    using System.Data.Entity;

    public class Model : DbContext
    {
        // Your context has been configured to use a 'Model' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EDKMO.Data.EntityFramework.Model' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model' 
        // connection string in the application configuration file.
        public Model()
            : base("name=Model")
        {
        }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Territory> Territories { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
            .HasRequired(c => c.Territory)
            .WithMany()
            .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

    }
}