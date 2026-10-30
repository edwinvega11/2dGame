using UnityEngine;

// play short sounds for game events
[RequireComponent(typeof(AudioSource))]
public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance { get; private set; }

    public AudioClip shot;
    public AudioClip creepHit;
    public AudioClip soul;
    public AudioClip playerHit;
    public AudioClip upgrade;
    public AudioClip waveClear;
    public AudioClip victory;
    public AudioClip defeat;

    AudioSource source;
    float nextHitSound;

    void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void Shoot() { source.PlayOneShot(shot, 0.2f); }
    public void Soul() { source.PlayOneShot(soul, 0.4f); }
    public void HurtPlayer() { source.PlayOneShot(playerHit, 0.65f); }
    public void Upgrade() { source.PlayOneShot(upgrade, 0.6f); }
    public void ClearWave() { source.PlayOneShot(waveClear, 0.5f); }
    public void Win() { source.PlayOneShot(victory, 0.7f); }
    public void Lose() { source.PlayOneShot(defeat, 0.7f); }

    public void HitCreep()
    {
        // keep overlapping hits from becoming too loud
        if (Time.unscaledTime < nextHitSound) return;
        nextHitSound = Time.unscaledTime + 0.06f;
        source.PlayOneShot(creepHit, 0.3f);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
