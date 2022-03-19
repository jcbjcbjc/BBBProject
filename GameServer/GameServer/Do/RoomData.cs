using System;

namespace GameServer
{
	[Serializable]
	public class RoomData
	{
		public int Id;
		public bool IsDeleted;
		public int RoomId;
		public int UserId;
		public int MaxCount;
	}
}

