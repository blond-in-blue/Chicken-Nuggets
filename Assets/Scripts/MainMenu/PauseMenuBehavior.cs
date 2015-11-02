using UnityEngine;
using System.Collections;

public class PauseMenuBehavior : MonoBehaviour {

    // Use this for initialization
    private Animator _animator;
    private CanvasGroup _canvasGroup;



    //Basicly controlls the animator controller
    public bool IsOpen
    {
        get
        {
            return _animator.GetBool("IsOpen");
        }
        set
        {
            _animator.SetBool("IsOpen", value);
        }

    }

    //Sets the amake state of the menues
    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();

        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);
    }


    //Opens closes depending on state
    public void Update()
    {
        if (!(_animator.GetCurrentAnimatorStateInfo(0).IsName("Open")))
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        }

        else
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
        }
    }




}