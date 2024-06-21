using Microsoft.AspNetCore.Mvc;
using ODTDemoAPI.Entities;
using ODTDemoAPI.OperationModel;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using ODTDemoAPI.EntityViewModels;
using Microsoft.AspNetCore.Authorization;

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
        private readonly ILogger<AccountController> _logger;
        public AccountController(OnDemandTutorContext context, IAuthService authService, IEmailService emailService, IMemoryCache memoryCache, ILogger<AccountController> logger)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [HttpPost("login-google")]
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

        [HttpPost("signin-google")]
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

                //gọi tên và email từ claim có được sau khi login google
                var givenName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                var surname = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Not found email in claims");
                }

                if(FindAccountByEmail(email) != null)
                {
                    return BadRequest("Your email has been registered before.");
                }

                var account = FindAccountByEmail(email);
                if (account != null)
                {
                    var token = _authService.GenerateToken(account);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = (DateTime?)null
                    };
                    var accountWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == account.Id);
                    if (account.Wallet == null)
                    {
                        if (accountWallet == null)
                        {
                            var wallet = new Wallet
                            {
                                WalletId = account.Id,
                                Balance = 0,
                            };
                            _context.Wallets.Add(wallet);

                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            decimal balance = accountWallet.Balance;
                            _context.Wallets.Remove(accountWallet);
                            await _context.SaveChangesAsync();

                            var wallet = new Wallet
                            {
                                WalletId = account.Id,
                                Balance = balance,
                            };
                            _context.Wallets.Add(wallet);

                            await _context.SaveChangesAsync();
                        }
                        account.Wallet = accountWallet;
                    }
                    Response.Cookies.Append("jwt", token, cookieOptions);
                    HttpContext.Session.SetObject("Account", account);
                    return Ok(new { token });
                }
                else
                {
                    //lưu tên và email để lấy sử dụng cho việc nhập thông tin
                    HttpContext.Session.SetString("GivenName", givenName!);
                    HttpContext.Session.SetString("Surname", surname!);
                    HttpContext.Session.SetString("Email", email);
                    HttpContext.Session.SetObject("Account", account);
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var account = new Account
                {
                    FirstName = HttpContext.Session.GetString("GivenName")!,
                    LastName = HttpContext.Session.GetString("Surname")!,
                    Email = HttpContext.Session.GetString("Email")!,
                    Password = BCrypt.Net.BCrypt.HashPassword("NoPassword", BCrypt.Net.BCrypt.GenerateSalt()),
                    RoleId = "LEARNER",
                    Status = true,
                    IsEmailVerified = true,
                    CreatedDate = DateTime.Now,
                };
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                var wallet = new Wallet { WalletId = account.Id, Balance = 0 };
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();

                account.NavigateAccount(account.RoleId);

                if (account.Learner != null)
                {
                    account.Learner.LearnerAge = model.Age;
                    account.Learner.LearnerEmail = account.Email!;
                    account.Learner.MembershipId = null;
                    if (model.Picture == null)
                    {
                        account.Learner.LearnerPicture = "";
                    }

                    else if (model.Picture.Length > 0)
                    {
                        var extension = Path.GetExtension(model.Picture.FileName);
                        var fileName = $"{account.Id}_{account.FirstName}{account.LastName}{extension}";
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                        using (var stream = System.IO.File.Create(path))
                        {
                            await model.Picture.CopyToAsync(stream);
                        }
                        account.Learner.LearnerPicture = "/images/" + fileName;
                    }

                    else
                    {
                        account.Learner.LearnerPicture = "";
                    }

                    _context.Learners.Add(account.Learner);
                    await _context.SaveChangesAsync();

                    return Ok(new { learner = account.Learner });
                }

                if (FindLearnerByEmail(account.Email!) == null && account.Learner != null)
                {
                    await _context.Learners.AddAsync(account.Learner);
                }

                await _context.SaveChangesAsync();
                return BadRequest(new { message = "Failed to register." });

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

                if (FindTutorByEmail(email!) == null)
                {
                    var account = FindAccountByEmail(email!);

                    account!.RoleId = "TUTOR";

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
                            var extension = Path.GetExtension(model.TutorImage.FileName);
                            var fileName = $"{account.Id}_{account.FirstName}{account.LastName}{extension}";
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                            using (var stream = System.IO.File.Create(path))
                            {
                                await model.TutorImage.CopyToAsync(stream);
                            }
                            account.Tutor.TutorPicture = "/images/" + fileName;

                            _context.Tutors.Add(account.Tutor);
                            _context.SaveChanges();

                            return Ok(new { tutor = account.Tutor });
                        }
                        else
                        {
                            _context.Accounts.Remove(account);
                            await _context.SaveChangesAsync();
                            return BadRequest("YOU MUST UPLOAD YOUR PHOTO WHEN REGISTERING AS A TUTOR!!");
                        }
                    }

                    if (FindTutorByEmail(email!) == null && account.Tutor != null)
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
        public async Task<IActionResult> VerifyCode(string email, string code)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            var storedCode = _memoryCache.Get<string>($"{email}_verificationCode");

            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Code is invalid!");
            }
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
            FindAccountByEmail(email)!.Status = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Verify email successfully!" , Account = account});
        }

        private static bool IsValidEmail(string email)
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
                        Status = false,
                        IsEmailVerified = false,
                        CreatedDate = DateTime.Now,
                    };

                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();

                    var wallet = new Wallet { WalletId = account.Id, Balance = 0 };
                    _context.Wallets.Add(wallet);
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
                            var extension = Path.GetExtension(registerTutorModel.TutorImage.FileName);
                            var fileName = $"{account.Id}_{account.FirstName}{account.LastName}{extension}";
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                            using (var stream = System.IO.File.Create(path))
                            {
                                await registerTutorModel.TutorImage.CopyToAsync(stream);
                            }
                            account.Tutor.TutorPicture = "/images/" + fileName;

                            _context.Tutors.Add(account.Tutor);
                            _context.SaveChanges();

                            return Ok(new { tutor = account.Tutor });
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
                        Status = false,
                        IsEmailVerified = false,
                        CreatedDate = DateTime.Now,
                    };
                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();

                    var wallet = new Wallet { WalletId = account.Id, Balance = 0 };
                    _context.Wallets.Add(wallet);
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
                            var extension = Path.GetExtension(registerLearnerModel.LearnerImage.FileName);
                            var fileName = $"{account.Id}_{account.FirstName}{account.LastName}{extension}";
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                            using (var stream = System.IO.File.Create(path))
                            {
                                await registerLearnerModel.LearnerImage.CopyToAsync(stream);
                            }
                            account.Learner.LearnerPicture = "/images/" + fileName;
                        }

                        else
                        {
                            account.Learner.LearnerPicture = "";
                        }

                        _context.Learners.Add(account.Learner);
                        await _context.SaveChangesAsync();
                        return Ok(new { learner = account.Learner });
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
                    Expires = loginModel.RememberMe ? DateTime.Now.AddDays(3) : (DateTime?)null
                };

                var accountWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == account.Id);
                if(account.Wallet == null)
                {
                    if(accountWallet == null)
                    {
                        var wallet = new Wallet
                        {
                            WalletId = account.Id,
                            Balance = 0,
                        };
                        _context.Wallets.Add(wallet);

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        decimal balance = accountWallet.Balance;
                        _context.Wallets.Remove(accountWallet);
                        await _context.SaveChangesAsync();

                        var wallet = new Wallet
                        {
                            WalletId = account.Id,
                            Balance = balance,
                        };
                        _context.Wallets.Add(wallet);

                        await _context.SaveChangesAsync();
                    }

                    account.Wallet = accountWallet;
                }

                Response.Cookies.Append("jwt", token, cookieOptions);
                HttpContext.Session.SetObject("Account", account);

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
                HttpContext.Session.Remove("Account");
                return Ok(new { message = "Logged out successfully!" });
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

        [HttpGet("all-accounts")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accountViewModels = new List<AccountViewModel>();
                var accounts = await _context.Accounts.ToListAsync();
                foreach (var account in accounts)
                {
                    accountViewModels.Add(new AccountViewModel
                    {
                        Id = account.Id,
                        FullName = account.FirstName + " " + account.LastName,
                        Email = account.Email,
                        RoleId = account.RoleId,
                        Status = account.Status,
                        IsEmailVerified = account.IsEmailVerified,
                        CreatedDate = account.CreatedDate,
                    });
                }
                return Ok(accountViewModels);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ex.Message });
            }
        }

        [HttpPut("toggle-status")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ToggleAccountStatus([FromBody] ToggleAccountStatusModel model)
        {
            try
            {
                var account = FindAccountByEmail(model.Email);
                if (account == null)
                {
                    return NotFound("Account not found");
                }

                account.Status = model.Status;
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Account status update successfully!", account });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<IActionResult> UpdateUserInfo(UpdateUserModel model, Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model == null)
            {
                return BadRequest(new { message = "Model is null." });
            }

            if (account == null)
            {
                return NotFound(new { message = "Account is null." });
            }

            try
            {
                if (model.PasswordModel == null || string.IsNullOrEmpty(model.PasswordModel.CurrentPassword)
                    || string.IsNullOrEmpty(model.PasswordModel.Password)
                    || string.IsNullOrEmpty(model.PasswordModel.ConfirmPassword))
                {
                    ModelState.Remove("PasswordModel.CurrentPassword");
                    ModelState.Remove("PasswordModel.Password");
                    ModelState.Remove("PasswordModel.ConfirmPassword");
                }

                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    account.FirstName = model.FirstName;
                }

                if (!string.IsNullOrEmpty(model.LastName))
                {
                    account.LastName = model.LastName;
                }

                if (model.PasswordModel != null)
                {
                    if (!string.IsNullOrEmpty(model.PasswordModel.CurrentPassword) &&
                        !string.IsNullOrEmpty(model.PasswordModel.Password) &&
                        !string.IsNullOrEmpty(model.PasswordModel.ConfirmPassword))
                    {
                        if (!BCrypt.Net.BCrypt.Verify(model.PasswordModel.CurrentPassword, account.Password))
                        {
                            return BadRequest(new { message = "Current password is incorrect." });
                        }

                        if (model.PasswordModel.Password != model.PasswordModel.ConfirmPassword)
                        {
                            return BadRequest(new { message = "New password and confirm password do not match." });
                        }

                        account.Password = BCrypt.Net.BCrypt.HashPassword(model.PasswordModel.Password, BCrypt.Net.BCrypt.GenerateSalt());
                    }
                }

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User update successfully!", account });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-learner")]
        public async Task<IActionResult> UpdateLearnerInfo([FromForm] UpdateLearnerModel model, int accountId)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
                if (account == null || account.RoleId != "LEARNER")
                {
                    return NotFound("Account not found.");
                }

                var result = await UpdateUserInfo(model, account);

                if (result is BadRequestObjectResult)
                {
                    return result;
                }

                var learner = FindLearnerByEmail(account.Email);

                if (learner == null)
                {
                    return BadRequest("Learner Not Found");
                }

                if (model == null)
                {
                    return BadRequest(new { message = "Learner model is invalid" });
                }
                else
                {
                    if (model.Age.HasValue)
                    {
                        learner.LearnerAge = model.Age.Value;
                    }

                    if (model.Image != null && model.Image.Length > 0)
                    {
                        var oldImagePath = learner.LearnerPicture;
                        var newImagePath = await SaveImageAsync(model.Image, account);
                        learner.LearnerPicture = newImagePath;

                        if (!string.IsNullOrEmpty(oldImagePath))
                        {
                            DeleteOldImage(oldImagePath);
                        }
                    }

                    _context.Learners.Update(learner);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "Update learner successfully!", Learner = learner });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-tutor")]
        public async Task<IActionResult> UpdateTutorModel([FromForm] UpdateTutorModel model, int accountId)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
                if (account == null || account.RoleId != "TUTOR")
                {
                    return Unauthorized("Account not found");
                }

                var result = await UpdateUserInfo(model, account);

                if (result is BadRequestObjectResult)
                {
                    return result;
                }

                var tutor = FindTutorByEmail(account.Email);

                if (tutor == null)
                {
                    return BadRequest(new { message = "Tutor not found" });
                }

                if (model == null)
                {
                    return BadRequest(new { message = "Tutor model is imvalid." });
                }
                else
                {
                    if (model.Age.HasValue)
                    {
                        tutor.TutorAge = model.Age.Value;
                    }

                    if (!string.IsNullOrEmpty(model.Nationality))
                    {
                        tutor.Nationality = model.Nationality;
                    }

                    if (!string.IsNullOrEmpty(model.Description))
                    {
                        tutor.TutorDescription = model.Description;
                    }

                    if (model.Image != null && model.Image.Length > 0)
                    {
                        var oldImagePath = tutor.TutorPicture;
                        var newImagePath = await SaveImageAsync(model.Image, account);
                        tutor.TutorPicture = newImagePath;

                        if (!string.IsNullOrEmpty(oldImagePath))
                        {
                            DeleteOldImage(oldImagePath);
                        }
                    }

                    _context.Tutors.Update(tutor);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "Update tutor successfully!", Tutor = tutor });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-email")]
        public async Task<IActionResult> UpdateEmail([FromForm] string email, int accountId)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
                if (account == null || account.RoleId != "LEARNER")
                {
                    return NotFound("Account not found.");
                }

                if (!string.IsNullOrEmpty(email) && email != account.Email)
                {
                    if (IsValidEmail(email))
                    {
                        account.Email = email;
                        account.IsEmailVerified = false;
                        _context.Accounts.Update(account);
                        if(account.RoleId == "LEARNER")
                        {
                            var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == accountId);
                            learner!.LearnerEmail = email;
                            _context.Learners.Update(learner);
                        }
                        else
                        {
                            var tutor = await _context.Tutors.FirstOrDefaultAsync(l => l.TutorId == accountId);
                            tutor!.TutorEmail = email;
                            _context.Tutors.Update(tutor);
                        }
                        await _context.SaveChangesAsync();

                        var verificationCode = GenerateVerificationCode();

                        _memoryCache.Set($"{account.Email}_verificationCode", verificationCode, TimeSpan.FromMinutes(30));

                        await _emailService.SendMailAsync(account.Email, "Verification Code", $"Your verification code is: {verificationCode}");
                    }
                }
                return Ok(new { message = "Update email successfully!", Account = FindAccountByEmail(email) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static async Task<string> SaveImageAsync(IFormFile image, Account account)
        {
            var extension = Path.GetExtension(image.FileName);
            var fileName = $"{account.Id}_{account.FirstName}{account.LastName}{extension}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using (var stream = System.IO.File.Create(path))
            {
                await image.CopyToAsync(stream);
            }
            return $"/wwwroot/images/{fileName}";
        }

        private static void DeleteOldImage(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        [HttpPost("forgot-password")]
        public IActionResult HandleForgotPassword(string email)
        {
            try
            {
                var account = FindAccountByEmail(email);
                if (account == null)
                {
                    return NotFound(new { message = "Not found account" });
                }
                return Ok(new { message = "Verification code has been sent to you." });//chuyển hướng đến action SendVerificationCode
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //xác thực mã xác thực để reset password
        [HttpGet("reset-password")]
        public IActionResult VerifyResetPasswordCode(string email, string code)
        {
            try
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
                return Ok(new { message = "Verify code successfully!" });//chuyển hướng đến action ResetPassword
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(string newPassword, string confirmPassword)
        {
            try
            {
                if (newPassword != confirmPassword)
                {
                    return BadRequest("Password and confirm password do not match.");
                }

                var email = User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
                if (email == null)
                {
                    return Unauthorized(new { message = "You are logging out or your session is out. Please check your login status." });
                }

                var account = FindAccountByEmail(email!);

                account!.Password = BCrypt.Net.BCrypt.HashPassword(newPassword, BCrypt.Net.BCrypt.GenerateSalt());
                return Ok(new { message = "Your password has been changed." });
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
