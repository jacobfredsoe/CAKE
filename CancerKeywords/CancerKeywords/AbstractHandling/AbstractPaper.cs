using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerKeywords.AbstractHandling
{
    class AbstractPaper : AbstractInterface
    {
        private int pubmedID;
        private AbstractFetcher fetcher;

        /// <summary>
        /// Proxy pattern for abstract. Real subject
        /// </summary>
        /// <param name="pubmedID"></param>
        public AbstractPaper(int pubmedID)
        {
            this.pubmedID = pubmedID;
            fetcher = new AbstractFetcher();
            fetcher.fetchAbstract(new int[] { pubmedID });
        }

        /// <summary>
        /// Getter for abstract text
        /// </summary>
        public string AbstractText
        {
            get
            {
                return fetcher.AbstractText;
            }
        }

        /// <summary>
        /// Getter for pudmed ID
        /// </summary>
        public int PubmedID
        {
            get
            {
                return pubmedID;
            }
        }

        /// <summary>
        /// Getter for the abstractFetcher
        /// </summary>
        public AbstractFetcher Fetcher
        {
            get { return fetcher; }
        }
    }
}
