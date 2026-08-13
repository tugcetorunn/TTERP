using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplierMaterials.Commands
{
    public class DeleteSupplierMaterialCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteSupplierMaterialCommand(int id)
        {
            Id = id;
        }
    }
}
