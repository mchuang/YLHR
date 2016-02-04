using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YLHR
{
    static class DatabaseControl
    {
        public static String employeeTable = "Employee";
        public static String[] employeeColumns = {"SSN", "EmployeeID", "FirstName", "MiddleName", "LastName",
                                                  "Phone1", "Phone2", "Phone3", "Address", "City", "State", "Zip",
                                                  "Birthday", "DriverLicenseNumber", "Email", "Gender", "Status",
                                                  "DateHired", "InsuranceInfo", "LastModified", "Comment", "CurrentEmployee"};
        public static String salaryTable = "Salary";
        public static String[] salaryColumns = {"SalaryType", "SalaryYearly", "SalaryMonthly", "SalaryHourly",
                                                "SalaryRange", "SalaryStep", "EffectiveDate", "EndDate" };

        public static String jobTable = "Job";
        public static String[] jobColumns = { "SSN", "DepartmentID", "ActionID", "JobTitle", "StartDate", "EndDate",
                                              "SalaryID", "DateReviewed", "Remarks" };
        
        public static String insertSql = "INSERT INTO {0} ({1}) VALUES ({2});";
        public static String selectSql = "SELECT {0} FROM {1};";
        public static String selectWhereSql = "SELECT {0} FROM {1} WHERE {2};";
        public static String updateSql = "UPDATE {0} SET {1} WHERE {2}";
        public static String lastIdentity = "Select Scope_Identity();";
        
        //Database and login credentials
        private static String credentials = "Data Source=VM-SQL2008;Initial Catalog=YLHR;User ID=ylhr;Password=ylhr1";

        //Returns a new connection
        public static SqlConnection Connect() {
            return new SqlConnection(credentials);
        }

        //Sets the parameters of a referenced SqlCommand with the given array of values
        //Assumes the parameters follow format of @value# where # is some integer
        public static void setSqlCommandParameters(ref SqlCommand cmd, Object[] values)
        {
            int i = 0;
            foreach (Object val in values)
            {
                if (val == null) { cmd.Parameters.Add(new SqlParameter("value" + i, DBNull.Value)); }
                else { cmd.Parameters.Add(new SqlParameter("value" + i, val)); }
                i++;
            }
        }

        public static void executeInsertQuery(String table, String[] columns, Object[] values)
        {
            String pair = "@value";
            String valuesSet = "";
            String columnSet = "";
            int i = 0;
            foreach (String col in columns)
            {
                columnSet += (col + ',');
                valuesSet += (pair + i + ',');
                i++;
            }
            columnSet = columnSet.Substring(0, columnSet.Length - 1);
            valuesSet = valuesSet.Substring(0, valuesSet.Length - 1);

            String query = String.Format(insertSql, table, columnSet, valuesSet) + lastIdentity;

            executeNonQueryWithParameters(query, values);
        }

        public static int executeInsertQueryWithId(String table, String[] columns, Object[] values)
        {
            String pair = "@value";
            String valuesSet = "";
            String columnSet = "";
            int i = 0;
            foreach (String col in columns)
            {
                columnSet += (col+',');
                valuesSet += (pair+i+',');
                i++;
            }
            columnSet = columnSet.Substring(0, columnSet.Length - 1);
            valuesSet = valuesSet.Substring(0, valuesSet.Length - 1);

            String query = String.Format(insertSql, table, columnSet, valuesSet) + lastIdentity;

            return executeQueryWithParameters(query, values);
        }

        //Generates the custom update query and executes it
        public static void executeUpdateQuery(String table, String[] columns, Object[] updates, String condition)
        {
            String pair = "{0}=@value";
            String set = "";
            int i = 0;
            foreach (String col in columns)
            {
                set += String.Format(pair+i+',', col);
                i++;
            }
            set = set.Substring(0, set.Length - 1);
            String query = String.Format(updateSql, table, set, condition);

            executeNonQueryWithParameters(query, updates);
        }

        //Executes the given query after setting the given values in the query
        public static int executeQueryWithParameters(String query, Object[] values)
        {
            SqlConnection conn = Connect();
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            setSqlCommandParameters(ref cmd, values);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return id;
        }

        public static void executeNonQueryWithParameters(String query, Object[] values)
        {
            SqlConnection conn = Connect();
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            setSqlCommandParameters(ref cmd, values);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static Object[] getSingleRecord(String field, String table, String condition, Object[] values=null)
        {
            String query = String.Format(DatabaseControl.selectWhereSql, field, table, condition);

            SqlConnection conn = DatabaseControl.Connect();
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            if (values != null) { setSqlCommandParameters(ref cmd, values); }
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            Object[] results = new Object[reader.FieldCount];
            reader.GetValues(results);
            reader.Close();
            conn.Close();
            return results;
        }

        public static ArrayList getMultipleRecord(String field, String table, String condition, Object[] values = null)
        {
            ArrayList records = new ArrayList();
            String query = String.Format(DatabaseControl.selectWhereSql, field, table, condition);

            SqlConnection conn = DatabaseControl.Connect();
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            if (values != null) { setSqlCommandParameters(ref cmd, values); }
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Object[] record = new Object[reader.FieldCount];
                reader.GetValues(record);
                records.Add(record);
            }
            reader.Close();
            conn.Close();
            return records;
        }

        public static void populateComboBox(ref ComboBox list, String table, String text, String value, String condition, bool idOnly = false)
        {
            String itemCombo = value + "," + text;
            String query = String.Format(DatabaseControl.selectWhereSql, itemCombo, table, condition);

            SqlConnection conn = DatabaseControl.Connect();
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Items.Add(new CommonTools.Item((int)reader.GetValue(0), reader.GetValue(1).ToString()));
            }
            reader.Close();
            conn.Close();
        }

        public static void populateDataTable(ref System.Data.DataTable dataTable, String field, String table, String condition, Object[] values=null)
        {
            String query = String.Format(DatabaseControl.selectWhereSql, field, table, condition);

            SqlConnection conn = DatabaseControl.Connect();
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            if (values != null) { setSqlCommandParameters(ref cmd, values); }
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);
            conn.Close();
        }
    }
}
