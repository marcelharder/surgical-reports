namespace surgical_reports.implementations;

public class CABGRepo : ICABGRepo
{
    private readonly DapperContext _context;

    public CABGRepo(DapperContext context)
    {
        _context = context;
    }

    public async Task<Class_CABG> getSpecificCABG(int id)
    {
        var query = "SELECT * FROM CABGS WHERE id = @id";
        using (var connection = _context.CreateConnection())
        {
            var report = await connection.QuerySingleOrDefaultAsync<Class_CABG>(query, new { id });
            return report;
        }
    }


}