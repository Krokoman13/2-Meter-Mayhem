using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] Node currentNode;
    [SerializeField] Node targetNode;

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

    [SerializeField] protected float maxSpeed = 10f;
    protected enum states { driving, arrived, waiting }
    [SerializeField] protected states state;

    public void Spawn(Node node)
    {
        if (node == null) return;
        currentNode = node;
        transform.position = node.transform.position;
    }

    protected virtual void Start()
    {
        state = states.driving;
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
            case states.driving:
                drive();
                break;

            case states.arrived:
                arrived();
                break;
        }
    }

    protected virtual void arrived()
    {
        if (targetNode != null)
        {
            currentNode = targetNode;
            transform.position = targetNode.transform.position;
            targetNode = null;
        }

        StartCoroutine(waitAtNode());
        state = states.driving;
    }

    protected virtual IEnumerator waitAtNode()
    {
        if (currentNode.waitTimeSecs > 0f)
        {
            RandomPrefab randomPrefab = GetComponent<RandomPrefab>();
            randomPrefab.randomize = true;
        }

        state = states.driving;
        yield return new WaitForSeconds(0.0f);
    }

    protected virtual void drive()
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

        transform.rotation = Quaternion.LookRotation(targetNode.transform.position - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, targetNode.transform.position, maxSpeed * Time.deltaTime);
    }

    protected Node pickNextNode()
    {
        return currentNode.connected[0];
    }
}
