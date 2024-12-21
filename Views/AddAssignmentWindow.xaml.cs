using System;
using System.Data.SqlClient;
using System.Windows;

namespace MathLearningApp
{
    public partial class AddAssignmentWindow : Window
    {
        private int _classID;

        // Constructor nhận classID để liên kết bài tập với lớp
        public AddAssignmentWindow(int classID)
        {
            InitializeComponent();
            _classID = classID;
        }

        // Sự kiện khi TextBox Tiêu Đề có focus (ẩn placeholder)
        private void TitleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Nếu TextBox chưa có nội dung, ẩn placeholder
            if (TitleTextBox.Text == "Tiêu Đề")
            {
                TitleTextBox.Text = "";
                TitleTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            }
        }

        // Sự kiện khi TextBox Tiêu Đề mất focus (hiển thị lại placeholder nếu rỗng)
        private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TitleTextBox.Text))
            {
                TitleTextBox.Text = "Tiêu Đề";
                TitleTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        // Khi nhấn nút "Tạo Bài Tập"
        private void AddAssignmentButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;
            DateTime? dueDate = DueDatePicker.SelectedDate;

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(title) || title == "Tiêu Đề" || !dueDate.HasValue)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Câu truy vấn INSERT
            string query = "INSERT INTO Assignments (ClassID, Title, Description, DueDate) VALUES (@ClassID, @Title, @Description, @DueDate)";

            try
            {
                // Gọi hàm ExecuteNonQuery để thực thi câu truy vấn
                DatabaseHelper.ExecuteNonQuery(query,
                    new SqlParameter("@ClassID", _classID),
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Description", description),
                    new SqlParameter("@DueDate", dueDate.Value));

                MessageBox.Show("Tạo bài tập thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Đóng cửa sổ sau khi thêm thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Có lỗi xảy ra khi tạo bài tập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
