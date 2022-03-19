using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class ProConst
    {
        /* 常量定义： UI窗体名称 */
        public const string LOGIN_FROMS = "LoginPanel";
        public const string MENU_UIFORM = "MenuPanel";
        public const string MESSAGE_FROMS = "MessagePanel";
        public const string REGISTER_FORM = "RegisterPanel";
        public const string ROOMLIST_UIFORM = "RoomListPanel";
        public const string ROOM_UIFORM = "RoomPanel";
        public const string START_UIFORM = "StartPanel";
        public const string MULTIPLE_UIFORM = "Multiple_ingame";
        public const string CLASSIC_UIFORM = "Classic_ingame";
        public const string GAMEOVER_UIFORM = "GameOverPanel";

        /* Networking MessageType */

        public const string DEFAULT = "Default";
        public const string LOGIN_MESSAGE = "Login";
        public const string REGISTER_MESSAGE = "Register";
        public const string GETRUSULTINFO_MESSENGE = "GetResultInfo";
        public const string CREATEROOM_MESSAGE = "CreateRoom";
        public const string GETROOMLIST_MESSAGE = "GetRoomList";
        public const string GETROOMINFO_MESSAGE = "GetRoomInfo";
        public const string JOINROOM_MESSAGE = "JoinRoom";
        public const string LEAVEROOM_MESSAGE = "LeaveRoom";
        public const string STARTGAME_MESSAGE = "StartGame";
        public const string OVERGAME_MESSAGE = "OverMessage";
        public const string GAME_CONTROL_MESSAGE = "GameControl";
        public const string GAME_INSTRUCTIONS_MESSAGE = "GameInstructions";
        public const string GAME_PROCESS_START_MESSAGE = "GameProcessStart";
        public const string CONFIRM_START_GAME = "ConfirmStartGame";
        public const string READYFORGAME = "ReadyForGame";

        /* Inside MessageType */

        public const string SHOW_MESSAGE_MESSAGE = "ShowMessage";
        public const string UPDATE_CLASSIC_UI = "UpdataClassic";
        public const string UPDATE_GAMEOVER_UI = "UpdataGameOver";
        public const string INIT_SET_UI = "InitSetUI";
        public const string UPDATE_SET_UI = "UpdateSet";

        /* GameInstructions */

        public const string NEXT_SET_INSTRUCTION = "Next_Set";
        public const string NEXT_BAN_INSTRUCTION = "Next_Ban";
        public const string SET_INSTRUCTION = "Set";
        public const string BAN_INSTRUCTION = "Ban";
        public const string MOVE_INSTRUCTION = "Move";
        public const string BRICK_INSTRUCTION = "Brick";
        public const string BREAK_INSTRUCTION = "Break";
        public const string BREAK_RESULT_MESSAGE = "BreakResult";
        public const string BRICK_FALSE_INSTRUCTION = "BrickFalse";
        public const string MOVE_FAST_INSTRUCTION = "MoveFast";
        public const string CONVEY_INSTRUCTIONS = "ConveyRight";
        public const string INIT_ORDER = "InitOrder";
        public const string CONDITION_HALF = "ConditionHalf";



        /* Const*/
        public const float MOVE_TIME = 0.2f;
        public const int BRICK_COST = 1;
        public const int BREAK_COST = 3;
        public const int MOVE_COST = 1;
    }
   
    public class RoomCode
    {
        public const int WaitJoin = 1;
        public const int WaitBattle = 2;
        public const int Battle = 3;
        public const int GameOver = 4;
    }

}
