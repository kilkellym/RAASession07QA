using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RAASession07QA
{
    public partial class Form1 : Form
    {
        public Form1(List<string> names, string label)
        {
            InitializeComponent();

            foreach(string name in names)
            {
                ViewTemplateList.Items.Add(name);
            }

            lblHeading.Text = label;
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ViewTemplateList.Items.Count; i++)
            {
                ViewTemplateList.SetItemChecked(i, false);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for(int i=0; i < ViewTemplateList.Items.Count; i++)
            {
                ViewTemplateList.SetItemChecked(i, true);
            }
        }

        public List<string> GetSelectedItems()
        {
            List<string> resultList = new List<string>();

            foreach(string item in ViewTemplateList.CheckedItems)
            {
                resultList.Add(item);
            }

            return resultList;
        }
    }
}
