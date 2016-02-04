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
    public partial class HumanResourceManagement : Form
    {
        public HumanResourceManagement()
        {
            InitializeComponent();
        }

        private void newEmployee_Click(object sender, EventArgs e)
        {
            CreateNewEmployee newEmployee = new CreateNewEmployee();
            this.Hide();
            newEmployee.ShowDialog();
            this.Show();
        }

        private void newAction_Click(object sender, EventArgs e)
        {
            CreateNewAction newAction = new CreateNewAction();
            this.Hide();
            newAction.ShowDialog();
            this.Show();
        }
    }
}
