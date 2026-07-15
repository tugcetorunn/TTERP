using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.SupplierMaterials.Commands;
using TTERP.Application.CQRS.SupplierMaterials.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierMaterialsController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public SupplierMaterialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(int? supplierId, int? materialId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetSupplierMaterialsQuery(supplierId, materialId, isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create(CreateSupplierMaterialCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}
