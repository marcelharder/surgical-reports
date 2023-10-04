
public class ValveRepo : IValveRepo
{
    private readonly DapperContext _context;

    public ValveRepo(DapperContext context)
    {
        _context = context;
    }

    public async Task<Class_Valve> getValve(string implantPosition, int procedure_id)
    {
         var query = "SELECT * FROM Valves WHERE ImplantPosition = @implantPosition AND PROCEDURE_ID = @procedure_id";
            using (var connection = _context.CreateConnection())
            {
                var report = await connection.QuerySingleOrDefaultAsync<Class_Valve>(query, new { implantPosition, procedure_id });
                return report;
            }
    }

    public async Task<Class_Valve> getValveBySerial(string serial)
    {
         var query = "SELECT * FROM Valves WHERE SERIAL_IMP = @serial";
        using (var connection = _context.CreateConnection())
        {
            var report = await connection.QuerySingleOrDefaultAsync<Class_Valve>(query, new { serial });
            return report;
        }
    }

    

}