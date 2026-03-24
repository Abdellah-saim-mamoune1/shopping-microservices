using User.API.Dtos;

namespace User.API.Repositories.Client
{
    public interface IClientRepository
    {
        Task<UserGetDto?> GetAsync(int id);
        Task UpdateAsync(int Id, UserUpdateDto user);
     
    }
}
