using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class GameDataControl : MonoBehaviour {

    public static GameDataControl gdControl;

    [Header("Gold Coins")]
    public int coinsEarned;
    public int coinsSpent;
    public int coinsTotal;

    [Header("Level Info")]
    public List<int> blocksUnlocked;
    public List<List<int>> all_level_results;
    public List<List<int>> blockSecretsUnlocked;
    public bool hardUnlocked;
    public bool extraBlock1Unlocked;
    public List<int> daily_level_results;
    public List<int> daily_easyLevel_Indexes;
    public List<int> daily_hardLevel_Indexes;
    public List<int> extra_level_block_1_results;
    public int profile_puzzlesSolved;

    [Header("Cosmetics")]
    public List<int> themes;


    private bool resetLoaded = false;

    private void Awake() {
        //PlayerPrefs.DeleteAll();
        //Run this on first time ever opening game.
        int firstTime = PlayerPrefs.GetInt("firstTime", 0);
        if (firstTime == 0) {
            PlayerPrefs.SetInt("firstTime", 20);
            Debug.Log("First Time Opening Game!");

            ResetPlayerData();
            resetLoaded = true;
        }

        if (gdControl == null) {
            DontDestroyOnLoad(gameObject);
            gdControl = this;
            if (!resetLoaded) {
                LoadPlayerData();
            }
        }
        else if (gdControl != this) {
            Destroy(gameObject);
        }
    }

    #region Testing

    private void Start() {
        //PlayerPrefs.DeleteAll();
        Debug.Log(Application.persistentDataPath);

        //string testString = "27755x1975x25780x2x1x5x13x549x111x111000000000100000001x1111111111x1000000000x0100000000x0000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000000x00000000000000000010x1111v1v2v3v4x1101v1v4v0v10x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x0000v0v0v0v0x1111v5v10v15v20x1x10000000100000000001";
        //LoadPlayFabStringIntoGameData(testString);
    }

    #endregion

    void OnApplicationQuit() {
        SetPlayFabGameDataAndStatistics();
    }

    void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus) {
            SetPlayFabGameDataAndStatistics();
        }
    }

    #region Main 3 Save,Load,Reset
    public void SavePlayerData() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/bluePathPlayerData2.dat");

        PlayerData data = new PlayerData();

        data.coinsEarned = coinsEarned;
        data.coinsSpent = coinsSpent;
        data.coinsTotal = coinsTotal;
        data.blocksUnlocked = blocksUnlocked;
        data.all_level_results = all_level_results;
        data.blockSecretsUnlocked = blockSecretsUnlocked;
        data.hardUnlocked = hardUnlocked;
        data.extraBlock1Unlocked = extraBlock1Unlocked;
        data.daily_level_results = daily_level_results;
        data.daily_easyLevel_Indexes = daily_easyLevel_Indexes;
        data.daily_hardLevel_Indexes = daily_hardLevel_Indexes;
        data.extra_level_block_1_results = extra_level_block_1_results;
        data.profile_puzzlesSolved = profile_puzzlesSolved;
        data.themes = themes;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Saved Player Data To Local File");
        //SavePlayFabStats();
        //SetPlayFabGameData();
    }

    public void LoadPlayerData() {
        if (File.Exists(Application.persistentDataPath + "/bluePathPlayerData2.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/bluePathPlayerData2.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();


            if ( data.themes == null) {
                AddingCosmeticData(data);
                bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + "/bluePathPlayerData2.dat", FileMode.Open);
                data = (PlayerData)bf.Deserialize(file);
                file.Close();
            }


            coinsEarned = data.coinsEarned;
            coinsSpent = data.coinsSpent;
            coinsTotal = data.coinsTotal;
            blocksUnlocked = data.blocksUnlocked;
            all_level_results = data.all_level_results;
            blockSecretsUnlocked = data.blockSecretsUnlocked;
            hardUnlocked = data.hardUnlocked;
            extraBlock1Unlocked = data.extraBlock1Unlocked;
            daily_level_results = data.daily_level_results;
            daily_easyLevel_Indexes = data.daily_easyLevel_Indexes;
            daily_hardLevel_Indexes = data.daily_hardLevel_Indexes;
            extra_level_block_1_results = data.extra_level_block_1_results;
            profile_puzzlesSolved = data.profile_puzzlesSolved;
            themes=data.themes;
            
            Debug.Log("Loaded Player Data from Local File");

        }
        else {
            Debug.Log("No Player Data found re-creating Player Data");
            ResetPlayerData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //PrintAllLevelResults();
            //PrintSecretResults();
        }
    }

    public void ResetPlayerData() {
        //PlayerPrefs.DeleteAll();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/bluePathPlayerData2.dat");
        //Debug.Log(Application.persistentDataPath);
        PlayerData data = new PlayerData();

        data.coinsEarned = 0;
        data.coinsSpent = 0;
        data.coinsTotal = 0;
        data.blocksUnlocked = new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        data.all_level_results = new List<List<int>> {
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }};
        data.blockSecretsUnlocked = new List<List<int>> {
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 },
            new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 }};
        data.hardUnlocked = false;
        data.extraBlock1Unlocked = false;
        data.daily_level_results = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        data.daily_easyLevel_Indexes = new List<int> { 0, 0, 0, 0, 0 };
        data.daily_hardLevel_Indexes = new List<int> { 0, 0, 0, 0, 0 };
        data.extra_level_block_1_results = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        data.profile_puzzlesSolved = 0;

        data.themes = new List<int> { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


        bf.Serialize(file, data);
        file.Close();

        Debug.Log("Reset Player Data in Local File");

        LoadPlayerData();
    }


    private void AddingCosmeticData(PlayerData oldData) {
        Debug.Log("No Cosmetic Data, Adding Cosmetic Data");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/bluePathPlayerData2.dat");

        PlayerData data = new PlayerData();

        //making sure we have the old data
        data.coinsEarned = oldData.coinsEarned;
        data.coinsSpent = oldData.coinsSpent;
        data.coinsTotal = oldData.coinsTotal;
        data.blocksUnlocked = oldData.blocksUnlocked;
        data.all_level_results = oldData.all_level_results;
        data.blockSecretsUnlocked = oldData.blockSecretsUnlocked;
        data.hardUnlocked = oldData.hardUnlocked;
        data.extraBlock1Unlocked = oldData.extraBlock1Unlocked;
        data.daily_level_results = oldData.daily_level_results;
        data.daily_easyLevel_Indexes = oldData.daily_easyLevel_Indexes;
        data.daily_hardLevel_Indexes = oldData.daily_hardLevel_Indexes;
        data.extra_level_block_1_results = oldData.extra_level_block_1_results;
        data.profile_puzzlesSolved = oldData.profile_puzzlesSolved;

        //adding new cosmetic data
        data.themes = new List<int> { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        bf.Serialize(file, data);
        file.Close();
    }



    #endregion

    #region Printing Functions
    public void PrintSecretResults() {
        int blocksIndex = gdControl.blockSecretsUnlocked.Count;
        for (int i = 0; i < blocksIndex; i++) {
            int firstSecret = gdControl.blockSecretsUnlocked[i][0];
            int secondSecret = gdControl.blockSecretsUnlocked[i][1];
            int thirdSecret = gdControl.blockSecretsUnlocked[i][2];
            int fourthSecret = gdControl.blockSecretsUnlocked[i][3];
            int firstLevel = gdControl.blockSecretsUnlocked[i][4];
            int secondLevel = gdControl.blockSecretsUnlocked[i][5];
            int thirdLevel = gdControl.blockSecretsUnlocked[i][6];
            int fourthLevel = gdControl.blockSecretsUnlocked[i][7];
            Debug.Log("Block " + i + " Secret Results " + firstSecret + " " + secondSecret + " " + thirdSecret + " " + fourthSecret + " " + firstLevel + " " + secondLevel + " " + thirdLevel + " " + fourthLevel);
        }
    }

    private void PrintAllLevelResults() {
        int blocksIndex = gdControl.all_level_results.Count;
        for (int i = 0; i < blocksIndex; i++) {
            string lvlResultsString = "";
            for (int x = 0; x < all_level_results[i].Count; x++) {
                lvlResultsString += all_level_results[i][x].ToString();
            }
            Debug.Log("Block " + i + " All Level Results " + lvlResultsString);
        }
    }
    #endregion

    #region Counting Functions
    public List<int> GetFullSecretsUnlocked(){
        List<int> SecretsUnlocked = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        int blocksIndex = gdControl.blockSecretsUnlocked.Count;
        for (int i = 0; i < blocksIndex; i++){
            int firstSecret = gdControl.blockSecretsUnlocked[i][0];
            int secondSecret = gdControl.blockSecretsUnlocked[i][1];
            int thirdSecret = gdControl.blockSecretsUnlocked[i][2];
            int fourthSecret = gdControl.blockSecretsUnlocked[i][3];
            if (firstSecret == 1 && secondSecret == 1 && thirdSecret == 1 && fourthSecret == 1){
                SecretsUnlocked[i] = 1;
            }
        }

        return SecretsUnlocked;
    }


    public void AddCoins(int ammount) {
        coinsEarned += ammount;
        coinsTotal += ammount;
    }

    public void AddPuzzleSolved() {
        profile_puzzlesSolved++;
    }
#endregion

    #region Parsing Functions
    public string GameDataListToString(List<int> nums) {
        string results = "";

        foreach (int num in nums) {
            results += num.ToString();
        }
        return results;
    }

    private int ExtraBlock1Unlocked_ToInt() {
        if (extraBlock1Unlocked) {
            return 1;
        }
        else {
            return 0;
        }
    }

    private bool ExtraBlock1Unlocked_ToBool(int num) {
        if (num == 0) {
            return false;
        }
        else {
            return true;
        }
    }

    public string GameDataFlowerListToString(List<int> nums) {
        string results = "";

        for (int i = 0; i< nums.Count; i++) {

            if (i < 4) {
                results += nums[i].ToString();
            }
            else {
                results += "v" + nums[i].ToString();
            }        
        }

        return results;
    }


    public string GameDataExtraBlockUnlockedToString() {
        if (extraBlock1Unlocked) {
            return "1";
        }
        else {
            return "0";
        }
    }

    public void SetTutorialAndAds(string nums) {

        PlayerPrefs.SetInt("MainMenuTutorial", (int)Char.GetNumericValue(nums[0]));
        PlayerPrefs.SetInt("FirstTutorialComplete", (int)Char.GetNumericValue(nums[1]));
        PlayerPrefs.SetInt("no_ads", (int)Char.GetNumericValue(nums[2]));

    }

    public List<int> ParsedStringIntoIntList(string numbers) {
        List<int> results = new List<int>();

        foreach(char num in numbers) {
            results.Add((int)Char.GetNumericValue(num));
        }

        return results;
    }

    public bool ParsedStringToBool(string number) {
        bool results = false;
        if (number == "1") {
            results = true;
        }
        return results;
    }

    public List<int> ParsedSecretStringIntoIntList(string numbers) {
        List<int> results = new List<int>();
        string[] numbersArray = numbers.Split('v');
        foreach (char num in numbersArray[0]) {
            results.Add((int)Char.GetNumericValue(num));
        }
        results.Add(Int32.Parse(numbersArray[1]));
        results.Add(Int32.Parse(numbersArray[2]));
        results.Add(Int32.Parse(numbersArray[3]));
        results.Add(Int32.Parse(numbersArray[4]));

        return results;
    }

    public string MakeLocalGameDataString() {
        string gamedata_string = "";

        gamedata_string += coinsEarned; //0
        gamedata_string += "x";
        gamedata_string += coinsSpent;
        gamedata_string += "x";
        gamedata_string += coinsTotal;
        gamedata_string += "x";
        gamedata_string += PlayerPrefs.GetInt("Times_Purchased_Gold_500", 0);
        gamedata_string += "x";
        gamedata_string += PlayerPrefs.GetInt("Times_Purchased_Gold_1500", 0);
        gamedata_string += "x";
        gamedata_string += PlayerPrefs.GetInt("Times_Purchased_Gold_5000", 0);
        gamedata_string += "x";
        gamedata_string += profile_puzzlesSolved;
        gamedata_string += "x";
        gamedata_string += Convert.ToInt32(PlayTimerMechanics.instance.currentTimePlayed);
        gamedata_string += "x";
        gamedata_string += PlayerPrefs.GetInt("MainMenuTutorial", 0).ToString() + PlayerPrefs.GetInt("FirstTutorialComplete", 0).ToString() + PlayerPrefs.GetInt("no_ads", 0).ToString();
        gamedata_string += "x";
        gamedata_string += GameDataListToString(blocksUnlocked);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[0]); //10
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[1]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[2]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[3]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[4]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[5]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[6]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[7]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[8]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[9]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[10]); //20
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[11]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[12]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[13]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[14]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[15]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[16]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[17]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[18]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[19]);
        gamedata_string += "x";
        gamedata_string += GameDataListToString(all_level_results[20]); //30
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[0]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[1]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[2]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[3]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[4]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[5]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[6]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[7]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[8]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[9]); //40
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[10]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[11]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[12]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[13]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[14]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[15]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[16]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[17]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[18]);
        gamedata_string += "x";
        gamedata_string += GameDataFlowerListToString(blockSecretsUnlocked[19]); //50
        gamedata_string += "x";
        gamedata_string += GameDataExtraBlockUnlockedToString();
        gamedata_string += "x";
        gamedata_string += GameDataListToString(extra_level_block_1_results);
        gamedata_string += "x";
        gamedata_string += "PMusernamePM";
        gamedata_string += GetUserName();
        gamedata_string += "PMusernamePM";
        gamedata_string += GameDataListToString(themes);

        return gamedata_string;
    }

    private string GetUserName() {
        string result = "";
        if (PlayerPrefs.HasKey("USERNAME")) {
            result = PlayerPrefs.GetString("USERNAME");
        }
        else {
            result = "none";
        }
        return result;
    }

    public void CheckGameDataRetreived(string playfabString) {
        string localString = MakeLocalGameDataString();
        if (playfabString != localString) {
            Debug.Log("Retreived Data from Playfabs is different");
            SyncPlayFabAndLocalGameDataThenLoad(playfabString, localString);
        }
        else {
            Debug.Log("Retreived Data from Playfabs is the same as local.");
        }

    }

    public void SyncPlayFabAndLocalGameDataThenLoad(string playfabString, string localString) {

        Debug.Log("Syncing Data from Playfabs and Local");

        string[] playfabArray = playfabString.Split('x');
        string[] localArray = localString.Split('x');


        int playfabCoinsEarned = Int32.Parse(playfabArray[0]);
        int localCoinsEarned = Int32.Parse(localArray[0]);

        //whoever has more coinsEarned
        if (playfabCoinsEarned > localCoinsEarned) {
            Debug.Log("PlayFabs has more coins earned");
            LoadStringIntoGameData(playfabString);
        }
        else if (localCoinsEarned > playfabCoinsEarned) {
            Debug.Log("Local has more coins earned");
        }
        //else whoever has more time played
        else {
            int playfabTime = Int32.Parse(playfabArray[7]);
            int localTime = Int32.Parse(localArray[7]);

            if (playfabTime > localTime) {
                Debug.Log("Playfabs has more time played");
                LoadStringIntoGameData(playfabString);
            }
            else if (localTime > playfabTime) {
                Debug.Log("Local has more time played");
            }
            //if they have the same earned coins and time played then keep local
            else {
                Debug.Log("Playfabs and Local have the same ammount of coins earned and time played keeping local data");
            }
        }
        
    }


    public void LoadStringIntoGameData(string playfabString) {
        
        string[] gameDataArray = playfabString.Split('x');


        coinsEarned = Int32.Parse(gameDataArray[0]);
        coinsSpent = Int32.Parse(gameDataArray[1]);
        coinsTotal = Int32.Parse(gameDataArray[2]);
        PlayerPrefs.SetInt("Times_Purchased_Gold_500", Int32.Parse(gameDataArray[3]));
        PlayerPrefs.SetInt("Times_Purchased_Gold_1500", Int32.Parse(gameDataArray[4]));
        PlayerPrefs.SetInt("Times_Purchased_Gold_5000", Int32.Parse(gameDataArray[5]));
        profile_puzzlesSolved = Int32.Parse(gameDataArray[6]);
        
        PlayTimerMechanics.instance.UpdateTimer((Int32.Parse(gameDataArray[7])));

        SetTutorialAndAds(gameDataArray[8]);
        blocksUnlocked = ParsedStringIntoIntList(gameDataArray[9]);
        all_level_results[0] = ParsedStringIntoIntList(gameDataArray[10]);
        all_level_results[1] = ParsedStringIntoIntList(gameDataArray[11]);
        all_level_results[2] = ParsedStringIntoIntList(gameDataArray[12]);
        all_level_results[3] = ParsedStringIntoIntList(gameDataArray[13]);
        all_level_results[4] = ParsedStringIntoIntList(gameDataArray[14]);
        all_level_results[5] = ParsedStringIntoIntList(gameDataArray[15]);
        all_level_results[6] = ParsedStringIntoIntList(gameDataArray[16]);
        all_level_results[7] = ParsedStringIntoIntList(gameDataArray[17]);
        all_level_results[8] = ParsedStringIntoIntList(gameDataArray[18]);
        all_level_results[9] = ParsedStringIntoIntList(gameDataArray[19]);
        all_level_results[10] = ParsedStringIntoIntList(gameDataArray[20]);
        all_level_results[11] = ParsedStringIntoIntList(gameDataArray[21]);
        all_level_results[12] = ParsedStringIntoIntList(gameDataArray[22]);
        all_level_results[13] = ParsedStringIntoIntList(gameDataArray[23]);
        all_level_results[14] = ParsedStringIntoIntList(gameDataArray[24]);
        all_level_results[15] = ParsedStringIntoIntList(gameDataArray[25]);
        all_level_results[16] = ParsedStringIntoIntList(gameDataArray[26]);
        all_level_results[17] = ParsedStringIntoIntList(gameDataArray[27]);
        all_level_results[18] = ParsedStringIntoIntList(gameDataArray[28]);
        all_level_results[19] = ParsedStringIntoIntList(gameDataArray[29]);
        all_level_results[20] = ParsedStringIntoIntList(gameDataArray[30]);
        blockSecretsUnlocked[0] = ParsedSecretStringIntoIntList(gameDataArray[31]);
        blockSecretsUnlocked[1] = ParsedSecretStringIntoIntList(gameDataArray[32]);
        blockSecretsUnlocked[2] = ParsedSecretStringIntoIntList(gameDataArray[33]);
        blockSecretsUnlocked[3] = ParsedSecretStringIntoIntList(gameDataArray[34]);
        blockSecretsUnlocked[4] = ParsedSecretStringIntoIntList(gameDataArray[35]);
        blockSecretsUnlocked[5] = ParsedSecretStringIntoIntList(gameDataArray[36]);
        blockSecretsUnlocked[6] = ParsedSecretStringIntoIntList(gameDataArray[37]);
        blockSecretsUnlocked[7] = ParsedSecretStringIntoIntList(gameDataArray[38]);
        blockSecretsUnlocked[8] = ParsedSecretStringIntoIntList(gameDataArray[39]);
        blockSecretsUnlocked[9] = ParsedSecretStringIntoIntList(gameDataArray[40]);
        blockSecretsUnlocked[10] = ParsedSecretStringIntoIntList(gameDataArray[41]);
        blockSecretsUnlocked[11] = ParsedSecretStringIntoIntList(gameDataArray[42]);
        blockSecretsUnlocked[12] = ParsedSecretStringIntoIntList(gameDataArray[43]);
        blockSecretsUnlocked[13] = ParsedSecretStringIntoIntList(gameDataArray[44]);
        blockSecretsUnlocked[14] = ParsedSecretStringIntoIntList(gameDataArray[45]);
        blockSecretsUnlocked[15] = ParsedSecretStringIntoIntList(gameDataArray[46]);
        blockSecretsUnlocked[16] = ParsedSecretStringIntoIntList(gameDataArray[47]);
        blockSecretsUnlocked[17] = ParsedSecretStringIntoIntList(gameDataArray[48]);
        blockSecretsUnlocked[18] = ParsedSecretStringIntoIntList(gameDataArray[49]);
        blockSecretsUnlocked[19] = ParsedSecretStringIntoIntList(gameDataArray[50]);
        extraBlock1Unlocked = ParsedStringToBool(gameDataArray[51]);
        extra_level_block_1_results = ParsedStringIntoIntList(gameDataArray[52]);

        if (playfabString.Contains("PMusernamePM")) {
            string[] moreGameData = playfabString.Split(new string[] { "PMusernamePM" }, System.StringSplitOptions.None);

            if (moreGameData[1] != "none") {
                PlayerPrefs.SetString("USERNAME", moreGameData[1]);
            }

            themes = ParsedStringIntoIntList(moreGameData[2]);//12000000000000000000
            
            //We are changin game data so we need to reload the theme manager.
            ThemeManager.TM.GetActiveThemeFromGameData();
        }
        
        

        SavePlayerData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region Get Data From PlayFabs
    public void GetPlayFabGameDataAndSync() {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = PlayFabControler.PFC.myID,
            Keys = null
        }, GetPlayFabDataAndSyncSuccess, OnErrorShared);
    }

    private void GetPlayFabDataAndSyncSuccess(GetUserDataResult result) {
        if(result.Data == null || !result.Data.ContainsKey("PM_GameData")) {
            Debug.Log("No Pocket Mazes Game Data on Playfab");
        }
        else {
            if (!PlayerPrefs.HasKey("_timer")) {
                PlayerPrefs.SetString("_timer", "Standby");
            }

            Debug.Log("Retreived Pocket Mazes GameData from Playfab");
            CheckGameDataRetreived(result.Data["PM_GameData"].Value);
        }
    }
    
    public void GetPlayFabGameDataAndLoad() {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = PlayFabControler.PFC.myID,
            Keys = null
        }, GetPlayFabDataAndLoadSuccess, OnErrorShared);
    }

    public void GetPlayFabDataAndLoadSuccess(GetUserDataResult result) {
        if (result.Data == null || !result.Data.ContainsKey("PM_GameData")) {
            Debug.Log("No Pocket Mazes Game Data on Playfab");
        }
        else {
            Debug.Log("Retreived Pocket Mazes GameData from Playfab");
            LoadStringIntoGameData(result.Data["PM_GameData"].Value);
        }
    }

    private static void OnErrorShared(PlayFabError error) {
        Debug.Log(error.GenerateErrorReport());
    }
    #endregion

    #region Send Data to PlayFabs

    public void SetPlayFabGameDataAndStatistics() {
        //server side
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest() {
            FunctionName = "UpdatePlayerGameDataAndStats",
            FunctionParameter = new {
                PM_GameData = MakeLocalGameDataString(),
                Coins_Earned = coinsEarned,
                Coins_Spent = coinsSpent,
                Coins_Total = coinsTotal,
                Flowers = GetFlowersCompleted(),
                Free_Coins = PlayerPrefs.GetInt("FreeCoinsCounter", 0),
                Hints_Used = PlayerPrefs.GetInt("HintsUsed", 0),
                Purchase_Gold_0500 = PlayerPrefs.GetInt("Times_Purchased_Gold_500", 0),
                Purchase_Gold_1500 = PlayerPrefs.GetInt("Times_Purchased_Gold_1500", 0),
                Purchase_Gold_5000 = PlayerPrefs.GetInt("Times_Purchased_Gold_5000", 0),
                Puzzles_Solved = profile_puzzlesSolved,
                Time_Played = Convert.ToInt32(PlayerPrefs.GetFloat("timePlayed", 0)),
                Wheel_Spins = PlayerPrefs.GetInt("WheelSpins", 0),
                Themes_Owned = GetThemesOwned(),
                Sleepy_Clicked = PlayerPrefs.GetInt("SleepyClicked", 0),
            },
            GeneratePlayStreamEvent = true
        };
        PlayFabClientAPI.ExecuteCloudScript(request, result => { Debug.Log("Player GameData Updated on Playfabs"); }, error => { error.GenerateErrorReport(); });
    }



    private int GetThemesOwned() {
        int counter = 0;
        for (int i =0; i < themes.Count; i++) {
            if (themes[i] != 0) {
                counter++;
            }
        }
        return counter;
    }

    private int GetFlowersCompleted() {
        int totalMysteryFlowersDiscovered = 0;

        for (int i = 0; i < GameDataControl.gdControl.blockSecretsUnlocked.Count; i++) {
            int petal1 = GameDataControl.gdControl.blockSecretsUnlocked[i][0];
            int petal2 = GameDataControl.gdControl.blockSecretsUnlocked[i][1];
            int petal3 = GameDataControl.gdControl.blockSecretsUnlocked[i][2];
            int petal4 = GameDataControl.gdControl.blockSecretsUnlocked[i][3];

            if (petal1 == 1 && petal2 == 1 && petal3 == 1 && petal4 == 1) {
                totalMysteryFlowersDiscovered++;
            }
        }

        return totalMysteryFlowersDiscovered;
    }

}

    #endregion



[Serializable]
class PlayerData {
    public int coinsEarned;
    public int coinsSpent;
    public int coinsTotal;
    public List<int> blocksUnlocked;
    public List<List<int>> all_level_results;
    public List<List<int>> blockSecretsUnlocked;
    public bool hardUnlocked;
    public bool extraBlock1Unlocked;
    public List<int> daily_level_results;
    public List<int> daily_easyLevel_Indexes;
    public List<int> daily_hardLevel_Indexes;
    public List<int> extra_level_block_1_results;
    public int profile_puzzlesSolved;
    public List<int> themes;
}
