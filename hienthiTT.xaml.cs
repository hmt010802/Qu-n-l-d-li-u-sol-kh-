using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace NPP__VIIRS_AOD_
{
    public partial class hienthiTT : Window
    {
        SqlConnection conn = new SqlConnection("Data Source=SKY\\SQLEXPRESS;Initial Catalog=BTL_DOTNET;User ID=sa;Password=admin");
        public hienthiTT()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            string query = "Select * from SOLNPP";
            cmd.CommandText = query;
            adapter.SelectCommand = cmd;
            cmd.Connection = conn;
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            conn.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }
    }
}