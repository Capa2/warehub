using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehub.repository.returnObjects
{
    public class GenericResponseDTO<T>(T data, bool isSuccess = true)
    {
        public bool IsSuccess { get; set; } = isSuccess;
        public T Data { get; set; } = data;
    }
}
