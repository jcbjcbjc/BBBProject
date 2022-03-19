using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
					var strs = str.Split('|');
					int requestCode = int.Parse(strs[0]);
					int actionCode = int.Parse(strs[1]);
					string data = strs[2];
					_server.OnRequest(requestCode,actionCode,data,this);
				}
				Start();
			} catch (Exception ex) {
				Console.WriteLine ("无法接收消息：" + ex.Message);
				Close ();
			}
		}

		public int Send(int requestCode, int actionCode, string data)
		{
			try {
				byte[] buffer = Encoding.UTF8.GetBytes(requestCode + "|" + actionCode + "|" + data);
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
			_resultDo.Add (userId);
			StringBuilder sb = new StringBuilder ();
			sb.Append (userId.ToString ());
			sb.Append (";");
			sb.Append (_userDo.GetUserInfo (userId, "Name"));
			sb.Append (";");
			sb.Append (_resultDo.GetResultInfo (userId, "TotalCount"));
			sb.Append (";");
			sb.Append (_resultDo.GetResultInfo (userId, "WinCount"));
			return sb.ToString ();
		}

		public string GetRoomInfo()
		{
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < Room.GetClientCount(); i++) {
				sb.Append (Room.GetClientList()[i].GetResultInfo (Room.GetClientList()[i].UserId) + "#");
			}
			if (sb.Length == 0) {
				sb.Append ("0");
			}
			else if (sb.Length > 0) {
				sb.Remove (sb.Length - 1, 1);
			}
			return sb.ToString ();
		}
		public string GetRoomInformation()
		{
			return Room.GetId().ToString()+";"+ Room.GetGamePattern().ToString();
		}
	}
}

