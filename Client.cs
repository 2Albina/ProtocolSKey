using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ProtocolSKey
{
    public partial class Client : Form
    {
        private App _parent;
        public readonly UserRepository _userRepository;
        
        string userName;
        public int userNumber;
        public int userCount;
        public string userOneTimePassword;
        string userPassword;
        public int userTotalCount;

        public Client(App parent, UserRepository userRepository)
        {
            InitializeComponent();
            _parent = parent;
            _parent.client = this;
            _userRepository = userRepository;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_parent.server != null) {
                userName = textBox1.Text;

                _parent.server.label1.Text = "";
                _parent.server.label2.Text = "";
                _parent.server.label3.Text = "";
                message1.Text = "";
                valueN.Text = "";
                valueM.Text = "";
                Answer.Text = "";
                textBox2.Text = "";
                if (_userRepository.HasUser(userName))
                {
                    _parent.server.userName = userName;
                    _parent.server.label1.Text = "Сообщение: Логин получен";
                    _parent.textBox1.Text += DateTime.Now.ToString("HH:mm:ss tt")
                        + $"Клиент {userName} отправил логин серверу\r\n";
                }
                else
                {
                    MessageBox.Show($"Пользователя с именем \"{userName}\"  не существует.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
                MessageBox.Show($"Окна сервера не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            _parent.пользовательToolStripMenuItem.Enabled = true;
            _parent.client = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_parent.server != null)
            {
                userPassword = textBox2.Text;
                string hashPassword = "";

                _parent.server.label2.Text = "";
                _parent.server.label3.Text = "";
                Answer.Text = "";

                if (message1.Text != "")
                {
                    hashPassword = _userRepository.HashCount(userPassword, userNumber, userCount);
                    _parent.server.userHashPassword = hashPassword;
                    _parent.server.label2.Text = "Сообщение: хэш пароля получен";
                    _parent.textBox1.Text += DateTime.Now.ToString("HH:mm:ss tt")
                        + $"Хэш пароля получен сервером от клиента {userName}\r\n";
                }
            }
            else
                MessageBox.Show($"Окна сервера не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
