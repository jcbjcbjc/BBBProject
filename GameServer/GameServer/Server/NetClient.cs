using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
namespace GameServer
{
    public class NetClient
	{
		Socket _socket;
		NetServer _server;
		byte[] _buffer = new byte[1024 * 1024];

		public int UserId;

		UserDo _userDo = new UserDo();
		ResultDo _resultDo = new ResultDo();
		RoomDo _roomDo = new RoomDo();

		SystemController controller = new SystemController();
		GameController gameController = new GameController();
		public Room Room;
		public int RoomId=0;

		public NetClient (Socket socket, NetServer server)
		{
			_socket = socket;
			_server = server;
			Start ();
		}

		public void Start()
		{
			_socket.BeginReceive (_buffer, 0, _buffer.Length, SocketFlags.None, StartReceiveCallback, _socket);
		}

		void StartReceiveCallback(IAsyncResult ar)
		{
			try {
				int length = _socket.EndReceive(ar);
				if (length == 0) {
					Close();
				}
				else if (length > 0) {
					string str = Encoding.UTF8.GetString(_buffer,0,length);
					TcpSendData Obj=JsonConvert.DeserializeObject<TcpSendData>(str);

					MessageCenter.SendMessage(Obj.MessageType,Obj.Data,this, _server);
				}
				Start();
			} catch (Exception ex) {
				Console.WriteLine ("无法接收消息：" + ex.Message);
				Close ();
			}
		}

		public int Send(string MessageType, object data)
		{
			try {
				TcpDataFromServer Obj = new TcpDataFromServer();
				Obj.MessageType = MessageType;
				Obj.Data = data;

				string Message = JsonConvert.SerializeObject(Obj);
				byte[] buffer = Encoding.UTF8.GetBytes(Message);
				return _socket.Send(buffer);
			} catch (Exception ex) {
				Console.WriteLine ("无法发送消息：" + ex.Message);
			}
			return -1;
		}

		public void Close()
		{
			try {
				if (RoomId != 0) {
					_roomDo.LeaveRoom(RoomId,UserId);
					RoomId = 0;
				}
				if (Room != null) {
					Room.Leave (this);
				}
				if (UserId != 0) {
					UserId = 0;
				}
				if (_socket != null) {
					_socket.Close();
				}
				_server.RemoveClient(this);
			} catch (Exception ex) {
				Console.WriteLine ("无法关闭连接：" + ex.Message);
			}
		}

		public bool Connected { get { return _socket != null && _socket.Connected; } }

		public string Address
		{
			get
			{
				if (Connected == true) {
					return _socket.RemoteEndPoint.ToString ();
				}
				return "";
			}
		}

		public string GetResultInfo(int userId)
		{
			_resultDo.Add(userId);
			StringBuilder sb = new StringBuilder();
			sb.Append(userId.ToString());
			sb.Append(";");
			sb.Append(_userDo.GetUserInfo(userId, "Name"));
			sb.Append(";");
			sb.Append(_resultDo.GetResultInfo(userId, "TotalCount"));
			sb.Append(";");
			sb.Append(_resultDo.GetResultInfo(userId, "WinCount"));
			return sb.ToString();
		}

		public ClientUserData GetUserInfo(int userId)
		{
			ClientUserData clientUserData = new ClientUserData(userId, _userDo.GetUserInfo(userId, "Name"), int.Parse(_resultDo.GetResultInfo(userId, "TotalCount")), int.Parse(_resultDo.GetResultInfo(userId, "WinCount")));
			_resultDo.Add (userId);
			
			return clientUserData;
		}

		public List<ClientUserData> GetRoomInfo()
		{
			List<ClientUserData> clientUserDatas = new List<ClientUserData> ();
			
			for (int i = 0; i < Room.GetClientCount(); i++) {
				clientUserDatas.Add(Room.GetClientList()[i].GetUserInfo(Room.GetClientList()[i].UserId));
			}
			
			return clientUserDatas;
		}
		public ClientRoomData GetRoomData()
		{
			return new ClientRoomData(Room.GetGamePattern(),Room.GetId(), Room.GetMaxCount());
		}
	}
}

