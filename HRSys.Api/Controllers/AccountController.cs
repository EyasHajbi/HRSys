using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRSys.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HRSys.Api.Exception;
using System.Net;
using HRSys.Api.Helpers;
using System.Linq.Expressions;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using HRSys.Services.Common.Interface;
using Newtonsoft.Json;
using HRSys.Api.Extensions;
using HRSys.Services.App_Users;
using HRSys.DTO.App_Users;
using Microsoft.Extensions.Configuration;

namespace HRSys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IEmployeesService _employeesService;
        private readonly ICustomSuccess _customSuccess;
        private readonly ISendSMS_Service _sendSMS_Service;
        private readonly IConfiguration _configuration;
        // private readonly IEmailSender _emailSender;

        public AccountController(IEmployeesService employeesService,
            ISendSMS_Service sendSMS_Service, ICustomSuccess customSuccess, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _employeesService = employeesService;
            _sendSMS_Service = sendSMS_Service;
            _customSuccess = customSuccess;
            _configuration = configuration;
        }
        [Authorize]
        [Route("GetEmployee")]
        [HttpGet]
        public IActionResult GetEmployee()
        {
            string lang = this.Request.Headers["language"].ToString();

            if (string.IsNullOrEmpty(lang))
                lang = "ar";

            string Id = CurrentUser.GetUserId(this.User, HttpContext.Session.GetString("JWToken") ?? "");
            var model = _employeesService.GetById(Convert.ToInt32(Id));

            if (model == null)
            {
                string message = ExceptionMessage.ResourceManager.GetString("AccountInfo." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("NoAccountInfo." + lang);

                throw new CustomException(HttpStatusCode.Forbidden, message, subMessage);
            }
            return Ok(model);
        }


        [Route("RegisterUser")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser()
        {
            string lang = this.Request.Headers["language"].ToString();

            if (string.IsNullOrEmpty(lang))
                lang = "ar";

            EmployeesDto user = await this.Request.GetRawBodyStringAsync<EmployeesDto>();

            bool isNationalIdExist = await _employeesService.CheckIsNationalIdExist(user.NationalId);

            string message = ExceptionMessage.ResourceManager.GetString("Register." + lang);

            if (isNationalIdExist)
            {
                string subMessage = ExceptionMessage.ResourceManager.GetString("NationalIdExist." + lang);

                throw new CustomException(HttpStatusCode.Forbidden, message, subMessage);
            }
            else
            {
                user.IsConfirmed = false;
                user.IsDeleted = false;
                user.CreatedOn = DateTime.Now;
                string ConfirmationCode = "1234";//GenerateRandomCode(4);
                user.ConfirmationCode = ConfirmationCode;
                user.ProfileImage = user.ProfileImage.SaveBase64("Employees", user.MobileNo, "png");
                _employeesService.Insert(user);

            }
            //var AccessToken = GenerateJWTAccessToken(user);
            Send_OTP_ConfirmCode(user);

            var response = _customSuccess.GetSuccessResponse("Register." + lang, "RegisterSuccess." + lang);
            return Ok(response);
        }
        [Authorize]
        [Route("UpdateUser")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser()
        {
            string lang = this.Request.Headers["language"].ToString();

            if (string.IsNullOrEmpty(lang))
                lang = "ar";

            EmployeesDto user = await this.Request.GetRawBodyStringAsync<EmployeesDto>();

            //string Id = CurrentUser.GetUserId(this.User, HttpContext.Session.GetString("JWToken") ?? "");

            //if(string.IsNullOrEmpty(Id))
            //{
            //    string message = ExceptionMessage.ResourceManager.GetString("AccountInfo." + lang);
            //    string subMessage = ExceptionMessage.ResourceManager.GetString("NoAccountInfo." + lang);
            //    throw new CustomException(HttpStatusCode.Forbidden, message, subMessage);
            //}
            var _user = _employeesService.GetById(Convert.ToInt32(user.Id));

            if (_user != null)
            {

                if (_user.MobileNo != user.MobileNo)
                {
                    string ConfirmationCode = "1234"; GenerateRandomCode(4);
                    _user.ConfirmationCode = ConfirmationCode;
                    _user.IsConfirmed = false;
                    _user.FirstName = user.FirstName;
                    _user.DeviceID = user.DeviceID;
                    _user.MobileNo = user.MobileNo;
                    _user.TokenID = user.TokenID;
                    _user.SecondName = user.SecondName;
                    _user.ModifiedDate = DateTime.Now;
                    _user.ProfileImage = user.ProfileImage.SaveBase64("Employees", user.MobileNo, "png");
                    _employeesService.Update(_user);
                    Send_OTP_ConfirmCode(_user);

                    string message = ExceptionMessage.ResourceManager.GetString("ActivateAccount." + lang);
                    string subMessage = ExceptionMessage.ResourceManager.GetString("InsertOPTCode." + lang);

                    throw new CustomException(HttpStatusCode.Unauthorized, message, subMessage);
                }
                else
                {
                    _user.FirstName = user.FirstName;
                    _user.DeviceID = user.DeviceID;
                    _user.MobileNo = user.MobileNo;
                    _user.TokenID = user.TokenID;
                    _user.SecondName = user.SecondName;
                    _user.ModifiedDate = DateTime.Now;
                    _employeesService.Update(_user);

                    var response = _customSuccess.GetSuccessResponse("UpdateUser." + lang, "UpdateUserSuccess." + lang);
                    return Ok(response);
                }
            }
            else
            {
                string message = ExceptionMessage.ResourceManager.GetString("ActivateAccount." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("SecurityCode." + lang);

                throw new CustomException(HttpStatusCode.Forbidden, message, subMessage);
            }
        }

        [Route("ActivateAccount")]
        [HttpPost]
        public async Task<IActionResult> ActivateAccount()
        {
            string lang = this.Request.Headers["language"].ToString();

            if (string.IsNullOrEmpty(lang))
                lang = "ar";

            EmployeesDto user = await this.Request.GetRawBodyStringAsync<EmployeesDto>();

            Expression<Func<Employees, bool>> expression = (u => u.IsDeleted == false && u.NationalId == user.NationalId && u.ConfirmationCode == user.ConfirmationCode);
            var _users = await _employeesService.All(true, expression);
            var _user = _users.FirstOrDefault();

            if (_user != null)
            {
                _user.IsConfirmed = true;
                _user.ConfirmationCode = user.ConfirmationCode;
                _employeesService.Update(_user);
                var AccessToken = GenerateJWTAccessToken(_user);

                return Ok(new { accessToken = AccessToken });
            }
            else
            {
                string message = ExceptionMessage.ResourceManager.GetString("ActivateAccount." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("SecurityCode." + lang);

                throw new CustomException(HttpStatusCode.Forbidden, message, subMessage);
            }
        }

        [Route("UserLogin")]
        [HttpPost]
        public async Task<IActionResult> UserLogin()
        {
            string lang = this.Request.Headers["language"].ToString();

            if (string.IsNullOrEmpty(lang))
                lang = "ar";

            EmployeesDto user = await this.Request.GetRawBodyStringAsync<EmployeesDto>();

            Expression<Func<Employees, bool>> expression = (u => u.IsDeleted == false && u.NationalId == user.NationalId
              && u.Password == user.Password);
            var _users = await _employeesService.All(true, expression);
            var _user = _users.FirstOrDefault();

            if (_user == null)
            {
                string message = ExceptionMessage.ResourceManager.GetString("Login." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("ErrorLogin." + lang);

                throw new CustomException(HttpStatusCode.BadRequest, message, subMessage);
            }


            if (!_user.IsConfirmed)
            {
                string ConfirmationCode = "1234"; GenerateRandomCode(4);
                _user.ConfirmationCode = ConfirmationCode;
                _user.IsConfirmed = false;
                _user.IsDeleted = false;
                _employeesService.Update(_user);
                Send_OTP_ConfirmCode(_user);

                string message = ExceptionMessage.ResourceManager.GetString("Login." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("IsNotConfirmed." + lang);

                throw new CustomException(HttpStatusCode.Unauthorized, message, subMessage);

            }

            _user.IsConfirmed = true;
            _user.ModifiedDate = DateTime.Now;
            _user.DeviceID = user.DeviceID;
            _user.TokenID = user.TokenID;
            _employeesService.Update(_user);


            //await AddClaim(user, lang);

            var AccessToken = GenerateJWTAccessToken(_user);
            HttpContext.Session.SetString("JWToken", AccessToken);
            return Ok(new { accessToken = AccessToken });
        }



        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword()
        {
            string lang = this.Request.Headers["language"].ToString();

            if (string.IsNullOrEmpty(lang))
                lang = "ar";

            EmployeesDto user = await this.Request.GetRawBodyStringAsync<EmployeesDto>();

            Expression<Func<Employees, bool>> expression = (u => u.IsDeleted == false && u.MobileNo == user.MobileNo);
            var _users = await _employeesService.All(true, expression);
            var _user = _users.FirstOrDefault();

            if (_user != null)
            {

                string ConfirmationCode = "1234"; GenerateRandomCode(4);
                _user.ConfirmationCode = ConfirmationCode;
                _user.IsConfirmed = false;
                _employeesService.Update(_user);
                Send_OTP_ConfirmCode(_user);

                var response = _customSuccess.GetSuccessResponse("ForgotPassword." + lang, "SendOTP." + lang);
                return Ok(response);
            }
            else
            {
                string message = ExceptionMessage.ResourceManager.GetString("ForgotPassword." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("ErrorEmail." + lang);

                throw new CustomException(HttpStatusCode.Forbidden, message, subMessage);
            }
        }
        void Send_OTP_ConfirmCode(EmployeesDto user)
        {
            _sendSMS_Service.SendSMS(user.MobileNo.Replace("+", ""), user.ConfirmationCode);
        }


        [Route("ConfirmForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ConfirmForgotPassword()
        {
            string lang = this.Request.Headers["language"].ToString();

            if (string.IsNullOrEmpty(lang))
                lang = "ar";

            EmployeesDto user = await this.Request.GetRawBodyStringAsync<EmployeesDto>();

            Expression<Func<Employees, bool>> expression = (u => u.IsDeleted == false && u.MobileNo == user.MobileNo
            && u.ConfirmationCode == user.ConfirmationCode);
            var _users = _employeesService.All(true, expression).GetAwaiter().GetResult();
            var _user = _users.FirstOrDefault();
            if (_user != null)
            {
                _user.IsConfirmed = true;
                _user.ConfirmationCode = user.ConfirmationCode;
                _employeesService.Update(_user);
                var AccessToken = GenerateJWTAccessToken(_user);

                return Ok(new { accessToken = AccessToken });
            }
            else
            {
                string message = ExceptionMessage.ResourceManager.GetString("ForgotPassword." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("SecurityCode." + lang);

                throw new CustomException(HttpStatusCode.Forbidden, message, subMessage);
            }
        }

        [Route("UpdateForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> UpdateForgotPassword()
        {

            string lang = this.Request.Headers["language"].ToString();

            if (string.IsNullOrEmpty(lang))
                lang = "ar";


            EmployeesDto user = await this.Request.GetRawBodyStringAsync<EmployeesDto>();

            Expression<Func<Employees, bool>> expression = (u => u.IsDeleted == false && u.MobileNo == user.MobileNo
            && u.ConfirmationCode == user.ConfirmationCode);
            var _users = await _employeesService.All(true, expression);
            var _user = _users.FirstOrDefault();
            if (_user != null)
            {
                _user.Password = user.Password;
                _user.ConfirmationCode = user.ConfirmationCode;
                _employeesService.Update(_user);
                var AccessToken = GenerateJWTAccessToken(_user);
                return Ok(new { accessToken = AccessToken });
            }
            else
            {
                string message = ExceptionMessage.ResourceManager.GetString("UpdatePassword." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("SecurityCode." + lang);
                throw new CustomException(HttpStatusCode.Forbidden, message, subMessage);
            }
        }
        public string GenerateRandomCode(int len)
        {
            Random rand = new Random();
            char[] allowableChars = "0123456789".ToCharArray();
            string final = String.Empty;
            for (int i = 0; (i
                        <= (len - 1)); i++)
            {
                final = (final + allowableChars[rand.Next((allowableChars.Length - 1))]);
            }

            return final;
        }

        private IEnumerable<Claim> GetUserClaims(EmployeesDto user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.Name, string.Format("{0} {1}", user.FirstName, user.LastName));
            claims.Add(_claim);
            _claim = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
            claims.Add(_claim);
            _claim = new Claim(ClaimTypes.MobilePhone, user.MobileNo.ToString());
            claims.Add(_claim);
            return claims.AsEnumerable<Claim>();
        }
        private string GenerateJWTAccessToken(EmployeesDto user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecurityKey"]);
            //Generate Token for user - JRozario
            var JWToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],//"http:localhost:54655/api/",
                audience: _configuration["Jwt:Audience"],//"http:localhost:54655/api/",
                claims: GetUserClaims(user),
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddYears(1)).DateTime,
                //Using HS256 Algorithm to encrypt Token - JRozario
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            var AccessToken = new JwtSecurityTokenHandler().WriteToken(JWToken);
            return AccessToken;
        }

        [Route("UpdateToken")]
        [HttpPost]
        public async Task<IActionResult> UpdateToken()
        {
            string lang = this.Request.Headers["language"].ToString();

            if (string.IsNullOrEmpty(lang))
                lang = "ar";

            EmployeesDto user = await this.Request.GetRawBodyStringAsync<EmployeesDto>();

            if (user == null)
            {
                string message = ExceptionMessage.ResourceManager.GetString("UpdateToken." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("UserNotExsit." + lang);

                throw new CustomException(HttpStatusCode.Unauthorized, message, subMessage);
            }

            //string Id = CurrentUser.GetUserId(this.User, HttpContext.Session.GetString("JWToken") ?? "");
            var _user = (await _employeesService.All(true, a => a.NationalId == user.NationalId && a.Password == user.Password)).FirstOrDefault();

            if (_user != null)
            {
                var AccessToken = GenerateJWTAccessToken(_user);
                return Ok(new { accessToken = AccessToken });
            }
            else
            {
                string message = ExceptionMessage.ResourceManager.GetString("UpdateToken." + lang);
                string subMessage = ExceptionMessage.ResourceManager.GetString("UserNotExsit." + lang);

                throw new CustomException(HttpStatusCode.Forbidden, message, subMessage);
            }
        }


        private async Task<string> AddClaim(EmployeesDto user, string lang)
        {
            CurrentUserInfo currentUser = new CurrentUserInfo()
            {
                UserId = user.Id.ToString(),
                UserName = user.NationalId,
                CompanyId = 1,
                Language = lang,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            await currentUser.RefreshClaims(true, userName: user.NationalId);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Generate Token for user 
            var JWToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: CreateClaims(currentUser),
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddMinutes(90)).DateTime,
                signingCredentials: creds
            );
            var userToken = new JwtSecurityTokenHandler().WriteToken(JWToken);
            HttpContext.Session.SetString("JWToken", userToken);

            if (userToken != null)
                return userToken;

            return string.Empty;
        }
        private IEnumerable<Claim> CreateClaims(CurrentUserInfo user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString());
            claims.Add(_claim);
            _claim = new Claim(ClaimTypes.Name, user.UserName);
            claims.Add(_claim);
            _claim = new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user));
            claims.Add(_claim);
            return claims.AsEnumerable<Claim>();
        }

    }
}
