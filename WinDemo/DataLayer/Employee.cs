using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class Employees
    {
        public List<Employee> EmployeList { get; set; }

        public Employee GetEmployeeDetails(int employeeId)
        {
            Employee employee = new Employee();
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"GetEmployeeDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter businessEntityId = new SqlParameter("businessEntityId", SqlDbType.Int)
                    {
                        Value = employeeId
                    };

                    cmd.Parameters.Add(businessEntityId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        employee.Load(reader);
                    }
                }
            }
            return employee;
        }

        public void UpdateDepartmentName(int departmentId, string newName)
        {
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UpdateDepartmentName";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter departmentIdParameter = new SqlParameter("departmentId", SqlDbType.Int)
                    {
                        Value = departmentId
                    };
                    cmd.Parameters.Add(departmentIdParameter);
                    SqlParameter newNameParameter = new SqlParameter("newName", SqlDbType.NVarChar, 100)
                    {
                        Value = newName
                    };
                    cmd.Parameters.Add(newNameParameter);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Employee GetEmployee(int employeeId)
        {
            Employee employee = new Employee();
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select * from HumanResources.Employee E
                                        JOIN Person.Person P ON E.BusinessEntityID = P.BusinessEntityID AND P.PersonType = 'EM'
                                        JOIN HumanResources.EmployeeDepartmentHistory EH ON E.BusinessEntityID = EH.BusinessEntityID
                                        JOIN HumanResources.Department D ON D.DepartmentID = EH.DepartmentID
                                        WHERE
	                                        E.BusinessEntityID = {0}";

                    cmd.CommandText = string.Format(cmd.CommandText, employeeId.ToString());

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        employee.Load(reader);
                    }
                }
            }
            return employee;
        }
    }
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public int DepartamentId { get; set; }
        public string DepartmentName { get; set; }

        public void Load(SqlDataReader reader)
        {
            EmployeeId = int.Parse(reader["BusinessEntityId"].ToString());
            FirstName = reader["FirstName"].ToString();
            LastName = reader["LastName"].ToString();
            DepartamentId = int.Parse(reader["DepartmentId"].ToString());
            DepartmentName = reader["Name"].ToString();
        }
    }
}