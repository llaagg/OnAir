using System;
using System.Threading;
using System.Windows.Forms;
using OnAir.App.Logic;

namespace OnAir.App
{
    public partial class Form1 : Form
    {
        private bool _isArduinoConnected=false;

        public ArduinoConnector arduinoClient = null;
        private bool _isCameraUsed = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _isCameraUsed= new CameraStateProvider().IsWebCamInUse();

            this._isArduinoConnected = this.arduinoClient.IsConnected();
            if (_isArduinoConnected)
            {
                if(_isCameraUsed)
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

        private void SetState()
        {

            textBox1.Text = $"Camera: {_isCameraUsed} Arduino {_isArduinoConnected}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arduinoClient = new ArduinoConnector();
            _isArduinoConnected = arduinoClient?.IsConnected() ?? false ;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}