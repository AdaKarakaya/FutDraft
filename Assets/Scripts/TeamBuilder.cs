using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamBuilder : MonoBehaviour
{
    [Header("UI Slotları (TextMeshProUGUI)")]
    // GK (1 adet)
    public TextMeshProUGUI gkSlotText;
    // DEFANS (4 adet: 2 CB, 1 LB, 1 RB) - Listeye atarken doğru sıralamaya dikkat!
    public List<TextMeshProUGUI> cbSlotTexts; // Örn: CB_Slot_1_Text, CB_Slot_2_Text
    public TextMeshProUGUI lbSlotText;
    public TextMeshProUGUI rbSlotText;
    // ORTA SAHA (3 adet: 2 CM, 1 CAM) - Listeye atarken doğru sıralamaya dikkat!
    public List<TextMeshProUGUI> cmSlotTexts; // Örn: CM_Slot_1_Text, CM_Slot_2_Text
    public TextMeshProUGUI camSlotText;
    // FORVET (3 adet: 1 LW, 1 RW, 1 ST)
    public TextMeshProUGUI lwSlotText;
    public TextMeshProUGUI rwSlotText;
    public TextMeshProUGUI stSlotText;

    [Header("Butonlar")]
    public Button startMatchButton;

    [Header("Panel Referansları")] 
    public GameObject draftPanel;
    public GameObject teamBuildingPanel;
    public GameObject matchResultPanel;

    private List<PlayerData> draftedTeam;

    void OnEnable() 
    {
        if (startMatchButton != null)
        {
            startMatchButton.onClick.RemoveAllListeners();
            startMatchButton.onClick.AddListener(StartMatch);
            Debug.Log("TeamBuilder: StartMatchButton listener eklendi.");
        }
        else
        {
            Debug.LogError("TeamBuilder: HATA: StartMatchButton referansı atanmamış! Lütfen Inspector'dan atayın.");
        }
    }

    void OnDisable() 
    {
        Debug.Log("TeamBuilder: OnDisable çağrıldı.");
        if (startMatchButton != null)
        {
            startMatchButton.onClick.RemoveListener(StartMatch);
        }
    }

    public void SetupTeamScreen()
    {
        Debug.Log("TeamBuilder: SetupTeamScreen çağrıldı. Takım kurma ekranı hazırlanıyor.");

        if (GameManager.Instance != null)
        {
            draftedTeam = GameManager.Instance.GetPlayerTeam();
            Debug.Log($"TeamBuilder: GameManager'dan {draftedTeam.Count} oyuncu alındı (SetupTeamScreen).");

            if (draftedTeam.Count != 11)
            {
                Debug.LogWarning($"TeamBuilder: Draft edilen oyuncu sayısı 11 değil! ({draftedTeam.Count}). Bu bir sorun olabilir.");
            }
        }
        else
        {
            Debug.LogError("TeamBuilder: HATA: GameManager bulunamadı! SetupTeamScreen'de. Test verileri kullanılıyor.");
            // TEST verilerini bu pozisyon kısaltmalarına göre güncelledim
            draftedTeam = new List<PlayerData>();
            draftedTeam.Add(new PlayerData("TEST Kaleci", "FB", 80, 0, 0, 0, 0, 0, null, 1, "GK"));
            draftedTeam.Add(new PlayerData("TEST CB 1", "GS", 75, 0, 0, 0, 0, 0, null, 2, "CB"));
            draftedTeam.Add(new PlayerData("TEST CB 2", "BJK", 78, 0, 0, 0, 0, 0, null, 3, "CB"));
            draftedTeam.Add(new PlayerData("TEST LB", "FB", 76, 0, 0, 0, 0, 0, null, 4, "LB"));
            draftedTeam.Add(new PlayerData("TEST RB", "GS", 79, 0, 0, 0, 0, 0, null, 5, "RB"));
            draftedTeam.Add(new PlayerData("TEST CM 1", "BJK", 82, 0, 0, 0, 0, 0, null, 6, "CM"));
            draftedTeam.Add(new PlayerData("TEST CM 2", "FB", 84, 0, 0, 0, 0, 0, null, 7, "CM"));
            draftedTeam.Add(new PlayerData("TEST CAM", "GS", 81, 0, 0, 0, 0, 0, null, 8, "CAM"));
            draftedTeam.Add(new PlayerData("TEST LW", "BJK", 88, 0, 0, 0, 0, 0, null, 9, "LW"));
            draftedTeam.Add(new PlayerData("TEST RW", "FB", 88, 0, 0, 0, 0, 0, null, 10, "RW"));
            draftedTeam.Add(new PlayerData("TEST ST", "GS", 86, 0, 0, 0, 0, 0, null, 11, "ST"));
        }

        DisplayTeamOnField();

        if (draftPanel != null) draftPanel.SetActive(false); else Debug.LogError("TeamBuilder: HATA: draftPanel atanmamış!");
        if (teamBuildingPanel != null) teamBuildingPanel.SetActive(true); else Debug.LogError("TeamBuilder: HATA: teamBuildingPanel atanmamış!");
        if (matchResultPanel != null) matchResultPanel.SetActive(false); else Debug.LogError("TeamBuilder: HATA: matchResultPanel atanmamış!");
    }

    void DisplayTeamOnField()
    {
        Debug.Log("TeamBuilder: DisplayTeamOnField çağrıldı (iç kısım).");

        // Tüm slotlardaki metinleri baştan temizle
        if (gkSlotText != null) { gkSlotText.text = ""; Debug.Log("TeamBuilder: GK slot metni temizlendi."); } else Debug.LogError("TeamBuilder: HATA: gkSlotText NULL!");
        
        // CB slotlarını temizle
        if (cbSlotTexts != null) {
            Debug.Log($"TeamBuilder: cbSlotTexts listesinde {cbSlotTexts.Count} eleman var.");
            for(int i = 0; i < cbSlotTexts.Count; i++) {
                if (cbSlotTexts[i] != null) { cbSlotTexts[i].text = ""; Debug.Log($"TeamBuilder: CB slot {i} metni temizlendi."); }
                else Debug.LogError($"TeamBuilder: HATA: cbSlotTexts[{i}] NULL!");
            }
        } else Debug.LogError("TeamBuilder: HATA: cbSlotTexts listesi NULL!");

        if (lbSlotText != null) { lbSlotText.text = ""; Debug.Log("TeamBuilder: LB slot metni temizlendi."); } else Debug.LogError("TeamBuilder: HATA: lbSlotText NULL!");
        if (rbSlotText != null) { rbSlotText.text = ""; Debug.Log("TeamBuilder: RB slot metni temizlendi."); } else Debug.LogError("TeamBuilder: HATA: rbSlotText NULL!");

        // CM slotlarını temizle
        if (cmSlotTexts != null) {
            Debug.Log($"TeamBuilder: cmSlotTexts listesinde {cmSlotTexts.Count} eleman var.");
            for(int i = 0; i < cmSlotTexts.Count; i++) {
                if (cmSlotTexts[i] != null) { cmSlotTexts[i].text = ""; Debug.Log($"TeamBuilder: CM slot {i} metni temizlendi."); }
                else Debug.LogError($"TeamBuilder: HATA: cmSlotTexts[{i}] NULL!");
            }
        } else Debug.LogError("TeamBuilder: HATA: cmSlotTexts listesi NULL!");
        if (camSlotText != null) { camSlotText.text = ""; Debug.Log("TeamBuilder: CAM slot metni temizlendi."); } else Debug.LogError("TeamBuilder: HATA: camSlotText NULL!");

        // FWD slotlarını temizle
        if (lwSlotText != null) { lwSlotText.text = ""; Debug.Log("TeamBuilder: LW slot metni temizlendi."); } else Debug.LogError("TeamBuilder: HATA: lwSlotText NULL!");
        if (rwSlotText != null) { rwSlotText.text = ""; Debug.Log("TeamBuilder: RW slot metni temizlendi."); } else Debug.LogError("TeamBuilder: HATA: rwSlotText NULL!");
        if (stSlotText != null) { stSlotText.text = ""; Debug.Log("TeamBuilder: ST slot metni temizlendi."); } else Debug.LogError("TeamBuilder: HATA: stSlotText NULL!");
        
        Debug.Log("TeamBuilder: Tüm UI slotları temizleme denemesi tamamlandı.");

        if (draftedTeam == null || draftedTeam.Count == 0)
        {
            Debug.LogWarning("TeamBuilder: DisplayTeamOnField: Draft edilmiş takım bulunamadı veya takım boş. Oyuncu adları gösterilemiyor.");
            return;
        }

        int cbCount = 0;
        int cmCount = 0;

        foreach (PlayerData player in draftedTeam)
        {
            Debug.Log($"TeamBuilder: Oyuncu yerleştirme döngüsünde: {player.playerName}, Pozisyon: {player.position}");
            switch (player.position)
            {
                case "GK":
                    if (gkSlotText != null) { gkSlotText.text = player.playerName; Debug.Log($"TeamBuilder: Kaleci slotuna '{player.playerName}' yazıldı."); }
                    else Debug.LogError("TeamBuilder: HATA: gkSlotText NULL GK KISMINDA!");
                    break;
                case "CB":
                    if (cbCount < cbSlotTexts.Count) {
                        if (cbSlotTexts[cbCount] != null) { cbSlotTexts[cbCount].text = player.playerName; Debug.Log($"TeamBuilder: CB slot {cbCount} -> '{player.playerName}' yazıldı."); }
                        else Debug.LogError($"TeamBuilder: HATA: cbSlotTexts[{cbCount}] NULL CB KISMINDA!");
                        cbCount++;
                    } else Debug.LogWarning($"TeamBuilder: CB slotları yetersiz! {player.playerName} yerleştirilemedi. Mevcut slot sayısı: {cbSlotTexts.Count}");
                    break;
                case "LB":
                    if (lbSlotText != null) { lbSlotText.text = player.playerName; Debug.Log($"TeamBuilder: LB slotuna '{player.playerName}' yazıldı."); }
                    else Debug.LogError("TeamBuilder: HATA: lbSlotText NULL LB KISMINDA!");
                    break;
                case "RB":
                    if (rbSlotText != null) { rbSlotText.text = player.playerName; Debug.Log($"TeamBuilder: RB slotuna '{player.playerName}' yazıldı."); }
                    else Debug.LogError("TeamBuilder: HATA: rbSlotText NULL RB KISMINDA!");
                    break;
                case "CM":
                    if (cmCount < cmSlotTexts.Count) {
                        if (cmSlotTexts[cmCount] != null) { cmSlotTexts[cmCount].text = player.playerName; Debug.Log($"TeamBuilder: CM slot {cmCount} -> '{player.playerName}' yazıldı."); }
                        else Debug.LogError($"TeamBuilder: HATA: cmSlotTexts[{cmCount}] NULL CM KISMINDA!");
                        cmCount++;
                    } else Debug.LogWarning($"TeamBuilder: CM slotları yetersiz! {player.playerName} yerleştirilemedi. Mevcut slot sayısı: {cmSlotTexts.Count}");
                    break;
                case "CAM":
                    if (camSlotText != null) { camSlotText.text = player.playerName; Debug.Log($"TeamBuilder: CAM slotuna '{player.playerName}' yazıldı."); }
                    else Debug.LogError("TeamBuilder: HATA: camSlotText NULL CAM KISMINDA!");
                    break;
                case "LW":
                    if (lwSlotText != null) { lwSlotText.text = player.playerName; Debug.Log($"TeamBuilder: LW slotuna '{player.playerName}' yazıldı."); }
                    else Debug.LogError("TeamBuilder: HATA: lwSlotText NULL LW KISMINDA!");
                    break;
                case "RW":
                    if (rwSlotText != null) { rwSlotText.text = player.playerName; Debug.Log($"TeamBuilder: RW slotuna '{player.playerName}' yazıldı."); }
                    else Debug.LogError("TeamBuilder: HATA: rwSlotText NULL RW KISMINDA!");
                    break;
                case "ST":
                    if (stSlotText != null) { stSlotText.text = player.playerName; Debug.Log($"TeamBuilder: ST slotuna '{player.playerName}' yazıldı."); }
                    else Debug.LogError("TeamBuilder: HATA: stSlotText NULL ST KISMINDA!");
                    break;
                default:
                    Debug.LogWarning($"TeamBuilder: Oyuncu {player.playerName} için bilinmeyen pozisyon: {player.position}. Yerleştirilemedi.");
                    break;
            }
        }
        Debug.Log("TeamBuilder: Tüm oyuncular yerleştirme denemesi tamamlandı.");
    }

    void StartMatch()
    {
        Debug.Log("TeamBuilder: Maç başlatılıyor butona basıldı.");
        MatchSimulator matchSimulator = FindAnyObjectByType<MatchSimulator>();
        if (matchSimulator != null)
        {
            matchSimulator.SetupMatchScreen();
        }
        else
        {
            Debug.LogError("TeamBuilder: HATA: MatchSimulator bulunamadı! Maç başlatılamıyor.");
        }

        if (teamBuildingPanel != null) teamBuildingPanel.SetActive(false);
        if (matchResultPanel != null) matchResultPanel.SetActive(true);
    }
}