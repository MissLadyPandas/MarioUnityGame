using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource oneUp;
    public AudioSource backgroundMusic;
    public AudioSource block;
    public AudioSource coin;
    public AudioSource goomba;
    public AudioSource lifeLost;
    public AudioSource pause;
    public AudioSource pipe;
    public AudioSource powerUpAppears;
    public AudioSource powerUpMario;
    public AudioSource shell;
    public AudioSource starman;
    public AudioSource stompNoDamage;
    public AudioSource superJump;
    public AudioSource smallJump;
    public AudioSource underground;
    public AudioSource CourseClear;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayOneUp()
    {
        oneUp.Play();
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }

    public void PlayBlock()
    {
        block.Play();
    }

    public void PlayCoin()
    {
        coin.Play();
    }

    public void PlayGoomba()
    {
        goomba.Play();
    }

    public void PlayLifeLost()
    {
        lifeLost.Play();
    }

    public void PlayPause()
    {
        pause.Play();
    }

    public void PlayPipe()
    {
        pipe.Play();
    }

    public void PlayPowerUpAppears()
    {
        powerUpAppears.Play();
    }

    public void PlayPowerUpMario()
    {
        powerUpMario.Play();
    }

    public void PlayShell()
    {
        shell.Play();
    }

    public void PlayStarman(float duration)
    {
        starman.loop = true;
        starman.Play();
        Invoke("StopStarman", duration);
    }

    private void StopStarman()
    {
        starman.loop = false;
        starman.Stop();
    }

    public void PlayStompNoDamage()
    {
        stompNoDamage.Play();
    }

    public void PlaySuperJump()
    {
        superJump.Play();
    }

    public void PlaySmallJump()
    {
        smallJump.Play();
    }

    public void PlayUnderground()
    {
        underground.Play();
    }

    public void PlayCourseClear()
    {
        CourseClear.Play();
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
    }
}
