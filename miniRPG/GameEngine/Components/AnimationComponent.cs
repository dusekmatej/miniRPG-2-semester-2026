using System.Collections.Generic;
using System.Drawing;

namespace miniRPG.GameEngine.Components;

public class AnimationComponent
{
    public int CurrentFrameIndex;
    public List<Image>? AnimationFrames;
    public Image? CurrentFrame;
}