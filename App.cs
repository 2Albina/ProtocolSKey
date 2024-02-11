using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProtocolSKey
{
    public partial class App : Form
    {
        public readonly UserRepository _userRepository;
        public Client client;
        public Server server;
        public App()
        {
            InitializeComponent();
            _userRepository = new UserRepository();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form about = new About();
            about.ShowDialog();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void пользовательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form clientForm = new Client(this, _userRepository);
            пользовательToolStripMenuItem.Enabled = false;
            clientForm.Show();
        }

        private void серверToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form serverForm = new Server(this, _userRepository);
            серверToolStripMenuItem.Enabled = false;
            serverForm.Show();
        }

    }
}
