using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using KOps.Application;
using MediatR;

namespace KOps.Gui
{
    public class GroupsListViewModel : ViewModel
    {
        private readonly IMediator mediator;

        public ObservableCollection<GroupViewModel> Groups { get; }
            = new ObservableCollection<GroupViewModel>();

        public GroupsListViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        internal void GroupUpdate(GroupUpdate groupUpdate)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var group = Groups.FirstOrDefault(g => g.Uri == groupUpdate.Uri);

                if (group == null)
                {
                    group = new GroupViewModel(mediator);
                    Groups.Add(group);
                }

                group.Uri = groupUpdate.Uri;
                group.Name = groupUpdate.Name;
            });
        }
    }
}
