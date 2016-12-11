using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmanDemo
{
    public interface IResolver
    {
        Task<IEnumerable<City>> CalculateAsync(IEnumerable<City> cities);

        void Calculate(IEnumerable<City> cities);
    }
}
