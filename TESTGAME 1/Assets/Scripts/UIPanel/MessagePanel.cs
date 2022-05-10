using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{
	Text _text;
	string _message = "";
	float _showMessageTime = 2;

	protected override void Start ()
	{
		_text = GetComponent<Text> ();
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
		if (_message != "") {
			_text.text = _message;
			Invoke ("Hide", _showMessageTime);
			_message = "";
		}
	}

	public void ShowMessage(string message, float showMessageTime = 2)
	{
		_message = message;
		_showMessageTime = showMessageTime;
	}

	void Hide()
	{
		OnExit ();
	}
}
