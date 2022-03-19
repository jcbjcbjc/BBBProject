using System;

namespace GameServer
{
	[Serializable]
	public class ResultData
	{
		public int Id;
		public bool IsDeleted;
		public int UserId;
		public int TotalCount;
		public int WinCount;
	}
}

