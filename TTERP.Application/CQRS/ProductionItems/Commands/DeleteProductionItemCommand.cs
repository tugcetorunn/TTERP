using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductionItems.Commands
{
    public class DeleteProductionItemCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteProductionItemCommand(int id)
        {
            Id = id;
        }
    }
}
