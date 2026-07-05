using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Productions.Commands;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Handlers
{
    public class StartProductionCommandHandler : IRequestHandler<StartProductionCommand, Response<int>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StartProductionCommandHandler(IProductionRepository productionRepository, IUnitOfWork unitOfWork, IParameterValueRepository parameterValueRepository)
        {
            _productionRepository = productionRepository;
            _unitOfWork = unitOfWork;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<int>> Handle(StartProductionCommand request, CancellationToken cancellationToken)
        {
            var production = await _productionRepository.FindAsync(request.ProductionId);

            if (production == null)
            {
                return Response<int>.Fail(404, "Üretim emri bulunamadı.");
            }

            if (production.ProductionStatus != await _parameterValueRepository.ParamValueToParamCode("ProductionStatus", "Planned", cancellationToken))
            {
                return Response<int>.Fail(400, "Sadece 'Planlandı' durumundaki üretimler başlatılabilir.");
            }

            production.ProductionStatus = await _parameterValueRepository.ParamValueToParamCode("ProductionStatus", "InProgress", cancellationToken); // üretimde

            production.ProductionDate = DateTime.Now; // planlanandan farklı olarak üretim başlatıldığında üretim tarihi güncellenir

            _productionRepository.Update(production);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(production.Id, 200, "Üretim başarıyla başlatıldı.");
        }
    }
}
