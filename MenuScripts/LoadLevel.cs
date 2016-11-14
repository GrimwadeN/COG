using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class LoadLevel : MonoBehaviour, ISelectHandler
{

    public string levelSelect;

    LevelSelectButtonScroller btnScroller;
    
    public void Start()
    {
        btnScroller = this.transform.parent.GetComponent<LevelSelectButtonScroller>();
    }

    public void SelectLevel()
    {
        SceneManager.UnloadScene(SceneManager.GetActiveScene());
        SceneManager.LoadScene(levelSelect);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (btnScroller == null)
            return;

        AxisEventData axisData = eventData as AxisEventData;
        if (axisData == null)
            return;

        if (axisData.moveDir == MoveDirection.Down)
            btnScroller.MoveDown();
        else if (axisData.moveDir == MoveDirection.Up)
            btnScroller.MoveUp();

    }
}
