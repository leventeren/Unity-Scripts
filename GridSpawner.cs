using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum shapeGrid
{
    Square = 0,
    Triangle = 1,
    Hexagon = 2,
}

public enum matGrid
{
    Standard = 0,
    HDRP = 1,
}

public class GridSpawner : EditorWindow
{
    string baseNameCell;
    public matGrid matShaderSelected;
    public shapeGrid shapeSelected;
    float sizeCell;
    float thicknessCell;
    int rowsGrid;
    int columnsGrid;
    float gutterGrid;
    public Object prefabSource;
    float sizePrefab;
    int amountRandom;

    float xPos;
    float zPos;
    bool triangleUp;
    bool rowEven;

    int numCell;
    int numPrefab;

    float cellAmount;
    int currentRandomCell;
    float placePrefab;
    bool[] cellPrefab;

    [MenuItem("Window/Grid Spawner")]
    static void OpenWindow()
    {
        // Setting up editor window
        GridSpawner window = (GridSpawner)GetWindow(typeof(GridSpawner), false, "Grid Spawner");
        window.minSize = new Vector2(150, 150);
        window.Show();
    }

    void OnGUI()
    {
        // Creating fields for the grid settings
        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
        baseNameCell = EditorGUILayout.TextField("Base Cell Name", baseNameCell);
        matShaderSelected = (matGrid)EditorGUILayout.EnumPopup("Base Material Shader", matShaderSelected);
        shapeSelected = (shapeGrid)EditorGUILayout.EnumPopup("Shape Cell ", shapeSelected);
        sizeCell = EditorGUILayout.FloatField("Size Cell", sizeCell);
        if (sizeCell < 0)
        {
            sizeCell = 0;
        }
        thicknessCell = EditorGUILayout.FloatField("Thickness Cell", thicknessCell);
        if (thicknessCell < 0)
        {
            thicknessCell = 0;
        }
        rowsGrid = EditorGUILayout.IntField("Rows", rowsGrid);
        if (rowsGrid < 0)
        {
            rowsGrid = 0;
        }
        columnsGrid = EditorGUILayout.IntField("Colums", columnsGrid);
        if (columnsGrid < 0)
        {
            columnsGrid = 0;
        }
        gutterGrid = EditorGUILayout.FloatField("Size Gutter", gutterGrid);
        if (gutterGrid < 0)
        {
            gutterGrid = 0;
        }

        EditorGUILayout.Space();

        // Creating fields for the prefab settings
        GUILayout.Label("Prefab settings", EditorStyles.boldLabel);
        prefabSource = EditorGUILayout.ObjectField("Prefab", prefabSource, typeof(Object), true);
        sizePrefab = EditorGUILayout.FloatField("Size", sizePrefab);
        if (sizePrefab < 0)
        {
            sizePrefab = 0;
        }

        EditorGUILayout.Space();

        // Creating fields for the prefab spawning settings
        GUILayout.Label("Prefab Spawning settings", EditorStyles.boldLabel);
        amountRandom = EditorGUILayout.IntSlider("Amount", amountRandom, 1, 100);

        EditorGUILayout.Space();

        // Creating button
        if (GUILayout.Button("Generate Grid"))
        {
            cellPrefab = new bool[columnsGrid * rowsGrid];

            GameObject parentPrefab = null;
            if (prefabSource != null)
            {
                if (sizePrefab != 0)
                {
                    parentPrefab = new GameObject("Prefabs");
                }
            }

            // Calculates the amount of cells that need to have a prefab present
            cellAmount = Mathf.Round((float)columnsGrid * (float)rowsGrid / 100 * (float)amountRandom);

            RandomPick();

            // Checks which base shape has been selected, and creates a grid
            // Square selected
            if ((int)shapeSelected == 0)
            {
                GameObject parentCell = new GameObject("Grid");
                for (int j = 0; j < rowsGrid; j++)
                {
                    for (int i = 0; i < columnsGrid; i++)
                    {
                        if (sizePrefab != 0)
                        {
                            if (cellPrefab[numCell])
                            {
                                numPrefab++;
                                GameObject prefabSpawned = (GameObject)Instantiate(prefabSource, new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0.5f), Quaternion.identity);
                                prefabSpawned.name = "Man" + numPrefab;
                                prefabSpawned.transform.parent = parentPrefab.transform;
                                prefabSpawned.transform.localScale = new Vector3(sizePrefab, sizePrefab, sizePrefab);
                            }
                        }
                        numCell++;
                        Square(parentCell);
                        xPos += sizeCell + gutterGrid;
                    }
                    xPos = 0f;
                    zPos += sizeCell + gutterGrid;
                }
                xPos = 0f;
                zPos = 0f;
                numCell = 0;
                numPrefab = 0;
                cellAmount = 0;
                currentRandomCell = 0;
                for (int i = 0; i < cellAmount; i++)
                {
                    cellPrefab[i] = false;
                }
            }
            // Triangle selected
            if ((int)shapeSelected == 1)
            {
                GameObject parentCell = new GameObject("Grid");
                for (int j = 0; j < rowsGrid; j++)
                {
                    for (int i = 0; i < columnsGrid; i++)
                    {
                        if (sizePrefab != 0)
                        {
                            if (triangleUp)
                            {
                                if (cellPrefab[numCell])
                                {
                                    GameObject prefabSpawned = (GameObject)Instantiate(prefabSource, new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0.33f), Quaternion.identity);
                                    numPrefab++;
                                    prefabSpawned.name = "Man" + numPrefab;
                                    prefabSpawned.transform.parent = parentPrefab.transform;
                                    prefabSpawned.transform.localScale = new Vector3(sizePrefab, sizePrefab, sizePrefab);
                                }
                            }
                            else
                            {
                                if (cellPrefab[numCell])
                                {
                                    GameObject prefabSpawned = (GameObject)Instantiate(prefabSource, new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0.67f), Quaternion.identity);
                                    numPrefab++;
                                    prefabSpawned.name = "Man" + numPrefab;
                                    prefabSpawned.transform.parent = parentPrefab.transform;
                                    prefabSpawned.transform.localScale = new Vector3(sizePrefab, sizePrefab, sizePrefab);
                                    prefabSpawned.transform.localRotation = Quaternion.Euler(0, 180, 0);
                                }
                            }
                        }
                        numCell++;
                        Triangle(parentCell);
                        xPos += sizeCell / 2 + gutterGrid;
                    }
                    if (triangleUp)
                    {
                        triangleUp = false;
                    }
                    else
                    {
                        triangleUp = true;
                    }
                    xPos = 0f;
                    zPos += sizeCell + gutterGrid;
                }
                xPos = 0f;
                zPos = 0f;
                numCell = 0;
                numPrefab = 0;
                cellAmount = 0;
                currentRandomCell = 0;
                for (int i = 0; i < cellAmount; i++)
                {
                    cellPrefab[i] = false;
                }
                triangleUp = true;
            }
            // Hexagon selected
            if ((int)shapeSelected == 2)
            {
                GameObject parentCell = new GameObject("Grid");
                for (int j = 0; j < rowsGrid; j++)
                {
                    if (rowEven)
                    {
                        xPos = 0f;
                        zPos += sizeCell - 0.5f + gutterGrid;
                        rowEven = false;
                    }
                    else
                    {
                        xPos = gutterGrid / 2 + sizeCell / 2;
                        zPos += sizeCell - 0.5f + gutterGrid;
                        rowEven = true;
                    }
                    for (int i = 0; i < columnsGrid; i++)
                    {
                        if (sizePrefab != 0)
                        {
                            if (cellPrefab[numCell])
                            {
                                GameObject prefabSpawned = (GameObject)Instantiate(prefabSource, new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0.5f), Quaternion.identity);
                                numPrefab++;
                                prefabSpawned.name = "Man" + numPrefab;
                                prefabSpawned.transform.parent = parentPrefab.transform;
                                prefabSpawned.transform.localScale = new Vector3(sizePrefab, sizePrefab, sizePrefab);
                            }
                        }
                        numCell++;
                        Hexagon(parentCell);
                        xPos += sizeCell + gutterGrid;
                    }
                }
                xPos = 0f;
                zPos = 0f;
                numCell = 0;
                numPrefab = 0;
                cellAmount = 0;
                currentRandomCell = 0;
                for (int i = 0; i < cellAmount; i++)
                {
                    cellPrefab[i] = false;
                }
            }
        }
    }

    // Creates a square mesh with a thickness (also considered a cube)
    void Square(GameObject parent)
    {
        // Creating arrays for mesh information
        Vector3[] vertices = {
            // Bottom vertices
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0f), //0
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0f), //1
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 1f), //2
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 1f), //3

            // Back vertices
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 1f), //4
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 1f), //5
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 1f), //6
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 1f), //7

            // Right vertices
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 1f), //8
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0f), //9
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0f), //10
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 1f), //11

            // Left vertices
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0f), //12
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 1f), //13
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 1f), //14
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0f), //15

            // Top vertices
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0f), //16
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0f), //17
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 1f), //18
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 1f), //19

            // Front vertices
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0f), //20
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0f), //21
            new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0f), //22
            new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0f), //23
        };

        int[] triangles = {
            // Bottom triangles
            2, 1, 0,
            3, 2, 0,
            // Back triangles
            6, 5, 4,
            7, 6, 4,
            // Right triangles
            10, 9, 8,
            11, 10, 8,
            // Left triangles
            14, 13, 12,
            15, 14, 12,
            // Top triangles
            18, 17, 16,
            19, 18, 16,
            // Front triangles
            22, 21, 20,
            23, 22, 20,
        };

        // Creating mesh
        Mesh cell = new Mesh();

        // Appointing mesh information to mesh
        cell.vertices = vertices;
        cell.triangles = triangles;
        cell.RecalculateNormals();
        cell.name = baseNameCell;

        // Creating game object
        GameObject cellObject = new GameObject(baseNameCell + numCell, typeof(MeshFilter), typeof(MeshRenderer));

        cellObject.GetComponent<MeshFilter>().mesh = cell;

        // Checks which shader is selected, and then creates a material
        if ((int)matShaderSelected == 0)
        {
            cellObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        }
        if ((int)matShaderSelected == 1)
        {
            cellObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("HDRP/Lit"));
        }

        cellObject.transform.parent = parent.transform;
    }

    // Creates a triangle mesh with a thickness
    void Triangle(GameObject parent)
    {
        // Checking which way the triangle is facing
        if (triangleUp)
        {
            // Creating arrays for mesh information
            Vector3[] vertices = {
                // Bottom vertices
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0f), //0
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0f), //1
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 1f), //2

                // Right vertices
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 1f), //3
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0f), //4
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0f), //5
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 1f), //6

                // Left vertices
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0f), //7
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 1f), //8
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 1f), //9
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0f), //10

                // Top vertices
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0f), //11
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0f), //12
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 1f), //13

                // Front vertices
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0f), //14
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0f), //15
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0f), //16
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0f), //17
            };

            int[] triangles = {
            // Bottom triangles
            2, 1, 0,
            // Right triangles
            5, 4, 3,
            6, 5, 3,
            // Left triangles
            9, 8, 7,
            10, 9, 7,
            // Top triangles
            13, 12, 11,
            // Front triangles
            16, 15, 14,
            17, 16, 14,
            };

            // Creating mesh
            Mesh cell = new Mesh();

            // Appointing mesh information to mesh
            cell.vertices = vertices;
            cell.triangles = triangles;
            cell.RecalculateNormals();
            cell.name = baseNameCell;

            // Creating game object
            GameObject cellObject = new GameObject(baseNameCell + numCell, typeof(MeshFilter), typeof(MeshRenderer));

            cellObject.GetComponent<MeshFilter>().mesh = cell;

            // Checks which shader is selected, and then creates a material
            if ((int)matShaderSelected == 0)
            {
                cellObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
            }
            if ((int)matShaderSelected == 1)
            {
                cellObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("HDRP/Lit"));
            }

            cellObject.transform.parent = parent.transform;

            triangleUp = false;
        }
        else
        {
            // Creating arrays for mesh information
            Vector3[] vertices = {
                // Bottom vertices
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 0f), //0
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 1f), //1
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 1f), //2

                // Back vertices
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 1f), //3
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 1f), //4
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 1f), //5
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 1f), //6

                // Right vertices
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 1f), //7
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 0f), //8
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0f), //9
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 1f), //10

                // Left vertices
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 0f), //11
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 1f), //12
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 1f), //13
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0f), //14

                // Top vertices
                new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0f), //15
                new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 1f), //16
                new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 1f), //17
            };

            int[] triangles = {
            // Bottom triangles
            2, 1, 0,
            // Back triangles
            5, 4, 3,
            6, 4, 5,
            // Right triangles
            9, 8, 7,
            10, 9, 7,
            // Left triangles
            13, 12, 11,
            14, 13, 11,
            // Top triangles
            17, 16, 15,
            };

            // Creating mesh
            Mesh cell = new Mesh();

            // Appointing mesh information to mesh
            cell.vertices = vertices;
            cell.triangles = triangles;
            cell.RecalculateNormals();
            cell.name = baseNameCell;

            // Creating game object
            GameObject cellObject = new GameObject(baseNameCell + numCell, typeof(MeshFilter), typeof(MeshRenderer));

            cellObject.GetComponent<MeshFilter>().mesh = cell;

            // Checks which shader is selected, and then creates a material
            if ((int)matShaderSelected == 0)
            {
                cellObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
            }
            if ((int)matShaderSelected == 1)
            {
                cellObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("HDRP/Lit"));
            }

            cellObject.transform.parent = parent.transform;

            triangleUp = true;
        }
    }

    // Creates a hexagon mesh with a thickness
    void Hexagon(GameObject parent)
    {
        // Creating arrays for mesh information
        Vector3[] vertices = {
        // Bottom vertices
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 1f), //0
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0.75f), //1
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0.25f), //2
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 0f), //3
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0.25f), //4
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0.75f), //5
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 0.5f), //6
        
        // Back right vertices
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 1f), //7
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0.75f), //8
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0.75f), //9
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 1f), //10

        // Right vertices
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0.75f), //11
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0.25f), //12
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0.25f), //13
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0.75f), //14

        // Front right vertices
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 0f, zPos + sizeCell * 0.25f), //15
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 0f), //16
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0f), //17
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0.25f), //18

        // Front left vertices
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 0f), //19
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0.25f), //20
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0.25f), //21
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0f), //22

        // Left vertices
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0.25f), //23
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0.75f), //24
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0.75f), //25
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0.25f), //26

        // Back left vertices
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 0f, zPos + sizeCell * 0.75f), //27
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 0f, zPos + sizeCell * 1f), //28
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 1f), //29
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0.75f), //30

        // Bottom vertices
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 1f), //31
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0.75f), //32
        new Vector3(xPos + sizeCell * 1f, thicknessCell * 1f, zPos + sizeCell * 0.25f), //33
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0f), //34
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0.25f), //35
        new Vector3(xPos + sizeCell * 0f, thicknessCell * 1f, zPos + sizeCell * 0.75f), //36
        new Vector3(xPos + sizeCell * 0.5f, thicknessCell * 1f, zPos + sizeCell * 0.5f), //37
        };

        int[] triangles = {
            // Bottom triangles
            1, 0, 6,
            2, 1, 6,
            3, 2, 6,
            4, 3, 6,
            5, 4, 6,
            0, 5, 6,
            // Back right triangles
            7, 9, 8,
            7, 10, 9,
            // Right triangles
            11, 13, 12,
            11, 14, 13,
            // Front right triangles
            15, 17, 16,
            15, 18, 17,
            // Front left triangles
            19, 21, 20,
            19, 22, 21,
            // Left triangles
            23, 25, 24,
            23, 26, 25,
            // Back left triangles
            27, 29, 28,
            27, 30, 29,
            // Top triangles
            37, 31, 32,
            37, 32, 33,
            37, 33, 34,
            37, 34, 35,
            37, 35, 36,
            37, 36, 31,
        };

        // Creating mesh
        Mesh cell = new Mesh();

        // Appointing mesh information to mesh
        cell.vertices = vertices;
        cell.triangles = triangles;
        cell.RecalculateNormals();
        cell.name = baseNameCell;

        // Creating game object
        GameObject cellObject = new GameObject(baseNameCell + numCell, typeof(MeshFilter), typeof(MeshRenderer));

        cellObject.GetComponent<MeshFilter>().mesh = cell;

        // Checks which shader is selected, and then creates a material
        if ((int)matShaderSelected == 0)
        {
            cellObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        }
        if ((int)matShaderSelected == 1)
        {
            cellObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("HDRP/Lit"));
        }

        cellObject.transform.parent = parent.transform;
    }

    void RandomPick()
    {
        for (int i = 0; i < columnsGrid * rowsGrid; i++)
        {
            if (currentRandomCell < cellAmount)
            {
                placePrefab = Mathf.Round(Random.value * ((float)columnsGrid * (float)rowsGrid));
                if (placePrefab > columnsGrid * rowsGrid - 1)
                {
                    placePrefab = columnsGrid * rowsGrid - 1;
                }
                if (!cellPrefab[(int)placePrefab])
                {
                    cellPrefab[(int)placePrefab] = true;
                    currentRandomCell++;
                }
                else
                {
                    i--;
                }
            }
        }
    }
}
