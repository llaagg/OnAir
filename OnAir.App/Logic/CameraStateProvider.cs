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
                var listOfApplications = key.GetSubKeyNames().ToList();
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

                            //// windows 11
                            //var startTime = subKey.GetValue("LastUsedTimeStart") is long
                            //    ? (long)subKey.GetValue("LastUsedTimeStart")
                            //    : -1;
                            //if (startTime > endTime)
                            //{
                            //    return true;
                            //}
                        }
                    }
                }
            }

            return false;
        }
    }
}