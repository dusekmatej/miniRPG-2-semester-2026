using miniRPG.GameEngine.Components;

namespace miniRPG.Helpers;

public static class OtherHelpers
{
    public static float GetPercentage(this float maxValue, float percentage)
    {
        return maxValue * (percentage / 100f);
    }

    public static float Lerp(float start, float end, float amount)
    {
        return start + (end - start) * amount;
    }
    
    public static float GetDistance(TransformComponent self, TransformComponent other)
    {
        var distanceX = self.X - other.X;
        var distanceY = self.Y - other.Y;
        return distanceX * distanceX + distanceY * distanceY; // Use pythagoras to get straight line to it
    } // NOTE: OUTPUT IS SQUARED DISTANCE
}