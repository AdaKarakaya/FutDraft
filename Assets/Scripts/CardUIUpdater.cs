using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIUpdater : MonoBehaviour
{
    public Image playerImage;
    public TextMeshProUGUI playerNameText;
    public Image teamLogoImage;
    public TextMeshProUGUI overallRatingText;

    public Sprite gsLogo;
    public Sprite fbLogo;
    public Sprite bjkLogo;

    public void UpdateCardUI(PlayerData player)
    {
        if (playerImage != null)
        {
            playerImage.sprite = null;
        }

        playerNameText.text = player.playerName;
        overallRatingText.text = player.overallRating.ToString();

        if (teamLogoImage != null)
        {
            switch (player.teamName)
            {
                case "Galatasaray":
                    teamLogoImage.sprite = gsLogo;
                    break;
                case "Fenerbahçe":
                    teamLogoImage.sprite = fbLogo;
                    break;
                case "Beşiktaş":
                    teamLogoImage.sprite = bjkLogo;
                    break;
                default:
                    Debug.LogWarning($"CardUIUpdater: Takım logosu bulunamadı veya eşleşmedi: {player.teamName}. Varsayılan atanıyor.");
                    teamLogoImage.sprite = null;
                    break;
            }
        }
        else
        {
            Debug.LogError("CardUIUpdater: teamLogoImage UI elemanı atanmamış! Lütfen Inspector'dan atayın.");
        }
    }
}