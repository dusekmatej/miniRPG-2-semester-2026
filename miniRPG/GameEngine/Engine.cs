using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Entities;
using miniRPG.GameEngine.System;

namespace miniRPG.GameEngine;

public class Engine
{
    public World World = new();
    private PlayerInputSystem Input = new();
    public GeneralRenderSystem GeneralRender = new GeneralRenderSystem();
    private MovementSystem Movement = new MovementSystem();
    public AnimationSystem Animation = new AnimationSystem();
    
    // Update now accepts elapsed milliseconds since last update
    public void Update(int deltaMs)
    {
        Input.Update(World);
        Movement.Update(World);
        Animation.Update(World, deltaMs);
    }
}