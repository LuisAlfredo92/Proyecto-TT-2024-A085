using System.Globalization;
using Patrimony_data.Card_number;
using Patrimony_data.Cvv;
using Patrimony_data.Emission_date;
using Patrimony_data.Expiration_date;
using Patrimony_data.Investment_return;
using Patrimony_data.Policies;
using Patrimony_data.Salary;
using Xunit.Abstractions;

namespace Validation_tests.Data_generators;

public class PatrimonyDataTests(ITestOutputHelper testOutputHelper)
{
    // Test salary generator
    [Fact]
    public void TestSalaryGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var salary = SalaryGenerator.GenerateSalary();
            Assert.True(salary > 0);
            testOutputHelper.WriteLine(salary.ToString("C"));
        }
    }

    // Test card number generator
    [Fact]
    public void TestCardNumberGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var cardNumber = CardNumberGenerator.GenerateCardNumber();
            Assert.True(cardNumber is >= 1000000000000000 and <= 9999999999999999);
            testOutputHelper.WriteLine(cardNumber.ToString(CultureInfo.InvariantCulture));
        }
    }

    // Test expiration date generator
    [Fact]
    public void TestExpirationDateGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var expirationDate = ExpirationDateGenerator.GenerateExpirationDate();
            Assert.True(expirationDate >= DateTime.Now);
            testOutputHelper.WriteLine(expirationDate.ToString("MM/yy"));
        }
    }

    // Test CVV generator
    [Fact]
    public void TestCvvGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var cvv = CvvGenerator.GenerateCvv();
            Assert.True(cvv is >= 100 and <= 999);
            testOutputHelper.WriteLine(cvv.ToString(CultureInfo.InvariantCulture));
        }
    }

    // Test emission date generator
    [Fact]
    public void TestEmissionDateGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var emissionDate = EmissionDateGenerator.GenerateEmissionDate();
            Assert.True(emissionDate <= DateTime.Now && emissionDate >= DateTime.Now.AddYears(-7));
            testOutputHelper.WriteLine(emissionDate.ToString("dd/MM/yyyy"));
        }
    }

    // Test investment return generator
    [Fact]
    public void TestInvestmentReturnGenerator()
    {
        for (var i = 0; i < 100; i++)
        {
            var investmentReturn = InvestmentReturnGenerator.GenerateInvestmentReturn();
            Assert.True(investmentReturn is >= 0 and <= 100);
            testOutputHelper.WriteLine(investmentReturn.ToString(CultureInfo.InvariantCulture));
        }
    }

    // Test policies generator
    [Fact]
    public void TestPoliciesGenerator()
    {
        var policiesGenerator = new PoliciesGenerator();
        var filePath = policiesGenerator.GeneratePolicy();
        testOutputHelper.WriteLine(filePath);
    }
}