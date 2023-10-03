using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using surgical_reports.helpers;

namespace surgical_reports.implementations;

public class HospitalRepository : IHospitalRepository
{
    private readonly DapperContext _context;
    private reportMapper _map;
    public HospitalRepository(DapperContext context, reportMapper map)
    {
        _context = context;
        _map = map;
    }

    public async Task<HospitalForReturnDTO> GetSpecificHospital(string id)
    {
        var query = "SELECT * FROM Hospitals WHERE HospitalNo = @id";
        using (var connection = _context.CreateConnection())
        {
            var report = await connection.QuerySingleOrDefaultAsync<Class_Hospital>(query, new { id });
        return _map.mapToHospitalForReturn(report);
        }
    }
}
