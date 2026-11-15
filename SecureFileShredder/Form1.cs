using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecureFileShredder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select a test file";
                dialog.Filter = "All Files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = dialog.FileName;
                    lblStatus.Text = "File selected successfully.";
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                MessageBox.Show("Please select a file first.");
                return;
            }

            string testFolder = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.Desktop),
                "ShredderTest"
            );

            string fullPath = Path.GetFullPath(filePath);

            if (!Directory.Exists(testFolder) ||
                !fullPath.StartsWith(
                    testFolder + Path.DirectorySeparatorChar,
                    StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "For this testing stage, select a file inside ShredderTest only."
                );
                return;
            }

            DialogResult answer = MessageBox.Show(
                "This will permanently delete your selected test file. Continue?",
                "Confirm Secure Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (answer != DialogResult.Yes)
                return;

            try
            {
                bool success = SecureDelete.DeleteFile(fullPath);

                if (success)
                {
                    lblStatus.Text = "Deletion completed.";
                    MessageBox.Show("SDelete completed. File path no longer exists.");
                    txtFilePath.Clear();
                }
                else
                {
                    lblStatus.Text = "Deletion could not be verified.";
                    MessageBox.Show("The file still exists. Check the operation.");
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Operation failed.";
                MessageBox.Show(ex.Message);
            }
        }
    }
}
