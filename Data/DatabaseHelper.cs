using MathLearningApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MathLearningApp
{
    public static class DatabaseHelper
    {
        private static string _connectionString = "Data Source=localhost;Initial Catalog=MathLearningDB;Integrated Security=True";

        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                if (parameters != null)
                {
                    dataAdapter.SelectCommand.Parameters.AddRange(parameters);
                }
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Lấy danh sách bài tập cho lớp dựa trên classID
        public static List<Assignment> GetAssignmentsForClass(int classID)
        {
            List<Assignment> assignments = new List<Assignment>();
            string query = "SELECT AssignmentID, ClassID, Title, Description, DueDate FROM Assignments WHERE ClassID = @ClassID";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    // Thêm tham số vào câu lệnh SQL
                    command.Parameters.AddWithValue("@ClassID", classID);

                    // Thực thi câu lệnh và đọc dữ liệu
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            assignments.Add(new Assignment
                            {
                                AssignmentID = (int)reader["AssignmentID"],
                                ClassID = (int)reader["ClassID"],
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                DueDate = (DateTime)reader["DueDate"]
                            });
                        }
                    }
                }
            }
            return assignments;
        }

        public static void DeleteAssignment(int assignmentID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Assignments WHERE AssignmentID = @AssignmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AssignmentID", assignmentID);
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Bài tập đã được xóa!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi xóa bài tập: " + ex.Message);
                }
            }
        }

        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);

                connection.Open();
                return command.ExecuteNonQuery(); // Trả về số dòng bị ảnh hưởng
            }
        }
    }
}
