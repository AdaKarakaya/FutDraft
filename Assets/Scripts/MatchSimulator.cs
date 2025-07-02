using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class MatchSimulator : MonoBehaviour
{
    [Header("UI Elemanları")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI goalScorersText;
    public Button mainMenuButton;

    [Header("Panel Referansları")]
    public GameObject draftPanel;
    public GameObject teamBuildingPanel;
    public GameObject matchResultPanel;

    private List<PlayerData> playerTeam;
    private List<PlayerData> opponentTeam;

    void OnEnable()
    {
        Debug.Log("MatchSimulator: OnEnable çağrıldı. (Bu metodun sadece buton listener'ı için kullanıldığından emin olun.)");
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            Debug.Log("MatchSimulator: MainMenuButton listener eklendi.");
        }
        else
        {
            Debug.LogError("MatchSimulator: HATA: MainMenuButton referansı atanmamış!");
        }
    }

    void OnDisable()
    {
        Debug.Log("MatchSimulator: OnDisable çağrıldı.");
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(GoToMainMenu);
        }
    }

    public void SetupMatchScreen() // TeamBuilder'dan çağrılacak
    {
        Debug.Log("MatchSimulator: SetupMatchScreen çağrıldı. Maç ekranı hazırlanıyor.");

        if (GameManager.Instance != null)
        {
            playerTeam = GameManager.Instance.GetPlayerTeam();
            Debug.Log($"MatchSimulator: Kendi takımınızda {playerTeam.Count} oyuncu var (SetupMatchScreen).");

            if (playerTeam.Count != 11)
            {
                Debug.LogWarning($"MatchSimulator: Kendi takımınızda 11 oyuncu yok! ({playerTeam.Count}). Maç simülasyonu etkilenebilir.");
            }

            opponentTeam = GenerateBalancedOpponentTeam(playerTeam);
            Debug.Log($"MatchSimulator: Rakip takımda {opponentTeam.Count} oyuncu var.");

            SimulateMatch();
        }
        else
        {
            Debug.LogError("MatchSimulator: HATA: GameManager bulunamadı! SetupMatchScreen'de. Test takımları kullanılıyor.");
            // TEST takımlarını bu pozisyon kısaltmalarına göre güncelledim
            playerTeam = new List<PlayerData>();
            playerTeam.Add(new PlayerData("Kendi GK", "Test", 80,0,0,0,0,0,null,1,"GK"));
            playerTeam.Add(new PlayerData("Kendi CB1", "Test", 75,0,0,0,0,0,null,1,"CB"));
            playerTeam.Add(new PlayerData("Kendi CB2", "Test", 75,0,0,0,0,0,null,1,"CB"));
            playerTeam.Add(new PlayerData("Kendi LB", "Test", 75,0,0,0,0,0,null,1,"LB"));
            playerTeam.Add(new PlayerData("Kendi RB", "Test", 75,0,0,0,0,0,null,1,"RB"));
            playerTeam.Add(new PlayerData("Kendi CM1", "Test", 82,0,0,0,0,0,null,1,"CM"));
            playerTeam.Add(new PlayerData("Kendi CM2", "Test", 82,0,0,0,0,0,null,1,"CM"));
            playerTeam.Add(new PlayerData("Kendi CAM", "Test", 82,0,0,0,0,0,null,1,"CAM"));
            playerTeam.Add(new PlayerData("Kendi LW", "Test", 88,0,0,0,0,0,null,1,"LW"));
            playerTeam.Add(new PlayerData("Kendi RW", "Test", 88,0,0,0,0,0,null,1,"RW"));
            playerTeam.Add(new PlayerData("Kendi ST", "Test", 88,0,0,0,0,0,null,1,"ST"));

            opponentTeam = new List<PlayerData>();
            opponentTeam.Add(new PlayerData("Rakip GK", "Test", 78,0,0,0,0,0,null,1,"GK"));
            opponentTeam.Add(new PlayerData("Rakip CB1", "Test", 73,0,0,0,0,0,null,1,"CB"));
            opponentTeam.Add(new PlayerData("Rakip CB2", "Test", 73,0,0,0,0,0,null,1,"CB"));
            opponentTeam.Add(new PlayerData("Rakip LB", "Test", 73,0,0,0,0,0,null,1,"LB"));
            opponentTeam.Add(new PlayerData("Rakip RB", "Test", 73,0,0,0,0,0,null,1,"RB"));
            opponentTeam.Add(new PlayerData("Rakip CM1", "Test", 80,0,0,0,0,0,null,1,"CM"));
            opponentTeam.Add(new PlayerData("Rakip CM2", "Test", 80,0,0,0,0,0,null,1,"CM"));
            opponentTeam.Add(new PlayerData("Rakip CAM", "Test", 80,0,0,0,0,0,null,1,"CAM"));
            opponentTeam.Add(new PlayerData("Rakip LW", "Test", 86,0,0,0,0,0,null,1,"LW"));
            opponentTeam.Add(new PlayerData("Rakip RW", "Test", 86,0,0,0,0,0,null,1,"RW"));
            opponentTeam.Add(new PlayerData("Rakip ST", "Test", 86,0,0,0,0,0,null,1,"ST"));
            
            scoreText.text = "TEST - 0 - 0";
            goalScorersText.text = "Maç başlatılamadı (GameManager yok).";
        }

        if (draftPanel != null) draftPanel.SetActive(false);
        if (teamBuildingPanel != null) teamBuildingPanel.SetActive(false);
        if (matchResultPanel != null) matchResultPanel.SetActive(true);
    }

    List<PlayerData> GenerateBalancedOpponentTeam(List<PlayerData> myTeam)
    {
        Debug.Log("MatchSimulator: GenerateBalancedOpponentTeam çağrıldı.");
        if (GameManager.Instance == null)
        {
            Debug.LogError("MatchSimulator: HATA: GenerateBalancedOpponentTeam GameManager.Instance NULL!");
            return new List<PlayerData>();
        }
        
        List<PlayerData> opponentTeamList = new List<PlayerData>();
        List<PlayerData> allAvailablePlayers = new List<PlayerData>(GameManager.Instance.allPlayers);

        foreach (var playerInMyTeam in myTeam)
        {
            allAvailablePlayers.RemoveAll(p => p.playerName == playerInMyTeam.playerName && p.teamName == playerInMyTeam.teamName);
        }

        // 4-3-3 Dizilişi için istenen pozisyon dağılımı
        Dictionary<string, int> desiredPositions = new Dictionary<string, int>
        {
            {"GK", 1},
            {"CB", 2},
            {"LB", 1},
            {"RB", 1},
            {"CM", 2},
            {"CAM", 1},
            {"LW", 1},
            {"RW", 1},
            {"ST", 1}
        };

        float myTeamOverall = CalculateTeamOverall(myTeam);
        float targetOpponentOverall = myTeamOverall + Random.Range(-5f, 5f);

        Debug.Log($"MatchSimulator: Rakip takım oluşturuluyor. Kendi takım overall: {myTeamOverall}, Hedef rakip overall: {targetOpponentOverall}");

        foreach (var entry in desiredPositions)
        {
            string position = entry.Key;
            int count = entry.Value;

            List<PlayerData> playersInPosition = allAvailablePlayers
                .Where(p => p.position == position)
                .OrderByDescending(p => p.overallRating)
                .ToList();

            for (int i = 0; i < count; i++)
            {
                PlayerData bestFitPlayer = null;
                float minOverallDifference = float.MaxValue;

                // Hedef overall'a en yakın oyuncuyu bul
                foreach (PlayerData p in playersInPosition)
                {
                    if (!opponentTeamList.Contains(p))
                    {
                        // Mevcut takımın ortalama overall'ını geçici olarak hesapla
                        float currentOpponentOverall = CalculateTeamOverall(opponentTeamList.Concat(new List<PlayerData> {p}).ToList());
                        float diff = Mathf.Abs(currentOpponentOverall - targetOpponentOverall);

                        if (diff < minOverallDifference)
                        {
                            minOverallDifference = diff;
                            bestFitPlayer = p;
                        }
                    }
                }

                if (bestFitPlayer != null)
                {
                    opponentTeamList.Add(bestFitPlayer);
                    allAvailablePlayers.Remove(bestFitPlayer);
                    Debug.Log($"MatchSimulator: Rakip takıma {bestFitPlayer.playerName} ({bestFitPlayer.position}, {bestFitPlayer.overallRating}) eklendi.");
                }
                else
                {
                    Debug.LogWarning($"MatchSimulator: Rakip takım için {position} pozisyonunda yeterli oyuncu bulunamadı. ({i + 1}/{count})");
                    if (allAvailablePlayers.Count > 0)
                    {
                        PlayerData randomPlayer = allAvailablePlayers[Random.Range(0, allAvailablePlayers.Count)];
                        opponentTeamList.Add(randomPlayer);
                        allAvailablePlayers.Remove(randomPlayer);
                        Debug.LogWarning($"MatchSimulator: Rakip takıma rastgele {randomPlayer.playerName} ({randomPlayer.position}, {randomPlayer.overallRating}) eklendi (Pozisyon eksikliği).");
                    }
                }
            }
        }
        
        if (opponentTeamList.Count < 11)
        {
            Debug.LogWarning($"MatchSimulator: Rakip takımda eksik oyuncu var. Sadece {opponentTeamList.Count} oyuncu toplanabildi. Kalan yerler rastgele dolduruluyor.");
            while (opponentTeamList.Count < 11 && allAvailablePlayers.Count > 0)
            {
                PlayerData randomPlayer = allAvailablePlayers[Random.Range(0, allAvailablePlayers.Count)];
                opponentTeamList.Add(randomPlayer);
                allAvailablePlayers.Remove(randomPlayer);
                Debug.LogWarning($"MatchSimulator: Rakip takıma eksik kalan yer için rastgele {randomPlayer.playerName} eklendi.");
            }
        }
        
        return opponentTeamList;
    }

    void SimulateMatch()
    {
        Debug.Log("MatchSimulator: SimulateMatch çağrıldı.");
        if (playerTeam == null || playerTeam.Count == 0)
        {
            Debug.LogError("MatchSimulator: HATA: Kendi takımınız boş! Maç simülasyonu yapılamıyor.");
            if (scoreText != null) scoreText.text = "HATA";
            if (goalScorersText != null) goalScorersText.text = "Takım Boş!";
            return;
        }
        if (opponentTeam == null || opponentTeam.Count == 0)
        {
            Debug.LogError("MatchSimulator: HATA: Rakip takım boş! Maç simülasyonu yapılamıyor.");
            if (scoreText != null) scoreText.text = "HATA";
            if (goalScorersText != null) goalScorersText.text = "Rakip Takım Boş!";
            return;
        }

        float playerTeamOverall = CalculateTeamOverall(playerTeam);
        float opponentTeamOverall = CalculateTeamOverall(opponentTeam);

        int playerGoals = 0;
        int opponentGoals = 0;
        List<string> goalScorers = new List<string>();

        int totalAttacks = Random.Range(5, 15);
        float playerAttackStrength = playerTeamOverall * 0.1f + CalculateTeamAttackStrength(playerTeam) * 0.9f;
        float opponentDefenseStrength = opponentTeamOverall * 0.1f + CalculateTeamDefenseStrength(opponentTeam) * 0.9f;

        // Rakip takımın atak gücü ve kendi takımınızın defans gücü
        float opponentAttackStrength = opponentTeamOverall * 0.1f + CalculateTeamAttackStrength(opponentTeam) * 0.9f;
        float playerDefenseStrength = playerTeamOverall * 0.1f + CalculateTeamDefenseStrength(playerTeam) * 0.9f;


        for (int i = 0; i < totalAttacks; i++)
        {
            // Kendi takımınızın atak yapma olasılığı
            if (Random.value < (playerAttackStrength / (playerAttackStrength + opponentDefenseStrength)))
            {
                // Kendi takımınız gol atma olasılığı
                if (Random.value < 0.6f) 
                {
                    playerGoals++;
                    // Golcü seçimi: Spesifik hücum pozisyonlarına öncelik ver
                    var potentialScorers = playerTeam
                        .Where(p => p.position == "ST" || p.position == "LW" || p.position == "RW" || p.position == "CAM" || p.position == "CM") // Kaleciler gol atmasın
                        .OrderByDescending(p => p.shooting + p.overallRating * 0.5f) // Şut ve overall'a göre sırala
                        .ToList();

                    if (potentialScorers.Any())
                    {
                        int pickIndex = Random.Range(0, Mathf.Min(potentialScorers.Count, 3)); // En iyi 3 golcü arasından rastgele
                        goalScorers.Add(potentialScorers[pickIndex].playerName + " (K)");
                    }
                    else
                    {
                        goalScorers.Add("Bilinmeyen Golcü (K)"); // Bu durum oluşmamalı eğer pozisyonlar doğruysa
                    }
                }
            }
            else
            {
                // Rakip takım atak yaptı
                if (Random.value < 0.6f)
                {
                    opponentGoals++;
                    // Rakip golcü seçimi: Spesifik hücum pozisyonlarına öncelik ver
                    var potentialOpponentScorers = opponentTeam
                        .Where(p => p.position == "ST" || p.position == "LW" || p.position == "RW" || p.position == "CAM" || p.position == "CM")
                        .OrderByDescending(p => p.shooting + p.overallRating * 0.5f)
                        .ToList();

                    if (potentialOpponentScorers.Any())
                    {
                        int pickIndex = Random.Range(0, Mathf.Min(potentialOpponentScorers.Count, 3));
                        goalScorers.Add(potentialOpponentScorers[pickIndex].playerName + " (R)");
                    }
                    else
                    {
                        goalScorers.Add("Bilinmeyen Golcü (R)");
                    }
                }
            }
        }

        // Skor ve golcüleri UI'a yaz
        if (scoreText != null) scoreText.text = $"{playerGoals} - {opponentGoals}"; else Debug.LogError("MatchSimulator: HATA: scoreText NULL!");
        if (goalScorersText != null) goalScorersText.text = "Gol Atanlar:\n" + (goalScorers.Any() ? string.Join("\n", goalScorers) : "Kimse gol atmadı."); else Debug.LogError("MatchSimulator: HATA: goalScorersText NULL!");

        if (playerGoals > opponentGoals) Debug.Log("Maçı Kazandınız!");
        else if (playerGoals < opponentGoals) Debug.Log("Maçı Kaybettiniz.");
        else Debug.Log("Maç Berabere Bitti.");
    }

    private float CalculateTeamOverall(List<PlayerData> team)
    {
        if (team == null || team.Count == 0) return 0;
        float totalOverall = 0;
        foreach (PlayerData player in team)
        {
            totalOverall += player.overallRating;
        }
        return totalOverall / team.Count;
    }

    private float CalculateTeamAttackStrength(List<PlayerData> team)
    {
        if (team == null || team.Count == 0) return 0;
        float attackScore = 0;
        foreach (PlayerData player in team)
        {
            // Hücum pozisyonları daha çok katkı yapar
            if (player.position == "ST" || player.position == "LW" || player.position == "RW")
            {
                attackScore += player.shooting * 0.7f + player.dribbling * 0.2f + player.overallRating * 0.1f;
            }
            else if (player.position == "CAM") // CAM'ler hem şut hem pas
            {
                attackScore += player.shooting * 0.5f + player.passing * 0.3f + player.dribbling * 0.1f + player.overallRating * 0.1f;
            }
            else if (player.position == "CM") // CM'ler pas ve genel katkı
            {
                attackScore += player.passing * 0.5f + player.shooting * 0.2f + player.overallRating * 0.2f + player.dribbling * 0.1f;
            }
            // Defans ve kaleciler daha az katkıda bulunur
            else
            {
                attackScore += player.overallRating * 0.05f;
            }
        }
        return attackScore / team.Count;
    }

    private float CalculateTeamDefenseStrength(List<PlayerData> team)
    {
        if (team == null || team.Count == 0) return 0;
        float defenseScore = 0;
        foreach (PlayerData player in team)
        {
            // Defans ve kaleciler daha çok katkı yapar
            if (player.position == "CB" || player.position == "LB" || player.position == "RB")
            {
                defenseScore += player.defending * 0.7f + player.pace * 0.2f + player.overallRating * 0.1f;
            }
            else if (player.position == "GK")
            {
                defenseScore += player.defending * 0.8f + player.overallRating * 0.2f; // Kaleci için özel
            }
            else if (player.position == "CM") // CM'ler defansa da katkı sağlar
            {
                defenseScore += player.defending * 0.3f + player.overallRating * 0.2f;
            }
            // Forvet ve ofansif orta sahalar daha az katkıda bulunur
            else
            {
                defenseScore += player.overallRating * 0.05f;
            }
        }
        return defenseScore / team.Count;
    }

    void GoToMainMenu()
    {
        Debug.Log("MatchSimulator: Ana Menüye Dönülüyor (Draft Ekranı sıfırlanıyor)...");
        if (matchResultPanel != null) matchResultPanel.SetActive(false);
        if (draftPanel != null) draftPanel.SetActive(true);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetPlayerTeam();
        }
        
        DraftController draftController = FindAnyObjectByType<DraftController>(); 
        if (draftController != null)
        {
            draftController.InitializeDraft();
        }
        else
        {
            Debug.LogError("MatchSimulator: HATA: DraftController bulunamadı! Draft resetlenemedi.");
        }
    }
}