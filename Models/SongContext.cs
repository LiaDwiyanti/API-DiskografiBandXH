using DiskografiBandXH.Helpers;
using Npgsql;

namespace DiskografiBandXH.Models
{
    public class SongContext
    {
        private string __constr;
        private string __ErrorMsg;
        public SongContext(string pConstr)
        {
            __constr = pConstr;
        }
        public List<Song> SongList()
        {
            List<Song> songList = new List<Song>();
            string query = string.Format(@"Select * from songs");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    songList.Add(new Song()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Title = reader["title"].ToString(),
                        Album = reader["album"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }
            return songList;
        }

        public Song GetSongByTitle(string title)
        {
            Song song = null;
            string query = string.Format(@"select * from songs where title = @title");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@title", title);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    song = new Song()
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Title = reader["title"].ToString(),
                        Album = reader["album"].ToString(),
                    };
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message;
            }

            return song;
        }

        public bool addSong(Song song)
        {
            bool result = false;
            string query = string.Format(@"insert into songs (title, album) values
                                            (@title, @album)");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@title", song.Title);
                    cmd.Parameters.AddWithValue("@album", song.Album);

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

        public bool updateSong(int songId, Song song)
        {
            bool result = false;
            string query = string.Format(@"update songs set title=@title, album=@album 
                                            where id=@id");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@id", songId);
                    cmd.Parameters.AddWithValue("@title", song.Title);
                    cmd.Parameters.AddWithValue("@album", song.Album);

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

        public bool delSong(int songId)
        {
            bool result = false;
            string query = string.Format(@"delete from songs where id=@id");
            DBHelper db = new DBHelper(this.__constr);
            try
            {
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@id", songId);

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
