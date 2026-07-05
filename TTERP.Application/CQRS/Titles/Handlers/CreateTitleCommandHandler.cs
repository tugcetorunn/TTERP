using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Titles.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Titles.Handlers
{
    public class CreateTitleCommandHandler : IRequestHandler<CreateTitleCommand, Response<int>>
    {
        private readonly ITitleRepository _titleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTitleCommandHandler(ITitleRepository titleRepository, IUnitOfWork unitOfWork)
        {
            _titleRepository = titleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateTitleCommand request, CancellationToken cancellationToken)
        {
            var title = request.Adapt<Title>();

            await _titleRepository.AddAsync(title);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(title.Id, 201, "Unvan başarıyla oluşturuldu.");
        }
    }
}
