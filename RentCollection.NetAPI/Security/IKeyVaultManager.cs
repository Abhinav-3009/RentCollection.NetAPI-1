using System;
using System.Threading.Tasks;

namespace RentCollection.NetAPI.Security
{
    public interface IKeyVaultManager
    {
        public Task<string> GetSecret(string secretName);
    }
}

