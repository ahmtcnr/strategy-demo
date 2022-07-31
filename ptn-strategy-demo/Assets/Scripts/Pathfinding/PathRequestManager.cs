using System;
using System.Collections;
using System.Collections.Generic;
using Units.Base;
using UnityEngine;

public class PathRequestManager : Singleton<PathRequestManager>
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    Pathfinding pathfinding;

    bool isProcessingPath;

    protected override void Awake()
    {
        base.Awake();

        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback, BaseForces baseForces)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback, baseForces);
        Instance.pathRequestQueue.Enqueue(newRequest);
        Instance.TryProcessNext();
    }


    private void Update()
    {
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        if (path.Length != 0)
        {
            if (currentPathRequest.baseForces.reservedNode != null)
            {
                currentPathRequest.baseForces.reservedNode.isReserved = false;
            }
            var reservedNode = GridSystem.Instance.GetNodeFromWorldPos(path[path.Length - 1]);
            currentPathRequest.baseForces.reservedNode = reservedNode;
            reservedNode.reservedUnit = currentPathRequest.baseForces;
            reservedNode.isReserved = true;
        }

       

        TryProcessNext();
    }


    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;
        public BaseForces baseForces;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback, BaseForces _baseForces)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
            baseForces = _baseForces;
        }
    }
}