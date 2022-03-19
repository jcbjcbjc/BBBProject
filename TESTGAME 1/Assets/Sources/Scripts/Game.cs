using UnityEngine;
using System.Collections.Generic;
using Common;
using Net;
using MessageHandler;
public class Game : MonoBehaviour
{
	private static Game _instance;
	public static Game Instance { get { if (!_instance) { _instance = GameObject.FindObjectOfType(typeof(Game)) as Game; } return _instance; } }


	void Awake()
	{
		if (_instance == null) {
			_instance = this;
		}
		User=new ClientUserData();
		Room=new ClientRoomData();
		GameConfig=new Game_BaseData();
		UIManager.GetInstance().ShowUIForms(ProConst.START_UIFORM);

	
		UIManager.GetInstance().ShowUIForms(ProConst.MESSAGE_FROMS);
	}

	void Start ()
	{
		NetConn.Instance.Connect ("218.89.171.149", 30809);
		NetConn.Instance.Send (ProConst.DEFAULT, new MessageData("Hello,Client"));
	}
	
	void Update ()
	{
		
	}

	public ClientUserData User;

	public ClientRoomData Room;

	public Game_BaseData GameConfig;
	void OnDestroy()
	{
		if (NetConn.Instance.Connected == true) {
			NetConn.Instance.Close ();
		}
	}

	public void _close_game()
	{
		Application.Quit();
	}

	public void ShowMessage(string message,int time=2) {
		string[] strArray = new string[] { message, time.ToString() };
		KeyValuesUpdate kvs = new KeyValuesUpdate( strArray);
		MessageCenter.SendMessage(ProConst.SHOW_MESSAGE_MESSAGE, kvs);
		
	}
}
