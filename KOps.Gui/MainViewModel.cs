using System;
using System.Collections.Generic;
using System.Text;

namespace KOps.Gui
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel(
            GroupsListViewModel groupsListViewModel,
            CallsListViewModel callsListViewModel,
            CallControlViewModel callControlViewModel)
        {
            GroupsListViewModel = groupsListViewModel;
            CallsListViewModel = callsListViewModel;
            CallControlViewModel = callControlViewModel;
        }

        public GroupsListViewModel GroupsListViewModel { get; }
        public CallsListViewModel CallsListViewModel { get; }
        public CallControlViewModel CallControlViewModel { get; }
    }
}
