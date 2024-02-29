// See https://aka.ms/new-console-template for more information


using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using PersonGenerator;

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All),
    Converters = { new JsonStringEnumConverter() }
};

var factory = new Factory(124567);
for (var i = 0; i < 20; i++)
{
    var person = factory.CreatePerson("sv");
    var json = JsonSerializer.Serialize(person, jsonOptions);
    Console.WriteLine(json);
}
