using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Tasks.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;
using Task = TTERP.Domain.Entities.Task;

namespace TTERP.Application.CQRS.Tasks.Handlers
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Response<int>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = request.Adapt<Task>();

            if (request.Assignments != null && request.Assignments.Any())
            {
                var assignments = request.Assignments.Select(a => a.Adapt<TaskAssignment>()).ToList();
                task.TaskAssignments = assignments;
            }

            await _taskRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(task.Id, 201, "Task başarıyla oluşturuldu.");
        }
    }
}
