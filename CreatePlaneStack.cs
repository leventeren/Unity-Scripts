using UnityEngine;

[ExecuteInEditMode]
public class CreatePlaneStack : MonoBehaviour
{
    public MeshFilter meshFilter;

    [Range(1, 64)]
    public int layers = 4;
    [Space]
    [Header("Dimensions")]
    [Range(1f, 50f)]
    public float width = 5f;
    [Range(1f, 50f)]
    public float length = 5f;
    [Range(1f, 50f)]
    public float height = 5f;

    [Header("Subdivision")]
    [Range(1, 64)]
    public int widthSegments = 6;
    [Range(1, 64)]
    public int lengthSegments = 6;

    [Header("UV")]
    [Range(0f, 4f)]
    public float uvWidth = 1f;
    [Range(0f, 4f)]
    public float uvLength = 1f;

    private void OnEnable()
    {
        if (!meshFilter) meshFilter = this.GetComponent<MeshFilter>();

        CreatePlaneMesh();
    }

    //Fired whenever inspector parameters are changed
    private void OnValidate()
    {
        CreatePlaneMesh();
    }

    public Mesh CreatePlaneMesh()
    {
        if (!meshFilter) return null;

        Mesh m = new Mesh();
        m.name = "StackedPlane_" + this.GetInstanceID();

        //Note: Does not take Unity's limit of 65k vertices into account, could explode
        int xCount = (widthSegments + 1);
        int zCount = (lengthSegments + 1);
        int triCount = (widthSegments * lengthSegments * 6);
        int vertCount = (xCount * zCount) * layers;

        Vector3[] vertices = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];
        int[] triangles = new int[triCount];
        Color[] colors = new Color[vertCount];

        float uvFactorX = uvWidth / widthSegments;
        float uvFactorY = uvLength / lengthSegments;
        float scaleX = width / widthSegments;
        float scaleY = length / lengthSegments;

        //The height each layer is offset by, in order to fit into the mesh height
        float layerHeight = (float)layers / height;

        int vertID = 0;
        for (int i = 0; i < layers; i++)
        {
            //Normalized value (0-1) over the height of the mesh
            float h = (float)i / layerHeight;

            for (float z = 0f; z < zCount; z++)
            {
                for (float x = 0f; x < xCount; x++)
                {
                    //Offset vertices on XZ axis by half of the mesh's size, to create a centered pivot point
                    vertices[vertID] = new Vector3(x * scaleX - (width * 0.5f), h, z * scaleY - (length * 0.5f));

                    //Store the relative height of the layers into the red channel. Creates a vertical gradient used in shaders
                    colors[vertID] = new Color((float)i / layers, 0f, 0f, 0f);

                    //You can swap X with vertices[vertID].x to do world-space UV's
                    uvs[vertID++] = new Vector2(x * uvFactorX, z * uvFactorY);
                }
            }
        }

        //Triangles for a single plane
        vertID = 0;
        for (int z = 0; z < lengthSegments; z++)
        {
            for (int x = 0; x < widthSegments; x++)
            {
                triangles[vertID] = (z * xCount) + x;
                triangles[vertID + 1] = ((z + 1) * xCount) + x;
                triangles[vertID + 2] = (z * xCount) + x + 1;

                triangles[vertID + 3] = ((z + 1) * xCount) + x;
                triangles[vertID + 4] = ((z + 1) * xCount) + x + 1;
                triangles[vertID + 5] = (z * xCount) + x + 1;
                vertID += 6;
            }
        }

        //Triangles for total planes
        triCount = (triangles.Length * layers);
        int[] layeredTris = new int[triCount];

        for (int i = 0; i < layers; i++)
        {
            for (int v = 0; v < triangles.Length; v++)
            {
                layeredTris[i * triangles.Length + v] = triangles[v] + ((vertices.Length / layers) * i);
            }
        }

        //Assign to mesh
        m.vertices = vertices;
        m.colors = colors;
        m.uv = uvs;
        m.triangles = layeredTris;

        m.RecalculateTangents();
        m.RecalculateNormals();
        m.RecalculateBounds();

        if (meshFilter) meshFilter.sharedMesh = m;

        return m;
    }
}
