using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Net;

public class MainPanel : BaseUIForm
{
	Button _getResultInfoButton;
	Button _getRoomListButton;
	
	void Start ()
	{
		_getResultInfoButton = transform.Find ("GetResultInfoButton").GetComponent<Button> ();
		_getRoomListButton = transform.Find ("GetRoomListButton").GetComponent<Button> ();
		
		_getResultInfoButton.onClick.AddListener (() => {
			OnGetResultInfoClick ();
		});
		_getRoomListButton.onClick.AddListener (() => {
			OnGetRoomListClick ();
		});
	}

	void OnGetResultInfoClick()
	{
		if (Game.Instance.User == null) {
			return;
		}
		NetConn.Instance.Send (ProConst.GETRUSULTINFO_MESSENGE, new MessageData(Game.Instance.User.Id.ToString()) );
	}

	void OnGetRoomListClick()
	{
		NetConn.Instance.Send (ProConst.GETROOMLIST_MESSAGE, "");
	}
}
