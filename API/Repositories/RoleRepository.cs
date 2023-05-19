using API.Contexts;
using API.Contracts;
using API.Models;

namespace API.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly BookingManagementDbContext _context;
    public RoleRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    /*
     * <summary>
     * Create a new university
     * </summary>
     * <param name="university">University object</param>
     * <returns>University object</returns>
     */
    public Role Create(Role role)
    {
        try
        {
            _context.Set<Role>().Add(role);
            _context.SaveChanges();
            return role;
        }
        catch
        {
            return new Role();
        }
    }

    /*
     * <summary>
     * Update a university
     * </summary>
     * <param name="university">University object</param>
     * <returns>true if data updated</returns>
     * <returns>false if data not updated</returns>
     */
    public bool Update(Role role)
    {
        try
        {
            _context.Set<Role>().Update(role);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /*
     * <summary>
     * Delete a university
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>true if data deleted</returns>
     * <returns>false if data not deleted</returns>
     */
    public bool Delete(Guid guid)
    {
        try
        {
            var role = GetByGuid(guid);
            if (role == null)
            {
                return false;
            }

            _context.Set<Role>().Remove(role);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /*
     * <summary>
     * Get all universities
     * </summary>
     * <returns>List of universities</returns>
     * <returns>Empty list if no data found</returns>
     */
    public IEnumerable<Role> GetAll()
    {
        return _context.Set<Role>().ToList();
    }

    /*
     * <summary>
     * Get a university by guid
     * </summary>
     * <param name="guid">University guid</param>
     * <returns>University object</returns>
     * <returns>null if no data found</returns>
     */
    public Role? GetByGuid(Guid guid)
    {
        return _context.Set<Role>().Find(guid);
    }
}
