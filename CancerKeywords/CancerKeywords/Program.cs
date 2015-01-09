using System;
using System.Windows.Forms;
using System.Threading;

namespace CancerKeywords
{
#if WINDOWS || XBOX
    static class Program
    {
        private static ControlForm controlForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static void Main(string[] args)
        {
            controlForm = new ControlForm();
            controlForm.Show();

            CancerKeywords game = new CancerKeywords(controlForm);
            game.Run();
        }
    }
#endif
}

