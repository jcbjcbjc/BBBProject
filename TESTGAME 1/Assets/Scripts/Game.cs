using UnityEngine;
using System.Collections.Generic;
using Common;

public class Game : MonoBehaviour
{
	private static Game _instance;
	public static Game Instance { get { if (!_instance) { _instance = GameObject.FindObjectOfType(typeof(Game)) as Game; } return _instance; } }

	public static bool _music = true;

	void Awake()
	{
		if (_instance == null) {
			_instance = this;
		}
		User=new UserData();
		Room=new RoomData();
		InitPanel ();
	}

	void Start ()
	{
		NetConn.Instance.Connect ("203.135.96.4",30810);
		NetConn.Instance.Send (RequestCode.Default, ActionCode.Default, "Hello,Client");
	}
	
	void Update ()
	{

	}

	public UserData User;

	public RoomData Room;
	public void OnResponse(int requestCode, int actionCode, string data)
	{
		if (requestCode == RequestCode.Default)
		{
			if (actionCode == ActionCode.Default)
			{
				Debug.Log(data);
			}
		}
		else if (requestCode == RequestCode.User)
		{
			if (actionCode == ActionCode.Login)
			{
				var loginPanel = ShowPanel("LoginPanel") as LoginPanel;
				loginPanel.OnLoginResponse(data);
			}
			else if (actionCode == ActionCode.Register)
			{
				var registerPanel = ShowPanel("RegisterPanel") as RegisterPanel;
				registerPanel.OnRegisterResponse(data);
			}
		}
		else if (requestCode == RequestCode.Result)
		{
			if (actionCode == ActionCode.GetResultInfo)
			{
				var resultInfoPanel = ShowPanel("ResultInfoPanel") as ResultInfoPanel;
				resultInfoPanel.OnGetResultInfoResponse(data);
			}
		}
		else if (requestCode == RequestCode.Room)
		{
			if (actionCode == ActionCode.CreateRoom)
			{
				var roomPanel = ShowPanel("RoomPanel") as RoomPanel;
				roomPanel.OnCreateRoomResponse(data);
			}
			else if (actionCode == ActionCode.GetRoomList)
			{
				var roomListPanel = ShowPanel("RoomListPanel") as RoomListPanel;
				roomListPanel.OnGetRoomListResponse(data);
			}
			else if (actionCode == ActionCode.JoinRoom)
			{
				var roomPanel = GetPanel("RoomPanel") as RoomPanel;
				roomPanel.OnJoinRoomResponse(data);
			}
			else if (actionCode == ActionCode.LeaveRoom)
			{
				var roomPanel = ShowPanel("RoomPanel") as RoomPanel;
				roomPanel.OnLeaveRoomResponse(data);
			}
			else if (actionCode == ActionCode.StartGame)
			{
				ControlManager.OnStartGameResponse(data);
			}
			else if (actionCode == ActionCode.Ready) {
				var roomPanel = ShowPanel("RoomPanel") as RoomPanel;
				roomPanel.OnReadyResponse(data);
			}
			/*
			else if (actionCode == ActionCode.GetRoomInfo) {
				var roomPanel = ShowPanel ("RoomPanel") as RoomPanel;
				roomPanel.OnGetRoomInfoResponse (data);
			}
			*/
		}
		else if (requestCode == RequestCode.Game) 
		{
			ControlManager.Onquest(actionCode, data);
		}
	}

	void OnDestroy()
	{
		if (NetConn.Instance.Connected == true) {
			NetConn.Instance.Close ();
		}
	}

	#region UIManager
	public Dictionary<string,BasePanel> _panelDict = new Dictionary<string, BasePanel> ();
	void InitPanel()
	{
		var startPanel = ViewManager.OpenPanel ("StartPanel", "Canvas");
		var loginPanel = ViewManager.OpenPanel ("LoginPanel", "Canvas");
		var registerPanel = ViewManager.OpenPanel ("RegisterPanel", "Canvas");
		var mainPanel = ViewManager.OpenPanel ("MainPanel", "Canvas");
		var resultInfoPanel = ViewManager.OpenPanel ("ResultInfoPanel", "Canvas");
		
		var roomListPanel = ViewManager.OpenPanel ("RoomListPanel", "Canvas");
		var roomPanel = ViewManager.OpenPanel ("RoomPanel", "Canvas");

		var menuPanel = ViewManager.OpenPanel("MenuPanel", "Canvas");
		var tutorPanel= ViewManager.OpenPanel("TutorPanel", "Canvas");
		var Multiple_UI = ViewManager.OpenPanel("Multiple_ingame", "Canvas");
		var Classic_UI = ViewManager.OpenPanel("Classic_ingame", "Canvas");
		///////////////////////////////////////////////////////////////////
		var GameOver = ViewManager.OpenPanel("GameOverPanel", "Canvas");
		var messagePanel = ViewManager.OpenPanel("MessagePanel", "Canvas");


		_panelDict.Add (startPanel.name, startPanel);
		_panelDict.Add (loginPanel.name, loginPanel);
		_panelDict.Add (registerPanel.name, registerPanel);
		_panelDict.Add (mainPanel.name, mainPanel);
		_panelDict.Add (resultInfoPanel.name, resultInfoPanel);
		
		_panelDict.Add (roomListPanel.name, roomListPanel);
		_panelDict.Add (roomPanel.name, roomPanel);
		_panelDict.Add(menuPanel.name, menuPanel);
		_panelDict.Add(tutorPanel.name, tutorPanel);
		_panelDict.Add(Multiple_UI.name, Multiple_UI);
		_panelDict.Add (Classic_UI.name, Classic_UI);
		/////////////////////////
		_panelDict.Add(GameOver.name, GameOver);
		_panelDict.Add(messagePanel.name, messagePanel);

		ShowPanel(startPanel.name);
		HidePanel (loginPanel.name);
		HidePanel (registerPanel.name);
		HidePanel (mainPanel.name);
		HidePanel (resultInfoPanel.name);
		HidePanel (roomListPanel.name);
		HidePanel (roomPanel.name);
		HidePanel(menuPanel.name);
		HidePanel(Multiple_UI.name);
		HidePanel(Classic_UI.name);
		HidePanel(tutorPanel.name);
		//////////////////////////////////////////
		HidePanel (GameOver.name);
		HidePanel(messagePanel.name);
	}
	public BasePanel GetPanel(string panelName) {
		BasePanel panel;
		_panelDict.TryGetValue(panelName, out panel);
		return panel;
	}
	public BasePanel ShowPanel(string panelName)
	{
		BasePanel panel;
		bool isGet = _panelDict.TryGetValue (panelName, out panel);
		if (isGet == true) {
			panel.OnEnter ();
		}
		return panel;
	}
	public void HidePanel(string panelName)
	{
		BasePanel panel;
		bool isGet = _panelDict.TryGetValue (panelName, out panel);
		if (isGet == true) {
			panel.OnExit ();
		}
	}
	public void HideAllPanel()
	{
		foreach (var item in _panelDict.Values) {
			item.OnExit ();
		}
	}
	public void ShowMessage(string message, float showMessageTime = 2)
	{
		var messagePanel = ShowPanel ("MessagePanel") as MessagePanel;
		messagePanel.ShowMessage (message, showMessageTime);
	}
	#endregion

	public void _close_game()
	{
		Application.Quit();
	}
}

public class UserData
{
	public int Id=0;
	public string Name=null;
	public int TotalCount=0;
	public int WinCount=0;
}
public class RoomData {
	public int GamePattern = -1;
	public int RoomId=0;
	public int RoomPositionID = 0;
	public int MaxCount = 0;
	public List<MyPoint> MapData;
	public List<PlayerData> PlayerData;
}
