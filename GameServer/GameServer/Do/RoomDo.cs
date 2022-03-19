using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameServer
{
	public class RoomDo
	{
		public string Name { get; private set; }
		public string Path { get; private set; }

		public RoomDo()
		{
			Name = "RoomData";
			Path = "C:\\Users\\lenovo\\Desktop\\BBBServer\\RoomData.txt";
		}

		public string Read()
		{
			FileStream fs = new FileStream (Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			fs.Close ();
			FileInfo file = new FileInfo (Path);
			var reader = file.OpenText ();
			string listJson = reader.ReadToEnd ();
			reader.Close ();
			return listJson;
		}

		public void Save(string listJson)
		{
			FileStream fs = new FileStream (Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			fs.Close ();
			FileInfo file = new FileInfo (Path);
			var writer = file.CreateText ();
			writer.WriteLine (listJson);
			writer.Close ();
		}

		public void Insert(string [] names, string [] values)
		{
			string listJson = Read ();
			if (listJson == "") {
				listJson += "RoomData";
				Save (listJson);
			} 
			else 
			{
				listJson += "&Id=" + (GetList ().Count + 1) + ",";
				listJson += "IsDeleted=" + false + ",";
				listJson += names [0] + "=" + values [0] + ",";
				listJson += names [1] + "=" + values [1] + ",";
				listJson += names [2] + "=" + values [2];
				Save (listJson);
			}	
		}

		public void Delete(int id)
		{
			var list = GetList ();
			if (list == null || list.Count <= 0) {
				return;
			}
			if (id == 0) {
				list.Clear ();
				string listJson = ToListJson (list);
				Save (listJson);
				return;
			}
			var room = list.Where (r => r.Id == id).FirstOrDefault ();
			if (room != null) {
				list.Remove (room);
				string listJson = ToListJson (list);
				Save (listJson);
			}
		}

		public void Update(int id, string name, string value)
		{
			var list = GetList ();
			if (list == null || list.Count <= 0) {
				return;
			}
			for (int i = 0; i < list.Count; i++) {
				var item = list [i];
				if (item.Id == id) {
					if (name == "IsDeleted") {
						item.IsDeleted = Convert.ToBoolean (value);
					}
					else if (name == "RoomId") {
						item.RoomId = int.Parse (value);
					} 
					else if (name == "UserId") {
						item.UserId = int.Parse (value);
					} 
					else if (name == "MaxCount") {
						item.MaxCount = int.Parse (value);
					} 
				}
			}
			string listJson = ToListJson (list);
			Save (listJson);
		}

		public string ToListJson(List<RoomData> list)
		{
			string listJson = "RoomData";
			if (list == null || list.Count <= 0) {
				return listJson;
			}
			for (int i = 0; i < list.Count; i++) {
				listJson += "&Id=" + list[i].Id + ",";
				listJson += "IsDeleted=" + list[i].IsDeleted + ",";
				listJson += "RoomId=" + list[i].RoomId + ",";
				listJson += "UserId=" + list [i].UserId + ",";
				listJson += "MaxCount=" + list[i].MaxCount;
			}
			return listJson;
		}

		public List<RoomData> GetList()
		{
			List<RoomData> list = new List<RoomData> ();
			string listJson = Read ();
			if (listJson == "") {
				return list;
			}
			if (!listJson.Contains("&")) {
				Console.WriteLine (listJson);
			}
			else 
			{
				var jsons = listJson.Split ('&');
				for (int i = 1; i < jsons.Length; i++) {
					var attrs = jsons [i].Split(',');
					RoomData room = new RoomData ();
					room.Id = int.Parse (attrs [0].Split ('=') [1]);
					room.IsDeleted = Convert.ToBoolean (attrs [1].Split ('=') [1]);
					room.RoomId = int.Parse (attrs [2].Split ('=') [1]);
					room.UserId = int.Parse (attrs [3].Split ('=') [1]);
					room.MaxCount = int.Parse (attrs [4].Split ('=') [1]);
					list.Add (room);
				}
			}
			return list;
		}

		public List<RoomData> GetHouseOwnerList()
		{
			var roomList = GetList ();
			roomList.Where ((x, i) => roomList.FindIndex (n => n.RoomId == x.RoomId) == i).ToList ();
			return roomList;
		}

		public void CreateRoom(int roomId, int userId, int maxCount)
		{
			var result = GetList ().Where (r => r.UserId == userId).FirstOrDefault ();
			if (result == null) {
				Insert (
					new string[]{ "RoomId", "UserId", "MaxCount" }, 
					new string[]{ roomId.ToString (), userId.ToString (), maxCount.ToString () }
				);
			}
		}

		public List<RoomData> GetRoom(int roomId)
		{
			return GetList ().Where (r => r.RoomId == roomId).ToList ();
		}

		public RoomData GetHouseOwner(int roomId)
		{
			return GetList ().Where (r => r.RoomId == roomId && r.UserId == roomId).FirstOrDefault ();
		}

		public void JoinRoom(int roomId, int userId)
		{
			var room = GetRoom (roomId);
			var houseOwner = GetHouseOwner (roomId);
			if (room == null || houseOwner == null) {
				Console.WriteLine ("房间不存在");
				return;
			}
			if (room.Count < houseOwner.MaxCount) {
				CreateRoom (roomId, userId, houseOwner.MaxCount);
			}
		}

		public void LeaveRoom(int roomId, int userId)
		{
			var person = GetList ().Where (r => r.RoomId == roomId && r.UserId == userId).FirstOrDefault ();
			if (person == null) {
				return;
			}
			if (person.RoomId == person.UserId) {
				Console.WriteLine ("房间[" + roomId + "]房主[" + userId + "]离开了房间");
				Delete (0);
			} 
			else {
				Console.WriteLine ("房间[" + roomId + "]房客[" + userId + "]离开了房间");
				Delete (person.Id);
			}
		}

		public void Ready(int roomId, int userId)
		{
			var person = GetList ().Where (r => r.RoomId == roomId && r.UserId == userId).FirstOrDefault ();
			if (person == null) {
				return;
			}
			Update (person.Id, "IsDeleted", true.ToString ());
		}

		public bool StartGame(int roomId, int userId)
		{
			var room = GetRoom(roomId);
			var houseOwner = GetHouseOwner(roomId);
			if (room.Count < houseOwner.MaxCount)
			{
				return false;
			}
			var person = GetList ().Where (r => r.RoomId == roomId && r.UserId == userId).FirstOrDefault ();
			if (person == null) {
				return false;
			}
			if (person.RoomId == person.UserId) {
				Console.WriteLine("执行房主开始游戏操作");
				return true;
			}
			return false;
		}
	}
}

