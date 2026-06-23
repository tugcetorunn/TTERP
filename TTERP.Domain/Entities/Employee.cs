using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Employee : IdentityUser<int>, IAuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        public string NationalId { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RegistrationNumber { get; set; } // otomatik olarak atanacak
        public string? ImagePath { get; set; }
        public int? Gender { get; set; }
        public int? MaritalStatus { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public int? TitleId { get; set; }
        public Title? Title { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Supply>? Supplies { get; set; }
        public ICollection<Task>? AssignedTasks { get; set; }
        public ICollection<Task>? CreatedTasks { get; set; }
        public ICollection<TaskAssignment>? TaskAssignments { get; set; }
        public double? Salary { get; set; }
        public double? RightToAnnualLeave { get; set; }
        public string? InternalPhone { get; set; }
        public bool IsPasswordChanged { get; set; } = false;


        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; private set; }
        public DateTime? DeletedDate { get; private set; }
        public string? CreatedBy { get; private set; }
        public string? UpdatedBy { get; private set; }
        public string? DeletedBy { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;

        public void SetCreated(string user)
        {
            CreatedBy = user;
        }

        public void SetUpdated(string user)
        {
            UpdatedDate = DateTime.UtcNow;
            UpdatedBy = user;
        }

        public void SoftDelete(string user)
        {
            IsDeleted = true;
            IsActive = false;
            DeletedDate = DateTime.UtcNow;
            DeletedBy = user;
        }
    }
}
