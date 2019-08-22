using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Hotel_Management_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'hotel_ResourcesDataSet2.STAFF' table. You can move, or remove it, as needed.
            this.sTAFFTableAdapter1.Fill(this.hotel_ResourcesDataSet2.STAFF);
            
        }

        public static string name
        {
            get;
            set;
        }

        public static string position
        {
            get;
            set;
        }

        public static string type
        {
            get;
            set;
        }

        public static string lname
        {
            get;
            set;
        }

        public static string username
        {
            get;
            set;
        }

        public static string password
        {
            get;
            set;
        }

        public static string id
        {
            get;
            set;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Hide();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Form1.ActiveForm.Hide();
                e.SuppressKeyPress = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OleDbConnection sql = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Hotel_Resources.accdb;");
            OleDbCommand cmd= new OleDbCommand();

            try
            {
                using (cmd = new OleDbCommand("SELECT * from Staff WHERE Username = @username AND PWord = @password"))
                {
                    cmd.Connection = sql;
                    sql.Open();
                    cmd.Parameters.AddWithValue("@username", textBox1.Text);
                    cmd.Parameters.AddWithValue("@password", textBox2.Text);
                    using (OleDbDataReader re = cmd.ExecuteReader())
                    {
                        if (re.Read())
                        {
                            id = (string)re["ID"].ToString();
                            name = (string)re["FirstName"];
                            position = (string)re["Position"];
                            type = (string)re["Type"];
                            lname = (string)re["LastName"];
                            username = (string)re["Username"];
                            password = (string)re["PWord"];
                            MainHub enter = new MainHub();
                            this.Hide();
                            enter.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid! Please enter the correct username or password.");
                        }
                    }
                    sql.Close();
                    sql.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }
    }
}
