using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;

namespace OnAir.App.Logic
{
    public class ArduinoConnector
    {
        ///http://www.wch.cn/download/CH341SER_ZIP.html
        private static SerialPort _port;

        public bool IsConnected()
        {
            var port = GetPort();
            if (port != null && port.IsOpen) return true;

            if (port != null)
                try
                {
                    port.Open();
                    if (port.IsOpen)
                        return true;
                }
                catch
                {
                    _port = null;
                }

            return false;
        }

        public void On()
        {
            Send();
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
            var connectionScope = new ManagementScope();
            var serialQuery = new SelectQuery("SELECT * FROM Win32_PnPEntity");
            var searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            var listOfPorts = SerialPort.GetPortNames();

            var devs = searcher.Get();

            foreach (ManagementObject item in devs)
            {
                var desc = item["Description"]?.ToString();
                var deviceId = item["DeviceID"]?.ToString();
                var name = item["name"]?.ToString();

                var anyPort = listOfPorts.FirstOrDefault(c =>
                    name?.Contains(c, StringComparison.CurrentCultureIgnoreCase) ?? false);
                if (!string.IsNullOrEmpty(name) &&
                    !string.IsNullOrWhiteSpace(anyPort)
                )
                    yield return (anyPort, $"{name} {desc} {deviceId}");
            }
        }

        private static SerialPort GetPort(string portName = null)
        {
            if (_port != null) return _port;

            if (portName == null)
            {
                var ports = GetComPorts().ToList();
                if (ports.Count == 1)
                    portName = ports[0].port;
                else
                    return null;
            }

            _port = new SerialPort(portName, 9600);
            _port.ReadTimeout = 2000;
            _port.Open();
            return _port;
        }

        private void PortWrite(string message)
        {
            if (_port != null && _port.IsOpen)
                try
                {
                    _port.Write(message);
                }
                catch
                {
                    _port = null;
                }
        }
    }
}