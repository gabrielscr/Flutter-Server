namespace Flutter.Server.Features.User
{
    using Flutter.Server.Infra;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Flutter.Server.Domain;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;

    public class List
    {
        public class Query : IRequest<UserDto[]>
        {
        }
        public class UserDto
        {
            public int Id { get; set; }

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

        public class QueryHandler : IRequestHandler<Query, UserDto[]>
        {
            private readonly AdminContext _adminContext;

            public QueryHandler(AdminContext adminContext)
            {
                _adminContext = adminContext;
            }

            public async Task<UserDto[]> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _adminContext
                    .Set<User>()
                    .Select(m => new UserDto
                    {
                        Id = m.Id,
                        Active = m.Active,
                        Age = m.Age,
                        Email = m.Email,
                        Gender = m.Gender,
                        LastName = m.LastName,
                        Name = m.Name,
                        Password = m.Password,
                        ProfileImage = m.ProfileImage,
                        UserLogin = m.UserLogin
                    })
                    .ToArrayAsync();
            }
        }
    }
}
