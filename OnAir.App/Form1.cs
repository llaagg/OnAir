using System;
using System.Threading;
using System.Windows.Forms;
using OnAir.App.Logic;

namespace OnAir.App
{
    public partial class Form1 : Form
    {
        private bool _isArduinoConnected;

        public ArduinoConnector arduinoClient = new();

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var isUsed = new CameraStateProvider().IsWebCamInUse();
            if (this.arduinoClient.IsConnected())
            {
                if(isUsed)
                    this.arduinoClient.On();
                else
                {
                    this.arduinoClient.Off();
                }
            }

            if (InvokeRequired) 
                Invoke(new ParameterizedThreadStart(SetState), isUsed);
            else
                SetState(isUsed);
        }

        private void SetState(object? obj)
        {
            var isUsed = (bool) obj;
            checkBox1.Checked = isUsed;

            textBox1.Text = $"Camera: {isUsed} Arduino {_isArduinoConnected}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _isArduinoConnected = arduinoClient.IsConnected();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}