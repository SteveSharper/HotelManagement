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
using System.IO;

namespace Hotel_Management_System
{
    public partial class MainHub : Form
    {
        Timer t = new Timer();

        public MainHub()
        {
            InitializeComponent();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Form1 back = new Form1();
            this.Hide();
            back.Show();
        }
        //Loads all relevant tables, sets Welcome Message (top left), starts clock, and displays appropriate panel based on user's credentials.
        //If the Employye is type "EMP", they have no additional features past the ones set by default. No specialized panel is loaded.
        private void MainHub_Load(object sender, EventArgs e)
        {
            this.sTAFFTableAdapter.Fill(this.hotel_ResourcesDataSet2.STAFF);
            this.financesTableAdapter.Fill(this.hotel_ResourcesDataSet2.Finances);
            this.reservationsTableAdapter.Fill(this.restaurant_ResorucesDataSet.Reservations);
            this.inventoryTableAdapter.Fill(this.restaurant_ResorucesDataSet.Inventory);
            this.menuTableAdapter.Fill(this.restaurant_ResorucesDataSet.Menu);
            this.billTableAdapter.Fill(this.customer_InfoDataSet.Bill);
            this.billTableAdapter.Fill(this.customer_InfoDataSet.Bill);
            this.customersTableAdapter.Fill(this.customer_InfoDataSet.Customers);

            string name = Form1.name;
            string position = Form1.position;
            string type = Form1.type;
            Label2.Text = "Welcome! Now logged in as " + name + " (" + position + ")";
            Label3.Text = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Year.ToString();
            t.Interval = 1000;
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();

            if (type == "MAN")
            {
                pnlManager.Visible = true;
                button22.Visible = true;
            }
            if (type == "ADM")
            {
                pnlAdmin.Visible = true;
            }
            if (type == "LBC")
            {
                pnlLobbyCl.Visible = true;
            }
            if (type == "RST")
            {
                pnlRestaraunt.Visible = true;
            }
            
        }
        //Sets Clock with Current Date and Time (Top Right corner)
        private void t_Tick(object sender, EventArgs e)
        {
            int hh = DateTime.Now.Hour;
            int mm = DateTime.Now.Minute;
            int ss = DateTime.Now.Second;
            string m = "";
            string time = "";
            if (hh < 10)
            {
                time += "0" + hh;
            }
            if (hh > 12)
            {
                time += hh - 12;
                m = "PM";
            }
            else
            {
                time += hh;
                if (hh < 12)
                {
                    m = "AM";
                }
            }
            time += ":";

            if (mm < 10)
            {
                time += "0" + mm;
            }
            else
            {
                time += mm;
            }
            time += ":";

            if (ss < 10)
            {
                time += "0" + ss;
            }
            else
            {
                time += ss;
            }

            Label4.Text = time+m;
        }
        //User Info; Loads Textboxes with User's Information
        private void Button2_Click(object sender, EventArgs e)
        {
            pnlUserInfo.BringToFront();
            pnlUserInfo.Visible = true;
            textBox1.Text = Form1.id;
            textBox2.Text = Form1.position;
            textBox3.Text = Form1.name;
            textBox4.Text = Form1.lname;
            textBox5.Text = Form1.username;
            textBox6.Text = Form1.password;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            pnlCheck.BringToFront();
            pnlCheck.Visible = true;
            CheckTime.Text = Label4.Text;
        }
        //Returns User to Main Menu
        private void Button19_Click(object sender, EventArgs e)
        {
            pnlCheck.Visible = false;
            pnlCheck.SendToBack();
            pnlUserInfo.Visible = false;
            pnlUserInfo.SendToBack();
            pnlAnnouncements.Visible = false;
            pnlAnnouncements.SendToBack();
            pnlPay.Visible = false;
            pnlPay.SendToBack();
            pnlReservations.Visible = false;
            pnlReservations.SendToBack();
            pnlRestReservation.Visible = false;
            pnlRestReservation.SendToBack();
            pnlInventory.Visible = false;
            pnlInventory.SendToBack();
            pnlMenu.Visible = false;
            pnlMenu.SendToBack();
            pnlSchedule.Visible = false;
            pnlSchedule.SendToBack();
            pnlFinances.Visible = false;
            pnlFinances.SendToBack();
            pnlEmpInfo.Visible = false;
            pnlEmpInfo.SendToBack();
            pnlFAQ.Visible = false;
            pnlFAQ.SendToBack();
        }
        //Update Username and/or Password
        private void button21_Click(object sender, EventArgs e)
        {
            string newusername = textBox5.Text;
            string newpassword = textBox6.Text;

            OleDbConnection sql = new OleDbConnection();
            sql.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Hotel_Resources.accdb;";
            OleDbCommand cmd = new OleDbCommand();
            string uname = Form1.username;
            string pword = Form1.password;

            try
            {
                if (textBox5.Text != "" | textBox5.Text != uname)
                {
                    sql.Open();
                        cmd.Connection = sql;
                        cmd.CommandText = "UPDATE Staff SET Username= @username WHERE FirstName= @id";
                        cmd.Parameters.AddWithValue("@username", textBox5.Text.ToString());
                        cmd.Parameters.AddWithValue("@id", textBox3.Text.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Update successful");
                }
                else
                {
                    MessageBox.Show("Error! Please enter a new username.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            pnlSchedule.BringToFront();
            pnlSchedule.Show();
        }
        //Fills ListBox with That Day's Schedule (Note: Select week of April 21st dates for example)
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            string workday = monthCalendar1.SelectionRange.Start.ToShortDateString();

            OleDbConnection sql = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=|DataDirectory|\Hotel_Resources.accdb;");
            OleDbCommand cmd = new OleDbCommand();

            try
            {
                using (cmd = new OleDbCommand("SELECT * from Schedule WHERE Workday = @day"))
                {
                    cmd.Connection = sql;
                    sql.Open();
                    cmd.Parameters.AddWithValue("@day", workday);
                    using (OleDbDataReader re = cmd.ExecuteReader())
                    {
                        if (re.Read() & listBox1.Items.Count == 0)
                        {
                            string wname = (string)re["FirstName"].ToString();
                            string start=(string)re["StartTime"].ToString();
                            string end=(string)re["EndTime"].ToString();
                            listBox1.Items.Add(wname + " (" + start + " to " + end + ")");
                        }
                        if (re.Read() & listBox1.Items.Count != 0)
                        {
                            listBox1.Items.Clear();
                            string wname = (string)re["FirstName"].ToString();
                            string start = (string)re["StartTime"].ToString();
                            string end = (string)re["EndTime"].ToString();
                            listBox1.Items.Add(wname + " (" + start + " to " + end + ")");
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
        //Employee Clock-In
        private void button23_Click(object sender, EventArgs e)
        {
            OleDbConnection sql = new OleDbConnection();
            sql.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Hotel_Resources.accdb;";
            OleDbCommand cmd = new OleDbCommand();

            try
            {
                    sql.Open();
                    if (Label4.Text.Length == 9)
                    {
                        cmd.Connection = sql;
                        cmd.CommandText = "Update Schedule Set CheckIn='" + Label4.Text.Substring(0, 4) + "' WHERE ID = " + Form1.id;
                        cmd.ExecuteNonQuery();
                        sql.Close();
                        MessageBox.Show("You are now Cheked-In. Good luck!");
                    }
                    else
                    {
                        cmd.Connection = sql;
                        cmd.CommandText = "Update Schedule Set CheckIn='" + Label4.Text.Substring(0, 5) + "' WHERE ID = " + Form1.id;
                        cmd.ExecuteNonQuery();
                        sql.Close();
                        MessageBox.Show("You are now Cheked-In. Good Luck!");
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //Employee Clock-Out
        private void button24_Click(object sender, EventArgs e)
        {
            OleDbConnection sql = new OleDbConnection();
            sql.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Hotel_Resources.accdb;";
            OleDbCommand cmd = new OleDbCommand();

            try
            {
                sql.Open();
                if (Label4.Text.Length == 9)
                {
                    cmd.Connection = sql;
                    cmd.CommandText = "Update Schedule Set CheckOut='" + Label4.Text.Substring(0,5) + "' WHERE ID = " + Form1.id;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("You are now Checked-Out. Have a good day!");
                }
                else
                {
                    cmd.Connection = sql;
                    cmd.CommandText = "Update Schedule Set CheckOut='" + Label4.Text.Substring(0, 5) + "' WHERE ID = " + Form1.id;
                    cmd.ExecuteNonQuery();
                    sql.Close();
                    MessageBox.Show("You are now Cheked-Out. Have a good day!");
                }
                sql.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //Schedule
        private void button22_Click(object sender, EventArgs e)
        {
            pnlAddSchedule.Visible = true;
        }
        //Add To Schedule
        private void button25_Click(object sender, EventArgs e)
        {
            string workday = monthCalendar1.SelectionRange.Start.ToShortDateString();
            OleDbConnection sql = new OleDbConnection();
            sql.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Hotel_Resources.accdb;";
            OleDbCommand cmd = new OleDbCommand();

            try
            {
                sql.Open();
                cmd.Connection = sql;
                cmd.CommandText = "Insert Into Schedule Values (@day, @id, @fname, @lname, @start, @end);";
                cmd.Parameters.AddWithValue("@day", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@id", textBox8.Text);
                cmd.Parameters.AddWithValue("@fname", textBox9.Text);
                cmd.Parameters.AddWithValue("@lname", textBox10.Text);
                cmd.Parameters.AddWithValue("start", textBox11.Text);
                cmd.Parameters.AddWithValue("@end", textBox12.Text);
                cmd.ExecuteNonQuery();
                sql.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Focus();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
        }
        //Announcement Tab
        private void Button1_Click(object sender, EventArgs e)
        {
            pnlAnnouncements.Visible = true;
            pnlAnnouncements.BringToFront();
            OleDbConnection sql = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Hotel_Resources.accdb;");
            OleDbCommand cmd = new OleDbCommand();

            try
            {
                using (cmd = new OleDbCommand("SELECT * from Announcements"))
                {
                    cmd.Connection = sql;
                    sql.Open();
                    using (OleDbDataReader re = cmd.ExecuteReader())
                    {

                        if (re.Read())
                        {
                            string post = (string)re["PostDate"].ToString();
                            string author = (string)re["Author"];
                            string title = (string)re["Title"];
                            string message = (string)re["Message"];
                            richTextBox1.Text = title + Environment.NewLine
                                + author + Environment.NewLine
                                + post +Environment.NewLine
                                + message;
                        }
                        else
                        {
                            richTextBox1.Text = "Looks like there are no announcements at this time. Check back later!";
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
        //Restauraunt Reservations
        private void Button12_Click(object sender, EventArgs e)
        {
            pnlReservations.Visible = true;
            pnlReservations.BringToFront();
            customersBindingNavigator.Visible = true;
        }

        private void customersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.customersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.customer_InfoDataSet);
        }
        //Guest Pay Panel Shows
        private void button27_Click(object sender, EventArgs e)
        {
            pnlPay.Visible = true;
            pnlPay.BringToFront();
        }
        //Inventory
        private void Button16_Click(object sender, EventArgs e)
        {
            pnlInventory.Visible = true;
            pnlInventory.BringToFront();
        }
        //Menu
        private void Button15_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = true;
            pnlMenu.BringToFront();
        }
        //Reservations
        private void Button17_Click(object sender, EventArgs e)
        {
            pnlRestReservation.Visible = true;
            pnlRestReservation.BringToFront();
        }
        //Manage Finances
        private void pnlFinances_Paint(object sender, PaintEventArgs e)
        {
            pnlFinances.Visible = true;
            pnlFinances.BringToFront();
        }
        //Manage Staff
        private void Button7_Click(object sender, EventArgs e)
        {
            pnlEmpInfo.Visible = true;
            pnlEmpInfo.BringToFront();
        }
        //FAQ
        private void Button11_Click(object sender, EventArgs e)
        {
            pnlFAQ.Visible = true;
            pnlFAQ.BringToFront();
            //Note: Filepath may need to be altered
            var fileStream = new FileStream(@"C:\Users\Parent\OneDrive\Documents\Visual Studio 2012\Projects\Senior Project\Hotel Management System\Hotel Management System\FAQ.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                richTextBox2.Text = streamReader.ReadToEnd();
            }
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button9_Click(object sender, EventArgs e)
        {
            pnlFinances.Visible = true;
            pnlFinances.BringToFront();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello! Thank you for vieiwng my beta HotelManagement System." + Environment.NewLine
                + "To return to the main menu, press the 'Main' button located on the left side of the screen." + Environment.NewLine
                + "To switch users, click 'Logout', also on the left of the screen." + Environment.NewLine
                + "To access additional features, logout and sign in as a new user with a different Employee Type:" + Environment.NewLine
                + "    MAN: 'Manage Finances' and view 'Employee Data'" + Environment.NewLine
                + "    LBC: Book 'Reservations' and refer to the 'FAQ'" + Environment.NewLine
                + "    RST: Display 'Menu', Book 'Reservations', and See 'Inventory'" + Environment.NewLine
                + "    ADM: 'Update' application (this feature is still in development)" + Environment.NewLine
                + "    EMP or HKR: Regular Employees have access to only the basic features ('Schedule' and 'Check-In/Out')");
        }
    }
}