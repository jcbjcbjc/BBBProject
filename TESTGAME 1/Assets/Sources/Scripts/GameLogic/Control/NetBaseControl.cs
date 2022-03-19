using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Net;
public class NetBaseControl : MonoBehaviour
{

    #region Declare
    int _ID;

    public int MyID { 
        get { return _ID; }
        set { _ID = value; }
    }

    public int Current_id;

    int _MaxCount;
    public int MaxCount
    {
        get { return _MaxCount; }
        set { _MaxCount = value; }
    }

    public int Next_ID;

    protected List<MyPoint> _MapData;
    protected List<Game_PlayerData> _PlayerData;

    public Navigation _navigation;
    public MapEditor _mapEditor;
    public CustomVar _game_configuration;

    public GameObject[] _Players;

    public GameObject _Player;

    protected Transform[] StartPoint;
    protected Transform[] EndPoint;

    public bool isPlayer;

    public int order;

    public bool _is_game;

    public int[] MovementPoint;

    public bool isMove;
    public bool isBrick;
    public bool isBreak;

    IEnumerator _Player_Move;


    #endregion

    #region Awake
    private void Awake()
    {
        Init();
        Register();
        Create_Map();
        InitGame();
        Create_Player();
        Camera_Change();
        UI_Init();
    }
    /*Register messageType to response */
    protected virtual void Register()
    {
    }
    /* Init the UI configuration */
    protected virtual void UI_Init() { }
    /* Create Map */
    protected void Create_Map()
    {
        
        _mapEditor = new MapEditor(_MapData);
        _navigation =new Navigation(_MapData);
    }

    /* Init the whole controller 
     * import all data needed*/
    protected void Init()
    {
        MyID = Game.Instance.GameConfig.RoomPositionID;
        MaxCount=Game.Instance.Room.MaxCount;
        _MapData = Game.Instance.GameConfig.MapData;
        _PlayerData = Game.Instance.GameConfig.PlayerData;
    }
    /* Init all detailed data according to essential data */
    protected virtual void InitGame()
    {
        _game_configuration = GameObject.Find("Custom").GetComponent<CustomVar>();
        MovementPoint=new int[MaxCount+1];
    }
    /* Create players */
    protected void Create_Player()
    {
        _Players=new GameObject[MaxCount+1];

        StartPoint = new Transform[MaxCount+1];
        EndPoint = new Transform[MaxCount+1];

        for (int i=1; i<=_PlayerData.Count;i++) {
            _Players[i]=PlayerManager.UOpenPlayer(_PlayerData[i]);
            StartPoint[i] =_mapEditor.GetMap(_PlayerData[i].StartPoint.row, _PlayerData[i].StartPoint.col).transform;
            EndPoint[i] = _mapEditor.GetMap(_PlayerData[i].EndPoint.row, _PlayerData[i].EndPoint.col).transform;
            if (MyID == i) { _Player = _Players[i]; }
        }

    }
    /* Change the camera */
    protected void  Camera_Change()
    {
        Camera.main.transform.position = new Vector3(_PlayerData[MyID].CameraPosition.x, _PlayerData[MyID].CameraPosition.y, _PlayerData[MyID].CameraPosition.z);
        Camera.main.transform.Rotate(0,0, _PlayerData[MyID].Direction) ;



    }

    #endregion


    /*Send loaded message to server to make sure that all client enter the game successfully*/
    void Start()
    {
        SendNetMessege(ProConst.CONFIRM_START_GAME,  "" );
    }

    protected virtual void Update()
    {

    }

    #region Message
    /*Send NetWorking Data*/
    public void SendNetMessege(string MessageType, object data)
    {
        NetConn.Instance.Send(MessageType, data);
    }
    /*Send Inside Data such as UI message*/
    public void SendInsideMessage(string MessageType, object data)
    {
        MessageCenter.SendMessage(MessageType, data);
    }
    #endregion

    /* Order */
    protected virtual void SendSelectOrderMessenge()
    {
        
    }
    /* Update UI with real-time data */
    protected virtual void UI_Update()
    {

    }
    /* Convert character */
    protected virtual void Convert()
    {

    }
    /* Jugde the success Condition */
    protected virtual void Condition_Jugde()
    {

    }

    #region OnquestResponse

    protected void OnBreakResultResponse(KeyValuesUpdate kv)
    {
        MessageDatas str = kv.Values as MessageDatas;
        int id = int.Parse(str.param1);
        int result = int.Parse(str.param2);
        if (result == 1)
        {
            SendInsideMessage(ProConst.UPDATE_SET_UI, new UI_Set_UpdateData(SetUICode.Success.ToString(), id));
        }
        else {
            SendInsideMessage(ProConst.UPDATE_SET_UI, new UI_Set_UpdateData(SetUICode.Failure.ToString(), id));
        }
        ///////////////////////////////////////////////////////////////////////////////////////
        ////linadajhdhaskdhasjdgkjasdhjahdjashdashdjkahdjkhasdhaskjhdhaskdjhaskjdh
        ///赌输了////
        ///////
    }
    protected void OnMoveResponse(KeyValuesUpdate kv)
    {
        Game_PositionData str = kv.Values as Game_PositionData;
        int id = str._id;
        int row = str._row;
        int col =str._col;
        Move_Player_to(id, _mapEditor.GetMap(row,col));
    }
    protected void OnBrickResponse(KeyValuesUpdate kv)
    {
        Game_PositionData str = kv.Values as Game_PositionData;
        int id = str._id;
        int row = str._row;
        int col = str._col;
        Brick_Player_to(id,_mapEditor.GetMap(row, col));
    }
    protected void OnBreakResponse(KeyValuesUpdate kv)
    {
        Game_PositionData str = kv.Values as Game_PositionData;
        int id = str._id;
        int row = str._row;
        int col = str._col;
        Break_Player_to(id, _mapEditor.GetMap(row, col));
    }


    #endregion

    #region detailed function
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
                MovementPoint[MyID] -= ProConst.BREAK_COST;

                isBreak = false;
                if (MovementPoint[MyID] <= 0)
                {
                    Convert();
                }
                return;
            }
            if (MovementPoint[MyID] >= ProConst.BREAK_COST)
            {
                isBrick = false;
                isMove = false;

                int Bet_Number = Random.Range(0, 2);
                if (Bet_Number == 1)
                {
                    isBreak = true;
                    SendNetMessege(ProConst.BREAK_RESULT_MESSAGE, new MessageDatas(MyID.ToString(), "1"));
                    SendInsideMessage(ProConst.UPDATE_SET_UI, new UI_Set_UpdateData(SetUICode.Success.ToString(), MyID));
                    //ui显示
                   
                }
                else
                {
                    isBreak = false;
                    SendNetMessege(ProConst.BREAK_RESULT_MESSAGE, new MessageDatas(MyID.ToString(), "0"));
                    SendInsideMessage(ProConst.UPDATE_SET_UI, new UI_Set_UpdateData(SetUICode.Failure.ToString(), MyID));
                    


                    MovementPoint[MyID] -= ProConst.BREAK_COST;
                    if (MovementPoint[MyID] <= 0)
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
    protected void Move_Player()
    {
        if (!isMove){ return; }
        if (MovementPoint[MyID] <= 0){ isMove = false; Convert(); return;}
        if (Input.GetMouseButtonDown(0))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out RaycastHit _hit))
            {
                if (IsMove(_Players[MyID].transform,_hit.collider.gameObject.transform)){

                    SendNetMessege(ProConst.MOVE_INSTRUCTION, new Game_PositionData(MyID, _hit.collider.gameObject.GetComponent<_platform>().row, _hit.collider.gameObject.GetComponent<_platform>().col,0));

                    Move_Player_to(MyID,_hit.collider.gameObject);
                }
            }
        }
    }
    protected void Brick_Player()
    {
        if (!isBrick) { return; }
        if (MovementPoint[MyID] <= 0) { isBrick = false; Convert(); return; }
        if (Input.GetMouseButtonDown(0))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out RaycastHit _hit))
            {
                if (IsBrick(_Players[MyID].transform, _hit.collider.gameObject))
                {
                    SendNetMessege(ProConst.BRICK_INSTRUCTION, new Game_PositionData(MyID, _hit.collider.gameObject.GetComponent<_platform>().row, _hit.collider.gameObject.GetComponent<_platform>().col, 0));

                    Brick_Player_to(MyID, _hit.collider.gameObject);
                }
            }
        }
    }
    protected void Break_Player() {
        if (!isBreak) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out RaycastHit _hit))
            {
                if (IsBreak(_Players[MyID].transform, _hit.collider.gameObject)) {
                    isBreak = false;
                    SendNetMessege(ProConst.BREAK_INSTRUCTION, new Game_PositionData(MyID, _hit.collider.gameObject.GetComponent<_platform>().row, _hit.collider.gameObject.GetComponent<_platform>().col, 0));

                    Break_Player_to(MyID, _hit.collider.gameObject);
                    if (MovementPoint[MyID] <= 0)
                    {
                        Convert();
                    }
                }
               
            }
            //更新行动点
        }
    }
    public void Move_Player_to(int ID,GameObject _go)
    {
       
        _Player_Move = _jump(_Players[ID],_go.transform.position);
        StartCoroutine(_Player_Move);

        MovementPoint[ID]--;

        //GameObject _t = Instantiate(Multiple_Controller.Instance._good_particles);
        //_t.GetComponent<ParticleSystem>().startColor = _go.GetComponent<_platform>()._sprt.color;
        //_t.transform.position = _go.transform.position;
    }
    public void Brick_Player_to(int ID,GameObject _go)
    {
        SetStatusBrick(_go.transform);

        

        MovementPoint[ID]-=ProConst.MOVE_COST;
    }
    public void Break_Player_to(int ID,GameObject _go)
    {
        MovementPoint[ID] -= ProConst.BREAK_COST;
        
        DeleteStatus(_go.transform);

        if (MovementPoint[MyID] <= 0)
        {
            Convert();
            return;
        }

    }
    IEnumerator _jump(GameObject Player,Vector3 _end)
    {
        //----------------------------------------------

        //----------------------------------------------
        Vector3 _start = Player.transform.position;
        //----------------------------------------------
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime <ProConst.MOVE_TIME)
        {
            //----------------------------------------------
            this.transform.position = Vector3.Lerp(_start, _end, (elapsedTime / ProConst.MOVE_TIME));
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();
        }
        Player.transform.position = _end;
    }
    #endregion

    #region tools
    public virtual bool Navigation_Obstacle(Transform _go)
    {
        return true;
    }
    public void SetStatusBrick(Transform _go)
    {
        _go.GetComponent<_platform>()._sprt.color = Color.yellow;

        MyPoint myPoint = gadget.SwitchToCoord(_go);
        /////////////////////////////////////////////////////////
        _navigation.ChangeMapStatus(myPoint.row, myPoint.col, 1);
        ////////////////////////////////////////////////////////////
        ///
        _mapEditor.ChangeMapStatus(myPoint.row, myPoint.col, MapCode.Brick);
    }
    public void SetStatusBan(Transform _go)
    {
        _go.GetComponent<_platform>()._sprt.color = Color.red;

        MyPoint myPoint = gadget.SwitchToCoord(_go);

        _navigation.ChangeMapStatus(myPoint.row, myPoint.col, 1);

        _mapEditor.ChangeMapStatus(myPoint.row, myPoint.col, MapCode.Ban);
    }
    public void DeleteStatus(Transform _go)
    {
        _go.GetComponent<_platform>()._sprt.color = _game_configuration._color_list[0];

        MyPoint myPoint = gadget.SwitchToCoord(_go);
        ///////////////////////////////////////////////////////////////
        _navigation.ChangeMapStatus(myPoint.row, myPoint.col, 0);
  
        _mapEditor.ChangeMapStatus(myPoint.row, myPoint.col, MapCode.Default);
    }
    
    bool IsMove(Transform obj, Transform arrive)
    {
        if (gadget.CalculateDistance(obj, arrive) > 0.1f && gadget.CalculateDistance(obj, arrive) < 1.7f)
        {
            if (arrive.GetComponent<_platform>().status == MapCode.Brick || arrive.GetComponent<_platform>().status == MapCode.Ban)
            {
                return false;
            }
            else { return true; }
        }
        else
        {
            return false;
        }
    }
    bool IsBrick(Transform obj, GameObject arrive)
    {
        if (arrive.GetComponent<_platform>().status != MapCode.Default) { return false; }
        for (int i = 1; i <= MaxCount; i++) {
            if (arrive.transform.position == _Players[i].transform.position) { return false; }
        }
        if (!Navigation_Obstacle(arrive.transform))
        {
            return false;
        }
        return true;
    }
    bool IsBreak(Transform obj, GameObject arrive) {
        if (arrive.GetComponent<_platform>().status != MapCode.Default) { return false; }
        return true;
    }
    #endregion

    #region GameOver
    protected virtual void OnDestroy()
    {
       
    }
    protected virtual void Gameover(KeyValuesUpdate kv)
    {

        
    }
    #endregion


    public void _close_game()
    {
        Application.Quit();
    }
}
