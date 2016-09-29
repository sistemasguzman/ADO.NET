using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;

namespace WinDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            try
            {
                DB.ApplicationName = "WinDemo";
                DB.ConnectionTimeout = 5;
                SqlConnection conn = DB.GetSqlConnection();

                DataTable tableLog = ApplicationLog.GetLog(DB.ApplicationName);
                dgvApplicationLog.DataSource = tableLog;
            }
            catch (SqlException sqlExc)
            {
                MessageBox.Show(this, sqlExc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void OnClickGo(object sender, EventArgs e)
        {
            Employees employees = new Employees();
            DB.EnableStatistics = true;
            Employee employee = employees.GetEmployeeDetails(int.Parse(txtEmployeeId.Text));
            RefreshStatistics(DB.LastStatistics);
            DB.EnableStatistics = false;

            txtFirstName.Text = employee.FirstName;
            txtLastName.Text = employee.LastName;
            txtDepartment.Text = employee.DepartmentName;
            lblDepartmentId.Text = employee.DepartamentId.ToString();

            ApplicationLog.Add4("Searched for user id: " + txtEmployeeId.Text);
        }

        private void OnClickDelete(object sender, EventArgs e)
        {
            ApplicationLog.DeleteCommentsForApp("WinDemo");
        }

        private void OnClickUpdateDepartment(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {

                Employees employees = new Employees();
                employees.UpdateDepartmentName(int.Parse(lblDepartmentId.Text), txtDepartment.Text);
            }
            catch (SqlException sqlExc)
            {
                MessageBox.Show(this, sqlExc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnClickUpdate(object sender, EventArgs e)
        {
            try
            {
                DataTable table = (DataTable)dgvApplicationLog.DataSource;
                DB.EnableStatistics = true;
                DataTable tableRes = ApplicationLog.UpdateLogChanges(table);
                RefreshStatistics(DB.LastStatistics);
                DB.EnableStatistics = false;
                dgvApplicationLog.DataSource = tableRes;
            }
            catch (SqlException sqlExc)
            {
                MessageBox.Show(this, sqlExc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshStatistics(ConnectionStatistics connectionStatistics)
        {
            lvStats.Items.Clear();
            foreach (string key in connectionStatistics.OriginalStats.Keys)
            {
                ListViewItem lvi = new ListViewItem(key);
                lvi.SubItems.Add(connectionStatistics.OriginalStats[key].ToString());
                lvStats.Items.Add(lvi);
            }
        }
    }
}
