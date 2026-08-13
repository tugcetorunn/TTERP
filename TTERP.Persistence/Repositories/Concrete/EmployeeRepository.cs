using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Models;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<bool> IsManagerAsync(int employeeId)
        {
            return await context.Set<TeamManager>()
                .AnyAsync(tm => tm.ManagerId == employeeId && !tm.IsDeleted && tm.IsActive); // TODO: isdeleted ve isactive i kaldırıp hepsini getirecek mi gör
        }

        public async Task<List<int>> GetTeamEmployeeIdsAsync(int managerId)
        {
            var managedTeamIds = await context.Set<TeamManager>()
                .Where(tm => tm.ManagerId == managerId && !tm.IsDeleted && tm.IsActive)
                .Select(tm => tm.TeamId)
                .ToListAsync();

            if (managedTeamIds.Any())
                return new List<int>();

            return await context.Set<Employee>()
                .Where(e => e.TeamId.HasValue &&
                            managedTeamIds.Contains(e.TeamId.Value) &&
                            !e.IsDeleted && e.IsActive)
                .Select(e => e.Id)
                .ToListAsync();

        }

        public async Task<int> GetMaxRegistrationNumberAsync(CancellationToken cancellationToken)
        {
            return await context.Set<Employee>()
                                .MaxAsync(e => (int?)e.RegistrationNumber, cancellationToken) ?? 0;
        }

        public async Task<string?> GetMaxInternalPhoneAsync(CancellationToken cancellationToken)
        {
            return await context.Set<Employee>()
                                .MaxAsync(e => e.InternalPhone, cancellationToken);
        }

        public async Task<bool> IsEmployeeInAnyTeamAsync(int memberId, CancellationToken cancellationToken)
        {
            return await context.Set<Employee>()
                          .AnyAsync(e => e.Id == memberId && e.TeamId.HasValue && !e.IsDeleted && e.IsActive, cancellationToken);
        }

        public async Task<IReadOnlyList<GetEmployeesDTO>> GetEmployeesWithRoleAsync(bool? isActive, bool? isDeleted, CancellationToken cancellationToken = default)
        {
            var query = from employee in context.Users
                        join userRole in context.UserRoles
                        on employee.Id equals userRole.UserId
                        into userRoleGroup from userRole in userRoleGroup
                        .DefaultIfEmpty()
                        join role in context.Roles
                        on userRole.RoleId equals role.Id
                        into roleGroup from role in roleGroup
                        .DefaultIfEmpty()
                        where employee.IsDeleted == (isDeleted ?? false) &&
                        (
                            !isActive.HasValue ||
                            employee.IsActive ==
                                isActive.Value
                        )

                        select new GetEmployeesDTO
                        {
                            Id = employee.Id,

                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            FullName = employee.FullName,

                            NationalId = employee.NationalId,
                            RegistrationNumber = employee.RegistrationNumber,

                            Email = employee.Email,
                            PhoneNumber = employee.PhoneNumber,
                            InternalPhone = employee.InternalPhone,

                            DateOfBirth = employee.DateOfBirth,
                            HireDate = employee.HireDate,

                            Gender = employee.Gender,
                            MaritalStatus = employee.MaritalStatus,

                            CountryId = employee.CountryId,
                            CountryName = employee.Country != null
                                    ? employee.Country!.Name
                                    : null,

                            CityId = employee.CityId,
                            CityName = employee.City != null
                                    ? employee.City!.Name
                                    : null,

                            TownId = employee.TownId,
                            TownName = employee.Town != null
                                    ? employee.Town!.Name
                                    : null,

                            DistrictId = employee.DistrictId,
                            DistrictName = employee.District != null
                                    ? employee.District!.Name
                                    : null,

                            NeighborhoodId = employee.NeighborhoodId,
                            NeighborhoodName =  employee.Neighborhood != null
                                    ? employee.Neighborhood!.Name
                                    : null,

                            AddressLine = employee.AddressLine,
                            ImagePath = employee.ImagePath,

                            TitleId = employee.TitleId,
                            TitleName = employee.Title != null
                                    ? employee.Title!.Name
                                    : null,

                            TeamId = employee.TeamId,
                            TeamName = employee.Team != null
                                    ? employee.Team!.Name
                                    : null,

                            RoleId = role != null
                                ? role.Id
                                : null,

                            RoleName = role != null
                                ? role.Name
                                : null,

                            Salary = employee.Salary,

                            AnnualLeaveUsed = employee.AnnualLeaveUsed,

                            RightToAnnualLeave = employee.RightToAnnualLeave!.Value,

                            IsActive = employee.IsActive,
                            IsDeleted = employee.IsDeleted
                        };

            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
