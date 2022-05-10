using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager 
{
	static Dictionary<string, BasePanel> _panelDict = new Dictionary<string, BasePanel> ();

	public static BasePanel OpenPanel(string name, string parentName)
	{
		GameObject obj = GameObject.Find (name);
		if (obj == null) {
			var parent = GameObject.Find (parentName).transform;
			if (parent.gameObject != null) {
				obj = GameObject.Instantiate<GameObject> (Resources.Load<GameObject> ("Panel/" + name), parent);
				obj.name = name;
				return obj.GetComponent<BasePanel> ();
			}
		}
		return obj.GetComponent<BasePanel> ();
	}

	public static GameObject OpenPrefab(string name, string parentName)
	{
		GameObject obj = GameObject.Find (name);
		if (obj == null) {
			var parent = GameObject.Find (parentName).transform;
			if (parent.gameObject != null) {
				obj = GameObject.Instantiate<GameObject> (Resources.Load<GameObject> ("Prefab/" + name), parent);
				obj.name = name;
				return obj;
			}
		}
		return obj;
	}

	public static void Close(string name)
	{
		var obj = GameObject.Find (name);
		if (obj != null) {
			GameObject.Destroy (obj);
		}
	}
}
