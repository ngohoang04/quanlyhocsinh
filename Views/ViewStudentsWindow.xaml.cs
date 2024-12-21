using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace MathLearningApp
{
    public partial class ViewStudentsWindow : Window
    {
        private int _classID;

        public ViewStudentsWindow(int classID)
        {
            InitializeComponent();
            _classID = classID;
            LoadStudents();
        }

        // Hàm tải danh sách học sinh vào DataGrid
        private void LoadStudents()
        {
            try
            {
                string query = "SELECT StudentID, FullName, Gender FROM Students WHERE ClassID = @ClassID";
                var parameters = new SqlParameter[] { new SqlParameter("@ClassID", _classID) };

                // Giả sử ExecuteQuery trả về DataTable
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                // Kiểm tra nếu DataTable có dữ liệu
                if (dataTable.Rows.Count > 0)
                {
                    StudentsDataGrid.ItemsSource = dataTable.DefaultView;
                }
                else
                {
                    MessageBox.Show("Không có học sinh trong lớp này.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu học sinh: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Sự kiện khi nhấn nút "Thêm học sinh"
        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Mở cửa sổ thêm học sinh
                AddStudentWindow addStudentWindow = new AddStudentWindow(_classID);

                // Sử dụng sự kiện Window.Closed để gọi lại LoadStudents sau khi cửa sổ đóng
                addStudentWindow.Closed += (s, args) =>
                {
                    LoadStudents(); // Tải lại danh sách học sinh sau khi thêm
                };

                addStudentWindow.ShowDialog(); // Mở cửa sổ thêm học sinh
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm học sinh: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện khi nhấn nút "Xem bài tập"
        private void ViewAssignmentsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Mở cửa sổ bài tập
                AssignmentWindow assignmentWindow = new AssignmentWindow(_classID);
                assignmentWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở cửa sổ bài tập: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
