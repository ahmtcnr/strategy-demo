using System;
using System.Collections;
using SOScripts;
using UnityEngine;
using System.Collections.Generic;

namespace Units.Base
{
    public class BaseForces : BaseUnit
    {
        Vector3[] path;
        private Vector3 targetPos;
        int targetIndex;

        public Node reservedNode;
        private Coroutine _moveRoutine;

        private bool isMoving;


        protected override void OnEnable()
        {
            base.OnEnable();

            Actions.OnBuildSuccess += RecalculatePath;
            OnSelected += ActivateMovement;
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            Actions.OnBuildSuccess -= RecalculatePath;
            OnSelected -= ActivateMovement;
        }

        public void SetDestination(Vector3 targetPosition)
        {
            //Debug.Log("Target node" + targetNode.gridIndex);
            if (GridSystem.Instance.TryGetNearestWalkableNode(targetPosition, out Node node))
            {
                PathRequestManager.RequestPath(transform.position, node.WorldPosition, OnPathFound, this);
                targetPos = targetPosition;
            }
        }

        private void ActivateMovement() => ListenDeselect();

        private void ListenDeselect()
        {
            Actions.OnUnitDeselected += DeselectUnit;
            Actions.OnRightClick += MoveToCursor;
        }

        private void DeselectUnit()
        {
            Actions.OnUnitDeselected -= DeselectUnit;
            Actions.OnRightClick -= MoveToCursor;
        }

        private void MoveToCursor()
        {
            SetDestination(GridSystem.Instance.GetNodeOnCursor().PivotWorldPosition);
        }


        private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                path = newPath;
                targetIndex = 0;

                if (path.Length == 0)
                {
                    var node = GridSystem.Instance.GetNodeFromWorldPos(transform.position);
                    node.isReserved = true;
                    node.reservedUnit = this;
                    reservedNode = node;
                    transform.position = node.PivotWorldPosition;
                }
                else
                {
                    if (_moveRoutine != null)
                    {
                        StopCoroutine(_moveRoutine);
                    }

                    _moveRoutine = StartCoroutine(FollowPath());
                }
            }
            else
            {
            }
        }

        private IEnumerator FollowPath()
        {
            // if (reservedNode != null)
            // {
            //     reservedNode.isReserved = false;
            // }

            isMoving = true;
            // capturedNode = GridSystem.Instance.GetNodeFromWorldPos(path[path.Length - 1]);
            // capturedNode.isReserved = true;
            // capturedNode.reservedUnit = this;
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        if (GridSystem.Instance.GetNodeFromWorldPos(transform.position).reservedUnit != this)
                        {
                            if (GridSystem.Instance.TryGetNearestWalkableNode(transform.position, out Node emptyNode))
                            {
                                SetDestination(emptyNode.WorldPosition);
                                isMoving = false;
                                yield break;
                            }
                        }

                        isMoving = false;
                        yield break;
                    }

                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, ((ForcesData)baseUnitData).moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void RecalculatePath(BaseBuilding bb)
        {
            if (!isMoving) return;
            SetDestination(targetPos);
        }

        // private void OnDrawGizmos()
        // {
        //     if (path != null)
        //     {
        //         for (int i = targetIndex; i < path.Length; i++)
        //         {
        //             Gizmos.color = Color.black;
        //             Gizmos.DrawCube(path[i], Vector3.one);
        //
        //             if (i == targetIndex)
        //             {
        //                 Gizmos.DrawLine(transform.position, path[i]);
        //             }
        //             else
        //             {
        //                 Gizmos.DrawLine(path[i - 1], path[i]);
        //             }
        //         }
        //     }
        // }
    }
}