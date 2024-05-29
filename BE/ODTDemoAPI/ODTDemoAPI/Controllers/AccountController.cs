using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTDemoAPI.Entities;
using ODTDemoAPI.OperationModel;
using BCrypt;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.AuthOperation;
using System.IdentityModel.Tokens.Jwt;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;
        private readonly IAuthService _authService;
        public AccountController(OnDemandTutorContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("tutor-register")]
        public async Task<IActionResult> RegisterTutor([FromBody] RegisterTutorModel registerTutorModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (FindTutorByEmail(registerTutorModel.Email) == null)
                {
                    var account = new Account
                    {
                        FirstName = registerTutorModel.FirstName,
                        LastName = registerTutorModel.LastName,
                        Email = registerTutorModel.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(registerTutorModel.Password, BCrypt.Net.BCrypt.GenerateSalt()),
                        RoleId = "TUTOR",
                        Status = true,
                    };
                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();

                    account.NavigateAccount(account.RoleId);

                    if (account.Tutor != null)
                    {
                        account.Tutor.TutorAge = registerTutorModel.TutorAge;
                        account.Tutor.TutorEmail = account.Email;
                        account.Tutor.Nationality = registerTutorModel.Nationality;
                        account.Tutor.TutorDescription = registerTutorModel.TutorDescription;
                        //tính năng upload picture chưa đc phát triển, tạm thời bỏ qua ahihi
                        account.Tutor.CertiStatus = CertiStatus.Pending;
                        account.Tutor.MajorId = registerTutorModel.MajorId;
                        TutorCerti tutorCerti = new()
                        {
                            TutorId = account.Tutor.TutorId,
                            TutorCertificate = registerTutorModel.CertificateLink
                        };
                    }

                    if (FindTutorByEmail(registerTutorModel.Email) == null && account.Tutor != null)
                    {
                        await _context.Tutors.AddAsync(account.Tutor);
                    }

                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Tutor registered successfully" });
                }
                else
                {
                    return BadRequest("Existed tutor! Please try again.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("learner-register")]
        public async Task<IActionResult> RegisterLearner([FromBody] RegisterLearnerModel registerLearnerModel)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if(FindLearnerByEmail(registerLearnerModel.Email) == null)
                {
                    var account = new Account
                    {
                        FirstName = registerLearnerModel.FirstName,
                        LastName = registerLearnerModel.LastName,
                        Email = registerLearnerModel.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(registerLearnerModel.Password, BCrypt.Net.BCrypt.GenerateSalt()),
                        RoleId = "LEARNER",
                        Status = true
                    };
                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();

                    account.NavigateAccount(account.RoleId);

                    if(account.Learner != null)
                    {
                        account.Learner.LearnerAge = registerLearnerModel.LearnerAge;
                        account.Learner.LearnerEmail = account.Email;
                        //tính năng upload hình ở đây cũng chưa phái triển ahihi
                        account.Learner.MembershipId = null;
                    }

                    if(FindLearnerByEmail(registerLearnerModel.Email) == null && account.Learner != null)
                    {
                        await _context.Learners.AddAsync(account.Learner);
                    }

                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Learner registered successfully." });
                }
                else
                {
                    return BadRequest("Existed user! Please try again.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var account = await _context.Accounts.SingleOrDefaultAsync(acc => acc.Email == loginModel.Email);
                if(account == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, account.Password))
                {
                    return Unauthorized(new { message = "Invalid credentials"});
                }
                var token = _authService.GenerateToken(account);

                //if(loginModel.RememberMe)
                //{
                //    var cookieOption = new CookieOptions
                //    {
                //        Expires = DateTime.UtcNow.AddDays(7),
                //        HttpOnly = true,
                //        Secure = true
                //    };
                //    Response.Cookies.Append("jwt", token, cookieOption);
                //}

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = loginModel.RememberMe ? DateTime.UtcNow.AddDays(3) : (DateTime?)null
                };
                Response.Cookies.Append("jwt", token, cookieOptions);

                return Ok(new { token});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("jwtToken");
                return Ok(new { message = "Logged out successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //kiểm tra token từ cookies để xác nhận trạng thái đăng nhập từ người dùng
        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            try
            {
                //lấy token từ cookies
                var token = HttpContext.Request.Cookies["jwtToken"];
                if(string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { isAuthenticated = false});
                }

                //kiểm tra tính hợp lệ của token
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken? jsonToken;

                try
                {
                    jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                }
                catch (Exception)
                {
                    return Unauthorized(new { isAuthenticated = false });
                }

                if(jsonToken == null || jsonToken.ValidTo <= DateTime.UtcNow)
                {
                    return Unauthorized(new { isAuthenticated = false });
                }

                return Unauthorized(new { isAuthenticated = true });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Tutor? FindTutorByEmail(string email)
        {
            var tutor = _context.Tutors.SingleOrDefault(t => t.TutorEmail == email);
            return tutor;
        }

        private Learner? FindLearnerByEmail(string email)
        {
            var learner = _context.Learners.SingleOrDefault(l => l.LearnerEmail == email);
            return learner;
        }
    }
}
