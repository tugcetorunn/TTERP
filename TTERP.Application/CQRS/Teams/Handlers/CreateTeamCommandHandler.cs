using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Teams.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Teams.Handlers
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Response<int>>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeamCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork, IEmployeeRepository employeeRepository)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
        }

        public async Task<Response<int>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = request.Adapt<Team>();

            foreach(var memberId in request.MemberIds!)
            {
                if(await _employeeRepository.IsEmployeeInAnyTeamAsync(memberId, cancellationToken))
                {
                    return Response<int>.Fail(400, $"{memberId} nolu çalışan zaten bir takıma üye.");
                }

                team.Members ??= new List<Employee>();
                team.Members.Add(new Employee { Id = memberId });
            }

            foreach(var managerId in request.ManagerIds!)
            {
                if(await _employeeRepository.IsEmployeeInAnyTeamAsync(managerId, cancellationToken))
                {
                    return Response<int>.Fail(400, $"{managerId} nolu çalışan zaten bir takımda müdür.");
                }

                team.Managers ??= new List<TeamManager>();
                team.Managers.Add(new TeamManager { ManagerId = managerId });
            }

            await _teamRepository.AddAsync(team);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(team.Id, 201, "Takım başarıyla oluşturuldu.");
        }
    }
}
