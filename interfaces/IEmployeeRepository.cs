namespace surgical_reports.interfaces;

    public interface IEmployeeRepository
    {
       
        Task<Class_Employee> getSpecificEmployee(int id);
       
    }
