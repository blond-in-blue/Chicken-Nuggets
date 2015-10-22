//I do not clame the rights to this code.
//All concepts came from the video:
//https://www.youtube.com/watch?v=QxRAIjXdfFU

using UnityEngine;
using System.Collections;

public class MenuBehavior : MonoBehaviour {


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
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
        }

        else
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        }
    }



	
}
