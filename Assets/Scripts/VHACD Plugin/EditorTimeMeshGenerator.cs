using MeshProcess;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ILOVEYOU.Environment
{
    [RequireComponent(typeof(VHACD))]
    public class EditorTimeMeshGenerator : MonoBehaviour
    {
        [SerializeField] private bool m_generateWithColliders = true;
        [SerializeField] private Material m_defaultMaterial;
        public void GenerateMeshes()
        {
            //Makes a new gObj, and loads the generated meshes into that
            GameObject parent = new();
            parent.name = gameObject.name + " convexed";

            parent.transform.position = Vector3.zero;
            parent.transform.rotation = Quaternion.identity;
            parent.transform.localScale = Vector3.one;

            //Figure out the file path
            string filePath = SceneManager.GetActiveScene().path;
            filePath = filePath.Remove(filePath.Length - ".unity".Length, ".unity".Length);

            //Check if the path exists
            if (!Directory.Exists($"{filePath}/Meshes/"))
            {
                AssetDatabase.CreateFolder(filePath, "Meshes");
            }
            filePath += "/Meshes/";

            //Generate all the meshes
            List<Mesh> meshList = GetComponent<VHACD>().GenerateConvexMeshes();
            for (int i = 0; i < meshList.Count; i++)
            {
                //Name the mesh we're working with
                meshList[i].name = $"{gameObject.name} part {i} mesh";
                //Make new object with name
                GameObject newObj = new();
                newObj.name = $"{gameObject.name} part {i}";

                //Add the renderer
                MeshRenderer rend = newObj.AddComponent<MeshRenderer>();
                //Check for and apply the default material
                if (m_defaultMaterial)
                    rend.material = m_defaultMaterial;
                //Update the mesh
                newObj.AddComponent<MeshFilter>().mesh = meshList[i];
                //Set up the collider
                if (m_generateWithColliders)
                    newObj.AddComponent<MeshCollider>().convex = true;

                //Set the object parent
                newObj.transform.SetParent(parent.transform);
                AssetDatabase.CreateAsset(meshList[i], $"{filePath}{meshList[i]}.asset");
                parent.transform.SetParent(transform);
            }
        }
    }
}