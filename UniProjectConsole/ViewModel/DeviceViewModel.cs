using Application.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel
{
    class DeviceViewModel : INotifyPropertyChanged
    {
        private DeviceManager _manager;

        public DeviceViewModel(DeviceManager manager)
        {
            this._manager = manager;

            _manager.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
