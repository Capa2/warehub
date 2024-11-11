using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.services.interfaces;

namespace warehub.services
{
    internal class GuidService : IIdService
    {
        public Guid GenerateId()
        {
            return Guid.NewGuid();
        }
    }
}
