using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class DraftController : MonoBehaviour
{
    [Header("UI Elemanları")]
    public GameObject cardPrefab;
    public Transform cardSpawnParent;
    public TextMeshProUGUI currentPositionText;

    [Header("Draft Ayarları")]
    public int numberOfCardsToDraft = 3;
    public int maxDraftRounds = 11; // 4-3-3 dizilişi için toplam 11 oyuncu

    [Header("Panel Referansları")]
    public GameObject draftPanel;
    public GameObject teamBuildingPanel;
    public GameObject matchResultPanel;

    private List<PlayerData> availablePlayersForDraft;
    private List<PlayerData> currentDraftOptions = new List<PlayerData>();
    
    private int currentDraftRound = 0;

    // 4-3-3 Dizilişi için spesifik pozisyonlar ve sıralama:
    // 1 GK, 2 CB, 1 LB, 1 RB, 2 CM, 1 CAM, 1 LW, 1 RW, 1 ST = toplam 11 oyuncu
    private List<(string position, int count)> draftPositions = new List<(string, int)>
    {
        ("GK", 1),      // 1 Kaleci
        ("CB", 1),      // 1. Stoper
        ("CB", 1),      // 2. Stoper
        ("LB", 1),      // Sol Bek
        ("RB", 1),      // Sağ Bek
        ("CM", 1),      // 1. Merkez Orta Saha
        ("CM", 1),      // 2. Merkez Orta Saha
        ("CAM", 1),     // Ofansif Orta Saha
        ("LW", 1),      // Sol Kanat
        ("RW", 1),      // Sağ Kanat
        ("ST", 1)       // Santrafor
    };

    // Script aktif hale geldiğinde
    void OnEnable()
    {
        Debug.Log("DraftController: OnEnable çağrıldı. InitializeDraft çağrılıyor.");
        InitializeDraft();
    }

    public void InitializeDraft()
    {
        Debug.Log("DraftController: InitializeDraft çağrıldı. Yeni bir draft başlatılıyor.");
        
        if (GameManager.Instance != null)
        {
            availablePlayersForDraft = new List<PlayerData>(GameManager.Instance.allPlayers);
            GameManager.Instance.ResetPlayerTeam();
        }
        else
        {
            Debug.LogError("DraftController: HATA: GameManager bulunamadı! Test verileri kullanılıyor.");
            availablePlayersForDraft = new List<PlayerData>();
            availablePlayersForDraft.Add(new PlayerData("Test Kaleci", "GS", 80, 70, 70, 70, 70, 70, null, 1, "GK"));
            availablePlayersForDraft.Add(new PlayerData("Test CB 1", "FB", 85, 75, 75, 75, 75, 75, null, 2, "CB"));
            availablePlayersForDraft.Add(new PlayerData("Test CB 2", "BJK", 83, 72, 70, 70, 78, 65, null, 3, "CB"));
            availablePlayersForDraft.Add(new PlayerData("Test LB", "GS", 81, 80, 68, 72, 76, 60, null, 4, "LB"));
            availablePlayersForDraft.Add(new PlayerData("Test RB", "FB", 79, 78, 65, 68, 75, 58, null, 5, "RB"));
            availablePlayersForDraft.Add(new PlayerData("Test CM 1", "GS", 90, 80, 80, 80, 80, 80, null, 6, "CM"));
            availablePlayersForDraft.Add(new PlayerData("Test CM 2", "FB", 88, 78, 78, 85, 70, 82, null, 7, "CM"));
            availablePlayersForDraft.Add(new PlayerData("Test CAM", "BJK", 86, 75, 72, 80, 68, 78, null, 8, "CAM"));
            availablePlayersForDraft.Add(new PlayerData("Test LW", "GS", 84, 90, 80, 78, 30, 87, null, 9, "LW"));
            availablePlayersForDraft.Add(new PlayerData("Test RW", "FB", 88, 85, 85, 80, 40, 88, null, 10, "RW"));
            availablePlayersForDraft.Add(new PlayerData("Test ST", "BJK", 86, 82, 82, 75, 35, 85, null, 11, "ST"));
        }

        if (draftPanel != null) draftPanel.SetActive(true); else Debug.LogError("DraftController: HATA: draftPanel Inspector'da atanmamış!");
        if (teamBuildingPanel != null) teamBuildingPanel.SetActive(false); else Debug.LogError("DraftController: HATA: teamBuildingPanel Inspector'da atanmamış!");
        if (matchResultPanel != null) matchResultPanel.SetActive(false); else Debug.LogError("DraftController: HATA: matchResultPanel Inspector'da atanmamış!");

        currentDraftRound = 0;
        StartNewDraftRound();
    }

    public void StartNewDraftRound()
    {
        Debug.Log("DraftController: StartNewDraftRound çağrıldı.");

        if (currentDraftRound >= draftPositions.Count)
        {
            Debug.Log("DraftController: Tüm pozisyonlar için oyuncular seçildi! Takım kurma ekranına geçiliyor.");
            
            TeamBuilder teamBuilder = FindAnyObjectByType<TeamBuilder>();
            if (teamBuilder != null)
            {
                teamBuilder.SetupTeamScreen();
            }
            else
            {
                Debug.LogError("DraftController: HATA: TeamBuilder bulunamadı! Takım kurma ekranına geçilemiyor.");
            }

            if (draftPanel != null) draftPanel.SetActive(false);
            if (teamBuildingPanel != null) teamBuildingPanel.SetActive(true);
            
            return;
        }

        (string requiredPosition, int playersToSelectThisRound) = draftPositions[currentDraftRound];

        if (currentPositionText != null)
        {
            currentPositionText.text = $"Sıradaki Pozisyon: {requiredPosition}";
        }
        else
        {
            Debug.LogError("DraftController: HATA: currentPositionText UI elemanı atanmamış!");
        }

        foreach (Transform child in cardSpawnParent)
        {
            Destroy(child.gameObject);
        }
        currentDraftOptions.Clear();

        List<PlayerData> playersOfRequiredPosition = availablePlayersForDraft
            .Where(p => p.position == requiredPosition)
            .ToList();

        if (playersOfRequiredPosition.Count == 0)
        {
            Debug.LogWarning($"DraftController: HATA: '{requiredPosition}' pozisyonunda hiç oyuncu kalmadı veya bulunamadı. Bu tur atlanıyor.");
            currentDraftRound++;
            StartNewDraftRound();
            return;
        }

        int actualNumberOfCardsToDraft = numberOfCardsToDraft;
        if (playersOfRequiredPosition.Count < actualNumberOfCardsToDraft)
        {
            actualNumberOfCardsToDraft = playersOfRequiredPosition.Count;
            Debug.LogWarning($"DraftController: '{requiredPosition}' pozisyonunda {numberOfCardsToDraft} yerine {actualNumberOfCardsToDraft} oyuncu gösteriliyor çünkü yeterli oyuncu yok.");
        }

        List<PlayerData> tempSelection = new List<PlayerData>();
        for (int i = 0; i < actualNumberOfCardsToDraft; i++)
        {
            int randomIndex = Random.Range(0, playersOfRequiredPosition.Count);
            PlayerData selectedPlayer = playersOfRequiredPosition[randomIndex];
            tempSelection.Add(selectedPlayer);
            availablePlayersForDraft.Remove(selectedPlayer);
            playersOfRequiredPosition.RemoveAt(randomIndex);
        }
        currentDraftOptions = tempSelection;

        for (int i = 0; i < currentDraftOptions.Count; i++)
        {
            PlayerData player = currentDraftOptions[i];
            GameObject cardGO = Instantiate(cardPrefab, cardSpawnParent);
            CardUIUpdater cardUI = cardGO.GetComponent<CardUIUpdater>();

            if (cardUI != null)
            {
                cardUI.UpdateCardUI(player);
            }
            else
            {
                Debug.LogError("DraftController: HATA: CardUIUpdater scripti cardPrefab üzerinde bulunamadı!");
            }

            Button selectButton = cardGO.GetComponentInChildren<Button>();
            if (selectButton != null)
            {
                int playerIndex = i;
                selectButton.onClick.AddListener(() => SelectPlayer(currentDraftOptions[playerIndex]));
            }
            else
            {
                Debug.LogError("DraftController: HATA: Kart prefab üzerinde 'Button' bileşeni bulunamadı!");
            }
        }
    }

    public void SelectPlayer(PlayerData player)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddPlayerToTeam(player);
            Debug.Log($"{player.playerName} takıma eklendi! Mevcut takım oyuncu sayısı: {GameManager.Instance.GetPlayerTeam().Count}");
        }
        else
        {
            Debug.LogError("DraftController: HATA: GameManager bulunamadı, oyuncu takıma eklenemedi!");
        }

        currentDraftRound++;
        StartNewDraftRound();
    }
}