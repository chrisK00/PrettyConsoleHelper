using FluentAssertions;
using System;
using System.Linq;
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

        [Fact]
        public void ParseOptions_DoesNotReturn_Options_When_ThereAreNoValues()
        {
            string[] options = { "-title", "-completed" };
            var result = _subject.ParseOptions(options, options);
            result.Count.Should().Be(0);
        }

        [Fact]
        public void ParseOptions_Returns_OptionsWithValues()
        {
            string[] inputsWithoutOptions = { "clean room", "yes" };
            string[] options = { "-title", "-completed" };
            string[] inputs = { options[0], inputsWithoutOptions[0], options[1], inputsWithoutOptions[1] };

            var result = _subject.ParseOptions(options, inputs);

            result.Count.Should().Be(options.Length);

            result[options[0]].Should().NotBeEmpty();
            result[options[1]].Should().NotBeEmpty();

            result[options[0]].Should().Be(inputsWithoutOptions[0]);
            result[options[1]].Should().Be(inputsWithoutOptions[1]);
        }

        [Fact]
        public void ParseOptions_Returns_Options_Without_PrefixAndToTitleCase()
        {
            string[] inputsWithoutOptions = { "clean room", "yep" };
            string[] options = { "-title", "-completed" };
            string[] inputs = { options[0], inputsWithoutOptions[0], options[1], inputsWithoutOptions[1]};

            var result = _subject.ParseOptions(options, inputs, "-");

            result.Count.Should().Be(options.Length);

            result.Keys.Should().Contain("Title");
        }
    }
}
