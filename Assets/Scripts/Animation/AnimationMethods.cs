using System.Collections;
using UnityEngine;



public class AnimationMethods : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string trigger;
    [SerializeField] private float animationDelay = 1f;
    private void turnYourselfOff()
    {
        this.gameObject.SetActive(false);
    }



    private IEnumerator animateWithDelay()
    {
        animator.speed = 0f;
        yield return new WaitForSeconds(animationDelay);
        animator.speed = 1f;
    }
}
