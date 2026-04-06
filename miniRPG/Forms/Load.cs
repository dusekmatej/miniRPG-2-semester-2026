// ReSharper disable InconsistentNaming

using Timer = System.Windows.Forms.Timer;

namespace miniRPG.Forms;

public partial class Load : Form
{
    private const int LABEL_OFFSET = 25;
    private const int PROGRESS_OFFSET = 40;
    
    public Load()
    {
        InitializeComponent();
        PlaceItems();
        _progressBar.Maximum = 105;
    }

    private void PlaceItems()
    {
        // Place the label
        _lblMain.Left = (ClientSize.Width - _lblMain.Width) / 2;
        _lblMain.Top = ((ClientSize.Height - _lblMain.Height) / 2) - LABEL_OFFSET;
        // Place the progress bar
        _progressBar.Left = (ClientSize.Width - _progressBar.Width) / 2;
        _progressBar.Top = ((ClientSize.Height - _lblMain.Height) / 2) + PROGRESS_OFFSET;
    }

    private void IncreaseProgressBar(object? sender, EventArgs e)
    {
        _progressBar.Value += 35;

        if (_progressBar.Value > 100)
        {
            _loadTimer.Enabled = false;
            _loadTimer.Stop();
            Close();
        }
    }
}