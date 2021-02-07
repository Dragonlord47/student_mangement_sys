using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiometricIdentificationSystem.Classes
{
    class User
    {
        public int SN { get; set; }

        public string UserID { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string AdminType { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
