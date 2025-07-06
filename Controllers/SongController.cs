using DiskografiBandXH.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiskografiBandXH.Controllers
{
    public class SongController : Controller
    {
        private string __constr;
        private readonly IConfiguration _configuration;

        public SongController(IConfiguration configuration)
        {
            _configuration = configuration;
            __constr = configuration.GetConnectionString("ApiDatabase");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("api/song"), Authorize(Roles = "User, Admin")]
        public ActionResult<Song> SongList()
        {
            try
            {
                SongContext context = new SongContext(this.__constr);
                List<Song> listSong = context.SongList();

                if(listSong == null || listSong.Count == 0)
                {
                    return NotFound("Belum ada data lagu yang ditambahkan");
                }
                return Ok(listSong);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = "Terdapat Kesalahan", error = ex.Message });
            }
        }

        [HttpPost("api/song/add"), Authorize(Roles = "Admin")]
        public IActionResult CreateSong([FromBody] Song song)
        {
            try
            {
                if(string.IsNullOrEmpty(song.Title) || string.IsNullOrEmpty(song.Album))
                {
                    return BadRequest(new { Message = "Semua data wajib diisi" });
                }

                SongContext context = new SongContext(this.__constr);
                Song exitingSong = context.GetSongByTitle(song.Title);
                if (exitingSong != null)
                {
                    return BadRequest(new { Message = "Lagu telah ditambahkan di sesi sebelumnya"});
                }

                bool isAdded = context.addSong(song);
                if (isAdded)
                {
                    return Ok(new { message = "Lagu berhasil ditambahkan" });
                }
                else
                {
                    return StatusCode(500, new { Message = "Lagu tidak berhasil ditambahkan" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = "Terdapat Kesalahan", error = ex.Message });
            }
        }

        [HttpPut("api/song/update/{Id}"), Authorize(Roles = "Admin")]
        public IActionResult UpdateSong(int Id, [FromBody] Song song)
        {
            try
            {
                if (string.IsNullOrEmpty(song.Title) || string.IsNullOrEmpty(song.Album))
                {
                    return BadRequest(new { Message = "Semua data wajib diisi" });
                }

                SongContext context = new SongContext(this.__constr);
                
                bool isUpdated = context.updateSong(Id, song);
                if(isUpdated)
                {
                    return Ok(new { message = "Data Lagu berhasil diperbarui!" });
                }
                else
                {
                    return StatusCode(500, new { Message = "Data Lagu tidak berhasil diperbarui" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = "Terdapat Kesalahan", error = ex.Message });
            }
        }

        [HttpDelete("api/song/delete/{Id}"), Authorize(Roles = "Admin")]
        public IActionResult DeleteSong(int Id)
        {
            try
            {
                SongContext context = new SongContext(this.__constr);
                bool isDeleted = context.delSong(Id);
                if(isDeleted)
                {
                    return Ok(new { message = "Lagu berhasil dihapus dari daftar" });
                }
                else
                {
                    return StatusCode(500, new { Message = "Lagu tidak berhasil dihapus" });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = "Terdapat Kesalahan", error = ex.Message });
            }
        }
    }
}
