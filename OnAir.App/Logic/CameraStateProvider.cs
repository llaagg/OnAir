using System.Linq;
using Microsoft.Win32;

namespace OnAir.App.Logic
{
    internal class CameraStateProvider
    {
        public bool IsWebCamInUse()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam\NonPackaged"))
            {
                foreach (var subKeyName in key.GetSubKeyNames())
                    using (var subKey = key.OpenSubKey(subKeyName))
                    {
                        var lastUsed = subKey.GetValueNames().Contains("LastUsedTimeStop");
                        if (lastUsed)
                        {
                            var endTime = subKey.GetValue("LastUsedTimeStop") is long
                                ? (long) subKey.GetValue("LastUsedTimeStop")
                                : -1;
                            if (endTime <= 0) return true;
                        }
                    }
            }

            return false;
        }
    }
}