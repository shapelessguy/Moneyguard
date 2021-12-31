using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;


namespace Moneyguard
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
            RegistrationForm_Load(null, null);
        }

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "Pe6v73iZHcC1psVlv3Dgixq31PWQz8LbwRFE6kiZ",
            BasePath = "https://mobilemoneyguard.firebaseio.com/",
        };
        IFirebaseClient client;

        void RegistrationForm_Load(object sender, EventArgs e)
        {

            try
            {
                client = new FireSharp.FirebaseClient(ifc);
            }
            catch
            {
                MessageBox.Show("No Internet or Connection Problem");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region Condition
            if (string.IsNullOrWhiteSpace(UsernameTbox.Text) &&
                string.IsNullOrWhiteSpace(passTbox.Text) &&
                string.IsNullOrWhiteSpace(GenderCbox.Text) &&
                string.IsNullOrWhiteSpace(nameTbox.Text) &&
                string.IsNullOrWhiteSpace(nicTbox.Text)) { MessageBox.Show("Please Fill all the fields"); }
            #endregion
            MyUser user = new MyUser()
            {
                Email = UsernameTbox.Text,//.Replace("^^", "@").Replace("^", "."),
                Password = passTbox.Text,
                Gender = GenderCbox.Text,
                Fullname = nameTbox.Text,
                NICno = nicTbox.Text,
            };

            try { SetResponse set = client.Set(@"Users/" + UsernameTbox.Text.Replace("@", "").Replace(".", ""), user); } catch (Exception ex) { MessageBox.Show(ex.Message); }
            //try { Login.SendMail(UsernameTbox.Text.Trim(), "Credentials verify", "Ciao"); } catch (Exception ex) { MessageBox.Show(ex.Message); }
            MessageBox.Show("Successfully registered!");
        }
    }
}
