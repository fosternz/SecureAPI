using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecureAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    public IConfiguration _config { get; }

    public record AuthenticationData(string? UserName, string? Password);
    public record UserData(int Id, string Name, string Title, string StaffId);

    public AuthenticationController(IConfiguration config)
    {
        _config = config;
    }       


    [HttpPost("token")]
    [AllowAnonymous]
    public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
    {
        var user = ValidateCredentials(data);

        if (user == null)
        {
            return Unauthorized();
        }

        var token = GenerateToken(user);

        return Ok(token);

    }

    private string GenerateToken(UserData user)
    {
        var secretKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey")));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.Name.ToString()));
        claims.Add(new("title", user.Title));
        claims.Add(new("staffId", user.StaffId));

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow, // Time token is valid
            DateTime.UtcNow.AddMinutes(60), // When token expires
            signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private UserData? ValidateCredentials(AuthenticationData data)
    {
        // This is just a demo - don't do this for prod!

        if (CompareValues(data.UserName, "Luke") && CompareValues(data.Password, "test123"))
        {
            return new UserData(1, data.UserName!, "Engineer", "S01");
        }

        return null;

    }

    private bool CompareValues(string? actual, string expected)
    {
        if (actual != null)
        {
            if(actual.Equals(expected))
            {
                return true;
            }
        }

        return false;
    }
}
