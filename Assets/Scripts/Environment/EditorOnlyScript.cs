using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorOnlyScript : MonoBehaviour
{
    private void Start()
    {
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<MeshFilter>());
        Destroy(this);
    }
}
