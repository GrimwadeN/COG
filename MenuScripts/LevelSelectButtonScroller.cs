using UnityEngine;
using System.Collections;

public class LevelSelectButtonScroller : MonoBehaviour {

    
    public float moveSpeed = 500;

    private Vector3 startingPosition;
    private float lerpMoveTime = 0;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float moveDistance = 246;
    // Use this for initialization
    void Start ()
    {
        startingPosition = this.transform.localPosition;
        targetPos = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            targetPos = startingPosition;
            startPos = this.transform.localPosition;
        }


        lerpMoveTime += Time.deltaTime * moveSpeed;
        Vector3 dir = Vector3.Normalize(targetPos - transform.localPosition);
        transform.localPosition = Vector3.Lerp(startPos, targetPos, lerpMoveTime);

    }

    public void MoveUp()
    {
        startPos = this.transform.localPosition;
        targetPos.y -= moveDistance;
        lerpMoveTime = 0;
    }

    public void MoveDown()
    {
        startPos = this.transform.localPosition;
        targetPos.y += moveDistance;
        lerpMoveTime = 0;
    }
}
