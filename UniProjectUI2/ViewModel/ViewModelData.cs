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
            InitializeDefaultValues();
        }

        private void InitializeDefaultValues(){
            DefaultLED();
        }

        private void DefaultLED(){
            _selectedLED = "Red"; // Directly set the backing field
            NotifyPropertyChanged(nameof(SelectedLED)); // Notify the UI of the change
        }

        //Data Binding Variables
        private int[] _latestPacket;
        private string _selectedLED;
        private byte _current;

        private bool _startReading;
        public bool StartReading{}{
            get{
                return _startReading;
            }
            set{
                if(_startReading != value)
                {                    
                    _startReading = value;
                    if(_startReading){
                      _manager.startRealTimeReading();
                    }else{
                      _manager.stopRealTimeReading();
                    }                        
                }                
            }
        }

        //Event-Caller - for Latest Packet
        public int[] VM_realTimePacket
        {
            get
            {
                return _latestPacket;
            }
            set
            {
                _latestPacket = value;
                NotifyPropertyChanged("VM_realTimePacket");
            }
        }
        //Commands to Model
        //Led selection
        public string SelectedLED
        {
            get
            {
                return _selectedLED;
            }
            set
            {
                if(_selectedLED != value)
                {                    
                    _selectedLED = value;
                    _manager.ChangeLed(_selectedLED);
                }                
            }
        }
        //Generic Event-Caller - for LED, Current and Resistance
        public string Current
        {
            get
            {
                return _current;
            }
            set
            {
                if(_current != value)
                {                    
                    _current = value;
                    int value = int.Parse(Current_inputbox.Text);
                    _manager.ChangeCurrent(int.Parse(_current));
                }                
            }
        }    

        public void StartRecording(int time){
            _manager.StartRecord(time);            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
