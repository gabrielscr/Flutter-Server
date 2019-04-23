namespace FlutterServer.Features.User
{
    using Flutter.Server.Infra;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Flutter.Server.Domain;

    public class InsertEdit
    {
        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
        {
            public int? Id { get; set; }

            public string UserLogin { get; set; }

            public string Password { get; set; }

            public string Name { get; set; }

            public string LastName { get; set; }

            public int Age { get; set; }

            public string Gender { get; set; }

            public string Email { get; set; }

            public bool? Active { get; set; }

            public string ProfileImage { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly AdminContext _adminContext;

            public QueryHandler(AdminContext adminContext)
            {
                _adminContext = adminContext;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _adminContext
                    .Set<User>()
                    .AsNoTracking()
                    .Where(e => e.Id == request.Id)
                    .Select(e => new Command
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Email = e.Email,
                        UserLogin = e.UserLogin,
                        Active = e.Active,
                        Age = e.Age,
                        Gender = e.Gender,
                        LastName = e.LastName,
                        Password = e.Password,
                        ProfileImage = e.ProfileImage
                    })
                    .FirstOrDefaultAsync();
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly AdminContext _adminContext;

            public CommandHandler(AdminContext adminContext)
            {
                _adminContext = adminContext;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await GetUser(request);

                MapUser(user, request);

                await _adminContext.SaveChangesAsync();
            }

            private async Task<User> GetUser(Command request)
            {
                if (!request.Id.HasValue)
                {
                    var user = new User();

                    await _adminContext.AddAsync(user);

                    return user;
                }

                return await _adminContext
                    .Set<User>()
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
            }

            private void MapUser(User user, Command request)
            {
                user.Name = request.Name;
                user.Email = request.Email;
                user.UserLogin = request.UserLogin;
                user.Active = request.Active;
                user.Age = request.Age;
                user.Gender = request.Gender;
                user.LastName = request.LastName;
                user.Password = request.Password;
                user.ProfileImage = request.ProfileImage;
            }
        }
    }
}