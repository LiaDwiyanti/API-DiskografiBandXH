using DiskografiBandXH.Helpers;
using Npgsql;

namespace DiskografiBandXH.Models
{
    public class UserContext
    {
        private string __constr;
        private string __errorMsg;

        public UserContext(string pObs)
        {
            __constr = pObs;
        }

        public List<User> ListUser()
        {
            List<User> listUser = new List<User>();
            string query = string.Format(@"select * from users");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listUser.Add(new User()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Nama = reader["nama"].ToString(),
                        Alamat = reader["alamat"].ToString(),
                        Email = reader["email"].ToString(),
                        Password = reader["password"].ToString(),
                        Role = reader["role"].ToString(),
                    });
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __errorMsg = ex.Message;
            }

            return listUser;
        }

        public User GetPersonByEmail(string email)
        {
            User user = null;
            string query = string.Format(@"select * from users where email = @email");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@email", email);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    user = new User()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Nama = reader["nama"].ToString(),
                        Alamat = reader["alamat"].ToString(),
                        Email = reader["email"].ToString(),
                        Password = reader["password"].ToString(),
                        Role = reader["role"].ToString(),
                    };
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __errorMsg = ex.Message;
            }

            return user;
        }
        public bool UserRegister(User user)
        {
            bool result = false;
            string query = string.Format(@"insert into users(nama, alamat, email, password, role) values (@nama, @alamat, @email, @password, @role)");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", user.Nama);
                cmd.Parameters.AddWithValue("@alamat", user.Alamat);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@role", user.Role);

                int rowsAffected = cmd.ExecuteNonQuery();
                result = rowsAffected > 0;

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __errorMsg = ex.Message;
            }
            return result;
        }
    }
}
