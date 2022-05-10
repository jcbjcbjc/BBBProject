using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GameCode
    {
		public const int Set = 1;
		public const int Ban = 2;
		public const int Move = 3;
		public const int Brick = 4;
		public const int Break = 5;
		public const int ToSet = 6;
		public const int ToBan = 7;
		public const int EndPre = 8;
		public const int StartPre = 9;
		public const int Convert = 10;
		public const int ConditionSuccess = 11;
		public const int Success = 12;
		public const int EnterGame = 13;
		public const int ConveyPoint = 14;
	}
	public class UIEventArgs : EventArgs
	{
		public int _UIEventCode;
		public Data _data;
		public UIEventArgs(int UIEventCode, Data data)
		{
			this._UIEventCode = UIEventCode;
			this._data = data;
		}
	}
	public class GameEventArgs : EventArgs { 

	}
	public class AudioEventArgs : EventArgs
	{
		public int _AudioEventCode;
		public Data _data;
		public AudioEventArgs(int AudioEventCode, Data data)
		{
			this._AudioEventCode = AudioEventCode;
			this._data = data;
		}
	}
}
