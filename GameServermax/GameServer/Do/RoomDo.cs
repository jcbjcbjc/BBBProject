using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class RoomDo
    {
        private DatabaseHandle dbhandle;

        public RoomDo()
        {
            dbhandle = new DatabaseHandle();
        }

		public List<RoomData> GetList()
		{
            return dbhandle.GetRoomDataList();
		}

        public List<RoomData> GetHouseOwnerList()
        {
            var roomList = GetList();
            roomList.Where((x, i) => roomList.FindIndex(n => n.RoomId == x.RoomId) == i).ToList();
            return roomList;
        }

		public void Insert(string[] names, string[] values)
		{
            var room = new RoomData();
            room.RoomId = int.Parse(values[0]);
            room.UserId = int.Parse(values[1]);
            room.MaxCount = int.Parse(values[2]);
            room.IsDeleted = false;
            dbhandle.InserRoomData(room);
		}

        public void Update(int id, string name, string value)
        {
            var list = GetList();
            if (list == null || list.Count <= 0)
            {
                return;
            }
            var item = list.Where(r => r.Id == id).First();
                if (item.Id == id)
                {
                    if (name == "IsDeleted")
                    {
                        item.IsDeleted = Convert.ToBoolean(value);
                    }
                    else if (name == "RoomId")
                    {
                        item.RoomId = int.Parse(value);
                    }
                    else if (name == "UserId")
                    {
                        item.UserId = int.Parse(value);
                    }
                    else if (name == "MaxCount")
                    {
                        item.MaxCount = int.Parse(value);
                    }
                }
            dbhandle.UpdateRoomData(item);
        }

        public List<RoomData> GetRoom(int roomId)
        {
            var list = GetList();
            if(list == null)
            {
                return null;
            }else
            {
                return list.Where(r => r.RoomId == roomId).ToList();

            }
        }
        public RoomData GetHouseOwner(int roomId)
        {
            var list = GetList();
            if (list == null)
            {
                return null;
            }
            else
            {
                return list.Where(r => r.RoomId == roomId && r.UserId == roomId).FirstOrDefault();

            }
        }

        public void CreateRoom(int roomId, int userId, int maxCount)
        {
            var list = GetList();
            RoomData result;
            if (list == null)
            {
                result = null;
            }else
            {
                result = list.Where(r => r.UserId == userId).FirstOrDefault();
            }
            if (result == null)
            {
                Insert(
                    new string[] { "RoomId", "UserId", "MaxCount" },
                    new string[] { roomId.ToString(), userId.ToString(), maxCount.ToString() }
                );
            }
        }
        public void JoinRoom(int roomId, int userId)
        {
            var room = GetRoom(roomId);
            var houseOwner = GetHouseOwner(roomId);
            if (room == null || houseOwner == null)
            {
                Console.WriteLine("房间不存在");
                return;
            }
            if (room.Count < houseOwner.MaxCount)
            {
                CreateRoom(roomId, userId, houseOwner.MaxCount);
            }
        }
        public void LeaveRoom(int roomId, int userId)
        {
            var person = GetList().Where(r => r.RoomId == roomId && r.UserId == userId).FirstOrDefault();
            if (person == null)
            {
                return;
            }
            if (person.RoomId == person.UserId)
            {
                Console.WriteLine("房间[" + roomId + "]房主[" + userId + "]离开了房间");
                Delete(0);
            }
            else
            {
                Console.WriteLine("房间[" + roomId + "]房客[" + userId + "]离开了房间");
                Delete(person.Id);
            }
        }

        public void Delete(int id)
        {
            var list = GetList();
            if (list == null || list.Count <= 0)
            {
                return;
            }
            if (id == 0)
            {
                dbhandle.DeleteAllRoom();
                return;
            }
            var room = list.Where(r => r.Id == id).FirstOrDefault();
            if (room != null)
            {
                dbhandle.DeleteRoom(room);
            }
        }
        public void Ready(int roomId, int userId)
        {
            var person = GetList().Where(r => r.RoomId == roomId && r.UserId == userId).FirstOrDefault();
            if (person == null)
            {
                return;
            }
            Update(person.Id, "IsDeleted", true.ToString());
        }

        public bool StartGame(int roomId, int userId)
        {
            var room = GetRoom(roomId);
            var houseOwner = GetHouseOwner(roomId);
            if (room.Count < houseOwner.MaxCount)
            {
                return false;
            }
            var person = GetList().Where(r => r.RoomId == roomId && r.UserId == userId).FirstOrDefault();
            if (person == null)
            {
                return false;
            }
            if (person.RoomId == person.UserId)
            {
                Console.WriteLine("执行房主开始游戏操作");
                return true;
            }
            return false;
        }
    }
}
