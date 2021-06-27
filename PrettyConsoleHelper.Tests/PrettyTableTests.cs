using System;
using FluentAssertions;
using Xunit;

namespace PrettyConsoleHelper.Tests
{
   public record Person(int Id, string Name, int Age);
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

        [Fact]
        public void GetHeaders_ReturnsCorrectHeaders()
        {
            string[] headers = { "id", "name" };
            var table = new PrettyTable()
                .AddHeaders(headers);

            table.Headers.Should().ContainAll(headers);
        }

        [Fact]
        public void AddDefaultHeaders_Adds_ClassPropertyNames_To_Headers()
        {
            var table = new PrettyTable()
                .AddDefaultHeaders<Person>();

            table.Headers.Should().ContainAll(nameof(Person.Name), nameof(Person.Age));
        }
    }
}
