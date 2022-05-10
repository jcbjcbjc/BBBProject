using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class BaseControl : MonoBehaviour
{
    public int ID;

    public int MaxCount;

    protected List<MyPoint> _MapData;
    protected List<PlayerData> _PlayerData;

    public bool isEnterGame = false;
    public bool isStartmyselfGame = false;

    public Navigation _navigation;
    public MapEditor _mapEditor;
    public CustomVar _game_configuration;

    public GameObject _Platforms;
    public GameObject _good_particles;
    public GameObject cam;

    public GameObject _all_parent;

    public GameObject[] _Players;

    public GameObject _Player;

    protected MyPoint[] StartPoint;
    protected MyPoint[] EndPoint;

    public bool isPlayer;

    public int order;

    public bool _is_game;

    public int MovementPoint;

    public bool isMove;
    public bool isBrick;
    public bool isBreak;
   

    protected void Create_Map()
    {
        _MapData = Game.Instance.Room.MapData;
        _mapEditor = new MapEditor(_MapData);
        _navigation =new Navigation(_MapData);
    }
    public virtual bool Navigation_Obstacle(GameObject _go) {
        return true;
    }
    protected void Init()
    {
        ID = Game.Instance.Room.RoomPositionID;
        MaxCount=Game.Instance.Room.MaxCount;
    }
    protected virtual void InitGame()
    {
        
    }
    protected void Create_Player()
    {
        _PlayerData=Game.Instance.Room.PlayerData;
        _Players=new GameObject[MaxCount];

        StartPoint = new MyPoint[MaxCount];
        EndPoint = new MyPoint[MaxCount];

        for (int i=0; i<_PlayerData.Count;i++) {
            _Players[i]=PlayerManager.UOpenPlayer(_PlayerData[i]);
            StartPoint[i] = _PlayerData[i].StartPoint;
            EndPoint[i] = _PlayerData[i].EndPoint;
            if (ID == i + 1) { _Player = _Players[i]; }
        }

    }
    protected virtual void  Camera_Change()
    {
   
    }
    public void SendRoomMessege(int RoomCode, string data)
    {
        NetConn.Instance.Send(RequestCode.Room, RoomCode, data);
    }
    public void SendGameMessege(int GameCode, string data)
    {
        NetConn.Instance.Send(RequestCode.Game, GameCode, data);
    }
    protected virtual void Onquest(int GaCode, string data)
    {
        
    }
    protected virtual void SendSelectOrderMessenge()
    {
        
    }
    protected virtual void Convert()
    {

    }

    private void Awake()
    {
        Init();
        Create_Map();
        InitGame();
        Create_Player();
        Camera_Change();
    }

    // Start is called before the first frame update
    void Start()
    {
        SendRoomMessege(ActionCode.StartGameConfirm, "");
    }
    protected virtual void Update()
    {
        if (isEnterGame)
        {
            SendSelectOrderMessenge();
            isEnterGame = false;
        }
        if (_is_game && isPlayer)
        {
            Move_Player();
            Brick_Player();
        }
        if (_is_game)
        {
            Condition_Jugde();
        }
        if (isBreak)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out RaycastHit _hit))
                {
                    _Player.GetComponent<TalentPlayer>().Break_Player_to(_hit.collider.gameObject);
                }
                //更新行动点
            }
        }
        execute();
    }
    protected virtual void OnDestroy() {
       
    }
    protected virtual void execute() {
    }
    protected virtual void Condition_Jugde()
    {

    }
    public void Stage_Move_Player()
    {
        if (isPlayer)
        {
            if (isBreak)
            {
                return;
            }
            if (isBrick)
            {
                isBrick = false;

                isMove = true;
                //ui显示
                return;
            }
            if (isMove)
            {
                //ui取消
                isMove = false;
                return;
            }
            isMove = true;
            //ui显示全部亮起
            //
            ///////////////
        }
    }
    public void Stage_Brick_Player()
    {
        if (isPlayer)
        {
            if (isBreak)
            {
                return;
            }
            if (isMove)
            {
                isMove = false;

                isBrick = true;
                //ui显示
                return;
            }
            if (isBrick)
            {
                //ui取消
                isBrick = false;
                return;
            }
            isBrick = true;
            //ui显示全部亮起
            //
            ///////////////
        }
    }
    public void Stage_Break_Player()
    {
        if (isPlayer)
        {
            if (isBreak)
            {
                MovementPoint -= 3;
                SendGameMessege(GameCode.ConveyPoint, MovementPoint.ToString() + ";" + "1");
                isBreak = false;
                if (MovementPoint <= 0)
                {
                    Convert();
                }
                return;
            }
            if (MovementPoint >= 3)
            {
                isBrick = false;
                isMove = false;

                int Bet_Number = Random.Range(0, 2);
                if (Bet_Number == 1)
                {
                    isBreak = true;
                    //ui显示
                    //_playersuccess = _success1();
                    //StartCoroutine(_playersuccess);
                }
                else
                {
                    //_playerfail = _fail1();
                    //StartCoroutine(_playerfail);
              
                    isBreak = false;

                    MovementPoint -= 3;
                    SendGameMessege(GameCode.ConveyPoint, MovementPoint.ToString() + ";" + "1");
                    if (MovementPoint <= 0)
                    {
                        Convert();
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
    public void Move_Player()
    {
        if (isMove)
        {
            if (MovementPoint > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out RaycastHit _hit))
                    {
                        _Player.GetComponent<TalentPlayer>().Move_Player_to(_hit.collider.gameObject);
                    }
                }
            }
            else
            {
                Convert();
                return;
            }
        }
    }
    public void Brick_Player()
    {
        if (isBrick)
        {
            if (MovementPoint > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out RaycastHit _hit))
                    {
                        _Player.GetComponent<TalentPlayer>().Brick_Player_to(_hit.collider.gameObject);
                    }
                }
            }
            else
            {
                Convert();
                return;
            }
        }
    }

    public void SetStatusBrick(GameObject _go)
    {
        MyPoint myPoint = gadget.SwitchToCoord(_go.transform);
        /////////////////////////////////////////////////////////
        _navigation.ChangeMapStatus(myPoint.row, myPoint.col, 1);
        ////////////////////////////////////////////////////////////
        ///
        _mapEditor.ChangeMapStatus(myPoint.row, myPoint.col, MapCode.Brick);
    }


    /******************************************************************************
     * BaseControl::SetStatusBan.                            *
     *                                                                            *
     *    The camera's pos(ition) is the absolute position on the destination     *
     *    image. The camera will draw the final width * height image at this pos  *
     *    on the pDestImg.                                                        *
     *                                                                            *
     * INPUT:  GameObject                        *
     *                                                                            *
     * OUTPUT:  none                                                              *
     *                                                                            *
     * WARNINGS:  none                                                            *
     *                                                                            *
     * HISTORY:                                                                   *
     *   2022/01/10 JCB : Created.                                               *
     *============================================================================*/

    public void SetStatusBan(GameObject _go)
    {
        MyPoint myPoint = gadget.SwitchToCoord(_go.transform);

        _navigation.ChangeMapStatus(myPoint.row, myPoint.col, 1);

        _mapEditor.ChangeMapStatus(myPoint.row, myPoint.col, MapCode.Ban);
    }



    public void DeleteStatus(GameObject _go)
    {
        MyPoint myPoint = gadget.SwitchToCoord(_go.transform);
        ///////////////////////////////////////////////////////////////
        _navigation.ChangeMapStatus(myPoint.row, myPoint.col, 0);
  
        _mapEditor.ChangeMapStatus(myPoint.row, myPoint.col, MapCode.Default);
    }
    //----------------------------------------------
    public void _close_game()
    {
        Application.Quit();
    }
    //----------------------------------------------
   
    
    public virtual void Gameover(string data)
    {
        
    }
    
}
