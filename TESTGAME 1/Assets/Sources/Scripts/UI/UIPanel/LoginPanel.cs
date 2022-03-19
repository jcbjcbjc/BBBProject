using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Net;
using MessageHandler;
public class LoginPanel : BaseUIForm
{
	Button _loginButton;
	Button _registerButton;
	Button _closeButton;
	InputField _nameInput;
	InputField _pwdInput;

	void Awake() {
		ReceiveMessage(ProConst.LOGIN_MESSAGE, OnLoginResponse);
	}
	
	void Start ()
	{
		RigisterButtonObjectEvent("LoginButton", OnLoginClick);
		RigisterButtonObjectEvent("RegisterButton", OnRegisterClick);
		RigisterButtonObjectEvent("CloseButton", OnCloseClick);
		//_loginButton = transform.Find ("LoginButton").GetComponent<Button> ();
		//_registerButton = transform.Find ("RegisterButton").GetComponent<Button> ();
		//_closeButton = transform.Find ("CloseButton").GetComponent<Button> ();
		_nameInput = transform.Find ("NameInput").GetComponent<InputField> ();
		_pwdInput = transform.Find ("PwdInput").GetComponent<InputField> ();

		//_loginButton.onClick.AddListener (() => {
		//	OnLoginClick ();
		//});
		//_registerButton.onClick.AddListener (() => {
		//	OnRegisterClick ();
		//});
		//_closeButton.onClick.AddListener (() => {
		//	OnCloseClick ();
		//});OnCloseClick(GameObject go)
	}

	void OnLoginClick(GameObject go)
	{
		if (_nameInput.text == "") {
			string[] strArray = new string[] { "用户名不能为空", "2" };
			SendMessage(ProConst.SHOW_MESSAGE_MESSAGE, "", strArray);
			return;
		}
		if (_pwdInput.text == "") {
			string[] strArray = new string[] { "密码不能为空", "2" };
			SendMessage(ProConst.SHOW_MESSAGE_MESSAGE, "", strArray);
			return;
		}
		NetConn.Instance.Send (ProConst.LOGIN_MESSAGE,new LoginData(_nameInput.text, _pwdInput.text));
	}

	public void OnLoginResponse(KeyValuesUpdate kv)
	{
		MessageData messageData = kv.Values as MessageData;
		string data = messageData._data;
		if (data == "")
		{
			return;
		}
		if (data.Contains(";"))
		{
			var array = data.Split(';');
			ClientUserData user = new ClientUserData();
			user.Id = int.Parse(array[0]);
			user.Name = array[1];
			user.TotalCount = int.Parse(array[2]);
			user.WinCount = int.Parse(array[3]);
			Game.Instance.User = user;
			OpenUIForm(ProConst.MENU_UIFORM);
			CloseUIForm();
			Game.Instance.ShowMessage("登录成功");
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

	void OnRegisterClick(GameObject go)
	{
		OpenUIForm(ProConst.REGISTER_FORM);
		CloseUIForm();
	}

	void OnCloseClick(GameObject go)
	{
		OpenUIForm(ProConst.START_UIFORM);
		CloseUIForm();
	}
}
