using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
    public void PlaySoundOnce(AudioClip audioClip)
    {
        StartCoroutine(PlaySoundCoroutine(audioClip));
    }

    IEnumerator PlaySoundCoroutine(AudioClip audioClip)
    {
        GetComponent<AudioSource>().PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        Destroy(gameObject);
    }
}
