using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UIHandler : MonoBehaviour
{

    [SerializeField]
    private PlayerInformation _playerInfo = null;
    [SerializeField]
    private Text moneyText = null;
    [SerializeField]
    private GameObject paper = null;
    [SerializeField]
    private Text headline = null;
    [SerializeField]
    private Text mainText = null;
    [SerializeField]
    private Text randomText = null;

    [SerializeField]
    private Image rButtonImage = null;
    [SerializeField]
    private Image escButtonImage = null;

    [SerializeField]
    private Player player = null;

    [SerializeField]
    private GameObject levelLinePanel = null;

    [SerializeField]
    private GameObject moneyPanel = null;

    [SerializeField]
    private GameObject gameOverPanel = null;

    private GameObject notePaper;
    [SerializeField]
    private NoteList[] noteList = null;

    [SerializeField]
    private GameObject completedGamePanel = null;

    [SerializeField]
    private Text completedGameText = null;

    [SerializeField]
    private Text completedGameButtonText = null;


    private string[] headlines = { "BREAKING NEWS! HUGE MONEY WERE STOLEN FROM THE CENTRAL BANK!", "NEW REPORT SAYS THE THIEF CAN BE CONNECTED WITH RUSSIAN HACKERS",
        "PEOPLE AFRAID FOR THEIR POCKETS!", "THE SHADOW: THAT THIEF IS INVISIBLE", "POLICE CAN'T FIND HIM. THIEF COULD BE CONNECTED WITH PD",
        "BREAKING NEWS: NORTH KOREA TOOK RESPONSOBILITY FOR THE MASS THIEVERY", "COULD IT BE THE NEW ROBIN HOOD?", "THIEF COULD BE GAME ADDICTED, EXPERT SAYS",
        "SOMEBODY ONCE TOLD ME THE WORLD IS GONNA ROLL ME", "JEREMY MAKE SOME NICE CLICKBAIT HEADER"};

    private string[] mainTexts =
    {
        "Yesterday Central Bank was robbed by an unknown person. Police says they have a trace on him and soon he will be in jail." +
            " and they are working hard to identify the thief. Some old lady said that she knows the man behind the robbery, but won't say his name.",
        "Donald Pump, the president of the US, think the Russians can be envolved. \"No matter who did this, he will be punished\" -- said Donald Pump. Very strong speech as always!",
        "New studies says that 42% of citizen afraid for their pockets. New popular trend -- people sewing their pockets to prevent the attacks. New hashtag #sewformoney reaching the top of Chirmer.",
        "Research says that this thief could be invisible. Dr. Emmerich says that some type of parasytes could be involved. \"It could be parasites, or nanomachines. I am not certain\" -- said Dr. Emmerich.",
        "The new connection: PD could be involved in the mass robbery. The main argument that the investigation is extremely slow. Police says it's because of lack of evidence, but we know better.",
        "Kim Jong Un: \"Feel the power of Juche Nation! We have unlimited resources! We will rob all your banks and buy some Bitcoins! Ave the Juche! I am the best president\" -- said North Korean leader.",
        "Robin Hood? More like robbing good. Banks lost their money but no one in jail. Billionares afraid of being the next target. Propeller family says they have no fear because the thief has no strength even to hold their daily profit.",
        "Experts says the thief could be game addicted. There is one game called PayWeek where you can rob banks and other things. This is extremely good argument. Some politicians said they want to ban videogames.",
        "I ain't the sharpest tool in the shed. She was looking kind of dumb with her finger and her thumb. In the shape of an L on her forehead. Well the years start coming and they don\'t stop coming",
        "Why should I always think of some nice title? I am BORED. Screw you. Just give me some information about this robbery thing and I will write an article for you. God, I am so tired of being a journalist!."
    };

    private string[] randomTexts = { "WTS: Twinblades of the Deceiver. 150K. Write to greatpwner2003@smail.com", "Hi guys, I have lots of spare time. Do you know some places where I can hang out with my swedish friend? H. Kojima",
        "BUY THE NEW INNOVATIVE GAME THE ELDERS BOWLS: SKYFEEL! Not a Todd Howard.", "Microtransactions are the best way to experience the game, study says. EA.",
        "CELEBRATE! The new 1000th Civilization VI DLC is out! Added new spear for Rome and new grass texture! Only for $29.99",
        "Stop kidding around. Snake? SNAAAAAAKE!", "Another Settlement Needs Our Help. I marked it on your map. P. Garvey",
        "Join the Epsilon Program at epsilonprogram.com. Kifflom!", "Hi! I am a new Minecraft letsplayer please subscribe and support! My channel is SteveCubeHead2007"
    };

    private void Awake()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");

        if (playerGO != null)
        {
            player = playerGO.GetComponent<Player>();
        }
    }

    void Start()
    {
        notePaper = transform.GetChild(0).GetChild(5).gameObject;
        if (noteList[SceneManager.GetActiveScene().buildIndex - 2].firstGoal != "")
        {
            notePaper.SetActive(true);
            notePaper.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = noteList[SceneManager.GetActiveScene().buildIndex - 2].firstGoal;
            notePaper.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = noteList[SceneManager.GetActiveScene().buildIndex - 2].secondGoal;
            notePaper.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = noteList[SceneManager.GetActiveScene().buildIndex - 2].thirdGoal;
        }

        if (player != null)
        {
            player.OnMoneyChange += UpdateMoney;
            player.OnReload += Reload;
            player.OnReloadTimeChange += FillRCircle;
            player.OnGameOver += GameOver;
            player.OnEscTimeChange += FillEscCircle;
            player.OnToMenu += MainMenu;
        }
    }

    private void FillRCircle(float time)
    {
        rButtonImage.fillAmount = time / player.HoldTreshold;
    }

    private void FillEscCircle(float time)
    {
        escButtonImage.fillAmount = time / player.HoldTreshold;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
        ClearPlayerInfo();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        ClearPlayerInfo();
    }

    public void FinishLevel()
    {
        paper.SetActive(false);
        _playerInfo.tier = new PlayerInformation.Tier();
        _playerInfo.OveralMoney += player.CurrentMoney;

        var ratio = (float)player.CurrentMoney / player.GoalMoney;
        if (ratio >= 0.08f)
        {
            _playerInfo.tier = PlayerInformation.SetFlag(_playerInfo.tier, PlayerInformation.Tier.First);
        }
        if (ratio >= 0.14f)
        {
            _playerInfo.tier = PlayerInformation.SetFlag(_playerInfo.tier, PlayerInformation.Tier.Second);
        }
        if (ratio >= 0.20f)
        {
            _playerInfo.tier = PlayerInformation.SetFlag(_playerInfo.tier, PlayerInformation.Tier.Third);
        }

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Current:" + currentIndex + "overall: " + SceneManager.sceneCountInBuildSettings);
        if (currentIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            completedGamePanel.SetActive(true);
            for (int i = 0; i < _playerInfo.LevelMoney.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        completedGameText.text += "MONDAY: $" + _playerInfo.LevelMoney[i];
                        break;
                    case 1:
                        completedGameText.text += "\nTUESDAY: $" + _playerInfo.LevelMoney[i];
                        break;
                    case 2:
                        completedGameText.text += "\nWEDNESDAY: $" + _playerInfo.LevelMoney[i];
                        break;
                    case 3:
                        completedGameText.text += "\nTHURSDAY: $" + _playerInfo.LevelMoney[i];
                        break;
                    case 4:
                        completedGameText.text += "\nFRIDAY: $" + _playerInfo.LevelMoney[i];
                        break;
                    case 5:
                        completedGameText.text += "\nSATURDAY: $" + _playerInfo.LevelMoney[i];
                        break;
                    case 6:
                        completedGameText.text += "\nSUNDAY: $" + _playerInfo.LevelMoney[i];
                        break;
                }
            }

            if (_playerInfo.OveralMoney >= player.GoalMoney)
            {
                completedGameButtonText.text = "GOOD JOB!";
                completedGameButtonText.color = Color.green;
            }
            else
            {
                completedGameButtonText.text = "NO LUCK!";
                completedGameButtonText.color = Color.red;
            }
        }
        else
        {
            levelLinePanel.SetActive(true);
            levelLinePanel.GetComponentInChildren<LevelProgressBar>().UpdateLevelLine(_playerInfo.OveralMoney, player.GoalMoney, currentIndex - 1);
            Invoke("ChangeLevel", 5f);
        }

        /*
         *  TODO: Alarm tier
         */
    }

    public void Finish()
    {
        player.gameObject.SetActive(false);
        moneyPanel.SetActive(false);
        paper.SetActive(true);
        notePaper.SetActive(false);
        GeneratePaper();
    }

    private void ChangeLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex + 1);
    }

    private void GeneratePaper()
    {
        int index;

        if (_playerInfo.PaperIndexes.Count >= mainTexts.Length)
        {
            _playerInfo.PaperIndexes.Clear();
        }

        do
        {
            index = UnityEngine.Random.Range(0, mainTexts.Length);
        } while (_playerInfo.PaperIndexes.Contains(index));

        _playerInfo.PaperIndexes.Add(index);

        mainText.text = mainTexts[index];
        headline.text = headlines[index];

        randomText.text = randomTexts[UnityEngine.Random.Range(0, randomTexts.Length)];
    }

    private void UpdateMoney()
    {
        moneyText.text = "$" + player.CurrentMoney;
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        player.FreezeInput = true;
        player.tag = "Untagged";
    }

    private void ClearPlayerInfo()
    {
        _playerInfo.LevelMoney = new List<int>();
        _playerInfo.OveralMoney = 0;
        _playerInfo.tier = new PlayerInformation.Tier();
        _playerInfo.PaperIndexes = new List<int>();
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [Serializable]
    public class NoteList
    {
        public string firstGoal;
        public string secondGoal;
        public string thirdGoal;
    }


}



