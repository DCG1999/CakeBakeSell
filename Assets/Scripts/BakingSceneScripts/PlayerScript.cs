using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    // add graphics
    // add menu 
    // add sound
    [Header("UI References")]
    [SerializeField] private GameObject stopBtn;
    [SerializeField] private GameObject dropBtn;
    [SerializeField] private GameObject objectModeBtn;
    [SerializeField] private TextMeshProUGUI timertext;

    private GameObject itemAddingPlaceHolder;
    private GameObject whiskPlaceHolder;
    private GameObject eggPlaceHolder;


    [Header("GameObject References")]
    [SerializeField] private GameObject NonFrostedCake;
    [SerializeField] private GameObject Frosting;
    [SerializeField] private GameObject FinishedCake;

    GameObject playerMadeCake;
    GameObject PourObject;
    GameObject activeBowl;
    GameObject plateSelected = null;
    GameObject ObjectInHand = null;

    [Header("Component References")]
    [SerializeField] Animator microwaveAnimator;
    [SerializeField] Animator fridgeDoorAnimator;
    [SerializeField] private Transform objectPlaceHolder;

    [Header("Bools")]
    bool fridgeDoorOpen;
    bool PickObjectMode;
    bool ItemAddMode;
    bool StirMode;
    bool BowlMode;
    bool EquipmentInHand;
    bool MicrowaveOpen;
    bool Microwaving;
    bool BowlInMicrowave;
    bool CakeMicrowaved;
    bool EnoughFrosting;
    bool canPour;
    bool CakeInFridge;

    [Header("Range and Rates")]
    [SerializeField] private float objectDetectionRange;
    [SerializeField] private float objectRotationSpeed;
    [SerializeField] private float scalingSpeed;

    float amountPoured = 0;

    [Header("Script References")]
    FirstPersonController firstPersonController;
    GameManager gameManager;
    HUDManager hudManager;

    [Header("Misc")]
    string eggGrade = "";


    void Start()
    {
        InitializeScriptReferences();
        InitializeAllBools();

        playerMadeCake = NonFrostedCake;
    }

    void InitializeScriptReferences()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.InitializeScene(1);

        hudManager = GameObject.FindObjectOfType<HUDManager>();
        firstPersonController = GetComponent<FirstPersonController>();

       
    }
    void InitializeAllBools()
    {
        BowlInMicrowave = false;
        canPour = true;
        PickObjectMode = false;
        MicrowaveOpen = false;
        Microwaving = false;
        BowlMode = false;
        StirMode = false;
        ItemAddMode = false;
        EquipmentInHand = false;
        EnoughFrosting = false;
        CakeMicrowaved = false;
        fridgeDoorOpen = false;
        CakeInFridge = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PickObjectMode)
        {
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began)
                {
                    DetectObject(t.position); // to detect objects when tapped upon in objectMode
                }
            }
        }
    }

    void DetectObject(Vector3 _touchPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(_touchPos);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, objectDetectionRange);

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                if (hit.collider)
                {
                    if (!ItemAddMode)
                    {
                        if (hit.collider.gameObject.CompareTag("Fridge"))
                        {
                            if (!fridgeDoorOpen)
                            {
                                fridgeDoorOpen = true;
                                fridgeDoorAnimator.SetTrigger("doorOpen");
                            }
                            else
                            {
                                fridgeDoorOpen = false;
                                fridgeDoorAnimator.SetTrigger("doorClose");
                                if(CakeInFridge)
                                {
                                    CakeGrader cakeGrader = GameObject.FindObjectOfType<CakeGrader>();
                                    cakeGrader.GradeCake();
                                    firstPersonController.canMove = false;
                                    hudManager.DisplayGradeScreen();
                                }
                            }
                        }
                        if (ObjectInHand) // when user has an object in his hands
                        {
                            if (BowlMode) // when user has bowl in his hands
                            {
                                if (hit.collider.gameObject.CompareTag("Plate"))
                                {
                                    if (hit.collider.gameObject.name == "MicrowavePlate")
                                    {
                                        BowlInMicrowave = true;
                                    }
                                    else if (hit.collider.gameObject.name == "FridgePlate")
                                    {

                                         CakeInFridge = true;
                                        if(!CakeMicrowaved)
                                        {
                                            playerMadeCake = ObjectInHand;
                                            playerMadeCake.SetActive(true);
                                        }
                                        else
                                        {
                                            playerMadeCake.transform.position = hit.collider.transform.position;
                                            playerMadeCake.SetActive(true);
                                        }

                                        playerMadeCake.transform.parent = hit.collider.transform;
                                        ObjectInHand = null;
                                    }

                                    if(hit.collider.gameObject.name != "FridgePlate")
                                    {
                                        plateSelected = hit.collider.gameObject;
                                        DropObject();
                                        dropBtn.SetActive(true);
                                    }
                                }

                                if (MicrowaveOpen)
                                {
                                    if (hit.collider.gameObject.name == "MicrowaveBowlPlaceholder")
                                    {
                                        if (ObjectInHand.name == "Bowl1")
                                        {
                                            ObjectInHand.transform.position = hit.transform.position;
                                            plateSelected = hit.collider.gameObject;
                                            DropObject();
                                        }
                                    }
                                }
                            }
                            if (hit.collider.gameObject.CompareTag("Bowl"))
                            {
                                if (hit.collider.gameObject.name != ObjectInHand.name)
                                {
                                    activeBowl = hit.collider.gameObject;
                                    if (ObjectInHand.name == "Egg")
                                    {
                                        DropEgg();
                                    }
                                    else { AddItemToMixture(); }


                                }
                            }
                        }

                        else if (!ObjectInHand && !BowlMode) // when hand is empty
                        {
                            if (hit.collider.gameObject.CompareTag("Item"))
                            {
                                if (hit.collider.gameObject.name == "Eggs") // exception for interaction with egg
                                {
                                    ObjectInHand = hit.collider.gameObject;
                                    if (eggGrade == "")
                                    {
                                        eggGrade = ObjectInHand.GetComponent<BakingGroceryScript>().BakeItem.itemGrade.ToString();
                                    }
                                    ObjectInHand = ObjectInHand.GetComponent<EggScript>().GetEgg();
                                    PickUpEgg();

                                }
                                else
                                {
                                    ObjectInHand = hit.collider.gameObject;
                                    PickUpObject(1);
                                    PourObject = ObjectInHand.transform.Find("PourSource").gameObject;
                                }
                            }

                            else if (hit.collider.gameObject.CompareTag("Equipment"))
                            {
                                EquipmentInHand = true;
                                ObjectInHand = hit.collider.gameObject;
                                PickUpObject(2);
                            }

                            else if (hit.collider.gameObject.CompareTag("Bowl"))
                            {

                                BowlInMicrowave = false;
                                BowlMode = true;
                                ObjectInHand = hit.collider.gameObject;
                                PickUpObject(3);
                                PourObject = ObjectInHand.transform.Find("PourSource").gameObject;
                                dropBtn.SetActive(false);

                            }
                        }
                    }
                }
            }
        }

    }


    void PickUpEgg()
    {
        dropBtn.SetActive(false);

        ObjectInHand.transform.Find("UncrackedEgg").gameObject.SetActive(true);
        ObjectInHand.transform.parent = objectPlaceHolder.transform;
        ObjectInHand.transform.localPosition = Vector3.zero;
        ObjectInHand.transform.localEulerAngles = Vector3.zero;
        ObjectInHand.GetComponent<Rigidbody>().isKinematic = true;
        ObjectInHand.GetComponent<Rigidbody>().useGravity = false;
    }

    void DropEgg()
    {
        dropBtn.SetActive(true);
        eggPlaceHolder = activeBowl.transform.Find("EggAddingPlaceholder").gameObject;
        ObjectInHand.transform.Find("CrackedEgg").gameObject.SetActive(true);
        ObjectInHand.transform.Find("UncrackedEgg").gameObject.SetActive(false);

        ObjectInHand.transform.parent = null;
        ObjectInHand.transform.position = eggPlaceHolder.transform.position;
        ObjectInHand.GetComponent<Rigidbody>().isKinematic = false;
        ObjectInHand.GetComponent<Rigidbody>().useGravity = true;

        activeBowl.GetComponent<BowlScript>().AddToBowl(activeBowl, ObjectInHand.name, "1", eggGrade);
        activeBowl = null;
        ObjectInHand = null;
    }
    void PickUpObject(int itemType)
    {
        //1 is Item 
        //2 is Equipment 
        // 3 for bowl
        if (itemType != 3)
        {
            ObjectInHand.GetComponent<Rigidbody>().isKinematic = true;
            ObjectInHand.GetComponent<Rigidbody>().useGravity = false;

            if (itemType == 1)
            {
                hudManager.ShowDisplays(ObjectInHand, "grocery", true);
            }
        }
        else
        {
            hudManager.ShowDisplays(ObjectInHand, "bowl", true);
        }
        SetObjectinHand(itemType);
    }

    void SetObjectinHand(int _itemType)
    {
        ObjectInHand.transform.parent = objectPlaceHolder;
        ObjectInHand.transform.localPosition = Vector3.zero;
        if (_itemType == 1)
        {
            ObjectInHand.transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        else if (_itemType == 2)
        {
            ObjectInHand.transform.localEulerAngles = new Vector3(0, 0, 180);
        }

    }

    void AddItemToMixture()
    {
        dropBtn.SetActive(false);
        firstPersonController.canMove = false;
        stopBtn.SetActive(true);

        if (!EquipmentInHand)
        {
            if (!BowlMode)
            {
                BakingGroceryScript bakingGroceryScript = ObjectInHand.GetComponent<BakingGroceryScript>();
                hudManager.UpdateQuantity(bakingGroceryScript.tempAmountPoured + bakingGroceryScript.amountPoured);
            }

            else
            {
                BowlScript bowlScript = ObjectInHand.GetComponent<BowlScript>();
                hudManager.UpdateQuantity(bowlScript.currentQuantity);
            }
            ItemAddMode = true;
            itemAddingPlaceHolder = activeBowl.transform.Find("ItemAddingPlaceholder").gameObject;
            ObjectInHand.transform.parent = itemAddingPlaceHolder.transform;
            ObjectInHand.transform.localPosition = Vector3.zero;
            ObjectInHand.transform.localEulerAngles = new Vector3(0, 180, 0);
            StartCoroutine(InitiatePouring());
        }

        else
        {
            StirMode = true;
            whiskPlaceHolder = activeBowl.transform.Find("WhiskPlaceholder").gameObject;
            ObjectInHand.transform.parent = whiskPlaceHolder.transform;
            ObjectInHand.transform.localPosition = Vector3.zero;
            ObjectInHand.transform.localEulerAngles = Vector3.zero;
            StartCoroutine((UseWhisk()));
        }

    }


    IEnumerator InitiatePouring()
    {
        Vector3 ZRot = Vector3.zero;
        Touch touch;
        Vector3 localRot = Vector3.zero;

        amountPoured = 0;
        canPour = true;


        BakingGroceryScript bakingGroceryScript = ObjectInHand.GetComponent<BakingGroceryScript>();

        BowlScript bowlScript = activeBowl.GetComponent<BowlScript>();
        while (true)
        {
            if (ItemAddMode) // disable the button when adding
            {
                PourItem(IsPouring(), true);

                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved)
                    {
                        CalculateItemRotation(touch, ZRot, localRot);
                    }
                }
            }
            else { break; }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }


    void ScaleCake(GameObject cake, float yScale, float _scalingSpeed)
    {
        Vector3 newScale = new Vector3();
        newScale.x = Mathf.Clamp((NonFrostedCake.transform.localScale.x + _scalingSpeed), 0f, 1.4f);
        newScale.y = Mathf.Clamp((NonFrostedCake.transform.localScale.y + _scalingSpeed), 0f, yScale);
        newScale.z = Mathf.Clamp((NonFrostedCake.transform.localScale.z + _scalingSpeed), 0f, 1.4f);
        cake.transform.localScale = newScale; 
    }
    IEnumerator UseWhisk()
    {

        // Destroy eggs in bowl when stirring
        Touch touch;
        Vector3 whiskPos = ObjectInHand.transform.localPosition;
        Vector3 whiskTempPos;

        GameObject[] eggsInBowl = GameObject.FindGameObjectsWithTag("Egg");
        if (eggsInBowl.Length != 0)
        {
            foreach (GameObject egg in eggsInBowl)
            {
                GameObject.Destroy(egg);
            }
        }
        while (true)
        {
            if (StirMode)
            {
                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Moved)
                    {
                        whiskTempPos = new Vector3(touch.deltaPosition.x * 0.02f, 0, touch.deltaPosition.y * 0.02f);
                        whiskPos += whiskTempPos;
                        ObjectInHand.transform.localPosition = whiskPos;
                    }

                }
            }
            else { break; }

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator StartMicrowave()
    {
        int timer = 20;
        timertext.gameObject.SetActive(true);
        timertext.text = timer.ToString();
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);

            timer--;
            timertext.text = timer.ToString();

        }
        CakeMicrowaved = true;
        timertext.text = "Done";
        yield return null;
    }
    void CalculateItemRotation(Touch _touch, Vector3 _rot, Vector3 _localRot)
    {
        _rot = new Vector3(0, 0, _touch.deltaPosition.y * objectRotationSpeed);
        _localRot = _rot + ObjectInHand.transform.localEulerAngles;
        ObjectInHand.transform.eulerAngles = _localRot;
    }

    bool IsPouring()
    {
        if (!BowlMode)
        {
            if (ObjectInHand.transform.localEulerAngles.z > 130 && ObjectInHand.transform.localEulerAngles.z < 170 && canPour) { return true; }
            else { return false; }
        }
        else
        {
            if (ObjectInHand.transform.localEulerAngles.z > 80 && ObjectInHand.transform.localEulerAngles.z < 120 && canPour) { return true; }
            else { return false; }
        }
    }

    void PourItem(bool pourState, bool interactionMode)
    {
        var em = PourObject.GetComponent<ParticleSystem>().emission;
        em.enabled = pourState;

        if (interactionMode && pourState)
        {
            if (!BowlMode)
            {
                BakingGroceryScript bakingGroceryScript = ObjectInHand.GetComponent<BakingGroceryScript>();
                amountPoured = bakingGroceryScript.tempAmountPoured + bakingGroceryScript.amountPoured;
                if (amountPoured >= bakingGroceryScript.maxCapacity) { canPour = false; }
                else { canPour = true; }

                bakingGroceryScript.tempAmountPoured += ObjectInHand.transform.eulerAngles.z / 130 * bakingGroceryScript.pourRate * Time.deltaTime;

                if (activeBowl.name == "Bowl1")
                {
                    ScaleCake(NonFrostedCake, 1.4f, scalingSpeed);
                }
                else if(activeBowl.name == "Bowl2")
                {
                    ScaleCake(Frosting,1f, scalingSpeed);
                }
                hudManager.UpdateQuantity(amountPoured);
            }
            else
            {
                BowlScript bowlScript = ObjectInHand.GetComponent<BowlScript>();
                bowlScript.currentQuantity -= ObjectInHand.transform.eulerAngles.z / 130 * 25f * Time.deltaTime;
                if (bowlScript.currentQuantity <= 0)
                {
                    canPour = false;
                    if (EnoughFrosting)
                    {
                        playerMadeCake = FinishedCake;
                        playerMadeCake.SetActive(true);
                    }
                }
                else
                {
                    ScaleCake(Frosting,1f,-scalingSpeed);
                    canPour = true;
                }
                hudManager.UpdateQuantity(bowlScript.currentQuantity);
            }
        }

    }
    public void StopInteractingWithBowl()
    {
        BowlScript bowlScript = activeBowl.GetComponent<BowlScript>();

        if (ItemAddMode)
        {
            ItemAddMode = false;

            StopCoroutine(InitiatePouring());

            if (!BowlMode)
            {
                BakingGroceryScript bakingGroceryScript = ObjectInHand.GetComponent<BakingGroceryScript>();
                bowlScript.AddToBowl(activeBowl, ObjectInHand.name, Mathf.Round(bakingGroceryScript.tempAmountPoured).ToString(), bakingGroceryScript.BakeItem.itemGrade.ToString());
                bakingGroceryScript.amountPoured += bakingGroceryScript.tempAmountPoured;
                bakingGroceryScript.BakeItem.currentQuantity -= bakingGroceryScript.tempAmountPoured;
                bowlScript.currentQuantity += bakingGroceryScript.tempAmountPoured;
                bakingGroceryScript.tempAmountPoured = 0;
                if (activeBowl.name == "Bowl2")
                {
                    EnoughFrosting = bowlScript.CheckMinReqForFrostingCake();
                }
                hudManager.ShowDisplays(ObjectInHand, "grocery", true);
                SetObjectinHand(1);
            }
            else
            {

                hudManager.ShowDisplays(ObjectInHand, "bowl", true);
                SetObjectinHand(3);
            }
;
            PourItem(false, false);


        }

        else if (StirMode)
        {
            bowlScript.AddToBowl(activeBowl, ObjectInHand.name, "Mix", "");
            StirMode = false;
            StopCoroutine(UseWhisk());

            SetObjectinHand(2);
        }

        activeBowl = null;
        firstPersonController.canMove = true;
        stopBtn.SetActive(false);
        dropBtn.SetActive(true);
    }

    public void DropObject()
    {

        if (ObjectInHand != null)
        {

            if (!BowlMode)
            {
                if (!EquipmentInHand)
                {
                    hudManager.ShowDisplays(ObjectInHand, "grocery", false);
                }
                else
                {
                    EquipmentInHand = false;
                }

                ObjectInHand.GetComponent<Rigidbody>().isKinematic = false;
                ObjectInHand.GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                ObjectInHand.transform.position = plateSelected.transform.position;
                ObjectInHand.transform.eulerAngles = Vector3.zero;
                plateSelected = null;
                BowlMode = false;
            }
            ObjectInHand.transform.parent = null;
            ObjectInHand = null;
        }

    }

    public void ObjectModeToggle(bool _toggle)
    {
        PickObjectMode = _toggle;
    }
    public void MicrowaveDoor(string _op)
    {
        if (!Microwaving)
        {
            if (_op == "open")
            {
                microwaveAnimator.SetTrigger("openDoor");
                MicrowaveOpen = true;
            }
            else if (_op == "close")
            {
                microwaveAnimator.SetTrigger("closeDoor");
                MicrowaveOpen = false;
                if (BowlInMicrowave)
                {
                    StartCoroutine(StartMicrowave());
                }
                else
                {
                    timertext.text = "";
                }

            }
        }
    }

    public void EndScene()
    {
        gameManager.NextScene();
    }
}
