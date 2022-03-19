using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Game_BaseDataDo
    {
        List<Game_BaseDataFromServer> GetGame_BaseDataFromServer(int GamePattern,int userid) {
            List < Game_BaseDataFromServer > game_BaseDataFromServers= new List <Game_BaseDataFromServer>();
            if (GamePattern == PatternCode.Multiple)
            {

            }
            else if (GamePattern == PatternCode.quartic)
            {

            }
            else if (GamePattern == PatternCode.hexgon)
            {

            }
            else if (GamePattern == PatternCode.Setting) {
                GetGame_BaseDataFromServerFromDataBase(userid);
            }
            return game_BaseDataFromServers;
        }
        List<Game_BaseDataFromServer> GetGame_BaseDataFromServerFromDataBase(int userid) {
            return;



        }
        List<Game_BaseDataFromServer> GetGame_BaseDataFromServerMultiple()
        {
            List<Game_BaseDataFromServer> game_BaseDataFromServers=new List<Game_BaseDataFromServer>();
            game_BaseDataFromServers.Add(new Game_BaseDataFromServer(0,));



        }
    }
}
