using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Net;
public class RoomItem : BaseUIForm
{
	int _roomId = 0;
	public Text username;
	public Text maxcount;
	public Text count;
	public Text gamepattern;
	public Button _joinRoomButton;

	void Start ()
	{
		
	}

	public void OnJoinRoomClick()
	{
        if (_roomId != 0) {
			NetConn.Instance.Send(ProConst.JOINROOM_MESSAGE, new MessageData(_roomId.ToString()));
		}
	}

	string _userName = "";
	string _count = "";
	string _maxcount = "";
	int game=-1;
	void Update ()
	{
		
		if (_userName != "" && _count != "" && _maxcount != "") {
			username.text = _userName;
			maxcount.text = "/" + _maxcount;
			//_totalCountText.text.Replace ("\n", "\\n");
			count.text = _count;
			gamepattern.text = GetPattern(game);
		}
	}

	public void SetText(string roomId, string userName, string count,string maxcount, string gamepattern)
	{
		_roomId = int.Parse (roomId);
		_userName = userName;
		_count = count;
		_maxcount=maxcount;
		game =int.Parse (gamepattern);
	}
	string GetPattern(int game) {
		string pattern = "";
		if (game == PatternCode.Multiple)
		{
			pattern = "Multiple";
		}
		else if (game == PatternCode.quartic)
		{
			pattern = "Quartic";
		}
		else if (game == PatternCode.hexgon) {
			pattern = "Hexgon";
		}
		return pattern;
	}
}
