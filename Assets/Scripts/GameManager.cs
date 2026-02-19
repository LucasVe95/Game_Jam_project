using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Suivi de la Course")]
    public List<DonneesAthlete> tousLesAthletes = new List<DonneesAthlete>();
    public int totalInfractionsNatation = 0;
    public int totalInfractionsT1 = 0; 
    public int totalInfractionsVelo = 0; 
    [Header("Contrôle de Fin d'Étape")]
    public EventSpawner spawner; 
    private bool natationFinie = false;
    private bool t1Finie = false;
    private bool veloFinie = false;
    public static bool menuFinalAffiche = false; 

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
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

    public void TerminerEtapeNatation()
    {
        Debug.Log("<color=green>Étape Natation terminée ! Passage à la T1.</color>");
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.FermerMenuDefinitif();
        }

        
        SceneManager.LoadScene("T1"); 
    }

    public void TerminerEtapeT1()
    {
        Debug.Log("<color=green>Étape T1 terminée ! Passage au Vélo.</color>");
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.FermerMenuDefinitif();
        }

        
        SceneManager.LoadScene("velo"); 
    }

    public void TerminerEtapeVelo()
    {
        Debug.Log("<color=green>Étape Vélo terminée ! Passage à la T2.</color>");
        
        AthleteVelo[] athletes = Object.FindObjectsByType<AthleteVelo>(FindObjectsSortMode.None);
        foreach (var athlete in athletes)
        {
            Destroy(athlete.gameObject);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.FermerMenuDefinitif();
        }

        
        SceneManager.LoadScene("T2"); 
    }
}

[System.Serializable]
public class DonneesAthlete
{
    public int dossard;
    public InfractionType infractionReelle;
    public InfractionType infractionChoisieParJoueur = InfractionType.None;
}