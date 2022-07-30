using System;
using System.Collections;
using System.Collections.Generic;
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

    public static void RequestPath(Vector3 pathStart, Node targetNode, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, targetNode, callback);
        Instance.pathRequestQueue.Enqueue(newRequest);
        Instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.targetNode);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Node targetNode;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Node targetNode, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            this.targetNode = targetNode;
            callback = _callback;
            
        }
    }
}