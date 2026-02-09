using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.Utilities
{
    internal class Mapper
    {
        public static T2 Map<T1, T2>(T1 source, Action<IMappingExpression<T1, T2>> action = null)
        {
            var config = new MapperConfiguration(cfg =>
            {
                var mappingExpression = cfg.CreateMap<T1, T2>();
                action?.Invoke(mappingExpression);
            }
            );
            var mapper = config.CreateMapper();
            return mapper.Map<T1, T2>(source);
        }

        public static IEnumerable<T2> Map<T1, T2>(IEnumerable<T1> sources,
        Action<IMappingExpression<T1, T2>> action = null)
        {
            var config = new MapperConfiguration(cfg =>
            {
                var mappingExpression = cfg.CreateMap<T1, T2>();
                action?.Invoke(mappingExpression);
            }
            );
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<T1>, IEnumerable<T2>>(sources);
        }
    }
}
