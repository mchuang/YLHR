using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YLHR
{
    public partial class CreateNewEmployee : Form
    {
        public CreateNewEmployee()
        {
            InitializeComponent();
        }

        private void createNewEmployee_Load(object sender, EventArgs e)
        {
            departmentInfo.Items.Add(new CommonTools.Item(1, "Admin"));
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            if (!validateInfo()) { return; }

            char currEmployee = 'Y';
            DateTime lastModified = DateTime.Today;
            Object[] employeeValues = { ssnInfo.Text, employeeIdInfo.Text, fNameInfo.Text, mNameInfo.Text, lNameInfo.Text, 
                                        primPhoneInfo.Text, homePhoneInfo.Text, mobilePhoneInfo.Text,
                                        addressInfo.Text, cityInfo.Text, stateInfo.Text, zipInfo.Text,
                                        birthdayInfo.Value, driverInfo.Text, emailInfo.Text, genderInfo.Text, statusInfo.Text,
                                        dateHiredInfo.Value, insuranceInfo.Text, lastModified, commentsInfo.Text, currEmployee};
            DatabaseControl.executeInsertQuery(DatabaseControl.employeeTable, DatabaseControl.employeeColumns, employeeValues);

            String salaryType = null;
            if (yearly.Checked) { salaryType = "Yearly"; }
            else if (monthly.Checked) { salaryType = "Monthly"; }
            else if (hourly.Checked) { salaryType = "Hourly"; }
            Object[] salaryValues = { salaryType, Convert.ToDecimal(yearlyInfo.Text), Convert.ToDecimal(monthlyInfo.Text), Convert.ToDecimal(hourlyInfo.Text),
                                      salRangeInfo.Text, salStepInfo.Text, dateHiredInfo.Value.Date, null };
            int salaryId = DatabaseControl.executeInsertQueryWithId(DatabaseControl.salaryTable, DatabaseControl.salaryColumns, salaryValues);

            int newHire = 0;
            int departmentId = ((CommonTools.Item)departmentInfo.SelectedItem).Value;
            Object[] jobValues = { ssnInfo.Text, departmentId, newHire, jobTitleInfo.Text, dateHiredInfo.Value, null, salaryId, null, null };
            DatabaseControl.executeInsertQuery(DatabaseControl.jobTable, DatabaseControl.jobColumns, jobValues);

            MessageBox.Show("New Employee Created.");
            this.Close();
        }

        private bool validateInfo()
        {
            return true;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void genderInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
