using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Net;
public class RoomPanel : BaseUIForm 
{
	List<ClientUserData> clientUserDatasInRoom=new List<ClientUserData>();

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
    private void Awake()
    {
		ReceiveMessage(ProConst.CREATEROOM_MESSAGE, OnCreateRoomResponse);
		ReceiveMessage(ProConst.JOINROOM_MESSAGE, OnJoinRoomResponse);
		ReceiveMessage(ProConst.LEAVEROOM_MESSAGE, OnLeaveRoomResponse);
		ReceiveMessage(ProConst.GAME_CONTROL_MESSAGE, OnReadyResponse);
		ReceiveMessage(ProConst.GETROOMINFO_MESSAGE, OnGetRoomInfoResponse);
	}

    void Start ()
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
		NetConn.Instance.Send(ProConst.STARTGAME_MESSAGE, "");
	}

	void OnLeaveRoomClick()
	{
		NetConn.Instance.Send (ProConst.LEAVEROOM_MESSAGE, "" );
	}
	void OnReadyClick() {
		NetConn.Instance.Send(ProConst.READYFORGAME, "");
	}
	public void OnCreateRoomResponse(KeyValuesUpdate kv)
	{
		string[] str = kv.Values as string[];
		string error = str[0];

		int roomId = int.Parse(str[1]);
		int Pattern = int.Parse(str[2]);

		Game.Instance.Room.RoomId = roomId;
		Game.Instance.Room.GamePattern = Pattern;


        _userOne = Game.Instance.User;

		Game.Instance.ShowMessage ("创建房间成功");
		//SetOneText (user.Name, user.TotalCount.ToString (), user.WinCount.ToString ());
		//ClearTwoText ();
		_userTwo = null;
		
		UIManager.GetInstance().CloseUIForms(ProConst.ROOMLIST_UIFORM);
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

	ClientUserData _userOne = null;
	ClientUserData _userTwo = null;

	void Update ()
	{
		
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
	public void OnReadyResponse(KeyValuesUpdate kv) {
		MessageData messageData=kv.Values as MessageData;
		
		Game.Instance.ShowMessage("已准备");
	}
	public void OnJoinRoomResponse(KeyValuesUpdate kv)
	{
		JoinRoomResponseData joinRoomResponseData=kv.Values as JoinRoomResponseData;
		if (joinRoomResponseData._error == -1) {

			Game.Instance.ShowMessage("加入房间失败");

			////////////////////////
		}
		if (joinRoomResponseData._error == 0) {
			UIManager.GetInstance().ShowUIForms(ProConst.ROOM_UIFORM);
			UIManager.GetInstance().CloseUIForms(ProConst.ROOMLIST_UIFORM);

			
			Game.Instance.Room = joinRoomResponseData._clientRoomData;

			clientUserDatasInRoom = joinRoomResponseData._clientRoomInfo._clientUserDatas;
		}
	}
	public void OnGetRoomInfoResponse(KeyValuesUpdate kv) {
		ClientRoomInfo clientRoomInfo = kv.Values as ClientRoomInfo;

		clientUserDatasInRoom=clientRoomInfo._clientUserDatas;
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

	public void OnLeaveRoomResponse(KeyValuesUpdate kv)
	{
		MessageData messageData = kv.Values as MessageData;
		string error = messageData._data;
		
		if (error == "0") {
			Game.Instance.Room.GamePattern = -1;
			Game.Instance.Room.RoomId = 0;
			Game.Instance.ShowMessage ("房主离开了房间");
			UIManager.GetInstance().ShowUIForms(ProConst.ROOMLIST_UIFORM);
			UIManager.GetInstance().CloseUIForms(ProConst.ROOM_UIFORM);
			
			_userOne = null;
			_userTwo = null;
			return;
		}
		if (error == "2")
		{
			Game.Instance.Room.GamePattern = -1;
			Game.Instance.Room.RoomId = 0;
			Game.Instance.ShowMessage("离开房间成功");

			UIManager.GetInstance().ShowUIForms(ProConst.ROOMLIST_UIFORM);
			UIManager.GetInstance().CloseUIForms(ProConst.ROOM_UIFORM);

			_userOne = null;
			_userTwo = null;
			return;
		}
		//else if (data.Contains ("#")) {
		//	var room = data.Split ('#');
		//	var arrayOne = room [0].Split (';');
		//	_userOne = new UserData ();
		//	_userOne.Id = int.Parse (arrayOne [0]);
		//	_userOne.Name = arrayOne [1];
		//	_userOne.TotalCount = int.Parse (arrayOne [2]);
		//	_userOne.WinCount = int.Parse (arrayOne [3]);

		//	//SetOneText (arrayOne [1], arrayOne [2], arrayOne [3]);

		//	var arrayTwo = room [1].Split (';');
		//	_userTwo = new UserData ();
		//	_userTwo.Id = int.Parse (arrayTwo [0]);
		//	_userTwo.Name = arrayTwo [1];
		//	_userTwo.TotalCount = int.Parse (arrayTwo [2]);
		//	_userTwo.WinCount = int.Parse (arrayTwo [3]);

		//	//SetTwoText (arrayTwo [1], arrayTwo [2], arrayTwo [3]);
		//} 
		
	}
	
}
