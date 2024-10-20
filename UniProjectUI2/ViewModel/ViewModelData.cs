using Application.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Diagnostics;

namespace Application.ViewModel
{
    // Data Setup for ViewModel
    partial class DeviceViewModel : INotifyPropertyChanged
    {
        private DeviceManager _manager;

        //Testing
        private Stopwatch stopwatch = new Stopwatch();

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
            _isReading = false;
        }

        private bool _isReading;
        public bool IsReading
        {
            get{
                return _isReading;
            }
            set{
                if(_isReading != value)
                {
                    _isReading = value;
                    _manager.RealTimeReading(_isReading);
                }                
            }
        }

        //Event-Caller - for Latest Packet
        public int[] VM_realTimePacket
        {
            get
            {
                return _manager.realTimePacket.GetProcessedData();
            }
            set
            {
            }
        }
        //Commands to Model
        //Led selection
        public string SelectedLED
        {
            set
            {
                _manager.ChangeLed(value);                
            }
        }
        //Generic Event-Caller - for LED, Current and Resistance
        private byte _current;
        public byte Current
        {
            set
            {
                if(_current != value)
                {                    
                    _current = value;
                    _manager.ChangeCurrent(_current);
                }                
            }
        }    

        public void StartRecording(byte time){
            _manager.record(time);            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        //Testing
        private bool _testClicked = false;
        public bool TestClicked
        {
            get{
                return _testClicked;
            }
            set{
                if(_testClicked != value)
                {
                    stopwatch.Start();                
                    _testClicked = value;
                    _manager.ModelTest();
                }                
            }
        }

        public string VM_TestText
        {
            get{
                stopwatch.Stop();
                return stopwatch.ElapsedMilliseconds.ToString();
            }
            set { }
            
        }
    }
}
