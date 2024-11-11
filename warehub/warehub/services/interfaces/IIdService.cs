using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehub.services.interfaces
{
    public interface IIdService
    {
        Guid GenerateId();
    }
}
