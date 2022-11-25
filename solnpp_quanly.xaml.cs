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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Win32;

namespace NPP__VIIRS_AOD_
{
    public partial class solnpp_quanly : Window
    {
        SqlConnection conn = new SqlConnection("Data Source=SKY\\SQLEXPRESS;Initial Catalog=BTL_DOTNET;User ID=sa;Password=admin");
        public solnpp_quanly()
        {
            InitializeComponent();
            loadData();
        }
        public void ClearData()
        {
            ID_txt.Clear();
            FileName_txt.Clear();
            Path_txt.Clear();
            SQATime_txt.Clear();
            UpdateTime_txt.Clear();
            Version_txt.Clear();
            loadData();
        }
        private void Cleardung_Click_1(object sender, RoutedEventArgs e)
        {
            ClearData();
        }



        public void loadData()
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
        public bool IsValid()
        {
            if (ID_txt.Text == String.Empty)
            {
                MessageBox.Show("ID is required", "Failer", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (FileName_txt.Text == String.Empty)
            {
                MessageBox.Show("FileName is required", "Failer", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (Path_txt.Text == String.Empty)
            {
                MessageBox.Show("Path is required", "Failer", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (SQATime_txt.Text == String.Empty)
            {
                MessageBox.Show("SQATime is required", "Failer", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (UpdateTime_txt.Text == String.Empty)
            {
                MessageBox.Show("UpdateTime is required", "Failer", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void Insertbtn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO SOLNPP VALUES (@ID, @FileNAme, @Path, @SQATime, @UpdateTime, @Version)");
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@ID", ID_txt.Text);
                    cmd.Parameters.AddWithValue("@FileName", FileName_txt.Text);
                    cmd.Parameters.AddWithValue("@Path", Path_txt.Text);
                    cmd.Parameters.AddWithValue("@SQATime", SQATime_txt.Text);
                    cmd.Parameters.AddWithValue("@UpdateTime", UpdateTime_txt.Text);
                    cmd.Parameters.AddWithValue("@Version", Version_txt.Text);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    loadData();
                    MessageBox.Show("Successfully resigter", "saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Deletebtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (ID_txt.Text.Length == 0)
            {
                MessageBox.Show("Chua Nhap ID De Xoa");
                return;
            }
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete from SOLNPP where ID = " + ID_txt.Text + " ");
            try
            {
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                conn.Close();
                ClearData();
                loadData();
                conn.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Not deleted" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Uploadbtn_Click_1(object sender, RoutedEventArgs e)
        {
            conn.Open();

            
            SqlCommand cmd = new SqlCommand("update SOLNPP set FileName = '" + FileName_txt.Text + "', " +
                                               "Path = '" + Path_txt.Text + "', " +
                                               "SQATime = '" + SQATime_txt.Text + "' , " +
                                               "UpdateTime = '" + UpdateTime_txt.Text + "' , " +
                                               "Version = '" + Version_txt.Text + "' WHERE ID = '" + ID_txt.Text + "'");
            try
            {
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
              
                MessageBox.Show("Record has been updated successfully", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                ClearData();
                loadData();
            }

        }

        private void search1_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM SOLNPP WHERE ID = '" + int.Parse(ID_txt.Text) + "'");// viết câu lệnh query để lấy dữ liệu
                DataTable dt = new DataTable();
                conn.Open();
                cmd.Connection = conn;
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                datagrid.ItemsSource = dt.DefaultView;
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
            finally  
            {
                conn.Close();
            }
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                SqlCommand cmd2 = new SqlCommand("SELECT ID FROM SOLNPP");
                DataTable dt = new DataTable();
                conn.Open();
                cmd2.Connection = conn;
                SqlDataReader reader = cmd2.ExecuteReader();
                dt.Load(reader);
                int n = dt.Rows.Count + 1;
                conn.Close();
                try
                {
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO SOLNPP VALUES (@ID,@FileName, @Path, @SQAtime, @UpdateTime , @Version)");
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@ID", n);
                        n++;
                        cmd.Parameters.AddWithValue("@FileName", System.IO.Path.GetFileName(filename));
                        cmd.Parameters.AddWithValue("@Path", filename);
                        String s = System.IO.Path.GetFileName(filename);
                        String s1 = s.Substring(3, 8);
                        String s2 = s.Substring(12, 2);
                        String s3 = s2.Insert(2, "/");
                        cmd.Parameters.AddWithValue("@SQAtime", s1);
                        cmd.Parameters.AddWithValue("@UpdateTime", s3);
                        cmd.Parameters.AddWithValue("@Version", "1");
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        loadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    MessageBox.Show("Successfully resigter", "saved", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}