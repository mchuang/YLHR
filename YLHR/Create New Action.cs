using System;
using System.Collections;
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
    public partial class CreateNewAction : Form
    {
        public CreateNewAction()
        {
            InitializeComponent();
        }

        private void Create_New_Action_Load(object sender, EventArgs e)
        {
            int ssn = 123456789;
            String fields = "*";//"Salary.EffectiveDate, JobTitle, Description, Department, SalaryYearly, SalaryMonthly, SalaryHourly," +
                            //"SalaryRange, SalaryStep, DateReviewed, Remarks";
            String table = DatabaseControl.jobTable + " INNER JOIN " + DatabaseControl.salaryTable + " ON Job.SalaryID=Salary.SalaryID";
            String condition = "SSN=@value0 ORDER BY Salary.EffectiveDate DESC";
            DataTable history = new DataTable(); 
            DatabaseControl.populateDataTable(ref history, fields, table, condition, new Object[] { ssn });

            historyTable.DataSource = history;


            DataRow current = history.Rows[0];
            effDateInfo.Value = (DateTime)current["EffectiveDate"];
            currJobInfo.Text = current["JobTitle"].ToString();
            //currDeptInfo.SelectedIndex = currDeptInfo.FindStringExact(CommonTools.findItem(currDeptInfo.Items, Convert.ToInt32(current[2])));
            currYearlyInfo.Text = current["SalaryYearly"].ToString();
            currMonthlyInfo.Text = current["SalaryMonthly"].ToString();
            currHourlyInfo.Text = current["SalaryHourly"].ToString();
            currRangeInfo.Text = current["SalaryRange"].ToString();
            currStepInfo.Text = current["SalaryStep"].ToString();  
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
