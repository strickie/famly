using FamlyCal.OutputFormatters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace FamlyCal.UnitTests.Tests
{
    public class DummyTest
    {
        private readonly ITestOutputHelper output;

        public DummyTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Dummy()
        {
            string str = VCalendarOutputFormatter.EscapeStrings("Børnehuset ved Damhussøen(Personale), Børnehuset ved Damhussøen(Børn)");

            var t = str.Length;

            output.WriteLine(str);
        }

    }
}
