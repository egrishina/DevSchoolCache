using DevSchoolCache;
using Microsoft.AspNetCore.Mvc;

namespace SchoolController;

[ApiController]
public class SchoolController(
    IRepository<School> schoolRepository,
    IRepository<Staff> staffRepository)
    : ControllerBase
{
    [HttpGet("/school/{id}")]
    public async Task<ActionResult<SchoolDto>> GetSchoolById(long id)
    {
        var school = await schoolRepository.TryGetById(id);
        if (school is null)
            return NotFound();

        return SchoolDto.FromSchool(school);
    }

    [HttpGet("/school/byStaff/{id}")]
    public async Task<ActionResult<SchoolDto>> GetSchoolByStaffId(long id)
    {
        var staff = await staffRepository.TryGetById(id);
        if (staff is null)
            return NotFound();

        var school = await schoolRepository.TryGetById(staff.SchoolId);

        if (school == null)
        {
            return NotFound();
        }

        return SchoolDto.FromSchool(school);
    }

    [HttpGet("/staff/{id}")]
    public async Task<ActionResult<StaffDto>> GetStaffById(long id)
    {
        var staff = await staffRepository.TryGetById(id);
        if (staff is null)
            return NotFound();

        return StaffDto.FromStaff(staff);
    }
}