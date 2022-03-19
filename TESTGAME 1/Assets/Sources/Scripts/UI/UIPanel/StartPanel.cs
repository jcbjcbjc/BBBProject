using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel :BaseUIForm
{
	Button _loginButton;
	Button _tutorButton;
	Button _StartSetting;
	Button _startaudio;

    private void Awake()
    {
		base.CurrentUIType.IsHaveOpenEffect = true;
	}
    void Start ()
	{
		_loginButton =GameObject.Find("StartButton").GetComponent<Button> ();
		_tutorButton = GameObject.Find("TutorPlay").GetComponent <Button> ();
		_StartSetting=GameObject.Find("StartSetting").GetComponent<Button>();
		_startaudio = GameObject.Find("StartAudio").GetComponent<Button>();
		
		_loginButton.onClick.AddListener (() => {
			OnStartClick ();
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
	protected override void PlayOpenEffect()
	{




	}
	void OnStartClick()
	{
		OpenUIForm(ProConst.LOGIN_FROMS);
		CloseUIForm();
	}
	void OnStartTutor()
	{
		//Game.Instance.ShowPanel("TutorPanel");
		//Game.Instance.HidePanel("StartPanel");
	}
	void OnSetting() { 

	}
    void OnAudio()
    {
		
	}
}
