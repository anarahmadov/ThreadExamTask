using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadExamTask.Domain
{

    public class CustomFile 
    {
        public DateTime CreationTime { get; set; }
        public long Lenght { get; set; }
        public string Fullname { get; set; }
        public string Name { get; set; }

    }
}
