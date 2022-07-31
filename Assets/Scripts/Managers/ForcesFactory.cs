using System;
using System.Collections;
using System.Collections.Generic;
using Units.Base;
using UnityEngine;
using Random = UnityEngine.Random;

public class ForcesFactory : Singleton<ForcesFactory>
{
    public Action<BaseForces, Vector3, Vector3> OnSpawnForces;
    private void OnEnable()
    {
        OnSpawnForces += SpawnForce;
    }

    private void OnDisable()
    {
        OnSpawnForces -= SpawnForce;
    }
    private void SpawnForce(BaseForces baseForce, Vector3 spawnPosition, Vector3 targetPos)
    {
        if (GridSystem.Instance.TryGetNearestWalkableNode(spawnPosition, out Node node))
        {
            var spawnedForce = Instantiate(baseForce, node.WorldPosition, Quaternion.identity);
            
            spawnedForce.SetDestination(targetPos);
        }
        #region Test
        // StartCoroutine(StressTest());
        //
        // IEnumerator StressTest()
        // {
        //     while (true)
        //     {
        //         if (GridSystem.Instance.TryGetNearestWalkableNode(spawnPosition, out Node node))
        //         {
        //             var spawnedForce = Instantiate(baseForce, node.PivotWorldPosition, Quaternion.identity);
        //
        //             spawnedForce.SetDestination(targetPos);
        //         }
        //         else
        //         {
        //             yield break;
        //         }
        //
        //         yield return new WaitForSeconds(0.1f);
        //     }
        // }
        

        #endregion
    }
}