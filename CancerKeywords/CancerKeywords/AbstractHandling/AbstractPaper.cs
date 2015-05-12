using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerKeywords.AbstractHandling
{
    class AbstractPaper : AbstractInterface
    {
        private int pubmedID;

        public AbstractPaper(int pubmedID)
        {
            this.pubmedID = pubmedID;
        }

        public string AbstractText
        {
            get
            {
                return "temp";
            }
        }

        public int PubmedID
        {
            get
            {
                return pubmedID;
            }
        }
    }
}
