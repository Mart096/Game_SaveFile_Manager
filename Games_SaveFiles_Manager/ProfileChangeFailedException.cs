using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Games_SaveFiles_Manager
{
    class ProfileChangeFailedException : Exception
    {
        public ProfileChangeFailedException()
        {

        }

        public ProfileChangeFailedException(string message) : base(message)
        {

        }
    }
}
