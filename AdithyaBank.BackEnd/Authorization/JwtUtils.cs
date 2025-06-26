namespace AdithyaBank.BackEnd.Authorization;

using AdithyaBank.BackEnd.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IJwtUtils
{
    public string GenerateJwtToken(UserModel user);
    public ApiBaseResultModel ValidateJwtToken(string token);
}

public class JwtUtils : IJwtUtils
{
    private readonly AppSettings _appSettings;

    public JwtUtils(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public string GenerateJwtToken(UserModel user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JWT.Secret);
        int age = DateTime.Today.Year - user.Dob.Year;
        if (user.Dob.Date > DateTime.Today.AddYears(-age)) age--;

        // Create a list of claims
        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("Age",age.ToString()),
            new Claim("role", user.Role.ToString()),
            new Claim("isSuccess", user.IsSuccess.ToString()),
            new Claim("message",user.Message)
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            //Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ApiBaseResultModel ValidateJwtToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JWT.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

            // return user id from JWT token if validation successful
            return new ApiBaseResultModel() { Id = userId,Data = jwtToken,IsSuccess = true,Message=""};
        }
        catch(Exception ex)
        {
            return new ApiBaseResultModel() { Id = null, IsSuccess = false, Message = ex.Message, StatusCode = ex.Message };
        }
    }
}