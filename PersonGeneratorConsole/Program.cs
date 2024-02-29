// See https://aka.ms/new-console-template for more information


using PersonGenerator;

var factory = new Factory(124567);
for (var i = 0; i < 20; i++)
{
    var person = factory.CreatePerson("sv");
    Console.WriteLine($"{person.FirstName} {person.LastName} {person.Personnummer}");
}
