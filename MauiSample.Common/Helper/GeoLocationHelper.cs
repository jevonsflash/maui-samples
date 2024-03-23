using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSample.Common.Helper
{
    public class GeoLocationHelper
    {
        public static async Task<Microsoft.Maui.Devices.Sensors.Location> GetNativePosition()
        {

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    return location;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                CommonHelper.Alert("请在设置中开启位置的访问权限", "位置无权限");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                CommonHelper.Alert("当您的网络信号或GPS信号弱的时候，我们无法获取您的位置信息", "无法获取位置信息");
            }
            catch (PermissionException pEx)
            {
                CommonHelper.Alert("请在设置中开启位置的访问权限", "位置无权限");
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            return null;

        }

    }
}
