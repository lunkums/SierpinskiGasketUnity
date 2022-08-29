using UnityEngine;

public class SierpinskiGasket : MonoBehaviour
{
    [SerializeField][Range(0, 10000)] private int maxPoints = 1000;
    [SerializeField][Range(1, 5)] private float largeTriangleSize = 3.0f;
    [SerializeField][Range(0.001f, 0.01f)] private float smallTriangleSize = 0.01f;
    [SerializeField][Min(0)] private int seed = 8080;
    [SerializeField][Range(0, 0.5f)] private float secondsBetweenFill = 0.1f;
    [SerializeField][Range(0.001f, 0.1f)] private float percentageOfPointsToFill = 0.1f;

    private Material lineMaterial;
    private int currentPoints;
    private float timer;
    private bool manualMode;

    private void Awake()
    {
        lineMaterial = CreateLineMaterial();
        currentPoints = 0;
        timer = secondsBetweenFill;
        manualMode = true;
    }

    private int Points
    {
        get => currentPoints;
        set => currentPoints = Mathf.Min(value, maxPoints);
    }

    public int MaxPoints
    {
        set
        {
            maxPoints = value;
            if (manualMode)
            {
                ReplayAnimation();
            }
            else
            {
                currentPoints = maxPoints;
            }
        }
    }

    public float FillPercentage
    {
        set => percentageOfPointsToFill = value;
    }

    public int Seed
    {
        get => seed;
        set
        {
            seed = Mathf.Abs(value);
            ReplayAnimation();
        }
    }

    public bool ManualMode
    {
        set
        {
            manualMode = value;
            currentPoints = manualMode ? 0 : maxPoints;
        }
    }

    private void Update()
    {
        if (!manualMode)
        {
            seed = Random.Range(0, 999999999);
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Points += Mathf.CeilToInt(maxPoints * percentageOfPointsToFill);
            timer = secondsBetweenFill;
        }
    }

    public void ReplayAnimation()
    {
        Points = 0;
    }

    /*
     * GL rendering methods
     */

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

    private void OnRenderObject()
    {
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(GL.LINES);

        SubmitGasket();

        GL.End();
        GL.PopMatrix();
    }

    /*
     * Gasket rendering methods
     */

    private void SubmitGasket()
    {
        Triangle outer = new Triangle(largeTriangleSize, Vector2.zero);
        System.Random random = new System.Random(seed);
        Vector2 currentPoint = Initialize(outer, random);
        Vector2 randomVertex, betweenPoint;
        float colorRatio,
            maxDistance = Vector2.Distance(outer.topMiddle, outer.bottomLeft);

        for (int i = 0; i < Points; i++)
        {
            randomVertex = outer.GetRandomVertex(random);
            betweenPoint = (currentPoint + randomVertex) / 2;
            colorRatio = Vector2.Distance(betweenPoint, outer.topMiddle) / maxDistance;

            GL.Color(new Color(colorRatio, 1 - colorRatio, 0));
            SubmitTriangle(new Triangle(smallTriangleSize, betweenPoint));
            currentPoint = betweenPoint;
        }

        Points++;
    }

    private Vector2 Initialize(Triangle triangle, System.Random random)
    {
        SubmitTriangle(triangle);
        return triangle.GetRandomPointWithin(random);
    }

    private void SubmitTriangle(Triangle triangle)
    {
        Line(triangle.bottomLeft, triangle.topMiddle);
        Line(triangle.topMiddle, triangle.bottomRight);
        Line(triangle.bottomRight, triangle.bottomLeft);
    }

    private void Line(Vector2 a, Vector2 b)
    {
        GL.Vertex3(a.x, a.y, 0);
        GL.Vertex3(b.x, b.y, 0);
    }
}
