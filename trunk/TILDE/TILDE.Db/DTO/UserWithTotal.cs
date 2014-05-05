using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TILDE.Db.DTO
{
    public class UserWithTotal
    {
        public User User { get; set; }
        public int Total { get; set; }
    }
}
