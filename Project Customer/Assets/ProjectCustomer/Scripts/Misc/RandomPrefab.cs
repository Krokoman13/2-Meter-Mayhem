using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class RandomPrefab : MonoBehaviour
{
    public bool randomize = true;
    [SerializeField] List<GameObject> prefabs;

    private void Awake()
    {
        if (randomize)
        {
            randomize = false;

            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }

            Instantiate(prefabs[Random.Range(0, prefabs.Count)], transform);
        }
    }

}
