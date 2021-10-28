using System.Collections.Generic;
using System.Threading.Tasks;
using LoginExampleServer.Models;

namespace LoginExampleServer.Data
{
    public interface IAdultService
    {
        Task<IList<Adult>> GetAdultsAsync();
        Task AddPersonAsync(Adult adult);
        Task RemovePersonAsync(int adultId);
        Task<Adult> GetAdultAsync(int id);
    }
}