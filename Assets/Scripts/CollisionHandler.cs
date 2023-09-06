using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool colisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            colisionDisabled = !colisionDisabled;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || colisionDisabled) { return; }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("hit start");
                break;
            case "Finish":

                startSuccessSequence();
                break;
            default:

                startCrashSequence();
                break;
        }
    }

    void startSuccessSequence()
    {
        successParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        GetComponent<Movement>().enabled = false;
        isTransitioning = true;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void startCrashSequence()
    {
        crashParticles.Play();
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
