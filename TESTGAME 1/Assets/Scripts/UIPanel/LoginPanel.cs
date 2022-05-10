using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class LoginPanel : BasePanel
{
	Button _loginButton;
	Button _registerButton;
	Button _closeButton;
	InputField _nameInput;
	InputField _pwdInput;


	
	protected override void Start ()
	{
		_loginButton = transform.Find ("LoginButton").GetComponent<Button> ();
		_registerButton = transform.Find ("RegisterButton").GetComponent<Button> ();
		_closeButton = transform.Find ("CloseButton").GetComponent<Button> ();
		_nameInput = transform.Find ("NameInput").GetComponent<InputField> ();
		_pwdInput = transform.Find ("PwdInput").GetComponent<InputField> ();
		base.Start ();
		_loginButton.onClick.AddListener (() => {
			OnLoginClick ();
		});
		_registerButton.onClick.AddListener (() => {
			OnRegisterClick ();
		});
		_closeButton.onClick.AddListener (() => {
			OnCloseClick ();
		});
	}

	void OnLoginClick()
	{
		if (_nameInput.text == "") {
			Game.Instance.ShowMessage ("用户名不能为空");
			return;
		}
		if (_pwdInput.text == "") {
			Game.Instance.ShowMessage ("密码不能为空");
			return;
		}
		NetConn.Instance.Send (RequestCode.User, ActionCode.Login, _nameInput.text + ";" + _pwdInput.text);
	}

	public void OnLoginResponse(string data)
	{
		if (data == "") {
			return;
		}
		if (data.Contains(";")) {
			var array = data.Split (';');
			UserData user = new UserData ();
			user.Id = int.Parse (array [0]);
			user.Name = array [1];
			user.TotalCount = int.Parse (array [2]);
			user.WinCount = int.Parse (array [3]);
			Game.Instance.User = user;
			Game.Instance.ShowPanel ("MenuPanel");
			Game.Instance.HidePanel ("LoginPanel");
			Game.Instance.ShowMessage ("登录成功");
		}
		else if (data == "0") {
			Game.Instance.ShowMessage ("登录失败，用户不存在");
		}
		else if (data == "-1") {
			Game.Instance.ShowMessage ("登录失败，密码错误");
		}
		else if (data == "-3") {
			Game.Instance.ShowMessage ("登录失败，另一个玩家登录了这个用户名");
		}
	}

	void OnRegisterClick()
	{
		Game.Instance.ShowPanel ("RegisterPanel");
		Game.Instance.HidePanel ("LoginPanel");
	}

	void OnCloseClick()
	{
		Game.Instance.ShowPanel ("StartPanel");
		Game.Instance.HidePanel ("LoginPanel");
	}
}
