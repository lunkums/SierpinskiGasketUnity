using UnityEngine;

public class SierpinskiGasket : MonoBehaviour
{
    [SerializeField][Range(0, 10000)] private int points = 1000;
    [SerializeField][Range(1, 5)] private float largeTriangleSize = 3.0f;
    [SerializeField][Range(0.001f, 0.01f)] private float smallTriangleSize = 0.01f;

    private Material lineMaterial;

    private void Awake()
    {
        lineMaterial = CreateLineMaterial();
    }

    private static Material CreateLineMaterial()
    {
        Material lineMaterial;

        // Unity has a built-in shader that is useful for drawing
        // simple colored things.
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        lineMaterial.SetInt("_ZWrite", 0);

        return lineMaterial;
    }

    public void OnRenderObject()
    {
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(GL.LINES);

        SubmitGasket();

        GL.End();
        GL.PopMatrix();
    }

    private void SubmitGasket()
    {
        Triangle outer = new Triangle(largeTriangleSize, Vector2.zero);
        Vector2 currentPoint = Initialize(outer);
        Vector2 randomVertex, betweenPoint;
        float colorRatio,
            maxDistance = Vector2.Distance(outer.TopMiddle, outer.BottomLeft);

        for (int i = 0; i < points; i++)
        {
            randomVertex = outer.GetRandomVertex();
            betweenPoint = (currentPoint + randomVertex) / 2;
            colorRatio = Vector2.Distance(betweenPoint, outer.TopMiddle) / maxDistance;

            GL.Color(new Color(colorRatio, 1 - colorRatio, 0));
            SubmitTriangle(new Triangle(smallTriangleSize, betweenPoint));
            currentPoint = betweenPoint;
        }
    }

    private Vector2 Initialize(Triangle triangle)
    {
        SubmitTriangle(triangle);
        return triangle.GetRandomPointWithin();
    }

    private void SubmitTriangle(Triangle triangle)
    {
        Line(triangle.BottomLeft, triangle.TopMiddle);
        Line(triangle.TopMiddle, triangle.BottomRight);
        Line(triangle.BottomRight, triangle.BottomLeft);
    }

    private void Line(Vector2 a, Vector2 b)
    {
        GL.Vertex3(a.x, a.y, 0);
        GL.Vertex3(b.x, b.y, 0);
    }
}
