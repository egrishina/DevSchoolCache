using DevSchoolCache;
using Microsoft.AspNetCore.Mvc;

namespace SchoolController;

[ApiController]
public class SchoolController(
    IHybridCacheService<School> schoolService,
    IHybridCacheService<Staff> staffService)
    : ControllerBase
{
    [HttpGet("/school/{id}")]
    public async Task<ActionResult<SchoolDto>> GetSchoolById(long id)
    {
        var school = await schoolService.GetOrAddAsync(id);
        if (school is null)
            return NotFound();

        return SchoolDto.FromSchool(school);
    }

    [HttpGet("/school/byStaff/{id}")]
    public async Task<ActionResult<SchoolDto>> GetSchoolByStaffId(long id)
    {
        var staff = await staffService.GetOrAddAsync(id);
        if (staff is null)
            return NotFound();

        var school = await schoolService.GetOrAddAsync(staff.SchoolId);

        if (school == null)
        {
            return NotFound();
        }

        return SchoolDto.FromSchool(school);
    }

    [HttpGet("/staff/{id}")]
    public async Task<ActionResult<StaffDto>> GetStaffById(long id)
    {
        var staff = await staffService.GetOrAddAsync(id);
        if (staff is null)
            return NotFound();

        return StaffDto.FromStaff(staff);
    }
}