using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Serializable]
public struct Score
{
    public int position;
    public float value;
    public string display_name;

    public Score(int position, float value, string display_name)
    {
        this.position = position;
        this.value = value;
        this.display_name = display_name;
    }
}

[System.Serializable]
public class ScoreList
{
    public int count;
    public int position;
    public Score[] scores;
}


public class RequestHandler
{

    private const string BaseUrl = "https://pallette-leaderboard.herokuapp.com/";
    private const string ApiVersion = "v1";

    public static bool isLoading = false;


    public static IEnumerator SendHealthCheck()
    {
         
        UnityWebRequest request = UnityWebRequest.Get(BaseUrl);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
    }

    public static IEnumerator PostRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        isLoading = false;
    }


    public static IEnumerator GetGameScores(string gameId, float playerScore, List<Score> ScoresOut)
    {
        isLoading = true;
        string gameScoreUrl = BaseUrl + $"api/{ApiVersion}/scores/{gameId}?score={playerScore}&limit=7";
        UnityWebRequest request = UnityWebRequest.Get(gameScoreUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            ScoreList scoreList = new ScoreList();
            string json = request.downloadHandler.text;

            scoreList = JsonUtility.FromJson<ScoreList>(json);

            if(scoreList.count == 0)
            {
                Debug.Log("No scores from game server!");
            }
            else
            {
                for (int i = 0; i < scoreList.count; i++)
                {
                    ScoresOut.Add(scoreList.scores[i]);
                }
            }
            
            isLoading = false;
        }
    }


    public static IEnumerator SubmitGameScore(string gameId,
        string displayName,
        float value
    )
    {
        isLoading = true;
        string newScoreUrl = BaseUrl + $"api/{ApiVersion}/scores/{gameId}";

        Score newScore = new Score(0, value, displayName);
        string payload = JsonUtility.ToJson(newScore);
        return PostRequest(newScoreUrl, payload);
    }

}


public class LeaderBoardService : MonoBehaviour
{

    /* These two fields required for sending data to LeaderBoardService  */
    public string GameId;
    public string SecretKey;
    public List<Score> GameScores;

    public string displayName;
    public int gamePlayerScore;

    public ScrollRect scoreScrollView;
    public GameObject ScoreItemPrefab;

    public static LeaderBoardService Instance;

    public HighScoreTable highScoreTable;

    private bool isValid = false;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (GameId == null || SecretKey == null)
        {
            Debug.LogError("Please make sure you have set up your " +
                "LeaderBoardService GameObject with the proper GameId " +
                "and Secret Key");
        }
        StartCoroutine(RequestHandler.SendHealthCheck());
        StartCoroutine(RequestHandler.GetGameScores(GameId, 0, GameScores));
    }



    public void ShowLeaderBoardForPlayerScore(float playerScore)
    {
        GameScores.Clear();
        StartCoroutine(RequestHandler.GetGameScores(GameId, playerScore, GameScores));
        StartCoroutine(OnScoresLoaded());
    }

    public void SubmitPlayerScore()
    {

        if (!isValid) return;

        StartCoroutine(RequestHandler.SubmitGameScore(GameId, displayName, gamePlayerScore));

        //Hide text entry and button
        transform.Find("LeaderBoard UI Canvas/InputField").gameObject.SetActive(false);
        transform.Find("LeaderBoard UI Canvas/Submit").gameObject.SetActive(false);

        StartCoroutine(OnScoreSaved());
    }


    public void SetPlayerName(string text)
    {
        displayName = text;
        if (text != null && text.Length > 2 && text.Length < 12)
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }
    }

    public void Reset()
    {
        transform.Find("LeaderBoard UI Canvas/InputField").gameObject.SetActive(true);
        transform.Find("LeaderBoard UI Canvas/Submit").gameObject.SetActive(true);
    }

    IEnumerator OnScoresLoaded()
    {

        while (RequestHandler.isLoading)
            yield return new WaitForSeconds(0.2f);

        highScoreTable.AddScores(GameScores);
    }


    IEnumerator OnScoreSaved()
    {

        while (RequestHandler.isLoading)
            yield return new WaitForSeconds(0.2f);

        ShowLeaderBoardForPlayerScore(gamePlayerScore);
    }

}
