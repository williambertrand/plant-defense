using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Leaderboard
{
    public class HighScoreTable : MonoBehaviour
    {

        public Transform entryListContainer;
        public Transform entryTemplate;


        private void Awake()
        {}


        //Add Ordered list of scores to high score table
        public void AddScores(List<Score> scores)
        {

            //entryListContainer = transform.Find("EntryListContainer");
            Debug.Log("adding scores to table: " + scores.Count);

            RectTransform container = entryListContainer.GetComponent<RectTransform>();
            //container.sizeDelta = new Vector2(container.sizeDelta.x, container.sizeDelta.y + (65f * scores.Count));


            float templateHeight = 50f;
            for (int i = 0; i < scores.Count; i++)
            {
                // TODO this should be loaded from the leaderboard service...
                Transform entryTransform = Instantiate(entryTemplate, entryListContainer);
                RectTransform entryRect = entryTransform.GetComponent<RectTransform>();

                entryRect.anchoredPosition = new Vector2(0, -1 * templateHeight * i - 2);
                //entryRect.sizeDelta = new Vector2(entryRect.sizeDelta.x, -50.0f);
                //entryRect.offsetMax = new Vector2(5.0f, 5.0f);
                entryTransform.gameObject.SetActive(true);

                //set text values
                entryTransform.Find("posText").GetComponent<TMP_Text>().text = "" + GameManager.FormatDay(i + 1);
                entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = "" + (int)scores[i].value;
                entryTransform.Find("nameText").GetComponent<TMP_Text>().text = scores[i].display_name;
            }
            entryTemplate.gameObject.SetActive(false);
        }


    }
}