using System;
using System.Text;
using Common;

namespace GameServer
{
	public class RoomController : BaseController 
	{
		public override string OnRequest (int requestCode, int actionCode, string data, NetClient client, NetServer server)
		{
			if (actionCode == ActionCode.CreateRoom)
			{
				return CreateRoom(data, client, server);
			}
			else if (actionCode == ActionCode.GetRoomList)
			{
				return GetRoomList(data, client, server);
			}
			else if (actionCode == ActionCode.JoinRoom)
			{
				return JoinRoom(data, client, server);
			}
			else if (actionCode == ActionCode.LeaveRoom)
			{
				return LeaveRoom(data, client, server);
			}
			else if (actionCode == ActionCode.StartGame) {
				return StartGame(data, client, server);
			}
			else if (actionCode == ActionCode.StopGame)
			{
				return StopGame(data, client, server);
			}
			else if (actionCode == ActionCode.Ready)
			{
				return Ready(data, client, server);
			}
			else if (actionCode == ActionCode.NoReady)
			{
				return NoReady(data, client, server);
			}
			else if (actionCode == ActionCode.StartGameConfirm)
			{
				return ConfirmStart(data, client, server);
			}
			/*
			else if (actionCode == ActionCode.GetRoomInfo) {
				return GetRoomInfo (data, client, server);
			}
			*/
			return "";
		}
		string StartGame(string data, NetClient client, NetServer server) {
			if (client.RoomId == 0) { return""; }
			var room = client.Room;
			if (room.GetId() != client.UserId) {
				Console.WriteLine("您不是房主");
				return "-2"; 
			}
			if (room.isStartGame())
			{
				Console.WriteLine("房间{0}开始对战",client.RoomId);
				for (int i = 0; i < room.GetClientCount(); i++)
				{
					room.GetClientList()[i].Send(RequestCode.Room, ActionCode.StartGame, (i + 1).ToString() + ";" + room.GetGamePattern().ToString());
				}
				room.SetRoomStatus(RoomCode.Battle);
			}
			else {
				Console.WriteLine("玩家还没准备");
				return "-1";
			}
			return "";
		}
		string StopGame(string data, NetClient client, NetServer server)
		{
			client.Room.CloseGame();
			return "";
		}
		string CreateRoom(string data, NetClient client, NetServer server)
		{
			Console.WriteLine ("开始创建房间 能否创建房间：" + (client.RoomId == 0));
			if (client.RoomId != 0) {
				client.Room.Leave(client);
				//_roomDo.LeaveRoom (client.RoomId, client.UserId);
				client.RoomId = 0;
				Console.WriteLine("移除房间");
			}

			//_roomDo.CreateRoom (client.UserId, client.UserId, 2);



			client.RoomId = client.UserId;
            if (client.Room != null)
            {
                return "";
            }
			Console.WriteLine("正在创建房间中");
			Console.WriteLine("创建的房间类型为{0}", int.Parse(data));
			server.CreateRoom(client, int.Parse(data));
			Console.WriteLine("创建房间成功");
			return client.GetRoomInformation();
        }

        string GetRoomList(string data, NetClient client, NetServer server)
		{
			//var roomList = _roomDo.GetHouseOwnerList ();
			//if (roomList == null || roomList.Count <= 0) {
			//	return "0";
			//}
			//StringBuilder sb = new StringBuilder ();
			//for (int i = 0; i < roomList.Count; i++) {
			//	var m_client = server.GetClient (roomList [i].RoomId);
			//	if (m_client != null) {
			//		sb.Append (m_client.GetResultInfo (roomList [i].RoomId) + "#");
			//	}
			//}
			//if (sb.Length == 0) {
			//	sb.Append ("0");
			//}
			//if (sb.Length > 0) {
			//	sb.Remove (sb.Length - 1, 1);
			//}
			//return sb.ToString ();

            var roomList = server.GetRoomList();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < roomList.Count; i++)
            {
                sb.Append(roomList[i].GetHouseOwnerInfo() + "#");
            }
            if (sb.Length == 0)
            {
                sb.Append("0");
            }
            else if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();

        }

        //TODO
        #region 解决房主第一次离开房间后的创建房间和加入房间不起作用问题？

        #endregion

        string JoinRoom(string data, NetClient client, NetServer server)
		{
			if (client.RoomId != 0) {
				client.Room.Leave(client);
				client.RoomId = 0;
			}

			int roomId = int.Parse (data);

			client.RoomId = roomId;
			server.JoinRoom(roomId, client);
			Broadcast (client, server, ActionCode.JoinRoom,""+"*"+ ""+"*"+client.GetRoomInfo ());
			client.Send (RequestCode.Room, ActionCode.JoinRoom, roomId.ToString()+"*"+ client.Room.GetGamePattern().ToString()+ "*" +client.GetRoomInfo ());
			return "";
		}

		string LeaveRoom(string data, NetClient client, NetServer server)
		{
			if (client.RoomId <= 0) {
				return "";
			}
			if (client.RoomId == client.UserId) {
				Broadcast (client, server, ActionCode.LeaveRoom, "0");
				server.LeaveRoom(client.RoomId, client);
				client.RoomId = 0;
			}
			else {
				Console.WriteLine("离开房间后的将更新房间信息为" + client.GetRoomInfo());
				Broadcast (client, server, ActionCode.LeaveRoom, client.GetRoomInfo ());
				server.LeaveRoom(client.RoomId, client);
				client.RoomId = 0;
			}
			client.RoomId = 0;
			return "2";
		}
		string Ready(string data, NetClient client, NetServer server) {
			var room=client.Room;
			int ok=client.Room.Ready(client);
			if (ok == 1)
			{
				return "1";
			}
			return"";
		}
		string ConfirmStart(string data, NetClient client, NetServer server) {
			Console.WriteLine("玩家{0}已经准备就绪", client.UserId);
			client.Room.ConfirmStart(client);
			if (client.Room.IsEnterGame()) {
				client.Room.GetClientList()[0].Send(RequestCode.Game, GameCode.EnterGame, "");
			}
			return "";
		}
		string NoReady(string data, NetClient client, NetServer server)
		{
			client.Room.NoReady(client);
			return "";
		}
		/*
		string GetRoomInfo(string data, NetClient client, NetServer server)
		{
			if (client.RoomId == 0) {
				return "";
			}
			var room = _roomDo.GetRoom (client.RoomId);
			Broadcast (client, server, ActionCode.GetRoomInfo, client.GetRoomInfo ());

			return client.GetRoomInfo ();
		}
		*/

		int Broadcast(NetClient client, NetServer server, int actionCode, string data)
		{
			var room = client.Room;
			if (client.Room == null) {
				Console.WriteLine("房间为空");
				return -1; 
			}
			for (int i = 0; i < room.GetClientCount(); i++) {
				if (room.GetClientList()[i].UserId != client.UserId) {
					return room.GetClientList()[i].Send (RequestCode.Room, actionCode, data);
				}
			}
			return -1;
		}
	}
}

