using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : PathfindingNPCBehaviour
{
    [SerializeField] private NodeFinder playerNodeFinder;
    public GameObject player;
    public float maxRangeToPlayer;
    public float minRangeToPlayer;
    public override bool walking { get { return true; } }

    float distToPlayer { get { return (player.transform.position - transform.position).magnitude; } }
    protected override Vector3 targetPosition
    {
        get
        {
            Vector3 temp;
            if (distToPlayer < maxRangeToPlayer || targetNode == null) temp = player.transform.position;
            else return base.targetPosition;

            temp.y = transform.position.y;
            return temp;
        }
    }

    protected override void Start()
    {
        base.Start();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    protected override void walk()
    {
        if (distToPlayer > maxRangeToPlayer)
        {
            base.walk();
            return;
        }

        targetNode = null;
        //currentNode = null;

        _body.transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
        if (distToPlayer > minRangeToPlayer) transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxSpeed * Time.deltaTime);
    }

    protected override void wait()
    {
        //
    }

    protected override void arrived()
    {
        base.arrived();
        regeneratePath();
        state = states.walking;
    }

    protected override void regeneratePath()
    {
        targetNode = pickNextNode();
        base.regeneratePath();
    }

    protected override Node pickNextNode()
    {
        if (playerNodeFinder == null)
        {
            playerNodeFinder = player.gameObject.AddComponent<NodeFinder>();
        }

        return playerNodeFinder.closestNode;
    }
}
