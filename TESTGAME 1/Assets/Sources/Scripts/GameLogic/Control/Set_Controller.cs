using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using MessageHandler;
public class Set_Controller :NetBaseControl
{
    bool[] isplayer_set ;
    bool[] isplayer_ban ;

    Transform[] SetPoint;

    int[] Ban_Point;
    bool[] is_Condition;
    protected override void UI_Init()
    {
        base.UI_Init();
        SendInsideMessage(ProConst.INIT_SET_UI, new Init_Set_UpdateData(MaxCount, MyID));
    }
    protected override void Register() {
        MessageCenter.AddMsgListener(ProConst.SET_INSTRUCTION, OnSetResponse);
        MessageCenter.AddMsgListener(ProConst.BAN_INSTRUCTION, OnBanResponse);
        MessageCenter.AddMsgListener(ProConst.MOVE_INSTRUCTION, OnMoveResponse);
        MessageCenter.AddMsgListener(ProConst.BRICK_INSTRUCTION, OnBrickResponse);
        MessageCenter.AddMsgListener(ProConst.BREAK_INSTRUCTION, OnBreakResponse);
        MessageCenter.AddMsgListener(ProConst.BREAK_RESULT_MESSAGE, OnBreakResultResponse);
        MessageCenter.AddMsgListener(ProConst.CONVEY_INSTRUCTIONS, OnConveyResponse);
        MessageCenter.AddMsgListener(ProConst.GAME_PROCESS_START_MESSAGE, OnStartGameProcessResponse);
        MessageCenter.AddMsgListener(ProConst.CONDITION_HALF,OnHlafCondition );
        MessageCenter.AddMsgListener(ProConst.OVERGAME_MESSAGE,Gameover);
        MessageCenter.AddMsgListener(ProConst.NEXT_SET_INSTRUCTION,OnNextSetResponse);
        MessageCenter.AddMsgListener(ProConst.NEXT_BAN_INSTRUCTION,OnNextBanResponse);
    }
    // Start is called before the first frame update
    protected override void InitGame()
    {
        base.InitGame();

        Next_ID = MyID % MaxCount + 1;

        SetPoint =new Transform[MaxCount+1];

        Ban_Point=new int[MaxCount+1];
        for (int i = 1; i <= MaxCount; i++) {
            Ban_Point[i] = 3;
        }

        is_Condition =new bool[MaxCount+1];

        isplayer_set=new bool[MaxCount+1];
        
        isplayer_ban=new bool[MaxCount+1];
    }
    protected override void SendSelectOrderMessenge()
    {
        if (MyID == 1)
        {
            order = Random.Range(1, MaxCount+1);

            SendNetMessege(ProConst.NEXT_SET_INSTRUCTION, new MessageData(order.ToString()));

            Current_id = MyID;

            if (order == MyID) {
                isPlayer = true;
                isplayer_set[MyID] = true;
            }
        }
    }
    protected override void Convert()
    {
        isPlayer = false;
        int Next_MovePoint = gadget.GenerateRamdomNumber();
        SendNetMessege(ProConst.CONVEY_INSTRUCTIONS, new MessageDatas(Next_ID.ToString(), Next_MovePoint.ToString()));

        Current_id = Next_ID;

        MovementPoint[Next_ID] = Next_MovePoint;


    }
    protected override void UI_Update()
    {
        SendInsideMessage(ProConst.UPDATE_SET_UI,new UI_Set_UpdateData(SetUICode.Updata.ToString(), MaxCount, Current_id, isplayer_set[Current_id], isplayer_ban[Current_id],MovementPoint[Current_id]) );
    }


    protected override void Update()
    {
        Process();
    }
    void Process() {
        if (_is_game&&isPlayer)
        {
            Set_Player();
            Ban_Player();
            Move_Player();
            Brick_Player();
            Break_Player();
            Condition_Jugde();
        }
        if (_is_game) {
            UI_Update();
        }
    }
    public void Set_Player()
    {
        if (isplayer_set[MyID])
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out RaycastHit _hit))
                {
                    if (IsSet(_Player.transform, _hit.collider.gameObject.transform)) {
                        SendNetMessege(ProConst.SET_INSTRUCTION, new Game_PositionData(MyID, _hit.collider.gameObject.GetComponent<_platform>().row, _hit.collider.gameObject.GetComponent<_platform>().col, 0));

                        Set_Player_to(MyID,_hit.collider.gameObject);
                        isPlayer = false;
                        isplayer_set[MyID] =false;

                        if (order !=Next_ID)
                        {
                            
                            SendNetMessege(ProConst.NEXT_SET_INSTRUCTION, new MessageData(Next_ID.ToString()));

                            Current_id = Next_ID;

                            isplayer_set[Next_ID] = true;
                        }
                        else
                        {
                            
                            SendNetMessege(ProConst.NEXT_BAN_INSTRUCTION, new MessageData(Next_ID.ToString()));

                            Current_id = Next_ID;

                            isplayer_ban[Next_ID]=true;
                        }
                    }
               
                }
            }
        }
    }
    public void Ban_Player()
    {
        if (isplayer_ban[MyID])
        {
            if (Ban_Point[MyID] > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out RaycastHit _hit))
                    {
                        if (IsBan(_Player.transform, _hit.collider.gameObject.transform)) {
                            SendNetMessege(ProConst.BAN_INSTRUCTION, new Game_PositionData(MyID, _hit.collider.gameObject.GetComponent<_platform>().row, _hit.collider.gameObject.GetComponent<_platform>().col, 0));

                            Ban_Player_to(MyID,_hit.collider.gameObject);
                        }
           
                    }
                }
            }
            else
            {
                isPlayer=false;
                isplayer_ban[MyID] = false;

                if (order != Next_ID)
                {
                    SendNetMessege(ProConst.NEXT_BAN_INSTRUCTION, new MessageData(Next_ID.ToString()));

                    Current_id = Next_ID;

                    isplayer_ban[Next_ID] = true;
                }
                else
                {
                    Convert();
                }
            }
        }
    }
    void Set_Player_to(int ID, GameObject _go) {
        SetStatusSet(ID, _go.transform);

    }
    void Ban_Player_to(int ID, GameObject _go) {
        SetStatusBan(_go.transform);

        Ban_Point[ID]--;
    }
    protected override void Condition_Jugde()
    {
        if (_Player.transform.position == SetPoint[MyID].transform.position)
        {
            SendNetMessege(ProConst.CONDITION_HALF, new MessageData(MyID.ToString()));

       
            is_Condition[MyID] = true;
        }
        if (is_Condition[MyID] && _Player.transform.position == EndPoint[MyID].position)
        {
            SendNetMessege(ProConst.OVERGAME_MESSAGE, new OverGameData(MyID,Game.Instance.User.Name));

            Gameover();

        }
    }
    public override bool Navigation_Obstacle(Transform _go)
    {
        SetStatusBrick(_go);
        for (int i = 1; i <= MaxCount; i++)
        {
            if (!_navigation.IsNavigation(gadget.SwitchToCoord(_Players[i].transform), _PlayerData[i].EndPoint))
            {
                DeleteStatus(_go);
                return false;
            }
        }
        DeleteStatus(_go);
        return true;
    }
    void OnNextSetResponse(KeyValuesUpdate kv) {
        MessageData str = kv.Values as MessageData;
       
        Current_id = int.Parse(str._data);
        if (Current_id == MyID)
        {
            isPlayer = true;
            isplayer_set[MyID]=true;
        }
    }
    void OnNextBanResponse(KeyValuesUpdate kv) {
        MessageData str = kv.Values as MessageData;

        Current_id = int.Parse(str._data);
        if (Current_id == MyID)
        {
            isPlayer = true;
            isplayer_ban[MyID] = true;
        }
    }
    void OnStartGameProcessResponse(KeyValuesUpdate kv)
    {
        Debug.Log("StartGameMainProcess");
        _is_game = true;
        SendSelectOrderMessenge();
    }
    void OnConveyResponse(KeyValuesUpdate kv)
    {
        MessageDatas str = kv.Values as MessageDatas;
        Current_id = int.Parse(str.param1);
        MovementPoint[Current_id] = int.Parse(str.param2);
        if (Current_id == MyID) {
            isPlayer = true;
        }
    }
    void OnSetResponse(KeyValuesUpdate kv)
    {
        Game_PositionData str = kv.Values as Game_PositionData;
       
        int id = str._id;
        int row = str._row;
        int col =str._col;
         Set_Player_to(id, _mapEditor.GetMap(row, col));
    }
    void OnBanResponse(KeyValuesUpdate kv)
    {
        Game_PositionData str = kv.Values as Game_PositionData;

        int id = str._id;
        int row = str._row;
        int col = str._col;
        Ban_Player_to(id, _mapEditor.GetMap(row, col));
    }

    void OnHlafCondition(KeyValuesUpdate kv) {
       MessageData str = kv.Values as MessageData;
       int id = int.Parse(str._data);
       is_Condition[id]=true;
    }
    void SetStatusSet(int ID,Transform _go) {

        //_go.GetComponent<_platform>()._text[1].gameObject.SetActive(true);
        //_go.GetComponent<_platform>()._text[1].text = "M";


        _go.GetComponent<_platform>()._sprt.color = Color.green;


        _go.GetComponent<_platform>().status = MapCode.Set;


        SetPoint[ID%MaxCount+1] = _go;
    }
    bool IsSet(Transform obj, Transform arrive)
    {
        if (arrive.GetComponent<_platform>().status != MapCode.Default)
        {
            return false;
        }
        return true;
    }
    bool IsBan(Transform obj, Transform arrive)
    {
        if (!Navigation_Obstacle(arrive))
        {
            return false;
        }
        if (arrive.GetComponent<_platform>().status != MapCode.Default)
        {
            return false;
        }
        return true;
    }

    //----------------------------------------------
    protected override void OnDestroy()
    {
        Camera.main.transform.position = new Vector3(21, -157, -881);
        _mapEditor.DestroyMap();

        for (int i = 0; i <= MaxCount; i++) {
            Destroy(_Players[i]);
        }
        Destroy(_Player);
    }
    void Gameover() {
        //ID and UserName
        SendInsideMessage(ProConst.UPDATE_GAMEOVER_UI, new OverGameData(MyID, Game.Instance.User.Name));

        UIManager.GetInstance().ShowUIForms(ProConst.GAMEOVER_UIFORM);
        UIManager.GetInstance().CloseUIForms(ProConst.MULTIPLE_UIFORM);

        Destroy(this);
    }
    protected override void Gameover(KeyValuesUpdate kv)
    {
       OverGameData str = kv.Values as OverGameData;
        

        UIManager.GetInstance().ShowUIForms(ProConst.GAMEOVER_UIFORM);
        UIManager.GetInstance().CloseUIForms(ProConst.MULTIPLE_UIFORM);

        SendInsideMessage(ProConst.UPDATE_GAMEOVER_UI, new OverGameData(str.winnerid, str.winnerusername));

        Destroy(this);
    }
}
