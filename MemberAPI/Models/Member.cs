namespace MemberAPI.Models;

public class Member
{
    public int Id {get; set;}
    public string FirstName{get; set;} = string.Empty;
    public string LastName{get; set;} = string.Empty;
    public string Email{get; set;} = string.Empty;

    public string PhoneNumber{get; set;} = string.Empty;

    public DateTime DateJoined{get; set;} = DateTime.UtcNow;

    public bool IsActive{get;set;} = true;
}