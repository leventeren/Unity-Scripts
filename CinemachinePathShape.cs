using UnityEngine;
using Cinemachine;
using Waypoint = Cinemachine.CinemachineSmoothPath.Waypoint;

public class CinemachinePathShape : MonoBehaviour
{
    public CinemachineSmoothPath path;

    [Space]

    [Range(2, 32)]
    public int points = 16;
    [Range(1f, 50f)]
    public float radius = 5f;
    [Range(0f, 360f)]
    public float startAngle = 0;
    [Range(15f, 360f)]
    public float endAngle = 360f;
    [Range(0f, 30f)]
    public float elevation = 0f;

    private Waypoint[] m_Waypoints = new Waypoint[0];

    private void OnValidate()
    {
        if (!path) path = this.GetComponent<CinemachineSmoothPath>();
        if (!path) return;

        Apply();
    }

    public void Apply()
    {
        m_Waypoints = new Waypoint[points];

        float angle = startAngle;
        float length = endAngle - startAngle;
        for (int i = 0; i < points; i++)
        {
            float t = (float)i / (float)points;

            m_Waypoints[i].position = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * angle) * (radius * 0.5f),
                t * (elevation * 0.5f),
                Mathf.Cos(Mathf.Deg2Rad * angle) * (radius * 0.5f));

            angle += (length / (float)points);
        }

        path.m_Waypoints = m_Waypoints;
        path.InvalidateDistanceCache();
    }
}
