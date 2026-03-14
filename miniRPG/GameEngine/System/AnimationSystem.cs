using System.Collections.Generic;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class AnimationSystem
{
    private readonly List<AnimationComponent> _animationComponents = new();

    // milliseconds between frames
    private readonly int _frameIntervalMs = 200;
    private int _accumulatorMs = 0;

    // Driven by the main game loop; deltaMs is elapsed ms since last update
    public void Update(World world, int deltaMs)
    {
        _animationComponents.Clear();

        foreach (var entity in world.Entities)
        {
            if (!entity.HasComponent<AnimationComponent>())
                continue;

            var comp = entity.GetComponent<AnimationComponent>();
            if (comp != null)
                _animationComponents.Add(comp);
        }

        if (_animationComponents.Count == 0)
            return;

        _accumulatorMs += deltaMs;
        if (_accumulatorMs < _frameIntervalMs)
            return;

        // advance frames as many intervals passed
        var steps = _accumulatorMs / _frameIntervalMs;
        _accumulatorMs = _accumulatorMs % _frameIntervalMs;

        foreach (var comp in _animationComponents)
        {
            var frames = comp.AnimationFrames;
            if (frames == null || frames.Count == 0)
                continue;

            // Ensure index is inside bounds
            if (comp.CurrentFrameIndex < 0 || comp.CurrentFrameIndex >= frames.Count)
                comp.CurrentFrameIndex = 0;

            comp.CurrentFrameIndex = (comp.CurrentFrameIndex + steps) % frames.Count;
            comp.CurrentFrame = frames[comp.CurrentFrameIndex];
        }
    }
}