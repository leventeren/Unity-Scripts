using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BambamGames
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        public bool _gameStart = false;

        public MeshFilter MF;
        public Vector3[] AllVertices;
        public GameObject p;
        private VerticesSet nearestVS;
        public class VerticesSet
        {
            public Vector3 vec;
            public List<int> nums;
    }
        private List<VerticesSet> VSList;

        public static GameManager instance { get; private set; }

        private void Awake()
        {
#if UNITY_IOS
    Application.targetFrameRate = 60;
#endif
            if (instance == null)
            {
                instance = this;
            }
        }

        void Start()
        {
            AllVertices = MF.mesh.vertices;
            VSList = new List<VerticesSet>();
            VSListSet();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                nearestVS = SelectNearestVertice();
            }

            if (Input.GetMouseButton(0))
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(mouseRay, out hit))
                {
                    Vector3 hitPoint = hit.point;// Intersection coordinates of black wall and mouseRay
                    nearestVS.vec = hitPoint;
                    for (int x = 0; x < nearestVS.nums.Count; x++)
                {
                        AllVertices[nearestVS.nums[x]] = hitPoint;
                    }
                    MF.mesh.vertices = AllVertices;
                    p.transform.position = hitPoint;
                    Debug.Log(hitPoint);
                    Debug.Log(nearestVS.vec);
                    Debug.Log(AllVertices[nearestVS.nums[0]]);
                }
            }
        }
        private void VSListSet() // Create VerticesSet for all vertices and store in VSList. There seems to be no problem here.
        {
            List <int> counter = new List<int>();
            for (int x = 0; x < AllVertices.Length; x++) { counter.Add(x); }
            for (int x = 0; 0 < counter.Count; x = counter[0])
        {
                List <int> c = new List<int>();
                var vs = new VerticesSet
                {
                    vec = AllVertices[x],
                    nums = new List<int>()
            };
            for (int y = 0; y < counter.Count; y++)
            {
                if (AllVertices[x] == AllVertices[counter[y]])
                {
                    c.Add(counter[y]);
                    vs.nums.Add(counter[y]);
                }
            }
            for (int z = 0; z < c.Count; z++) { counter.Remove(c[z]); }
            VSList.Add(vs);
            if (counter.Count == 0) { break; }
        }
    }
    private VerticesSet SelectNearestVertice() // Output VerticesSet for the closest vertex to the touched coordinates. There seems to be no problem here either.
    {
        Vector2 MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector3 VecToWP = Camera.main.WorldToScreenPoint(VSList[0].vec);
        Vector2 v = new Vector2(VecToWP.x, VecToWP.y);
        float distance = (MousePos - v).sqrMagnitude;
        int whichVS = 0;
        for (int x = 0; x < VSList.Count; x++)
        {
            Vector3 v3 = Camera.main.WorldToScreenPoint(VSList[x].vec);
            Vector2 v2 = new Vector2(v3.x, v3.y);
            var d = (MousePos - v2).sqrMagnitude;
            if (distance >= d)
            {
                distance = d;
                whichVS = x;
            }
        }
        return VSList[whichVS];
    }
}
}
