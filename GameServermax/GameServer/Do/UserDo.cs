using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class UserDo
    {
        private DatabaseHandle dbhandle;
        public UserDo() 
        {
            dbhandle = new DatabaseHandle();
        }

        public List<UserData> GetList()
        {
            return dbhandle.GetUserDataList();
        }
        public void Update(int id, string name, string value)
        {
            List<UserData> lists = GetList();
            UserData item = lists.Where(u => u.Id == id).First();
            if(item == null)
            {
                return ;
            }
            if (name == "IsDeleted")
            {
                item.IsDeleted = Convert.ToBoolean(value);
            }
            else if (name == "Name")
            {
                item.Name = value;
            }
            else if (name == "Pwd")
            {
                item.Pwd = value;
            }
            else if (name == "QQEmail")
            {
                item.QQEmail = value;
            }
            else if (name == "Type")
            {
                item.Type = int.Parse(value);
            }
            else if (name == "Gold")
            {
                item.Gold = int.Parse(value);
            }
            else if (name == "RegisterTime")
            {
                item.RegisterTime = value;
            }
            else if (name == "EndLoginTime")
            {
                item.EndLoginTime = value;
            }
            else if (name == "UpdateTime")
            {
                item.UpdateTime = value;
            }

            dbhandle.SaveUserData(item);
        }

        public int Login(string name)
        {
            var user = GetList().Where(u => u.Name == name).FirstOrDefault();
            if (user != null)
            {
                return user.Id;
            }
            else
            {
                return 0;
            }
        }

        public string GetUserInfo(int id, string name)
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
                if (item.Id == id)
                {
                    if (name == "IsDeleted")
                    {
                        value = item.IsDeleted.ToString();
                    }
                    else if (name == "Name")
                    {
                        value = item.Name;
                    }
                    else if (name == "Pwd")
                    {
                        value = item.Pwd;
                    }
                    else if (name == "QQEmail")
                    {
                        value = item.QQEmail;
                    }
                    else if (name == "Type")
                    {
                        value = item.Type.ToString();
                    }
                    else if (name == "Gold")
                    {
                        value = item.Gold.ToString();
                    }
                    else if (name == "RegisterTime")
                    {
                        value = item.RegisterTime;
                    }
                    else if (name == "EndLoginTime")
                    {
                        value = item.EndLoginTime;
                    }
                    else if (name == "UpdateTime")
                    {
                        value = item.UpdateTime;
                    }
                }
            }
            return value;
        }

        public void SetUserInfo(int id, string name, string value)
        {
            Update(id, name, value);
        }

        public void Insert(string[] names, string[] values)
        {
            var userData = new UserData();
            userData.Name = values[0];
            userData.Pwd = values[1];
            userData.QQEmail = values[2];
            userData.Type  = int.Parse(values[3]);
            userData.Gold = int.Parse(values[4]);
            userData.RegisterTime = values[5];
            userData.EndLoginTime = values[6];
            userData.UpdateTime = values[7];
            userData.IsDeleted = false;
            dbhandle.InsertUserData(userData);
        }

        public void DeleteUser(int id)
        {
            var list = GetList();
            if (list == null || list.Count <= 0)
            {
                return;
            }
            var user = list.Where(u => u.Id == id).FirstOrDefault();
            if (user != null)
            {
                dbhandle.DeleteUserData(user);
            }
        }

        public int Register(string name, string pwd)
        {
            var user = GetList().Where(u => u.Name == name).FirstOrDefault();
            if (user == null)
            {
                try
                {
                    Insert(
                        new string[] { "name", "pwd", "qqemail", "type", "gold", "registertime", "endlogintime", "updatetime" },
                        new string[] { name, pwd, "", "0", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString() });
                    return 1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            return -2;
        }

    }


}
