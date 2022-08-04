using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChengTuoChecker
{
    public partial class Form_SubImg : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        public void ShapedForm_MouseDown(object sender, EventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        public Form_SubImg()
        {
            InitializeComponent();

        }

        private void Form_SubImg_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(0, 0, 1);
            this.TransparencyKey = this.BackColor;

            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
            //ShapedForm_MouseDown(sender, e);
        }


    }
}
