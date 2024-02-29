using ValueObjects;

namespace PersonGenerator.Models;

public class Person
{
    public Guid Id { get; }
    public Personnummer Personnummer { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public Address Address { get; }
    public ContactInfo ContactInfo { get; }
    public Person(Guid id, Personnummer personnummer, string firstName, string lastName /*, ContactInfo contactInfo, Address address*/)
    {
        Id = id;
        Personnummer = personnummer;
        FirstName = firstName;
        LastName = lastName;
//        ContactInfo = contactInfo;
//        Address = address;
    }
    
}