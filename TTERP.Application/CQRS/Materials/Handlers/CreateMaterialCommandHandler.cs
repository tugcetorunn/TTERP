using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Materials.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Materials.Handlers
{
    public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, Response<int>>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMaterialCommandHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork)
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = request.Adapt<Material>();

            await _materialRepository.AddAsync(material);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(material.Id, 201, "Malzeme başarıyla oluşturuldu.");
        }
    }
}
