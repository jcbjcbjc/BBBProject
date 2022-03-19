using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameServer
{
	public class ResultDo
	{
		public string Name { get; private set; }
		public string Path { get; private set; }

		public ResultDo()
		{
			Name = "ResultData";
			Path = "C:\\Users\\lenovo\\Desktop\\BBBServer\\ResultData.txt";
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
				listJson += "ResultData";
				Save (listJson);
				/*
				listJson += "Id=1,";
				listJson += "IsDeleted=" + false + ",";
				listJson += names [0] + "=" + values [0] + ",";
				listJson += names [1] + "=" + values [1] + ",";
				listJson += names [2] + "=" + values [2] + ",";
				listJson += names [3] + "=" + values [3] + ",";
				listJson += names [4] + "=" + values [4] + ",";
				listJson += names [5] + "=" + values [5] + ",";
				listJson += names [6] + "=" + values [6] + ",";
				listJson += names [7] + "=" + values [7];
				DataTool.Save (listJson);
				*/
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
			var result = list.Where (u => u.Id == id).FirstOrDefault ();
			if (result != null) {
				list.Remove (result);
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
					} else if (name == "UserId") {
						item.UserId = int.Parse (value);
					} else if (name == "TotalCount") {
						item.TotalCount = int.Parse (value);
					} else if (name == "WinCount") {
						item.WinCount = int.Parse (value);
					}
				}
			}
			string listJson = ToListJson (list);
			Save (listJson);
		}

		public string ToListJson(List<ResultData> list)
		{
			string listJson = "ResultData";
			if (list == null || list.Count <= 0) {
				return listJson;
			}
			for (int i = 0; i < list.Count; i++) {
				listJson += "&Id=" + list[i].Id + ",";
				listJson += "IsDeleted=" + list[i].IsDeleted + ",";
				listJson += "UserId=" + list[i].UserId + ",";
				listJson += "TotalCount=" + list[i].TotalCount + ",";
				listJson += "WinCount=" + list[i].WinCount;
			}
			return listJson;
		}

		public List<ResultData> GetList()
		{
			List<ResultData> list = new List<ResultData> ();
			string listJson = Read ();
			if (listJson == "") {
				return list;
			}
			if (!listJson.Contains("&")) {
				Console.WriteLine (listJson);
				/*
				var attrs = listJson.Split (',');
				UserData user = new UserData ();
				user.Id = int.Parse (attrs [0].Split ('=') [1]);
				user.IsDeleted = Convert.ToBoolean (attrs [1].Split ('=') [1]);
				user.Name = attrs [2].Split ('=') [1];
				user.Pwd = attrs [3].Split ('=') [1];
				user.QQEmail = attrs [4].Split ('=') [1];
				user.Type = int.Parse (attrs [5].Split ('=') [1]);
				user.Gold = int.Parse (attrs [6].Split ('=') [1]);
				user.RegisterTime = attrs [7].Split ('=') [1];
				user.EndLoginTime = attrs [8].Split ('=') [1];
				user.UpdateTime = attrs [9].Split ('=') [1];
				list.Add (user);
				*/
			}
			else 
			{
				var jsons = listJson.Split ('&');
				for (int i = 1; i < jsons.Length; i++) {
					var attrs = jsons [i].Split(',');
					ResultData result = new ResultData ();
					result.Id = int.Parse (attrs [0].Split ('=') [1]);
					result.IsDeleted = Convert.ToBoolean (attrs [1].Split ('=') [1]);
					result.UserId = int.Parse (attrs [2].Split ('=') [1]);
					result.TotalCount = int.Parse (attrs [3].Split ('=') [1]);
					result.WinCount = int.Parse (attrs [4].Split ('=') [1]);
					list.Add (result);
				}
			}
			return list;
		}

		public int Add(int userId)
		{
			var result = GetList ().Where (r => r.UserId == userId).FirstOrDefault ();
			if (result == null) {
				Insert (new string[]{ "UserId", "TotalCount", "WinCount" }, 
					new string[]{ userId.ToString (), "0", "0" });
				return 1;
			} else {
				return 0;
			}
		}

		public string GetResultInfo(int userId, string name)
		{
			string value = "";
			var list = GetList ();
			if (list == null || list.Count <= 0) {
				return value;
			}
			for (int i = 0; i < list.Count; i++) {
				var item = list [i];
				if (item.UserId == userId) {
					if (name == "Id") {
						value = item.Id.ToString ();
						return value;
					}
					else if (name == "IsDeleted") {
						value = item.IsDeleted.ToString ();
						return value;
					}
					else if (name == "TotalCount") {
						value = item.TotalCount.ToString ();
						return value;
					}
					else if (name == "WinCount") {
						value = item.WinCount.ToString ();
						return value;
					}
				}
			}
			return value;
		}

		public void SetResultInfo(int userId, string name, string value)
		{
			string result = GetResultInfo (userId, "Id");
			if (result != "") {
				Update (int.Parse (result), name, value);
			}
		}
	}
}

