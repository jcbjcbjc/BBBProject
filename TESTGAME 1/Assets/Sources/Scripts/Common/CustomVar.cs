using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CustomVar:MonoBehaviour
{

	[Header("Configure Game")]
	//----------------------------------------------
	[Header("Game")]
	public int _attemps = 3;
	public int _score_on_touch = 250;
	public float _time_to_create_new_platform = 5f;
	public int _score_to_win_level = 5000;
	[Header("Design")]
	[Header("Colors")]
	public Color[] _color_list;
	[Header("Music")]
	public AudioSource[] _sounds;

	//----------------------------------------------
}
