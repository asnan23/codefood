
using CodeFood_API.Asnan.Enum;
using CodeFood_API.Asnan.Models;
using CodeFood_API.Asnan.Models.Dto;
using CodeFood_API.Asnan.Models.Response;
using CodeFood_API.Asnan.Models.Setting;
using CodeFood_API.Asnan.Models.ViewModel;
using CodeFood_API.Asnan.Repository;
using CodeFood_API.Asnan.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Controllers
{
    [AllowAnonymous]
    [Route("/auth")]
    public class authController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        protected SuccessDTO _success;
        protected ErrorDTO _error;
        private readonly JWTSettings _jwtSettings;
        private readonly IValidationErrorRepository _validationErrorRepository;
        private readonly IUnitOfWork _unitOfWork;
        public authController(UserManager<ApplicationUser> userManager, 
            IOptions<JWTSettings> jwtSettings, SignInManager<ApplicationUser> signInManager,
            IValidationErrorRepository validationErrorRepository, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._success = new SuccessDTO();
            this._error = new ErrorDTO();
            _jwtSettings = jwtSettings.Value;
            _validationErrorRepository = validationErrorRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var validasi = _validationErrorRepository.Validate(ModelState);
                    _error.message = validasi;
                    return BadRequest(_error);
                }

                var user = new ApplicationUser
                {
                    Email = model.username,
                    UserName = model.username.Split('@')[0]
                   
                };

                var userWithSameEmail = await _userManager.FindByEmailAsync(user.Email);
                if (userWithSameEmail == null)
                {
                    var result = await _userManager.CreateAsync(user, model.password);
                    if (!result.Succeeded)
                    {
                        var res = "";
                        foreach (var error in result.Errors)
                        {
                            res += error.Description;
                        }
                        _error.message = res;
                        return BadRequest(_error);
                    }
                    var userLogin = await _userManager.FindByEmailAsync(user.Email);
                    var masterUser = new MasterUser { userId = userLogin.Id, email = userLogin.Email };
                    _unitOfWork.MasterUser.Add(masterUser);
                    _unitOfWork.Save();

                    await _userManager.AddToRoleAsync(user, Roles.User);
                    
                    _success.message = "Success";
                    _success.data = new { id = masterUser.id, username = masterUser.email };
                    return Ok(_success);
                }
                else
                {
                    _error.message = $"username '{model.username}' already registered";
                    return BadRequest(_error);
                }
            }
            catch (Exception e)
            {
                _error.message = e.Message;
                return StatusCode(500, _error);
            } 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var validasi = _validationErrorRepository.Validate(ModelState);
                    _error.message = validasi;
                    return BadRequest(_error);
                }

                var user = await _userManager.FindByEmailAsync(request.username);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, request.password, false, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        var masterUser = _unitOfWork.MasterUser.GetFirstOrDefault(t => t.userId == user.Id);
                        int ExpiredTokenMinutes = Convert.ToInt32(_jwtSettings.DurationInMinutes);
                        //create claims details based on the user information
                        var claims = new[] {
                        new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, _jwtSettings.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserId", masterUser.id.ToString()),
                        new Claim("Email", user.Email),
                        new Claim(ClaimTypes.Name, user.UserName)
                       };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, expires: DateTime.UtcNow.AddMinutes(ExpiredTokenMinutes), signingCredentials: signIn);
                        _success.data = new { token = new JwtSecurityTokenHandler().WriteToken(token) };
                        _success.message = "Success";
                        return Ok(_success);
                    }
                    else
                    {
                        if (result.IsLockedOut)
                        {
                            _error.message = "Too many invalid login, please wait for 1 minute";
                            return StatusCode(403, _error);
                        }
                        else
                        {
                            _error.message = "Invalid username or Password";
                            return StatusCode(401,_error);
                        }
                        
                    }
                }
                else
                {
                    _error.message = "Invalid username or Password";
                    return StatusCode(401, _error);
                }
            }
            catch (Exception e)
            {
                _error.message = e.Message;
                return StatusCode(500, _error);
            }
        }

    }
}
