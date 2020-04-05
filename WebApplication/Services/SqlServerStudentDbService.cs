using System;
using System.Data;
using System.Data.SqlClient;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class SqlServerStudentDbService : IStudentServiceDb
    {
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
             using (SqlConnection connection = new SqlConnection("ConnectionString"))
             {
                 connection.Open();
                 SqlCommand command = connection.CreateCommand();
                 SqlTransaction transaction = connection.BeginTransaction("Transaction");
                 command.Connection = connection;
                 command.Transaction = transaction;

                 try
                 {
                     int idStudies;
                     command.CommandText = "SELECT * FROM Studies WHERE Name=@StudiesName";
                     command.Parameters.AddWithValue("StudiesName", request.Studies);
                     using (var response = command.ExecuteReader())
                     {
                         if (!response.Read())
                         {
                             return null;
                         }

                         idStudies = int.Parse(response["IdStudy"].ToString());
                     }
                     
                     int idEnrollment;
                     command.CommandText = "SELECT * FROM Enrollment WHERE Semester=1 AND IdStudy=@IdStudies";
                     command.Parameters.AddWithValue("IdStudies", idStudies);
                     using (var response = command.ExecuteReader())
                     {
                         if (!response.Read())
                         {
                             command.CommandText = "INSERT INTO Enrollment(Semester, IdStudy, StartDate) VALUES (1, @IdStudies, @StartDate)";
                             command.Parameters.AddWithValue("IdStudies", idStudies);
                             command.Parameters.AddWithValue("StartDate", DateTime.Today);
                             command.ExecuteNonQuery();
                             idEnrollment = 1;
                         }
                         else
                         {
                             idEnrollment = (int) response["IdEnrollment"];
                         }
                     }

                     command.CommandText = "SELECT * FROM Student WHERE IndexNumber=@IndexNumber";
                     command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                     using (var response = command.ExecuteReader())
                     {
                         if (response.Read())
                         {
                             return null;
                         }
                     }

                     command.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES (@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                     command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                     command.Parameters.AddWithValue("FirstName", request.FirstName);
                     command.Parameters.AddWithValue("LastName", request.LastName);
                     command.Parameters.AddWithValue("BirthDate", request.BirthDate);
                     command.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                     command.ExecuteNonQuery();

                     transaction.Commit();

                     return new EnrollStudentResponse()
                     {
                         Semester = 1,
                         LastName = request.LastName
                     };
                 }
                 catch (Exception)
                 {
                     transaction.Rollback();
                     return null;
                 }
             }
        }

        public PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request)
        {
            using (SqlConnection connection = new SqlConnection("ConnectionString"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PromoteStudents", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Semester", request.Semester));
                command.Parameters.Add(new SqlParameter("@Studies", request.Studies));
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var response = new PromoteStudentsResponse()
                    {
                        IdEnrollment = int.Parse(reader["IdEnrollment"].ToString()),
                        Semester = int.Parse(reader["Semester"].ToString()),
                        Study = reader["Name"].ToString(),
                        StartDate = DateTime.Parse(reader["StartDate"].ToString())
                    };
                    reader.Close();
                    return response;
                }
                reader.Close();
            }
            return null;
        }
    }
}