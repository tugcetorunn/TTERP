using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.TaskAssignments.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.TaskAssignments.Handlers
{
    // daha önce oluşturulmuş bir task için assignment eklemek için.
    public class CreateTaskAssignmentCommandHandler : IRequestHandler<CreateTaskAssignmentCommand, Response<int>>
    {
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskAssignmentCommandHandler(ITaskAssignmentRepository taskAssignmentRepository, ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskAssignmentRepository = taskAssignmentRepository;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateTaskAssignmentCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.FindAsync(request.TaskId);

            if (task == null)
            {
                return Response<int>.Fail(404, "Task ataması yapılacak task bulunamadı.");
            }

            var assignment = request.Adapt<TaskAssignment>();

            await _taskAssignmentRepository.AddAsync(assignment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(assignment.Id, 201, "Task ataması başarıyla oluşturuldu.");
        }
    }
}
