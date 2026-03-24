using User.API.Data;

namespace User.API.Repositories.Shared
{
    public class UserRepository: IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task RegisreUserAsync(Entities.User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

    }
}
