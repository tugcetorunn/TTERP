using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Domain.Interfaces.ServiceInterfaces
{
    public interface IJWTService
    {
        string GenerateToken(Employee user, IList<string> roles);
    }
}
