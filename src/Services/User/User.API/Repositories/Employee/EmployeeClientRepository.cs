using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.Dtos;

namespace User.API.Repositories.Employee
{
    public class EmployeeClientRepository(UserDbContext _context): IEmployeeClientRepository
    {
        public async Task<UserGetDto?> GetByIdAsync(int id)
     => await _context.Users.Where(u => u.Id == id).Select(
         u => new UserGetDto
         {
             Id= u.Id,
             LastName = u.LastName,
             Email = u.Email,
             FirstName = u.FirstName,
             PhoneNumber = u.PhoneNumber,
             CreatedAt =u.CreatedAt,
             Type = u.Type,
         })
         .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<List<UserGetDto>> GetAllAsync()
        {
            return await _context.Users.Where(u => u.Type == "Client").Select(u => new UserGetDto
            {
                Email = u.Email,
                FirstName = u.FirstName,
                Id = u.Id,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                CreatedAt = u.CreatedAt,
                Type = u.Type,

            }).ToListAsync();
        }
        public async Task<int> AddAsync(UserCreateDto user)
        {
            var User = new Entities.User
            {

                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.LastName,
                Type="Client",
               
            };

            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return User.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
