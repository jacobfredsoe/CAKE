using System;
using System.Windows.Forms;
using System.Threading;

namespace CancerKeywords
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main(string[] args)
        {
            CancerKeywords game = new CancerKeywords();
            game.Run();
        }
    }
#endif
}

