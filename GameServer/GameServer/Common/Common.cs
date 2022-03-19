using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	public class MyPoint
	{
		public int row;
		public int col;
		public int status;
		public MyPoint()
		{
			this.row = 0;
			this.col = 0;
			status = 0;
		}
		public MyPoint(int row, int col)
		{
			this.row = row;
			this.col = col;
			status = 0;
		}


		//为什么引用不了啊啊啊啊啊
	}
	public class Vec3 {
		public float x;
		public float y;
		public float z;
		public Vec3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
	public class MapCode
	{
		public const int Default = 0;
		public const int Set = 1;
		public const int Ban = 2;
		public const int Brick = 3;
		public const int StartorEndPoint = 4;
		public const int Player = 5;
	}
	public class PatternCode
	{
		public const int Multiple = 1;
		public const int single = 2;
		public const int classic = 3;
		public const int hexgon = 4;
		public const int quartic = 5;
		public const int Setting = 6;
	}
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
