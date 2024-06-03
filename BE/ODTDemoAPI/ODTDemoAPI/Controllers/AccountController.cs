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
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _memoryCache;
        public AccountController(OnDemandTutorContext context, IAuthService authService, IEmailService emailService, IMemoryCache memoryCache)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
            _memoryCache = memoryCache;
        }

        [HttpGet("login-google")]
        public IActionResult LoginByGoogle()
        {
            try
            {
                var redirectUrl = Url.Action("RespondWithGoogle", "Account");
                var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("signin-google")]
        public async Task<IActionResult> RespondWithGoogle()
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

                var givenName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                var surname = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if(string.IsNullOrEmpty(email))
                {
                    return BadRequest("Not found email in claims");
                }

                var account = FindAccountByEmail(email);
                if (account != null)
                {
                    var token = _authService.GenerateToken(account);
                    return Ok(new { token });
                }
                else
                {
                    HttpContext.Session.SetString("GivenName", givenName);
                    HttpContext.Session.SetString("Surname", surname);
                    HttpContext.Session.SetString("Email", email);
                }

                return Ok(claims);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("enter-information-google-login")]
        public async Task<IActionResult> EnterInforGoogleLogin([FromForm] GoogleLoginInputModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var account = new Account
                {
                    FirstName = HttpContext.Session.GetString("GivenName"),
                    LastName = HttpContext.Session.GetString("Surname"),
                    Email = HttpContext.Session.GetString("Email"),
                    Password = BCrypt.Net.BCrypt.HashPassword("NoPassword", BCrypt.Net.BCrypt.GenerateSalt()),
                    RoleId = "LEARNER",
                    Status = true,
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now,
                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                account.NavigateAccount(account.RoleId);

                if (account.Learner != null)
                {
                    account.Learner.LearnerAge = model.Age;
                    account.Learner.LearnerEmail = account.Email;
                    account.Learner.MembershipId = null;
                    if (model.Picture == null)
                    {
                        account.Learner.LearnerPicture = "";
                    }

                    else if (model.Picture.Length > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", model.Picture.FileName);
                        using (var stream = System.IO.File.Create(path))
                        {
                            await model.Picture.CopyToAsync(stream);
                        }
                        account.Learner.LearnerPicture = "/images/" + account.Learner.LearnerId + "_" + account.FirstName + account.LastName;
                    }

                    else
                    {
                        account.Learner.LearnerPicture = "";
                    }

                    _context.Learners.Add(account.Learner);
                    await _context.SaveChangesAsync();
                    return Ok(account.Learner);
                }

                if (FindLearnerByEmail(account.Email) == null && account.Learner != null)
                {
                    await _context.Learners.AddAsync(account.Learner);
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Learner registered successfully." });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register-tutor-google-account")]
        public async Task<IActionResult> RegisterTutorGoogleAccount([FromForm] GoogleTutorModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var email = HttpContext.Session.GetString("Email");

                if (FindTutorByEmail(email) == null)
                {
                    var account = FindAccountByEmail(email);

                    account.RoleId = "TUTOR";

                    await _context.SaveChangesAsync();

                    account.NavigateAccount(account.RoleId);

                    if (account.Tutor != null)
                    {
                        account.Tutor.TutorAge = model.TutorAge;
                        account.Tutor.TutorEmail = account.Email;
                        account.Tutor.Nationality = model.Nationality;
                        account.Tutor.TutorDescription = model.TutorDescription;
                        account.Tutor.CertiStatus = CertiStatus.Pending;
                        account.Tutor.MajorId = model.MajorId;

                        TutorCerti tutorCerti = new()
                        {
                            TutorId = account.Tutor.TutorId,
                            TutorCertificate = model.CertificateLink
                        };

                        if (model.TutorImage.Length > 0)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", model.TutorImage.FileName);
                            using (var stream = System.IO.File.Create(path))
                            {
                                await model.TutorImage.CopyToAsync(stream);
                            }
                            account.Tutor.TutorPicture = "/images/" + account.Tutor.TutorId + "_" + account.FirstName + account.LastName;

                            _context.Tutors.Add(account.Tutor);
                            _context.SaveChanges();

                            return RedirectToAction("SendVerificationCode", "Account", new { tutor = account.Tutor });
                        }
                        else
                        {
                            _context.Accounts.Remove(account);
                            await _context.SaveChangesAsync();
                            return BadRequest("YOU MUST UPLOAD YOUR PHOTO WHEN REGISTERING AS A TUTOR!!");
                        }
                    }

                    if (FindTutorByEmail(email) == null && account.Tutor != null)
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

        [HttpPost("send-verification-code")]
        public async Task<IActionResult> SendVerificationCode([FromQuery] string toEmail)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                return BadRequest("Email address is required.");
            }

            if (!IsValidEmail(toEmail))
            {
                return BadRequest("Invalid email address format.");
            }

            try
            {
                var verificationCode = GenerateVerificationCode();

                if (verificationCode == null)
                {
                    return BadRequest("Failed to generate verification code.");
                }

                _memoryCache.Set($"{toEmail}_verificationCode", verificationCode, TimeSpan.FromMinutes(30));

                await _emailService.SendMailAsync(toEmail, "Verification Code", $"Your verification code is: {verificationCode}");

                return Ok(new { message = "Verification code has been sent to you." });
            }
            catch
            {
                return BadRequest("An error occurred while sending the verification code.");
            }
        }

        [HttpPost("verify-code")]
        public IActionResult VerifyCode(string email, string code)
        {
            var storedCode = _memoryCache.Get<string>($"{email}_verificationCode");

            if (storedCode == null)
            {
                return BadRequest("Code is expired.");
            }
            if (storedCode != code)
            {
                return BadRequest("Wrong code!");
            }

            _memoryCache.Remove($"{email}_verificationCode");
            FindAccountByEmail(email)!.IsEmailVerified = true;

            return RedirectToAction("GetAllApprovedTutors", "Tutor", new { message = "Verify email successfully!" });
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
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
                        IsEmailVerified = false,
                        CreatedDate = DateTime.Now,
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

                    return BadRequest(new { message = "Failed to register." });
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (FindLearnerByEmail(registerLearnerModel.Email) == null)
                {
                    var account = new Account
                    {
                        FirstName = registerLearnerModel.FirstName,
                        LastName = registerLearnerModel.LastName,
                        Email = registerLearnerModel.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(registerLearnerModel.Password, BCrypt.Net.BCrypt.GenerateSalt()),
                        RoleId = "LEARNER",
                        Status = true,
                        IsEmailVerified = false,
                        CreatedDate = DateTime.Now,
                    };
                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();

                    account.NavigateAccount(account.RoleId);

                    if (account.Learner != null)
                    {
                        account.Learner.LearnerAge = registerLearnerModel.LearnerAge;
                        account.Learner.LearnerEmail = account.Email;
                        account.Learner.MembershipId = null;
                        if (registerLearnerModel.LearnerImage == null)
                        {
                            account.Learner.LearnerPicture = "";
                        }

                        else if (registerLearnerModel.LearnerImage.Length > 0)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", registerLearnerModel.LearnerImage.FileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await registerLearnerModel.LearnerImage.CopyToAsync(stream);
                            }
                            account.Learner.LearnerPicture = "/images/" + account.Learner.LearnerId + "_" + account.FirstName + account.LastName;
                        }

                        else
                        {
                            account.Learner.LearnerPicture = "";
                        }

                        _context.Learners.Add(account.Learner);
                        await _context.SaveChangesAsync();
                        return Ok(account.Learner);
                    }

                    if (FindLearnerByEmail(registerLearnerModel.Email) == null && account.Learner != null)
                    {
                        await _context.Learners.AddAsync(account.Learner);
                    }

                    await _context.SaveChangesAsync();
                    return BadRequest(new { message = "Failed to register." });
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var account = await _context.Accounts.SingleOrDefaultAsync(acc => acc.Email == loginModel.Email);
                if (account == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, account.Password))
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }
                if (account.Status == false)
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

                return Ok(new { token });
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
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { isAuthenticated = false });
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

                if (jsonToken == null || jsonToken.ValidTo <= DateTime.UtcNow)
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

        [HttpPost("get-account-by-email")]
        public Account? GetAccountByEmail(string email)
        {
            var account = _context.Accounts.SingleOrDefault(a => a.Email == email);
            return account;
        }

        //tính năng only for admin
        [HttpPost("avtivate-account")]
        public async Task<IActionResult> OperateAccountStatus(string email)
        {
            try
            {
                if (FindLearnerByEmail(email) == null || FindTutorByEmail(email) == null)
                {
                    return BadRequest("Not found!");
                }
                if (FindTutorByEmail(email) != null)
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

        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
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
