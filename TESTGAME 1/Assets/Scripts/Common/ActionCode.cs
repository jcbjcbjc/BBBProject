using System;

namespace Common
{
	public class ActionCode
	{
		public const int Default = 0;
		public const int Login = 1;
		public const int Register = 2;
		public const int GetResultInfo = 3;
		public const int CreateRoom = 4;
		public const int GetRoomList = 5;
		public const int JoinRoom = 6;
		public const int LeaveRoom = 7;
		public const int GetRoomInfo = 8;
		public const int StartGame = 9;
		public const int StopGame = 10;
		public const int StartGameConfirm = 11;
		public const int Ready = 12;
		public const int NoReady = 13;
	}
	public enum UIMessage
	{
		/// <summary>空</summary>
		ENull = 0,
		/// <summary>打开窗口<summary>
		OpenForm = 1,
		/// <summary>关闭窗口<summary>
		CloseForm = 2
	}
	public enum AudioMessage { 

	}
	public enum GameMessage { 

	} 
}

