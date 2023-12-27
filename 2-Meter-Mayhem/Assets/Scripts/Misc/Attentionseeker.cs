using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Attentionseeker : MonoBehaviour
{
    [SerializeField] Transform player;
    LineRenderer line;
    Renderer renderer;
    
    float distanceToPlayer
    {
        get
        {
            return (player.transform.position - transform.position).magnitude;
        }
    }
    public float minDistance;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        renderer = GetComponent<Renderer>();
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        line.startWidth = 0.25f;
        line.endWidth = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!renderer.enabled) return;
        line.SetPosition(0, new Vector3(transform.position.x, player.transform.position.y, transform.position.z));
        line.SetPosition(1, player.transform.position);

        if (distanceToPlayer < minDistance)
        {
            line.enabled = false;
            enabled = false;
        }
    }
}
