using Application.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModel
{
    // Data Setup for ViewModel
    partial class DeviceViewModel : INotifyPropertyChanged
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

        //Data Binding Variables
        private int[] _latestPacket;
        private string _selectedLed;
        private byte _Current;
        private byte _Resistance

        //Event-Caller - for Latest Packet
        public int[] VM_latestPacket
        {
            get
            {
                return _latestPacket;
            }
            set
            {
                _latestPacket = value;
                NotifyPropertyChanged("VM_latestPacket");
            }
        }
        //Event-Caller - led selection
        public string SelectedLED
        {
            get
            {
                return _selectedLed;
            }
            set
            {
                if(_selectedLed != value)
                {                    
                    _selectedLed = value;
                    _manager.ChangeLed(_selectedLed);
                }                
            }
        }
        //Generic Event-Caller - for LED, Current and Resistance
        public byte Variable
        {
            get
            {
                return _variable;
            }
            set
            {
                _variable = value;
                NotifyPropertyChanged("VM_"+nameof(Variable));
            }
        }

        

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
