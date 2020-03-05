//modified from http://ilkinulas.github.io/development/unity/2016/04/30/cube-mesh-in-unity3d.html
//modification adds the ability to specify an offset and size
void CreateCube(Vector3 offset, Vector3 size) {

    Vector3[] vertices = {
        new Vector3 (0, 0, 0),
        new Vector3 (1, 0, 0),
        new Vector3 (1, 1, 0),
        new Vector3 (0, 1, 0),
        new Vector3 (0, 1, 1),
        new Vector3 (1, 1, 1),
        new Vector3 (1, 0, 1),
        new Vector3 (0, 0, 1),
    };

    for(int i=0; i < vertices.Length; i++) {
        vertices[i].Scale(size);
        vertices[i] += offset;
    }

    int[] triangles = {
        0, 2, 1, //face front
		0, 3, 2,
        2, 3, 4, //face top
		2, 4, 5,
        1, 2, 5, //face right
		1, 5, 6,
        0, 7, 4, //face left
		0, 4, 3,
        5, 4, 7, //face back
		5, 7, 6,
        0, 6, 7, //face bottom
		0, 1, 6
    };

    Mesh mesh = GetComponent<MeshFilter>().mesh;

    mesh.Clear();

    mesh.vertices = vertices;
    mesh.triangles = triangles;

    mesh.RecalculateNormals();
}
