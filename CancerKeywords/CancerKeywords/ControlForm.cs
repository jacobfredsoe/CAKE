using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CancerKeywords
{
    public partial class ControlForm : Form
    {
        private bool newInformation = false;
        public bool isAlive = true;

        public ControlForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newInformation = true;
            toolStripStatusLabel1.Text = "Search for \"" + textBox1.Text + "\" started.";
        }

        public bool newInfo()
        {
            if (newInformation)
            {
                newInformation = false;
                return true;
            }
            else return false;
        }

        public string getSearchString()
        {
            return textBox1.Text.Replace(" ", "+");
        }

        public int getSpeed()
        {
            return (int)numericUpDown1.Value;
        }

        internal void postResult(SortedDictionary<string, CancerBubble> data)
        {
            List<CancerBubble> cancerBubblesList = data.Values.ToList();

            cancerBubblesList.Sort();

            dataGridView1.DataSource = cancerBubblesList.Select(o => new { Name = o.Text, Cases = o.Cases }).ToList();
        }

        private void ControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isAlive = false;
        }
    }
}
