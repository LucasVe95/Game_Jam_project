using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawner : MonoBehaviour
{
    public GameObject athletePrefab;
    
    [Header("Paramètres du Groupe")]
    public int maxAthletes = 10; 
    public float rayonDeGroupement = 3f; 
    public bool courseEnCours = true;

    [Header("Configuration du Parcours")]
    public List<Transform> cheminDeCourse;

    void Start()
    {
        if (athletePrefab == null)
        {
            Debug.LogError("Attention : Le prefab 'Athlète' n'est pas assigné !");
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.totalInfractionsNatation = 0;
        }
        
        LancerDepartGroupe();
    }

    void LancerDepartGroupe()
    {
        for (int i = 0; i < maxAthletes; i++)
        {
            Vector3 positionDepart = transform.position + new Vector3(
                Random.Range(-rayonDeGroupement, rayonDeGroupement), 
                Random.Range(-rayonDeGroupement, rayonDeGroupement), 
                0
            );

            GameObject nouvelAthlete = Instantiate(athletePrefab, positionDepart, Quaternion.identity);
            AthleteEvent infos = nouvelAthlete.GetComponent<AthleteEvent>();
            
            if (infos != null)
            {
                infos.dossard = Random.Range(100, 999);
                infos.speed = Random.Range(4f, 8f); 
                infos.waypoints = cheminDeCourse;

                float probaTriche = Random.value;

                if (probaTriche > 0.7f) 
                {
                    infos.infractionReelle = InfractionType.gener_concurents;

                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.totalInfractionsNatation++;
                        Debug.Log($"<color=cyan>[NATATION] Infraction créée ! Total à trouver : {GameManager.Instance.totalInfractionsNatation}</color>");
                    }
                }
                else 
                { 
                    infos.infractionReelle = InfractionType.None; 
                }

                infos.InitialiserVisuels();

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.EnregistrerNouvelAthlete(infos.dossard, infos.infractionReelle);
                }
            }
        }

        courseEnCours = false; 
        Debug.Log("Départ de " + maxAthletes + " nageurs. Bon courage pour le comptage !");
    }
}