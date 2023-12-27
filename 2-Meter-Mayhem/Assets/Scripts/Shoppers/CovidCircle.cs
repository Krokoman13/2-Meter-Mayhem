using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidCircle : MonoBehaviour
{
    public Color myColor;
    SpriteRenderer myRenderer;
    [SerializeField] GameObject player;
    ExposureMeter exposureMeter = null;
    static float infection = 18f;
    [SerializeField] float distanceMultiplier;
    LineRenderer line;

    [SerializeField] float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        //line.enabled = false;

        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        myColor = Color.cyan;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        player = other.gameObject;
        exposureMeter = player.GetComponent<ExposureMeter>();
      
        AudioHandler.instance.PlayRandomSFXFromArray(AudioHandler.instance.sfx_coughing);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        player = null;
    }

    private void Update()
    {
        myRenderer.color = myColor;
        line.SetPosition(0, transform.position);

        if (player != null)
        {
            //myRenderer.enabled = false;
            line.SetPosition(1, player.transform.position);

            Vector3 distance = transform.position - player.transform.position;
            distance.y = 0;

            distanceToPlayer = distance.magnitude;

            if (distanceToPlayer < 5f)
            {
                line.endColor = line.startColor = myColor = Color.Lerp(Color.red, Color.yellow, distanceToPlayer / 5f);

                distanceMultiplier = 2f - distanceToPlayer / 5;
                distanceMultiplier *= distanceMultiplier;
                distanceMultiplier *= distanceMultiplier;
                distanceMultiplier *= distanceMultiplier;
                //distanceMultiplier -= 2f;
                distanceMultiplier = Mathf.Clamp(distanceMultiplier, 0.1f, 100f);

                exposureMeter.Raise(distanceMultiplier * infection * Time.deltaTime);
                return;
            }

            line.endColor = line.startColor = myColor = Color.cyan;
            //myColor.a = 0.25f;
            return;
        }

        myColor = new Color(211, 189, 189);
        //myColor.a = 0.20f;
        myRenderer.enabled = true;
        line.SetPosition(1, transform.position);
    }
}

