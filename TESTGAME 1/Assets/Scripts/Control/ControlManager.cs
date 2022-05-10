using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class ControlManager :MonoBehaviour
{
	public static bool isOpenMultiple = false;
    public void Start()
    {
        
    }
    public void Update()
    {
		if (isOpenMultiple) {
			Debug.Log("打开控制器"+ Game.Instance.Room.RoomPositionID.ToString());
			OpenControlConnet("MultipleControl", Game.Instance.Room.RoomPositionID);
			Game.Instance.ShowPanel("Multiple_ingame");
			Game.Instance.HidePanel("RoomPanel");
			Game.Instance.HidePanel("RoomListPanel");
			isOpenMultiple=false;
		}
    }
    public static void Open(string data)
	{
		var strs = data.Split(';');
		int ID = int.Parse(strs[0]);
		int GamePattern = int.Parse(strs[1]);
		if (GamePattern == PatternCode.Multiple)
		{
			Game.Instance.Room.RoomPositionID = ID;
			isOpenMultiple =true;
		}
		else if (GamePattern == PatternCode.quartic)
		{

		}
		else if (GamePattern == PatternCode.hexgon) { 

		}
	}
	public static void OpenControlConnet(string name, int id) {
		GameObject obj = GameObject.Find(name);
		if (obj == null)
		{
			var parent = GameObject.Find("_game_control").transform;
			if (parent.gameObject != null)
			{
				obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Control/" + name), parent);
				obj.name = name;
				Debug.Log(id.ToString());
				obj.GetComponent<Multiple_Controller>().ID = id;
				
				return;
			}
		}
		return;
	}
	public static void OpenControlLocal(string name) {
		GameObject obj = GameObject.Find(name);
		if (obj == null)
		{
			var parent = GameObject.Find("_game_control").transform;
			if (parent.gameObject != null)
			{
				obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Control/" + name), parent);
				obj.name = name;
				return;
			}
		}
		return;
	}
	//public static void OpenPanel(string name) {
	//	BasePanel panel;
	//	bool isGet = Game.Instance._panelDict.TryGetValue(name, out panel);
	//	if (isGet == true)
	//	{
	//		panel.OnEnter();
	//	}
	//	else {
	//		var Panel = ViewManager.OpenPanel(name, "Canvas");
	//		Game.Instance._panelDict.Add(Panel.name, Panel);
	//		Panel.OnEnter();
	//	}
	//}
	public static void OnStartGameResponse(string data) {
		if (data == "-1") {
			Debug.Log("玩家还未准备");
			Game.Instance.ShowMessage("玩家还未准备");
			return;
		}
		if (data == "-2") {
			Debug.Log("您不是房主");
			Game.Instance.ShowMessage("您不是房主");
			return;
		}
		Open(data);
	}
	public static void Onquest(int GameCode, string data)
	{
		if (Game.Instance.Room.GamePattern == PatternCode.Multiple) {
			Multiple_Controller.Instance.Onquest(GameCode, data);
		}
	}
}
