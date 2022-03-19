using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

namespace GameServer
{
	public class GameController
	{
		public GameController()
		{
			MessageCenter.AddMsgListener(ProConst.ULT, OnRequest);
			MessageCenter.AddMsgListener(ProConst.REGITER_MESSAGE, OnRequest);
			MessageCenter.AddMsgListener(ProConst.LOG_MESSAGE, OnRequest);
			MessageCenter.AddMsgListener(ProConst.STARGAME_MESSAGE, OnRequest);
			MessageCenter.AddMsgListener(ProConst.GETOMLIST_MESSAGE, OnRequest);
			MessageCenter.AddMsgListener(ProConst.CREAEROOM_MESSAGE, OnRequest);
			MessageCenter.AddMsgListener(ProConst.JOINOOM_MESSAGE, OnRequest);
			MessageCenter.AddMsgListener(ProConst.LEAVROOM_MESSAGE, OnRequest);
			MessageCenter.AddMsgListener(ProConst.READYORGAME, OnRequest);
			MessageCenter.AddMsgListener(ProConst.CONFRM_START_GAME, OnRequest);
			MessageCenter.AddMsgListener(ProConst.OVERAME_MESSAGE, OnRequest);
		}
		public void OnRequest(KeyValuesUpdate kv)
		{
			object data = kv.Values;
			string message = kv._messageType;
			NetClient client = kv.NetClient;
			NetServer server = kv.Server;
			
			Broadcast(client, server, message, data);
			
			return ;
		}
		int Broadcast(NetClient client, NetServer server, string messageType, object data)
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

