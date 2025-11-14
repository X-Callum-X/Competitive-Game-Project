using UnityEngine;
using TMPro;

public class UpdateCurrency : MonoBehaviour
{
    public TMP_Text currencyText;
    private PlayerController player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();  
    }

    private void Update()
    {
        currencyText.text = player.playerCurrency.ToString();
    }
}
