using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class PlayerManager 
{
	public static GameObject OpenPlayer(string name)
	{
		var parent = GameObject.Find("_game_control").transform;

		var obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Player/" + name), parent);
		obj.name = "Player";
		return obj;
	}
	public static GameObject OpenClassicPlayer(string name) {
		var parent = GameObject.Find("_game_control").transform;

		var obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Player/" + name), parent);
		obj.name = "Player";
		return obj;
	}
	public static GameObject UOpenPlayer(Data _data)
	{
		var parent = GameObject.Find("_game_control").transform;

		var obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Player/" + (_data as PlayerData).name), parent);

		obj.transform.position=gadget.SwitchToPosition((_data as PlayerData).StartPoint.row, (_data as PlayerData).StartPoint.col);





		obj.name = "Player";
		return obj;
	}
}
