using System.Collections;
using SOScripts;
using UnityEngine;
using System.Collections.Generic;
namespace Units.Base
{
    public class BaseForces: BaseUnit
    {
        float speed = 20;
        Vector3[] path;
        private Node targetNode;
        int targetIndex;
        
        public void SetDestination(Node targetNode)
        {
            PathRequestManager.RequestPath(transform.position,targetNode, OnPathFound);
            this.targetNode = targetNode;
        }


        public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
            if (pathSuccessful) {
                path = newPath;
                targetIndex = 0;
                
                targetNode.isReserved = true;
                targetNode.reservedUnit = this;
                
                StopCoroutine(FollowPath());
                StartCoroutine(FollowPath());
            }
        }

        IEnumerator FollowPath() {
            
            Vector3 currentWaypoint = path[0];
            while (true) {
                if (transform.position == currentWaypoint) {
                    targetIndex ++;
                    if (targetIndex >= path.Length)
                    {
                       // GridSystem.Instance.GetNodeFromWorldPos(transform.position) = true;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
                yield return null;

            }
            
            
        }

        public void OnDrawGizmos() {
            if (path != null) {
                for (int i = targetIndex; i < path.Length; i ++) {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex) {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else {
                        Gizmos.DrawLine(path[i-1],path[i]);
                    }
                }
            }
        }
    }
}