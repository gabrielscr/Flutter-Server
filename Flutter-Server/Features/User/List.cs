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
        public class Query : IRequest<Dto[]>
        {
            public int PageSize { get; set; }

            public int PageIndex { get; set; }

            public string Filter { get; set; }
        }

        public class Dto
        {
            public int? Id { get; set; }

            public string UserLogin { get; set; }

            public string Password { get; set; }

            public string Name { get; set; }

            public string LastName { get; set; }

            public string Age { get; set; }

            public string Gender { get; set; }

            public string Email { get; set; }

            public bool? Active { get; set; }

            public string ProfileImage { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Dto[]>
        {
            private readonly AdminContext _adminContext;

            public QueryHandler(AdminContext adminContext)
            {
                _adminContext = adminContext;
            }

            public async Task<Dto[]> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = GetUsers();

                users = FilterUsers(users, request);

                users = PaginateUsers(users, request);

                return await users.ToArrayAsync();
            }

            private IQueryable<Dto> GetUsers()
            {
                return _adminContext
                    .Set<User>()
                    .AsNoTracking()
                    .Select(e => new Dto
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
                    .AsQueryable();
            }

            private IQueryable<Dto> FilterUsers(IQueryable<Dto> users, Query request)
            {
                if (string.IsNullOrEmpty(request.Filter))
                    return users;

                return users
                    .Where(e => e.Name.Contains(request.Filter) || e.Email.Contains(request.Filter));
            }

            private IQueryable<Dto> PaginateUsers(IQueryable<Dto> users, Query request)
            {
                if (!string.IsNullOrEmpty(request.Filter))
                    return users;

                return users
                        .Skip((request.PageIndex - 1) * request.PageSize)
                        .Take(request.PageSize);
            }
        }
    }
}
