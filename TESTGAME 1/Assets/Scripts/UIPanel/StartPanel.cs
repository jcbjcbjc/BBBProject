using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
	Button _loginButton;
	Button _tutorButton;
	Button _StartSetting;
	Button _startaudio;
	protected override void Start ()
	{
		_loginButton =GameObject.Find("StartButton").GetComponent<Button> ();
		_tutorButton = GameObject.Find("TutorPlay").GetComponent <Button> ();
		_StartSetting=GameObject.Find("StartSetting").GetComponent<Button>();
		_startaudio = GameObject.Find("StartAudio").GetComponent<Button>();
		base.Start ();
		_loginButton.onClick.AddListener (() => {
			OnLoginClick ();
		});
		_tutorButton.onClick.AddListener(() =>
	    {
		   OnStartTutor();
	   });
		_StartSetting.onClick.AddListener(() =>
		{
			OnSetting();
		});
		_startaudio.onClick.AddListener(() =>
		{
			OnAudio();
		});
	}

	void OnLoginClick()
	{
		Game.Instance.ShowPanel ("LoginPanel");
		Game.Instance.HidePanel (name);
	}
	void OnStartTutor()
	{
		Game.Instance.ShowPanel("TutorPanel");
		Game.Instance.HidePanel("StartPanel");
	}
	void OnSetting() { 

	}
    void OnAudio()
    {
		if (Game._music)
		{
			GameObject.Find("_theme").GetComponent<AudioSource>().mute = true;
		}
		else {
			GameObject.Find("_theme").GetComponent<AudioSource>().mute = false;
		}
	}
}
