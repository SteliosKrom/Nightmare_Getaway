using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LightingEffect : MonoBehaviour
{
    private float startDelay;
    private float stopDelay = 1f;

    #region SERVICES
    private GameManager gameManager;
    #endregion

    #region PARTICLES
    [Header("PARTICLES")]
    [SerializeField] private ParticleSystem lightingParticle;
    #endregion

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();

        startDelay = Random.Range(40, 60);
        StartCoroutine(StartLightingDelay());
    }

    private IEnumerator StartLightingDelay()
    {
        lightingParticle.Stop();
        yield return new WaitForSeconds(startDelay);
        lightingParticle.Play();
        yield return new WaitForSeconds(stopDelay);
        lightingParticle.Stop();

        while (true)
        {
            if (gameManager.CurrentGameState != GameState.OnPause
                && gameManager.CurrentMenuState != MenuState.OnGameSettings)
            {
                yield return new WaitForSeconds(startDelay);
                lightingParticle.Play();
                yield return new WaitForSeconds(stopDelay);
                lightingParticle.Stop();
            }
            else
            {
                lightingParticle.Stop();
            }
        }
    }
}
