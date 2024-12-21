using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MathLearningApp
{
    public partial class AddStudentWindow : Window
    {
        private int _classID;

        public AddStudentWindow(int classID)
        {
            InitializeComponent();
            _classID = classID;
        }

        

        

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text;
            DateTime? dateOfBirth = DateOfBirthPicker.SelectedDate;
            string gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            

            // Kiểm tra tên học sinh không trống
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Tên học sinh không thể trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra ngày sinh không trống
            if (!dateOfBirth.HasValue)
            {
                MessageBox.Show("Ngày sinh không thể trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra giới tính không bị bỏ trống
            if (string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Câu lệnh SQL để thêm học sinh vào bảng, bao gồm cả ClassID và Age
            string query = "INSERT INTO Students (FullName, Gender, DateOfBirth, ClassID) VALUES (@FullName, @Gender, @DateOfBirth, @ClassID)";

            try
            {
                // Thực hiện câu lệnh SQL với các tham số đã kiểm tra
                DatabaseHelper.ExecuteNonQuery(query,
                    new SqlParameter("@FullName", fullName),
                    new SqlParameter("@Gender", gender),
                    new SqlParameter("@DateOfBirth", dateOfBirth),
                    
                    new SqlParameter("@ClassID", _classID)); // Truyền ClassID vào câu lệnh SQL

                MessageBox.Show("Thêm học sinh thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Đóng cửa sổ sau khi thêm học sinh thành công
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
