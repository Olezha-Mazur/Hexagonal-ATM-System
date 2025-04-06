namespace Models;

public class SystemPassword
{
    public SystemPassword(string password)
    {
        Password = password;
    }

    public string Password { get; set; }
}