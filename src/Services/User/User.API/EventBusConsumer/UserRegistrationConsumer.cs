using EventBus.Messages.Events;
using MassTransit;
using User.API.Repositories.Shared;

namespace User.API.EventBusConsumer
{
    public class UserRegistrationConsumer : IConsumer<UserRegistrationEvent>
    {
        IUserRepository _rp;

       public UserRegistrationConsumer(IUserRepository rp)
        {
            _rp = rp;
        }
        public async Task Consume(ConsumeContext<UserRegistrationEvent> context)
        {
            Entities.User user = new Entities.User
            {
               Email=context.Message.Account,
               FirstName=context.Message.FirstName,
               CreatedAt=context.Message.CreatedAt,
               Id=context.Message.Id,
               LastName=context.Message.LastName,
               PhoneNumber=context.Message.PhoneNumber,
               Type=context.Message.Type

            };

            await _rp.RegisreUserAsync(user);
            
           
           
        }
    }
}
