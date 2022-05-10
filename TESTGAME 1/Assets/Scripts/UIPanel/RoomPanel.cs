using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class RoomPanel : BasePanel 
{
	Text _oneUserNameText;
	Text _oneTotalCountText;
	Text _oneWinCountText;

	Text _twoUserNameText;
	Text _twoTotalCountText;
	Text _twoWinCountText;

	public Button _startGameButton;
	Button _leaveRoomButton;
	public Button _readyButton;
	public Text Pattern;

	protected override void Start ()
	{
		_oneUserNameText = transform.Find ("OnePanel/UserNameText").GetComponent<Text> ();
		_oneTotalCountText = transform.Find ("OnePanel/TotalCountText").GetComponent<Text> ();
		_oneWinCountText = transform.Find ("OnePanel/WinCountText").GetComponent<Text> ();

		_twoUserNameText = transform.Find ("TwoPanel/UserNameText").GetComponent<Text> ();
		_twoTotalCountText = transform.Find ("TwoPanel/TotalCountText").GetComponent<Text> ();
		_twoWinCountText = transform.Find ("TwoPanel/WinCountText").GetComponent<Text> ();

		_startGameButton = transform.Find ("StartGameButton").GetComponent<Button> ();
		_leaveRoomButton = transform.Find ("LeaveRoomButton").GetComponent<Button> ();
		_readyButton=transform.Find ("ReadyGame").GetComponent <Button> ();
		base.Start ();
		_startGameButton.onClick.AddListener (() => {
			OnStartGameClick ();
		});
		_leaveRoomButton.onClick.AddListener (() => {
			OnLeaveRoomClick ();
		});
		_readyButton.onClick.AddListener(() => {
			OnReadyClick();
		});
	}

	void OnStartGameClick()
	{
		NetConn.Instance.Send(RequestCode.Room, ActionCode.StartGame, "");
	}

	void OnLeaveRoomClick()
	{
		NetConn.Instance.Send (RequestCode.Room, ActionCode.LeaveRoom, "");
	}
	void OnReadyClick() {
		NetConn.Instance.Send(RequestCode.Room, ActionCode.Ready, "");
	}
	public void OnCreateRoomResponse(string data)
	{
		var strs = data.Split(';');
		int roomId = int.Parse(strs[0]);
		int Pattern = int.Parse(strs[1]);

		Game.Instance.Room.RoomId = roomId;
		Game.Instance.Room.GamePattern = Pattern;


        _userOne = Game.Instance.User;

		Game.Instance.ShowMessage ("创建房间成功");
		//SetOneText (user.Name, user.TotalCount.ToString (), user.WinCount.ToString ());
		//ClearTwoText ();
		_userTwo = null;
		Game.Instance.HidePanel ("RoomListPanel");
	}


	void SetOneText(string userName, string totalCount, string winCount)
	{
		_oneUserNameText.text = userName;
		_oneTotalCountText.text = "总场数\n" + totalCount;
		_oneTotalCountText.text.Replace ("\n", "\\n");
		_oneWinCountText.text = "胜利\n" + winCount;
		_oneWinCountText.text.Replace ("\n", "\\n");
	}

	void SetTwoText(string userName, string totalCount, string winCount)
	{
		_twoUserNameText.text = userName;
		_twoTotalCountText.text = "总场数\n" + totalCount;
		_twoTotalCountText.text.Replace ("\n", "\\n");
		_twoWinCountText.text = "胜利\n" + winCount;
		_twoWinCountText.text.Replace ("\n", "\\n");
	}

	void ClearTwoText()
	{
		_twoUserNameText.text = "等待加入";
		_twoTotalCountText.text = "";
		_twoWinCountText.text = "";
	}

	UserData _userOne = null;
	UserData _userTwo = null;

	protected override void Update ()
	{
		base.Update ();
		if (_userOne != null) {
			SetOneText (_userOne.Name, _userOne.TotalCount.ToString (), _userOne.WinCount.ToString ());
		}

		if (_userTwo != null) {
			SetTwoText (_userTwo.Name, _userTwo.TotalCount.ToString (), _userTwo.WinCount.ToString ());
		}
		else {
			ClearTwoText ();
		}
	}
	public void OnReadyResponse(string data) {
		if (data != "") { 
			Game.Instance.ShowMessage("已准备");
		}
	}
	public void OnJoinRoomResponse(string data)
	{
		if (data != "") {
			Game.Instance.HidePanel("RoomListPanel");
			Game.Instance.ShowPanel("RoomPanel");
			var da = data.Split("*");
			var ID=da[0];
			if (ID != "") { Game.Instance.Room.RoomId = int.Parse(ID); }
			var pattern=da[1];
			if (pattern != "") { Game.Instance.Room.GamePattern = int.Parse(pattern); }
			var dat=da[2];
			if (dat.Contains ("#")) {
				Debug.Log (dat);
				var room = dat.Split ('#');
				var arrayOne = room [0].Split (';');
				_userOne = new UserData ();
				_userOne.Id = int.Parse (arrayOne [0]);
				_userOne.Name = arrayOne [1];
				_userOne.TotalCount = int.Parse (arrayOne [2]);
				_userOne.WinCount = int.Parse (arrayOne [3]);

				//SetOneText (arrayOne [1], arrayOne [2], arrayOne [3]);

				var arrayTwo = room [1].Split (';');
				_userTwo = new UserData ();
				_userTwo.Id = int.Parse (arrayTwo [0]);
				_userTwo.Name = arrayTwo [1];
				_userTwo.TotalCount = int.Parse (arrayTwo [2]);
				_userTwo.WinCount = int.Parse (arrayTwo [3]);

				//SetTwoText (arrayTwo [1], arrayTwo [2], arrayTwo [3]);
			} 
			else {
				Debug.Log (dat);
				if (dat.Contains (";")) {
					var array = dat.Split (';');
					_userOne = new UserData ();
					_userOne.Id = int.Parse (array [0]);
					_userOne.Name = array [1];
					_userOne.TotalCount = int.Parse (array [2]);
					_userOne.WinCount = int.Parse (array [3]);

					//SetOneText (array [1], array [2], array [3]);

					_userTwo = null;

					//ClearTwoText ();
				}
			}
		}
	}

	/*
	public void OnGetRoomInfoResponse(string data)
	{
		if (data != "") {
			if (data.Contains ("#")) {
				Debug.Log (data);
				var room = data.Split ('#');
				var arrayOne = room [0].Split (';');
				_userOne = new UserData ();
				_userOne.Id = int.Parse (arrayOne [0]);
				_userOne.Name = arrayOne [1];
				_userOne.TotalCount = int.Parse (arrayOne [2]);
				_userOne.WinCount = int.Parse (arrayOne [3]);

				//SetOneText (arrayOne [1], arrayOne [2], arrayOne [3]);

				var arrayTwo = room [1].Split (';');
				_userTwo = new UserData ();
				_userTwo.Id = int.Parse (arrayTwo [0]);
				_userTwo.Name = arrayTwo [1];
				_userTwo.TotalCount = int.Parse (arrayTwo [2]);
				_userTwo.WinCount = int.Parse (arrayTwo [3]);

				//SetTwoText (arrayTwo [1], arrayTwo [2], arrayTwo [3]);
			} 
			else {
				Debug.Log (data);
				if (data.Contains (";")) {
					var array = data.Split (';');
					_userOne = new UserData ();
					_userOne.Id = int.Parse (array [0]);
					_userOne.Name = array [1];
					_userOne.TotalCount = int.Parse (array [2]);
					_userOne.WinCount = int.Parse (array [3]);

					//SetOneText (array [1], array [2], array [3]);

					_userTwo = null;

					//ClearTwoText ();
				}
			}
		}
	}
	*/

	public void OnLeaveRoomResponse(string data)
	{
		if (data != "") {
			if (data == "0") {
				Game.Instance.Room.GamePattern = -1;
				Game.Instance.Room.RoomId = 0;
				Game.Instance.ShowMessage ("房主离开了房间");
				Game.Instance.HidePanel ("RoomPanel");
				Game.Instance.ShowPanel ("RoomListPanel");
				_userOne = null;
				_userTwo = null;
				return;
			}
			else if (data.Contains ("#")) {
				var room = data.Split ('#');
				var arrayOne = room [0].Split (';');
				_userOne = new UserData ();
				_userOne.Id = int.Parse (arrayOne [0]);
				_userOne.Name = arrayOne [1];
				_userOne.TotalCount = int.Parse (arrayOne [2]);
				_userOne.WinCount = int.Parse (arrayOne [3]);

				//SetOneText (arrayOne [1], arrayOne [2], arrayOne [3]);

				var arrayTwo = room [1].Split (';');
				_userTwo = new UserData ();
				_userTwo.Id = int.Parse (arrayTwo [0]);
				_userTwo.Name = arrayTwo [1];
				_userTwo.TotalCount = int.Parse (arrayTwo [2]);
				_userTwo.WinCount = int.Parse (arrayTwo [3]);

				//SetTwoText (arrayTwo [1], arrayTwo [2], arrayTwo [3]);
			} 
			else {
				if (data.Contains (";")) {
					var array = data.Split (';');
					_userOne = new UserData ();
					_userOne.Id = int.Parse (array [0]);
					_userOne.Name = array [1];
					_userOne.TotalCount = int.Parse (array [2]);
					_userOne.WinCount = int.Parse (array [3]);

					//SetOneText (array [1], array [2], array [3]);

					_userTwo = null;

					//ClearTwoText ();
				}
			}
			if (data == "2") {
				Game.Instance.Room.GamePattern = -1;
				Game.Instance.Room.RoomId = 0;
				Game.Instance.ShowMessage ("离开房间成功");
				Game.Instance.HidePanel ("RoomPanel");
				Game.Instance.ShowPanel ("RoomListPanel");
				_userOne = null;
				_userTwo = null;
				return;
			}
		}
	}
	//void UISwitch() {
	//	if (Game.Instance.Room.RoomId == Game.Instance.User.Id)
	//	{
	//		var canvasGroup = _startGameButton.GetComponent<CanvasGroup>();
	//		canvasGroup.blocksRaycasts = true;
	//		canvasGroup.interactable = true;
	//		canvasGroup.ignoreParentGroups = true;
	//		canvasGroup.alpha = 1;
	//		var acanvasGroup = _readyButton.GetComponent<CanvasGroup>();
	//		acanvasGroup.blocksRaycasts = false;
	//		acanvasGroup.interactable = false;
	//		acanvasGroup.ignoreParentGroups = false;
	//		acanvasGroup.alpha = 0;
	//	}
	//	else
	//	{
	//		var canvasGroup = _readyButton.GetComponent<CanvasGroup>();
	//		canvasGroup.blocksRaycasts = true;
	//		canvasGroup.interactable = true;
	//		canvasGroup.ignoreParentGroups = true;
	//		canvasGroup.alpha = 1;
	//		var acanvasGroup = _startGameButton.GetComponent<CanvasGroup>();
	//		acanvasGroup.blocksRaycasts = false;
	//		acanvasGroup.interactable = false;
	//		acanvasGroup.ignoreParentGroups = false;
	//		acanvasGroup.alpha = 0;
	//	}
	//}
}
