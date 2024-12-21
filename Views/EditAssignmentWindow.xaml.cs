using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace MathLearningApp
{
    public partial class EditAssignmentWindow : Window
    {
        private int _classID;
        private int _assignmentID;

        // Constructor để nhận classID và assignmentID
        public EditAssignmentWindow(int classID, int assignmentID)
        {
            InitializeComponent();
            _classID = classID;
            _assignmentID = assignmentID;
            LoadAssignment();
        }

        // Hàm tải dữ liệu bài tập vào form
        private void LoadAssignment()
        {
            try
            {
                string query = "SELECT Title, Description, DueDate FROM Assignments WHERE AssignmentID = @AssignmentID";
                var parameters = new SqlParameter[] { new SqlParameter("@AssignmentID", _assignmentID) };
                var dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    TitleTextBox.Text = row["Title"].ToString();
                    DescriptionTextBox.Text = row["Description"].ToString();
                    DueDatePicker.SelectedDate = Convert.ToDateTime(row["DueDate"]);
                }
                else
                {
                    MessageBox.Show("Bài tập không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện khi nhấn nút "Lưu Thay Đổi"
        private void EditAssignmentButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;
            DateTime? dueDate = DueDatePicker.SelectedDate;

            if (string.IsNullOrEmpty(title) || !dueDate.HasValue)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Kiểm tra nếu có sự thay đổi so với dữ liệu cũ
                string queryCheck = "SELECT Title, Description, DueDate FROM Assignments WHERE AssignmentID = @AssignmentID";
                var parametersCheck = new SqlParameter[] { new SqlParameter("@AssignmentID", _assignmentID) };
                var dataTableCheck = DatabaseHelper.ExecuteQuery(queryCheck, parametersCheck);

                if (dataTableCheck.Rows.Count > 0)
                {
                    DataRow rowCheck = dataTableCheck.Rows[0];

                    // Nếu không có thay đổi, không cần thực hiện cập nhật
                    if (title == rowCheck["Title"].ToString() &&
                        description == rowCheck["Description"].ToString() &&
                        dueDate.Value == Convert.ToDateTime(rowCheck["DueDate"]))
                    {
                        MessageBox.Show("Không có thay đổi nào được thực hiện!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                // Cập nhật thông tin bài tập vào cơ sở dữ liệu
                string queryUpdate = "UPDATE Assignments SET Title = @Title, Description = @Description, DueDate = @DueDate WHERE AssignmentID = @AssignmentID";
                var parametersUpdate = new SqlParameter[] {
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Description", description),
                    new SqlParameter("@DueDate", dueDate.Value),
                    new SqlParameter("@AssignmentID", _assignmentID)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(queryUpdate, parametersUpdate);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Sửa bài tập thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không có thay đổi nào được lưu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
