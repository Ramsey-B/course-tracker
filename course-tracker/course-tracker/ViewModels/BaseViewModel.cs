using course_tracker.Models;
using course_tracker.Services;
using coursetracker.SQL;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace course_tracker.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private static SQLConnection SqlConn => DependencyService.Get<SQLConnection>();
        public static SQLiteAsyncConnection SqliteConn = SqlConn.GetAsyncConnection();

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public Term GetDefaultTerm()
        {
            var terms = SqliteConn.Table<Term>().OrderBy(term => term.Start).ToListAsync().Result;
            var currentTerm = terms.FirstOrDefault(t => t.IsCurrent);

            if (currentTerm != null) return currentTerm;

            return terms.FirstOrDefault(t => t.Start >= DateTime.Now);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
