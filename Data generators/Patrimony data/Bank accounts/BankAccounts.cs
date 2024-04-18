using System.Text;

namespace Patrimony_data.Bank_accounts;

public class BankAccounts
{
    public static string GenerateBankAccount()
    {
        var length = Random.Shared.Next(1, 21);
        var bankAccount = new StringBuilder();
        for (var i = 0; i < length; i++)
            bankAccount.Append(Random.Shared.Next(0, 10));
        return bankAccount.ToString().PadLeft(20, '0');
    }
}