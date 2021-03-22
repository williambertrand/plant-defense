using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{

    private Transform entryListContainer;
    private Transform entryTemplate;


    private void Awake()
    {

    }


    //Add Ordered list of scores to high score table
    public void AddScores(List<Score> scores)
    {
        entryListContainer = transform.Find("EntryListContainer");
        entryTemplate = transform.Find("ScoreEntry");



        if(transform.Find("EntryListContainer/ScoreEntry(Clone)") != null)
        {
            //TODO: Move score entry up so all children are clones, then delete all children transforms of the entrylist container
            foreach (Transform child in entryListContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        entryTemplate.gameObject.SetActive(false);


        float templateHeight = 20f;
        for (int i = 0; i < scores.Count; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryListContainer);
            RectTransform entryRect = entryTransform.GetComponent<RectTransform>();

            entryRect.anchoredPosition = new Vector2(0, -1 * templateHeight * i);
            entryTransform.gameObject.SetActive(true);

            //set text values
            entryTransform.Find("posText").GetComponent<Text>().text = "" + (i + 1);
            entryTransform.Find("scoreText").GetComponent<Text>().text = "" + scores[i].value;
            entryTransform.Find("nameText").GetComponent<Text>().text = scores[i].display_name;
        }
    }

    
}
