using System;
using System.Collections.Generic;
using System.Text;

namespace KOps.Gui
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel(
            CallsListViewModel callsListViewModel,
            CallControlViewModel callControlViewModel)
        {
            CallsListViewModel = callsListViewModel;
            CallControlViewModel = callControlViewModel;
        }

        public CallsListViewModel CallsListViewModel { get; }
        public CallControlViewModel CallControlViewModel { get; }
    }
}
