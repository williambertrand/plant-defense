using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *  May eventually want to split this up
 *  but for now this will handle a lot of the player actions
 */
public class Player : MonoBehaviour
{

    public int seedCount;
    public static Player Instance;
    public Text seedCountText;

    TopDownCharacterController playerController;

    public GameObject showPlusSeedPrefab;
    private GameObject showPlusSeedInstance;
    private Animator seedAnimator;


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
        seedCount = GameConstants.Player.StartSeeds;
        seedCountText.text = "" + seedCount;
        playerController = this.GetComponent<TopDownCharacterController>();

        showPlusSeedInstance = Instantiate(showPlusSeedPrefab, this.transform);
        seedAnimator = showPlusSeedInstance.GetComponent<Animator>();
        showPlusSeedInstance.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSeedCollect(bool showAnim)
    {
        seedCount++;
        seedCountText.text = "" + seedCount;
        if (showAnim) ShowSeedCollectAnim();
    }

    public void OnSeedSpend(int cost)
    {
        seedCount -= cost;
        seedCountText.text = "" + seedCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameConstants.Tags.Seed)
        {
            OnSeedCollect(true);
            Destroy(collision.gameObject);
        }
    }

    private void ShowSeedCollectAnim()
    {
        showPlusSeedInstance.transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
        showPlusSeedInstance.SetActive(true);
        seedAnimator.Play("SeedShow", -1, 0.0f);

    }

}
