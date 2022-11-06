using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CreakPlayer : MonoBehaviour
{
    public float playEverySeconds = 80f;
    
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        while (true)
        {
            _audioSource.Play();
            yield return new WaitForSeconds(playEverySeconds);
        }
    }
}
