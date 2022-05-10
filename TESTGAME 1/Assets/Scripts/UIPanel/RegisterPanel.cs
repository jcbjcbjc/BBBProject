using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class RegisterPanel : BasePanel
{
	Button _registerButton;
	Button _closeButton;
	InputField _nameInput;
	InputField _pwdInput;
	InputField _repeatPwdInput;

	protected override void Start ()
	{
		_registerButton = transform.Find ("RegisterButton").GetComponent<Button> ();
		_closeButton = transform.Find ("CloseButton").GetComponent<Button> ();
		_nameInput = transform.Find ("NameInput").GetComponent<InputField> ();
		_pwdInput = transform.Find ("PwdInput").GetComponent<InputField> ();
		_repeatPwdInput = transform.Find ("RepeatPwdInput").GetComponent<InputField> ();
		base.Start ();
		_registerButton.onClick.AddListener (() => {
			OnRegisterClick ();
		});
		_closeButton.onClick.AddListener (() => {
			OnCloseClick ();
		});
	}

	void OnRegisterClick()
	{
		if (_nameInput.text == "") {
			Game.Instance.ShowMessage ("用户名不能为空");
			return;
		}
		if (_pwdInput.text == "") {
			Game.Instance.ShowMessage ("密码不能为空");
			return;
		}
		if (_pwdInput.text != _repeatPwdInput.text) {
			Game.Instance.ShowMessage ("两次密码输入不一致");
			return;
		}
		NetConn.Instance.Send (RequestCode.User, ActionCode.Register, _nameInput.text + ";" + _pwdInput.text);
	}

	public void OnRegisterResponse(string data)
	{
		if (data == "") {
			return;
		}
		if (data == "1") {
			Game.Instance.ShowPanel ("StartPanel");
			Game.Instance.HidePanel ("RegisterPanel");
			Game.Instance.ShowMessage ("注册成功");
		}
		else if (data == "-2") {
			Game.Instance.ShowMessage ("注册失败，用户已存在");
		}
	}

	void OnCloseClick()
	{
		Game.Instance.ShowPanel ("StartPanel");
		Game.Instance.HidePanel ("RegisterPanel");
	}
}
