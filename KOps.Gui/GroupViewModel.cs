using System;
using System.Collections.Generic;
using System.Text;
using KOps.Application;
using MediatR;

namespace KOps.Gui
{
    public class GroupViewModel : ViewModel
    {
        private readonly IMediator mediator;
        private string uri;
        private string name;
        private bool selected;

        public GroupViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public string Uri
        {
            get => uri;
            set => SetProperty(ref uri, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public bool Selected
        {
            get => selected;
            set
            {
                mediator.Publish(
                    new GroupSelectionChanged(
                        Name, 
                        Uri, 
                        value));

                SetProperty(ref selected, value);
            }
        }
    }
}
