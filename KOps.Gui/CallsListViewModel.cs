using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using KOps.Application;

namespace KOps.Gui
{
    public class CallsListViewModel : ViewModel
    {
        public ObservableCollection<CallViewModel> Calls { get; }
            = new ObservableCollection<CallViewModel>();
            //{
            //    new CallViewModel { CallId = 1, CallStatus = "Connected", FloorStatus = FloorStatus.Idle, Transmitting = false, Highlight = new SolidColorBrush() },
            //    new CallViewModel { CallId = 2, CallStatus = "Connected", FloorStatus = FloorStatus.Idle, Transmitting = false, Highlight = new SolidColorBrush() },
            //    new CallViewModel { CallId = 3, CallStatus = "Connected", FloorStatus = FloorStatus.Granted, Transmitting = true, Highlight = new SolidColorBrush(Colors.LightGreen)},
            //    new CallViewModel { CallId = 4, CallStatus = "Connected", FloorStatus = FloorStatus.Idle, Transmitting = false, Highlight = new SolidColorBrush() },
            //    new CallViewModel { CallId = 5, CallStatus = "Connected", FloorStatus = FloorStatus.Granted, Transmitting = true, Highlight = new SolidColorBrush(Colors.LightGreen) },
            //};

        internal void CallUpdate(CallUpdate callUpdate)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var call = Calls.FirstOrDefault(c => c.CallId == callUpdate.CallId);

                if (call == null)
                {
                    call = new CallViewModel();
                    Calls.Add(call);
                }

                call.CallId = callUpdate.CallId;
                call.ChannelId = callUpdate.ChannelId;
                call.CallStatus = callUpdate.CallStatus;
                call.FloorStatus = callUpdate.FloorStatus;
                call.Talker = callUpdate.Talker;
                call.GroupId = callUpdate.GroupId;

                call.Transmitting = callUpdate.FloorStatus == FloorStatus.Granted;
                call.Receiving = callUpdate.FloorStatus == FloorStatus.Taken;
                call.Highlight = call.Transmitting ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush();
            });
        }
    }
}
