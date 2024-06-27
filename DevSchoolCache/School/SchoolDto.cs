namespace DevSchoolCache;

public record SchoolDto
{
    public long Id { get; set; }

    public string FullName { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public string Email { get; set; }

    public static SchoolDto FromSchool(School school)
    {
        return new SchoolDto
        {
            Id = school.Id, FullName = school.FullName, City = school.City, Address = school.Address,
            Email = school.Email
        };
    }
}