using Application.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel
{
    // Data Setup for ViewModel
    partial class DeviceViewModel : INotifyPropertyChanged
    {
        private DeviceManager _manager;

        //Data Binding Variables


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
