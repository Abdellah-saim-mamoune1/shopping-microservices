using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.Dtos;

namespace User.API.Repositories.Employee
{
    public class EmployeeRepository(UserDbContext _context) : IEmployeeRepository
    {
        public async Task<UserGetDto?> GetAsync(int id)
       => await _context.Users.Where(u => u.Id == id ).Select(
           u => new UserGetDto
           {
               Id = u.Id,
               LastName = u.LastName,
               Email = u.Email,
               FirstName = u.FirstName,
               PhoneNumber = u.PhoneNumber,
               Type=u.Type,
               CreatedAt=u.CreatedAt,
               
           })
           .FirstOrDefaultAsync();


        public async Task UpdateAsync(int Id,UserUpdateDto user)
        {
            var existing = await _context.Users.FirstAsync(u => u.Id == Id);

            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
       

            await _context.SaveChangesAsync();
        }

        public async Task<List<UserGetDto>> GetAllAsync()
        {
            return await _context.Users.Where(u => u.Type != "Client").Select(u=>new UserGetDto
            {
                Email=u.Email,
                FirstName=u.FirstName,
                Id=u.Id,
                LastName=u.LastName,
                PhoneNumber=u.PhoneNumber,
                CreatedAt=u.CreatedAt,
                Type = u.Type

            }).ToListAsync();
        }
    }
}
