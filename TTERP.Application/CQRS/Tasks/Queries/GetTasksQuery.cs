using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Tasks.Queries
{
    public class GetTasksQuery : IRequest<Response<IReadOnlyList<GetTasksDTO>>>
    {
        [JsonIgnore]
        public int CurrentUserId { get; set; }
        public List<int>? EmployeeIds { get; set; } // tek id de olabilir (çalışanlar kendi tasklarını görecek), müdürler için bölündeki çalışanların birden fazla id leri de olabilir (müdürler bölümlerindeki tüm çalışanların tasklarını görecek)
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
