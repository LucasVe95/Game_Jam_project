using UnityEngine;
using UnityEngine.UI;

public class DebriefingUI : MonoBehaviour
{
    public GameObject panelDebrief;
    public Text texteResultats;
    public int scoreFinal = 0;

    public void GenererDebrief()
    {
        panelDebrief.SetActive(true);
        scoreFinal = 0;
        string resume = "RÉSULTATS FINAUX :\n\n";

        foreach (var athlete in GameManager.Instance.tousLesAthletes)
        {
            resume += $"Dossard {athlete.dossard} : ";

            if (athlete.infractionChoisieParJoueur == athlete.infractionReelle)
            {
                
                if (athlete.infractionReelle != InfractionType.None)
                {
                    scoreFinal += 100;
                    resume += "<color=green>CORRECT (+100)</color>";
                }
                else
                {
                    resume += "RAS"; 
                }
            }
            else
            {
                scoreFinal -= 50;
                resume += $"<color=red>ERREUR (-50)</color> (Reel: {athlete.infractionReelle})";
            }
            resume += "\n";
        }

        texteResultats.text = resume + $"\n\nSCORE TOTAL : {scoreFinal}";
    }
}