using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{

    public class clsRefreshToken
    {
        public static void SaveRefreshToken(RefreshTokenDTO RefreshTokenDTO)
        {
            bool isRefreshTokenForUser = clsRefreshToken.IsRefreshTokenForUser(RefreshTokenDTO.UserId);
            if (!isRefreshTokenForUser)
            {
                clsRefreshTokenData.AddNewRefreshToken(RefreshTokenDTO);
            }

            clsRefreshTokenData.UpdateRefreshToken(RefreshTokenDTO);
        }

        public static RefreshTokenDTO GetRefreshToken(string token)
        {
            return clsRefreshTokenData.GetRefreshToken(token);
        }

        public static bool IsRefreshTokenForUser(int userId)
        {
            return clsRefreshTokenData.IsRefreshTokenForUser(userId);
        }
    }
}
