using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace CancerKeywords
{
    public partial class AbstractViewer : Form
    {
        private bool isAlive = true;
        private List<AbstractHandling.AbstractProxy> abstracts;
        private float timeSinceLastUpdate = 500;
        private bool fetcherDone = false;

        public AbstractViewer()
        {
            InitializeComponent();
            isAlive = true;
        }

        /// <summary>
        /// Enables abstracts to be sendt to the viewer.
        /// </summary>
        /// <param name="selectedBubble">The cancer bubble, which abstracts should be displayed</param>
        internal void sendAbstracts(CancerBubble selectedBubble)
        {
            if (selectedBubble != null)
            {
                abstracts = new List<AbstractHandling.AbstractProxy>();

                foreach(int pubmedID in selectedBubble.abstractIDs)
                {
                    abstracts.Add(new AbstractHandling.AbstractProxy(pubmedID));
                }

                listBox1.DataSource = abstracts.Select(cb => cb.PubmedID).ToList();
            }
        }

        /// <summary>
        /// Alows addition of an abstract
        /// </summary>
        /// <param name="pubmedID">Pubmed ID of the abstract</param>
        public void addAbstract(int pubmedID)
        {
            abstracts.Add(new AbstractHandling.AbstractProxy(pubmedID));

            //Update the datasource
            int currentlySelected = listBox1.SelectedIndex;
            listBox1.DataSource = null;
            listBox1.DataSource = abstracts.Select(cb => cb.PubmedID).ToList();
            listBox1.SelectedIndex = currentlySelected;
        }

        /// <summary>
        /// Allows automatic updating of the abstracts
        /// </summary>
        /// <param name="gameTime">Gametime object from XNA</param>
        public void update(GameTime gameTime)
        {
            //Only update every half second
            if(timeSinceLastUpdate >= 500)
            {
                //If it is a real abstract
                if (abstracts != null && !abstracts[listBox1.SelectedIndex].IsProxy)
                {
                    //And it is done
                    if(abstracts[listBox1.SelectedIndex].RealAbstract.Fetcher.IsDone && !fetcherDone)
                    {
                        //Update the box once
                        richTextBox1.Text = abstracts[listBox1.SelectedIndex].AbstractText;
                        fetcherDone = true;
                    }
                }
                timeSinceLastUpdate = 0;
            }
            else
            {
                timeSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        /// <summary>
        /// Motering closing of the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AbstractViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            isAlive = false;
        }

        /// <summary>
        /// Change which abstract should be displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            { 
                richTextBox1.Text = abstracts[listBox1.SelectedIndex].AbstractText;
                
                //If it is a proxy, the fetcher is not done
                if (!abstracts[listBox1.SelectedIndex].IsProxy)
                {
                    fetcherDone = false;
                }
            }
        }

        /// <summary>
        /// Getter for alive status
        /// </summary>
        public bool IsAlive
        {
            get { return isAlive; }
        }

    }
}
