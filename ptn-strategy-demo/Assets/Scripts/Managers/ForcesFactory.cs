using System;
using System.Collections;
using System.Collections.Generic;
using Units.Base;
using UnityEngine;

public class ForcesFactory : Singleton<ForcesFactory>
{
    public Action<BaseForces, Vector3,Node> OnSpawnForces;


    private void OnEnable()
    {
        OnSpawnForces += SpawnForce;
    }

    private void OnDisable()
    {
        OnSpawnForces -= SpawnForce;
    }

    private void SpawnForce(BaseForces baseForce, Vector3 position, Node targetNode)
    {
        if (GridSystem.Instance.TryGetNearestWalkableNode(position, out Node node))
        {
            var spawnedForce = Instantiate(baseForce, node.PivotWorldPosition, Quaternion.identity);
            
            spawnedForce.SetDestination(targetNode);
            
        }
    }
}