namespace surgical_reports.implementations;

public class CPBRepo : ICPBRepo
{
    private readonly DapperContext _context;

    public CPBRepo(DapperContext context)
    {
        _context = context;
    }
    Task<Class_CPB> ICPBRepo.getSpecificCPB(int id)
    {
        var query = "SELECT * FROM CPBS WHERE id = @id";
        using(var connection = _context.CreateConnection()){
            
        }

    }
}