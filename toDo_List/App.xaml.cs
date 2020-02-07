using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using toDo_List.DAL;
using toDo_List.Migrations;

namespace toDo_List
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        //override 

        protected override void OnStartup(StartupEventArgs e)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ToDo_DbContext, Configuration>());
            base.OnStartup(e);
        }
    }
}
