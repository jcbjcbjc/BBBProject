using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using Common;

namespace GameServer
{
	public class GameController : BaseController
	{
		public override string OnRequest(int requestCode, int actionCode, string data, NetClient client, NetServer server)
		{
			Console.WriteLine(requestCode.ToString()+ actionCode.ToString() + data+ client.UserId.ToString());
			GameBroadcast(client, server, actionCode, data);
			
			return "";
		}
		public int GameBroadcast(NetClient client, NetServer server, int actionCode, string data)
		{
			var room = client.Room;
			for (int i = 0; i < room.GetClientCount(); i++)
			{
				if (room.GetClientList()[i].UserId != client.UserId)
				{
					return room.GetClientList()[i].Send(RequestCode.Game, actionCode, data);
				}
			}
			return -1;
		}
	}
}

