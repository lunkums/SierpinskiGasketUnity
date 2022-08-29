using UnityEngine;

public struct Triangle
{
    public readonly float size;
    public readonly Vector2 center;

    public readonly Vector2 bottomLeft;
    public readonly Vector2 topMiddle;
    public readonly Vector2 bottomRight;

    public Triangle(float size, Vector2 center)
    {
        this.size = size;
        this.center = center;

        bottomLeft = center + new Vector2(-1, -1) * size;
        topMiddle = center + new Vector2(0, 1) * size;
        bottomRight = center + new Vector2(1, -1) * size;
    }
}

public static class TriangleExtensions
{
    public static Vector2 GetRandomVertex(this Triangle triangle,
        System.Random random)
    {
        int randomNum = random.Next(3);
        return randomNum == 0 ? triangle.bottomLeft : randomNum == 1
            ? triangle.topMiddle : triangle.bottomRight;
    }

    public static Vector2 GetRandomPointWithin(this Triangle triangle,
        System.Random random)
    {
        Vector2 a = triangle.topMiddle - triangle.bottomLeft;
        Vector2 b = triangle.bottomRight - triangle.bottomLeft;
        float u1 = (float)random.NextDouble();
        float u2 = (float)random.NextDouble();

        if (u1 + u2 > 1)
        {
            u1 = 1 - u1;
            u2 = 1 - u2;
        }

        return (u1 * a + u2 * b) + triangle.bottomLeft;
    }
}