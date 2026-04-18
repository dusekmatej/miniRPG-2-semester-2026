using System.Collections.Generic;
using System.Drawing;

namespace miniRPG.GameEngine.Components;

public class AnimationComponent
{
    public int CurrentFrameIndex;
    public Texture? CurrentFrame;

    public bool IsIdle;
    public bool IsRunningUp;
    public bool IsRunningDown;
    public bool IsRunningRight;
    public bool IsRunningLeft;
    
    public Texture[] AnimationFramesIdle;
    public Texture[] AnimationFramesUp;
    public Texture[] AnimationFramesDown;
    public Texture[] AnimationFramesLeft;
    public Texture[] AnimationFramesRight;
}