using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Common.HttpHelper
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UseTranAttribute:Attribute
    {

    }
}
