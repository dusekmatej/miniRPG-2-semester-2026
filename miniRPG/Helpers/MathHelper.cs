namespace miniRPG.Helpers;

public static class MathHelper
{
    public static float GetPercentage(this float maxValue, float percentage)
    {
        return maxValue * (percentage / 100f);
    }

    public static float Lerp(float start, float end, float amount)
    {
        return start + (end - start) * amount;
    }
}