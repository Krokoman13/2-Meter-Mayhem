using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathfindingNPCBehaviour : MonoBehaviour
{
    public virtual bool walking
    {
        get 
        {
            if (targetNode != null) 
                if (targetNode.occupied == this) return true;
            return false;
        }
    }

    [SerializeField] private List<Node> _nodes = new List<Node>() { null, null };
    [SerializeField] protected GameObject _body;
    protected PathFinder _pathFinder;
    protected bool path
    {
        get { return _nodes.Count > 3; }
    }
    protected Node lastNode
    {
        get { return _nodes[_nodes.Count - 1]; }
    }
    protected Node previousNode
    {
        get
        {
            //if (nodes[0] = null) nodes[0] = currentNode;
            return _nodes[0];
        }
        set
        {
            _nodes[0] = value;
        }
    }
    public Node currentNode
    {
        get 
        {
            if (_nodes.Count < 2) return null;
            return _nodes[1]; 
        }
        set
        {
            _nodes[0] = currentNode;
            if (_nodes[0] != null) _nodes[0].occupied = null;

            _nodes[1] = value;
            if (value != null) _nodes[1].occupied = this;
        }
    }
    protected virtual Vector3 targetPosition
    {
        get
        {
            return targetNode.transform.position;
        }
    }
    protected Node targetNode
    {
        get
        {
            if (_nodes.Count < 3) return null;
            return _nodes[2];
        }
        set
        {
            if (currentNode.Connected(value) || value == null)
            {
                if (_nodes.Count > 2) _nodes.RemoveRange(2, _nodes.Count - 2);
                if (value != null)
                {
                    _nodes.Add(value);
                }
            }

            SetTargetNodes(_pathFinder.Generate(currentNode, value), 1);
        }
    }
    protected float distanceToTargetNode
    {
        get
        {
            if (targetNode == null) return 0;
            Vector3 distance = targetNode.transform.position - transform.position;
            distance.y = 0;

            return distance.magnitude;
        }
    }
    
    [SerializeField] protected float maxSpeed = 1f;
    protected enum states { walking, arrived, waiting }
    [SerializeField] protected states state;

    public void Spawn(Node node)
    {
        if (node == null) return;
        //Debug.Log("Spawning!");
        _nodes = new List<Node>() {null, node};
        currentNode = node;
        transform.position = node.transform.position;
    }

    protected virtual void Start()
    {
        _nodes = new List<Node>() { null, null };
        _pathFinder = gameObject.AddComponent<PathFinder>();
        if (_body == null) _body = transform.GetChild(0).gameObject;
        state = states.walking;
    }

    private void Update()
    {
        if (currentNode == null)
        {
            NodeFinder nodeFinder = gameObject.AddComponent<NodeFinder>();
            Spawn(nodeFinder.closestNode);
            Destroy(nodeFinder);
            return;
        }

        switch (state)
        {
            case states.walking:
                walk();
                break;

            case states.arrived:
                arrived();
                break;

            case states.waiting:
                wait();
                break;
        }
    }

    protected virtual void arrived()
    {
        if (targetNode != null)
        {
            currentNode = targetNode;
            _nodes.Remove(targetNode);
        }
    }

    protected virtual void wait()
    {
        //
    }

    protected virtual void walk()
    {
        if (targetNode == null)
        {
            targetNode = pickNextNode();
        }

        if (distanceToTargetNode < 0.1f)
        {
            state = states.arrived;
            return;
        }

        if (targetNode.occupied == null)
        {
            targetNode.occupied = this;
        }

        if (targetNode.occupied == this)
        {
            _body.transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.position = Vector3.MoveTowards(transform.position, targetNode.transform.position, maxSpeed * Time.deltaTime);
            return;
        }

        regeneratePath();
    }

    protected virtual IEnumerator waitAtNode()
    {
        if (currentNode.waitTimeSecs > 0f)
        {
            state = states.waiting;
            yield return new WaitForSeconds(currentNode.waitTimeSecs);
        }

        state = states.walking;
        yield return new WaitForSeconds(0.0f);
    }

    protected abstract Node pickNextNode();

    private void SetTargetNodes(List<Node> pNodes, int removeAmount = 0)
    {
        if (pNodes == null) return;
        if (pNodes.Count - removeAmount < 1) return;

        List<Node> toAdd = new List<Node>(pNodes);
        toAdd.RemoveRange(0, removeAmount);

        if (_nodes.Count > 2) _nodes.RemoveRange(2, _nodes.Count - 2);

        foreach (Node node in toAdd)
        {
            _nodes.Add(node);
        }
    }

    protected virtual void regeneratePath()
    {
        SetTargetNodes(_pathFinder.Generate(currentNode, lastNode), 1);
    }
}
