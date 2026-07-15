using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Domain.Interfaces
{
    public interface IParameterValueRepository : IBaseRepository<ParameterValue>
    {
        Task<ParameterValue?> GetByParamTypeAndCodeAsync(string paramType, int targetStatusCode, int languageId, CancellationToken cancellationToken = default);
        Task<ParameterValue?> GetByShortCodeAsync(string paramType, string shortCode, int languageId, CancellationToken cancellationToken);
        Task<string?> GetParamTypeByValueAsync(int id, int languageId, CancellationToken cancellationToken);
        Task<List<ParameterValue>> GetParamValuesByParamTypeAsync(string paramType, int languageId, CancellationToken cancellationToken);
        Task<string?> ParamCodeToParamValue(string paramType, int paramCode, int languageId, CancellationToken cancellationToken = default);
        Task<int?> ParamValueToParamCode(string paramType, string paramValue, CancellationToken cancellationToken = default);
    }
}
