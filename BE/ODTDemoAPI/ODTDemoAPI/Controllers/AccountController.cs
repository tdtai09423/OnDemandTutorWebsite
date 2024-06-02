using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTDemoAPI.Entities;
using ODTDemoAPI.OperationModel;
using BCrypt;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Runtime.CompilerServices;
using System.Security;

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

        [HttpGet("login-google")]
        public IActionResult LoginByGoogle()
        {
            try
            {
                var redirectUrl = Url.Action("ResponseWithGoogle", "Account");
                var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("signin-google")]
        public async Task<IActionResult> ResponseWithGoogle()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!result.Succeeded)
                {
                    return BadRequest();
                }
                var claims = result.Principal.Identities.SingleOrDefault()?.Claims.Select(c => new
                {
                    c.Type,
                    c.Value
                });
                return Ok(claims);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("tutor-register")]
        public async Task<IActionResult> RegisterTutor([FromForm] RegisterTutorModel registerTutorModel)
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
                        account.Tutor.CertiStatus = CertiStatus.Pending;
                        account.Tutor.MajorId = registerTutorModel.MajorId;
                        
                        TutorCerti tutorCerti = new()
                        {
                            TutorId = account.Tutor.TutorId,
                            TutorCertificate = registerTutorModel.CertificateLink
                        };

                        if (registerTutorModel.TutorImage.Length > 0)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", registerTutorModel.TutorImage.FileName);
                            using (var stream = System.IO.File.Create(path))
                            {
                                await registerTutorModel.TutorImage.CopyToAsync(stream);
                            }
                            account.Tutor.TutorPicture = "/images/" + account.Tutor.TutorId + "_" + account.FirstName + account.LastName;

                            _context.Tutors.Add(account.Tutor);
                            _context.SaveChanges();

                            return Ok(account.Tutor);
                        }
                        else
                        {
                            _context.Accounts.Remove(account);
                            await _context.SaveChangesAsync();
                            return BadRequest("YOU MUST UPLOAD YOUR PHOTO WHEN REGISTERING AS A TUTOR!!");
                        }
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
        public async Task<IActionResult> RegisterLearner([FromForm] RegisterLearnerModel registerLearnerModel)
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
                        account.Learner.MembershipId = null;
                        if (registerLearnerModel.LearnerImage == null)
                        {
                            account.Learner.LearnerPicture = "";
                        }
                        
                        else if(registerLearnerModel.LearnerImage.Length > 0)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", registerLearnerModel.LearnerImage.FileName);
                            using(var stream = System.IO.File.Create(path))
                            {
                                await registerLearnerModel.LearnerImage.CopyToAsync(stream);
                            }
                            account.Learner.LearnerPicture = "/images/" + registerLearnerModel.LearnerImage.FileName;
                        }
                        
                        else
                        {
                            account.Learner.LearnerPicture = "";
                        }

                        _context.Learners.Add(account.Learner);
                        await _context.SaveChangesAsync();
                        return Ok(account.Learner);
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
                if(account.Status == false)
                {
                    return BadRequest("Your account is inactivated. Contact Hotline một chín không không một không không biết for advisory.");
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

        //tính năng only for admin
        [HttpPost("avtivate-account")]
        public async Task<IActionResult> OperateAccountStatus(string email)
        {
            try
            {
                if(FindLearnerByEmail(email) == null || FindTutorByEmail(email) == null)
                {
                    return BadRequest("Not found!");
                }
                if(FindTutorByEmail(email) != null)
                {
                    FindAccountByEmail(email).Status = false;
                }
                if (FindLearnerByEmail(email) != null)
                {
                    FindAccountByEmail(email).Status = false;
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Account? FindAccountByEmail(string email)
        {
            var account = _context.Accounts.SingleOrDefault(a => a.Email == email);
            return account;
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
