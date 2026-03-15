using System.Collections.Generic;
using System.Drawing;

namespace miniRPG.GameEngine.Components;

public class AnimationComponent
{
    public int CurrentFrameIndex;
    public Image? CurrentFrame;

    public bool IsIdle;
    public bool IsRunningUp;
    public bool IsRunningDown;
    public bool IsRunningRight;
    public bool IsRunningLeft;
    
    public List<Image>? AnimationFramesIdle;
    public List<Image>? AnimationFramesUp;
    public List<Image>? AnimationFramesDown;
    public List<Image>? AnimationFramesLeft;
    public List<Image>? AnimationFramesRight;
}