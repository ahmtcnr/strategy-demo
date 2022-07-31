using System;
using System.Collections;
using SOScripts;
using UnityEngine;
using System.Collections.Generic;

namespace Units.Base
{
    public class BaseForces : BaseUnit
    {
        public Node ReservedNode;

        private Vector3[] _path;
        private Vector3 _targetPos;
        private int _targetIndex;
        private Coroutine _moveRoutine;
        private bool _isMoving;

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
                _path = newPath;
                _targetIndex = 0;

                if (_path.Length == 0)
                {
                    EmptyCurrentReservedNode();
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
                EmptyCurrentReservedNode();
            }
        }

        private IEnumerator FollowPath()
        {
            _isMoving = true;
            Vector3 currentWaypoint = _path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    _targetIndex++;
                    if (_targetIndex >= _path.Length) // if the last node of the path is reserved, go to the nearest unreserved node 
                    {
                        if (GridSystem.Instance.GetNodeFromWorldPos(transform.position).ReservedUnit != this)
                        {
                            if (GridSystem.Instance.TryGetNearestWalkableNode(transform.position, out Node emptyNode))
                            {
                                SetDestination(emptyNode.WorldPosition);
                                _isMoving = false;
                                yield break;
                            }
                        }

                        _isMoving = false;
                        yield break;
                    }

                    currentWaypoint = _path[_targetIndex];
                }


                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, ((ForcesData)baseUnitData).MoveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void RecalculatePath(BaseBuilding bb)
        {
            if (!_isMoving) return;
            SetDestination(_targetPos);
        }


        private void EmptyCurrentReservedNode()
        {
            if (ReservedNode != null)
            {
                ReservedNode.IsReserved = false;
                ReservedNode.ReservedUnit = null;
            }

            ReservedNode = GridSystem.Instance.GetNodeFromWorldPos(transform.position);
            ReservedNode.IsReserved = true;
            ReservedNode.ReservedUnit = this;
            transform.position = ReservedNode.PivotWorldPosition;
        }

        public void SetDestination(Vector3 targetPosition)
        {
            if (GridSystem.Instance.TryGetNearestWalkableNode(targetPosition, out Node node))
            {
                PathRequestManager.RequestPath(transform.position, node.WorldPosition, OnPathFound, this);
                _targetPos = targetPosition;
            }
        }

        #region Gizmos

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

        #endregion
    }
}