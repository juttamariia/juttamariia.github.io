using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeistManager : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private Transform heistListTargetsParent;
    [SerializeField] private GameObject stealSuccessText;

    private List<ArtInfo> heistList = new List<ArtInfo>();
    private List<Image> heistListObjects = new List<Image>();

    private List<ArtInfo> artInfoGeneral = new List<ArtInfo>();
    private List<ArtInfo> artInfoDigital = new List<ArtInfo>();
    private List<ArtInfo> artInfoCrafts = new List<ArtInfo>();

    public static HeistManager instance;

    private GameObject currentStolenArt;
    private int currentStolenArtIndex;
    private int amountOfStolenArt;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("There is more than one Heist Manager in the scene!");
            Destroy(gameObject);
        }

        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        artInfoGeneral = ArtManager.instance.GetArtInfo(ArtCategory.General);
        artInfoDigital = ArtManager.instance.GetArtInfo(ArtCategory.Digital);
        artInfoCrafts = ArtManager.instance.GetArtInfo(ArtCategory.Crafts);

        SetHeistList();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.onWinMinigame += AfterMinigameWon;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.onWinMinigame -= AfterMinigameWon;
    }

    private void SetHeistList()
    {
        // select random art pieces from each exhibition
        heistList.Add(artInfoGeneral[Random.Range(0, artInfoGeneral.Count)]);
        heistList.Add(artInfoDigital[Random.Range(0, artInfoDigital.Count)]);
        heistList.Add(artInfoCrafts[Random.Range(0, artInfoCrafts.Count)]);

        // show the selected art pieces in UI
        for(int i = 0; i < heistList.Count; i++)
        {
            Image listImage = heistListTargetsParent.GetChild(i).GetChild(0).GetComponent<Image>();

            heistListObjects.Add(listImage);
            listImage.sprite = heistList[i].heistListSprite;
        }
    }

    public bool IsPaintingOnHeistList(ArtInfo foundPainting, GameObject paintingObject)
    {
        bool paintingOnList = false;

        if (heistList.Contains(foundPainting))
        {
            paintingOnList = true;
            currentStolenArtIndex = heistList.IndexOf(foundPainting);
            currentStolenArt = paintingObject;
        }

        return paintingOnList;
    }

    private void AfterMinigameWon()
    {
        // if the final minigame is won, end the whole game
        if(amountOfStolenArt >= 3)
        {
            Debug.Log("Win game!");
            GameEventsManager.instance.gameEvents.WinGame();
            return;
        }

        Debug.Log("Painting stolen!");
        stealSuccessText.SetActive(true);

        StartCoroutine(HideStolenArt());

        amountOfStolenArt++;

        // check if all the paintings are collected; if yes, trigger the events related to final state of the game
        if (amountOfStolenArt >= 3)
        {
            Debug.Log("All the paintings stolen!");
            GameEventsManager.instance.gameEvents.AllPaintingsCollected();
        }

        // close minigame view
        GameEventsManager.instance.gameEvents.ToggleMinigameView(false, "close");
    }

    public GameObject GetCurrentPainting()
    {
        return currentStolenArt;
    }

    private IEnumerator HideStolenArt()
    {
        // set the painting invisible
        currentStolenArt.transform.GetChild(0).gameObject.SetActive(false);

        // disable collider
        currentStolenArt.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(2f);

        // trigger the animation in the heist list object
        heistListObjects[currentStolenArtIndex].transform.parent.GetComponent<Animator>().SetBool("found", true);
    }
}
