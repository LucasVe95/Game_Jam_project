using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public GameObject panelSaisie; 
    public TMP_InputField inputDossard; 
    
    private InfractionType infractionSelectionnee;

    void Awake() => Instance = this;

    public void ClickInfraction(int typeIndex)
    {
        infractionSelectionnee = (InfractionType)typeIndex;
        panelSaisie.SetActive(true); 
        inputDossard.ActivateInputField();


    }

    public void ValiderDossard()
    {
        if (int.TryParse(inputDossard.text, out int numero))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ValiderDecision(numero, infractionSelectionnee);
                Debug.Log($"Infraction {infractionSelectionnee} enregistrée pour le dossard {numero}");
            }
        }
        
        inputDossard.text = "";
        inputDossard.ActivateInputField(); 
    }

    public void FermerMenuDefinitif()
    {
        panelSaisie.SetActive(false); 
        Time.timeScale = 1f; 
    }
}