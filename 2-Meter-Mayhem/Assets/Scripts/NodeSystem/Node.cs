using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private NodeGraph nodeGraph;
    [SerializeField] private GameObject directionIndicator;
    [SerializeField] private GameObject faceTowards;

    public List<Node> connected;
    private int _id;
    public int id
    {
        set
        {
            _id = value;
            name = "Node: " + _id;
        }
    }

    public Node nodeParent = null;

    public float gCost;     //Total calculated distance to the start node
    public float hCost;     //Estimated distance to the end node
    public float fCost
    {
        get
        {
            return gCost + hCost;   //gCost and Hcost combined
        }
    }

    public float waitTimeSecs = 0.1f;
    public PathfindingNPCBehaviour occupied = null;

    [SerializeField] bool disconnectNodes;

    private void Awake()
    {
        nodeGraph = transform.parent.GetComponent<NodeGraph>();
        if (nodeGraph == null) return;

        foreach (Node node in connected)
        {
            nodeGraph.connectNodes(this, node);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(transform.GetChild(0).gameObject);
    }

/*    private void OnDrawGizmosSelected()
    {
        connected.RemoveAll(item => item == null);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);

        UnityEditor.Handles.Label(transform.position, name);

        foreach (Node node in connected)
        {

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, node.transform.position);
            UnityEditor.Handles.Label(node.transform.position, node.name);

            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(node.transform.position, 0.1f);
        }
    }

    private void OnValidate()
    {
        if (faceTowards != null)
        {
            waitTimeSecs = 5f;
            transform.rotation = Quaternion.LookRotation(faceTowards.transform.position - transform.position);
            faceTowards = null;
        }

        if (transform.childCount > 0)
        {
            if (waitTimeSecs > 0) directionIndicator.SetActive(true);
            else directionIndicator.SetActive(false);
        }

        if (disconnectNodes)
        {
            disconnectNodes = false;

            foreach (Node node in connected)
            {
                node.connected.Remove(this);
            }

            //connected = new List<Node>();
        }
    }*/

    public bool Connected(Node other)
    {
        if (other == null) return false;

        if (connected.Contains(other)) return true;
        if (other.connected.Contains(this)) return true;

        return false;
    }
}
