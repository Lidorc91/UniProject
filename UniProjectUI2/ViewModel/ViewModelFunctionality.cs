using Application.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Presentation Logic for the ViewModel
namespace Application.ViewModel
{
	partial class DeviceViewModel
	{
		public Packet GetData()
		{
           return this._manager.getLatestPacket();
		}
	}
}
