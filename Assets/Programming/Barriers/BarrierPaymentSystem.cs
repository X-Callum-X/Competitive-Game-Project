using System.Collections;
using TMPro;
using UnityEngine;

public class BarrierPaymentSystem : MonoBehaviour
{
    public int amountToPay;

    public TMP_Text amountToPayText;

    private void Start()
    {
        amountToPayText.text = amountToPay.ToString();
    }
}
