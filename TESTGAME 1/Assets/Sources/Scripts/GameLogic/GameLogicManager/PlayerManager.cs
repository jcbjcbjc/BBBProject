using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class PlayerManager 
{
	public static GameObject UOpenPlayer(Game_PlayerData _data)
	{
		var parent = GameObject.Find("_game_control").transform;

		var obj = ResourcesMgr.GetInstance().LoadAsset("Player\\Player", false);
		
		obj.transform.position=gadget.SwitchToPosition(_data .StartPoint.row, _data.StartPoint.col);

		obj.name = "Player"+ _data.RoomPositionID.ToString();
		return obj;
	}
	public static GameObject OpenClassicPlayer(string name)
	{
		var parent = GameObject.Find("_game_control").transform;

		var obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Player/" + name), parent);
		obj.name = "Player";
		return obj;
	}
	
}
