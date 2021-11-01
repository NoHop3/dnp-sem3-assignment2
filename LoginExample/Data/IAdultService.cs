using System.Collections.Generic;
using System.Threading.Tasks;
using LoginExample.Models;

namespace LoginExample.Data
{
    public interface IAdultService
    {
        public Task<IList<Adult>> GetAdultsAsync();
        public Task AddPersonAsync(Adult adult);
        public Task RemovePersonAsync(int adultId);
        public Task<Adult> GetAdultAsync(int id);
    }
}