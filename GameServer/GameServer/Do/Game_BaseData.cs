using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	[Serializable]
	public class Game_BaseData
	{
		public int RoomPositionID;

		public List<MyPoint> MapData;
		public List<Game_PlayerData> PlayerData;

		public Game_BaseData(int ID, List<MyPoint> myPoints, List<Game_PlayerData> game_PlayerDatas)
		{
			RoomPositionID = ID;
			MapData = myPoints;
			PlayerData = game_PlayerDatas;
		}
		public Game_BaseData()
		{

		}
	}
	[Serializable]
	public class Game_BaseDataFromServer
	{
		public Game_BaseData _game_BaseData;
		public int _error;
		public Game_BaseDataFromServer(int error)
		{
			_error = error;
		}
		public Game_BaseDataFromServer(int error, Game_BaseData game_BaseData)
		{
			_error = error;
			_game_BaseData = game_BaseData;
		}
	}

	[Serializable]
	public class Game_PlayerData
	{
		public int UserID;
		public int RoomPositionID;
		public string name;
		public MyPoint StartPoint;
		public int appearance;
		public MyPoint EndPoint;
		public float Direction;
		public Vec3 CameraPosition;
	}
	[Serializable]
	public class Game_PositionData
	{
		public int _id;
		public int _row;
		public int _col;
		public int _z;

		public Game_PositionData(int id, int row, int col, int z)
		{
			_id = id;
			_row = row;
			_col = col;
			_z = z;
		}
	}
}
