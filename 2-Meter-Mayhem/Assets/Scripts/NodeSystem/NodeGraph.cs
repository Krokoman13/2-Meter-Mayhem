using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraph : MonoBehaviour
{
    public int nodeCount
    {
        get 
        {
            if (nodes == null) return 0;
            return nodes.Count; 
        }
    }
    public List<Node> nodes;
    [SerializeField] Node _enteranceNode;
    public List<Transform> objectsToSpawn;
    [HideInInspector] public List<Node> waitNodes;
    private int unitsSpawned;

    // Start is called before the first frame update
    void Start()
    {
        nameNodes();
        waitNodes = new List<Node>(nodes);
        waitNodes.RemoveAll(node => node.waitTimeSecs < 0.1f);

        foreach (Node node in nodes)
        {
            foreach (Node connected in node.connected)
            {
                connectNodes(node, connected);
            }
        }

        if (objectsToSpawn != null)
        {
            objectsToSpawn.RemoveAll(item => item == null);
        }
    }

    private void Update()
    {
        if (objectsToSpawn == null) return;
        if (objectsToSpawn.Count < 1) return;

        if (PlayData.instance.obtainedScore > (unitsSpawned + 1) * 500)
        {
            if (_enteranceNode.occupied == null)
            {
                spawn(objectsToSpawn[0]);
                unitsSpawned++;
            }
        }
    }

    public void connectNodes(Node nodeA, Node nodeB)
    {
        if (!nodeA.connected.Contains(nodeB)) nodeA.connected.Add(nodeB);
        if (!nodeB.connected.Contains(nodeA)) nodeB.connected.Add(nodeA);

        AddNode(nodeA);
        AddNode(nodeB);
    }

    public void AddNode(Node node)
    {
        if (nodes.Contains(node)) return;
        
        nodes.Add(node);
    }

    private void spawn(Transform toSpawn)
    {
        objectsToSpawn.Remove(toSpawn);
        toSpawn.gameObject.SetActive(true);

        //Node spawnPosition = spawnableNodes[UnityEngine.Random.Range(0, spawnableNodes.Count)];
        PathfindingNPCBehaviour npc = toSpawn.GetComponent<PathfindingNPCBehaviour>();

        if (npc != null)
        {
            npc.Spawn(_enteranceNode);
        }
    }


    private void OnDrawGizmos()
    {
        foreach (Node node in nodes)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(node.transform.position, 0.1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        nameNodes();
    }

    private void nameNodes()
    {
        if (transform.childCount != nodes.Count)
        {
            nodes = new List<Node>();

            foreach (Transform child in transform)
            {
                nodes.Add(child.GetComponent<Node>());
            }
        }

        nodes.RemoveAll(node => node == null);

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].id = i;
        }
    }
}
