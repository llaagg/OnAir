using OnAir.App.Model;
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
        private string portName;

        public ArduinoConnector(string portName)
        {
            this.portName = portName;
        }

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

                if (PortRead() != "OK")
                {
                }
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

        public static IEnumerable<Port> GetComPorts()
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
                    !string.IsNullOrWhiteSpace(anyPort))
                {
                    yield return  new Port()
                    {
                        Name = anyPort,
                        Description= $"{name} {desc} {deviceId}"
                    } ;
                }
            }
        }

        private SerialPort GetPort()
        {
            if (_port != null) return _port;

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