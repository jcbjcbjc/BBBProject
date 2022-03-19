using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;


namespace GameServer
{
	public class Room
	{
		NetServer _server;
		List<NetClient> _clientList = new List<NetClient> ();

		int _maxCount;
		int _roomState = RoomCode.WaitJoin;
		int GamePattern=-1;

		List<NetClient> ReadyList = new List<NetClient>();
		List<NetClient> ConfirmStartList = new List<NetClient>();

		public bool IsWaitJoin { get { return _roomState == RoomCode.WaitJoin; } }
		UserDo _userDo = new UserDo();


		public Room (NetServer server, int Pattern,int Maxcount)
		{
			_server = server;
			GamePattern = Pattern;
			_maxCount=Maxcount;
			_roomState = RoomCode.WaitJoin;
		}
		public int GetGamePattern()
		{
			return GamePattern;
		}
		public int GetMaxCount() { 
			return _maxCount;
		}
		public int GetClientCount()
		{
			return _clientList.Count;
		}
		public List<NetClient> GetClientList()
		{
			return _clientList;
		}
		public void AddClient(NetClient client)
		{
			if (_clientList.Count >= _maxCount) {
				Console.WriteLine ("房间满人了");
				return;
			}
			if (_roomState == RoomCode.WaitJoin) {
				if (_clientList.Contains (client) == false) {
					client.Room = this;
					_clientList.Add (client);
					client.RoomId = GetId();
				}
			}
			if (_clientList.Count >= _maxCount) {
				_roomState = RoomCode.WaitBattle;
			}
		}
		public int Ready(NetClient client) {
			if (client.RoomId == 0) { return 0; }
			if (client.Room.GetId() == client.UserId)
			{
				return 0;
			}
			if (!ReadyList.Contains(client)) { 
				ReadyList.Add(client);
				return 1;
			}
			return 0;
		}
		public void ConfirmStart(NetClient client)
		{
			if (client.RoomId == 0) { return; }

			if (!ConfirmStartList.Contains(client))
			{
				ConfirmStartList.Add(client);
			}
		}
		public void NoReady(NetClient client) {
			if (client.RoomId == 0) { return; }
			if (_roomState == RoomCode.Battle) { return; }
			if (ReadyList.Contains(client))
			{
				ReadyList.Remove(client);
			}
		}
		public bool isStartGame() {
			if (ReadyList.Count == _maxCount - 1)
			{
				return true;
			}
			return false;
		}
		public bool IsEnterGame() {
			if (ConfirmStartList.Count == _maxCount)
			{
				return true;
			}
			return false;
		}
		public void RemoveClient(NetClient client)
		{
			if (_clientList.Contains(client)) {
				client.Room = null;
				client.RoomId = 0;
				_clientList.Remove (client);
				Console.WriteLine("房间[" + this.GetId() + "][" + client.UserId + "]离开房间");
			}
			if (_clientList.Count < _maxCount) {
				_roomState = RoomCode.WaitJoin;
			}
			else if (_clientList.Count == _maxCount) {
				_roomState = RoomCode.WaitBattle;
			}
		}

		public void Leave(NetClient client)
		{
			if (IsHouseOwner(client)) {
				Close ();
			} 
			else {
				RemoveClient (client);
			}
		}

		public void Close()
		{
			for (int i = 0; i < _clientList.Count; i++) {
				_clientList [i].Room = null;
				_clientList[i].RoomId = 0;
			}
			Console.WriteLine("房间[" + this.GetId() + "]已经销毁");
			_server.RemoveRoom (this);
		}

		public int GetId()
		{
			return _clientList[0].UserId;
		}

		public bool IsHouseOwner(NetClient client)
		{
			if (_clientList == null || _clientList.Count <= 0) {
				return false;
			}
			return _clientList [0] == client;
		}

		public string GetHouseOwnerInfo()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(GetId().ToString());
			sb.Append(";");
			sb.Append(_userDo.GetUserInfo(_clientList[0].UserId, "Name"));
			sb.Append(";");
			sb.Append(GetClientCount().ToString());
			sb.Append(";");
			sb.Append(GetMaxCount().ToString());
			sb.Append(";");
			sb.Append(GetGamePattern().ToString());
			return sb.ToString();
		}

		public string GetRoomInfo()
		{
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < _clientList.Count; i++) {
				sb.Append (_clientList [i].GetResultInfo (_clientList[i].UserId) + "#");
			}
			if (sb.Length == 0) {
				sb.Append ("0");
			}
			else if (sb.Length > 0) {
				sb.Remove (sb.Length - 1, 1);
			}
			return sb.ToString ();
		}

		public void SetRoomStatus(int status) {
			if (status == RoomCode.Battle)
			{
				_roomState = RoomCode.Battle;
			}
			else if (status == RoomCode.WaitBattle) {
				_roomState = RoomCode.WaitBattle;
			}
		}
		public void CloseGame() {
			SetRoomStatus(RoomCode.WaitBattle);
			ReadyList.Clear();
			ConfirmStartList.Clear();
		}
	}
}

