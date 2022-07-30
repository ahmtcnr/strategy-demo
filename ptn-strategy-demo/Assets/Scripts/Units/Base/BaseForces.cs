using System.Collections;
using SOScripts;
using UnityEngine;
using System.Collections.Generic;

namespace Units.Base
{
    public class BaseForces : BaseUnit
    {
        float speed = 20;
        Vector3[] path;
        private Vector3 targetPos;
        int targetIndex;

        public void SetDestination(Vector3 targetPosition)
        {
            StopCoroutine(FollowPath());
            //Debug.Log("Target node" + targetNode.gridIndex);
            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
            targetPos = targetPosition;
        }


        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {

            if (pathSuccessful)
            {
                path = newPath;
                targetIndex = 0;
                StopCoroutine(FollowPath());
                StartCoroutine(FollowPath());
            }
            else
            {
                // if (GridSystem.Instance.TryGetNearestWalkableNode(targetPos, out Node emptyNode))
                // {
                //     Debug.Log(":::"+emptyNode.gridIndex);
                //     SetDestination(emptyNode.WorldPosition);
                //     //this.targetNode = emptyNode;
                // }
                // else
                // {
                //     Debug.Log("whaaaa");
                // }
            }
        }

        IEnumerator FollowPath()
        {
            if (path.Length == 0) yield break;

            var node = GridSystem.Instance.GetNodeFromWorldPos(path[path.Length - 1]);
            node.isReserved = true;
            node.reservedUnit = this;
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        // GridSystem.Instance.GetNodeFromWorldPos(transform.position) = true;
                        // if (!GridSystem.Instance.GetNodeFromWorldPos(transform.position).isWalkable)
                        // {
                        //     if (GridSystem.Instance.TryGetNearestWalkableNode(transform.position,out Node node))
                        //     {
                        //         SetDestination(node);
                        //         break;
                        //     }
                        //     
                        // }
                        if (GridSystem.Instance.GetNodeFromWorldPos(transform.position).reservedUnit == this)
                        {
                            GridSystem.Instance.GetNodeFromWorldPos(transform.position).isReserved = true;
                        }
                        else
                        {
                            if (GridSystem.Instance.TryGetNearestWalkableNode(transform.position, out Node emptyNode))
                            {
                                SetDestination(emptyNode.WorldPosition);
                                //this.targetNode = emptyNode;
                                yield break;
                            }
                        }


                        yield break;
                    }

                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}