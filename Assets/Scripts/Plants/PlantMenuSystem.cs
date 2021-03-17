using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TODO: This is duplicating some logic/ info used by PlayerPlanting, we should have
// A better, more centralized way to load info for any plants in the game
public struct PlantInfo
{
    public string displayName;
    public string prefabResourceName;
    public string color;
    public string sprinteName;
    public int cost;

    public PlantInfo(string displayName, string prefabResourceName, string color, string sprinteName, int cost)
    {
        this.displayName = displayName;
        this.prefabResourceName = prefabResourceName;
        this.color = color;
        this.sprinteName = sprinteName;
        this.cost = cost;
    }
}

public class PlantOptions
{
    public static Dictionary<PlantType, List<PlantInfo>> menuOptions;
}

public class PlantMenuSystem : MonoBehaviour
{

    public static PlantMenuSystem Instance;

    public PlantType? currentOpen;

    // Buttons at bse level to open full menu for that type
    public Button defenseButton;
    public Button pointsButton;
    public Button otherButton;

    public Canvas menuHolder;
    public Text openTitleText;

    [SerializeField]
    public List<Button> openMenuOptions;

    // Highlighted plant info view
    public Canvas infoPlantView;
    public Text infoPlantTitle;
    public Text infoPlantAge;
    public Text infoPlantHealth;


    // Sub canvas for damage type plants
    public Canvas defensePlantView;
    public Text infoPlantDamage;
    public Text infoPlantAttackSpeed;

    // Sub canvas for point plantss
    public Canvas pointPlantView;
    public Text infoPlantTotalPoints;
    public Text infoPlantPointRate;


    void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }

        // Set up plant options
        PlantOptions.menuOptions = new Dictionary<PlantType, List<PlantInfo>>();


        // Defense Plants
        List<PlantInfo> defensePlantList = new List<PlantInfo>();

        PlantInfo defenseRose = new PlantInfo(
           "Rose",
           "rose",
           "#69cfef",
           "rose-icon-bg",
           4
        );

        PlantInfo defenseNettle = new PlantInfo(
            "Nettle",
            "nettle",
            "#d1b3ff",
            "nettle-icon-bg",
            8
        );

        defensePlantList.Add(defenseRose);
        defensePlantList.Add(defenseNettle);
        PlantOptions.menuOptions.Add(PlantType.DEFENSE, defensePlantList);


        // Point Plants
        List<PlantInfo> pointPlantList = new List<PlantInfo>();

        PlantInfo pointDaffodil = new PlantInfo(
           "Daffodil",
           "daffodil",
           "#d1b3ff",
           "daffodil-icon-bg",
           5
        );

        PlantInfo pointTulip = new PlantInfo(
           "Tulip",
           "tulip",
           "#69cfef",
           "tulip-icon-bg",
           8
        );

        pointPlantList.Add(pointDaffodil);
        pointPlantList.Add(pointTulip);
        PlantOptions.menuOptions.Add(PlantType.POINTS, pointPlantList);


        // Other Plants
        List<PlantInfo> otherPlantList = new List<PlantInfo>();

        PlantInfo otherPlantSunflower = new PlantInfo(
           "Sunflower",
           "sunflower",
           "#8ddb34",
           "sunflower-icon",
           10
        );

        otherPlantList.Add(otherPlantSunflower);
        PlantOptions.menuOptions.Add(PlantType.AUGMENT, otherPlantList);

    }

    // Start is called before the first frame update
    void Start()
    {
        defenseButton.onClick.AddListener( () => {
            ClickMenuType(PlantType.DEFENSE);
        });

        pointsButton.onClick.AddListener(() => {
            ClickMenuType(PlantType.POINTS);
        });

        otherButton.onClick.AddListener(() => {
            ClickMenuType(PlantType.AUGMENT);
        });

        menuHolder.enabled = false;
        openTitleText.text = "Title";
    }

    private void AddTitleForType(PlantType t)
    {
        openTitleText.text = Plant.TypeToString(t) + " Plants";
        openTitleText.enabled = true;
    }

    private void SetOptionsForType(PlantType t)
    {

        for (int i = 0; i < openMenuOptions.Count; i++)
        {
            openMenuOptions[i].gameObject.SetActive(false);
            openMenuOptions[i].onClick.RemoveAllListeners();
        }

            List<PlantInfo> infoList = PlantOptions.menuOptions[t];
        for (int i = 0; i < infoList.Count; i++)
        {

            openMenuOptions[i].GetComponentInChildren<Text>().text = infoList[i].displayName;
            Color color;
            if (ColorUtility.TryParseHtmlString(infoList[i].color, out color))
            {
                //if (Player.Instance.seedCount < infoList[i].cost)
                //{
                //    color.a = 0.7f;
                //}
                //else
                //{
                //    color.a = 1.0f;
                //}
                openMenuOptions[i].GetComponent<Image>().color = color;

            }
            
            // Set cost title
            Transform costTextTransform = openMenuOptions[i].transform.Find("Cost");
            if (costTextTransform != null)
            {
                costTextTransform.GetComponent<Text>().text = "" + infoList[i].cost;
            } else
            {
                Debug.LogError("Could not find cost text for: " + transform.name);
            }

            // Set plant Sprite icon
            Sprite icon = Resources.Load<Sprite>(infoList[i].sprinteName);
            if (icon == null)
            {
                Debug.LogError("Could not load icon: " + infoList[i].sprinteName);
            } else
            {
                Transform iconTransform = openMenuOptions[i].transform.Find("Background");
                if (iconTransform != null)
                {
                    iconTransform.GetComponent<Image>().sprite = icon;
                    //if (Player.Instance.seedCount <= infoList[i].cost)
                    //{
                    //    iconTransform.GetComponent<Image>().color = color;
                    //}
                }
                else
                {
                    Debug.LogError("Could not find cost text for: " + transform.name);
                }
            }

            openMenuOptions[i].gameObject.SetActive(true);
            string name = infoList[i].prefabResourceName;
            openMenuOptions[i].onClick.AddListener(() => { OnClickPlant(name); });
        }
    }

    /**
     *  Open the menu
     *  Add the type name as a title to the panel
     *  Add buttons for all plants of that type
     */
    void ClickMenuType(PlantType type)
    {
        if(currentOpen == type)
        {
            menuHolder.enabled = false;
            currentOpen = null;
            return;
        }
        AddTitleForType(type);
        SetOptionsForType(type);
        menuHolder.enabled = true;
        currentOpen = type;
    }


    void OnClickPlant(string id)
    {
        PlayerPlanting.Instance.PlantSeedling(id);
    }

    public void OnPlantHighlighted(Plant p)
    {
        // Pass in a null plant to de-select
        if(p == null)
        {
            infoPlantView.enabled = false;
            defensePlantView.enabled = false;
            pointPlantView.enabled = false;
            return;
        }

        infoPlantView.enabled = true;
        infoPlantTitle.text = p.plantName;
        infoPlantAge.text = "" + p.age;
        infoPlantHealth.text = "" + p.health + "/" + p.maxHealth;
        if(p is DefensePlant)
        {
            DefensePlant defP = (DefensePlant) p;
            infoPlantDamage.text = "" + defP.attackDamage;
            infoPlantAttackSpeed.text = "" + defP.attackSpeed.ToString("F2");
            defensePlantView.enabled = true;
            pointPlantView.enabled = false;
        }
        else if (p is PointPlant)
        {
            PointPlant pp = (PointPlant)p;
            infoPlantTotalPoints.text = "" + pp.currentPointVal.ToString("F2");
            infoPlantPointRate.text = "" + pp.pointRate.ToString("F2");
            pointPlantView.enabled = true;
            defensePlantView.enabled = false;

        }
    }


    public Text notifyText;
    public void Notify(string message, float duration)
    {
        notifyText.text = message;
        notifyText.gameObject.SetActive(true);
        StartCoroutine(HideAfterTime(duration, notifyText.gameObject));
    }


    IEnumerator HideAfterTime(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        obj.SetActive(false);
    }
}
