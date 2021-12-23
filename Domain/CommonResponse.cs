using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public class CommonResponse<T>
    {
        public int status { get; set; }
        public string message { get; set; }

     
    }
}
