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
        var hospitalNo = id.makeSureTwoChar();
        var query = "SELECT * FROM Hospitals WHERE HospitalNo = @hospitalNo";
        using (var connection = _context.CreateConnection())
        {
            var report = await connection.QuerySingleOrDefaultAsync<Class_Hospital>(query, new { hospitalNo });
            var result = _map.Map<HospitalForReturnDTO>(report);

        return result;
        }
    }
}
