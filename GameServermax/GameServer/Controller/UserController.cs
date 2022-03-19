using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using Common;

namespace GameServer
{
	public class UserController : BaseController
	{
		UserDo _userDo = new UserDo ();
		ResultDo _resultDo = new ResultDo ();

		public override string OnRequest (int requestCode, int actionCode, string data, NetClient client, NetServer server)
		{
			if (actionCode == ActionCode.Login) {
				return Login (data, client, server);
			}
			else if (actionCode == ActionCode.Register) {
				return Register (data, client, server);
			}
			return "";
		}

		string Register(string data, NetClient client, NetServer server)
		{
			if (data == "") {
				return "";
			}
			var array = data.Split (';');
			int result = _userDo.Register (array [0], array [1]);
			if (result == 1) {
				Console.WriteLine ("User Do Register New User");
				client.Send (RequestCode.User, ActionCode.Register, "1");
			}
			else if (result == -2) {
				Console.WriteLine ("User Do Register的返回值为-2");
				client.Send (RequestCode.User, ActionCode.Register, "-2");
			}
			else if (result == -1) {
				Console.WriteLine ("User Do Register的返回值为-1");
			}
			return "";
		}

		string Login(string data, NetClient client, NetServer server)
		{
			if (data == "") {
				return "";
			}
			var array = data.Split (';');
			if (server.IsOnLine(array[0]) == true) {
				Console.WriteLine ("另一个客户端登录了User Id[" + client.UserId + "]");
				client.Send (RequestCode.User, ActionCode.Login, "-3");
				client.UserId = 0;
				return "";
			} 
			int userId = _userDo.Login (array [0]);
			if (userId > 0) {
				var user = _userDo.GetList ().Where (u => u.Id == userId).FirstOrDefault ();
				if (user.Pwd == array [1]) {
					user.EndLoginTime = DateTime.Now.ToString ();
					_userDo.Update (user.Id, "EndLoginTime", user.EndLoginTime);
					client.UserId = user.Id;
					Console.WriteLine ("UserDo Login New User");
					client.Send (RequestCode.User, ActionCode.Login, client.GetResultInfo (client.UserId));
				} else {
					Console.WriteLine ("UserDo Login的返回值为-1");
					client.Send (RequestCode.User, ActionCode.Login, "-1");
				}
			} else if (userId == 0) {
				Console.WriteLine ("UserDo Login的返回值为0");
				client.Send (RequestCode.User, ActionCode.Login, "0");
			} else {
				Console.WriteLine ("UserDo Login的返回值为-2");
			}
			return "";
		}
	}
}

