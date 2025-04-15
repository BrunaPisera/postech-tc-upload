using Microsoft.AspNetCore.Http;

namespace Upload.Domain.Interfaces
{
    public interface IVideoStorage
    {
        Task StoreAsync(IFormFile file, string fileName);
    }
}
