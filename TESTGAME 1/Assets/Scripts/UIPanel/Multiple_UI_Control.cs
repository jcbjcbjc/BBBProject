using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using DG.Tweening;
public class Multiple_UI_Control : BasePanel
{
	public Text[] texts;

	// Music
	// Music Off
	// FX
	// FX off
	[HideInInspector]
	public Image[] _hud_change_color;
    // InGame
    // GameOver
    // Start
    //----------------------------------------------
    bool startUI = false;

	static Button Move;
	static Button Brick;
	static Button Break;
	static Button Multiple_ingame_Setting;

    public GameObject[] images;

    protected override void Start()
	{
		Move = transform.Find("Move").GetComponent<Button>();
		Brick = transform.Find("Brick").GetComponent<Button>();
		Break = transform.Find("Break").GetComponent <Button>();
        Multiple_ingame_Setting = transform.Find("Multiple_ingame_Setting").GetComponent<Button>();

        base.Start();
		Move.onClick.AddListener(() =>
		{
			Multiple_Controller.Instance.Stage_Move_Player();
		});
		Brick.onClick.AddListener(() =>
		{
			Multiple_Controller.Instance.Stage_Brick_Player();
		});
		Break.onClick.AddListener(() =>
		{
			Multiple_Controller.Instance.Stage_Break_Player();
		});
        Multiple_ingame_Setting.onClick.AddListener(() =>
        {
            
        });
    }
	protected override void Update()
	{
		base.Update();
        if (startUI) {
            if (Multiple_Controller.Instance._is_game_pre)
            {
                if (Multiple_Controller.Instance.isplayer_set)
                {
                    texts[0].text = "S";
                    texts[0].color = Color.blue;
                    texts[1].text = "";
                }
                if (Multiple_Controller.Instance.isplayer_ban)
                {
                    texts[0].text = Multiple_Controller.Instance.Ban_Point.ToString();
                    texts[0].color = Color.red;
                    texts[1].text = "";
                }
                if (!Multiple_Controller.Instance.isplayer_set && !Multiple_Controller.Instance.isplayer_ban) {
                    texts[0].text = "";
                }
            }
            if (Multiple_Controller.Instance._is_game)
            {
                if (Multiple_Controller.Instance.isBrick)
                {
                    texts[0].color = Color.yellow;
                }
                if (Multiple_Controller.Instance.isBreak)
                {
                    texts[0].color = Color.red;
                }

                if (Multiple_Controller.Instance.isMove)
                {
                    texts[0].color = Multiple_Controller.Instance._game_configuration._color_list[1];
                }

                if (!Multiple_Controller.Instance.isMove && !Multiple_Controller.Instance.isBreak && !Multiple_Controller.Instance.isBrick)
                {
                    texts[0].color = Color.blue;
                }
                texts[0].text = Multiple_Controller.Instance.MovementPoint.ToString();
            }
        }
    }
    public void SetNumber(string data,int color) { 
        texts[1].text=data;
        if (color == 1) { texts[1].color =Color.blue; }
        else if (color == 2) { texts[1].color = Color.red; }
    }
    public void Success(bool can) {
        if (can)
        {
            images[0].SetActive(true);
        }
        else
        {
            images[0].SetActive(false);
        }
    }
    public void failure(bool can)
    {

        if (can)
        {
            images[1].SetActive(true);
        }
        else { 
            images[1].SetActive(false);
        }
    }
    public void SetStartUI(bool isget)
    {
        if (isget)
        {
            startUI = true;
        }
        else
        {
            startUI = false;
        }
    }

}
