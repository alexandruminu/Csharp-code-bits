using UnityEngine;

public static class GameTime
{
    public static void Pause()
    {
        Time.timeScale = 0;
    }
    public static void Resume()
    {
        Time.timeScale = 1;
    }
}
