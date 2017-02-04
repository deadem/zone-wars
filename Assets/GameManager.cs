using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameObject audioPrefab;
    public GameObject audioPrefabSource;
    public AudioClip shotSound;
	public Messages messages;

    public static GameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioPrefab = audioPrefabSource;
			messages = new Messages();
			messages.init();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    public void Shot()
    {
        GameObject go = GameObject.Instantiate(audioPrefab) as GameObject;
        go.transform.parent = instance.transform;

        Audio a = go.GetComponent<Audio>();
        a.PlaySoundOnce(shotSound);
    }
}
