using System;
using FluentAssertions;
using Xunit;


namespace PrettyConsoleOutput.Tests
{
    public class InputHelperTests
    {
        [Fact]
        public void GetEnumInput_ThrowsArgumentException_When_SentInType_IsNotAnEnum()
        {
            FluentActions.Invoking(() => InputHelper.GetEnumInput<int>())
                .Should().ThrowExactly<ArgumentException>();
        }
    }
}
