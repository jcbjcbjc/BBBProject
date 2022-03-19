using System;
using Common;

namespace GameServer
{
	public class DefaultController : BaseController
	{
		public override string OnRequest (int requestCode, int actionCode, string data, NetClient client, NetServer server)
		{
			if (actionCode == ActionCode.Default) {
				Console.WriteLine ("[" + DateTime.Now + "]" + "[" + client.Address + "]" + data);
			}
			return "";
		}
	}
}

