using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	[Serializable]
	public class ClientUserData
	{
		public int Id;
		public string Name;
		public int TotalCount;
		public int WinCount;
		public ClientUserData(int id,string name,int totaocount,int wincount) {
			Id=id;
			Name = name;
			TotalCount=totaocount;
			WinCount=wincount;
		}
	}
	[Serializable]
	public class ClientRoomData
	{
		public int GamePattern;
		public int RoomId;
		public int MaxCount;
		public ClientRoomData(int pat,int Rid,int Mcount) {
			GamePattern=pat;
			RoomId = Rid;
			MaxCount=Mcount;
		}
	}
	[Serializable]
	public class ClientRoomInfo
	{
		List<ClientUserData> _clientUserDatas;
		public ClientRoomInfo(List<ClientUserData> clientUserDatas)
		{
			_clientUserDatas = clientUserDatas;
		}
	}
	
	[Serializable]
	public class MessageData
	{
		public string _data;

		public MessageData(string data)
		{
			_data = data;
		}
	}
	[Serializable]
	public class MessageDatas
	{
		public string param1;
		public string param2;
		public MessageDatas(string data, string datas)
		{
			param1 = data;
			param2 = datas;
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
	public class UI_Set_UpdateData
	{
		public string _messageType;
		public int _MaxCount;
		public int _current_id;
		public bool _is_player_set;
		public bool _is_player_ban;
		public int _Movement;

		public UI_Set_UpdateData(string messageType, int MaxCount, int current_id, bool is_player_set, bool is_player_ban, int Movement)
		{
			_messageType = messageType;
			_MaxCount = MaxCount;
			_current_id = current_id;
			_is_player_set = is_player_set;
			_is_player_ban = is_player_ban;
			_Movement = Movement;
		}
		public UI_Set_UpdateData(string messageType, int current_id)
		{
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
	[Serializable]
	public class AudioData
	{
		public string AudioType;
		public string Instructions;
		public AudioData(string audioType, string instructions)
		{
			AudioType = audioType;
			Instructions = instructions;
		}
	}
	[Serializable]
	public class LoginData
	{
		public string _nameInput;
		public string _pwdInput;

		public LoginData(string nameInput, string pwdInput)
		{
			_nameInput = nameInput;
			_pwdInput = pwdInput;
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
