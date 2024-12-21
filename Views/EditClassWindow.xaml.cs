using System;
using System.Windows;
using System.Data.SqlClient;

namespace MathLearningApp
{
    public partial class EditClassWindow : Window
    {
        private int classID;
        private string originalClassName;
        private string originalDescription;

        public EditClassWindow(int classID, string className, string description)
        {
            InitializeComponent();
            this.classID = classID;
            this.originalClassName = className;
            this.originalDescription = description;

            ClassNameTextBox.Text = className;
            DescriptionTextBox.Text = description;
        }

        // Lưu thay đổi thông tin lớp
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string newClassName = ClassNameTextBox.Text;
            string newDescription = DescriptionTextBox.Text;

            if (newClassName == originalClassName && newDescription == originalDescription)
            {
                MessageBox.Show("Không có thay đổi nào.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = false;
                Close();
                return;
            }

            try
            {
                string query = "UPDATE Classes SET ClassName = @ClassName, Description = @Description WHERE ClassID = @ClassID";
                SqlParameter[] parameters = {
                    new SqlParameter("@ClassID", classID),
                    new SqlParameter("@ClassName", newClassName),
                    new SqlParameter("@Description", newDescription)
                };

                // Gọi phương thức thực thi truy vấn SQL
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Sửa thông tin lớp thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Cập nhật kết quả trả về
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu thay đổi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
