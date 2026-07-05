using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.TeamManagers.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.TeamManagers.Handlers
{
    public class CreateTeamManagerCommandHandler : IRequestHandler<CreateTeamManagerCommand, Response<int>>
    {
        private readonly ITeamManagerRepository _teamManagerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeamManagerCommandHandler(ITeamManagerRepository teamManagerRepository, IUnitOfWork unitOfWork, ITeamRepository teamRepository)
        {
            _teamManagerRepository = teamManagerRepository;
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
        }

        public async Task<Response<int>> Handle(CreateTeamManagerCommand request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.FindAsync(request.TeamId);

            if (team == null)
            {
                return Response<int>.Fail(404, "Müdür eklemeye çalıştığınız takım bulunamadı.");
            }

            var teamManager = request.Adapt<TeamManager>();

            await _teamManagerRepository.AddAsync(teamManager);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(teamManager.Id, 200, "Takım yöneticisi başarıyla atandı.");
        }
    }
}
