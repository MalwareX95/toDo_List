using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toDo_List.DAL
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Descrption { get; set; }
        public DateTime Date { get; set; }
    }

    public class ToDo_DbContext : DbContext
    {
        public DbSet<ToDoItem> ToDos { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<ToDoItem>()
                        .HasKey(x => x.Id, buildaction => buildaction.IsClustered(false));
            modelBuilder.Entity<ToDoItem>()
                        .HasIndex(x => x.Date)
                        .IsClustered(true);

        }
    }
}
