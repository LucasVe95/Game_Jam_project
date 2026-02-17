using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AthleteEvent : MonoBehaviour, IPointerClickHandler
{
    [Header("Caractéristiques")]
    public int dossard;
    public float speed = 5f;
    public InfractionType infractionReelle;

    [Header("Navigation")]
    public List<Transform> waypoints; 
    private int currentWaypointIndex = 0;
    public float distanceSeuil = 0.2f; 
void Start()
{

    if (infractionReelle == InfractionType.parcour_coupée)
    {
        if (waypoints != null && waypoints.Count > 1)
        {
            currentWaypointIndex = 1; 

        }
    }
}
void Update()
{
    if (waypoints == null || waypoints.Count == 0 || currentWaypointIndex >= waypoints.Count)
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
    }
    else
    {
        Vector3 cible = waypoints[currentWaypointIndex].position;

        Vector2 direction = (cible - transform.position).normalized;
        transform.right = direction; 
        transform.position = Vector2.MoveTowards(transform.position, cible, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, cible) < distanceSeuil)
        {
            currentWaypointIndex++;
           
        }
    }


    if (transform.position.x > 25f || transform.position.x < -25f || 
        transform.position.y > 20f || transform.position.y < -20f)
    {
        Destroy(gameObject);
    }
}

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLIC ! Dossard : " + dossard);
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}