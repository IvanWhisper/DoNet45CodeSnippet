using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            new NettyService.NettyClient().RunClientAsync().Wait();
            
        }
    }
}
