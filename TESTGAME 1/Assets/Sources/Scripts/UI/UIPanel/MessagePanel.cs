using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessageHandler;
public class MessagePanel : BaseUIForm
{
	Text _text;
	string _message = "";
	float _showMessageTime = 2;
	private void Awake()
	{
		base.CurrentUIType.UIForms_Type = UIFormType.PopUp;
		
		base.CurrentUIType.UIForm_LucencyType = UIFormLucenyType.Translucence/*.Pentrate*/;

		ReceiveMessage(ProConst.SHOW_MESSAGE_MESSAGE, ShowMessage);
	}
	void Start ()
	{
		_text = GetComponent<Text> ();
		
	}

	void Update ()
	{
		if (_message != "") {
			_text.text = _message;
			Invoke ("Hide", _showMessageTime);
			_message = "";
		}
	}

	public void ShowMessage(KeyValuesUpdate kv)
	{
		OpenUIForm(ProConst.MESSAGE_FROMS);
		string[] strArray = kv.Values as string[];
		_message=strArray[0];
		_showMessageTime = int.Parse(strArray[1]);
	}

	void Hide()
	{
		CloseUIForm() ;
	}
}
