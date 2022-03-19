using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace GameServer
{
	public class SystemController
	{
		UserDo _userDo = new UserDo();
		ResultDo _resultDo = new ResultDo();

		public SystemController() {
			MessageCenter.AddMsgListener(ProConst.DEFAULT, OnDefaultRequest);
			MessageCenter.AddMsgListener(ProConst.REGISTER_MESSAGE, Register);
			MessageCenter.AddMsgListener(ProConst.LOGIN_MESSAGE, Login);
			MessageCenter.AddMsgListener(ProConst.STARTGAME_MESSAGE, StartGame);
			MessageCenter.AddMsgListener(ProConst.GETROOMLIST_MESSAGE, GetRoomList);
			MessageCenter.AddMsgListener(ProConst.CREATEROOM_MESSAGE, CreateRoom);
			MessageCenter.AddMsgListener(ProConst.JOINROOM_MESSAGE, JoinRoom);
			MessageCenter.AddMsgListener(ProConst.LEAVEROOM_MESSAGE, LeaveRoom);
			MessageCenter.AddMsgListener(ProConst.READYFORGAME, Ready);
			MessageCenter.AddMsgListener(ProConst.CONFIRM_START_GAME,ConfirmStart);
			MessageCenter.AddMsgListener(ProConst.OVERGAME_MESSAGE, OverGame);
		}
		public void OnDefaultRequest (KeyValuesUpdate kv)
		{
			Console.WriteLine ("[" + DateTime.Now + "]" + "[" + kv.NetClient.Address + "]" + kv.Values);
		}
        #region user
        void Register(KeyValuesUpdate kv)
		{
			LoginData loginData = JsonConvert.DeserializeObject<LoginData>(kv.Values.ToString());
			if (loginData == null)
			{
				return ;
			}
			int result = _userDo.Register(loginData._nameInput, loginData._pwdInput);
			if (result == 1)
			{
				Console.WriteLine("User Do Register New User");
				kv.NetClient.Send(ProConst.REGISTER_MESSAGE, new MessageData("1"));
			}
			else if (result == -2)
			{
				Console.WriteLine("User Do Register的返回值为-2");
				kv.NetClient.Send(ProConst.REGISTER_MESSAGE, new MessageData("-2"));
			}
			else if (result == -1)
			{
				Console.WriteLine("User Do Register的返回值为-1");
			}
			return ;
		}
		void Login(KeyValuesUpdate kv)
		{
			LoginData loginData = JsonConvert.DeserializeObject < LoginData > (kv.Values.ToString()) ;
			if (loginData == null)
			{
				return ;
			}
			
			if (kv.Server.IsOnLine(loginData._nameInput) == true)
			{
				Console.WriteLine("另一个客户端登录了User Id[" + kv.NetClient.UserId + "]");
				kv.NetClient.Send(ProConst.LOGIN_MESSAGE, new MessageData("-3"));
				kv.NetClient.UserId = 0;
				return ;
			}
			int userId = _userDo.Login(loginData._nameInput);
			if (userId > 0)
			{
				var user = _userDo.GetList().Where(u => u.Id == userId).FirstOrDefault();
				if (user.Pwd == loginData._pwdInput)
				{
					user.EndLoginTime = DateTime.Now.ToString();
					_userDo.Update(user.Id, "EndLoginTime", user.EndLoginTime);
					kv.NetClient.UserId = user.Id;
					Console.WriteLine("UserDo Login New User");
					kv.NetClient.Send(ProConst.LOGIN_MESSAGE, new MessageData(kv.NetClient.GetResultInfo(kv.NetClient.UserId)));
				}
				else
				{
					Console.WriteLine("UserDo Login的返回值为-1");
					kv.NetClient.Send(ProConst.LOGIN_MESSAGE, new MessageData("-1"));
				}
			}
			else if (userId == 0)
			{
				Console.WriteLine("UserDo Login的返回值为0");
				kv.NetClient.Send(ProConst.LOGIN_MESSAGE, new MessageData("0"));
			}
			else
			{
				Console.WriteLine("UserDo Login的返回值为-2");
			}
			return ;
		}
		#endregion
		void StartGame(KeyValuesUpdate kv)
		{
			NetClient client = kv.NetClient;


			if (client.RoomId == 0) { return ; }
			var room = client.Room;
			if (room.GetId() != client.UserId)
			{
				Console.WriteLine("您不是房主");

				client.Send(ProConst.STARTGAME_MESSAGE, new Game_BaseDataFromServer(-2));
				
			}
			if (room.isStartGame())
			{
				Console.WriteLine("房间{0}开始对战", client.RoomId);

				List<Game_BaseDataFromServer> game_BaseDatas = ;

				for (int i = 0; i < room.GetClientCount(); i++)
				{
					room.GetClientList()[i].Send(ProConst.STARTGAME_MESSAGE, ,,,,,,,,(i + 1).ToString() + ";" + room.GetGamePattern().ToString());
				}
				room.SetRoomStatus(RoomCode.Battle);
			}
			else
			{
				Console.WriteLine("玩家还没准备");
				client.Send(ProConst.STARTGAME_MESSAGE, new Game_BaseDataFromServer(-1));
			}
			return ;
		}
		void OverGame(KeyValuesUpdate kv) {
			NetClient client = kv.NetClient;
			NetServer server = kv.Server;

			client.Room.CloseGame();
			Broadcast(client,server,ProConst.OVERGAME_MESSAGE,kv.Values);

		}
		void CreateRoom(KeyValuesUpdate kv)
		{
			ClientRoomData messageData= JsonConvert.DeserializeObject < ClientRoomData > (kv.Values.ToString() );
			NetClient client = kv.NetClient;
			NetServer server = kv.Server;
			Console.WriteLine("开始创建房间 能否创建房间：" + (client.RoomId == 0));
			if (client.RoomId != 0)
			{
				client.Room.Leave(client);
				//_roomDo.LeaveRoom (client.RoomId, client.UserId);
				client.RoomId = 0;
				Console.WriteLine("移除房间");
			}

			//_roomDo.CreateRoom (client.UserId, client.UserId, 2);



			client.RoomId = client.UserId;
			if (client.Room != null)
			{
				return ;
			}
			Console.WriteLine("正在创建房间中");
			Console.WriteLine("创建的房间类型为{0}",messageData.GamePattern);
			server.CreateRoom(client, messageData);
			Console.WriteLine("创建房间成功");

			client.Send(ProConst.CREATEROOM_MESSAGE, client.GetRoomData());

			return ;
		}
		void GetRoomList(KeyValuesUpdate kv)
		{
			NetClient client = kv.NetClient;
			NetServer server = kv.Server;
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
			client.Send(ProConst.GETROOMLIST_MESSAGE, new MessageData(sb.ToString()));
			return;

		}
		void JoinRoom(KeyValuesUpdate kv)
		{
			MessageData messageData= JsonConvert.DeserializeObject < MessageData > (kv.Values.ToString() );
			NetClient client = kv.NetClient;
			NetServer server = kv.Server;
			if (client.RoomId != 0)
			{
				client.Room.Leave(client);
				client.RoomId = 0;
			}

			int roomId = int.Parse(messageData._data);

			client.RoomId = roomId;
			bool iscan= server.JoinRoom(roomId, client);
			if (!iscan) {
				client.Send(ProConst.JOINROOM_MESSAGE, new JoinRoomResponseData(-1));
				return;
			}
			Broadcast(client, server, ProConst.GETROOMINFO_MESSAGE, new ClientRoomInfo( client.GetRoomInfo()));
			client.Send(ProConst.JOINROOM_MESSAGE, new JoinRoomResponseData(client.GetRoomData(), new ClientRoomInfo( client.GetRoomInfo()),0));
			return ;
		}

		void LeaveRoom(KeyValuesUpdate kv)
		{
			NetClient client = kv.NetClient;
			NetServer server = kv.Server;
			if (client.RoomId <= 0)
			{
				return ;
			}
			if (client.RoomId == client.UserId)
			{
				Broadcast(client, server, ProConst.LEAVEROOM_MESSAGE,new MessageData("0"));
				server.LeaveRoom(client.RoomId, client);
				client.RoomId = 0;
			}
			else
			{
				Console.WriteLine("离开房间后的将更新房间信息为" + client.GetRoomInfo());
				Broadcast(client, server, ProConst.LEAVEROOM_MESSAGE,new ClientRoomInfo( client.GetRoomInfo()));
				server.LeaveRoom(client.RoomId, client);
				client.RoomId = 0;
			}
			client.RoomId = 0;
			client.Send(ProConst.LEAVEROOM_MESSAGE, new MessageData("2"));
			return ;
		}
		void Ready(KeyValuesUpdate kv)
		{
			NetClient client = kv.NetClient;
			var room = client.Room;
			int ok = client.Room.Ready(client);
			if (ok == 1)
			{
				client.Send(ProConst.READYFORGAME, new MessageData("1"));
				return ;
			}
			return ;
		}
		void ConfirmStart(KeyValuesUpdate kv)
		{
			NetClient client = kv.NetClient;
			Console.WriteLine("玩家{0}已经准备就绪", client.UserId);
			client.Room.ConfirmStart(client);
			if (client.Room.IsEnterGame())
			{
				for (int i = 0; i < client.Room.GetClientCount(); i++)
				{
					client.Room.GetClientList()[i].Send(ProConst.GAME_PROCESS_START_MESSAGE, "");
				}
			}
			return ;
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
		int Broadcast(NetClient client,NetServer server,string messageType, object data)
		{
			var room = client.Room;
			if (client.Room == null)
			{
				Console.WriteLine("房间为空");
				return -1;
			}
			for (int i = 0; i < room.GetClientCount(); i++)
			{
				if (room.GetClientList()[i].UserId != client.UserId)
				{
					return room.GetClientList()[i].Send(messageType, data);
				}
			}
			return -1;
		}
	}
}

