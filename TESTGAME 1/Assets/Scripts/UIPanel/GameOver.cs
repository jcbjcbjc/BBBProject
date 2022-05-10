using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
public class GameOver : BasePanel
{
	static int PatternGame=-1;
	public static Text winner;
	public static Button ReturnRoom;
	public static Button _quitbutton;
	public static Button _qxitLianjibutton;
	protected override void Start()
	{
		base.Start();
	}

	public void OnClickReturn()
	{
		if (PatternGame == PatternCode.classic) {
			_stage_control.Instance.retry_game();
			Game.Instance.ShowPanel("Classic_ingame");
			Game.Instance.HidePanel("GameOverPanel");
		}
	}
	public void OnClickQuit() {
		if (PatternGame == PatternCode.classic)
		{
			_stage_control.Instance._Destroy_game();
			Game.Instance.HidePanel("GameOverPanel");
			Game.Instance.ShowPanel("MenuPanel");
			Game.Instance.Room.GamePattern = -1;
		}
	}
	public void OnClickQuitLianji() {
		Game.Instance.ShowPanel("RoomPanel");
		Game.Instance.HidePanel(name);
	}
	public static void SetWinnername(string data) { 
		winner.text = data;
	}
	public static void Setfrom(int st) {
		PatternGame = st;
		if (st == PatternCode.classic)
		{
			ReturnRoom.enabled = true;
			_qxitLianjibutton.enabled = false;
			_quitbutton.enabled = true;
		}
		else if (st == PatternCode.Multiple) {
			ReturnRoom.enabled = false;
			_qxitLianjibutton.enabled = true;
			_quitbutton.enabled = false;
		}
	}
}
