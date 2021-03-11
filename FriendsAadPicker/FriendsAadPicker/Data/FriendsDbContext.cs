using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FriendsAadPicker.Data
{
    public class FriendsDbContext : DbContext
    {
        public FriendsDbContext(DbContextOptions<FriendsDbContext> options) : base(options) { }

        public DbSet<Friend> Friends { get; set; }
        
        public async Task<Friend> AddFriend(Friend newFriend)
        {
            Friends.Add(newFriend);
            await SaveChangesAsync();
            return newFriend;
        }
        public async Task RemoveFriend(Friend friend)
        {
            Friends.Remove(friend);
            await SaveChangesAsync();
        }
    }
    class FriendsContextFactory : IDesignTimeDbContextFactory<FriendsDbContext>
    {
        public FriendsDbContext CreateDbContext(string[]? args = null)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<FriendsDbContext>();
            optionsBuilder
                // Uncomment the following line if you want to print generated
                // SQL statements on the console.
                // .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            return new FriendsDbContext(optionsBuilder.Options);
        }
    }
}
