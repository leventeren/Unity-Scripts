using UnityEngine;
using System.Collections;





    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]

    public class DragShotMover : MonoBehaviour
    {

        /*
            Drag-Shot Mover v01

            Desc: Allows force to be applied to the object by clicking that body
            and dragging to designate a force vector with a maximum magnitude.

            TODO:
                + set dead zone to a percentage
                + add a "tension" option to the feel of the drag
                + fancy, non-debug line
                + add something to the disc that will make it clear how its tilted.

            Known Issues:

            Reminders / Caveats / Gotchas:
                + OnMouseDown doesn't see the target object under another collider.  This is just
                  how Unity works.

        */

        public float magBase = 2; // this is the base magnitude and the maximum length of the line drawn in the user interface
        public float magMultiplier = 5; // multiply the line length by this to allow for higher force values to be represented by shorter lines
        public Vector3 dragPlaneNormal = Vector3.up; // a vector describing the orientation of the drag plan relative to world-space but centered on the target
        public SnapDir snapDirection = SnapDir.away; // force is applied either toward or away from the mouse on release
        public ForceMode forceTypeToApply = ForceMode.VelocityChange;

        public bool overrideVelocity = true; // cancel the existing velocity before applying the new force
        public bool pauseOnDrag = true; // causes the simulation to pause when the object is clicked and unpause when released

        public Color noForceColor = Color.yellow; // color of the visualization helpers at force 0
        public Color maxForceColor = Color.red; // color of the visualization helpers at maximum force

        public enum SnapDir { toward, away }

        private Vector3 forceVector;
        private float magPercent = 0;

        private bool mouseDragging = false;
        private Vector3 mousePos3D;
        private float dragDistance;
        private Plane dragPlane;
        private Ray mouseRay;
        private GameObject dragZone;

        private string shaderString = "Transparent/Diffuse";
        private Material dzMat;

        public Rigidbody rb;

        void Start()
        {

            Color currentColor = noForceColor;
            dzMat = new Material(Shader.Find(shaderString));

            // create the dragzone visual helper
            dragZone = new GameObject("dragZone_" + gameObject.name);
            dragZone.AddComponent<MeshFilter>().mesh = MakeDiscMeshBrute(magBase / 4);
            //dragZone.GetComponent.MeshFilter.
            dragZone.AddComponent<MeshRenderer>();
            dragZone.GetComponent<Renderer>().enabled = false;

            dragZone.name = "dragZone_" + gameObject.name;
            dragZone.transform.localScale = new Vector3(magBase * 2, 0.025f, magBase * 2);
            dragZone.GetComponent<Renderer>().material = dzMat;
            dragZone.GetComponent<Renderer>().material.color = currentColor * new Color(1, 1, 1, 0.2f);

            // create the dragplane
            dragPlane = new Plane(dragPlaneNormal, transform.position);

            // orient the drag plane
            if (dragPlaneNormal != Vector3.zero)
            {
                dragZone.transform.rotation = Quaternion.LookRotation(dragPlaneNormal) * new Quaternion(1, 0, 0, 1);
            }
            else Debug.LogError("Drag plane normal cannot be equal to Vector3.zero.");

            //update the position of the dragzone
            dragZone.transform.position = transform.position;
        }

        void OnMouseDown()
        {
            mouseDragging = true;

            if (pauseOnDrag)
            {
                // pause the simulation
                Time.timeScale = 0;
            }
            // update the dragplane
            dragPlane = new Plane(dragPlaneNormal, transform.position);

            // orient the drag plane
            if (dragPlaneNormal != Vector3.zero)
            {
                dragZone.transform.rotation = Quaternion.LookRotation(dragPlaneNormal) * new Quaternion(1, 0, 0, 1);
            }
            else Debug.LogError("Drag plane normal cannot be equal to Vector3.zero.");

            //update the position of the dragzone
            dragZone.transform.position = transform.position;

            dragZone.GetComponent<Renderer>().enabled = true;
        }



        void OnMouseDrag()
        {
            Color currentColor = noForceColor;
            // update the plane if the target object has left it
            if (dragPlane.GetDistanceToPoint(transform.position) != 0)
            {
                // update dragplane by constructing a new one -- I should check this with a profiler
                dragPlane = new Plane(dragPlaneNormal, transform.position);
            }

            // create a ray from the camera, through the mouse position in 3D space
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // if mouseRay intersects with dragPlane
            float intersectDist = 0.0f;

            if (dragPlane.Raycast(mouseRay, out intersectDist))
            {
                // update the world space point for the mouse position on the dragPlane
                mousePos3D = mouseRay.GetPoint(intersectDist);

                // calculate the distance between the 3d mouse position and the object position
                dragDistance = Mathf.Clamp((mousePos3D - transform.position).magnitude, 0, magBase);

                // calculate the force vector
                if (dragDistance * magMultiplier < 1) dragDistance = 0; // this is to allow for a "no move" buffer close to the object
                forceVector = mousePos3D - transform.position;
                forceVector.Normalize();
                forceVector *= dragDistance * magMultiplier;

                // update color the color
                // calculate the percentage value of current force magnitude out of maximum
                magPercent = (dragDistance * magMultiplier) / (magBase * magMultiplier);
                // choose color based on how close magPercent is to either 0 or max
                currentColor = noForceColor * (1 - magPercent) + maxForceColor * magPercent;

                // dragzone color
                dragZone.GetComponent<Renderer>().material.color = currentColor * new Color(1, 1, 1, 0.2f);

                // draw the line
                Debug.DrawRay(transform.position, forceVector / magMultiplier, currentColor);
            }

            //update the position of the dragzone
            dragZone.transform.position = transform.position;
        }

        void OnMouseUp()
        {
            mouseDragging = false;

            if (overrideVelocity)
            {
                // cancel existing velocity
                rb.AddForce(-rb.velocity, ForceMode.VelocityChange);

            }

            // add new force
            int snapD = 1;
            if (snapDirection == SnapDir.away) snapD = -1; // if snapdirection is "away" set the force to apply in the opposite direction
            //Rigidbody.AddForce(snapD * forceVector, forceTypeToApply);
            rb.AddForce(snapD * forceVector, forceTypeToApply);
            

            // cleanup
            dragZone.GetComponent<Renderer>().enabled = false;

            if (pauseOnDrag)
            {
                // un-pause the simulation
                Time.timeScale = 1;
            }

        }

        void OnGUI()
        {
            if (mouseDragging)
            {
                Vector2 guiMouseCoord = GUIUtility.ScreenToGUIPoint(Input.mousePosition);
                GUI.Box(new Rect(guiMouseCoord.x - 30, Screen.height - guiMouseCoord.y + 15, 100, 20), "force: " + Mathf.Round((forceVector).magnitude));
            }
        }

        Mesh MakeDiscMeshBrute(float r)
        {
            Mesh discMesh;
            Vector3[] dmVerts = new Vector3[18];
            Vector3[] dmNorms = new Vector3[18];
            Vector2[] dmUVs = new Vector2[18];
            int[] dmTris = new int[48];
            int i = 0;

            discMesh = new Mesh();

            dmVerts[0] = new Vector3(0, 0, 0);
            dmVerts[1] = new Vector3(0, 0, r);
            dmVerts[2] = new Vector3(1, 0, 1).normalized * r; // find the vector at the correct distance the hacky-hillbilly way!
            dmVerts[3] = new Vector3(r, 0, 0);
            dmVerts[4] = new Vector3(1, 0, -1).normalized * r;
            dmVerts[5] = new Vector3(0, 0, -r);
            dmVerts[6] = new Vector3(-1, 0, -1).normalized * r;
            dmVerts[7] = new Vector3(-r, 0, 0);
            dmVerts[8] = new Vector3(-1, 0, 1).normalized * r;

            // set the other side to the same points
            for (i = 0; i < dmVerts.Length / 2; i++)
            {
                dmVerts[dmVerts.Length / 2 + i] = dmVerts[i];
            }
            for (i = 0; i < dmNorms.Length; i++)
            {
                if (i < dmNorms.Length / 2) dmNorms[i] = Vector3.up; // set side one to face up
                else dmNorms[i] = -Vector3.up; // set side two to face down
            }

            dmUVs[0] = new Vector2(0, 0);
            dmUVs[1] = new Vector2(0, r);
            dmUVs[2] = new Vector2(1, 1).normalized * r; ;
            dmUVs[3] = new Vector2(r, 0);
            dmUVs[4] = new Vector2(1, -1).normalized * r; ;
            dmUVs[5] = new Vector2(0, -r);
            dmUVs[6] = new Vector2(-1, -1).normalized * r; ;
            dmUVs[7] = new Vector2(-r, 0);
            dmUVs[8] = new Vector2(-1, 1).normalized * r; ;

            // set the other side to the same points
            for (i = 0; i < dmUVs.Length / 2; i++)
            {
                dmUVs[dmUVs.Length / 2 + i] = dmUVs[i];
            }

            dmTris[0] = 0;
            dmTris[1] = 1;
            dmTris[2] = 2;

            dmTris[3] = 0;
            dmTris[4] = 2;
            dmTris[5] = 3;

            dmTris[6] = 0;
            dmTris[7] = 3;
            dmTris[8] = 4;

            dmTris[9] = 0;
            dmTris[10] = 4;
            dmTris[11] = 5;

            dmTris[12] = 0;
            dmTris[13] = 5;
            dmTris[14] = 6;

            dmTris[15] = 0;
            dmTris[16] = 6;
            dmTris[17] = 7;

            dmTris[18] = 0;
            dmTris[19] = 7;
            dmTris[20] = 8;

            dmTris[21] = 0;
            dmTris[22] = 8;
            dmTris[23] = 1;

            // side two
            dmTris[24] = 9;
            dmTris[25] = 11;
            dmTris[26] = 10;

            dmTris[27] = 9;
            dmTris[28] = 12;
            dmTris[29] = 11;

            dmTris[30] = 9;
            dmTris[31] = 13;
            dmTris[32] = 12;

            dmTris[33] = 9;
            dmTris[34] = 14;
            dmTris[35] = 13;

            dmTris[36] = 9;
            dmTris[37] = 15;
            dmTris[38] = 14;

            dmTris[39] = 9;
            dmTris[40] = 16;
            dmTris[41] = 15;

            dmTris[42] = 9;
            dmTris[43] = 17;
            dmTris[44] = 16;

            dmTris[45] = 9;
            dmTris[46] = 10;
            dmTris[47] = 17;

            discMesh.vertices = dmVerts;
            discMesh.uv = dmUVs;
            discMesh.normals = dmNorms;
            discMesh.triangles = dmTris;

            return discMesh;
        }
    }
