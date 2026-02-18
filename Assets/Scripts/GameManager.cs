using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Suivi de la Course")]
    public List<DonneesAthlete> tousLesAthletes = new List<DonneesAthlete>();

    [Header("Contrôle de Fin d'Étape")]
    public EventSpawner spawner; 
    private bool natationFinie = false;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!natationFinie && spawner != null && !spawner.courseEnCours && tousLesAthletes.Count > 0)
        {
            if (GameObject.FindObjectsOfType<AthleteEvent>().Length == 0)
            {
                natationFinie = true;
                TerminerEtapeNatation();
            }
        }
    }

    public void EnregistrerNouvelAthlete(int bib, InfractionType reel)
    {
        tousLesAthletes.Add(new DonneesAthlete { dossard = bib, infractionReelle = reel });
    }

    public void ValiderDecision(int bib, InfractionType choix)
    {
        DonneesAthlete rapport = tousLesAthletes.Find(r => r.dossard == bib);

        if (rapport != null)
        {
            rapport.infractionChoisieParJoueur = choix;
            Debug.Log($"<color=cyan>Arbitrage enregistré : Dossard {bib} -> {choix}</color>");
        }
        else
        {
            Debug.LogWarning($"Dossard {bib} introuvable dans la course !");
        }
    }

    private void TerminerEtapeNatation()
    {
        Debug.Log("<color=green>Étape Natation terminée ! Passage à la T1.</color>");
        
        // On ferme le menu si besoin
        if (UIManager.Instance != null)
        {
            UIManager.Instance.FermerMenuDefinitif();
        }

        
        SceneManager.LoadScene("T1"); 
    }
}

[System.Serializable]
public class DonneesAthlete
{
    public int dossard;
    public InfractionType infractionReelle;
    public InfractionType infractionChoisieParJoueur = InfractionType.None;
}