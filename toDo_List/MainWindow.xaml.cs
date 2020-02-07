using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using toDo_List.DAL;
using toDo_List.Views;

namespace toDo_List
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly ToDo_DbContext Db = new ToDo_DbContext();

        private readonly TimeSpan NotificationPeriod = TimeSpan.FromMinutes(3);

        private const int MaxNotificationCount = 5;

        private readonly Dictionary<ToDoItem, Timer> Timers = new Dictionary<ToDoItem, Timer>(MaxNotificationCount);

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public ObservableCollection<ToDoItem> _ToDos;

        public Timer CheckIncomingToDos;

        public Timer CreateTimer(ToDoItem toDoItem) => new Timer(TimerCallback, toDoItem, TimeSpan.Zero, NotificationPeriod);

        public ObservableCollection<ToDoItem> ToDos
        {
            get => _ToDos;
            set
            {
                _ToDos = value;
                OnPropertyChanged();
            }
        }

        public void TimerCallback(object state)
        {
            var toDoItem = state as ToDoItem;
            Timer timer;
            if (!Timers.TryGetValue(toDoItem, out timer)) return;
            DateTime dateTime;
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = $"Zadanie: {toDoItem.Date}",
                Message = toDoItem.Descrption,
                Type = NotificationType.Information
            }, expirationTime: TimeSpan.FromSeconds(10));

            if (toDoItem.Date <= DateTime.Now)
            {
                Timers.Remove(toDoItem);
            }
            else if ((dateTime = DateTime.Now.Add(NotificationPeriod)) > toDoItem.Date)
            {
                timer.Change(NotificationPeriod - (dateTime - toDoItem.Date), TimeSpan.FromMilliseconds(-1));
            }
        }

        public async void func(object state)
        {
            Db.SaveChanges();
            if (Timers.Count < MaxNotificationCount)
            {
                _ = Db.ToDos
                      .OrderBy(x => x.Date)
                      .Where(x => x.Date > DateTime.Now)
                      .Take(MaxNotificationCount)
                      .ToListAsync()
                      .ContinueWith(task =>
                      {
                          task.Result
                              .Except(Timers.Keys)
                              .Select(toDoItem => (timer: CreateTimer(toDoItem), toDoItem))
                              .ToList()
                              .ForEach(tuple => Timers.Add(tuple.toDoItem, tuple.timer));
                      });
            }
        }

        private void OnPropertyChanged([CallerMemberName] string property = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Closing += MainWindow_Closing;
            CheckIncomingToDos = new Timer(func, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Db.SaveChangesAsync();
            App.Current.Windows
                       .Cast<Window>()
                       .Except(new[] { App.Current.MainWindow })
                       .ToList()
                       .ForEach(x => x.Close());
        }

        private async void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = e.AddedItems[0] as DateTime?;
            await Db.SaveChangesAsync();
            ToDos = new ObservableCollection<ToDoItem>(await Db.ToDos.SqlQuery("Select * from ToDoItem Where CAST(Date as date) = {0}", date).ToListAsync());
            ToDos.CollectionChanged += (s, @event) =>
            {
                switch (@event.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        Db.Entry(@event.NewItems[0]).State = EntityState.Added;
                    break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        foreach(var item in @event.OldItems.Cast<ToDoItem>())
                        {
                            Db.Entry(item).State = EntityState.Deleted;
                            if(Timers.TryGetValue(item, out var timer))
                            {
                                timer.Dispose();
                                GC.SuppressFinalize(timer);
                                Timers.Remove(item);
                            }
                        }
                    break;
                }
                Db.SaveChanges();
            };
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) => ToDos?.Add(new ToDoItem { Date = SelectedDate });

        private void DeleteButton_Click(object sender, RoutedEventArgs e) => new List<ToDoItem>(Board.SelectedItems.Cast<ToDoItem>()).ForEach(x => ToDos?.Remove(x));

    }
}
