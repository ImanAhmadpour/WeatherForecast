using Domain.GeoCoding.Models;
using Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.GeoCoding
{
    public interface IGeoCodingService
    {
        Task<(ResultStatus, List<GeoCodingData>)> GetGeoCodingByCityAsync(string city);
    }
}
