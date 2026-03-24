namespace User.API.Repositories.Shared
{
    public interface IUserRepository
    {
        public Task RegisreUserAsync(Entities.User user);
    }
}
