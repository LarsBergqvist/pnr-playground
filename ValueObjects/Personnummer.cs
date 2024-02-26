using System;
using System.Linq;

namespace ValueObjects;

// Value object for a Swedish personnummer
// https://en.wikipedia.org/wiki/Personal_identity_number_(Sweden)
public record Personnummer
{
    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    public long Pnr { get; }
    public Gender PersonGender { get; private init; }
    
    public string To12DigitString() => Pnr.ToString("D12");

    public static Personnummer CreateFrom12DigitString(string yyyymmddnnnn)
    {
        ValidatePnr(yyyymmddnnnn);

        var gender = (yyyymmddnnnn[10] - '0') % 2 == 0 ? Gender.Female : Gender.Male;

        return new Personnummer(long.Parse(yyyymmddnnnn), gender);
    }

    private Personnummer(long pnr, Gender gender)
    {
        Pnr = pnr;
        PersonGender = gender;
    }

    private static void ValidatePnr(string yyyymmddnnnn)
    {
        if (yyyymmddnnnn.Length != 12)
        {
            throw new ArgumentException("Personnummer must be 12 characters long");
        }

        if (yyyymmddnnnn.Any(c => !char.IsDigit(c)))
        {
            throw new ArgumentException("Personnummer must be all digits");
        }
        
        var month = int.Parse(yyyymmddnnnn[4..6]);
        var day = int.Parse(yyyymmddnnnn[6..8]);
        if (month < 1 || month > 12 || day < 1 || day > 31)
        {
            throw new ArgumentException("Personnummer has invalid month or day");
        }
        
        if (!IsChecksumCorrect(yyyymmddnnnn[2..]))
        {
            throw new ArgumentException("Personnummer failed checkum");
        }
    }

    // https://en.wikipedia.org/wiki/Luhn_algorithm
    private static bool IsChecksumCorrect(string digits)
    {
        return digits.All(char.IsDigit) && 
               digits.Reverse()
                   .Select(c => c - '0')
                   .Select(
                    (digit, i) => 
                        i % 2 == 0
                        ? digit
                        : (digit *= 2) > 9 ? digit - 9 : digit
            )
            .Sum() % 10 == 0;
    }
}