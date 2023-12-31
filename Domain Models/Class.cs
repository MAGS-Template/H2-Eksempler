using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Models
{
    public class Class : Common
    {
        public string ClassName { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }
    }
}
