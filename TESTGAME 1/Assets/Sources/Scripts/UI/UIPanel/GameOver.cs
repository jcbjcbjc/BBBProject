using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using MessageHandler;
public class GameOver : BaseUIForm
{
	static int PatternGame=-1;
	public Text winnerid;
	public Text winnername;
	public Button ReturnRoom;
	public Button _quitbutton;
	public Button _qxitLianjibutton;
    private void Awake()
    {
		ReceiveMessage(ProConst.UPDATE_GAMEOVER_UI, OnUpdateRequest);
    }

    void Start()
	{
		
	}

	public void OnClickReturn()
	{
		if (PatternGame == PatternCode.classic) {
			//_stage_control.Instance.retry_game();
			OpenUIForm(ProConst.CLASSIC_UIFORM);
			CloseUIForm();
			
		}
	}
	public void OnClickQuit() {
		if (PatternGame == PatternCode.classic)
		{
			//_stage_control.Instance._Destroy_game();
			CloseUIForm();
			OpenUIForm(ProConst.MENU_UIFORM);
			
			Game.Instance.Room.GamePattern = -1;
		}
	}
	public void OnClickQuitLianji() {
		CloseUIForm();
		OpenUIForm(ProConst.ROOM_UIFORM);
	}
	void OnUpdateRequest(KeyValuesUpdate kv) {
		string[] str = kv.Values as string[];
		string error = str[0];
		string GameInsideID=str[1];
		string UserName=str[2];
		winnerid.text = GameInsideID;
		winnername.text = UserName;
	}
}
