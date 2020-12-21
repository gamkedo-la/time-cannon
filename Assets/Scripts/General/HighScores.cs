/**
 * Description: Saved and loads high scores.
 * Authors: Kornel
 * Copyright: © 2020 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;

public enum LevelName
{
    City,
    Ocean,
    Countryside,
    ChaosDimension
}

public static class HighScores
{
    /// <summary>
    /// Gets high scores.
    /// </summary>
    /// <param name="levelName">Name of the level.</param>
    /// <param name="time">Name of game mode.</param>
    /// <returns></returns>
    public static int GetHighScore(LevelName levelName, TimePeriod time)
    {
        return PlayerPrefs.GetInt(string.Concat(levelName.ToString(), time.ToString()), 0);
    }

    /// <summary>
    /// Gets high scores.
    /// </summary>
    /// <param name="levelName">Name of the level.</param>
    /// <param name="mode">Name of game mode.</param>
    /// <returns></returns>
	public static int GetHighScore(string levelName, string mode)
    {
        return PlayerPrefs.GetInt(string.Concat(levelName, mode), 0);
    }

    /// <summary>
    /// Saves high score.
    /// </summary>
    /// <param name="levelName">Name of the level.</param>
    /// <param name="time">Name of game mode.</param>
    /// <param name="score">Score amount.</param>
    public static void SaveHighScore(LevelName levelName, TimePeriod time, int score)
    {
        PlayerPrefs.SetInt(string.Concat(levelName.ToString(), time.ToString()), score);
    }

    /// <summary>
    /// Saves high score.
    /// </summary>
    /// <param name="levelName">Name of the level.</param>
    /// <param name="time">Name of game mode.</param>
    /// <param name="score">Score amount.</param>
    public static void SaveHighScore(string levelName, string time, int score)
    {
        PlayerPrefs.SetInt(string.Concat(levelName, time), score);
    }
}
