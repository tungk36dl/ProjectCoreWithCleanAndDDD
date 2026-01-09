using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Application.UseCases.Users.Queries.GetUserByUserNameOrEmail
{
    public class GetUserByUserNameOrEmailQuery
    {
        public string UserNameOrEmail { get; }
        public GetUserByUserNameOrEmailQuery(string userNameOrEmail)
        {
            UserNameOrEmail = userNameOrEmail;
        }
    }
}
