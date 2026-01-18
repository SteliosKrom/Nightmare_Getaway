using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    private float typingSpeed = 0.05f; // Seconds per character

    [SerializeField] private bool introTitleTextCoroutineRunning = false;

    [Header("AUDIO")]
    [SerializeField] private AudioSource typewriterAudioSource;

    [SerializeField] private AudioClip typewriterAudioClip;

    public bool CoroutineIsRunning { get => introTitleTextCoroutineRunning; set => introTitleTextCoroutineRunning = value; }

    // Play sound for each character except spaces
    public IEnumerator PlayStoryIntroTextTypeWriterDelay(TextMeshProUGUI storyIntroText, string fullStoryIntroText)
    {
        storyIntroText.text = "";

        foreach (char c in fullStoryIntroText)
        {
            storyIntroText.text += c;

            if (!char.IsWhiteSpace(c))
            {
                typewriterAudioSource.PlayOneShot(typewriterAudioClip);
            }
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        introTitleTextCoroutineRunning = false;
    }

    public IEnumerator PlayStoryIntroTitleTextTypeWriterDelay(TextMeshProUGUI storyTitleIntroText, string fullStoryTitleIntroText)
    {
        introTitleTextCoroutineRunning = true;

        storyTitleIntroText.text = "";

        foreach (char c in fullStoryTitleIntroText)
        {
            storyTitleIntroText.text = storyTitleIntroText.text.ToUpper() + c;

            if (!char.IsWhiteSpace(c))
            {
                typewriterAudioSource.PlayOneShot(typewriterAudioClip);
            }
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}
