using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Common
{
	[Serializable]
	public class ClientUserData
	{
		public int Id;
		public string Name;
		public int TotalCount;
		public int WinCount;
	}

	[Serializable]
	public class ClientRoomData
	{
		public int GamePattern;
		public int RoomId;
		public int MaxCount;
	}
	[Serializable]
	public class ClientRoomInfo {
		public List<ClientUserData> _clientUserDatas;
		public ClientRoomInfo(List<ClientUserData> clientUserDatas) {
			_clientUserDatas=clientUserDatas;
		}
	}

	#region GameData
	[Serializable]
	public class Game_BaseData
	{
		public int RoomPositionID;
		public List<MyPoint> MapData;
		public List<Game_PlayerData> PlayerData;

		public Game_BaseData(int ID, List<MyPoint> myPoints, List<Game_PlayerData> game_PlayerDatas) {
			RoomPositionID = ID;
			MapData = myPoints;
			PlayerData = game_PlayerDatas;
		}
		public Game_BaseData() { 

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

		public Game_PositionData(int id,int row,int col, int z) {
			_id= id;
			_row= row;
			_col= col;
			_z= z;
		}
	}
	[Serializable]
	public class Game_InstructionData
	{
		public int _id;

		public Game_InstructionData(int id)
		{
			_id = id;
		}
	}
	[Serializable]
	public class OverGameData
	{
		public int winnerid;
		public string winnerusername;

		public OverGameData(int id, string name)
		{
			winnerid = id;
			winnerusername = name;
		}
	}
	#endregion


	[Serializable]
	public class MessageData {
		public string _data;

		public MessageData(string data) {
			_data = data;
		}
	}
	[Serializable]
	public class MessageDatas
	{
		public string param1;
		public string param2;
		public MessageDatas(string data,string datas)
		{
			param1 = data;
			param2 = datas;
		}
	}

    #region UIData
    [Serializable]
	public class UI_Set_UpdateData
	{
		public string _messageType;
		public int _MaxCount;
		public int _current_id;
		public bool _is_player_set;
		public bool _is_player_ban;
		public int _Movement;

		public UI_Set_UpdateData(string messageType,int MaxCount,int current_id,bool is_player_set,bool is_player_ban,int Movement) { 
			_messageType = messageType;
			_MaxCount = MaxCount;
			_current_id = current_id;
			_is_player_set=is_player_set;
			_is_player_ban=is_player_ban;
			_Movement=Movement;
		}
		public UI_Set_UpdateData(string messageType, int current_id) {
			_messageType = messageType;
			_current_id = current_id;
		}
	}
	[Serializable]
	public class Init_Set_UpdateData
	{
		public int _MaxCount;
		public int _Myid;
		public Init_Set_UpdateData(int MaxCount, int Myid)
		{
			_MaxCount = MaxCount;
			_Myid = Myid;
		}
	}
    #endregion

    [Serializable]
	public class AudioData
	{
		public string AudioType;
		public string Instructions;
		public AudioData(string audioType,string instructions) {
			AudioType = audioType;
			Instructions = instructions;
		}
	}

	[Serializable]
	public class LoginData {
		public string _nameInput;
		public string _pwdInput;

		public LoginData(string nameInput,string pwdInput) {
			_nameInput=nameInput;
			_pwdInput=pwdInput;
		}
	}


	[Serializable]
	public class JoinRoomResponseData
	{
		public int _error;
		public ClientRoomData _clientRoomData;
		public ClientRoomInfo _clientRoomInfo;
		
		public JoinRoomResponseData(ClientRoomData data, ClientRoomInfo RoomInfo, int error)
		{
			_clientRoomData = data;
			_clientRoomInfo = RoomInfo;
			_error = error;
		}
		public JoinRoomResponseData(int error)
		{
			_error = error;
		}
	}
}
