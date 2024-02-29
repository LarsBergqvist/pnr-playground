using Bogus;
using Bogus.DataSets;
using ValueObjects;
using Person = PersonGenerator.Models.Person;

namespace PersonGenerator;

public class Factory
{
    private readonly int _seed;
    private Personnummer[] _malePnrList;
    private Personnummer[] _femalePnrList;

    public Factory(int seed)
    {
        _seed = seed;
        Randomizer.Seed = new Random(_seed);
        LoadPersonnummer();
    }

    protected void LoadPersonnummer()
    {
        var pnrList = File.ReadAllLines("Testpersonnummer.csv")
            .Select(Personnummer.CreateFrom12DigitString).ToArray();

        _malePnrList = pnrList.Where(pnr => pnr.PersonGender == Personnummer.Gender.Male).ToArray();
        _femalePnrList = pnrList.Where(pnr => pnr.PersonGender == Personnummer.Gender.Female).ToArray();
    }
    
    public Person CreatePerson(string locale)
    {
        var personFaker = new Faker<Person>(locale)
            .CustomInstantiator(faker => 
            {
                var person = new Bogus.Person(locale);
                Personnummer pnr;
                pnr = person.Gender == Name.Gender.Male ? 
                    _malePnrList.ElementAt(Randomizer.Seed.Next(_malePnrList.Length)) : 
                    _femalePnrList.ElementAt(Randomizer.Seed.Next(_femalePnrList.Length));
                return new Person(
                    Guid.NewGuid(),
                    pnr,
                    person.FirstName,
                    person.LastName
                );
            });
        
        return personFaker.Generate();
    }
}