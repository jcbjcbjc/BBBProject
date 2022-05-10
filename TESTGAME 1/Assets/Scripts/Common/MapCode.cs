using System;

namespace Common
{
	public class MapCode
	{
		public const int Default = 0;
		public const int Set = 1;
		public const int Ban = 2;
		public const int Brick = 3;
		public const int StartorEndPoint = 4;
	}
	public class Data {
		string _data;
	}
	public class PlayerData:Data {
		public int ID;
		public string name;
		public MyPoint StartPoint;
		public int appearance;
		public MyPoint EndPoint;
	}
	public class PositionData : Data {
		public int row;
		public int col;
	}
}
