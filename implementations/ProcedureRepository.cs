namespace surgical_reports.implementations;

    public class ProcedureRepository : IProcedureRepository
    {
        private readonly DapperContext _context;
       
        public ProcedureRepository(DapperContext context)
        {
            _context = context;
            
        }

    public async Task<int> getProcedureIdFromHash(string hash)
    {   var result = 0;
        var query = "SELECT * FROM Procedures WHERE emailHash = @hash";
            using (var connection = _context.CreateConnection())
            {
                var report = await connection.QuerySingleOrDefaultAsync<Class_Procedure>(query, new { hash });
                if(report != null){result = report.ProcedureId;}
                return result;
            }
    }

    public async Task<Class_Procedure> getSpecificProcedure(int id)
        {
            var query = "SELECT * FROM Procedures WHERE ProcedureId = @id";
            using (var connection = _context.CreateConnection())
            {
                var report = await connection.QuerySingleOrDefaultAsync<Class_Procedure>(query, new { id });
                return report;
            }
        }
    }
