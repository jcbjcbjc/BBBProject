﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class RoomListPanel : BasePanel
{
	RoomItem _roomItemPrefab;
	Button _createRoomButton;
	Button _updateButton;
	Button _closeButton;

	protected override void Start ()
	{
		_roomItemPrefab = Resources.Load<RoomItem> ("Item/RoomItem");
		_createRoomButton = transform.Find ("CreateRoomButton").GetComponent<Button> ();
		_updateButton = transform.Find ("UpdateButton").GetComponent<Button> ();
		_closeButton = transform.Find ("CloseButton").GetComponent<Button> ();
		base.Start ();

		_createRoomButton.onClick.AddListener (() => {
			OnCreateRoomClick ();
		});
		_updateButton.onClick.AddListener (() => {
			OnUpdateClick ();
		});
		_closeButton.onClick.AddListener (() => {
			OnCloseClick ();
		});
	}

	void OnCreateRoomClick()
	{
		NetConn.Instance.Send (RequestCode.Room, ActionCode.CreateRoom, PatternCode.Multiple.ToString());
	}

	void OnUpdateClick()
	{
		NetConn.Instance.Send (RequestCode.Room, ActionCode.GetRoomList, "");
	}

	void OnCloseClick()
	{
		Game.Instance.HidePanel ("RoomListPanel");
		Game.Instance.ShowPanel("MenuPanel");
	}

	protected override void Update ()
	{
		base.Update ();
		if (_data != "") {
			if (_data == "0") {
				Debug.Log ("房间列表为空");
				var content = transform.Find ("Content");
				for (int i = 0; i < content.childCount; i++) {
					Destroy (content.GetChild (i).gameObject);
				}
				_haveItem = false;
			}
			else if (_data.Contains("#") == true) {
				Debug.Log (_data);
				var roomList = _data.Split ('#');
				if (roomList.Length <= 10) {
					AddItem (roomList.Length);
				}
				else if (roomList.Length > 10) {
					AddItem (10);
				}
				SetItems (roomList);
			}
			else if (_data.Contains("#") == false) {
				if (_data.Contains(";")) {
					var array = _data.Split (';');
					Debug.Log ("12345");
					AddItem (1);
					SetItem (array [0], array [1], array [2], array [3],array[4]);
				}
			}
			_data = "";
		}
	}

	bool _haveItem = false;
	public void AddItem(int count)
	{
		var content = transform.Find ("Content");
		if (_haveItem == false) {
			for (int i = 0; i < count; i++) {
				var roomItem = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Item/" + "RoomItem"), content);
				roomItem.name = "RoomItem";
				roomItem.transform.SetParent (content);
			}
			_haveItem = true;
		}
	}

	public void SetItems(string [] roomList)
	{
		var content = transform.Find ("Content");
		for (int i = 0; i < content.childCount; i++) {
			var array = roomList [i].Split (';');
			var roomItem = content.GetChild (i).GetComponent<RoomItem> ();
			roomItem.SetText (array [0], array [1], array [2], array [3],array[4]);
		}
	}

	public void SetItem(string roomId, string userName, string count,string maxcount, string winCount)
	{
		var content = transform.Find ("Content");
		var roomItem = content.GetChild (0).GetComponent<RoomItem> ();
		roomItem.SetText (roomId, userName, count, maxcount, winCount);
	}

	string _data = "";
	public void OnGetRoomListResponse(string data)
	{
		_data = data;
	}
}
