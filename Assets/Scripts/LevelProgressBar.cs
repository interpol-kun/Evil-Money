using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelProgressBar : MonoBehaviour {

    private Image line;

    [SerializeField]
	private List<Text> numbers = null;

    [SerializeField]
	private Color highlightedColor = Color.white;

    [SerializeField]
	private Text overallMoneyText = null;

    [SerializeField]
    private Text tip = null;

    private static string[] tips = {
        "TIP: If you have too much money you can drop them. It can be usefull to distract the enemy or to move faster.",
        "TIP: Don't be greedy. Money slow down your movement. Carefully plan your thievery and amount of money you can steal.",
        "TIP: Big thievery has big consequences. The next day will become harder based on your today profit",
        "TIP: Stealth is an option. Who can stop you from steal'n'run?",
        "TIP: Dog can't catch you. But it will chase you making enemies know your location.",
        "TIP: CCTV can spot you. But if you fast enough to runaway, enemies will only know your last position. But they will check the area.",
        "TIP: Don't drop the money on an enemy route. Cash which is not in its place can catch an attention."
    };

	private void Awake () {
        line = GetComponent<Image>();
        if(tip == null)
        {
            tip = GameObject.Find("Tips").GetComponent<Text>();
        }
        else
        {
        }
        if(overallMoneyText == null)
        {
            overallMoneyText = GameObject.Find("StolenMoney").GetComponent<Text>();
        }
	}

    public void UpdateLevelLine(int overallMoney, int maxMoney, int nextLevel)
    {
		tip.text = tips [Random.Range (0, tips.Length)];
        overallMoneyText.text = "$" + overallMoney;
        line.fillAmount = (float)overallMoney/maxMoney;
        numbers[nextLevel].color = highlightedColor;
        numbers[nextLevel - 1].color = Color.white;
    }
}
