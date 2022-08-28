using UnityEngine;

public struct Triangle
{
    public float size;
    public Vector2 center;

    public Triangle(float size, Vector2 center)
    {
        this.size = size;
        this.center = center;
    }

    public Vector2 BottomLeft => center + new Vector2(-1, -1) * size;
    public Vector2 TopMiddle => center + new Vector2(0, 1) * size;
    public Vector2 BottomRight => center + new Vector2(1, -1) * size;
}

public static class TriangleExtensions
{
    public static Vector2 GetRandomVertex(this Triangle triangle,
        System.Random random)
    {
        int randomNum = random.Next(3);
        return randomNum == 0 ? triangle.BottomLeft : randomNum == 1
            ? triangle.TopMiddle : triangle.BottomRight;
    }

    public static Vector2 GetRandomPointWithin(this Triangle triangle,
        System.Random random)
    {
        Vector2 a = triangle.TopMiddle - triangle.BottomLeft;
        Vector2 b = triangle.BottomRight - triangle.BottomLeft;
        float u1 = (float)random.NextDouble();
        float u2 = (float)random.NextDouble();

        if (u1 + u2 > 1)
        {
            u1 = 1 - u1;
            u2 = 1 - u2;
        }

        return (u1 * a + u2 * b) + triangle.BottomLeft;
    }
}