using System;
using System.Windows.Forms;

namespace SharedService
{
    public partial class ArchiveForm : Form
    {
        public ArchiveForm()
        {
            InitializeComponent();
        }

        public ArchiveForm(string infomation)
        {
            InitializeComponent();
            Infomation.Text = infomation;
        }

        private void Archive_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void Replace_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
