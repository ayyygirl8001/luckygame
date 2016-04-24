using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameState
{
    Ready,
    Play,
    Result,
}

public class GameManager : MonoBehaviour {

    public Transform touchBoxParent;
    public Text currentProbabilityText;
    public Text totalProbabilityText;
    public Text totalScoreText;

    private const int CHOICE_MAX = 5;
    private const int GAME_SCORE = 100;

    private double currentProbability = 0;
    private double totalChoiceCount = 0;
    private double prevChoiceCount = 0;
    private int gameScore = 0;
    private int gameCount = 0;
    private int choiceCount = 0;
    private GameState state = GameState.Ready;

    // Use this for initialization
    void Start ()
    {
        ResetGame();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void ResetGame()
    {
        currentProbability = 1;
        totalChoiceCount = 0;
        prevChoiceCount = 1;
        gameScore = 0;
        gameCount = 0;
        choiceCount = 0;
        state = GameState.Ready;

        totalProbabilityText.text = "1\n1";
        currentProbabilityText.text = string.Format("{0}%", currentProbability * 100);
        totalScoreText.text = gameScore.ToString();

        NextGame();
    }

    void NextGame()
    {
        HideBoxs();

        choiceCount++;
        if (choiceCount > CHOICE_MAX)
        {
            choiceCount = CHOICE_MAX;
        }
        totalChoiceCount = (prevChoiceCount==0?1:prevChoiceCount) * choiceCount;
        prevChoiceCount = totalChoiceCount;

        currentProbability = GetProbability(choiceCount);

        gameScore += GAME_SCORE + (choiceCount * (GAME_SCORE/2));
        gameCount++;

        totalProbabilityText.text = string.Format("1\n{0}", totalChoiceCount);
        currentProbabilityText.text = string.Format("{0}%", currentProbability * 100);
        totalScoreText.text = gameScore.ToString();

        //setting boxs.
        touchBoxParent.GetChild(choiceCount - 1).gameObject.SetActive(true);

        state = GameState.Play;
    }

    void HideBoxs()
    {
        for (int i = 0; i < touchBoxParent.GetChildCount(); i++)
        {
            touchBoxParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    double GetProbability(int total)
    {
        return (1/System.Convert.ToDouble(total));
    }

    bool Challanger(int total)
    {
        System.Random rand = new System.Random();
        int seed = 100;
        int result = rand.Next(1, total * seed);
        return result <= seed;
    }

    public void TouchingObject(GameObject sender)
    {
        if (state == GameState.Play)
        {
            iTween.ScaleTo(sender, new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        }
    }

    public void TouchedObject(GameObject sender)
    {
        if (state == GameState.Play)
        {
            iTween.ScaleTo(sender, new Vector3(1f, 1f, 1f), 0.2f);
            
            if (Challanger(choiceCount))
            {
                NextGame();
            }
            else
            {
                ResetGame();
            }
        }
    }
}
