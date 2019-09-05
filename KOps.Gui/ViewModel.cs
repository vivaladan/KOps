using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;


namespace KOps.Gui
{
    public class ViewModel : INotifyPropertyChanged
    {
        private SynchronizationContext syncContext;

        public ViewModel()
        {
            syncContext = SynchronizationContext.Current;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            else
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
        }

        protected void RunOnMainThread(Action callback)
        {
            if (syncContext != null)
            {
                syncContext.Post(o =>
                {
                    callback.Invoke();
                }, null);
            }
            else
            {
                callback.Invoke();
            }
        }

        protected EventHandler<T> HandleEventOnMainThread<T>(Action<object, T> callback)
        {
            return new EventHandler<T>((sender, e) =>
            {
                if (syncContext != null)
                {
                    syncContext.Post(o =>
                    {
                        callback.Invoke(sender, e);
                    }, null);
                }
                else
                {
                    callback.Invoke(sender, e);
                }
            });
        }

        public virtual void Loaded()
        {

        }
    }
}
