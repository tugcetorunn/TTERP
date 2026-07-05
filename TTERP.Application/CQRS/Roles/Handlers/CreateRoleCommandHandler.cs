using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Roles.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Roles.Handlers
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response<int>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = request.Adapt<Role>();

            await _roleRepository.AddAsync(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(role.Id, 201, "Rol başarıyla oluşturuldu.");
        }
    }
}
