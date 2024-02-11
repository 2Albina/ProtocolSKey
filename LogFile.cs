using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProtocolSKey
{
    public partial class LogFile : Form
    {
        private App _parent;
        public LogFile(App parent)
        {
            InitializeComponent();
            _parent = parent;
            textBox1.Text = _parent.textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var file = File.Open(fbd.SelectedPath+"\\LogFile.txt", FileMode.Create);

                using (var stream = new StreamWriter(file))
                {
                    stream.Write(textBox1.Text);
                }
                file.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = _parent.textBox1.Text;
        }
    }
}
