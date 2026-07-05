using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Warehouses.Commands
{
    public class CreateWarehouseCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
