using System.ComponentModel;
using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace miniRPG.Forms;

partial class MainForm
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
        components = new System.ComponentModel.Container();
        _tmrGameLoop = new System.Windows.Forms.Timer(components);
        SuspendLayout();
        // 
        // _tmrGameLoop
        // 
        _tmrGameLoop.Enabled = true;
        _tmrGameLoop.Interval = 16;
        _tmrGameLoop.Tick += Update;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        DoubleBuffered = true;
        KeyPreview = true;
        Text = "Mainform";
        Paint += MainForm_Paint;
        KeyDown += MainForm_KeyDown;
        KeyUp += MainForm_KeyUp;
        MouseMove += MainForm_MouseMove;
        ResumeLayout(false);
    }

    private System.Windows.Forms.Timer _tmrGameLoop;
    private System.Diagnostics.Stopwatch _stopwatch;

    #endregion
}