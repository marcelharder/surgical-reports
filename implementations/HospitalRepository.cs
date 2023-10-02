using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using surgical_reports.helpers;

namespace surgical_reports.implementations;

public class HospitalRepository : IHospitalRepository
{
    private readonly DapperContext _context;
    private IMapper _map;
   
    public HospitalRepository(DapperContext context, IMapper map)
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
            var result = _map.Map<HospitalForReturnDTO>(report);

        return result;
        }
    }
}
