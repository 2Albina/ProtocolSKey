using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ProtocolSKey
{
    public partial class Registration : Form
    {
        private App _parent;
        private readonly UserRepository _userRepository;

        public static bool hasLower = true;
        public static bool hasUpper = true;
        public static bool hasDigit = true;
        public static bool hasPunctuation = true;

        public Registration(UserRepository userRepository, App parent)
        {
            _userRepository = userRepository;
            _parent = parent;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var userName = LoginInput.Text;
            var password = PasswordInput.Text;
            if (_userRepository.HasUser(userName))
            {
                MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка добавления пользователя", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            if (PasswordInput.Text != PasswordInput2.Text)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка ввода пароля", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            if (!HasLimitations(password)){
                MessageBox.Show("Пароль не удовлетворяет ограничениям", "Ошибка ввода пароля", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            int count;
            try
            {
                count = Convert.ToInt32(CountInput.Text);
            }
            catch {
                MessageBox.Show("Некорректное количествo одноразовых паролей", "Ошибка ввода количества", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            Random rnd = new Random();
            int number = rnd.Next(100);
            string passwordHash = _userRepository.HashCount(password, number, count+1);
            _userRepository.AppendUser(new User(userName, number, count, passwordHash, password, count));


            _parent.textBox1.Text += DateTime.Now.ToString("HH:mm:ss tt")
            + $"Клиент {userName} зарегестрирован\r\n";

            MessageBox.Show("Пользователь \"" + userName + "\" добавлен.", "Добавление пользователя", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Close();
        }
        public static bool HasLimitations(string password)
        {
            foreach (var symbol in password)
            {
                hasLower |= char.IsLower(symbol);
                hasUpper |= char.IsUpper(symbol);
                hasDigit |= char.IsDigit(symbol);
                hasPunctuation |= char.IsPunctuation(symbol);
            }

            return hasLower && hasUpper && hasDigit && hasPunctuation;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            hasLower = !checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            hasUpper = !checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            hasDigit = !checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            hasPunctuation = !checkBox4.Checked;
        }

    }
}
