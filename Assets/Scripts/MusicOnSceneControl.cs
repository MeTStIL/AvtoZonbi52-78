
public class MusicOnSceneControl : Sounds
{
    private bool isPaused;

    private void Start()
    {
        PlaySound(objectSounds[0], p1: 1f, p2: 1f, fadeInTime: 0f);
    }

    private void Update()
    {
        isPaused = PauseMenu.IsPaused;
        if(isPaused)
            PauseMusic();
        else
            ResumeMusic();
    }
}
