using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.System;

public class DirectionDetectionSystem
{
    public void Update(World world)
    {
        foreach (var entity in world.Entities)
        {
            var velocityComponent = entity.GetComponent<VelocityComponent>();
            var animationComp = entity.GetComponent<AnimationComponent>();

            if (velocityComponent == null || animationComp == null)
                continue;
            
            var velocity = velocityComponent;
            var threshold = 0.1f;

            if (Math.Abs(velocity.X) < threshold && Math.Abs(velocity.Y) < threshold)
            {
                animationComp.IsIdle = true;
                animationComp.IsRunningUp = false;
                animationComp.IsRunningDown = false;
                animationComp.IsRunningLeft = false;
                animationComp.IsRunningRight = false;
                continue;
            }

            animationComp.IsIdle = false;
            animationComp.IsRunningUp = false;
            animationComp.IsRunningDown = false;
            animationComp.IsRunningLeft = false;
            animationComp.IsRunningRight = false;

            if (Math.Abs(velocity.X) >= Math.Abs(velocity.Y))
            {
                if (velocity.X < -threshold)
                    animationComp.IsRunningLeft = true;
                else if (velocity.X > threshold)
                    animationComp.IsRunningRight = true;
            }
            else
            {
                if (velocity.Y < -threshold)
                    animationComp.IsRunningUp = true;
                else if (velocity.Y > threshold)
                    animationComp.IsRunningDown = true;
            }
        }
    }
}