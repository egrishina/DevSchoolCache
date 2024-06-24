namespace DevSchoolCache;

public class Staff
{
    public long Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string MiddleName { get; set; }
    
    public DateOnly Birthday { get; set; }
    
    public Position Position { get; set; }
    
    public string Email { get; set; }
}