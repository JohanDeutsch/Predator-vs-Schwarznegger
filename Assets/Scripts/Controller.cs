using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public static bool isOnGame = true;
    public static int game_score = 0;
    private static int last_game_score = 0;
    public static int record_game_score;

    private static AudioSource audioGameBackground;
    public void Start()
    {
        audioGameBackground = GameObject.Find("audio_game_background").GetComponent<AudioSource>();
    }
    public static void gameOver()
    {
        isOnGame = false;
        if (game_score > PlayerPrefs.GetInt("Score", 0))
        {
            PlayerPrefs.SetInt("Score", game_score);
            record_game_score = game_score;
        }
        else
        {
            record_game_score = PlayerPrefs.GetInt("Score");
        }
    }

    public void restartGame()
    {
        game_score = 0; last_game_score = 0;
        GameVariables.current_fall_speed = GameVariables.FALL_SPEED;
        GameVariables.curent_spawnloop_speed = GameVariables.SPAWNLOOP_SPAWN;
        isOnGame = true;
        SceneManager.LoadScene("MainScene");
    }

    public static void scoreIncrement()
    {
        game_score++;
        if (game_score - last_game_score > GameVariables.complication_period)
        {
            complicateGame();
            last_game_score = game_score;
        }

        if (game_score > 10 && !audioGameBackground.isPlaying)
        {
            audioGameBackground.Play();
        }
    }

    private static void complicateGame()
    {
        GameVariables.curent_spawnloop_speed -= GameVariables.curent_spawnloop_speed * GameVariables.PERCENT_COMPLICATING_LOOP / 100;
        GameVariables.current_fall_speed += GameVariables.current_fall_speed * GameVariables.PERCENT_COMPLICATING_FALL_SPEED / 100;
    }
}