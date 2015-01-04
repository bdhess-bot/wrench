using System;
using System.Collections.Generic;
using System.Linq;

namespace BrowningStyle.Tests
{
    class TestCollateral
    {
        /// <summary>
        /// A test case that should fail since it does not end with a new line.
        /// </summary>
        internal const string FailureEndingNewLine = @"
using System;

namespace BrowningStyle.Tests.Collateral
{
    class EndingNewLineFailureCase
    {
    }
}";

        /// <summary>
        /// A test case that should fail due to trailing white space.
        /// </summary>
        internal const string FailureTrailingWhiteSpace = @"
using System;

namespace Collateral
{
    class TrailingWhiteSpaceFailureCase   
    {
 
    }
            
}
";

        /// <summary>
        /// A test case that should fail since it uses contextual keyword <code>var</code>.
        /// </summary>
        internal const string FailureUseOfVar = @"
using System;

namespace Collateral
{
    class UseOfVarFailureCase
    {
        static void Main()
        {
            var x = 3;
            Console.WriteLine(x);
        }
    }
}
";

        /// <summary>
        /// A test case that should succeed with all rules.
        /// </summary>
        internal const string Success = @"
using System;

namespace Collateral
{
    class SuccessCase
    {
        static void Main()
        {
            int var = 3;
            Console.WriteLine(var);
        }
    }
}
";

        /// <summary>
        /// A StyleCop settings file.
        /// </summary>
        internal const string Settings = @"
<?xml version=""1.0"" encoding=""utf-8"" ?>
<StyleCopSettings Version=""105"">
</StyleCopSettings>";

    }
}
