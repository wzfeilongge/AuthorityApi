using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Applicaion.ViewModel
{
    public class UserRegisterModel
    {
        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string OtherPassword { get; set; }

        public int State { get; set; } = 1;
    }
}
