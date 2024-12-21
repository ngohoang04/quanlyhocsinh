using System;
using System.Data.SqlClient;
using System.Windows;

namespace MathLearningApp
{
    public partial class AddClassWindow : Window
    {
        private int _teacherID; // Lưu ID của giáo viên

        public AddClassWindow(int teacherID) // Nhận ID của giáo viên từ MainWindow
        {
            InitializeComponent();
            _teacherID = teacherID;
        }

        private void AddClassButton_Click(object sender, RoutedEventArgs e)
        {
            string className = ClassNameTextBox.Text;
            string description = DescriptionTextBox.Text;

            // Kiểm tra tên lớp học không để trống
            if (string.IsNullOrEmpty(className))
            {
                MessageBox.Show("Tên lớp học không thể trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Thêm lớp học với TeacherID vào cơ sở dữ liệu
                string query = "INSERT INTO Classes (ClassName, Description, TeacherID) VALUES (@ClassName, @Description, @TeacherID)";
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@ClassName", className),
                    new SqlParameter("@Description", description),
                    new SqlParameter("@TeacherID", _teacherID) // Lưu ID của giáo viên
                };

                DatabaseHelper.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Thêm lớp học thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm lớp học: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
