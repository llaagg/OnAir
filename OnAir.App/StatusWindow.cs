using System;
using System.Threading;
using System.Windows.Forms;
using OnAir.App.Logic;
using System.Linq;
using System.Collections.Generic;
using OnAir.App.Model;

namespace OnAir.App
{
    public partial class StatusWindow : Form
    {

        public ArduinoConnector arduinoClient = null;
        private CameraStateProvider cameraState = new CameraStateProvider();

        private bool _isCameraUsed = false;
        private bool _isArduinoConnected=false;
        private string selectPort;

        public List<Port> Ports { get; private set; }

        public StatusWindow()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.selectPort != null)
            {
                this.arduinoClient = new ArduinoConnector(this.selectPort);
                this._isCameraUsed = cameraState.IsWebCamInUse();
                this._isArduinoConnected = this.arduinoClient.IsConnected();

                if (_isArduinoConnected)
                {
                    if (_isCameraUsed)
                        this.arduinoClient.On();
                    else
                    {
                        this.arduinoClient.Off();
                    }
                }

                if (InvokeRequired)
                    Invoke(new ThreadStart(SetState));
                else
                    SetState();
            }
        }

        private void SetState()
        {
            tbStatus.Text = $"Camera: {_isCameraUsed} Arduino {_isArduinoConnected}";
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void StatusWindow_Load(object sender, EventArgs e)
        {
            this.Ports =
                ArduinoConnector
                .GetComPorts()
                .ToList();

            this.comboBox1.Items.AddRange(Ports.ToArray());
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectPort = (comboBox1.SelectedItem as Port)?.Name;

        }
    }
}