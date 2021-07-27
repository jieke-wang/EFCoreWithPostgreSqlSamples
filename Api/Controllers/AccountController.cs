using System.Threading.Tasks;

using Entities;

using EntityFrameworkCore;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly PostgreDbContext _dbContext;

        public AccountController(PostgreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<Account> CreateAccountAsync([FromBody]Account account)
        {
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }

        [HttpGet("{id}")]
        public async Task<Account> GetAccountAsync([FromRoute]long id)
        {
            return await _dbContext.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPut("{id}")]
        public async Task<Account> UpdateAccountAsync([FromBody] Account account, [FromRoute]long id)
        {
            if (account.Id != id)
                throw new BadHttpRequestException("请求参数有误");

            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();

            return account;
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteAccountAsync([FromRoute]long id)
        {
            _dbContext.Accounts.Remove(new Account { Id = id });
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
