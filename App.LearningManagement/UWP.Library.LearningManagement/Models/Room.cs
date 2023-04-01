using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Models
{
    public class Room
    {
        public string Name { get; set; }
        public string Building { get; set; }

        public override string ToString()
        {
            return $"{Building}_{Name}";
        }
    }
}
