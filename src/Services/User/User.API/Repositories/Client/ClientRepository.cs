using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.Dtos;

namespace User.API.Repositories.Client
{
    public class ClientRepository : IClientRepository
    {
        private readonly UserDbContext _context;

        public ClientRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<UserGetDto?> GetAsync(int id)
            => await _context.Users.Where(u => u.Id == id).Select(
                u=>new UserGetDto {
                    Id = u.Id,
                    LastName=u.LastName,
                    Email=u.Email,
                    FirstName=u.FirstName,
                    PhoneNumber=u.PhoneNumber,
                    CreatedAt=u.CreatedAt,
                    Type=u.Type
                 })
                .FirstOrDefaultAsync(u => u.Id == id);


        public async Task UpdateAsync(int Id, UserUpdateDto user)
        {
            var existing = await _context.Users.FirstAsync(u=>u.Id==Id);

            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.PhoneNumber = user.PhoneNumber;
            

            await _context.SaveChangesAsync();
        }
    }
}
