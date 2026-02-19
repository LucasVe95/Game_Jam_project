using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 

public class NatationFinalUI : MonoBehaviour
{
    [Header("Réglages UI")]
    public GameObject panelComptage; 
    public TMP_InputField inputJoueur; 
    
    private bool menuAffiche = false;

    void Start()
    {
        if (panelComptage != null) panelComptage.SetActive(false);
        
        if (inputJoueur != null) 
        {
            inputJoueur.interactable = true;
            inputJoueur.text = ""; 
        }
    }

    void Update()
    {
        if (!menuAffiche)
        {
            AthleteEvent[] athletes = Object.FindObjectsByType<AthleteEvent>(FindObjectsSortMode.None);

            if (athletes.Length == 0 && Time.timeSinceLevelLoad > 3f) 
            {
                AfficherLeMenu();
            }
        }
    }

    void AfficherLeMenu()
    {
        menuAffiche = true;
        GameManager.menuFinalAffiche = true; 
        if (panelComptage != null) 
        {
            panelComptage.SetActive(true); 
            Time.timeScale = 0f; 
            Cursor.visible = true; 
            
            Debug.Log("EventSystem existe : " + (EventSystem.current != null));
            
            StartCoroutine(ForcerFocusInput());
        }
    }

    System.Collections.IEnumerator ForcerFocusInput()
    {
        yield return null;
        
        if (inputJoueur != null && EventSystem.current != null)
        {
    
            EventSystem.current.SetSelectedGameObject(inputJoueur.gameObject);
            inputJoueur.ActivateInputField(); 
            inputJoueur.Select(); 
        }
        else
        {
            Debug.LogWarning("Input joueur ou EventSystem manquant !");
        }
    }

    public void ValiderReponse()
    {
        if (string.IsNullOrEmpty(inputJoueur.text)) return;

        if (int.TryParse(inputJoueur.text, out int reponse))
        {
            int reel = GameManager.Instance.totalInfractionsNatation;
            
            if (reponse == reel) Debug.Log("<color=green>JUSTE !</color>");
            else Debug.Log($"<color=red>FAUX ! Il y avait {reel}.</color>");

            Time.timeScale = 1f; 
            GameManager.menuFinalAffiche = false; 
            GameManager.Instance.TerminerEtapeNatation();
        }
    }
}