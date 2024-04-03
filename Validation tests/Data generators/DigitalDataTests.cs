using Digital_data.Email;
using Digital_data.Usernames;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class DigitalDataTests(ITestOutputHelper testOutputHelper)
{
    // Test email generator
    [Fact]
    public void TestEmailGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var email = EmailGenerator.GenerateEmail();
            var divided = email.Split('@');
            Assert.NotNull(email);
            Assert.NotEmpty(email);
            Assert.Contains('@', email);
            Assert.Contains('.', email);
            Assert.True(divided[0].Length <= 255);
            Assert.True(divided[1].Length <= 64);
            testOutputHelper.WriteLine(email);
        }
    }

    // Test username generator
    [Fact]
    public void TestUsernameGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var username = UsernamesGenerator.GenerateUsername();
            Assert.NotNull(username);
            Assert.NotEmpty(username);
            Assert.True(username.Length is >= 6 and <= 30);
            testOutputHelper.WriteLine(username);
        }
    }
}