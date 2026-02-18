using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AthleteEvent : MonoBehaviour, IPointerClickHandler
{
    [Header("Caractéristiques")]
    public int dossard;
    public float speed = 5f;
    public InfractionType infractionReelle = InfractionType.None;

    [Header("Visuels de Faute")]
    public GameObject pointVertVisuel;    // Natation : Gêner concurrent
    public GameObject visuelFauteCasque;   // T1 : Point Noir (Pas de casque)
    public GameObject visuelFauteDossard;  // T1 : Point Rouge (Dossard devant, après arrêt)
    public GameObject visuelFauteSousBarre; // T1 : NOUVEAU -> Point Bleu (Coupe la trajectoire)

    public bool aSonCasque = true;
    public bool dossardArriere = true;

    [Header("Navigation")]
    public List<Transform> waypoints; 
    public int currentWaypointIndex = 0;
    public float distanceSeuil = 0.2f; 
    public int indexStopVelo = 1; 

    private bool estEnPauseVelo = false;
    private bool aPrisSonVelo = false;

    public void ActiverAlerteVictime()
    {
        if (pointVertVisuel != null) pointVertVisuel.SetActive(true);
    }

    public void InitialiserVisuels()
    {
        // 1. On éteint tout par sécurité au début
        if (pointVertVisuel != null) pointVertVisuel.SetActive(false);
        if (visuelFauteCasque != null) visuelFauteCasque.SetActive(false);
        if (visuelFauteDossard != null) visuelFauteDossard.SetActive(false);
        if (visuelFauteSousBarre != null) visuelFauteSousBarre.SetActive(false);

        string scene = SceneManager.GetActiveScene().name;

        if (scene == "T1")
        {
            // --- GESTION DES FAUTES T1 ---

            // A. PAS DE CASQUE -> Point Noir immédiat
            if (visuelFauteCasque != null && !aSonCasque) 
                visuelFauteCasque.SetActive(true);

            // B. SOUS LA BARRE -> Point Bleu immédiat (NOUVEAU)
            if (visuelFauteSousBarre != null && infractionReelle == InfractionType.SousBarre)
                visuelFauteSousBarre.SetActive(true);

            // C. DOSSARD DEVANT -> Point Rouge (s'activera plus tard après l'arrêt vélo)
        }
        else 
        {
            // --- GESTION NATATION ---
            if (infractionReelle == InfractionType.gener_concurents) ActiverAlerteVictime();
            aPrisSonVelo = true; 
        }
    }

    void Start() { InitialiserVisuels(); }

    void Update()
    {
        if (estEnPauseVelo) return; 

        // ... (Code de navigation standard) ...
        if (waypoints == null || waypoints.Count == 0 || currentWaypointIndex >= waypoints.Count)
        {
            Vector3 dir = (SceneManager.GetActiveScene().name == "T1") ? Vector3.down : Vector3.right;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
        }
        else
        {
            Vector3 cible = waypoints[currentWaypointIndex].position;
            transform.right = (cible - transform.position).normalized; 
            transform.position = Vector2.MoveTowards(transform.position, cible, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, cible) < distanceSeuil)
            {
                if (currentWaypointIndex == indexStopVelo && !aPrisSonVelo) StartCoroutine(SimulerPriseVelo());
                else currentWaypointIndex++;
            }
        }

        if (transform.position.y < -35f || transform.position.x > 55f) Destroy(gameObject);
    }

    IEnumerator SimulerPriseVelo()
    {
        estEnPauseVelo = true;
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        aPrisSonVelo = true; 

        // Activation du Point Rouge si le dossard est mal mis
        if (visuelFauteDossard != null && !dossardArriere)
        {
            visuelFauteDossard.SetActive(true);
            Debug.Log($"<color=red>[INFRACTION] Dossard {dossard} repart avec le DOSSARD DEVANT !</color>");
        }

        estEnPauseVelo = false; 
        currentWaypointIndex++; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clic sur {dossard}. Infraction : {infractionReelle}");
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}