using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;


namespace GameServer
{
	//public class ResultController
	//{
	//	UserDo _userDo = new UserDo ();
	//	ResultDo _resultDo = new ResultDo ();

	//	public string OnRequest (int requestCode, int actionCode, string data, NetClient client, NetServer server)
	//	{
	//		if (actionCode == ActionCode.GetResultInfo) {
	//			return GetResultInfo (data, client, server);
	//		}
	//		return "";
	//	}

	//	string GetResultInfo(string data, NetClient client, NetServer server)
	//	{
	//		if (data == "") {
	//			return "";
	//		}
	//		int userId = int.Parse (data);
	//		if (client.UserId == int.Parse(data)) {
	//			#region 使用官方登录的用户Id获取战绩信息
	//			client.Send (RequestCode.Result, ActionCode.GetResultInfo, (client.GetResultInfo (client.UserId)));
	//			#endregion
	//		} 
	//		else {
	//			#region 使用第三方登录的用户Id获取战绩信息
	//			client.Send (RequestCode.Result, ActionCode.GetResultInfo, (client.GetResultInfo (userId)));
	//			#endregion
	//		}
	//		return "";
	//	}
	//}
}

