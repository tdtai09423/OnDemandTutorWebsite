using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ODTDemoAPI.Services;
using ODTDemoAPI.Entities;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Stripe;
using Stripe.Checkout;
using ODTDemoAPI.ChatHubs;

namespace ODTDemoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
            builder.Services.Configure<SmtpSetting>(builder.Configuration.GetSection("Smtp"));
            builder.Services.Configure<VNPaySetting>(builder.Configuration.GetSection("VNPay"));
            builder.Services.AddScoped<SessionService>();

            builder.Services.AddSingleton<IStripeClient>(new StripeClient(builder.Configuration.GetSection("Stripe:SecretKey").Get<string>()));

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            builder.Services.AddMemoryCache();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

                options.ClientId = googleAuthNSection["ClientId"]!;
                options.ClientSecret = googleAuthNSection["ClientSecret"]!;
                options.CallbackPath = "/login-google";
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["jwtToken"];
                        if(!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddSingleton<UserStatusService>();
            builder.Services.AddScoped<VNPayService>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireLearnerRole", policy => policy.RequireRole("LEARNER"));
                options.AddPolicy("RequireTutorRole", policy => policy.RequireRole("TUTOR"));
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("ADMIN"));
            });
            builder.Services.AddScoped<IAuthService,AuthService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            builder.Services.AddDbContext<OnDemandTutorContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("OnDemandTutor"));
            });

            builder.Services.AddLogging();

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll", builder =>
            //    {
            //        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            //    });
            //});
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins("https://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            builder.Services.AddSingleton<AccountData>();
            builder.Services.AddSingleton<BookingData>();
            builder.Services.AddSingleton<NotificationData>();
            builder.Services.AddSingleton<SectionData>();
            builder.Services.AddHostedService<AutomaticCleanUpService>();
            builder.Services.AddHostedService<AutomaticNotifyService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseSession();
            app.UseCors("CorsPolicy");

            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub");

            app.MapGet("/accounts-messages", (AccountData data) => data.AccountsData.Order());
            app.MapGet("/bookings-messages", (BookingData data) => data.BookingsData.Order());
            app.MapGet("/notifications-messages", (NotificationData data) => data.NotificationsData.Order());
            app.MapGet("/sections-messages", (SectionData data) => data.SectionsData.Order());

            app.Run();
        }
    }
}
