using Application.DTOs;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using MediatR;
using System;
using System.Data;

namespace Application.Workers.Commands
{
    public class WorkerCreateCommand : IRequest<Guid>
    {
        public WorkerCreateCommand(WorkerDto create)
        {
            Name = create.Name;
            FirstName = create.FirstName;
            Specialization = create.Specialization;
            Email = create.Email;
            Available = create.Available;
            IsGuard = create.IsGuard;
            Percent = create.Percent;
        }

        public string Name { get; set; }
        public string? FirstName { get; set; }
        public string? Specialization { get; set; }
        public string? Email { get; set; }
        public bool Available { get; set; }
        public bool IsGuard { get; set; }
        public double Percent { get; set; }
    }

    public class WorkerCreateCommandHandler : IRequestHandler<WorkerCreateCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;

        public WorkerCreateCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(WorkerCreateCommand request, CancellationToken cancellationToken)
        {
            var existing = _appDbContext.Workers.FirstOrDefault(u => u.Email == request.Email);
            if (existing != null) return Guid.Empty;
            
            var create = new Worker
            {
                Name = request.Name,
                FirstName = request.FirstName,
                Specialization = request.Specialization,
                Email = request.Email,
                Available = request.Available,
                IsGuard = request.IsGuard,
                Percent = request.Percent,
            };

            _appDbContext.Workers.Add(create);

            var workerRole = _appDbContext.UserRoles.FirstOrDefault(r => r.RoleName == "Lucrător");

            var user = new User
            {
                Email = request.Email,
                Username = request.Name,
                UserRole = workerRole,
                UserRoleId = workerRole.Id,
                SecurityCode = "",
                CreatedAt = DateTime.UtcNow,
                PasswordHash = Crypto.HashPassword(AuthorizationVariables.Salt + request.Name)
            };

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return create.Id;
        }
    }
}
