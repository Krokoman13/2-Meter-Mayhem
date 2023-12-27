using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathFinder : MonoBehaviour
{

	protected List<Node> toDoList;
	protected List<Node> doneList;

	[SerializeField] public Node startNode;
	[SerializeField] public Node endNode;

	protected NodeGraph _nodeGraph;

	[SerializeField] protected List<Node> _path = null;

	public List<Node> Generate(Node pFrom, Node pTo, bool debug = false)
	{
		if (pFrom == null || pTo == null) return null;
		

		startNode = pFrom;
		endNode = pTo;

		_path = null;
		toDoList = new List<Node>();
		doneList = new List<Node>();
		toDoList.Add(pFrom);

		bool done = false;
		int iterationCounter = 0;
		pFrom.gCost = 0;

		while (!done)
		{
			iterationCounter++;

			if (toDoList.Count < 1)
			{
				done = true;
				Debug.LogWarning("Path not found");
				continue;
			}

			Node currentNode = getNodeFrom(toDoList);

			toDoList.Remove(currentNode);
			doneList.Add(currentNode);

			if (currentNode == pTo)
			{
				_path = generateList(pFrom, pTo);
				done = true;
				continue;
			}

			addNewNodes(currentNode);
		}
		if (debug)
		{
			Debug.Log("Amount of nodes checked: " + doneList.Count);
		}
		return _path;
	}

	public List<Node> Generate()
	{
		_path = Generate(startNode, endNode);
		return _path;
	}

	protected virtual void addNewNodes(Node currentNode)
	{
		foreach (Node neighbour in currentNode.connected)
		{
			if (doneList.Contains(neighbour) || neighbour.occupied != null)
			{
				continue;
			}

			float newMomentCostToNeighbour = currentNode.gCost + distance(currentNode, neighbour);

			if (newMomentCostToNeighbour < neighbour.gCost || !toDoList.Contains(neighbour))
			{
				neighbour.gCost = newMomentCostToNeighbour;
				neighbour.hCost = distance(neighbour, endNode);

				neighbour.nodeParent = currentNode;

				if (!toDoList.Contains(neighbour))
				{
					toDoList.Add(neighbour);
				}
			}
		}
	}

	protected Node getNodeFrom(List<Node> possibleNodes)
	{
		Node currentNode = possibleNodes[0];

		for (int i = 1; i < possibleNodes.Count; i++)
		{
			if (possibleNodes[i].fCost < currentNode.fCost || (possibleNodes[i].fCost == currentNode.fCost && possibleNodes[i].hCost < currentNode.hCost))
			{
				currentNode = possibleNodes[i];
			}
		}

		return currentNode;
	}

	protected List<Node> generateList(Node pFrom, Node pTo)
	{
		Node parent = pTo;
		List<Node> path = new List<Node>();

		while (parent != pFrom)
		{
			path.Add(parent);
			parent = parent.nodeParent;
		}

		path.Add(pFrom);
		path.Reverse();

		return path;
	}

	protected float distance(Node pFrom, Node pTo)
	{
		return 4.5f;
	}

/*    private void Update()
    {
		if (!validPath(_path)) _path = Generate(startNode, endNode);
	}*/

    private bool validPath(List<Node> path)
    {
		if (path.Count < 1) return false;
		return (path[0] == startNode) && (path[path.Count - 1] == endNode);
    }
    private void OnDrawGizmosSelected()
    {
		if (_path == null) return;

		Node previousNode = null;
		Gizmos.color = Color.yellow;
		foreach (Node node in _path)
		{
			if (previousNode != null)
			{
				Gizmos.DrawLine(previousNode.transform.position, node.transform.position);
			}
			previousNode = node;
		}

		Gizmos.color = Color.green;
		Gizmos.DrawSphere(startNode.transform.position, 0.25f);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(endNode.transform.position, 0.25f);
	}
}

