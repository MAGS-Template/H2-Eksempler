using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Models
{
    public class Teacher : Common
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Department Department { get; set; }
        public List<Class> Classes { get; set; }
    }
}
