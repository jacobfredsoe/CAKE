using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerKeywords.AbstractHandling
{
    class AbstractProxy : AbstractInterface
    {
        private AbstractPaper realAbstract;
        private int pubmedID;

        public AbstractProxy(int pubmedID)
        {
            this.pubmedID = pubmedID;
        }

        public string AbstractText
        {
            get
            {
                if (realAbstract == null)
                {
                    realAbstract = new AbstractPaper(pubmedID);
                }

                return realAbstract.AbstractText;
            }
        }

        public int PubmedID
        {
            get
            {
                if (realAbstract == null)
                {
                    realAbstract = new AbstractPaper(pubmedID);
                }

                return realAbstract.PubmedID;
            }
        }
    }
}
