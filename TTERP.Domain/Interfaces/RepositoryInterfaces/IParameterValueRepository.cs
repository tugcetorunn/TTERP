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
        Task<string?> GetParamTypeByValueAsync(int id, CancellationToken cancellationToken);
        Task<string?> ParamCodeToParamValue(string paramType, int paramCode, CancellationToken cancellationToken = default);
        Task<int?> ParamValueToParamCode(string paramType, string paramValue, CancellationToken cancellationToken = default);
    }
}
