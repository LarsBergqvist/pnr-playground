using Bogus;
using Bogus.DataSets;
using PersonGenerator.Models;
using ValueObjects;

namespace PersonGenerator;

public class Factory
{
    private Personnummer[] _malePnrList;
    private Personnummer[] _femalePnrList;

    public Factory(int seed)
    {
        Randomizer.Seed = new Random(seed);
        LoadPersonnummer();
    }

    protected void LoadPersonnummer()
    {
        var pnrList = File.ReadAllLines("Testpersonnummer.csv")
            .Select(Personnummer.CreateFrom12DigitString).ToArray();

        _malePnrList = pnrList.Where(pnr => pnr.PersonGender == Personnummer.Gender.Male).ToArray();
        _femalePnrList = pnrList.Where(pnr => pnr.PersonGender == Personnummer.Gender.Female).ToArray();
    }
    
    public Models.Person CreatePerson(string locale)
    {
        var addressFaker = new Faker<Models.Address>(locale)
            .CustomInstantiator(f => new Models.Address(
                f.Address.StreetAddress(),
                f.Address.City(),
                f.Address.ZipCode()
            ));
        var contactInfoFaker = new Faker<ContactInfo>(locale)
            .CustomInstantiator(f => new ContactInfo(
                f.Internet.Email(),
                f.Phone.PhoneNumber("07#-###-####")
            ));
        var personFaker = new Faker<Models.Person>(locale)
            .CustomInstantiator(_ => 
            {
                var person = new Bogus.Person(locale);
                var pnr = person.Gender == Name.Gender.Male ? 
                    _malePnrList.ElementAt(Randomizer.Seed.Next(_malePnrList.Length)) : 
                    _femalePnrList.ElementAt(Randomizer.Seed.Next(_femalePnrList.Length));
                return new Models.Person(
                    Guid.NewGuid(),
                    pnr,
                    person.FirstName,
                    person.LastName,
                    contactInfoFaker.Generate(),
                    addressFaker.Generate()
                );
            })
            .FinishWith((f, c) => c.ContactInfo.Email = f.Internet.Email(c.FirstName, c.LastName));
        
        return personFaker.Generate();
    }
}