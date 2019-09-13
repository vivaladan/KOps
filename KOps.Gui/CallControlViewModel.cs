using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using KOps.Application;

namespace KOps.Gui
{
    public class CallControlViewModel : ViewModel
    {
        private readonly TalkgroupHandler talkgroupHandler;

        public ICommand Push { get; }
        public ICommand Release { get; }

        public CallControlViewModel(TalkgroupHandler talkgroupHandler)
        {
            Push = new DelegateCommand(OnPush);
            Release = new DelegateCommand(OnRelease);
            
            this.talkgroupHandler = talkgroupHandler;
        }

        private async void OnPush(object obj)
        {
            await talkgroupHandler.AcquireFloor();
        }

        private async void OnRelease(object obj)
        {
            await talkgroupHandler.ReleaseFloor();
        }
    }
}
