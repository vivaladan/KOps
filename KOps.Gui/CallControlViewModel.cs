using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using KOps.Application;

namespace KOps.Gui
{
    public class CallControlViewModel : ViewModel
    {
        private readonly ICdeApi cdeApi;

        public ICommand Push { get; }
        public ICommand Release { get; }

        public CallControlViewModel(ICdeApi cdeApi)
        {
            Push = new DelegateCommand(OnPush);
            Release = new DelegateCommand(OnRelease);
            this.cdeApi = cdeApi;
        }

        private async void OnPush(object obj)
        {
            await cdeApi.AcquireFloor();
        }

        private async void OnRelease(object obj)
        {
            await cdeApi.ReleaseFloor();
        }
    }
}
