using System;
using System.Collections.Generic;
using System.Text;

namespace KOps.Gui
{
    public class GroupViewModel : ViewModel
    {
        private string uri;
        private string name;

        public GroupViewModel()
        {
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
    }
}
