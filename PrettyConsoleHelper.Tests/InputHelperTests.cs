using System;
using FluentAssertions;
using Xunit;


namespace PrettyConsoleHelper.Tests
{
    public enum Season
    {
        Summer
    }

    public class InputHelperTests
    {
        readonly MockPrettyConsole _console = new MockPrettyConsole();
        readonly InputHelper _subject;

        public InputHelperTests()
        {
            _subject = new InputHelper(_console);
        }

        [Fact]
        public void GetEnumInput_ThrowsArgumentException_When_SentInType_IsNotAnEnum()
        {
            FluentActions.Invoking(() => _subject.GetEnumInput<int>())
                .Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void GetEnumInput_IsCaseInsensetive()
        {
            _console.ReturnValue = "summer";
            var result = _subject.GetEnumInput<Season>();
            result.Should().BeEquivalentTo(Season.Summer);
        }

        [Fact]
        public void GetIntInput_ReturnsParsedInput_WhenInValidRange()
        {
            int returnValue = 4;
            _console.ReturnValue = returnValue.ToString();
            var result = _subject.GetIntInput(maxValue: 5, minValue: 2);

            result.Should().BeOfType(typeof(int));
            result.Should().Be(returnValue);
        }
    }
}
