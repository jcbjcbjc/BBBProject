using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace GameServer
{
	public class NetServer
	{
		Socket _socket;
		List<NetClient> _clientList = new List<NetClient> ();
		List<Room> _roomList = new List<Room> ();

		public NetServer (string ip, int port)
		{
			_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_socket.Bind (new IPEndPoint (IPAddress.Parse (ip), port));
			_socket.Listen (1000);
			Console.WriteLine ("启动服务器成功");
			_socket.BeginAccept (StartAcceptCallback, _socket);
		}

		void StartAcceptCallback(IAsyncResult ar)
		{
			try {
				Socket clientSocket = _socket.EndAccept(ar);
				NetClient client = new NetClient(clientSocket, this);
				_clientList.Add(client);
				_socket.BeginAccept (StartAcceptCallback, _socket);
			} catch (Exception ex) {
				Console.WriteLine ("无法接收连接：" + ex.Message);
			}
		}

		public void RemoveClient(NetClient client)
		{
			_clientList.Remove (client);
		}

		public void Close()
		{
			if (_clientList == null || _clientList.Count <= 0) {
				return;
			}
			for (int i = 0; i < _clientList.Count; i++) {
				lock (_clientList[i]) {
					_clientList [i].Close ();
				}
			}
		}

		public bool IsOnLine(string userName)
		{
			if (_clientList == null || _clientList.Count <= 0) {
				return false;
			}
			UserDo userDo = new UserDo ();
			for (int i = 0; i < _clientList.Count; i++) {
				string name = userDo.GetUserInfo (_clientList [i].UserId, "Name");
				if (name == userName && _clientList[i].UserId != 0) {
					return true;
				}
			}
			userDo = null;
			return false;
		}
		public NetClient GetClient(int userId)
		{
			if (_clientList == null || _clientList.Count <= 0) {
				return null;
			}
			for (int i = 0; i < _clientList.Count; i++) {
				if (_clientList[i].UserId == userId) {
					return _clientList [i];
				}
			}
			return null;
		}
		public void LeaveRoom(int RoomID, NetClient client)
		{
			var room = GetRoom(RoomID);
			if (room == null)
			{
				Console.WriteLine("房间不存在");
				return;
			}
			if (!room.GetClientList().Contains(client))
			{
				return;
			}
			room.Leave(client);
		}
		public void CreateRoom(NetClient client,ClientRoomData clientRoomData)
		{
			if (client.Room == null) {
				Room room = new Room (this, clientRoomData.GamePattern, clientRoomData.MaxCount);
				room.AddClient (client);
				_roomList.Add (room);
			}
		}
		public bool JoinRoom(int RoomID, NetClient client) {
			var room = GetRoom(RoomID);
			if (room == null)
			{
				Console.WriteLine("房间不存在");
				return false;
			}
			if (room.GetClientCount() >= room.GetMaxCount())
			{
				Console.WriteLine("房间人满了");
				return false;
			}
			room.AddClient(client);
			return true;
		}
		public void RemoveRoom(Room room)
		{
			if (_roomList == null || _roomList.Count <= 0) {
				return;
			}
			_roomList.Remove (room);
		}
		public Room GetRoom(int roomId)
		{
			for (int i = 0; i < _roomList.Count; i++) {
				if (_roomList[i].GetId() == roomId) {
					return _roomList [i];
				}
			}
			return null;
		}
		public List<Room> GetRoomList()
		{
			return _roomList;
		}

		

		public void SendResponse(string MessageType, object data, NetClient client)
		{
			client.Send (MessageType, data);
		}
	}
}

