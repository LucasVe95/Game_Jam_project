using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawner : MonoBehaviour
{
    public GameObject athletePrefab;
    public float tempsEntreApparitions = 3f;
    
    [Header("Limites de la course")]
    public int maxAthletes = 10; 
    private int athletesSpawnes = 0; 
    public bool courseEnCours = true;

    [Header("Configuration du Parcours")]
    public List<Transform> cheminDeCourse;

    void Start()
    {
        StartCoroutine(GenererAthletes());
    }

    IEnumerator GenererAthletes()
    {
        while (courseEnCours && athletesSpawnes < maxAthletes)
        {
            GameObject nouvelAthlete = Instantiate(athletePrefab, transform.position, Quaternion.identity);
   
            AthleteEvent infos = nouvelAthlete.GetComponent<AthleteEvent>();
            
            infos.dossard = Random.Range(100, 999);
            infos.speed = Random.Range(1.5f, 3.5f);
            infos.waypoints = cheminDeCourse;

            infos.infractionReelle = (Random.value > 0.8f) ? 
                InfractionType.parcour_coupée : InfractionType.None;

            athletesSpawnes++;
            

            yield return new WaitForSeconds(tempsEntreApparitions);
        }

        courseEnCours = false;
        Debug.Log("Tous les athlètes sont partis. Fin du spawn.");
    }
}