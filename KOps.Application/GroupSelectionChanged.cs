using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace KOps.Application
{
    public class GroupSelectionChanged : INotification
    {
        public GroupSelectionChanged(string name, string uri, bool selected)
        {
            this.Name = name;
            this.Uri = uri;
            this.Selected = selected;
        }

        public string Name { get; }

        public string Uri { get; }

        public bool Selected { get; }
    }
}
