using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class World
    {
        public Int32 Height { get; set; }
        public Int32 Width { get; set; }
        public Farm Farm { get; set; }
    }
}
