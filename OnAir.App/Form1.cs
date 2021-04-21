using System;
using System.Threading;
using System.Windows.Forms;
using OnAir.App.Logic;

namespace OnAir.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var isUsed = new CameraStateProvider().IsWebCamInUse();

            if (InvokeRequired) Invoke(new ParameterizedThreadStart(SetState), isUsed);
            else
            {
                SetState(isUsed);
            }
        }

        private void SetState(object? obj)
        {
            var isUsed = (bool) obj;
            checkBox1.Checked = isUsed;
        }
    }
}