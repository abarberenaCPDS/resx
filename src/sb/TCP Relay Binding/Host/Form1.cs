using System;
using System.Windows.Forms;

namespace Host
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int Counter
        {
            get => Convert.ToInt32(this.lblCounter.Text);
            set => this.lblCounter.Text = value.ToString();
        }
    }
}