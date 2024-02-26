using System.Text.Json;
using ValueObjects;

namespace UnitTests;

public class PersonnummerTests
{
    [Fact]
    public void Should_Create_Valid_Personnummer()
    {
        var lines = File.ReadAllLines("Testpersonnummer 1990-1999.csv");
        foreach (var line in lines.Skip(1))
        {
            var pnr = Personnummer.CreateFrom12DigitString(line);
            Assert.Equal(line, pnr.To12DigitString());
            Assert.True(!pnr.IsSamordningsNummer);
        }
    }
    
    [Fact]
    public void Should_Create_Valid_Samordningsnummer()
    {
        var lines = File.ReadAllLines("Test_samordn_nr2019.csv");
        foreach (var line in lines.Skip(1))
        {
            var pnr = Personnummer.CreateFrom12DigitString(line);
            Assert.Equal(line, pnr.To12DigitString());
            Assert.True(pnr.IsSamordningsNummer);
        }
    }
    
    [Fact]
    public void ValidMalePnr_CreatesCorrectPersonnummer()
    {
        var validPnr = "199001072397";
        var result = Personnummer.CreateFrom12DigitString(validPnr);

        Assert.Equal(199001072397, result.ToLong());
        Assert.Equal(Personnummer.Gender.Male, result.PersonGender);
        Assert.False(result.IsSamordningsNummer);
    }

    [Fact]
    public void ValidFemalePnr_CreatesCorrectPersonnummer()
    {
        var validPnr = "199001052381";
        var result = Personnummer.CreateFrom12DigitString(validPnr);

        Assert.Equal(199001052381, result.ToLong());
        Assert.Equal(Personnummer.Gender.Female, result.PersonGender);
        Assert.False(result.IsSamordningsNummer);
    }
    
    [Fact]
    public void InvalidLength_ThrowsException()
    {
        var invalidPnr = "20010101123";
        Assert.Throws<ArgumentException>(() => Personnummer.CreateFrom12DigitString(invalidPnr));
    }

    [Fact]
    public void InvalidCharacters_ThrowsException()
    {
        var invalidPnr = "20010101A234";
        Assert.Throws<ArgumentException>(() => Personnummer.CreateFrom12DigitString(invalidPnr));
    }

    [Fact]
    public void InvalidChecksum_ThrowsException()
    {
        var invalidPnr = "200101011236";
        Assert.Throws<ArgumentException>(() => Personnummer.CreateFrom12DigitString(invalidPnr));
    }
    
    [Fact]
    public void Should_Serialize_Personnummer()
    {
        var pnr = Personnummer.CreateFrom12DigitString("199001142398");
        var json = JsonSerializer.Serialize(pnr);
        Assert.Equal("{\"Pnr\":199001142398,\"PersonGender\":0,\"IsSamordningsNummer\":false}", json);
    }
    
    [Fact]
    public void Test_Equality()
    {
        var pnr1 = Personnummer.CreateFrom12DigitString("199001172387");
        var pnr2 = Personnummer.CreateFrom12DigitString("199001172387");
        
        Assert.Equal(pnr1, pnr2);
        Assert.Equal("199001172387", pnr1.To12DigitString());
        Assert.Equal(199001172387, pnr1.Pnr);
    }
}