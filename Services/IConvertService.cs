using Microsoft.AspNetCore.Http;
using WorkSampleExperis.Models;

namespace WorkSampleExperis.Services
{
    public interface IConvertService
    {
        ConvertResult Convert(IFormFile file);
    }
}

