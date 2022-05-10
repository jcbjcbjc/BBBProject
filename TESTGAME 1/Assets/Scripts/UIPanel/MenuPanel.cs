using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class MenuPanel : BasePanel
{
	
	public Button Single;
	public Button Multiple;
	public Button Quartic;
	public Button Hexagon;
	public Button Classic;
	
	public Button Return;
	public bool _music=true;

	public Sprite[] _sprt_mfx;
	protected override void Start()
	{
		base.Start();
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
		Game.Instance.ShowPanel("RoomListPanel");
		Game.Instance.HidePanel("MenuPanel");
		NetConn.Instance.Send(RequestCode.Room, ActionCode.GetRoomList, "");
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
		Game.Instance.ShowPanel("Classic_ingame");
		Game.Instance.HidePanel("MenuPanel");
		Game.Instance.Room.GamePattern = PatternCode.classic;
	}
	void OnStartTutor()
	{
		Game.Instance.ShowPanel("TutorPanel");
		Game.Instance.HidePanel("MenuPanel");
	}
	public void _close_game()
	{
		Game.Instance.HidePanel("MenuPanel");
		Game.Instance.ShowPanel("StartPanel");
	}
}
