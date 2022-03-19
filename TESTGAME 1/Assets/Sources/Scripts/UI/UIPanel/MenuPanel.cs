using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Net;

public class MenuPanel : BaseUIForm
{
	
	public Button Single;
	public Button Multiple;
	public Button Quartic;
	public Button Hexagon;
	public Button Classic;
	
	public Button Return;
	public bool _music=true;

	public Sprite[] _sprt_mfx;
	
	void Start()
	{
		
		Single.onClick.AddListener(() => {
			OnStartSingle();
		});
		Multiple.onClick.AddListener(() => {
			OnStartMultiple();
		});
		Quartic.onClick.AddListener(() => {
			OnStartQuartic();
		});
		Hexagon.onClick.AddListener(() => {
			OnStartQuartic();
		});
		Classic.onClick.AddListener(() => {
			OnStartClassic();
		});
		Return.onClick.AddListener(() => 
		{
			_close_game();
		});
	}
	void OnStartSingle() {

	}
	void OnStartMultiple()
	{
		OpenUIForm(ProConst.ROOMLIST_UIFORM);
		CloseUIForm();
		
		NetConn.Instance.Send(ProConst.GETROOMLIST_MESSAGE, "");
	}

	void OnStartQuartic()
	{

	}

	void OnStartHexagon()
	{

	}

	void OnStartClassic()
	{
		ControlManager.OpenControlLocal("ClassicControl");
		OpenUIForm(ProConst.CLASSIC_UIFORM);
		CloseUIForm();
		
		Game.Instance.Room.GamePattern = PatternCode.classic;
	}
	//void OnStartTutor()
	//{
	//	Game.Instance.ShowPanel("TutorPanel");
	//	Game.Instance.HidePanel("MenuPanel");
	//}
	public void _close_game()
	{
		OpenUIForm(ProConst.START_UIFORM);
		CloseUIForm();
	}
}
