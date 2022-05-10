using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
//----------------------------------------------
public class _stage_control : MonoBehaviour {
    //单例模式构建控制代码
    private static _stage_control _instance;
    public static _stage_control Instance { get { if (!_instance) { _instance = GameObject.FindObjectOfType(typeof(_stage_control)) as _stage_control; } return _instance; } }

    public BasePanel UIControl;

    public CustomVar _game_configuration;

    public static Queue<GameObject> retry_queue;
    public GameObject _Platforms;
    public GameObject _good_particles;

    public GameObject cam;

    public GameObject _Player1;
    public GameObject _Player2;

    public bool is_Condition_1;
    public bool is_Condition_2;

    public GameObject SetPoint1;
    public GameObject SetPoint2;

    public GameObject BeginPoint1;
    public GameObject EndPoint1;

    public GameObject BeginPoint2;
    public GameObject EndPoint2;

    public bool isplayer1_set;
    public bool isplayer2_set;
    public bool isplayer1_ban;
    public bool isplayer2_ban;

    public int MovementPoint1 = 0;
    public int MovementPoint2 = 0;

    public bool isMove1 = false;
    public bool isBrick1 = false;
    public bool isBreak1 = false;

    public bool isMove2 = false;
    public bool isBrick2 = false;
    public bool isBreak2 = false;

    public bool isPlayer1 = false;
    public bool isPlayer2 = false;

    public int Ban_Point1 = 3;
    public int Ban_Point2 = 3;

    public int Bet_Number1;
    public int Bet_Number2;

    public int order;

    public int[] RandomPool;

    public AudioSource[] _sounds;
    // Good
    // Bad
    // Click
    GameObject[] map;

    IEnumerator _playersuccess;
    IEnumerator _playerfail;

    public bool _is_gameover = false;

    public bool _is_game = false;

    public bool _is_game_pre = false;

    public bool _music = true;

    float _Bet_time = 2f;

    void Awake()
    {
        BeginPoint1 = new GameObject();
        EndPoint1 = new GameObject();
        BeginPoint2 = new GameObject();
        EndPoint2 = new GameObject();

        UIControl = Game.Instance.GetPanel("Classic_ingame");

        _game_configuration = GameObject.Find("Custom").GetComponent<CustomVar>();
        cam = GameObject.Find("MainCamera");

        map = new GameObject[73];

        cam.transform.position = new Vector3(6, 50, 3.9f);
        retry_queue = new Queue<GameObject>();


        for (int i = 1; i <= 9; i++)
        {
            map[i] = generate_platform();
            map[i].transform.position = new Vector3(1.5f * i - 1.5f, 0, 0);
        }

        for (int i = 1; i <= 10; i++)
        {
            map[9 + i] = generate_platform();
            map[9 + i].transform.position = new Vector3(1.5f * i - 2.25f, 0, 1.3f);
        }

        for (int i = 1; i <= 11; i++)
        {
            map[19 + i] = generate_platform();
            map[19 + i].transform.position = new Vector3(1.5f * i - 3f, 0, 2.6f);
        }

        for (int i = 1; i <= 12; i++)
        {
            map[30 + i] = generate_platform();
            map[30 + i].transform.position = new Vector3(1.5f * i - 3.75f, 0, 3.9f);
        }

        for (int i = 1; i <= 11; i++)
        {
            map[42 + i] = generate_platform();
            map[42 + i].transform.position = new Vector3(1.5f * i - 3f, 0, 5.2f);
        }

        for (int i = 1; i <= 10; i++)
        {
            map[53 + i] = generate_platform();
            map[53 + i].transform.position = new Vector3(1.5f * i - 2.25f, 0, 6.5f);
        }

        for (int i = 1; i <= 9; i++)
        {
            map[63 + i] = generate_platform();
            map[63 + i].transform.position = new Vector3(1.5f * i - 1.5f, 0, 7.8f);
        }

        BeginPoint1.transform.position = new Vector3(14.25f, 0, 3.9f);
        BeginPoint2.transform.position = new Vector3(-2.25f, 0, 3.9f);
        EndPoint1.transform.position = new Vector3(-2.25f, 0, 3.9f);
        EndPoint2.transform.position = new Vector3(14.25f, 0, 3.9f);

        SetPoint1 = new GameObject();
        SetPoint2 = new GameObject();

        SetPoint1.transform.position = new Vector3(1, 0, 0);
        SetPoint2.transform.position = new Vector3(0, 0, 0);


        is_Condition_1 = false;
        is_Condition_2 = false;

        _Player1 = PlayerManager.OpenClassicPlayer("Player1");
        _Player2 = PlayerManager.OpenClassicPlayer("Player2");

        _Player1.transform.position = new Vector3(14.25f, 0, 3.9f);
        _Player2.transform.position = new Vector3(-2.25f, 0, 3.9f);

        

        RandomPool = new int[20];
        for (int i = 1; i <= 4; i++)
        {
            RandomPool[i] = 1;
        }
        for (int i = 5; i <= 8; i++)
        {
            RandomPool[i] = 2;
        }
        for (int i = 9; i <= 12; i++)
        {
            RandomPool[i] = 3;
        }
        for (int i = 13; i <= 16; i++)
        {
            RandomPool[i] = 4;
        }
        for (int i = 17; i <= 18; i++)
        {
            RandomPool[i] = 5;
        }
        RandomPool[19] = 6;
    }
    private void Start()
    {
        var obj = Game.Instance.GetPanel("Classic_ingame");
        obj.GetComponent<_UI_Control>().SetStartUI(true);
        Multiple_Start_game();
    }
    public void Multiple_Start_game()
    {
        cam.transform.position = new Vector3(6, 50, 3.9f);

        _is_game_pre = true;

        order = Random.Range(1, 3);

        if (order == 1)
        {
            isplayer1_set = true;
        }
        else
        {
            isplayer2_set = true;
        }

        GameObject.Find("_theme").GetComponent<AudioSource>().mute = false;
    }
    public void retry_game()
    {
        Multiple_Start_game();
        Ban_Point1 = 3;
        Ban_Point2 = 3;

        SetPoint1.GetComponent<_platform>()._text[1].gameObject.SetActive(false);
        SetPoint2.GetComponent<_platform>()._text[1].gameObject.SetActive(false);

        is_Condition_1 = false;
        is_Condition_2 = false;

        MovementPoint1 = 0;
        MovementPoint2 = 0;

        while (retry_queue.Count != 0) {
            retry_queue.Dequeue().GetComponent<_platform>()._sprt.color = _stage_control.Instance._game_configuration._color_list[0];
        }
        NavigationClassic.Instance.ClearGrid();

        _Player1.transform.position = new Vector3(14.25f, 0, 3.9f);
        _Player2.transform.position = new Vector3(-2.25f, 0, 3.9f);
    }
    GameObject generate_platform()
    {
        return Instantiate(_Platforms);
    }
    public void ConvertTo2()
    {
        //ui显示行动点不足且停留几秒
        //
        //移交转换权
        isPlayer1 = false;
        isPlayer2 = true;

        isBrick1 = false;
        isMove1 = false;
        isBreak1 = false;
        Start_2();
    }
    public void ConvertTo1()
    {
        //ui显示行动点不足且停留几秒
        //
        //移交转换权
        isPlayer2 = false;
        isPlayer1 = true;

        isBrick2 = false;
        isBreak2 = false;
        isMove2 = false;
        Start_1();
    }
    public void Start_1()
    {
        //ui
        int RandomPoint = Random.Range(1, 20);
        MovementPoint1 = RandomPool[RandomPoint];
        MovementPoint1 = Random.Range(1, 7);
    }
    public void Start_2()
    {
        //ui
        int RandomPoint = Random.Range(1, 20);
        MovementPoint2 = RandomPool[RandomPoint];
        MovementPoint2 = Random.Range(1, 7);
    }

    public void Stage_Move_Player1()
    {
        if (isPlayer1)
        {
            if (isBreak1) {
                return;
            }
            if (isBrick1)
            {
                isBrick1 = false;

                isMove1 = true;
                //ui显示
                return;
            }
            if (isMove1)
            {
                //ui取消
                isMove1 = false;
                return;
            }
            Debug.Log("lail");
            isMove1 = true;
            //ui显示全部亮起
            //
            ///////////////
        }
    }
    public void Stage_Move_Player2()
    {
        if (isPlayer2)
        {
            if (isBrick2 || isBreak2)
            {
                isBrick2 = false;
                isBreak2 = false;

                isMove2 = true;
                return;
            }
            if (isMove2)
            {
                //ui取消
                isMove2 = false;
                return;
            }
            Debug.Log("lail");
            isMove2 = true;
            //ui显示全部亮起
            //
            ///////////////

        }

    }
    public void Stage_Brick_Player1()
    {
        if (isPlayer1)
        {
            if (isBreak1)
            {
                return;
            }
            if (isMove1)
            {
                isMove1 = false;

                isBrick1 = true;
                //ui显示
                return;
            }
            if (isBrick1)
            {
                //ui取消
                isBrick1 = false;
                return;
            }
            isBrick1 = true;
            //ui显示全部亮起
            //
            ///////////////
        }
    }
    public void Stage_Brick_Player2()
    {
        if (isPlayer2)
        {
            if (isBreak2 || isMove2)
            {
                isBreak2 = false;
                isMove2 = false;

                isBrick2 = true;
                //ui显示
                return;
            }
            if (isBrick2)
            {
                //ui取消
                isBrick2 = false;
                return;
            }
            isBrick2 = true;
            //ui显示全部亮起
            //
            ///////////////

        }

    }
    public void Stage_Break_Player1()
    {
        if (isPlayer1)
        {
            if (isBreak1) {
                MovementPoint1 -= 3;
                isBreak1 = false;
                if (MovementPoint1 <= 0)
                {
                    ConvertTo2();
                }
                return;
            }
            if (MovementPoint1 >= 3)
            {
                isBrick1 = false;
                isMove1 = false;

                Bet_Number1 = Random.Range(0, 2);
                if (Bet_Number1 == 1)
                {
                    isBreak1 = true;
                    //ui显示
                    _playersuccess = _success1();
                    StartCoroutine(_playersuccess);
                }
                else
                {
                    _playerfail = _fail1();
                    StartCoroutine(_playerfail);
                    if (_music)
                    {
                        _sounds[0].Play();
                    }

                    isBreak1 = false;

                    MovementPoint1 -= 3;
                    if (MovementPoint1 <= 0)
                    {
                        ConvertTo2();
                        return;
                    }
                }
            }
            else {
                //UI显示
            }
        }
    }
    public void Stage_Break_Player2()
    {
        if (isPlayer2)
        {
            if (isBreak2)
            {
                MovementPoint2 -= 3;
                isBreak2 = false;
                if (MovementPoint2 <= 0)
                {
                    ConvertTo1();
                }
                return;
            }
            if (MovementPoint2 >= 3)
            {
                isBrick2 = false;
                isMove2 = false;

                Bet_Number2 = Random.Range(0, 2);
                if (Bet_Number2 == 1)
                {
                    isBreak2 = true;
                    //ui显示

                    _playersuccess = _success2();
                    StartCoroutine(_playersuccess);
                }
                else
                {

                    _playerfail = _fail2();
                    StartCoroutine(_playerfail);
                    if (_music)
                    {
                        _sounds[0].Play();
                    }

                    isBreak2 = false;

                    MovementPoint2 -= 3;
                    if (MovementPoint2 <= 0)
                    {
                        ConvertTo1();
                        return;
                    }
                }
            }
            else
            {
                //UI显示
            }
        }
    }

    public void _gameover(int winner)
    {
        _is_gameover = true;
        _is_game = false;

        cam.transform.position = new Vector3(21, -157, -881);
        GameOver.Setfrom(PatternCode.classic);
        Game.Instance.ShowPanel("GameOverPanel");

        var obj = Game.Instance.GetPanel("Classic_ingame");
        obj.GetComponent<_UI_Control>().SetStartUI(false);

        Game.Instance.HidePanel("Classic_ingame");

        GameObject.Find("_theme").GetComponent<AudioSource>().mute = true;
    }
    public void _close_game()
    {
        Application.Quit();
    }
    //----------------------------------------------
    public void _Destroy_game()
    {
        cam.transform.position = new Vector3(21, -157, -881);
        GameObject.Find("_theme").GetComponent<AudioSource>().mute = true;
        for (int i = 0; i <= 72; i++) {
            Destroy(map[i]);
        }
        Destroy(BeginPoint1);
        Destroy(EndPoint1);
        Destroy(EndPoint2);
        Destroy(BeginPoint2);
        Destroy(SetPoint1);
        Destroy(SetPoint2);
        Destroy(_Player1);
        Destroy(_Player2);
        Destroy(GameObject.Find("ClassicControl"));
    }
        
    IEnumerator _success1()
    {
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime < _Bet_time)
        {
            //----------------------------------------------
            UIControl.GetComponent<_UI_Control>()._UI_Little[0].SetActive(true);
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();

            UIControl.GetComponent<_UI_Control>()._UI_Little[0].SetActive(false);
        }
    }
    IEnumerator _fail1()
    {
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime < _Bet_time)
        {
            //----------------------------------------------
            UIControl.GetComponent<_UI_Control>()._UI_Little[1].SetActive(true);
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();

            UIControl.GetComponent<_UI_Control>()._UI_Little[1].SetActive(false);
        }
    }
    IEnumerator _success2()
    {
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime < _Bet_time)
        {
            //----------------------------------------------
            UIControl.GetComponent<_UI_Control>()._UI_Little[2].SetActive(true);
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();

            UIControl.GetComponent<_UI_Control>()._UI_Little[2].SetActive(false);
            
        }
    }
    IEnumerator _fail2()
    {
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime < _Bet_time)
        {
            //----------------------------------------------
            UIControl.GetComponent<_UI_Control>()._UI_Little[3].SetActive(true);
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();

            UIControl.GetComponent<_UI_Control>()._UI_Little[3].SetActive(false);
            
        }
    }

    IEnumerator _sorrymuch() {
        float elapsedTime = 0;
        float time = 0.5f;
        //----------------------------------------------
        while (elapsedTime < time)
        {
            //----------------------------------------------
            UIControl.GetComponent<_UI_Control>()._UI_Little[4].SetActive(true);
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();

            UIControl.GetComponent<_UI_Control>()._UI_Little[4].SetActive(false);

        }
    }
}