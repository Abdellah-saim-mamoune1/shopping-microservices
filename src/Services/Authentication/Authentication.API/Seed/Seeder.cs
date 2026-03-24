using Authentication.API.Data;
using Authentication.API.Dtos;
using Authentication.API.Services;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Seed
{
    public class Seeder
    {
        IAuthenticationService authenticationService;
        AppDbContext context;

        public Seeder(AppDbContext context, IAuthenticationService authenticationService)
        {
            this.context = context;
            this.authenticationService = authenticationService;
        }


        public async Task Seed()
        {

            var clients = GetClients();
            var employees = GetEmployees();


            if (await context.Users.AnyAsync())
                return;

            foreach (var client in clients)
            {

                await authenticationService.RegisterClientAsync(client);
            }



            foreach (var employee in employees)
            {

                await authenticationService.RegisterEmployeeAsync(employee);
            }
        }

        private List<ClientRegistrationDto> GetClients()
        {
            var clients = new List<ClientRegistrationDto>
               {
             new ClientRegistrationDto
              {
        FirstName = "Ahmed",
        LastName = "Benali",
        PhoneNumber = "+213661234567",
        Account = "ahmed_client@gmail.com",
        Password = "StrongP@ss1"
              },

           new ClientRegistrationDto
              {
        FirstName = "Sara",
        LastName = "Kaci",
        PhoneNumber = "+213550987654",
        Account = "sara_client@gmail.com",
        Password = "MySecure@123"
              }
             };

            return clients;
        }



        private List<EmployeeRegistrationDto> GetEmployees()
        {
            var employees = new List<EmployeeRegistrationDto>
             {
         new EmployeeRegistrationDto
          {
        FirstName = "Karim",
        LastName = "Mansouri",
        PhoneNumber = "+213770112233",
        Account = "karim_emp@gmail.com",
        Password = "Admin@456"
               },
        new EmployeeRegistrationDto
        {
        FirstName = "Lina",
        LastName = "Zeroual",
        PhoneNumber = "+213699445566",
        Account = "lina_emp@gmail.com",
        Password = "Secure#789"
         }
        };

            return employees;
        }
    }
}
