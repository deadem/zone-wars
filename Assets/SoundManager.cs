using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private static GameObject audioPrefab;
    public GameObject audioPrefabSource;
    public AudioClip shotSound;

    public static SoundManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioPrefab = audioPrefabSource;
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
