using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NodeFinder : MonoBehaviour
{
    [SerializeField] float maxDistance = 4.5f;
    [SerializeField] LayerMask nodesLayer = 1<<9;
    [SerializeField] private Node currentNode;
    private PathFinder pathFinder;

    public Node closestNode
    {
        get 
        {
            if (distanceToCurrentNode > maxDistance)
            {
                Node potentialNode = findNewNode();
                if (potentialNode != null) currentNode = potentialNode;
            }
            return currentNode; 
        }
    }

    float distanceToCurrentNode
    {
        get 
        {
            if (currentNode == null) return float.MaxValue;
            return (currentNode.transform.position - transform.position).magnitude; 
        }
    }

    private Node findNewNode()
    {
        //Gizmos.DrawSphere(transform.position, maxDistance);
        return getClosestNode(Physics.OverlapSphere(transform.position, maxDistance, nodesLayer));
    }

    Node getClosestNode(Collider[] colliders)
    {
        Node bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider collider in colliders)
        {
            //if (!collider.gameObject.CompareTag("Node")) continue;

            Vector3 directionToTarget = collider.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = collider.GetComponent<Node>();
            }
        }

        return bestTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (currentNode != null) Gizmos.DrawSphere(currentNode.transform.position, 0.2f);
    }
}
