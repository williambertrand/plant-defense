using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Assets.Scripts.Leaderboard;

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

    private const string BaseUrl = "https://simply-leaderboard.herokuapp.com/";
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

    public static IEnumerator PostRequest(string url, string json, string accessKey)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("slb_access", accessKey);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }

        isLoading = false;
    }


    public static IEnumerator GetGameScores(string gameId, float playerScore, List<Score> ScoresOut)
    {
        isLoading = true;
        string gameScoreUrl = BaseUrl + $"api/{ApiVersion}/scores/{gameId}?player_score={playerScore}";
        
        UnityWebRequest request = UnityWebRequest.Get(gameScoreUrl);
        request.SetRequestHeader("slb_access", LeaderBoardService.Instance.SecretKey);
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

            if (scoreList.count == 0)
            {
                Debug.Log("No scores from game server!");
            }
            else
            {
                Debug.Log("Scores loaded: " + scoreList.count);
                for (int i = 0; i < scoreList.count; i++)
                {
                    ScoresOut.Add(scoreList.scores[i]);
                }
            }

            isLoading = false;
            Debug.Log("loading..." + isLoading);
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
        return PostRequest(newScoreUrl, payload, LeaderBoardService.Instance.SecretKey);
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
    public Button submitButton;

    public ScrollRect scoreScrollView;
    public GameObject ScoreItemPrefab;

    public static LeaderBoardService Instance;

    public HighScoreTable highScoreTable;

    private bool isValid = false;
    public string mode;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if(submitButton != null)
        {
            submitButton.gameObject.SetActive(false);
        }

        //TODO: Submit all stats updates as well....
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
        gamePlayerScore = GameStats.Points;
        StartCoroutine(RequestHandler.SendHealthCheck());

        if(mode == "view")
        {
            StartCoroutine(RequestHandler.GetGameScores(GameId, 0, GameScores));
            StartCoroutine(OnScoresLoaded());
        }
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

        Debug.Log("SUBMIT name: " + displayName + " Points: " + gamePlayerScore);

        StartCoroutine(RequestHandler.SubmitGameScore(GameId, displayName, gamePlayerScore));

        //Hide text entry and button
        //transform.Find("LeaderBoard UI Canvas/InputField").gameObject.SetActive(false);
        //transform.Find("LeaderBoard UI Canvas/Submit").gameObject.SetActive(false);

        StartCoroutine(OnScoreSaved());
    }


    public void SetPlayerName(string text)
    {
        displayName = text;
        if (text != null && text.Length > 2 && text.Length < 12)
        {
            Debug.Log("Valid player name: " + displayName);
            isValid = true;
            submitButton.gameObject.SetActive(true);
        }
        else
        {
            isValid = false;
        }
    }

    public void Dismiss()
    {
        StartCoroutine(RequestHandler.GetGameScores(GameId, 0, GameScores));
        StartCoroutine(OnScoresLoaded());
    }

    IEnumerator OnScoresLoaded()
    {
        while (RequestHandler.isLoading)
        {
            yield return new WaitForSeconds(0.25f);
            Debug.Log("loading...");
        }
        highScoreTable.AddScores(GameScores);
    }


    IEnumerator OnScoreSaved()
    {

        while (RequestHandler.isLoading)
            yield return new WaitForSeconds(0.2f);
        GameScores.Clear();
        ShowLeaderBoardForPlayerScore(gamePlayerScore);
        transform.Find("AddScoreDisplay").gameObject.SetActive(false);

    }

}