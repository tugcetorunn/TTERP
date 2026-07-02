using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Announcements.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Announcements.Handlers
{
    public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, Response<int>>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAnnouncementCommandHandler(IAnnouncementRepository announcementRepository, IUnitOfWork unitOfWork)
        {
            _announcementRepository = announcementRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var announcement = request.Adapt<Announcement>();
            await _announcementRepository.AddAsync(announcement);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(announcement.Id, 201,"Duyuru başarıyla oluşturuldu.");
        }
    }
}
