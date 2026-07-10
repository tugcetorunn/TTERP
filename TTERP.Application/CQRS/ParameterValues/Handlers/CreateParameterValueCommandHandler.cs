using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ParameterValues.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterValues.Handlers
{
    public class CreateParameterValueCommandHandler : IRequestHandler<CreateParameterValueExceptDefinitionCommand, Response<int>>
    {
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IParameterDefinitionRepository _parameterDefinitionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateParameterValueCommandHandler(IParameterValueRepository parameterValueRepository, IUnitOfWork unitOfWork, IParameterDefinitionRepository parameterDefinitionRepository)
        {
            _parameterValueRepository = parameterValueRepository;
            _unitOfWork = unitOfWork;
            _parameterDefinitionRepository = parameterDefinitionRepository;
        }

        public async Task<Response<int>> Handle(CreateParameterValueExceptDefinitionCommand request, CancellationToken cancellationToken)
        {
            var value = request.Adapt<ParameterValue>();

            var definition = await _parameterDefinitionRepository.FindAsync(value.Id);

            if (definition == null)
            {
                return Response<int>.Fail(404, "Parametre tanımı bulunamadı.");
            }

            value.ParameterDefinitionId = definition.Id;

            await _parameterValueRepository.AddAsync(value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(value.Id, 201, "Parametre değeri başarıyla oluşturuldu.");
        }
    }
}
