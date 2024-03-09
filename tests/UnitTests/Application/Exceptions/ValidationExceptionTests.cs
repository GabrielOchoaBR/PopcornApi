using Application.Exceptions;
using AutoFixture;
using FluentAssertions;

namespace UnitTests.Application.Exceptions
{
    public class ValidationExceptionTests : BaseTests
    {
        [Fact]
        public void ValidationException_ThrowNewException_ReturnsOk()
        {
            //Arrange
            Random random = new();
            var dictionary = fixture.CreateMany<string>(5)
                                    .Select(x => new { Key = x, Values = fixture.CreateMany<string>(random.Next(1, 5)).ToArray() })
                                    .ToDictionary(x => x.Key, x => x.Values);

            //Act
            try
            {
                throw new ValidationException(dictionary);
            }

            //Assert
            catch (ValidationException ex)
            {
                ex.Title.Should().Be("Validation Failure");
                ex.Message.Should().Be("One or more validation errors occurred");

                ex.ErrorsDictionary.Count.Should().Be(dictionary.Count);

                var randomItem = dictionary.Keys.ToArray()[random.Next(1, 5)];
                dictionary[randomItem!].Should().NotBeNull();
                ex.ErrorsDictionary[randomItem!].Should().NotBeNull();
            }
        }
    }
}
