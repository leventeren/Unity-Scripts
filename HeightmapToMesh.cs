using UnityEngine;

[ExecuteInEditMode]
public class HeightmapToMesh : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Texture2D heightmap;

    [Space]
    [Tooltip("This value should be the same as the source terrain's height as configured in its settings")]
    public float heightScale = 100;
    [Range(8, 8197)]
    [Tooltip("Uniform scale of the output mesh")]
    public int size = 512;
    [Range(1, 128)]
    [Tooltip("Distance between vertices in meters/units")]
    public int vertexDistance = 32;

    private const int MIN_SIZE = 16;
    private const int MAX_SIZE = 16392;

    private void OnEnable()
    {
        if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
    }

    private void OnValidate()
    {
        if (!meshFilter || !heightmap) return;

        meshFilter.sharedMesh = CreateMeshFromHeightmap(heightmap, size, heightScale, vertexDistance);
    }

    //Note: standalone function, can be moved to an editor script
    public static Mesh CreateMeshFromHeightmap(Texture2D heightmap, int size, float heightScale, int vertexDistance)
    {
        Mesh m = new Mesh();
        m.name = heightmap.name + "_mesh";

        int m_size = Mathf.Clamp(size, MIN_SIZE, MAX_SIZE);

        float vertexDensity = (m_size / vertexDistance);
        vertexDensity = Mathf.Clamp(vertexDensity, 1, 256);

        int xAmount = (int)vertexDensity + 1;
        int yAmount = (int)vertexDensity + 1;
        int vertexCount = xAmount * yAmount;
        int triangleCount = (int)vertexDensity * (int)vertexDensity * 6;

        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];
        int[] triangles = new int[triangleCount];

        int vertIndex = 0;

        //Ensure mesh retains size when changing vertex distance
        float scaleX = m_size / vertexDensity;
        float scaleY = m_size / vertexDensity;

        //Downscale heightmap texture if size is smaller than heightmap
        Texture2D m_heightmap = CopyAndScaleTexture(heightmap, m_size);

        for (int y = 0; y < xAmount; y++)
        {
            for (int x = 0; x < yAmount; x++)
            {
                //Texture sample positions

                //0-1 range
                float sX = (float)x / (float)xAmount;

                //Scale by texel size
                sX = Mathf.FloorToInt(m_heightmap.width * sX);

                float sY = (float)y / (float)yAmount;
                sY = Mathf.FloorToInt(m_heightmap.height * sY);

                float height = m_heightmap.GetPixel((int)sX, (int)sY).r * heightScale;

                vertices[vertIndex] = new Vector3(x * scaleX, height, y * scaleY);

                //Planar UV mapping on Y-axis
                uvs[vertIndex++] = new Vector2(x * (1f / vertexDensity), y * (1f / vertexDensity));
            }
        }

        vertIndex = 0;
        for (int y = 0; y < vertexDensity; y++)
        {
            for (int x = 0; x < vertexDensity; x++)
            {
                triangles[vertIndex] = (y * xAmount) + x;
                triangles[vertIndex + 1] = ((y + 1) * xAmount) + x;
                triangles[vertIndex + 2] = (y * xAmount) + x + 1;

                triangles[vertIndex + 3] = ((y + 1) * xAmount) + x;
                triangles[vertIndex + 4] = ((y + 1) * xAmount) + x + 1;
                triangles[vertIndex + 5] = (y * xAmount) + x + 1;
                vertIndex += 6;
            }
        }

        m.vertices = vertices;
        m.uv = uvs;
        m.triangles = triangles;
        m.RecalculateNormals();
        m.RecalculateTangents();
        m.RecalculateBounds();

        return m;
    }

    //Even if it doesn't require scaling, passing the texture through the GPU also has the benefit of the source heightmap not needing to be readable
    public static Texture2D CopyAndScaleTexture(Texture2D src, int resolution)
    {
        //Never upscale
        if (resolution > src.width) resolution = src.width;

        Rect texRect = new Rect(0, 0, resolution, resolution);

        RenderTexture rt = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGBFloat);
        rt.isPowerOfTwo = false;

        //Render quad
        Graphics.SetRenderTarget(rt);
        GL.LoadPixelMatrix(0, 1, 1, 0);
        GL.Clear(true, true, Color.black);
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);

        //GPU back to CPU
        Texture2D result = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false, true);
        result.ReadPixels(texRect, 0, 0, false);
        result.name = src.name;

        result.Apply();

        return result;
    }

}
