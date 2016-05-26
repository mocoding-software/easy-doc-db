using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Mocoding.EasyDocDb.CrudSample.Models;

namespace Mocoding.EasyDocDb.CrudSample.DataAccess
{
    public class CustomUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private readonly IDocumentCollection<ApplicationUser> _users;

        public CustomUserStore(IDocumentCollection<ApplicationUser> users)
        {
            _users = users;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var newUser = _users.New();
            newUser.Data.Id = user.Id;
            newUser.Data.LockoutEnabled = user.LockoutEnabled;
            newUser.Data.LockoutEnd = user.LockoutEnd;
            newUser.Data.NormalizedEmail = user.NormalizedEmail;
            newUser.Data.NormalizedUserName = user.NormalizedUserName;
            newUser.Data.PasswordHash = user.PasswordHash;
            newUser.Data.PhoneNumber = user.PhoneNumber;
            newUser.Data.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            newUser.Data.Email = user.Email;
            newUser.Data.UserName = user.UserName;
            await newUser.Save();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var currentUser = _users.All().FirstOrDefault(i => i.Data.Id == user.Id);
            await currentUser.Delete();
            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _users.All().FirstOrDefault(i => i.Data.Id == userId);
            return Task.FromResult(user?.Data ?? null);
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _users.All().FirstOrDefault(i => i.Data.NormalizedUserName == normalizedUserName);
            return Task.FromResult(user?.Data ?? null);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            await Task.FromResult(user.NormalizedUserName = normalizedName);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }

        public async Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            await Task.FromResult(user.UserName = userName);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var currentUser = _users.All().FirstOrDefault(i => i.Data.Id == user.Id);     
            currentUser.Data.LockoutEnabled = user.LockoutEnabled;
            currentUser.Data.LockoutEnd = user.LockoutEnd;
            currentUser.Data.NormalizedEmail = user.NormalizedEmail;
            currentUser.Data.NormalizedUserName = user.NormalizedUserName;
            currentUser.Data.PasswordHash = user.PasswordHash;
            currentUser.Data.PhoneNumber = user.PhoneNumber;
            currentUser.Data.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            currentUser.Data.Email = user.Email;
            currentUser.Data.UserName = user.UserName;
            await currentUser.Save();
            return IdentityResult.Success;
        }
    }
}
