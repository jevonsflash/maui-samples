using System.Threading.Tasks;

namespace AMap.Location
{
    public interface ILocationResolveProvider
    {

        Task<GecodeLocation> GeocodeAsync(string address, string city = null);

        Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng, int radius = 50);
    }
}
