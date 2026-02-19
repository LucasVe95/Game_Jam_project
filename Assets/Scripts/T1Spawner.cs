using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class T1Spawner : MonoBehaviour
{
    public GameObject athletePrefab;
    public float tempsEntreAthletes = 0.5f;

    [Header("Points de Passage")]
    public Transform entreeParc; 
    public Transform sortieParc; 

    [Header("Réglages des Barres")]
    public List<Transform> lesBarres; 
    public float hauteurBarre = 6.5f; 
    public float decalageMilieuAllee = 1.3f; 

    void Start()
    {
        if (GameManager.Instance == null) {
            Debug.LogError("ERREUR : GameManager introuvable !");
            return;
        }

        if (GameManager.Instance.tousLesAthletes.Count == 0) {
            Debug.LogWarning("ATTENTION : La liste des athlètes est VIDE dans le GameManager.");
        }

        if (lesBarres.Count == 0) {
            Debug.LogError("ERREUR : Aucune barre n'est assignée dans la liste 'Les Barres'.");
            return;
        }

        if (athletePrefab == null) {
            Debug.LogError("ERREUR : Le Prefab Athlète n'est pas assigné !");
            return;
        }

        StartCoroutine(SpawnAvecNavigationTriche());
    }

    IEnumerator SpawnAvecNavigationTriche()
    {
        Debug.Log("Lancement du Spawn pour " + GameManager.Instance.tousLesAthletes.Count + " athlètes.");

        foreach (var donnee in GameManager.Instance.tousLesAthletes)
        {
            GameObject athlete = Instantiate(athletePrefab, entreeParc.position, Quaternion.identity);
            AthleteEvent script = athlete.GetComponent<AthleteEvent>();

            if (script != null)
            {
                script.dossard = donnee.dossard;
                script.speed = Random.Range(4.5f, 6.5f);

                float des = Random.value;
                script.infractionReelle = InfractionType.None;
                script.aSonCasque = true;
                script.dossardArriere = true;

                if (des < 0.15f) script.infractionReelle = InfractionType.SousBarre;
                else if (des < 0.30f) { script.infractionReelle = InfractionType.PasDeCasque; script.aSonCasque = false; }
                else if (des < 0.45f) { script.infractionReelle = InfractionType.DossardDevant; script.dossardArriere = false; }

                if (script.infractionReelle != InfractionType.None)
                {
                    GameManager.Instance.totalInfractionsT1++;
                }

                script.InitialiserVisuels();

                Transform barre = lesBarres[Random.Range(0, lesBarres.Count)];
                float cote = (Random.value > 0.5f) ? decalageMilieuAllee : -decalageMilieuAllee;
                float posX = barre.position.x + cote;
                float posY = barre.position.y + Random.Range(-hauteurBarre/2.2f, hauteurBarre/2.2f);
                Vector3 posVelo = new Vector3(posX, posY, 0);

                script.waypoints = new List<Transform>();

                if (script.infractionReelle != InfractionType.SousBarre)
                {
                    GameObject w0 = new GameObject("WP_Align_" + script.dossard);
                    w0.transform.position = new Vector3(posX, entreeParc.position.y, 0);
                    script.waypoints.Add(w0.transform);
                    script.indexStopVelo = 1; 
                }
                else { script.indexStopVelo = 0; }

                GameObject w1 = new GameObject("WP_Stop_" + script.dossard);
                w1.transform.position = posVelo;
                script.waypoints.Add(w1.transform);

                GameObject w2 = new GameObject("WP_Exit_" + script.dossard);
                w2.transform.position = new Vector3(posX, sortieParc.position.y, 0);
                script.waypoints.Add(w2.transform);

                script.waypoints.Add(sortieParc);
                script.currentWaypointIndex = 0;
            }
            yield return new WaitForSeconds(tempsEntreAthletes);
        }
    }
}