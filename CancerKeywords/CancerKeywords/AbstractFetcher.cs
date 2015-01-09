using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;

namespace CancerKeywords
{
    class AbstractFetcher
    {
        private bool isWorking;
        private WebClient wc;
        public int LastID { get; set; }
        private string abstractText;

        public AbstractFetcher()
        {
            isWorking = false;
            wc = new WebClient();
            LastID = 0;
            abstractText = "";
        }

        /// <summary>
        /// Is the fetcher currently in the progress of fetching an abstract
        /// </summary>
        /// <returns>True while trying to fetch abstract</returns>
        public bool IsWorking()
        {
            return isWorking;
        }

        /// <summary>
        /// Getter for abstractText
        /// </summary>
        /// <returns>The value currently stored as the abstract</returns>
        public string getAbstractText()
        {
            return abstractText;
        }

        /// <summary>
        /// Will attempt to fetch the abstract from an ID. When called, the fetcher will show as working
        /// until the abstract has been parsed.
        /// </summary>
        /// <param name="ID">The ID number of the abstract</param>
        public void fetchAbstract(int[] IDs)
        {
            isWorking = true;
            string idString = "";
            
            for(int i = 0; i < IDs.Length; i++)
            {
                idString += IDs[i] + ",";
            }

            abstractText = "fetching abstract of ID=" + idString;
            LastID = IDs[IDs.Length-1];
            string URL = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&id=" + idString + "&retmode=text&rettype=abstract";

            //In order to allow other resources to function while this one is trying to fetch the abstract, a new thread is created for each abstract.
            Thread myNewThread = new Thread(() => getData(URL));
            myNewThread.Start();
        }

        /// <summary>
        /// Will get the text from a given URL and replace any special characters with *.
        /// When done, the fecther will show isWorking == false.
        /// </summary>
        /// <param name="URL">The URL</param>
        private void getData(string URL)
        {
            string result = "";
            try
            {
                byte[] raw = wc.DownloadData(URL);
                
                //Replace special characters with *
                for(int i = 0; i < raw.Length; i++)
                {
                    if (raw[i] > 126) raw[i] = 42;
                }
                
                result = System.Text.Encoding.UTF8.GetString(raw);
            }
            catch (Exception ex)
            {
                Console.WriteLine("URL: " + URL);
                Console.WriteLine(ex.ToString());
                result = ex.ToString();
            }
            finally
            {
                abstractText = result;
                isWorking = false;
            }
        }
    }
}
