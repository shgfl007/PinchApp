//Kitchen Sink
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverIcons : VRObject {

    Animator hoverAnim;
    private GameObject feature;
    private GameObject hover;
    private Image iconSprite;
    public GameObject zoomFocus;
    private GameObject everythingContainer;
    private float speed = 15;
    private bool zoomIn = false;
    public Sprite defaultHover;
    private GameObject hoverHighlight;
    float mouseTimerLimit = 0.5f;
    public KeyCode keyActivate; //Assigned Key
    private bool hasExited;
    public bool introComplete = false;


    // Use this for initialization
    void Start () {
        feature = transform.Find("Feature").gameObject;
        hover = transform.Find("Hover").gameObject;
        hoverAnim = hover.GetComponent<Animator>();
       // Debug.Log(hoverAnim);
        hoverAnim.SetBool("animateHover", false);
        iconSprite = transform.Find("Icon").GetComponent<Image>();
        everythingContainer = GameObject.Find("EverythingContainer");
        hoverHighlight = transform.Find("Hover Highlight").gameObject;
        hover.GetComponent<SpriteRenderer>().sprite = defaultHover;
        Color temp = Color.white;
        temp.a = 0;
        hover.GetComponent<SpriteRenderer>().color = temp;
        hover.gameObject.SetActive(false);
    }


    public override void start(ref Cursor cursorInput)
    {
        single = cursorInput;
        mode = "single";

    }

    public override void end()
    {
        base.end();
        if (!hasExited)
        {
            checkMouseDoubleClick();
        }
    }


    // Update is called once per frame
    void Update () {

        if (zoomIn)
        {
            float step = speed * Time.deltaTime;
            everythingContainer.transform.position = Vector3.MoveTowards(everythingContainer.transform.position, zoomFocus.transform.position, step);
        }
        if (Vector3.Distance(everythingContainer.transform.position, zoomFocus.transform.position) < 0.1f && zoomIn) {
            zoomIn = false;
        }

        //for unity editor purpose
        if (Input.GetKeyDown(keyActivate)) {
            zoomedIn();
        }
    }

    public override void enterHover(Cursor c) {
        if (introComplete)
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
            ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerEnterHandler);
            hover.gameObject.SetActive(true);
            hoverAnim.SetBool("animateHover", true);
            //Debug.Log(gameObject.name);
            hasExited = false;
        }
    }

    public override void exitHover(Cursor c)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
        ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerExitHandler);
        hoverAnim.SetBool("animateHover", false);
        hover.GetComponent<SpriteRenderer>().sprite = defaultHover;
        Color temp = Color.white;
        temp.a = 0;
        hover.GetComponent<SpriteRenderer>().color = temp;
        hover.gameObject.SetActive(false);
        hasExited = true;

    }


    //if clicked on an inactive features, start that feature
    private void checkMouseDoubleClick()
    {

            zoomedIn();

    }

    //focus on the active feature
    public void zoomedIn() {
        zoomIn = true;
        FeatureManager.instance.ResetFeature();//resets other active features
        ActivateFeature();
    }

    //reset active feature
    public void Reset() {
        feature.SetActive(false);
        iconSprite.enabled = true;
        hover.GetComponent<SpriteRenderer>().sprite = defaultHover;
        hover.SetActive(false);
        hoverHighlight.SetActive(false);
        //    mouseClicksStarted = false;
        hasExited = true;
    }

    //activate feature in focus
    public void ActivateFeature() {
        feature.SetActive(true);
        iconSprite.enabled = false;
        hover.SetActive(false);
        hoverHighlight.SetActive(true);
        hasExited = true;
    }
}
