using MeshProcess;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.Environment
{
    [RequireComponent(typeof(VHACD))]
    public class EditorTimeMeshGenerator : MonoBehaviour
    {
        [SerializeField] private bool m_generateWithColliders = true;
        public void GenerateMeshes()
        {
            //Makes a new gObj, and loads the generated meshes into that
            GameObject parent = new();
            parent.name = gameObject.name + " convexed";
            List<Mesh> meshList = GetComponent<VHACD>().GenerateConvexMeshes();
            for (int i = 0; i < meshList.Count; i++)
            {
                GameObject newObj = new();
                newObj.name = $"{gameObject.name} part {i}";
                newObj.AddComponent<MeshRenderer>();
                newObj.AddComponent<MeshFilter>().mesh = meshList[i];
                if (m_generateWithColliders)
                    newObj.AddComponent<MeshCollider>().convex = true;

                newObj.transform.SetParent(parent.transform);
            }
        }
    }
}