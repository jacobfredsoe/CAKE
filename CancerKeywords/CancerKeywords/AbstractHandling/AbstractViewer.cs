﻿using System;
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
        private List<AbstractHandling.AbstractProxy> abstracts = new List<AbstractHandling.AbstractProxy>();

        public AbstractViewer()
        {
            InitializeComponent();
            isAlive = true;
        }

        internal void sendAbstracts(SortedDictionary<string, CancerBubble> data)
        {
            if(data != null)
            {
                List<CancerBubble> selectedBubble = data.Values.ToList().Where(cbubble => cbubble.Selected).ToList();

                if (selectedBubble.Count > 0)
                {
                    abstracts = new List<AbstractHandling.AbstractProxy>();

                    foreach(int pubmedID in selectedBubble[0].abstractIDs)
                    {
                        abstracts.Add(new AbstractHandling.AbstractProxy(pubmedID));
                    }

                    listBox1.DataSource = selectedBubble[0].abstractIDs;
                }
            }
        }

        private void AbstractViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            isAlive = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = abstracts[listBox1.SelectedIndex].AbstractText;
        }

        public bool IsAlive
        {
            get { return isAlive; }
        }

    }
}
