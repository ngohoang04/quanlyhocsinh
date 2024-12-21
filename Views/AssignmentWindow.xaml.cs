using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using MathLearningApp.Models; // Đảm bảo rằng bạn đã thêm namespace này

namespace MathLearningApp
{
    public partial class AssignmentWindow : Window
    {
        private int _classID;

        public AssignmentWindow(int classID)
        {
            InitializeComponent();
            _classID = classID;
            LoadAssignments();
        }

        // Hàm tải danh sách bài tập của lớp từ cơ sở dữ liệu
        private void LoadAssignments()
        {
            try
            {
                string query = "SELECT AssignmentID, Title, Description, DueDate FROM Assignments WHERE ClassID = @ClassID";
                var parameters = new SqlParameter[] { new SqlParameter("@ClassID", _classID) };
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    AssignmentsDataGrid.ItemsSource = dataTable.DefaultView;
                }
                else
                {
                    MessageBox.Show("Không có bài tập trong lớp này.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu bài tập: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddAssignmentButton_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ thêm bài tập
            var addWindow = new AddAssignmentWindow(_classID);
            addWindow.ShowDialog();

            // Tải lại danh sách bài tập sau khi thêm
            LoadAssignments();
        }
        // Sự kiện sửa bài tập
        private void EditAssignmentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAssignment = AssignmentsDataGrid.SelectedItem as DataRowView;
            if (selectedAssignment == null)
            {
                MessageBox.Show("Vui lòng chọn bài tập để sửa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Lấy đối tượng Assignment từ DataRowView
            var assignment = new Assignment
            {
                AssignmentID = Convert.ToInt32(selectedAssignment["AssignmentID"]),
                Title = selectedAssignment["Title"].ToString(),
                Description = selectedAssignment["Description"].ToString(),
                DueDate = Convert.ToDateTime(selectedAssignment["DueDate"])
            };

            // Tạo cửa sổ sửa bài tập và truyền vào đối tượng Assignment
            // Sửa lại cách gọi cửa sổ EditAssignmentWindow
            var editWindow = new EditAssignmentWindow(_classID, assignment.AssignmentID);
            editWindow.ShowDialog();


            // Tải lại danh sách bài tập sau khi sửa
            LoadAssignments();
        }

        // Sự kiện xóa bài tập
        private void DeleteAssignmentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAssignment = AssignmentsDataGrid.SelectedItem as DataRowView;
            if (selectedAssignment == null)
            {
                MessageBox.Show("Vui lòng chọn bài tập để xóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa bài tập này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                // Lấy ID bài tập từ DataRowView
                int assignmentID = Convert.ToInt32(selectedAssignment["AssignmentID"]);

                // Gọi phương thức DeleteAssignment và truyền vào assignmentID
                DatabaseHelper.DeleteAssignment(assignmentID);

                // Tải lại danh sách bài tập
                LoadAssignments();
                MessageBox.Show("Bài tập đã được xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
