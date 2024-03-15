namespace mycalcul
{
    public partial class Form1 : Form
    {
        static int clear = 0;
        static bool flag_clear = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            ++clear;
            if (clear % 2 == 0) { textBox2.Text = ""; }
        }

        private void _7_Click(object sender, EventArgs e)
        {
            string str = ((Button)sender).Text;
            string TextBox = textBox1.Text;
            if (str == "," && (TextBox == "" || "+-,()×÷".Contains(TextBox[TextBox.Length - 1])))
            {
                textBox1.Text += "0,";
            }
            else { textBox1.Text += str; }
        }

        private void Del_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
        }

        private void minus_Click(object sender, EventArgs e)
        {
            string TextBox = textBox1.Text;
            string str = ((Button)sender).Text;
            if (TextBox != "" && TextBox != ".")
            {
                if (str == "-" && "0123456789".Contains(TextBox[TextBox.Length - 1]))
                {
                    textBox1.Text += "+";
                }
                textBox1.Text += str;
            }
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            BKT bkt = new BKT();
            /*try
            {*/
                textBox2.Text = bkt.BKT_sol(textBox1.Text);
           /* }
            catch
            {*/
                //textBox2.Text = "Неверное выражение";
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void sin_Click(object sender, EventArgs e)
        {
            //Check.Trig = true;
            string button = ((Button)sender).Text;
            textBox1.Text += button.Remove(button.Length - 2);
        }

        private void pow2_Click(object sender, EventArgs e)
        {
            //Check.Pow = true;
            if (textBox1.Text != "" && "123456789)".Contains(textBox1.Text[textBox1.Text.Length - 1]))
            {
                textBox1.Text += '^';
            }
            if (((Button)sender).Text == "√x")
            {
                textBox1.Text += "0,5";
            }
        }
    }
}