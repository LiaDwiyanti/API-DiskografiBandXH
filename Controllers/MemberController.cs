using DiskografiBandXH.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiskografiBandXH.Controllers
{
    public class MemberController : Controller
    {
        private string __constr;
        private readonly IConfiguration _configuration;

        public MemberController(IConfiguration configuration)
        {
            _configuration = configuration;
            __constr = configuration.GetConnectionString("ApiDatabase");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("api/member"), Authorize(Roles = "User, Admin")]
        public ActionResult<Member> ReadMember()
        {
            try
            {
                MemberContext context = new MemberContext(this.__constr);
                List<Member> listMember = context.MemberList();

                if (listMember == null || listMember.Count == 0)
                {
                    return NotFound(new { Message = "Belum ada data member Xdinary Heroes yang ditambahkan" });
                }
                return Ok(listMember);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Terdapat Kesalahan", error = ex.Message });
            }
        }

        [HttpPost("api/member/add"), Authorize(Roles = "Admin")]
        public IActionResult CreateSong([FromBody] Member member)
        {
            try
            {
                if (string.IsNullOrEmpty(member.Nama) || string.IsNullOrEmpty(member.Inst))
                {
                    return BadRequest(new { Message = "Semua data wajib diisi" });
                }

                MemberContext context = new MemberContext(this.__constr);
                Member exitingMember = context.GetMemberByName(member.Nama);
                if (exitingMember != null)
                {
                    return BadRequest(new { Message = "Data Member Xdinary Heroes telah ditambahkan di sesi sebelumnya" });
                }

                bool isAdded = context.addMember(member);
                if (isAdded)
                {
                    return Ok(new { message = "Data Member Xdinary Heroes berhasil ditambahkan" });
                }
                else
                {
                    return StatusCode(500, new { Message = "Data Member Xdinary Heroes tidak berhasil ditambahkan" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Terdapat Kesalahan", error = ex.Message });
            }
        }

        [HttpPut("api/member/update/{Id}"), Authorize(Roles = "Admin")]
        public IActionResult UpdateMember(int Id, [FromBody] Member member)
        {
            try
            {
                if (string.IsNullOrEmpty(member.Nama) || string.IsNullOrEmpty(member.Inst))
                {
                    return BadRequest(new { Message = "Semua data wajib diisi" });
                }

                MemberContext context = new MemberContext(this.__constr);

                bool isUpdated = context.updateMember(Id, member);
                if (isUpdated)
                {
                    return Ok(new { message = "Data Member Xdinary Heroes berhasil diperbarui!" });
                }
                else
                {
                    return StatusCode(500, new { Message = "Data Member Xdinary Heroes tidak berhasil diperbarui" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Terdapat Kesalahan", error = ex.Message });
            }
        }

        [HttpDelete("api/member/delete/{Id}"), Authorize(Roles = "Admin")]
        public IActionResult DeleteMember(int Id)
        {
            try
            {
                MemberContext context = new MemberContext(this.__constr);
                bool isDeleted = context.delMember(Id);
                if (isDeleted)
                {
                    return Ok(new { message = "Data Member Xdinary Heroes berhasil dihapus dari daftar" });
                }
                else
                {
                    return StatusCode(500, new { Message = "Data Member Xdinary Heroes tidak berhasil dihapus" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Terdapat Kesalahan", error = ex.Message });
            }
        }
    }
}
