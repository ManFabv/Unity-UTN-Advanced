using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIAnimator : MonoBehaviour
{
    private enum UIAnimatorState 
    {
        None,
        FadeIn,
        FadeOut
    }

#pragma warning disable 0649
    [SerializeField] private UIAnimatorState InitialState = UIAnimatorState.None;
#pragma warning restore 0649

    private Animator cachedAnimator;
    
    private void Awake()
    {
        string tweenToExecute = string.Empty;
        if(InitialState != UIAnimatorState.None)
        {
            tweenToExecute = InitialState.ToString();
        }
        if(!string.IsNullOrEmpty(tweenToExecute))
        {
            cachedAnimator = this.GetComponent<Animator>();
            cachedAnimator.SetTrigger(tweenToExecute);
        }
    }

    public void FadeIn()
    {
        cachedAnimator.SetTrigger(UIAnimatorState.FadeIn.ToString());
    }
    
    public void FadeOut()
    {
        cachedAnimator.SetTrigger(UIAnimatorState.FadeOut.ToString());
    }
}
