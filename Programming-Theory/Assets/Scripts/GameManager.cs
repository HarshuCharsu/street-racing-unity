using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Cars { Convertible, Pickup, Jumpy, SharkTruck }

    [System.Serializable]
    public class HighscoreData
    {
        public System.TimeSpan convertibleScore;
        public System.TimeSpan pickupScore;
        public System.TimeSpan jumpyScore;
        public System.TimeSpan sharkScore;
    }

    public static GameManager Instance { get; private set; } // ENCAPSULATION
    public static bool IsGameOver { get; set; } // ENCAPSULATION
    public static bool IsGameStarted { get; set; } // ENCAPSULATION

    public Cars choosenCarType;
    public GameObject player;
    public HighscoreData highscoreData = new HighscoreData();

    // New additions for car ownership and tokens
    public Dictionary<Cars, bool> ownedCars = new Dictionary<Cars, bool>();
    public int playerTokens = 100; // Default token count

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize car ownership
        InitializeCarOwnership();
    }

    private void InitializeCarOwnership()
    {
        // Convertible is owned by default, others need to be purchased
        ownedCars[Cars.Convertible] = true;
        ownedCars[Cars.Pickup] = false;
        ownedCars[Cars.Jumpy] = false;
        ownedCars[Cars.SharkTruck] = false;
    }

    public bool IsSelectedVehicle(Cars cRef)
    { // ABSTRACTION
        return cRef == choosenCarType;
    }

    private bool IsScoreLowerThan(System.TimeSpan score1, System.TimeSpan score2)
    { // ABSTRACTION
        return score1 < score2;
    }

    public void SaveHighscore(System.TimeSpan score, Cars cRef)
    { // ABSTRACTION
        HighscoreData data = highscoreData;

        switch (cRef)
        {
            case Cars.Convertible:
                if (highscoreData.convertibleScore == new System.TimeSpan() ||
                    IsScoreLowerThan(score, highscoreData.convertibleScore))
                    data.convertibleScore = score;
                break;
            case Cars.Pickup:
                if (highscoreData.pickupScore == new System.TimeSpan() ||
                    IsScoreLowerThan(score, highscoreData.pickupScore))
                    data.pickupScore = score;
                break;
            case Cars.Jumpy:
                if (highscoreData.jumpyScore == new System.TimeSpan() ||
                    IsScoreLowerThan(score, highscoreData.jumpyScore))
                    data.jumpyScore = score;
                break;
            case Cars.SharkTruck:
                if (highscoreData.sharkScore == new System.TimeSpan() ||
                    IsScoreLowerThan(score, highscoreData.sharkScore))
                    data.sharkScore = score;
                break;
        }

        highscoreData = data;
    }


}
