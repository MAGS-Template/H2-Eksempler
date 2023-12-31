using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Models
{
    public class Department : Common
    {
        public string DepartmentName { get; set; }

        public List<Teacher> Teachers { get; set; } 
    }
}
