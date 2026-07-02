using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Announcements.Queries;
using TTERP.Application.Models.DTOs.Announcements;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Announcements.Handlers
{
    public class GetAnnouncementQueryHandler : IRequestHandler<GetAnnouncementsQuery, Response<IReadOnlyList<GetAnnouncementsDTO>>>
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public GetAnnouncementQueryHandler(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<Response<IReadOnlyList<GetAnnouncementsDTO>>> Handle(GetAnnouncementsQuery request, CancellationToken cancellationToken)
        {
            var announcements = await _announcementRepository.GetListWithFilterAsync(
                select: a => a.Adapt<GetAnnouncementsDTO>(), // bu kod sadece dto içindeki alanların select sorgusunu atar, tüm tabloyu belleğe yüklemez.
                where: a => a.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || a.IsActive == request.IsActive.Value));
                // isActive değeri requestten boş gelirse hepsini getirsin, isActive boş değil ve false işaretlenirse active olmayanları getirsin.

            return Response<IReadOnlyList<GetAnnouncementsDTO>>.Success(announcements.ToList());
        }
    }
}
