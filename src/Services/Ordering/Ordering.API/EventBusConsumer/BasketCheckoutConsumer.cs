using EventBus.Messages.Events;
using MassTransit;
using Ordering.API.Dtos;
using Ordering.API.Entities;
using Ordering.API.Repositories;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IOrderingRepository _repository;

        public BasketCheckoutConsumer(IOrderingRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var message = context.Message;

           
            var items = message.Items.Select(i => new Item
            {
                ProductId = i.ProductId,
                Name = i.Name,
                Category = i.Category,
                Quantity = i.Quantity,
                ImageUrl = i.ImageUrl,
                TotalPrice = i.TotalPrice
            }).ToList();

         
            var order = new OrderCheckoutDto
            {
                Items = items,
                TotalPrice = message.TotalPrice,

           
                FirstName = message.FirstName,
                LastName = message.LastName,
                EmailAddress = message.EmailAddress,
                AddressLine = message.AddressLine,
                Country = message.Country,
                State = message.State,
                ZipCode = message.ZipCode,

              
                PaymentMethod = message.PaymentMethod
            };

            await _repository.CheckOutOrder(order, message.UserId);
        }
    }
}