namespace surgical_reports.implementations;

    public class UserRepository : IUserRepository
    {
         private readonly DapperContext _context;
       
        public UserRepository(DapperContext context)
        {
            _context = context;
            
        }
        public async Task<AppUser> GetUser(int id)
        {
             var query = "SELECT * FROM AspNetUsers WHERE Id = @id";
            using (var connection = _context.CreateConnection())
            {
                var report = await connection.QuerySingleOrDefaultAsync<AppUser>(query, new { id });
                return report;
            }
        }
    }
