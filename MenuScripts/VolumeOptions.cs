using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour {

    private GameObject volume;
    private float gameVolume = 100;
	// Use this for initialization
	void Start () {
        volume = GameObject.Find("VolumeLevel");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReduceSound()
    {
        if(gameVolume == 0)
        {
            return;
        }
        else
        {
            gameVolume -= 1;
            AudioListener.volume -= gameVolume / 100;
            volume.GetComponent<Text>().text = gameVolume.ToString();
        }
        
        
    }

    public void IncreaseSound()
    {
        if (gameVolume == 100)
        {

            return;
        }
        else
        {
            gameVolume += 1;
            AudioListener.volume += gameVolume / 100;
            volume.GetComponent<Text>().text = gameVolume.ToString();
        }
    }
}
