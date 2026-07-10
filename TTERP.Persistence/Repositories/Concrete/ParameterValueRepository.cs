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

        public async Task<string?> GetParamTypeByValueAsync(int id, CancellationToken cancellationToken)
        {
            return await context.ParameterValues
                                .Where(x => x.Id == id && x.IsActive && !x.IsDeleted)
                                .Select(x => x.ParameterDefinition!.ParamType)
                                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<string?> ParamCodeToParamValue(string paramType, int paramCode, CancellationToken cancellationToken = default)
        {
            return await context.ParameterValues.Where(v => v.ParameterDefinition!.ParamType == paramType && v.ParamCode == paramCode).Select(v => v.ParamValue).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int?> ParamValueToParamCode(string paramType, string paramValue, CancellationToken cancellationToken = default)
        {
            return await context.ParameterValues.Where(v => v.ParameterDefinition!.ParamType == paramType && v.ParamValue == paramValue).Select(v => v.ParamCode).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
