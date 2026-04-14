using miniRPG.Forms;

namespace miniRPG;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // First show the load screen only
        // using (Load loadScreen = new Load())
        // {
        //     loadScreen.ShowDialog();
        // }
        // Uncomment when done!
        
        Application.Run(new MainForm()); 
        // Application.Run(new Load()); // Only testing purposes
    }
}