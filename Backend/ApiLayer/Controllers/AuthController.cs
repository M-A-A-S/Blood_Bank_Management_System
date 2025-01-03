using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController(JwtOptions jwtOptions) : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login(UserDTO userDTO)
        {
            if (!clsUser.Login(userDTO))
            {
                return Unauthorized("Invalid username or password");
            }

            var accessToken = clsUtil.GenerateAccessToken(userDTO, jwtOptions);
            var refreshToken = clsUtil.GenerateRefreshToken(clsUser.GetUserByUsername(userDTO.Username).Id);
            clsRefreshToken.SaveRefreshToken(refreshToken);
            return Ok( new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("refresh")]
        public IActionResult Refresh(string refreshToken)
        {
            var storedToken = clsRefreshToken.GetRefreshToken(refreshToken);
            if (refreshToken == null)
            {
                return Unauthorized("Invalid refresh token");
            }
            //clsRefreshToken.SaveRefreshToken(storedToken);
            var user = clsUser.GetUserById(storedToken.UserId);
            var newAccessToken = clsUtil.GenerateAccessToken(user, jwtOptions);
            return Ok(new { AccessToken = newAccessToken});
        }
    }
}
