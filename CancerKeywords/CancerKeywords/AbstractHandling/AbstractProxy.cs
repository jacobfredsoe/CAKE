using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerKeywords.AbstractHandling
{
    class AbstractProxy : AbstractInterface
    {
        private AbstractPaper realAbstract = null;
        private int pubmedID;

        /// <summary>
        /// Proxy pattern for abstract. Proxy
        /// </summary>
        /// <param name="pubmedID">Pudmed ID number</param>
        public AbstractProxy(int pubmedID)
        {
            this.pubmedID = pubmedID;
        }

        /// <summary>
        /// Getter for abstract text.
        /// </summary>
        public string AbstractText
        {
            get
            {
                //Create the real abstract
                if (realAbstract == null)
                {
                    realAbstract = new AbstractPaper(pubmedID);
                }

                return realAbstract.AbstractText;
            }
        }

        /// <summary>
        /// Getter for Pudmed ID
        /// </summary>
        public int PubmedID
        {
            get
            {
                return pubmedID;
            }
        }

        /// <summary>
        /// Getter for if the proxy contains a real subject
        /// </summary>
        public bool IsProxy
        {
            get { return realAbstract == null; }
        }


        /// <summary>
        /// Getter for the real subject
        /// </summary>
        public AbstractPaper RealAbstract
        {
            get { return realAbstract; }
        }
    }
}
