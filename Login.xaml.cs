using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
namespace NPP__VIIRS_AOD_
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        SqlConnection conn = new SqlConnection(@"Data Source=SKY\SQLEXPRESS;Initial Catalog=BTL_DOTNET;User ID=sa;Password=admin");
        public Login()
        {
            InitializeComponent();
        }
        private bool allowLogin()
        {
            if (username.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập tài khoản", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (password.Password.Trim() == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private void Exit_txt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void login_txt_Click(object sender, RoutedEventArgs e)
        {
            ArrayList arr_list_account = new ArrayList();
            if (!allowLogin())
            { return; 
            }

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM login",conn);
                conn.Open();
                var sqldatareader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (sqldatareader.HasRows)
                {
                    while (sqldatareader.Read())
                    {
                        if(username.Text == sqldatareader.GetString(0) && password.Password == sqldatareader.GetString(1)){
                            MessageBox.Show("Dang nhap thanh cong");
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();

            //DataTable dtData = Connection
        }
    }
}
