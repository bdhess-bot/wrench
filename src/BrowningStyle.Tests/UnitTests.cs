using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BrowningStyle.Tests
{
    public class UnitTests
    {
        private readonly bool theTruth = true;

        [Fact]
        public void Fact()
        {
            Assert.True(this.theTruth);
        }
    }
}
