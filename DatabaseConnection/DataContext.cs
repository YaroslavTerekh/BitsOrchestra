using ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.DatabaseConnection
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }


        public DbSet<Contact> Contacts { get; set; }
    }
}
