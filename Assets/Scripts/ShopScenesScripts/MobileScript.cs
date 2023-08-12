using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MobileScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    GameManager gameManager;    
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMoney()
    {
        moneyText.text = GameManager.Money.ToString();
    }
}
