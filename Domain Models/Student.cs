using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Models
{
    public class Student : Common
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Class> Classes { get; set; }
    }
}
