using TeamsBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsBot.Logic
{
    public class UseUserInformation
    {
        public void CreateNewUserEntry()
        {
            var userInformation = new GetUserInformation();
            var userInformationList = userInformation.UserInformation();

            string userId = userInformationList.ID;
            string userName = userInformationList.Name;

            
        }
    }
}
