using System;
using System.Data.SqlClient;
using System.Windows;

namespace MathLearningApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = UsernameTextBox.Text;  // Sử dụng email thay vì tên đăng nhập
            string password = PasswordBox.Password;

            // Kiểm tra thông tin đăng nhập từ cơ sở dữ liệu và lấy teacherID
            int teacherID = AuthenticateTeacher(email, password);

            if (teacherID != -1)
            {
                // Nếu thông tin đăng nhập đúng, mở cửa sổ chính của giáo viên và truyền teacherID
                MainWindow main = new MainWindow(teacherID);  // Truyền teacherID vào constructor
                main.Show();
                this.Close(); // Đóng cửa sổ đăng nhập
            }
            else
            {
                MessageBox.Show("Email hoặc mật khẩu không đúng.", "Lỗi Đăng Nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int AuthenticateTeacher(string email, string password)
        {
            try
            {
                // Lấy connection string từ App.config
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MathLearningAppDB"].ConnectionString;

                // Kiểm tra nếu connection string không tồn tại
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Không tìm thấy connection string trong App.config.");
                }

                // Câu lệnh SQL để kiểm tra thông tin đăng nhập
                string query = "SELECT TeacherID, Password FROM Teachers WHERE Email = @Email";  // Sử dụng Email thay vì Username

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Sử dụng tham số để bảo vệ chống SQL Injection
                        command.Parameters.AddWithValue("@Email", email);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["Password"].ToString();
                                int teacherID = Convert.ToInt32(reader["TeacherID"]);

                                // So sánh mật khẩu với dữ liệu trong cơ sở dữ liệu
                                if (password == storedPassword)
                                {
                                    return teacherID;
                                }
                            }
                        }
                    }
                }

                return -1;  // Trả về -1 nếu không tìm thấy giáo viên hoặc mật khẩu sai
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và hiển thị thông báo lỗi cho người dùng
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi Đăng Nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }
    }
}
