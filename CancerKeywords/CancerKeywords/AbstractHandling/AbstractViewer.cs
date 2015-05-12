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
        private bool isAlive = true;
        private BindingList<AbstractHandling.AbstractProxy> abstracts;

        public AbstractViewer()
        {
            InitializeComponent();
            isAlive = true;
        }

        internal void sendAbstracts(CancerBubble selectedBubble)
        {
            if (selectedBubble != null)
            {
                abstracts = new BindingList<AbstractHandling.AbstractProxy>();

                foreach(int pubmedID in selectedBubble.abstractIDs)
                {
                    abstracts.Add(new AbstractHandling.AbstractProxy(pubmedID));
                }

                listBox1.DataSource = abstracts;
                listBox1.DisplayMember = "pubmedID";
            }
        }

        public void updateAbstracts(int pubmedID)
        {
            abstracts.Add(new AbstractHandling.AbstractProxy(pubmedID));
            listBox1.DataSource = null;
            listBox1.DisplayMember = "pubmedID";
            listBox1.DataSource = abstracts;
        }

        private void AbstractViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            isAlive = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1) richTextBox1.Text = abstracts[listBox1.SelectedIndex].AbstractText;
        }

        public bool IsAlive
        {
            get { return isAlive; }
        }

    }
}
