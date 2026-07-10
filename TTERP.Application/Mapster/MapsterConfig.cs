using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ParameterValues;
using TTERP.Domain.Entities;

namespace TTERP.Application.Mapster
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<ParameterValue, GetParameterValuesDTO>
                .NewConfig()
                .Map(dest => dest.ParamType, src => src.ParameterDefinition!.ParamType);
        }
    }
}
