using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Notifications;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Employees
{
    public class GetEmployeesDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public string? AddressLine { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? TownId { get; set; }
        public string? TownName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? NeighborhoodId { get; set; }
        public string? NeighborhoodName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RegistrationNumber { get; set; } 
        public string? ImagePath { get; set; }
        public int? Gender { get; set; }
        public string? GenderName { get; set; }
        public int? MaritalStatus { get; set; }
        public string? MaritalStatusName { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public int? TitleId { get; set; } // filtre için
        public string? TitleName { get; set; } // listede göstermek için
        public int? TeamId { get; set; }
        public string? TeamName { get; set; }
        public decimal? Salary { get; set; }
        public double? RightToAnnualLeave { get; set; }
        public string? InternalPhone { get; set; }
        public ICollection<GetMyNotificationsDTO>? Notifications { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
