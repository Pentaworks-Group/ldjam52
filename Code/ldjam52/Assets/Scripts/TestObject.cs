using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class TestObject
    {
        public TestObject(DateTime timestamp)
        {
            this.Timestamp = timestamp;
        }

        public DateTime Timestamp { get; set; }
    }
}
