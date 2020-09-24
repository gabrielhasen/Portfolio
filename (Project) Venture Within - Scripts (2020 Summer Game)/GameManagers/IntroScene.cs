using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    public IntroSequence introSequence;
    private int index;

    public AudioClip backgroundMusicClip;

    public float FadeInDuration;
    public float FadeOutDuration;

    private AudioSource introSource;
    private GameObject backgroundImageObj;
    private Image backgroundSprite;

    private float minAnchorWithDialogue;
    private bool canContinue;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        canContinue = false;

        // get and set the background Image and Sprite objects
        backgroundImageObj = this.transform.GetChild(0).gameObject;
        if(backgroundImageObj == null)
        {
            Debug.LogError("Could not find the background image object");
        }

        backgroundSprite = backgroundImageObj.GetComponent<Image>();
        if(backgroundSprite == null)
        {
            Debug.LogError("Could not find the background image sprite");
        }

        minAnchorWithDialogue = backgroundImageObj.GetComponent<RectTransform>().anchorMin.y;
        if(minAnchorWithDialogue == null)
        {
            Debug.LogError("Could not find the background image anchors");
        }

        // get intro audio source
        introSource = this.GetComponent<AudioSource>();
        if(backgroundMusicClip != null && introSource != null) introSource.clip = backgroundMusicClip;
        else Debug.LogError("missing background music clip or audio source");

        // start music
        SoundManager.Instance.PlayBackgroundMusic(introSource);

        // start intro sequence
        ExecuteSequence();
    }

    void LateUpdate(){
        // move through introSequence when 'F' is pressed to move to next dialogue element
        if(canContinue && Input.GetKeyDown(KeyCode.E)){
            // trigger next introSequence element
            //Debug.Log("next sequence element");
            SetNextSequenceElement();
        }
    }

    private void ExecuteSequence()
    {
        SetNextSequenceElement();
    }

    private void SetNextSequenceElement()
    {
        //Debug.Log("calling set next sequence element");
        canContinue = false;
        if(index < introSequence.sequence.Length)
        {
            // if there is no dialogue set the image and scale it to fill whole screen
            if(!introSequence.sequence[index].hasDialogue)
            {
                // disable dialogue system and panel
                DialogueManager.Instance.HideDialogue();

                // scale the background image to whole screen instead of just the bottom portion
                backgroundImageObj.GetComponent<RectTransform>().anchorMin = new Vector2(backgroundImageObj.GetComponent<RectTransform>().anchorMin.x, 0.0f);

                backgroundSprite.sprite = introSequence.sequence[index].image;
                StartCoroutine(Prompt(introSequence.sequence[index].timer));
                StartCoroutine(FadeIn());

            } else // set the background image, and, if there is a dialogue element set that too
            {
                canContinue = true;
                backgroundImageObj.GetComponent<RectTransform>().anchorMin = new Vector2(backgroundImageObj.GetComponent<RectTransform>().anchorMin.x, minAnchorWithDialogue);

                DialogueManager.Instance.MakeComment(introSequence.sequence[index].dialogueLine, introSequence.sequence[index].timer);
                backgroundSprite.sprite = introSequence.sequence[index].image;
                StartCoroutine(FadeIn());
            }
            index++;

        } else  // end sequence and enter sub
        {
            DialogueManager.Instance.transform.GetChild(0).gameObject.SetActive(false); // make sure that the dialogue ui is disabled before entering sub
            FadeOut();
            EnterSub();
        }
    }

    private void FadeOut()
    {
        Debug.Log("Fade out");
        Image FadeImage = this.transform.GetChild(1).GetComponent<Image>();

        float currentTime = 0;

        while(currentTime < FadeOutDuration)
        {
            currentTime += Time.deltaTime;
            FadeImage.color =  Color.Lerp(Color.clear, Color.black, currentTime/FadeOutDuration);
            introSource.volume = Mathf.Lerp(1, 0, currentTime/FadeOutDuration);
        }
    }

    private IEnumerator FadeIn()
    {
        Image FadeImage = this.transform.GetChild(1).GetComponent<Image>();
        FadeImage.color = Color.black;

        float currentTime = 0;

        while(currentTime < FadeInDuration)
        {
            currentTime += Time.deltaTime;
            FadeImage.color =  Color.Lerp(Color.black, Color.clear, currentTime/FadeInDuration);
            yield return null;
        }
        yield break;
    }

    private IEnumerator Prompt(float time)
    {
        canContinue = false;
        GameObject prompt = this.transform.GetChild(2).gameObject;
        prompt.SetActive(false);
        yield return new WaitForSeconds(time);
        prompt.SetActive(true);
        canContinue = true;
    }

    private void EnterSub()
    {
        Debug.Log("Enter Sub");
        MMGameEvent.Trigger("Save");
        CorgiEngineEvent.Trigger(CorgiEngineEventTypes.LevelEnd);
        LoadingSceneManager.LoadScene("base");

    }
}
