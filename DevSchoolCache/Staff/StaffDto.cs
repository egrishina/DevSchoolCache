namespace DevSchoolCache;

public record StaffDto
{
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MiddleName { get; set; }

    public DateOnly Birthday { get; set; }

    public Position Position { get; set; }

    public long SchoolId { get; set; }

    public string Email { get; set; }

    public static StaffDto FromStaff(Staff staff)
    {
        return new StaffDto
        {
            Id = staff.Id,
            FirstName = staff.FirstName,
            LastName = staff.LastName,
            MiddleName = staff.MiddleName,
            Birthday = staff.Birthday,
            Position = staff.Position,
            SchoolId = staff.SchoolId,
            Email = staff.Email
        };
    }
}