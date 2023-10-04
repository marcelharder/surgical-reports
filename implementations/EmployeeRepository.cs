namespace surgical_reports.implementations;

    public class EmployeeRepository : IEmployeeRepository
    {
 private readonly DapperContext _context;

    public EmployeeRepository(DapperContext context)
    {
        _context = context;
    }
        public async Task<Class_Employee> getSpecificEmployee(int id)
        {
             var query = "SELECT * FROM Employees WHERE id = @id";
        using (var connection = _context.CreateConnection())
        {
            var report = await connection.QuerySingleOrDefaultAsync<Class_Employee>(query, new { id });
            return report;
        }
        }
    }
