using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace OnAir.App.Logic
{
    internal class CameraStateProvider
    {
        public bool IsWebCamInUse()
        {
            // packaged apps (just like default camer app)
            using (var key = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam"))
            {
                var listOfApplications = key.GetSubKeyNames().ToList();
                if (IsWebCamInUse(listOfApplications, key))
                    return true;
            }

            // non packaged apps
            using (var key = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam\NonPackaged"))
            {
                var listOfApplications = key.GetSubKeyNames().ToList();
                if(IsWebCamInUse(listOfApplications, key))
                    return true;
            }

            return false;
        }

        private bool IsWebCamInUse(List<string> listOfApplications, RegistryKey key)
        {
            foreach (var subKeyName in listOfApplications)
            {

                using (var subKey = key.OpenSubKey(subKeyName))
                {
                    var lastUsed = subKey.GetValueNames().Contains("LastUsedTimeStop");
                    if (lastUsed)
                    {
                        var endTime = subKey.GetValue("LastUsedTimeStop") is long
                            ? (long)subKey.GetValue("LastUsedTimeStop")
                            : -1;

                        // windows 10
                        if (endTime <= 0) return true;

                    }
                }
            }
            return false;
        }
    }
}