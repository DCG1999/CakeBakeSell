using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip buySound;
    [SerializeField] AudioClip ObjectSelectSound;
    [SerializeField] AudioClip ObjectDeSelectSound;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string op)
    {
        switch(op)
        {
            case "Buy":
                audioSource.PlayOneShot(buySound);
                break;
            case "Select":
                audioSource.PlayOneShot(ObjectSelectSound);
                break;
            case "Deselect":
                audioSource.PlayOneShot(ObjectDeSelectSound);
                break;
        }
    }
}
