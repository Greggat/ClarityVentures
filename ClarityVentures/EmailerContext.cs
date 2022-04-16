using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ClarityVentures
{
    internal class EmailerContext : DbContext
    {
        public DbSet<Email> Emails { get; set; }
        public string DbPath { get; }

        public EmailerContext()
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            // var path = Environment.GetFolderPath(folder);
            //DbPath = Path.Join(path, "emailer.db");

            //Database.EnsureCreated();
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            DbPath = Path.Join(path, "guildbuddy.db");

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }
}
