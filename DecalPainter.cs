// Author: Damien Mayance (http://dmayance.com)
// 2013 - Pixelnest Studio (http://pixelnest.io)
// 
// This script paints decals on surfaces it hits from a point.
// See http://dmayance.com/2013-10-09-unity-paint-part-2/ for further explanations.
//
// Usage: 
// - Attach it to an unique object that won't be deleted of your choice. There should be only one instance of this script.
// - Then fill the "PaintDecalPrefabs" list with your decals prefabs.
// - Finally, just call DecalPainter.Instance.Paint from another script to paint!
//
// It included a debug mode where you can click in the scene to paint. Everything between UNITY_EDITOR is just debug stuff that can be removed.
//
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generate paint decals
/// </summary>
public class DecalPainter : MonoBehaviour
{
    public static DecalPainter Instance;

    /// <summary>
    /// Paint decals to reproduce on textures
    /// </summary>
    public List<Transform> PaintDecalPrefabs;

    /// <summary>
    /// Parent to affect for scene management
    /// </summary>
    public Transform DecalsParent;

    /// <summary>
    /// Minimal scale of a prefab
    /// </summary>
    public float MinScale = 0.75f;

    /// <summary>
    /// Maximal scale of a prefab
    /// </summary>
    public float MaxScale = 3f;

    /// <summary>
    /// Range of the splash raycast
    /// </summary>
    public float SplashRange = 1.5f;

    /// <summary>
    /// Number of decals
    /// </summary>
    public int PoolSize = 300;

    private Transform[] paintDecals;
    private int currentPoolIndex;
    private List<Material> materials;


#if UNITY_EDITOR
    private bool mDrawDebug;
    private Vector3 mHitPoint;
    private List<Ray> mRaysDebug = new List<Ray>();
#endif

    void Awake()
    {
        materials = new List<Material>();

        if (Instance != null) Debug.LogError("More than one Painter has been instanciated in this scene!");
        Instance = this;

        if (PaintDecalPrefabs.Count == 0) Debug.LogError("Missing Paint decals prefabs!");

        paintDecals = new Transform[PoolSize];
        currentPoolIndex = 0;
    }

    void Update()
    {
#if UNITY_EDITOR
        // Check for a click
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Paint!
                Color color = Color.red;
                Paint(hit.point + hit.normal * 1f, color, 10); // Step back a little
            }
        }
#endif
    }

    public void Paint(Vector3 location, Color color, int drops, float scaleBonus = 1f)
    {

#if UNITY_EDITOR
        mHitPoint = location;
        mRaysDebug.Clear();
        mDrawDebug = true;
#endif
        
        RaycastHit hit;

        // Generate multiple decals in once
        int n = 0;
        while(n < drops)
        {
            var dir = transform.TransformDirection(Random.onUnitSphere * SplashRange);

            // Avoid raycast backward as we're in a 2D space
            if (dir.z < 0) dir.z = Random.Range(0f, 1f);

            // Raycast around the position to splash everwhere we can
            if (Physics.Raycast(location, dir, out hit, SplashRange))
            {
                PaintDecal(hit, color, scaleBonus);

#if UNITY_EDITOR
                mRaysDebug.Add(new Ray(location, dir));
#endif

                n++;
            }
        }
    }

    private void PaintDecal(RaycastHit hit, Color color, float scaleBonus)
    {
        // Create a splash if we found a surface
        int randomIndex = Random.Range(0, PaintDecalPrefabs.Count);
        Transform paintDecal = PaintDecalPrefabs[randomIndex];

        var paintSplatter = GameObject.Instantiate(paintDecal,
                                                   hit.point,
            // Rotation from the original sprite to the normal
            // Prefab are currently oriented to z+ so we use the opposite
                                                   Quaternion.FromToRotation(Vector3.back, hit.normal)
                                                   ) as Transform;

        // Find an existing material to enable batching
        var sharedMat = materials.Where(m => m.name.Equals(paintSplatter.renderer.material.name)
                                            && m.color.Equals(color)
                                        ).FirstOrDefault();

        // New material
        if (sharedMat == null)
        {
            Material mat = paintSplatter.renderer.material;
            mat.color = color;

            materials.Add(mat);
        }
        // Old one
        else
        {
            paintSplatter.renderer.sharedMaterial = sharedMat;
        }

        // Random scale
        var scaler = Random.Range(MinScale, MaxScale) * scaleBonus;

        paintSplatter.localScale = new Vector3(
            paintSplatter.localScale.x * scaler,
            paintSplatter.localScale.y * scaler,
            paintSplatter.localScale.z
        );

        // Random rotation effect
        var rater = Random.Range(0, 359);
        paintSplatter.transform.RotateAround(hit.point, hit.normal, rater);

        paintSplatter.parent = DecalsParent;

        // Pool
        if (paintDecals[currentPoolIndex] != null)
        {
            Destroy(paintDecals[currentPoolIndex].gameObject);
            paintDecals[currentPoolIndex] = null;
        }

        paintDecals[currentPoolIndex] = paintSplatter;
        currentPoolIndex++;

        if (currentPoolIndex >= PoolSize) currentPoolIndex = 0;
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (mDrawDebug)
        {
            Gizmos.DrawSphere(mHitPoint, 0.2f);
            foreach (var r in mRaysDebug)
            {
                Gizmos.DrawRay(r);
            }
        }
#endif
    }
}
