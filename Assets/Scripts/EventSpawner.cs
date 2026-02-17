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
        if (athletePrefab == null)
        {
            Debug.LogError("Attention : Le prefab 'Athlète' n'est pas assigné dans l'inspecteur du générateur !");
            return;
        }
        
        StartCoroutine(GenererAthletes());
    }

    IEnumerator GenererAthletes()
    {
        while (courseEnCours && athletesSpawnes < maxAthletes)
        {
            GameObject nouvelAthlete = Instantiate(athletePrefab, transform.position, Quaternion.identity);
            
            AthleteEvent infos = nouvelAthlete.GetComponent<AthleteEvent>();
            
            if (infos != null)
            {
                infos.dossard = Random.Range(100, 999);
                infos.speed = Random.Range(5f, 10f);
                infos.waypoints = cheminDeCourse;

                float probaTriche = Random.value;
                if (probaTriche > 0.7f) 
                {
                    infos.infractionReelle = (InfractionType)Random.Range(1, 4);
                }
                else 
                {
                    infos.infractionReelle = InfractionType.None;
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.EnregistrerNouvelAthlete(infos.dossard, infos.infractionReelle);
                }
                else
                {
                    Debug.LogWarning("GameManager.Instance est introuvable ! L'athlète ne sera pas jugé au débrief.");
                }

                athletesSpawnes++;
            }

            yield return new WaitForSeconds(tempsEntreApparitions);
        }

        courseEnCours = false;
        Debug.Log("Fin de la génération : tous les athlètes sont en course.");
    }
}