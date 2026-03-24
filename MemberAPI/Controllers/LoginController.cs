using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MemberAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
namespace MemberAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ChurchRepository _repo;
    public LoginController(IConfiguration configuration,ChurchRepository repo)
    {
        _configuration = configuration;
        _repo = repo;
    }

    [HttpPost, Route("login")]
    public IActionResult Login(string phoneNo, string password)
    {   
        
        try
        {
            if(string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(password))
                return BadRequest("Username and password not specified");

            else if(!string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(phoneNo))
            {
                if(_repo.GetPassword(password)==null || _repo.GetPhoneNumber(phoneNo) == null){
                    return Unauthorized("Incorrect Phone Number or Password");
                }

                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secret = jwtSettings["SecretKey"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];
                var expiryMinutes = int.TryParse(jwtSettings["ExpiryMinutes"], out var minutes) ? minutes : 60;

                if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
                {
                    return StatusCode(500, "JWT settings are missing in configuration");
                }

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { token = tokenString });
            }
        
            
        }
        catch
        {
            return BadRequest("An error occured");
        }
        return Unauthorized();
    }
    
}
