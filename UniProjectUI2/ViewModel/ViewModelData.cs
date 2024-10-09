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

        public DeviceViewModel(DeviceManager manager)
        {
            this._manager = manager;
            _manager.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            InitializeDefaultValues();

            //Testing
            Stopwatch stopwatch = new Stopwatch();         
        }

        private void InitializeDefaultValues(){
            DefaultLED();
            _startReading = false;
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
        public bool StartReading
        {
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
        public byte Current
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
        private bool _testClicked;
        public bool TestClicked
        {
            get{
                return TestClicked;
            }
            set{
                if(TestClicked != value)
                {
                    //stopwatch.Start();                
                    TestClicked = value;
                    _manager.ModelTest()
                }                
            }
        }

        private string _testText;
        public bool VM_TestText
        {
            get{
                return VM_TestText;
            }
            set{
                if(VM_TestText != value)
                {                    
                    VM_TestText = value;
                    NotifyPropertyChanged(nameof(VM_TestText));                    
                }                
            }
        }
    }
}
