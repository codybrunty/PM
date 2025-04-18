using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyButton : MonoBehaviour {

    public enum ItemType {
        Gold_500,
        Gold_1500,
        Gold_5000
    }

    public ItemType itemType;
    public TextMeshProUGUI priceText;
    public CoinTotal coinTotal;
    private string defaultText;
    [SerializeField] GameObject dc_image=default;

    void Start () {
        defaultText = priceText.text;
        StartCoroutine(LoadPriceRoutine());
	}

    public void ClickBuy() {
        switch (itemType) {
            case ItemType.Gold_500:
                IAPManager.iapManager.Buy_Gold_500();
                break;
            case ItemType.Gold_1500:
                IAPManager.iapManager.Buy_Gold_1500();
                break;
            case ItemType.Gold_5000:
                IAPManager.iapManager.Buy_Gold_5000();
                break;
        }
    }

    private IEnumerator LoadPriceRoutine() {
        while (!IAPManager.iapManager.IsInitialized())
            yield return null;

        gameObject.GetComponent<Button>().interactable = true;
        dc_image.SetActive(false);
        LoadPrices();
    }

    private void LoadPrices() {
        string loadedPrice = "";

        switch (itemType) {
            case ItemType.Gold_500:
                loadedPrice = IAPManager.iapManager.GetProductPriceFromStore(IAPManager.iapManager.Gold_500);
                break;
            case ItemType.Gold_1500:
                loadedPrice = IAPManager.iapManager.GetProductPriceFromStore(IAPManager.iapManager.Gold_1500);
                break;
            case ItemType.Gold_5000:
                loadedPrice = IAPManager.iapManager.GetProductPriceFromStore(IAPManager.iapManager.Gold_5000);
                break;
        }

        priceText.text = loadedPrice;
    }
}
