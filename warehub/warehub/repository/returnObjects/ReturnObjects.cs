using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehub.repository.returnObjects
{
    public class GenericResponseDTO<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        // Optional metadata property
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        public GenericResponseDTO(T data, bool isSuccess = true, string message = "")
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
