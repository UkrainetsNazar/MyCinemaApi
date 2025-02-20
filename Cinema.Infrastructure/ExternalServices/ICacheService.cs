using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Infrastructure.ExternalServices
{
    public interface ICacheService
    {
        T? Data<T>(string key);
        void SetData<T>(string key, T data, int durationInMinutes);
        void ClearDataByPattern(string pattern);
    }
}
