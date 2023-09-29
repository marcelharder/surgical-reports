namespace surgical_reports.interfaces;

    public interface IUserRepository
    {
      
        Task<AppUser> GetUser(int id);
       
    }
