using DevSchoolCache;
using Microsoft.AspNetCore.Mvc;

namespace SchoolController;

[ApiController]
[Route("school")]
public class SchoolController(
    IHybridCacheService<School> schoolService,
    IHybridCacheService<Staff> staffService)
    : ControllerBase
{
    // GET: schools/get-all
    [HttpGet("/{id}")]
    public async Task<ActionResult<School>> GetSchoolById(long id)
    {
        var school = await schoolService.GetOrAddAsync(id);
        if (school is null)
            return NotFound();

        return school;
    }

    // GET: staff/1
    [HttpGet("/staff/{id}")]
    public async Task<ActionResult<School>> GetSchoolByStaffId(long id)
    {
        var staff = await staffService.GetOrAddAsync(id);
        if (staff is null)
            return NotFound();
        
        var school = await schoolService.GetOrAddAsync(staff.SchoolId);

        if (school == null)
        {
            return NotFound();
        }

        return school;
    }
}