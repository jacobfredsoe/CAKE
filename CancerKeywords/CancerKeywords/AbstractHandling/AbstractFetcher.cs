using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;

namespace CancerKeywords
{
    class AbstractFetcher
    {
        private bool isWorking;
        private WebClient wc;
        public int LastID { get; set; }
        private string abstractText;
        int[] currentIDs;
        private bool isDone;

        /// <summary>
        /// Constructor
        /// </summary>
        public AbstractFetcher()
        {
            isWorking = false;
            isDone = false;
            wc = new WebClient();
            LastID = 0;
            abstractText = "";
        }

        /// <summary>
        /// Is the fetcher currently in the progress of fetching an abstract
        /// </summary>
        /// <returns>True while trying to fetch abstract</returns>
        public bool IsWorking
        {
            get { return isWorking; }
        }

        /// <summary>
        /// Is true when the fetcher is done with its job
        /// </summary>
        public bool IsDone
        {
            get { return isDone; }
        }

        /// <summary>
        /// Returns the abstract text of all abstracts fetched as one big string
        /// </summary>
        /// <returns>The value currently stored as the abstract</returns>
        public string AbstractText
        {
            get { return abstractText; }
        }

        /// <summary>
        /// Returns the abstracts as a dictionary with pubmedID as key
        /// </summary>
        /// <returns>Dictionary of abstracts with PMID as key</returns>
        public Dictionary<int, string> getAbstractKvP()
        {
            //Create the dictionary
            Dictionary<int, string> results = new Dictionary<int, string>();

            //Split all the abstracts
            string[] abstractTexts = Regex.Split(abstractText, "\n\n\n");

            //Get the PMID and add the abstract to the dictionary
            for (int i = 0; i < abstractTexts.Length; i++ )
            {
                results.Add(currentIDs[i], abstractTexts[i]);
            }

            return results;

        }
        
        /// <summary>
        /// Will attempt to fetch the abstract from an ID. When called, the fetcher will show as working
        /// until the abstract has been parsed.
        /// </summary>
        /// <param name="ID">The ID number of the abstract</param>
        public void fetchAbstract(int[] IDs)
        {
            isWorking = true;
            currentIDs = IDs;
            string idString = "";
            
            for(int i = 0; i < IDs.Length; i++)
            {
                idString += IDs[i] + ",";
            }

            abstractText = "fetching abstract of ID=" + idString;
            LastID = IDs[IDs.Length-1];
            string URL = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&id=" + idString + "&retmode=text&rettype=abstract";

            //In order to allow other resources to function while this one is trying to fetch the abstract, a new thread is created for each URL.
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
                isDone = true;
            }
        }
    }
}
