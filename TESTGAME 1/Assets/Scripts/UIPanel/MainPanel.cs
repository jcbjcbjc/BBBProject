using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class MainPanel : BasePanel
{
	Button _getResultInfoButton;
	Button _getRoomListButton;

	protected override void Start ()
	{
		_getResultInfoButton = transform.Find ("GetResultInfoButton").GetComponent<Button> ();
		_getRoomListButton = transform.Find ("GetRoomListButton").GetComponent<Button> ();
		base.Start ();
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
		NetConn.Instance.Send (RequestCode.Result, ActionCode.GetResultInfo, Game.Instance.User.Id.ToString ());
	}

	void OnGetRoomListClick()
	{
		NetConn.Instance.Send (RequestCode.Room, ActionCode.GetRoomList, "");
	}
}
