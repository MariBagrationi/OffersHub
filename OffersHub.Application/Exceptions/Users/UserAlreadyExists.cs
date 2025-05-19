using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersHub.Application.Exceptions.Users
{
    public class UserAlreadyExists : Exception
    {
        public UserAlreadyExists(string message) : base(message) { }
    }
}
