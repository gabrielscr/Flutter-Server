namespace Flutter.Server.Features.User
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

    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
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
                var user = await _adminContext.Set<User>()
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                _adminContext.Remove(user);

                await _adminContext.SaveChangesAsync();
            }
        }
    }
}
