using UnityEngine;

public class RestorePurchases : MonoBehaviour
{
    void Start()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer ||
            Application.platform != RuntimePlatform.OSXPlayer) {
            gameObject.SetActive(false);
        }
    }

    public void ClickRestorePurchaseButton() {
        IAPManager.instance.RestorePurchases();
    }
}
