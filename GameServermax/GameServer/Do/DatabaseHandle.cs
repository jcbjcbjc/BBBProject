using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GameServer
{
    class DatabaseHandle
    {
        private string dsn= "server=10.0.224.16;port=3306;user=root;password=; database=gameserver_local;";
        private MySqlConnection conn;
        public DatabaseHandle()
        {
            conn = new MySqlConnection(dsn);
        }

        // userData handle
        public List<UserData> GetUserDataList()
        {
            List<UserData> list = new List<UserData>();

            try
            {
                conn.Open();
            }catch(MySqlException ex)
            {
                Console.Write(ex.Message);
                return null;
            }
            string sql = "select * from user_datas";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                UserData user = new UserData();
                user.Id = reader.GetInt32("id");
                user.IsDeleted = reader.GetInt32("is_deleted")==1;
                user.Name = reader.GetString("name");
                user.Pwd = reader.GetString("pwd");
                user.QQEmail = reader.GetString("qq_email");
                user.Type = reader.GetInt32("type");
                user.Gold = reader.GetInt32("gold");
                user.RegisterTime = reader.GetString("register_time");
                user.EndLoginTime = reader.GetString("end_login_time");
                user.UpdateTime = reader.GetString("update_time");
                list.Add(user);
            }

            conn.Close();
            return list;
        }
        public void SaveUserData(UserData userData)
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return ;
            }

            string sql = "UPDATE  user_datas SET is_deleted = @is_deleted, name = @name, pwd = @pwd , qq_email = @qq_email, type = @type , gold = @gold,register_time = @register_time,end_login_time=@end_login_time,update_time=@update_time WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("is_deleted", userData.IsDeleted ? 1 : 0);
            cmd.Parameters.AddWithValue("name", userData.Name);
            cmd.Parameters.AddWithValue("qq_email ", userData.QQEmail);
            cmd.Parameters.AddWithValue("type ", userData.Type);
            cmd.Parameters.AddWithValue("pwd ", userData.Pwd);
            cmd.Parameters.AddWithValue("gold ", userData.Gold);
            cmd.Parameters.AddWithValue("register_time ", userData.RegisterTime);
            cmd.Parameters.AddWithValue("update_time  ", userData.UpdateTime);
            cmd.Parameters.AddWithValue("end_login_time ", userData.EndLoginTime);
            cmd.Parameters.AddWithValue("id ", userData.Id);
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void InsertUserData(UserData userData)
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return;
            }
            string sql = "INSERT INTO user_datas (is_deleted,`name`,qq_email,type,pwd,gold register_time,update_time,end_login_time) VALUES (@is_deleted,@name,@qq_email,@type,@pwd,@gold ,@register_time,@update_time,@end_login_time)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("is_deleted", userData.IsDeleted ? 1 : 0);
            cmd.Parameters.AddWithValue("name", userData.Name);
            cmd.Parameters.AddWithValue("qq_email ", userData.QQEmail);
            cmd.Parameters.AddWithValue("type ", userData.Type);
            cmd.Parameters.AddWithValue("pwd ", userData.Pwd);
            cmd.Parameters.AddWithValue("gold ", userData.Gold);
            cmd.Parameters.AddWithValue("register_time ", userData.RegisterTime);
            cmd.Parameters.AddWithValue("update_time  ", userData.UpdateTime);
            cmd.Parameters.AddWithValue("end_login_time ", userData.EndLoginTime);
            cmd.Parameters.AddWithValue("id ", userData.Id);
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteUserData(UserData userData)
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return;
            }
            string sql = "DELETE FROM user_datas WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id ", userData.Id);
            int rows = cmd.ExecuteNonQuery();
            conn.Close();

        }


        public List<ResultData> GetResultDataList()
        {
            List<ResultData > list = new List<ResultData>();

            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return null;
            }
            string sql = "select * from result_datas";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var user = new ResultData();
                user.Id = reader.GetInt32("id");
                user.IsDeleted = reader.GetInt32("is_deleted") == 1;
                user.TotalCount = reader.GetInt32("total_count");
                user.UserId = reader.GetInt32("user_id");
                user.WinCount = reader.GetInt32("win_count");
                list.Add(user);
            }
            conn.Close();
            return list;
        }

        public void SaveResaultData(ResultData result)
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return ;
            }
            string sql = "UPDATE result_datas is_deleted=@is_deleted,user_id = @user_id,total_count=@total_count,win_count =@win_count WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("is_deleted", result.IsDeleted ? 1 : 0);cmd.Parameters.AddWithValue("is_deleted", result.IsDeleted ? 1 : 0);
            cmd.Parameters.AddWithValue("is_deleted", result.IsDeleted ? 1 : 0); cmd.Parameters.AddWithValue("total_count", result.TotalCount);
            cmd.Parameters.AddWithValue("is_deleted", result.IsDeleted ? 1 : 0); cmd.Parameters.AddWithValue("win_count", result.WinCount);
            cmd.Parameters.AddWithValue("is_deleted", result.IsDeleted ? 1 : 0); cmd.Parameters.AddWithValue("user_id", result.UserId);
            cmd.Parameters.AddWithValue("is_deleted", result.IsDeleted ? 1 : 0); cmd.Parameters.AddWithValue("id", result.Id);

            var rows = cmd.ExecuteNonQuery();
            conn.Close();
        }
   
        public void InsertResultdata(ResultData result)
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return;
            }

            string sql = "INSERT INTO result_datas (user_id,total_count,win_count,is_deleted) VALUES(@user_id,@total_count,@win_count,@is_deleted)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("user_id", result.UserId);
            cmd.Parameters.AddWithValue("total_count", result.TotalCount);
            cmd.Parameters.AddWithValue("is_deleted", result.IsDeleted ?1 :0);
            cmd.Parameters.AddWithValue("win_count", result.WinCount);

            var rows = cmd.ExecuteNonQuery();
            conn.Close();
        }
    
        public void DeleteResultData(ResultData result)
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return;
            }

            string sql = "DELETE FROM result_datas WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", result.Id);

            var rows = cmd.ExecuteNonQuery();
            conn.Close();

        }
    
        public List<RoomData> GetRoomDataList()
        {
            List<RoomData> list = new List<RoomData>();

            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return null;
            }

            string sql = "select * from room_datas";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var room = new RoomData();
                room.Id = reader.GetInt32("id");
                room.IsDeleted = reader.GetInt32("is_deleted") ==1;
                room.MaxCount = reader.GetInt32("max_count");
                room.RoomId = reader.GetInt32("room_id");
                room.UserId = reader.GetInt32("user_id");
                list.Add(room);
            }
            conn.Close();
            return list;

        }

        public void UpdateRoomData(RoomData room)
        {

            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return ;
            }

            string sql = "UPDATE room_datas SET is_deleted=@is_deleted,max_count = @max_count, room_id=@room_id,user_id =@user_id WHERE id=@id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", room.Id);
            cmd.Parameters.AddWithValue("is_deleted", room.IsDeleted?1:0);
            cmd.Parameters.AddWithValue("max_count", room.MaxCount);
            cmd.Parameters.AddWithValue("room_id", room.RoomId);
            cmd.Parameters.AddWithValue("user_id", room.UserId);
            var rows = cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void InserRoomData(RoomData room)
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return;
            }

            string sql = "INSERT INTO room_datas (room_id,max_count,user_id,is_deleted) VALUES (@room_id,@max_count,@user_id,@is_deleted)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("room_id", room.RoomId);
            cmd.Parameters.AddWithValue("max_count", room.MaxCount);
            cmd.Parameters.AddWithValue("user_id", room.UserId);
            cmd.Parameters.AddWithValue("is_deleted", room.IsDeleted?1:0);
            var rows = cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteRoom(RoomData room)
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return;
            }

            string sql = "DELETE FROM room_datas WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", room.Id);

            var rows = cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void DeleteAllRoom()
        {
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                return;
            }

            string sql = "DELETE FROM room_datas WHERE 1";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            var rows = cmd.ExecuteNonQuery();
            conn.Close();
        }

    }
}
