using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductionProgresses.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductionProgresses.Handlers
{
    public class AddProductionProgressCommandHandler
    : IRequestHandler<AddProductionProgressCommand, Response<int>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IProductionProgressRepository _productionProgressRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public AddProductionProgressCommandHandler(
            IProductionRepository productionRepository,
            IProductionProgressRepository productionProgressRepository,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _productionRepository = productionRepository;
            _productionProgressRepository = productionProgressRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(
            AddProductionProgressCommand request,
            CancellationToken cancellationToken)
        {
            if (request.ProducedQuantity <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Üretilen miktar sıfırdan büyük olmalıdır.");
            }

            var production = await _productionRepository.FindAsync(
                request.ProductionId);

            if (production == null)
            {
                return Response<int>.Fail(
                    404,
                    "Üretim emri bulunamadı.");
            }

            var employeeId = default(int?);

            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out var userId))
            {
                employeeId = userId;
            }

            var progress = new ProductionProgress
            {
                ProductionId = production.Id,
                ProducedQuantity = request.ProducedQuantity,
                Note = request.Note,
                ProgressDate = DateTime.UtcNow,
                EmployeeId = employeeId
            };

            production.ActualQuantity =
                (production.ActualQuantity ?? 0) +
                request.ProducedQuantity;

            await _productionProgressRepository.AddAsync(progress);
            _productionRepository.Update(production);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(
                progress.Id,
                201,
                "Üretim ilerlemesi başarıyla kaydedildi.");
        }
    }
}
