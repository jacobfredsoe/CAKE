using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using System.Threading;

namespace CancerKeywords
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CancerKeywords : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont stdFont;
        int currentID = 0;
        AbstractFetcher abstractFetcher;
        bool gotTypes = false;
        Texture2D bubbleTexture;
        TextBubble keywordBubble;
        static SortedDictionary<string, CancerBubble> cancerBubbles;
        string progress;
        Random ran;
        List<int> IDs;
        int batchSize;
        ControlForm controlForm;
        string[] cancerTypes;
        bool canClick = true;
        static AbstractViewer abstractViewer;
        float timeSincelastUpdate = 500;

        public CancerKeywords() //CAncerKEywords (CAKE)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            abstractViewer = new AbstractViewer();
            abstractViewer.Show();

            controlForm = new ControlForm();
            controlForm.Show();

            //List of potentional cancer types
            cancerTypes = new string[] {"adenocarcinoma",
                                        "adrenocortical",
                                        "astrocytic",
                                        "bladder",
                                        "brain",
                                        "breast",
                                        "colerectal",
                                        "colon",
                                        "colorectal",
                                        "dysplasia",
                                        "embryonal",
                                        "endometrial",
                                        "endometrioid ",
                                        "epithelial",
                                        "esophageal",
                                        "fibrosarcoma",
                                        "gallbladder",
                                        "gastric",
                                        "gastrointestinal",
                                        "hepatic",
                                        "hapatocellular",
                                        "hepatocellular",
                                        "intestine",
                                        "intraepithelial",
                                        "liver",
                                        "lung",
                                        "mammary",
                                        "mediastinal",
                                        "melanoma",
                                        "mesenchymal",
                                        "nasopharyngeal",
                                        "neuroendocrine",
                                        "ovarian",
                                        "pancreatic",
                                        "papillary",
                                        "pituitary",
                                        "prostate",
                                        "rectal",
                                        "renal",
                                        "retinoblastoma",
                                        "squamous",
                                        "testicular",
                                        "thymic",
                                        "thyroid",
                                        "tonsillar",
                                        "urothelial"};

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            stdFont = Content.Load<SpriteFont>("StandardFont");
            bubbleTexture = Content.Load<Texture2D>("BubbleTexture");
            abstractFetcher = new AbstractFetcher();
            IDs = new List<int>();
            ran = new Random();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);

            if(!controlForm.isAlive || !abstractViewer.IsAlive)
            {
                this.Exit();
            }

            batchSize = controlForm.getSpeed();

            //If go has been activated in controlform, restart the search
            if(controlForm.newInfo())
            {
                IDs = new List<int>();
                currentID = 0;
                string keyword = controlForm.getSearchString();
                cancerBubbles = new SortedDictionary<string, CancerBubble>();
                keywordBubble = new TextBubble(bubbleTexture, 100, new Vector2(350, 350), keyword, stdFont, Color.Green);

                IDs = fetchIDs(keyword);

                abstractFetcher.fetchAbstract(new int[] { IDs[currentID] });
                currentID++;
            }


            //Keeping track of how many abstracts have been search and how many are left
            progress = "Processing abstract " + (currentID + 1) + " out of " + IDs.Count;
            if (IDs.Count != 0 && !abstractFetcher.IsWorking && abstractFetcher.LastID != IDs[currentID] && gotTypes)
            {
                int batchMax = batchSize;

                List<int> IDBatch = new List<int>();
                while (currentID < IDs.Count - 1 && batchMax > 0)
                {
                    IDBatch.Add(IDs[currentID]);
                    currentID++;
                    batchMax--;
                }
                if(IDBatch.Count != 0)
                {
                    abstractFetcher.fetchAbstract(IDBatch.ToArray());
                    gotTypes = false;
                }
            }


            //If the abstract fetcher is done and the types have been extracted, add the new cancer types
            if (IDs.Count != 0 && !abstractFetcher.IsWorking && !gotTypes)
            {
                Dictionary<int, string> abs = abstractFetcher.getAbstractKvP();

                foreach(KeyValuePair<int, string> kvp in abs)
                {
                    List<string> types = Misc.getCancerTypes(kvp.Value);

                    foreach (string type in types)
                    {
                        if (Misc.isInCancerList(type, cancerTypes)) //If it is a cancer
                        {
                            if (cancerBubbles.ContainsKey(type)) //And the bubble already exists, add 1 to the bubble
                            {
                                cancerBubbles[type].addCase(kvp.Key);
                            }
                            else //Else create a new bubble for the cancer
                            {
                                cancerBubbles.Add(type, new CancerBubble(bubbleTexture, stdFont, type, 300, keywordBubble));
                                cancerBubbles[type].Angle = ran.Next(0, 360);
                                cancerBubbles[type].addCase(kvp.Key);
                            }
                            keywordBubble.addCase(); //Add one to the total number
                        }
                    }
                }

                //Jobs done!
                gotTypes = true;
                controlForm.postResult(cancerBubbles);
            }


            //Check if any bubbles are colliding and update the size of the biggest bubble
            if (IDs.Count != 0)
            {
                int maxCount = 0;
                String[] keys = new String[cancerBubbles.Count];
                cancerBubbles.Keys.CopyTo(keys, 0);

                for (int i = 0; i < keys.Length; i++)
                {
                    for(int j = 1; j < keys.Length; j++)
                    {
                        if(cancerBubbles[keys[i]].getBoundingCircle().intersect(cancerBubbles[keys[j]].getBoundingCircle()))
                        {
                            cancerBubbles[keys[i]].collide(cancerBubbles[keys[j]]);
                        }
                    }
                    if (maxCount < cancerBubbles[keys[i]].Cases) maxCount = cancerBubbles[keys[i]].Cases;
                }

                //Update all the bubbles
                keywordBubble.highestCount = maxCount;
                keywordBubble.Update(gameTime);

                MouseState mouse = Mouse.GetState();
                Vector2 mouseLocation = new Vector2(mouse.X, mouse.Y);

                foreach (KeyValuePair<string, CancerBubble> kvp in cancerBubbles)
                {
                    kvp.Value.Update(gameTime);

                    //If mouse is over the bubble
                    if (Vector2.Distance(kvp.Value.getCenter(), mouseLocation) <= kvp.Value.Size / 2)
                    {
                        //And is clicked, select it
                        if(mouse.LeftButton == ButtonState.Pressed && canClick)
                        {
                            setSelected(kvp.Key);
                            canClick = false;
                        }
                    }
                }
            }

            //Update the abstract viewer
            updateAbstractViewer(gameTime);
            
            if (Mouse.GetState().LeftButton == ButtonState.Released) canClick = true;
        }

        /// <summary>
        /// Selectes the bubble with the given name
        /// </summary>
        /// <param name="bubblename">Name of the bublble</param>
        public void setSelected(string bubblename)
        {
            foreach (KeyValuePair<string, CancerBubble> kvp in cancerBubbles)
            {
                if(kvp.Key.Equals(bubblename))
                {
                    kvp.Value.Selected = true;
                }
                else
                {
                    kvp.Value.Selected = false;
                }
            }
        }

        /// <summary>
        /// Allows the main program to the notified if a bubble has become selected
        /// </summary>
        /// <param name="cbubble">The selected bubble</param>
        internal static void selectedChanged(CancerBubble cbubble)
        {
            abstractViewer.sendAbstracts(cbubble);
        }

        /// <summary>
        /// Allows the main program to the notified if a bubble has added an abstract
        /// </summary>
        /// <param name="pubmedID">Pubmed ID of the abstract</param>
        internal static void abstractAdded(int pubmedID)
        {
            abstractViewer.addAbstract(pubmedID);
        }

        /// <summary>
        /// Allows the main program to the notified if the abstract viewer should be updated
        /// </summary>
        /// <param name="gameTime"></param>
        internal static void updateAbstractViewer(GameTime gameTime)
        {
            abstractViewer.update(gameTime);
        }

        /// <summary>
        /// Fetches the abstact IDs from a given pubmed search
        /// </summary>
        /// <param name="keyword">the search string for pubmed</param>
        /// <param name="start">optional param: starting position for recursive calls</param>
        /// <returns>List of IDs</returns>
        private List<int> fetchIDs(string keyword, int start = 1)
        {
            List<int> results = new List<int>();
            //Fetches the XML document from the pubmed search with cancer + keyword
            string searchString = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term=" + keyword + "&retstart=" + start + "&retmax=10000";
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(searchString);

            int count = 0;

            //Extract the IDs from all the papers.
            foreach (XmlNode xNode in xDoc.DocumentElement.ChildNodes)
            {
                if (xNode.Name == "IdList")
                {
                    foreach (XmlNode IdNodes in xNode.ChildNodes)
                    {
                        results.Add(Convert.ToInt32(IdNodes.InnerText));
                    }
                }
                if(xNode.Name == "Count")
                {
                    count = Convert.ToInt32(xNode.InnerText);
                }
            }

            //Pubmed eutils wont return more than 10,000 IDs in one go. Consequently recursive calls are made until all IDs have been fetched.
            if(count - start >= 10000)
            {
                results.AddRange(fetchIDs(keyword, start + 10000));
            }

            return results;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            base.Draw(gameTime);

            //Draw the text
            if (IDs.Count != 0)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(stdFont, progress, new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(stdFont, "Speed: " + controlForm.getSpeed().ToString(), new Vector2(0, 20), Color.Black);
                spriteBatch.End();

                //Draw the bubbles
                foreach (KeyValuePair<string, CancerBubble> kvp in cancerBubbles)
                {
                    kvp.Value.Draw(spriteBatch);
                }

                keywordBubble.Draw(spriteBatch);
            }
        }
    }
}
