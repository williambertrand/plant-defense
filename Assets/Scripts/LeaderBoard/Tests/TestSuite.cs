using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestSuite
{

    string TEST_GAME_ID = "ec33385e";
    string TEST_SECRET = "ec33385e";
    float timeout = 2.0f;

    LeaderBoardService ls;


    [UnitySetUp]
    public IEnumerator SetUp()
    {

        GameObject test = GetGameMock();
        test.AddComponent<LeaderBoardService>();
        ls = test.GetComponent<LeaderBoardService>();
        ls.GameId = TEST_GAME_ID;
        ls.SecretKey = TEST_SECRET;

        yield return new EnterPlayMode();

    }

    [UnityTest]
    public IEnumerator GetScoresWithPosition()
    {

        ls.ShowLeaderBoardForPlayerScore(100);

        yield return new WaitForSeconds(timeout);

        Assert.IsTrue(true);
    }

    [UnityTest]
    public IEnumerator SubmitScore()
    {

        ls.gamePlayerScore = 10;
        ls.displayName = "test...";

        ls.SubmitPlayerScore();

        yield return new WaitForSeconds(timeout);

        Assert.IsTrue(true);
    }



    private GameObject GetGameMock()
    {
        return new GameObject();
    }

    [UnitySetUp]
    public IEnumerator TearDown()
    {

        yield return new ExitPlayMode();
    }
}