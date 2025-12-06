using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class BarrierPaymentSystem : MonoBehaviour
{
    public int amountToPay;

    public TMP_Text amountToPayText;

    private PlayerController player;

    public GameObject notEnoughCurrencyText;

    public GameObject barrier;

    private void Start()
    {
        amountToPayText.text = amountToPay.ToString();
        player = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && player.playerCurrency >= amountToPay)
        {
            player.playerCurrency -= amountToPay;
            Destroy(barrier);
        }
        else if (other.gameObject.CompareTag("Player") && player.playerCurrency < amountToPay)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayNotEnoughText());
        }
    }

    private IEnumerator DisplayNotEnoughText()
    {
        notEnoughCurrencyText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        notEnoughCurrencyText.gameObject.SetActive(false);
    }
}
