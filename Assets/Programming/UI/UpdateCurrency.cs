using UnityEngine;
using TMPro;

public class UpdateCurrency : MonoBehaviour
{
    private PlayerController player;

    public TMP_Text currencyText;
    public TMP_Text secretsCollectedText;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();  
    }

    private void Update()
    {
        currencyText.text = player.playerCurrency.ToString();
        secretsCollectedText.text = player.secretsCollected.ToString() + "/3";
    }
}
