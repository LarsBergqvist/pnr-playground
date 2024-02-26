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
    public bool IsSamordningsNummer { get; private init; }
    
    public long ToLong() => Pnr;
    public string To12DigitString() => Pnr.ToString("D12");

    public static Personnummer CreateFrom12DigitString(string yyyymmddnnnn)
    {
        ValidatePnr(yyyymmddnnnn);

        var gender = (yyyymmddnnnn[10] - '0') % 2 == 0 ? Gender.Female : Gender.Male;
        var isSamordningsnummer = (yyyymmddnnnn[6] - '0') >= 6;

        return new Personnummer(long.Parse(yyyymmddnnnn), gender, isSamordningsnummer);
    }

    private Personnummer(long pnr, Gender gender, bool isSamordningsNummer)
    {
        Pnr = pnr;
        PersonGender = gender;
        IsSamordningsNummer = isSamordningsNummer;
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