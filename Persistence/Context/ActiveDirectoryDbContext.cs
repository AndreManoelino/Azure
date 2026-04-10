using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Persistence.Context
{
    public class ActiveDirectoryDbContext : DbContext
    {
        public ActiveDirectoryDbContext(DbContextOptions<ActiveDirectoryDbContext> options)
            : base(options)
        {
            
        }
    }
}