using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class RoomItem : BasePanel
{
	int _roomId = 0;
	public Text username;
	public Text maxcount;
	public Text count;
	public Text gamepattern;
	public Button _joinRoomButton;

	protected override void Start ()
	{
		base.Start ();
	}

	public void OnJoinRoomClick()
	{
        if (_roomId != 0) {
			NetConn.Instance.Send(RequestCode.Room, ActionCode.JoinRoom, _roomId.ToString());
		}
	}

	string _userName = "";
	string _count = "";
	string _maxcount = "";
	int game=-1;
	protected override void Update ()
	{
		base.Update ();
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
