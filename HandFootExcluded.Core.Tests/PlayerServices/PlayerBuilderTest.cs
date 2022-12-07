using HandFootExcluded.Common;
using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.Tests.PlayerServices;

public class PlayerBuilderTest
{
    [Fact]
    public void Initialize()
    {
        var builder = BuildPlayerBuilder();

        Assert.IsAssignableFrom<INonPositionalPlayerBuilder>(builder);
    }

    [Theory]
    [InlineData("John Jacob Smith", "John", "Jacob", "Smith", "JJS")]
    [InlineData("John J Smith", "John", "J", "Smith", "JJS")]
    [InlineData("John J. Smith", "John", "J.", "Smith", "JJS")]
    [InlineData("John Smith", "John", "", "Smith", "JS")]
    [InlineData(" John   Jacob      Smith  ", "John", "Jacob", "Smith", "JJS")]
    [InlineData("  John  J      Smith   ", "John", "J", "Smith", "JJS")]
    [InlineData("   John    J.  Smith    ", "John", "J.", "Smith", "JJS")]
    [InlineData("    John      Smith      ", "John", "", "Smith", "JS")]
    public void Valid_WithFullName_ReturnsNonPositionalPlayer(string fullName, string expectedFirstName, string expectedMiddleName, string expectedLastName, string expectedInitials)
    {
        var builder = BuildPlayerBuilder();

        var result = builder
                    .WithFullName(fullName)
                    .Build();

        Assert.IsAssignableFrom<INonPositionalPlayer>(result);
        Assert.Equal(expectedFirstName, result.FirstName);
        Assert.Equal(expectedMiddleName, result.MiddleName);
        Assert.Equal(expectedLastName, result.LastName);
        Assert.Equal(expectedInitials, result.Initials);
        Assert.Equal(fullName.Trim().RemoveMultipleSpaces(), result.FullName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("John")]
    [InlineData(null)]
    public void Invalid_WithFullName_ReturnsUnknownPlayer(string fullName)
    {
        var builder = BuildPlayerBuilder();

        var result = builder.WithFullName(fullName)
                            .Build();

        Assert.IsType<UnknownPlayer>(result);
    }

    [Theory]
    [InlineData("John", "Smith", "John Smith", "JS")]
    [InlineData("   John    ", "    Smith        ", "John Smith", "JS")]
    public void Valid_WithFirstName_WithLastName_ReturnsNonPositionalPlayer(string firstName, string lastName, string expectedFullName, string expectedInitials)
    {
        var builder = BuildPlayerBuilder();

        var result = builder.WithFirstName(firstName)
                            .WithLastName(lastName)
                            .Build();

        Assert.IsAssignableFrom<INonPositionalPlayer>(result);
        Assert.Empty(result.MiddleName);
        Assert.Equal(expectedInitials, result.Initials);
        Assert.Equal(expectedFullName, result.FullName);
        Assert.Equal(firstName.Trim().RemoveMultipleSpaces(), result.FirstName);
        Assert.Equal(lastName.Trim().RemoveMultipleSpaces(), result.LastName);
    }

    [Theory]
    [InlineData("John", "")]
    [InlineData("", "Smith")]
    [InlineData("John", "   ")]
    [InlineData("   ", "Smith")]
    [InlineData("John", null)]
    [InlineData(null, "Smith")]
    public void Invalid_WithFirstName_WithLastName_ReturnsUnknownPlayer(string firstName, string lastName)
    {
        var builder = BuildPlayerBuilder();

        var result = builder.WithFirstName(firstName)
                            .WithLastName(lastName)
                            .Build();

        Assert.IsType<UnknownPlayer>(result);
    }

    [Theory]
    [InlineData("John", "Jacob", "Smith", "John Jacob Smith", "JJS")]
    [InlineData("   John    ", "         Jacob    ", "    Smith        ", "John Jacob Smith", "JJS")]
    [InlineData("John", "J", "Smith", "John J Smith", "JJS")]
    [InlineData("   John    ", "         J    ", "    Smith        ", "John J Smith", "JJS")]
    [InlineData("John", "J.", "Smith", "John J. Smith", "JJS")]
    [InlineData("   John    ", "         J.    ", "    Smith        ", "John J. Smith", "JJS")]
    [InlineData("John", "", "Smith", "John Smith", "JS")]
    [InlineData("John", "   ", "Smith", "John Smith", "JS")]
    [InlineData("John", null, "Smith", "John Smith", "JS")]
    public void Valid_WithFirstName_WithMiddleName_WithLastName_ReturnsNonPositionalPlayer(string firstName, string middleName, string lastName, string expectedFullName, string expectedInitials)
    {
        var builder = BuildPlayerBuilder();

        var result = builder.WithFirstName(firstName)
                            .WithMiddleName(middleName)
                            .WithLastName(lastName)
                            .Build();

        Assert.IsAssignableFrom<INonPositionalPlayer>(result);
        Assert.Equal(expectedInitials, result.Initials);
        Assert.Equal(expectedFullName, result.FullName);
        Assert.Equal(firstName.Trim().RemoveMultipleSpaces(), result.FirstName);
        Assert.Equal(middleName?.Trim().RemoveMultipleSpaces() ?? string.Empty, result.MiddleName);
        Assert.Equal(lastName.Trim().RemoveMultipleSpaces(), result.LastName);
    }

    [Theory]
    [InlineData("John", "Jacob", "")]
    [InlineData("", "Jacob", "Smith")]
    [InlineData("John", "Jacob", "   ")]
    [InlineData("   ", "Jacob", "Smith")]
    [InlineData("John", "Jacob", null)]
    [InlineData(null, "Jacob", "Smith")]
    public void Invalid_WithFirstName_WithMiddleName_WithLastName_ReturnsUnknownPlayer(string firstName, string middleName, string lastName)
    {
        var builder = BuildPlayerBuilder();

        var result = builder.WithFirstName(firstName)
                            .WithMiddleName(middleName)
                            .WithLastName(lastName)
                            .Build();

        Assert.IsType<UnknownPlayer>(result);
    }

    private static INonPositionalPlayerBuilder BuildPlayerBuilder() => new NonPositionalPlayerBuilder();
}