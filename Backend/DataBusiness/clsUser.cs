using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{

    public class clsUser
    {
        public static UserDTO GetUserByUsername(string username)
        {
            return clsUserData.GetUserByUsername(username);
        }

        public static UserDTO GetUserById(int id)
        {
            return clsUserData.GetUserById(id);
        }

        public static bool Login(UserDTO UserDTO)
        {
            var user = clsUser.GetUserByUsername(UserDTO.Username);

            if (user != null)
            {
                if (user.Password == clsUtil.ComputeHash(UserDTO.Password))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
