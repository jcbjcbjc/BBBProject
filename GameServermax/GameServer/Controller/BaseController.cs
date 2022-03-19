using System;

namespace GameServer
{
	public class BaseController
	{
		public virtual string OnRequest(int requestCode, int actionCode, string data, NetClient client, NetServer server)
		{
			return "";
		}
	}
}

