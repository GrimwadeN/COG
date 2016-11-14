using UnityEngine;
using System.Collections;

public class MainMenuMandyAnims : MonoBehaviour {

    public Animator anim;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        StartCoroutine(PlayRandomAnimation(1));

	}

    public IEnumerator PlayRandomAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        anim.SetInteger("mandyIdleRandom", RandomIdle());

    }

    int RandomIdle()
    {
        return Random.Range(1, 100);
    }
}
