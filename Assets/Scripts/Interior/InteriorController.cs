using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteriorController : MonoBehaviour
{
    //处理场景的动态烘焙
    NavMeshSurface navMeshSurface;

    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void BulidNavMesh()
    {
        navMeshSurface.RemoveData();
        navMeshSurface.BuildNavMesh();
    }
}
