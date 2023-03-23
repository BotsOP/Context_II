using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public GameObject Player;

    public string ShopName;
    public string ItemName;
    public int stockAmount;
    public float basePrice;
    public float finalPrice;
    public bool isBadShop;
    public ShopManager shopManager;

    public int temporaryGoodFuelAmount;
    public int temporaryBadFuelAmount;
    public float temporaryPrice;

    public float totalPrice;

    private bool inRange;

    public GameObject ShopScreen;

    public TextMeshProUGUI shopName;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI stock;
    public TextMeshProUGUI Productprice;
    public TextMeshProUGUI Totalprice;
    public TextMeshProUGUI itemsInbasket;
    public TextMeshProUGUI itemsTypeInbasket;

    public Button AddButton;
    public Button RemoveButton;
    public Button ConfirmButton;
    public Button ExitButton;

    //fmod Code 
    public FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;
    public FMOD.Studio.EventInstance instance2;
    public FMODUnity.EventReference fmodEvent2;
    public int workshopCounter = 0;
    public int bigCompanyCounter = 0;

    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance2 = FMODUnity.RuntimeManager.CreateInstance(fmodEvent2);

        inRange = false;
        totalPrice= 0;
        finalPrice = basePrice;
        if (isBadShop)
        {
            shopManager.BadShopDict.Add(ShopName, this); //does 'this' work?
        }
        else
        {
            shopManager.GoodShopDict.Add(ShopName, this);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inRange)
            {
                OpenShopWindow();
            }
        }

       /// AddButton.onClick.AddListener(delegate { ButtonClicked(1); });
       /// RemoveButton.onClick.AddListener(delegate { ButtonClicked(2); });
       /// ConfirmButton.onClick.AddListener(delegate { ButtonClicked(3); });
       /// ExitButton.onClick.AddListener(delegate { ButtonClicked(4); });

    }

    public void ButtonClicked(int button)
    {
        switch (button)
        {
            case 1:
                AddToBasket();
                break;
            case 2:
                RemoveFromBasket();
                break;
            case 3:
                ConfirmOrder();
                break;
            case 4:
                CloseShopWindow();
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            inRange = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            CloseShopWindow();
            inRange = false;
        }
    }

    public void AddToBasket()
    {
        Debug.Log("Trigger AddToBasket");
        if (stockAmount > 1)
        {
            stockAmount--;
            if (isBadShop)
            {
                temporaryBadFuelAmount++;
            }
            else
            {
                temporaryGoodFuelAmount++;
            }
            totalPrice += finalPrice;
        }
        UpdateShopWindow();
    }
    public void RemoveFromBasket()
    {
        if (isBadShop)
        {
            if(temporaryBadFuelAmount > 1)
            {
                temporaryBadFuelAmount--;
                stockAmount++;
            }
            
        }
        else
        {
            if(temporaryGoodFuelAmount > 1)
            {
                temporaryGoodFuelAmount--;
                stockAmount++;
            }
        }
        totalPrice -= finalPrice;
        UpdateShopWindow();
    }

    public void ConfirmOrder()
    {
        if(totalPrice <= Player.GetComponent<PlayerInventory>().moneyInInventory)
        {
            if (isBadShop)
            {
                Player.GetComponent<PlayerInventory>().BadFuelPacks += temporaryBadFuelAmount;
                Player.GetComponent<PlayerInventory>().moneyInInventory -= totalPrice;

                bigCompanyCounter++;
                instance.setParameterByName("BCFeedback", bigCompanyCounter);
                instance.start();
            }
            else
            {
                Player.GetComponent<PlayerInventory>().GoodFuelPacks += temporaryGoodFuelAmount;
                Player.GetComponent<PlayerInventory>().moneyInInventory -= totalPrice;

                workshopCounter++;
                instance2.setParameterByName("WSFeedback", workshopCounter);
                instance2.start();
            }
            CloseShopWindow();
        }
    }

    public void OpenShopWindow()
    {
        shopManager.CurrentShop = this;
        ShopScreen.SetActive(true);
        shopName.text = ShopName;
        itemName.text = ItemName;
        itemsTypeInbasket.text = ItemName;
        Productprice.text = finalPrice.ToString();
        if (isBadShop)
        {
            itemsInbasket.text = temporaryBadFuelAmount.ToString();
        }
        else
        {
            itemsInbasket.text = temporaryGoodFuelAmount.ToString();
        }  
        stock.text = stockAmount.ToString();
        Totalprice.text = totalPrice.ToString();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CloseShopWindow()
    {
        shopManager.CurrentShop = null;
        ShopScreen.SetActive(false);
        if (isBadShop)
        {
            for(int i = temporaryBadFuelAmount; i > 0; i--)
            {
                temporaryBadFuelAmount--;
                stockAmount++;
            }
        }
        else
        {
            for (int i = temporaryGoodFuelAmount; i > 0; i--)
            {
                temporaryBadFuelAmount--;
                stockAmount++;
            }
        }
        totalPrice = 0;

        UpdateShopWindow();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateShopWindow()
    {
        stock.text = stockAmount.ToString();
        Totalprice.text = totalPrice.ToString();
        if (isBadShop)
        {
            itemsInbasket.text = temporaryBadFuelAmount.ToString();
        }
        else
        {
            itemsInbasket.text = temporaryGoodFuelAmount.ToString();
        }
    }
}
