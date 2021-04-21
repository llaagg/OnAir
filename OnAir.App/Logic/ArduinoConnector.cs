using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;

namespace OnAir.App.Logic
{
    public class ArduinoConnector
    {
        ///http://www.wch.cn/download/CH341SER_ZIP.html

        private static System.IO.Ports.SerialPort _port = null;

        public bool IsConnected()
        {
            var port = GetPort();
            if (port != null && port.IsOpen)
            {
                return true;
            }

            if (port != null)
            {
                try
                {
                    port.Open();
                    if (port.IsOpen)
                        return true;
                }
                catch
                {
                    port = null;
                }
            }

            return false;
        }

        public void On()
        {
            Send("1");
        }

        public void Off()
        {
            Send("0");
        }

        public void Send(string data = "1")
        {
            var port = GetPort();
            if (port != null && port.IsOpen)
            {
                PortWrite(data);

                var response = PortRead();
            }
        }

        private string PortRead()
        {
            try
            {
                return _port.ReadLine();
            }
            catch
            {
                return null;
            }
            
        }

        public static IEnumerable<(string port, string description)> GetComPorts()
        {
            
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_PnPEntity");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            var listOfPorts = SerialPort.GetPortNames();

            var devs = searcher.Get();
            
            foreach (ManagementObject item in devs)
            {
                string desc = item["Description"]?.ToString();
                string deviceId = item["DeviceID"]?.ToString();
                var name = item["name"]?.ToString();

                var anyPort = listOfPorts.FirstOrDefault(c => name?.Contains((string) c, StringComparison.CurrentCultureIgnoreCase) ?? false);
                if (!string.IsNullOrEmpty(name) &&
                    !string.IsNullOrWhiteSpace(anyPort)
                    )
                {
                    yield return (anyPort, $"{name} {desc} {deviceId}");
                }
            }
        }

        private static SerialPort GetPort(string portName= null)
        {
            if (_port != null)
            {
                return _port;
            }

            if (portName == null)
            {
                var ports = GetComPorts().ToList();
                if (ports.Count == 1)
                {
                    portName = ports[0].port;
                }
                else
                {
                    return null;
                }
            }

            _port = new SerialPort(portName, 9600);
            _port.Open();
            return _port;
        }

        void PortWrite(string message)        
        {
            if (_port != null && _port.IsOpen)
            {
                _port.Write(message);
            }
        }
    }
}
