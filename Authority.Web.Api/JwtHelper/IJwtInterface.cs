using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Web.Api.JwtHelper
{
   public interface IJwtInterface
    {

        string IssueJwt(TokenModelJwt tokenModel);



        TokenModelJwt SerializeJwt(string jwtStr);

    }
}
