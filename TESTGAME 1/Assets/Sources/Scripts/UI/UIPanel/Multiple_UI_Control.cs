using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using DG.Tweening;
using System.Collections;

public class Multiple_UI_Control : BaseUIForm
{

    IEnumerator _playersuccess;
    IEnumerator _playerfail;

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
    private void Awake()
    {
        //ReceiveMessage(ProConst.UPDATE_MULTIPLE_UI, OnUpdateResponse);
        //ReceiveMessage(ProConst.SETTING_SET_UI, OnSettingResponse);
    }

    void Start()
	{
		Move = transform.Find("Move").GetComponent<Button>();
		Brick = transform.Find("Brick").GetComponent<Button>();
		Break = transform.Find("Break").GetComponent <Button>();
        Multiple_ingame_Setting = transform.Find("Multiple_ingame_Setting").GetComponent<Button>();

       
		Move.onClick.AddListener(() =>
		{
			//Multiple_Controller.Instance.Stage_Move_Player();
		});
		Brick.onClick.AddListener(() =>
		{
			//Multiple_Controller.Instance.Stage_Brick_Player();
		});
		Break.onClick.AddListener(() =>
		{
			//Multiple_Controller.Instance.Stage_Break_Player();
		});
        Multiple_ingame_Setting.onClick.AddListener(() =>
        {
            
        });
    }
	//void Update()
	//{
		
 //       if (startUI) {
 //           if (Multiple_Controller.Instance._is_game)
 //           {
 //               if (Multiple_Controller.Instance.isplayer_set)
 //               {
 //                   texts[0].text = "S";
 //                   texts[0].color = Color.blue;
 //                   texts[1].text = "";
 //               }
 //               if (Multiple_Controller.Instance.isplayer_ban)
 //               {
 //                   texts[0].text = Multiple_Controller.Instance.Ban_Point.ToString();
 //                   texts[0].color = Color.red;
 //                   texts[1].text = "";
 //               }
 //               if (!Multiple_Controller.Instance.isplayer_set && !Multiple_Controller.Instance.isplayer_ban) {
 //                   texts[0].text = "";
 //               }
 //               if (Multiple_Controller.Instance.isBrick)
 //               {
 //                   texts[0].color = Color.yellow;
 //               }
 //               if (Multiple_Controller.Instance.isBreak)
 //               {
 //                   texts[0].color = Color.red;
 //               }

 //               if (Multiple_Controller.Instance.isMove)
 //               {
 //                   texts[0].color = Multiple_Controller.Instance._game_configuration._color_list[1];
 //               }

 //               if (!Multiple_Controller.Instance.isMove && !Multiple_Controller.Instance.isBreak && !Multiple_Controller.Instance.isBrick)
 //               {
 //                   texts[0].color = Color.blue;
 //               }
 //               texts[0].text = Multiple_Controller.Instance.MovementPoint.ToString();
 //           }
            
            
            
 //       }
 //   }
    /*Response to Update*/
    void OnUpdateResponse(KeyValuesUpdate kv) {
        string[] str = kv.Values as string[];
        string error = str[0];
        if (error == "") {
            if (str[1] == "1")
            {
                startUI = true;
            }
            else if (str[1] == "0")
            {
                startUI = false;
            }
            else if (str[1] == "2")
            {
                _playersuccess = Animation_success();
                StartCoroutine(_playersuccess);
            }
            else if (str[1] == "3") {
                _playerfail = Animation_failure();
                StartCoroutine(_playerfail);
            }
        }
    }
    void OnSettingResponse(KeyValuesUpdate kv) { 

    }
    public void Success(bool can)
    {
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
        else
        {
            images[1].SetActive(false);
        }
    }
    /*Play bet success animation */
    IEnumerator Animation_success()
    {
        float elapsedTime = 0;
        float _Bet_time = 2f;
        //----------------------------------------------
        while (elapsedTime < _Bet_time)
        {
            //----------------------------------------------
            
            Success(true);
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();

            Success(false);
        }
    }
    /*Play bet failure animation */
    IEnumerator Animation_failure()
    {
        float elapsedTime = 0;
        float _Bet_time = 2f;
        //----------------------------------------------
        while (elapsedTime < _Bet_time)
        {
            //----------------------------------------------
            
            failure(true);
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();

            failure(false);
        }
    }
}
