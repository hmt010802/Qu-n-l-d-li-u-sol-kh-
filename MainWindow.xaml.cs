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
using System.Data;

namespace NPP__VIIRS_AOD_
{
    public partial class MainWindow : Window
    {
        SqlConnection conn = new SqlConnection("Data Source=SKY\\SQLEXPRESS;Initial Catalog=BTL_DOTNET;User ID=sa;Password=admin");
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool AllowLogin()
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

        private void Login_txt_Click(object sender, RoutedEventArgs e)
        {
            if (!AllowLogin())
            {
                return;
            }

            try
            {
                string sql = "SELECT * FROM login WHERE username = '" + username.Text + "' AND password = '" + password.Password.Trim() + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                int count = 0;
                while (rdr.Read())
                {
                    count++;
                    if (rdr.GetString(0).ToString() == "admin" && rdr.GetString(1).ToString() == "admin")
                    {

                        MessageBox.Show("Dang nhap thanh cong trang quan ly");
                        solnpp_quanly solql = new solnpp_quanly();
                        this.Close();
                        solql.Show();
                        break;
                    }
                    if (rdr.GetString(0) != "admin" && rdr.GetString(1) != "admin")
                    {
                        MessageBox.Show("Dang nhap thanh cong trang xem thong tin");
                        hienthiTT hienthiinfo = new hienthiTT();
                        this.Close();
                        hienthiinfo.Show();
                        break;
                    }
                }
                if (count == 0)
                {
                    MessageBox.Show("Tai Khoan Hoac Mat Khau Khong Dung!");
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}