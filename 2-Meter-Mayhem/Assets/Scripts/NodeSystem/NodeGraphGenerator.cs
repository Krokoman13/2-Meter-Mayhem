using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class NodeGraphGenerator : MonoBehaviour
{
    private NodeGraph nodeGraph;
    [SerializeField] bool StartGenerating;
    [SerializeField] Node nodePrefab;

    Vector3[,] points;

    Node[,] nodes;

    [SerializeField] int horizontal;
    [SerializeField] int vertical;
    [SerializeField] float distanceBetween;

    float width
    {
        get { return horizontal * distanceBetween + distanceBetween/2f; }
    }
    float height
    {
        get { return vertical * distanceBetween * 0.80f; }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 1f, height));
        Gizmos.color = Color.yellow;

        for (int i = 0; i < points.GetLength(0); i++)
        {
            for (int j = 0; j < points.GetLength(1); j++)
            {
                Gizmos.DrawSphere(points[i, j] + transform.position, 0.5f);
            }
        }
    }

    private void OnValidate()
    {
        points = new Vector3[horizontal, vertical];

        for (int i = 0; i < points.GetLength(0); i++)
        {
            for (int j = 0; j < points.GetLength(1); j++)
            {
                points[i, j] = new Vector3((i * distanceBetween), 0f, (j * distanceBetween * 0.80f));
                if (j % 2 == 0) points[i, j] += new Vector3(distanceBetween / 2f, 0f, 0f);

                points[i, j] -= new Vector3((width - distanceBetween)/2f, 0f, (height - distanceBetween) / 2f);
            }
        }

        if (StartGenerating)
        {
            nodeGraph = GetComponent<NodeGraph>();
            if (nodeGraph == null) Debug.LogError("Nodegraph not present!");

            for (int i = transform.childCount; i > 0; --i)
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                };
                #endif
            }

            nodes = new Node[horizontal, vertical];

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j] = Instantiate(nodePrefab.gameObject, points[i, j] + transform.position, Quaternion.Euler(Vector3.zero), transform).GetComponent<Node>();

                    Node currentNode = nodes[i, j];
                    nodeGraph.AddNode(currentNode);
                    if (i > 0)
                    {
                        nodeGraph.connectNodes(currentNode, nodes[i - 1, j]);

                        if (j % 2 != 0)
                        {
                            if (j > 0) nodeGraph.connectNodes(currentNode, nodes[i - 1, j - 1]);
                            if (j < nodes.GetLength(1) - 1) nodeGraph.connectNodes(currentNode, nodes[i - 1, j + 1]);
                        }
                    }
                    if (j > 0)
                    {
                        nodeGraph.connectNodes(currentNode, nodes[i, j - 1]);
                    }
                }
            }
        }
    }
}

