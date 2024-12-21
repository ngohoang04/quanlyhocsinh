using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using MathLearningApp.Data;

namespace MathLearningApp
{
    public partial class MainWindow : Window
    {
        private int teacherID; // Lưu trữ ID của giáo viên

        // Constructor mặc định
        public MainWindow()
        {
            InitializeComponent();
            LoadClasses();
        }

        // Constructor nhận teacherID
        public MainWindow(int teacherID)
        {
            InitializeComponent();
            this.teacherID = teacherID;  // Gán teacherID vào biến
            TeacherNameTextBlock.Text = "Xin chào, " + GetTeacherNameById(teacherID);  // Hiển thị tên giáo viên
            LoadClasses();
        }

        // Hàm lấy tên giáo viên từ cơ sở dữ liệu
        private string GetTeacherNameById(int teacherID)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MathLearningAppDB"].ConnectionString;
            string query = "SELECT FullName FROM Teachers WHERE TeacherID = @TeacherID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeacherID", teacherID);

                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString() : "Không tìm thấy tên giáo viên";
                }
            }
        }

        // Tải danh sách lớp học của giáo viên
        private void LoadClasses()
        {
            string query = "SELECT ClassID, ClassName, Description FROM Classes WHERE TeacherID = @TeacherID";
            DataTable dataTable = DatabaseHelper.ExecuteQuery(query, new SqlParameter("@TeacherID", teacherID));
            ClassesDataGrid.ItemsSource = dataTable.DefaultView;
        }

        // Xử lý sự kiện nhấn đúp chuột vào lớp học để chỉnh sửa
        private void ClassesDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ClassesDataGrid.SelectedItem != null)
            {
                // Lấy lớp được chọn từ DataGrid dưới dạng DataRowView
                DataRowView selectedRow = ClassesDataGrid.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    // Lấy ClassID từ DataRowView
                    int classID = Convert.ToInt32(selectedRow["ClassID"]);

                    // Truyền ClassID vào cửa sổ ViewStudentsWindow
                    ViewStudentsWindow viewStudentsWindow = new ViewStudentsWindow(classID);
                    viewStudentsWindow.Show();
                }
            }
        }

        // Thêm lớp học mới
        private void AddClass_Click(object sender, RoutedEventArgs e)
        {
            AddClassWindow addClassWindow = new AddClassWindow(teacherID);
            addClassWindow.ShowDialog();
            LoadClasses();
        }

        // Chỉnh sửa lớp học đã chọn
        private void EditClass_Click(object sender, RoutedEventArgs e)
        {
            if (ClassesDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int classID = Convert.ToInt32(selectedRow["ClassID"]);
                string className = selectedRow["ClassName"].ToString();
                string description = selectedRow["Description"].ToString();

                EditClassWindow editClassWindow = new EditClassWindow(classID, className, description);
                if (editClassWindow.ShowDialog() == true)
                {
                    LoadClasses();
                }
            }
        }

        // Xóa lớp học đã chọn
        private void DeleteClass_Click(object sender, RoutedEventArgs e)
        {
            if (ClassesDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int classID = Convert.ToInt32(selectedRow["ClassID"]);

                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa lớp này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    string query = "DELETE FROM Classes WHERE ClassID = @ClassID";
                    DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@ClassID", classID));
                    MessageBox.Show("Xóa lớp học thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadClasses();
                }
            }
        }
    }
}
