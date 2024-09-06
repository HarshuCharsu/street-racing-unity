using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    public Animator startAnimator;
    public Animator arrowAnimator;
    public Animator fadeOutAnimator;

    public GameObject selection1;
    public GameObject selection2;
    public GameObject selection3;
    public GameObject selection4;

    public GameObject border;
    public Button buyButton;

    private readonly Dictionary<GameManager.Cars, GameObject> _selectionsDic = new Dictionary<GameManager.Cars, GameObject>();

    private GameObject _highlightedSelection;
    private GameObject _chosenSelection;
    public TMP_Text chosenText;

    public float rotationSpeed = 60f;

    public Text tokenCountText; // Reference to your Text component

    public Button sellButton;


    private void Start()
    {
        GameManager.IsGameStarted = false;
        UIManager.SetCursorVisibility(true);

        _selectionsDic.Add(GameManager.Cars.Convertible, selection1);
        _selectionsDic.Add(GameManager.Cars.Pickup, selection2);
        _selectionsDic.Add(GameManager.Cars.Jumpy, selection3);
        _selectionsDic.Add(GameManager.Cars.SharkTruck, selection4);

        ChooseSelection(GameManager.Cars.Convertible);

        UpdateSellButton(GameManager.Instance.choosenCarType);

    }

    private void Update()
    {
        tokenCountText.text = "Tokens Remaining: " + GameManager.Instance.playerTokens;

        if (Input.anyKeyDown)
        {
            startAnimator.SetTrigger("OnClick");
            arrowAnimator.SetBool("HasClicked", true);
        }

        if (_highlightedSelection != null)
        {
            Transform highlightedCar = _highlightedSelection.GetComponent<MenuSelection>().car;
            highlightedCar.localRotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);
        }
        else if (_chosenSelection != null)
        {
            MenuSelection menuSelection = _chosenSelection.GetComponent<MenuSelection>();
            menuSelection.car.localRotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);
        }

        if (_chosenSelection != null)
        {
            MenuSelection menuSelection = _chosenSelection.GetComponent<MenuSelection>();
            border.GetComponent<Image>().color = menuSelection.difficultyColor;
            border.transform.SetParent(_chosenSelection.transform);
            border.transform.SetAsFirstSibling();
            border.transform.position = menuSelection.borderReferencePosition.position;
        }
    }

    public void Connect()
    {
        SceneManager.LoadScene(2);
    }

    public void ChooseSelection(GameManager.Cars cRef)
    {
        _chosenSelection = _selectionsDic[cRef];

        MenuSelection menuSelection = _selectionsDic[cRef].GetComponent<MenuSelection>();
        chosenText.text = menuSelection.car.name;
        chosenText.color = menuSelection.difficultyColor;

        GameManager.Instance.choosenCarType = cRef;

        // Enable or disable buy button based on car ownership
        if (GameManager.Instance.ownedCars[cRef])
        {
            buyButton.interactable = false; // Car already owned
        }
        else
        {
            buyButton.interactable = true;  // Car not owned, allow purchase
        }

        UpdateSellButton(cRef);
    }

    private void UpdateSellButton(GameManager.Cars cRef)
    {
        // Convertible cannot be sold, so disable the sell button for it
        if (cRef == GameManager.Cars.Convertible)
        {
            sellButton.interactable = false;
        }
        else
        {
            sellButton.interactable = GameManager.Instance.ownedCars[cRef];
        }
    }

    public void HighlightSelection(GameManager.Cars cRef)
    {
        _highlightedSelection = _selectionsDic[cRef];
    }

    public void RemoveHighlightedSelection()
    {
        _highlightedSelection = null;
    }

    public void BuyCar()
    {
        GameManager.Cars carType = GameManager.Instance.choosenCarType;

        // Check if player has enough tokens and car is not already owned
        if (carType.ToString() == "Pickup")
        {
            if (GameManager.Instance.playerTokens >= 25 && !GameManager.Instance.ownedCars[carType])
            {
                GameManager.Instance.playerTokens -= 25;
                GameManager.Instance.ownedCars[carType] = true;
                GameManager.Instance.choosenCarType = carType; // Update chosenCarType after purchase

                buyButton.interactable = false;
            }
        }

        else if (carType.ToString() == "Jumpy")
        {
            if (GameManager.Instance.playerTokens >= 50 && !GameManager.Instance.ownedCars[carType])
            {
                GameManager.Instance.playerTokens -= 50;
                GameManager.Instance.ownedCars[carType] = true;
                GameManager.Instance.choosenCarType = carType; // Update chosenCarType after purchase

                buyButton.interactable = false;
            }
        }

        else if (carType.ToString() == "SharkTruck")
        {
            if (GameManager.Instance.playerTokens >= 75 && !GameManager.Instance.ownedCars[carType])
            {
                GameManager.Instance.playerTokens -= 75;
                GameManager.Instance.ownedCars[carType] = true;
                GameManager.Instance.choosenCarType = carType; // Update chosenCarType after purchase

                buyButton.interactable = false;
            }
        }
    }

    public void SellCar()
    {
        GameManager.Cars carType = GameManager.Instance.choosenCarType;

        // Check if the selected car is owned and is not the default convertible
        if (GameManager.Instance.ownedCars[carType] && carType != GameManager.Cars.Convertible)
        {
            int refundAmount = 0;

            // Determine the refund amount based on the car type
            switch (carType)
            {
                case GameManager.Cars.Pickup:
                    refundAmount = 25;
                    break;
                case GameManager.Cars.Jumpy:
                    refundAmount = 50;
                    break;
                case GameManager.Cars.SharkTruck:
                    refundAmount = 75;
                    break;
            }

            // Refund the player and update ownership
            GameManager.Instance.playerTokens += refundAmount;
            GameManager.Instance.ownedCars[carType] = false;

            // After selling, disable the sell button and enable the buy button
            sellButton.interactable = false;
            buyButton.interactable = true;

            Debug.Log($"Sold {carType}, refunded {refundAmount} tokens.");
        }
        else
        {
            Debug.Log("Cannot sell the default car or an unowned car.");
        }
    }

    public void Play()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        // Check if a car is chosen (GameManager.choosenCarType is not null)
        if (GameManager.Instance.choosenCarType != GameManager.Cars.Convertible) // Convertible is default selection
        {
            // Check if the chosen car is owned
            if (!GameManager.Instance.ownedCars[GameManager.Instance.choosenCarType])
            {
                // Show a message indicating the car needs to be purchased
                Debug.Log("Please purchase the car before starting the game.");
                return;
            }
        }

        GameManager.IsGameOver = false;
        StartCoroutine(ChangeScene());
    }


    private IEnumerator ChangeScene()
    {
        fadeOutAnimator.SetTrigger("Fade");

        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(1);
    }
}
