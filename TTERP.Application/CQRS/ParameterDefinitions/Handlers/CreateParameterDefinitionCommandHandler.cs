using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ParameterDefinitions.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterDefinitions.Handlers
{
    public class CreateParameterDefinitionCommandHandler : IRequestHandler<CreateParameterDefinitionCommand, Response<int>>
    {
        private readonly IParameterDefinitionRepository _parameterDefinitionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateParameterDefinitionCommandHandler(IParameterDefinitionRepository parameterDefinitionRepository, IUnitOfWork unitOfWork)
        {
            _parameterDefinitionRepository = parameterDefinitionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateParameterDefinitionCommand request, CancellationToken cancellationToken)
        {
            var definition = request.Adapt<ParameterDefinition>();

            if(request.ParameterValues != null && request.ParameterValues.Any())
            {
                foreach (var value in request.ParameterValues)
                {
                    var parameterValue = value.Adapt<ParameterValue>();
                    definition.ParameterValues ??= new List<ParameterValue>();
                    definition.ParameterValues.Add(parameterValue);
                }
            }

            await _parameterDefinitionRepository.AddAsync(definition);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(definition.Id, 201, "Parametre tanımı ve değerleri başarıyla oluşturuldu.");
        }
    }
}
