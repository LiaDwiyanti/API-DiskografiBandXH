using DiskografiBandXH.Helpers;
using DiskografiBandXH.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiskografiBandXH.Controllers
{
    public class AuthController : Controller
    {
        private readonly string __constr;
        private readonly IConfiguration _configuration;
        public IActionResult Index()
        {
            return View();
        }
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            __constr = configuration.GetConnectionString("ApiDatabase");
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User registerData)
        {
            if (string.IsNullOrEmpty(registerData.Email) || string.IsNullOrEmpty(registerData.Password))
            {
                return BadRequest(new { Message = "Data Email dan Password Wajib Diisi"});
            }

            UserContext context = new UserContext(__constr);

            User exitingUser = context.GetPersonByEmail(registerData.Email);
            if(exitingUser != null)
            {
                return BadRequest(new { Message = "Akun email telah terdaftar" });
            }

            string selectedRole = registerData.Role?.ToLower();
            if(selectedRole != "user" && selectedRole != "admin")
            {
                return BadRequest(new { Message = "Mohon pilih role User atau Admin" });
            }

            registerData.Role = selectedRole == "admin" ? "Admin" : "User";
            bool isResistered = context.UserRegister(registerData);
            if (isResistered)
            {
                return Ok(new { Message = "Registrasi Berhasil" });
            }

            else
            {
                return StatusCode(500, new { Message = "Registrasi tidak berhasil" });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login loginData)
        {
            UserContext context = new UserContext(__constr);
            User user = context.GetPersonByEmail(loginData.Email);

            if(user == null || user.Password != loginData.Password)
            {
                return Unauthorized(new { Message = "Password atau Email tidak cocok" });
            }

            JWTHelper jwtHelper = new JWTHelper(_configuration);
            var token = jwtHelper.GenerateToken(user);

            return Ok(new
            {
                token = token,
                user = new
                {
                    id = user.Id,
                    nama = user.Nama,
                    email = user.Email,
                    role = user.Role
                }
            });
        }
    }
}
