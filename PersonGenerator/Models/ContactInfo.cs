namespace PersonGenerator.Models;

public class ContactInfo
{
    public string Email { get; set; }
    public string Phone { get; }

    public ContactInfo(string email, string phone)
    {
        Email = email;
        Phone = phone;
    }
    
}