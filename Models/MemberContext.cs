using DiskografiBandXH.Helpers;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace DiskografiBandXH.Models
{
    public class MemberContext : Controller
    {
        private string __constr;
        public string __ErrorMsg;
        public MemberContext(string pConstr)
        {
            __constr = pConstr;
        }
        public List<Member> MemberList()
        {
            List<Member> memberList = new List<Member>();
            string query = string.Format(@"Select * from members");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    memberList.Add(new Member()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Nama = reader["nama"].ToString(),
                        Inst = reader["inst"].ToString(),
                        Asal = reader["asal_kota"].ToString(),
                        Tinggi = float.Parse(reader["tinggi"].ToString()),
                    });
                }
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
            return memberList;
        }

        public Member GetMemberByName(string name)
        {
            Member member = null;
            string query = string.Format(@"select * from members where nama = @nama");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", name);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    member = new Member()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Nama = reader["nama"].ToString(),
                        Inst = reader["inst"].ToString(),
                        Asal = reader["asal_kota"].ToString(),
                        Tinggi = float.Parse(reader["tinggi"].ToString())
                    };
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }

            return member;
        }

        public bool addMember(Member member)
        {
            bool result = false;
            string query = string.Format(@"insert into members (nama, inst, asal_kota, tinggi) 
                                            values (@nama, @inst, @asal_kota, @tinggi)");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@nama", member.Nama);
                    cmd.Parameters.AddWithValue("@inst", member.Inst);
                    cmd.Parameters.AddWithValue("@asal_kota", member.Asal);
                    cmd.Parameters.AddWithValue("@tinggi", member.Tinggi);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    result = rowsAffected > 0;

                    cmd.Dispose();
                    db.closeConnection();
                }
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
            return result;
        }

        public bool updateMember(int memberId, Member member)
        {
            bool result = false;
            string query = string.Format(@"update members set nama=@nama, inst=@inst, asal_kota=@asal_kota, tinggi=@tinggi where id=@id");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@id", memberId);
                    cmd.Parameters.AddWithValue("@nama", member.Nama);
                    cmd.Parameters.AddWithValue("@inst", member.Inst);
                    cmd.Parameters.AddWithValue("@asal_kota", member.Asal);
                    cmd.Parameters.AddWithValue("@tinggi", member.Tinggi);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    result = rowsAffected > 0;

                    cmd.Dispose();
                    db.closeConnection();
                }
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
            return result;
        }

        public bool delMember(int memberId)
        {
            bool result = false;
            string query = string.Format(@"delete from members where id=@id");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@id", memberId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    result = rowsAffected > 0;

                    cmd.Dispose();
                    db.closeConnection();
                }
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
            return result;
        }
    }
}
