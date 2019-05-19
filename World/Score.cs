using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {


    private static Text scoreUI;
    private static int killCount;
    private static int multiplicatorPoint;
    private static int scorePoints;


    private void Start()
    {
        scoreUI = GetComponentInChildren<Text>();
        scoreUI.text = "0";
        scorePoints = 0;
        killCount = 0;
        scoreUI.text = scorePoints.ToString();
    }



    public static void AddPoint(int point)
    {
        killCount++;
        scorePoints += point;
        scoreUI.text = scorePoints.ToString();
    }
    public static int GetPoints()
    {
        return scorePoints;
    }

}
