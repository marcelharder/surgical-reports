namespace surgical_reports.implementations;

    public class ProcedureRepository : IProcedureRepository
    {
        private readonly DapperContext _context;
        private reportMapper _map;
        public ProcedureRepository(DapperContext context, reportMapper map)
        {
            _context = context;
            _map = map;
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
