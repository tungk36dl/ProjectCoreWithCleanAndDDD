using Microsoft.EntityFrameworkCore;
using ProjectCore.Domain.Entities;
using ProjectCore.Domain.Interfaces;
using ProjectCore.Domain.ValueObjects.User;
using ProjectCore.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        }

        public async Task<User?> GetByUserNameAsync(
            UserName userName,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    x => x.UserName.Value == userName.Value,
                    cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(
            Email email,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Email.Value == email.Value,
                    cancellationToken);
        }

        public async Task AddAsync(
            User user,
            CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }

        public Task<User?> GetByUserNameOrEmailAsync(string userNameOrEmail, CancellationToken cancellationToken = default)
        {
            return _context.Users
                .FirstOrDefaultAsync(x => x.UserName.Value == userNameOrEmail || x.Email.Value == userNameOrEmail, cancellationToken);
        }

        public Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            return _context.Users.AnyAsync(x => x.Email.Value == email.Value, cancellationToken);
        }

        public Task<bool> ExistsByUserNameAsync(UserName userName, CancellationToken cancellationToken = default)
        {
            return _context.Users.AnyAsync(x => x.UserName.Value == userName.Value, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(x => x.UserRoles)
                .ToListAsync(cancellationToken);
        }
    }

}
