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
    
    public required Texture[] AnimationFramesIdle;
    public required Texture[] AnimationFramesUp;
    public required Texture[] AnimationFramesDown;
    public required Texture[] AnimationFramesLeft;
    public required Texture[] AnimationFramesRight;
}