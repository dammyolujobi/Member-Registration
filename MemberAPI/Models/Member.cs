namespace MemberAPI.Models;

public class Member
{
    public int id {get; set;}
    public string FullName{get; set;} = string.Empty;
    public string Email{get; set;} = string.Empty;

    public string PhoneNumber{get; set;} = string.Empty;

    public DateTime DateJoined{get; set;} = DateTime.UtcNow;

    public bool isActive{get;set;} = true;
}