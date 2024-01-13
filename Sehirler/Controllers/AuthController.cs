using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sehirler.Data;
using Sehirler.Dtos;
using Sehirler.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sehirler.Controllers
{
	[ApiController]
	[Route("api/Auth")]
	public class AuthController : Controller
	{
		IAuthRepository _Autorepository;
		IConfiguration _configuration;

		

		public AuthController(IAuthRepository repository, IConfiguration configuration)
        {
			_Autorepository = repository;
			_configuration = configuration;
		}
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto) // Burda Bir Kaydolma Actionu aldık
		{
			if (await _Autorepository.UserExists(userForRegisterDto.UserName))// Burda DAha önceden yazdığımız UserExist MEtodunu ÇAğırarak Daha Önceden Giriş Yapıldımı Diye kontrol ettik
			{
				ModelState.AddModelError("UserName", "UserName Aleady exists");
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var userToCreate = new User// Burda User Nesnesini Çağırarak usur name atama işlemi yaptık 
			{
				Username = userForRegisterDto.UserName
			};

			var createdUser = await _Autorepository.Register(userToCreate, userForRegisterDto.Password); // Burda da oluşturduk

			return StatusCode(201);

		}
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto )
		{
			var user = await _Autorepository.Login(userForLoginDto.UserName, userForLoginDto.Password);
			
			if (user==null)
			{
				return Unauthorized();
			}
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
					new Claim(ClaimTypes.Name,user.Username)
				}),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)

			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenstring = tokenHandler.WriteToken(token);

			return Ok(tokenstring);
		}

    }
}
