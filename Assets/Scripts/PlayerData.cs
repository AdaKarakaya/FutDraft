using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public string teamName;
    public int overallRating;
    public int pace;
    public int shooting;
    public int passing;
    public int defending;
    public int dribbling;
    public Sprite playerImage; // Şu an kullanılmıyor ama tutulabilir
    public int jerseyNumber; // Şu an kullanılmıyor ama tutulabilir
    public string position; // GK, CB, LB, RB, CM, CAM, LW, RW, ST gibi spesifik pozisyonlar

    public PlayerData(string name, string team, int overall, int pace, int shooting, int passing, int defending, int dribbling, Sprite image, int jersey, string position)
    {
        playerName = name;
        teamName = team;
        overallRating = overall;
        this.pace = pace;
        this.shooting = shooting;
        this.passing = passing;
        this.defending = defending;
        this.dribbling = dribbling;
        playerImage = image;
        jerseyNumber = jersey;
        this.position = position;
    }
}