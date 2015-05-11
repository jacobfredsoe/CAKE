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
    public partial class AbstractViewer : Form
    {
        public bool isAlive;

        public AbstractViewer()
        {
            InitializeComponent();
            isAlive = true;
        }

        internal void sendAbstracts(SortedDictionary<string, CancerBubble> data)
        {
            if(data != null)
            {
                List<CancerBubble> selectedBubbles = data.Values.ToList().Where(cbubble => cbubble.Selected).ToList();

                if (selectedBubbles.Count > 0) listBox1.DataSource = selectedBubbles[0].abstractIDs;
            }
        }

        private void AbstractViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            isAlive = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
