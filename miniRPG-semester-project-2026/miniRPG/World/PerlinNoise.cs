using System.Numerics;

namespace miniRPG.GameEngine.Other;

public class PerlinNoise
{
    private int[] perm;

    public PerlinNoise(int seed)
    {
        perm = new int[512];
        int[] basePerm = GeneratePermutation(seed);

        for (int i = 0; i < 512; i++)
            perm[i] = basePerm[i % 256];
    }

    public float Sample(float x, float y)
    {
        // Floor once (optimization)
        int xi = (int)MathF.Floor(x) & 255;
        int yi = (int)MathF.Floor(y) & 255;

        float xf = x - MathF.Floor(x);
        float yf = y - MathF.Floor(y);

        // Hash corners
        int A = (perm[xi] + yi) & 255;
        int B = (perm[xi + 1] + yi) & 255;

        int AA = perm[A];
        int AB = perm[(A + 1) & 255];
        int BA = perm[B];
        int BB = perm[(B + 1) & 255];

        // Gradients
        Vector2 gradAA = GetGradient(AA);
        Vector2 gradAB = GetGradient(AB);
        Vector2 gradBA = GetGradient(BA);
        Vector2 gradBB = GetGradient(BB);

        // Dot products
        float dotAA = Dot(gradAA, xf, yf);
        float dotBA = Dot(gradBA, xf - 1, yf);
        float dotAB = Dot(gradAB, xf, yf - 1);
        float dotBB = Dot(gradBB, xf - 1, yf - 1);

        // Fade curves
        float u = Fade(xf);
        float v = Fade(yf);

        // Interpolate
        float lerpBottom = Lerp(dotAA, dotBA, u);
        float lerpTop = Lerp(dotAB, dotBB, u);
        float result = Lerp(lerpBottom, lerpTop, v);

        // Normalize to 0–1
        return (result + 1f) / 2f;
    }

    private float Dot(Vector2 grad, float x, float y)
    {
        return grad.X * x + grad.Y * y;
    }

    private float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }

    private int[] GeneratePermutation(int seed)
    {
        Random rand = new Random(seed);

        int[] p = new int[256];

        for (int i = 0; i < 256; i++)
            p[i] = i;

        for (int i = 255; i > 0; i--)
        {
            int swapIndex = rand.Next(i + 1);

            int temp = p[i];
            p[i] = p[swapIndex];
            p[swapIndex] = temp;
        }

        return p;
    }

    private Vector2 GetGradient(int hash)
    {
        switch (hash & 3)
        {
            case 0: return new Vector2(1, 1);
            case 1: return new Vector2(-1, 1);
            case 2: return new Vector2(1, -1);
            default: return new Vector2(-1, -1);
        }
    }
}