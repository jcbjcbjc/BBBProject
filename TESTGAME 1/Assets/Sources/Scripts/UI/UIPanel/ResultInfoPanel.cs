using Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultInfoPanel : BaseUIForm
{
	Text _userNameText;
	Text _totalCountText;
	Text _winCountText;
	Button _closeButton;

	void Start ()
	{
		_userNameText = transform.Find ("UserNameText").GetComponent<Text> ();
		_totalCountText = transform.Find ("TotalCountText").GetComponent<Text> ();
		_winCountText = transform.Find ("WinCountText").GetComponent<Text> ();
		_closeButton = transform.Find ("CloseButton").GetComponent<Button> ();
		
		_closeButton.onClick.AddListener (() => {
			OnCloseClick ();
		});
	}

	void OnCloseClick()
	{
		CloseUIForm();
	}

	void Update ()
	{
		
        //if (Game.Instance.User.Id != 0)
        //{
  //      var user = Game.Instance.User;
		//_userNameText.text = user.Name;
		//_totalCountText.text = "总场数：" + user.TotalCount;
		//_winCountText.text = "胜利：" + user.WinCount;
        //}
    }

	public void OnGetResultInfoResponse(string data)
	{
		if (data.Contains(";")) {
			var array = data.Split (';');
			ClientUserData user = new ClientUserData ();
			user.Id = int.Parse (array [0]);
			user.Name = array [1];
			user.TotalCount = int.Parse (array [2]);
			user.WinCount = int.Parse (array [3]);
			Game.Instance.User = user;
		}
	}
}
