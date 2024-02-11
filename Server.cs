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
    public partial class Server : Form
    {
        private App _parent;
        public readonly UserRepository _userRepository;
        private User user;

        public string userName;
        public int userNumber;
        public int userCount;
        public string userOneTimePassword;
        public string userPassword;
        public int userTotalCount;

        public string userHashPassword; // хэш пароля, полученный от клиентв

        public Server(App parent, UserRepository userRepository)
        {
            InitializeComponent();
            _parent = parent;
            _parent.server = this;
            _userRepository = userRepository;
        }

        private void логфайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form log = new LogFile(_parent);
            log.Show();
        }

        private void регистрацияКлиентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form reg = new Registration(_userRepository, _parent);
            reg.Show(); 
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (_parent.client != null)
            {
                user = _userRepository.GetUser(userName);
                userNumber = user.Number;
                userCount = user.Count;
                userOneTimePassword = user.OneTimePassword;
                userPassword = user.Password;
                userTotalCount = user.TotalCount;

                _parent.client.userNumber = userNumber;
                _parent.client.userCount = userCount;

                _parent.client.valueN.Text = "N = " + userNumber;
                _parent.client.valueM.Text = "M = " + userCount;
                _parent.client.message1.Text = "Сообщение: Получены M, N";
                _parent.textBox1.Text += DateTime.Now.ToString("HH:mm:ss tt")
                    + $"Клиент {userName} получил от сервера значения M, N \r\n";
            }
            else
                MessageBox.Show($"Окна клиента не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            _parent.серверToolStripMenuItem.Enabled = true;
            _parent.server = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_parent.client != null)
            {
                string hash = _userRepository.HashCount(userHashPassword, userNumber, 1);
                if (hash == userOneTimePassword)
                {
                    label3.Text = "Сообщение: Пароли совпали!";
                    _parent.client.Answer.Text = "Сообщение: Успешная аутентификация!";
                    _parent.textBox1.Text += DateTime.Now.ToString("HH:mm:ss tt")
                        + $"Клиент {userName} успешно аутентифицировался \r\n";

                    if (userCount == 0)
                    {
                        Random rnd = new Random();
                        userNumber = rnd.Next(100);
                        userCount = userTotalCount;
                        userOneTimePassword = _userRepository.HashCount(userPassword, userNumber, userCount + 1);
                        _userRepository.Update(new User(userName, userNumber, userCount, userOneTimePassword, userPassword, userTotalCount));

                        _parent.client.userNumber = userNumber;
                        _parent.client.userCount = userCount;
                        _parent.client.valueN.Text = "N = " + userNumber;
                        _parent.client.valueM.Text = "M = " + userCount;
                        _parent.textBox1.Text += DateTime.Now.ToString("HH:mm:ss tt")
                        + $"Сгенерирован новый список одноразовых паролей для клиента {userName}\r\n";
                    }
                    else
                    {
                        userCount -= 1;
                        _parent.client.userCount = userCount;
                        _parent.client.valueM.Text = "M = " + userCount;
                        _userRepository.Update(new User(userName, userNumber, userCount, userHashPassword, userPassword, userTotalCount));
                    }
                }
                else
                {
                    label3.Text = "Сообщение: Пароли не совпали!";
                    _parent.client.Answer.Text = "Сообщение: Неверный пароль!";
                    _parent.textBox1.Text += DateTime.Now.ToString("HH:mm:ss tt")
                        + $"Клиент {userName} не прошёл аутентификацию\r\n";
                }
            }
            else
            {
                MessageBox.Show($"Окна клиента не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
