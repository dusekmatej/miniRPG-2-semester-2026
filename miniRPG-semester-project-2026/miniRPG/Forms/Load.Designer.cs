using System.ComponentModel;
using Timer = System.Threading.Timer;

namespace miniRPG.Forms;

partial class Load
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        _lblMain = new System.Windows.Forms.Label();
        _progressBar = new System.Windows.Forms.ProgressBar();
        SuspendLayout();
        // 
        // _lblMain
        // 
        _lblMain.Font = new System.Drawing.Font("Segoe UI", 24F);
        _lblMain.Location = new System.Drawing.Point(12, 41);
        _lblMain.Name = "_lblMain";
        _lblMain.Size = new System.Drawing.Size(292, 50);
        _lblMain.TabIndex = 0;
        _lblMain.Text = "Loading...";
        _lblMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // _testTimer
        //
        _loadTimer = new();
        _loadTimer.Enabled = true;
        _loadTimer.Interval = 1000;
        _loadTimer.Start();
        _loadTimer.Tick += IncreaseProgressBar;
        // 
        // _progressBar
        // 
        _progressBar.Location = new System.Drawing.Point(12, 132);
        _progressBar.Name = "_progressBar";
        _progressBar.Size = new System.Drawing.Size(292, 23);
        _progressBar.TabIndex = 1;
        // 
        // Load
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(400, 175);
        Controls.Add(_progressBar);
        Controls.Add(_lblMain);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Load";
        ResumeLayout(false);
    }

    private System.Windows.Forms.ProgressBar _progressBar;
    private System.Windows.Forms.Timer _loadTimer;
    private System.Windows.Forms.Label _lblMain;

    #endregion
}