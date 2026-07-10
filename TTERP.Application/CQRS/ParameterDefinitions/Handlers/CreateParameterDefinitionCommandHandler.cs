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
            if (request.ParameterValues != null)
            {
                var duplicateValues = request.ParameterValues!
                                                .GroupBy(x => new { x.ParamCode, x.LanguageId })
                                                .Where(x => x.Count() > 1)
                                                .Select(x => new
                                                {
                                                    x.Key.ParamCode,
                                                    x.Key.LanguageId
                                                })
                                                .ToList();

                if (duplicateValues.Any())
                {
                    throw new Exception("Aynı değer kodu aynı dilde birden fazla kez gönderilemez.");
                }
            }

            var definition = request.Adapt<ParameterDefinition>();

            await _parameterDefinitionRepository.AddAsync(definition);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(definition.Id, 201, "Parametre tanımı ve değerleri başarıyla oluşturuldu.");
        }
    }
}
