using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Titles.Commands
{
    public class DeleteTitleCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteTitleCommand(int id)
        {
            Id = id;
        }
    }
}
