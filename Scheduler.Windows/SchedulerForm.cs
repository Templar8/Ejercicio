using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scheduler.Windows
{
    public partial class SchedulerForm : Form
    {
        public SchedulerForm()
        {
            InitializeComponent();
        }

        private void SchedulerForm_Load(object sender, EventArgs e)
        {
            this.CbxOccurs.SelectedItem = Frecuency.Daily.ToString();
            this.CbxType.SelectedItem = RecurringType.Once.ToString();
        }

        private void CbxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CbxType.SelectedItem.ToString() == RecurringType.Once.ToString())
            {
                this.CbxOccurs.Enabled = this.NUDRecurrency.Enabled = true;
                this.DtpConfigurationDate.Text = string.Empty;
                this.DtpConfigurationTime.Text = string.Empty;
            }
        }

        private void BttCalculateNextDate_Click(object sender, EventArgs e)
        {
            CalculatedDate Calculator = new CalculatedDate(this.DtpCurrentDate.Value, (this.DtpConfigurationDate.Value.Date.Add(this.DtpConfigurationTime.Value.TimeOfDay)),
                (RecurringType)Enum.Parse(typeof(RecurringType), this.CbxType.SelectedItem.ToString()),
                (Frecuency)Enum.Parse(typeof(Frecuency), this.CbxOccurs.SelectedItem.ToString()), Convert.ToInt32(this.NUDRecurrency.Value), this.DtpStartDate.Value, this.DtpEndDate.Value);
            this.TxtNextExecution.Text = Calculator.GetNextExecutionDate().ToString();
            this.TxtDescription.Text = Calculator.GetDescription().ToString();
        }
    }
}
