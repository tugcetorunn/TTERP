using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class ParameterValueRepository : BaseRepository<ParameterValue>, IParameterValueRepository
    {
        public ParameterValueRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<ParameterValue?> GetByParamTypeAndCodeAsync(string paramType, int targetStatusCode, int languageId, CancellationToken cancellationToken)
        {
            return await context.ParameterValues
                                .Include(x => x.ParameterDefinition)
                                .FirstOrDefaultAsync(
                                    x =>
                                        x.ParameterDefinition!.ParamType == paramType &&
                                        x.ParamCode == targetStatusCode && x.LanguageId == languageId &&
                                        x.IsActive &&
                                        !x.IsDeleted,
                                    cancellationToken);
        }

        public async Task<ParameterValue?> GetByShortCodeAsync(string paramType, string shortCode, int languageId, CancellationToken cancellationToken)
        {
            return await context.ParameterValues
                                .Include(x => x.ParameterDefinition)
                                .FirstOrDefaultAsync(
                                    x =>
                                        x.ParameterDefinition!.ParamType == paramType &&
                                        x.ShortCode == shortCode && x.LanguageId == languageId &&
                                        x.IsActive &&
                                        !x.IsDeleted,
                                    cancellationToken);
        }

        public async Task<string?> GetParamTypeByValueAsync(int id, int languageId, CancellationToken cancellationToken)
        {
            return await context.ParameterValues
                                .Where(x => x.Id == id && x.LanguageId == languageId && x.IsActive && !x.IsDeleted)
                                .Select(x => x.ParameterDefinition!.ParamType)
                                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<ParameterValue>> GetParamValuesByParamTypeAsync(string paramType, int languageId, CancellationToken cancellationToken)
        {
            return await context.ParameterValues
                                .Include(x => x.ParameterDefinition)
                                .Where(x => x.ParameterDefinition!.ParamType == paramType && x.LanguageId == languageId && x.IsActive && !x.IsDeleted)
                                .ToListAsync(cancellationToken);
        }

        public async Task<string?> ParamCodeToParamValue(string paramType, int paramCode, int languageId, CancellationToken cancellationToken = default)
        {
            return await context.ParameterValues.Where(v => v.ParameterDefinition!.ParamType == paramType && v.ParamCode == paramCode && v.LanguageId == languageId).Select(v => v.ParamValue).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int?> ParamValueToParamCode(string paramType, string paramValue, CancellationToken cancellationToken = default)
        {
            return await context.ParameterValues.Where(v => v.ParameterDefinition!.ParamType == paramType && v.ParamValue == paramValue).Select(v => v.ParamCode).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
