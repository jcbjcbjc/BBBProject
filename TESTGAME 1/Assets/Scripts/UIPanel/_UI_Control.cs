using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _UI_Control : BasePanel
{

    bool startUI=false;

    Button Move1;
    Button Move2;
    Button Brick1;
    Button Brick2;
    Button Break1;
    Button Break2;

    public Button Classic_Return;
    public Text[] texts;
   
    public GameObject[] _UI_Little;
    // InGame
    // GameOver
    // Start
    //----------------------------------------------
    public Renderer _background;
    protected override void Start()
    {
        Move1 = transform.Find("Move1").GetComponent<Button>();
        Brick1 = transform.Find("Brick1").GetComponent<Button>();
        Break1 = transform.Find("Break1").GetComponent<Button>();
        Move2 = transform.Find("Move2").GetComponent<Button>();
        Brick2 = transform.Find("Brick2").GetComponent<Button>();
        Break2 = transform.Find("Break2").GetComponent<Button>();
        base.Start();
        Move1.onClick.AddListener(() =>
        {
            _stage_control.Instance.Stage_Move_Player1();
        });
        Brick1.onClick.AddListener(() =>
        {
            _stage_control.Instance.Stage_Brick_Player1();
        });
        Break1.onClick.AddListener(() =>
        {
            _stage_control.Instance.Stage_Break_Player1();
        });
        Move2.onClick.AddListener(() =>
        {
            _stage_control.Instance.Stage_Move_Player2();
        });
        Brick2.onClick.AddListener(() =>
        {
            _stage_control.Instance.Stage_Brick_Player2();
        });
        Break2.onClick.AddListener(() =>
        {
            _stage_control.Instance.Stage_Break_Player2();
        });
        Classic_Return.onClick.AddListener(() =>
        {
            ClassicReturn();
        });
    }
    protected override void Update()
    {
        base.Update();
        if (startUI)
        {
            if (_stage_control.Instance._is_game_pre)
            {
                if (_stage_control.Instance.isplayer1_set)
                {
                    texts[0].text = "S";
                    texts[0].color = Color.blue;
                    texts[1].text = "";
                }
                if (_stage_control.Instance.isplayer2_set)
                {
                    texts[1].text = "S";
                    texts[1].color = Color.blue;
                    texts[0].text = "";
                }
                if (_stage_control.Instance.isplayer1_ban)
                {
                    texts[0].text = _stage_control.Instance.Ban_Point1.ToString();
                    texts[0].color = Color.red;
                    texts[1].text = "";
                }
                if (_stage_control.Instance.isplayer2_ban)
                {
                    texts[1].text = _stage_control.Instance.Ban_Point2.ToString();
                    texts[1].color = Color.red;
                    texts[0].text = "";
                }
            }
            if (_stage_control.Instance._is_game)
            {
                if (_stage_control.Instance.isBrick1)
                {
                    texts[0].color = Color.yellow;
                }
                if (_stage_control.Instance.isBrick2)
                {
                    texts[1].color = Color.yellow;
                }
                if (_stage_control.Instance.isBreak1)
                {
                    texts[0].color = Color.red;
                }
                if (_stage_control.Instance.isBreak2)
                {
                    texts[1].color = Color.red;
                }
                if (_stage_control.Instance.isMove1)
                {
                    texts[0].color = _stage_control.Instance._game_configuration._color_list[1];
                }
                if (_stage_control.Instance.isMove2)
                {
                    texts[1].color = _stage_control.Instance._game_configuration._color_list[1];
                }
                if (!_stage_control.Instance.isMove1 && !_stage_control.Instance.isBreak1 && !_stage_control.Instance.isBrick1)
                {
                    texts[0].color = Color.blue;
                }
                if (!_stage_control.Instance.isMove2 && !_stage_control.Instance.isBreak2 && !_stage_control.Instance.isBrick2)
                {
                    texts[1].color = Color.blue;
                }

                texts[0].text = _stage_control.Instance.MovementPoint1.ToString();
                texts[1].text = _stage_control.Instance.MovementPoint2.ToString();
            }
        }
    }
    public void SetStartUI(bool isget) {
        if (isget)
        {
            startUI = true;
        }
        else {
            startUI = false;
        }
    }
    public void ClassicReturn() {
        startUI = false;
        Game.Instance.HidePanel("Classic_ingame");
        Game.Instance.ShowPanel("MenuPanel");
        Game.Instance.Room.GamePattern = -1;
        _stage_control.Instance._Destroy_game();
    }
    public void _change_hud(Color _c)
    {
        //----------------------------------------------
        _background.material.SetColor("_Color", _c);
        _background.material.SetColor("_EmissionColor", _c);
    }
}
