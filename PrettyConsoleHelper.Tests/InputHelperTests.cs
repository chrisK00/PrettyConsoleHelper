using FluentAssertions;
using System;
using Xunit;


namespace PrettyConsoleHelper.Tests
{
    public enum Season
    {
        Summer,
        Winter
    }

    public class InputHelperTests
    {
        readonly MockPrettyConsole _console = new();
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
            _console.ReturnValue = Season.Summer.ToString().ToLower();
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

        [Fact]
        public void GetDateTime_ReturnsParesedInput_WhenValidRange()
        {
            DateTime dateTime = new(2022, 9, 10);

            _console.ReturnValue = dateTime.ToString();

            var result = _subject.GetDateTime(minDateTime: new DateTime(2022, 1, 1), maxDateTime: new DateTime(2022, 12, 12));

            result.Should().Be(dateTime);
        }

        [Fact]
        public void GetDateTime_ThrowsArgumentException_When_InvalidRange()
        {
            _subject.Invoking(_ => _.GetDateTime(
               minDateTime: new DateTime(2022, 12, 12), maxDateTime: new DateTime(2022, 1, 1)))
              .Should().ThrowExactly<ArgumentException>();
        }
    }
}
