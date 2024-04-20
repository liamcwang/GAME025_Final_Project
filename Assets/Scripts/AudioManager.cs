using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip levelMusicClip; // Reference to the level music AudioClip
    public AudioClip bossMusicClip; // Reference to the boss music AudioClip

    private bool levelMusicPlaying = true; // Flag to track whether level music is currently playing
    private AudioSource audioSource; // Reference to the AudioSource component attached to this GameObject

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GameManager.PlayerCamera.GetComponent<AudioSource>();

        // Play the level music when the game starts
        PlayLevelMusic();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger zone
        if (other.tag == "musictrigger")
        {

            Debug.Log("Player exited the trigger zone.");

            // Stop the current music
            audioSource.Stop();

            // Play the boss music
            audioSource.clip = bossMusicClip;
            audioSource.Play();

            // Update flag to indicate boss music is playing
            levelMusicPlaying = false;
        }
        else
        {
            Debug.Log("Other GameObject entered the trigger zone. Tag: " + other.tag);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits the trigger zone
        if (other.CompareTag("Player"))
        {

            Debug.Log("Player exited the trigger zone.");

            // Stop the current music
            audioSource.Stop();

            // Play the level music
            PlayLevelMusic();

            // Update flag to indicate level music is playing
            levelMusicPlaying = true;
        }
        else
        {
            Debug.Log("Other GameObject entered the trigger zone. Tag: " + other.tag);
        }
    }

    void PlayLevelMusic()
    {
        // Set the default music clip to the level music
        audioSource.clip = levelMusicClip;
        // Ensure the default music clip is set to loop
        audioSource.loop = true;
        // Play the level music
        audioSource.Play();
    }
}
