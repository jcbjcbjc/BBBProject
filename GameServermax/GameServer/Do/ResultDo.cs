using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class ResultDo
    {
        private DatabaseHandle dbhandle;
        public ResultDo()
        {
            dbhandle = new DatabaseHandle();
        }

        public List<ResultData> GetList()
        {
            return dbhandle.GetResultDataList();
        }

		public void Update(int id, string name, string value)
		{
			var list = GetList();

			var item = list.Where(r => r.Id == id).First();
			if (item==null)
			{
				return;
			}
			if (name == "IsDeleted")
			{
				item.IsDeleted = Convert.ToBoolean(value);
			}
			else if (name == "UserId")
			{
				item.UserId = int.Parse(value);
			}
			else if (name == "TotalCount")
			{
				item.TotalCount = int.Parse(value);
			}
			else if (name == "WinCount")
			{
				item.WinCount = int.Parse(value);
			}
				
			dbhandle.SaveResaultData(item);
		}

		public int Add(int userId)
        {
			var result = GetList().Where(r => r.UserId == userId).FirstOrDefault();
			if (result == null)
			{
				Insert(new string[] { "UserId", "TotalCount", "WinCount" },
					new string[] { userId.ToString(), "0", "0" });
				return 1;
			}
			else
			{
				return 0;
			}
		}

		public void Insert(string[] names, string[] values)
		{
			var result = new ResultData();

			result.UserId = int.Parse(values[0]);
			result.TotalCount = int.Parse(values[1]);
			result.WinCount = int.Parse(values[2]);
			result.IsDeleted = false;
			dbhandle.InsertResultdata(result);
		}

		public string GetResultInfo(int userId, string name)
		{
			string value = "";
			var list = GetList();
			if (list == null || list.Count <= 0)
			{
				return value;
			}
			for (int i = 0; i < list.Count; i++)
			{
				var item = list[i];
				if (item.UserId == userId)
				{
					if (name == "Id")
					{
						value = item.Id.ToString();
						return value;
					}
					else if (name == "IsDeleted")
					{
						value = item.IsDeleted.ToString();
						return value;
					}
					else if (name == "TotalCount")
					{
						value = item.TotalCount.ToString();
						return value;
					}
					else if (name == "WinCount")
					{
						value = item.WinCount.ToString();
						return value;

					}
				}
			}
			return value;
		}

		public void SetResultInfo(int userId, string name, string value)
		{
			string result = GetResultInfo(userId, "Id");
			if (result != "")
			{
				Update(int.Parse(result), name, value);
			}
		}

		public void Delete(int id)
		{
			var list = GetList();
			if (list == null || list.Count <= 0)
			{
				return;
			}
			var result = list.Where(u => u.Id == id).FirstOrDefault();
			if (result != null)
			{
				dbhandle.DeleteResultData(result);
			}
		}

	}
}
