

using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext: DbContext
    {
        //Db Context method can be re-used when we derived from Db Context class Name
        
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

        public DbSet<Value>  Values{ get; set; }
        public DbSet<User> Users { get; set; }
    }
}