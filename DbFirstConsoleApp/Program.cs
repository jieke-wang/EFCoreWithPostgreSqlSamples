using System;
using System.Text.Json;
using System.Threading.Tasks;

using DALEntity.Entities;

using Microsoft.EntityFrameworkCore;

namespace DbFirstConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connStr = "User ID=demo_user; Password=password; Host=192.168.199.133; Port=5432; Database=demo_db;";
            DbContextOptionsBuilder<PostgreDbContext> builder = new DbContextOptionsBuilder<PostgreDbContext>();
            builder.UseNpgsql(connStr);

            Account objAccount = new Account 
            {
                Name = "jack",
                Age = 18,
                RegisterDate = DateTime.Now,
                RegisteredUser = true,
                Balance = 100.9m
            };

            using (PostgreDbContext dbContext = new PostgreDbContext(builder.Options))
            {
                await dbContext.Accounts.AddAsync(objAccount);
                await dbContext.SaveChangesAsync();
                PrintAccount(objAccount);

                objAccount = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == objAccount.Id);
                PrintAccount(objAccount);

                objAccount.Age += 5;
                dbContext.Accounts.Update(objAccount);
                await dbContext.SaveChangesAsync();

                objAccount = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == objAccount.Id);
                PrintAccount(objAccount);

                dbContext.Accounts.Remove(objAccount);
                await dbContext.SaveChangesAsync();
            }
        }

        private static void PrintAccount(Account objAccount)
        {
            Console.WriteLine(JsonSerializer.Serialize(objAccount));
        }
    }
}
