namespace DevSchoolCache;

public interface IStaffRepository
{
    public Staff? TryGetById(long id);
    
    public IQueryable<Staff> GetAll();
}