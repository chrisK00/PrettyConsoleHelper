using System;
using FluentAssertions;
using Xunit;

namespace PrettyConsoleOutput.Tests
{
    public class PrettyTableTests
    {
        [Theory]
        [InlineData("1")]
        [InlineData("1", "2", "3")]
        public void AddRow_ThrowsArgumentException_When_RowLength_IsNotEqualTo_HeadersLength(params string[] row)
        {
            var subject = new PrettyTable("id", "name");

            subject.Invoking(_ => _.AddRow(row))
                .Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void RowCount_ReturnsCorrectAmountOfRows()
        {
            var subject = new PrettyTable("id", "name");
            subject.AddRow("1", "chris");
            subject.AddRow("2", "chrisk");

            subject.RowCount.Should().Be(2);
        }
    }
}
