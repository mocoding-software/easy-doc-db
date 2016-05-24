using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Mocoding.EasyDocDb.CrudSample.DataAccess
{
    public class CustomRoleStore : IRoleStore<IdentityRole<string>>
    {
        private readonly IDocumentCollection<IdentityRole<string>> _roles;

        public CustomRoleStore(IDocumentCollection<IdentityRole<string>> roles)
        {
            _roles = roles;
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole<string> role, CancellationToken cancellationToken)
        {
            var newRole = _roles.New();
            newRole.Data.NormalizedName = role.NormalizedName;
            newRole.Data.Name = role.Name;
            newRole.Data.Id = role.Id;
            newRole.Data.ConcurrencyStamp = role.ConcurrencyStamp;
            await newRole.Save();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole<string> role, CancellationToken cancellationToken)
        {
            var currentRole = _roles.All().FirstOrDefault(i => i.Data.Id == role.Id);
            await currentRole.Delete();
            return IdentityResult.Success;
        }

        public void Dispose()
        {

        }

        public Task<IdentityRole<string>> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var role = _roles.All().FirstOrDefault(i => i.Data.Id == roleId);
            return Task.FromResult(role.Data);
        }

        public Task<IdentityRole<string>> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = _roles.All().FirstOrDefault(i => i.Data.NormalizedName == normalizedRoleName);
            return Task.FromResult(role.Data);
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole<string> role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(IdentityRole<string> role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(IdentityRole<string> role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public async Task SetNormalizedRoleNameAsync(IdentityRole<string> role, string normalizedName, CancellationToken cancellationToken)
        {
            await Task.FromResult(role.NormalizedName = normalizedName);
        }

        public async Task SetRoleNameAsync(IdentityRole<string> role, string roleName, CancellationToken cancellationToken)
        {
            await Task.FromResult(role.Name = roleName);
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole<string> role, CancellationToken cancellationToken)
        {
            var currentRole = _roles.All().FirstOrDefault(i => i.Data.Id == role.Id);
            currentRole.Data.NormalizedName = role.NormalizedName;
            currentRole.Data.Name = role.Name;
            currentRole.Data.Id = role.Id;
            currentRole.Data.ConcurrencyStamp = role.ConcurrencyStamp;
            await currentRole.Save();
            return IdentityResult.Success;
        }
    }
}
