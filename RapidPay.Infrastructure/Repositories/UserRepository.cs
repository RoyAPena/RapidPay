using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.User;
using RapidPay.Infrastructure.Data;

namespace RapidPay.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RapidPayContext _context;

        public UserRepository(RapidPayContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAndPassword(string userName, string password, CancellationToken cancelationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == userName && u.Password == password, cancelationToken);
        }

        public async Task Insert(User user, CancellationToken cancelationToken)
        {
            await _context.Users.AddAsync(user, cancelationToken);
        }

        public async Task<bool> ExistsUsername(string username, CancellationToken cancelationToken)
        {
            return await _context.Users.AnyAsync(x => x.Username == username, cancelationToken);
        }
    }
}