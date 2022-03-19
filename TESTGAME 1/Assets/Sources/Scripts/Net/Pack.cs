using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net
{
	[Serializable]
	public class TcpSendData
	{
		//Type
		public string MessageType;
		//Data
		public object Data;

	}
	[Serializable]
	public class TcpDataFromServer
	{
		//Type
		public string MessageType;

		public object Data;
		
	}

}
