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

        public AbstractPaper(int pubmedID)
        {
            this.pubmedID = pubmedID;
            fetcher = new AbstractFetcher();
            fetcher.fetchAbstract(new int[] { pubmedID });
        }

        public string AbstractText
        {
            get
            {
                return fetcher.AbstractText;
            }
        }

        public int PubmedID
        {
            get
            {
                return pubmedID;
            }
        }

        public AbstractFetcher Fetcher
        {
            get { return fetcher; }
        }
    }
}
