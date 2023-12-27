using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingShopper : PathfindingNPCBehaviour
{
    NodeGraph _nodeGraph;
    [SerializeField] Transform cart;
    [SerializeField] Transform walkingSpot;
    [SerializeField] Transform waitingSpot;

    protected override void walk()
    {
        base.walk();

        cart.position = walkingSpot.position;
    }

    protected override void wait()
    {
        _body.transform.rotation = currentNode.transform.rotation;
        cart.position = waitingSpot.position;
    }

    protected override void arrived()
    {
        base.arrived();

        if (path)
        {
            state = states.walking;
            return;
        }

        StartCoroutine(waitAtNode());
    }

    protected override Node pickNextNode()
    {
        if (path) return lastNode;
        if (_nodeGraph == null) _nodeGraph = currentNode.transform.parent.GetComponent<NodeGraph>();
        Node toReturn = _nodeGraph.waitNodes[Random.Range(0, _nodeGraph.waitNodes.Count)];
        return toReturn;
    }
}
