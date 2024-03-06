using ServiceModelEx.ServiceBus;
using System;
using System.Configuration;
using System.Media;
using System.Windows.Forms;

namespace Client
{
    public partial class frmClient : Form
    {
        private MyContractClient proxy;
        SoundPlayer m_Player = new SoundPlayer();

        public frmClient()
        {
            InitializeComponent();

            string sbKeyName = ConfigurationManager.AppSettings["sb-key-name"];
            string sbKeyValue = ConfigurationManager.AppSettings["sb-key-value"];

            this.proxy = new MyContractClient();
            this.proxy.SetServiceBusCredentials(sbKeyName, sbKeyValue);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string musicFile = ConfigurationManager.AppSettings["asset-name"];

            var musicFileStream = this.proxy.StreamMusic(musicFile);
            this.m_Player.Stop();
            this.m_Player.Stream = musicFileStream;
            this.m_Player.Play();

            Cursor.Current = Cursors.Default;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.proxy.Close();
        }
    }
}