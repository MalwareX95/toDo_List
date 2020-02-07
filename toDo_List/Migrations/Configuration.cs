namespace toDo_List.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<toDo_List.DAL.ToDo_DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(toDo_List.DAL.ToDo_DbContext context)
        {
            //context.Database.ExecuteSqlCommand("")
            //  This method will be called after migrating to the latest version.
            //context.ToDos.AddOrUpdate(new DAL.ToDoItem {Id = 1, Title = "Pójœæ do sklepu", Description = "sad" , Date = DateTime.Now.Date });
            //context.ToDos.AddOrUpdate(new DAL.ToDoItem {Id = 2, Title = "Posprz¹taæ w domu", Description = "sad", Date = DateTime.Now.Date.AddDays(-1)});
            //context.SaveChanges();
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
