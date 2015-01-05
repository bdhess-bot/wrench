using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wrench.BrowningStyle.Tests.Collateral
{
    class UseOfVarFailureCase
    {
        static void Main()
        {
            var x = "This is a test";
            Console.WriteLine(x);
        }

        static void Main2()
        {
            int x = 3;
            var y = x + 1;
            Console.WriteLine(y);
        }
    }
}
