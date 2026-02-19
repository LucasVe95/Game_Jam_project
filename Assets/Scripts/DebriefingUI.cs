using UnityEngine;
using TMPro;

public class DebriefingUI : MonoBehaviour
{
    public GameObject panelDebrief;
    public TMP_Text texteResultats;
    public UnityEngine.UI.Button boutonQuitter; 
    public int scoreFinal = 0;

    void Start()
    {
        GenererDebrief();
        if (boutonQuitter != null)
        {
            boutonQuitter.onClick.AddListener(QuitterJeu);
        }
    }

    public void GenererDebrief()
    {
        panelDebrief.SetActive(true);
        string resume = "RÉSULTATS FINAUX :\n\n";

        int totalInfractionsNatation = GameManager.Instance.totalInfractionsNatation;
        int totalInfractionsT1 = GameManager.Instance.totalInfractionsT1;
        int totalInfractions = totalInfractionsNatation + totalInfractionsT1;

        resume += $"Étape Natation : {totalInfractionsNatation} infractions détectées\n";
        resume += $"Étape T1 : {totalInfractionsT1} infractions détectées\n";
        resume += $"Total : {totalInfractions} infractions\n\n";

        int score = totalInfractions * 10; 
        resume += $"Score : {score} points\n\n";

        resume += "Cliquez sur le bouton pour quitter.";

        texteResultats.text = resume;
    }

    public void QuitterJeu()
    {
        Debug.Log("Jeu terminé !");
        Application.Quit(); 
    }
}