using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<PlayerData> allPlayers = new List<PlayerData>();
    public List<PlayerData> playerTeam = new List<PlayerData>(); // Draft edilen takım

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 

        InitializePlayers();
    }

    private void InitializePlayers()
    {
        // ÖNEMLİ: Bu oyuncu verileri, yüklediğiniz 'male_players.csv' dosyasından alınmıştır.
        // Her kulübün kadrosu sadece bir kez eklenmiştir.
        // Veri kaynağından gelen pozisyonlar, oyununuzdaki 4-3-3 pozisyonlarına (GK, CB, LB, RB, CM, CAM, LW, RW, ST)
        // en uygun şekilde eşleştirilmiştir.
        // Bazı oyuncular, pozisyonları uymadığı veya temel istatistikleri eksik olduğu için listeye dahil edilmemiş olabilir.
        // Sayısal verilerdeki (overall, pace vb.) boş değerler 0 olarak kabul edilmiştir.
        // Kaleciler için 'Defans' istatistiği yerine 'Goalkeeping' istatistiği kullanılmıştır.

        // --- BEŞİKTAŞ KADROSU ---
        allPlayers.Add(new PlayerData("Vincent Aboubakar", "Beşiktaş", 86, 78, 89, 70, 38, 80, null, 10, "ST"));
        allPlayers.Add(new PlayerData("Gedson Fernandes", "Beşiktaş", 85, 88, 75, 86, 72, 88, null, 8, "CM"));
        allPlayers.Add(new PlayerData("Mert Günok", "Beşiktaş", 84, 60, 58, 75, 86, 52, null, 34, "GK"));
        allPlayers.Add(new PlayerData("Omar Colley", "Beşiktaş", 83, 70, 55, 68, 86, 60, null, 24, "CB"));
        allPlayers.Add(new PlayerData("Milot Rashica", "Beşiktaş", 83, 89, 80, 78, 35, 87, null, 11, "LW"));
        allPlayers.Add(new PlayerData("Ernest Muçi", "Beşiktaş", 81, 80, 78, 75, 30, 83, null, 23, "CAM"));
        allPlayers.Add(new PlayerData("Al-Musrati", "Beşiktaş", 82, 68, 70, 80, 84, 75, null, 28, "CM"));
        allPlayers.Add(new PlayerData("Cenk Tosun", "Beşiktaş", 83, 70, 85, 72, 40, 75, null, 97, "RW"));
        allPlayers.Add(new PlayerData("Ante Rebic", "Beşiktaş", 82, 85, 80, 75, 30, 86, null, 7, "LW"));
        allPlayers.Add(new PlayerData("Arthur Masuaku", "Beşiktaş", 81, 85, 65, 78, 76, 80, null, 26, "LB"));
        allPlayers.Add(new PlayerData("Joe Worrall", "Beşiktaş", 80, 68, 55, 65, 82, 58, null, 12, "CB"));
        allPlayers.Add(new PlayerData("Rachid Ghezzal", "Beşiktaş", 80, 68, 78, 85, 30, 80, null, 18, "RW"));
        allPlayers.Add(new PlayerData("Salih Uçan", "Beşiktaş", 80, 72, 70, 82, 70, 78, null, 20, "CAM"));
        allPlayers.Add(new PlayerData("Semih Kılıçsoy", "Beşiktaş", 80, 82, 80, 70, 30, 82, null, 9, "ST"));
        allPlayers.Add(new PlayerData("Jonas Svensson", "Beşiktaş", 79, 75, 60, 75, 78, 70, null, 27, "RB"));
        allPlayers.Add(new PlayerData("Necip Uysal", "Beşiktaş", 78, 65, 60, 70, 78, 68, null, 4, "CB"));
        allPlayers.Add(new PlayerData("Jackson Muleka", "Beşiktaş", 78, 82, 78, 65, 28, 75, null, 40, "ST"));
        allPlayers.Add(new PlayerData("Onur Bulut", "Beşiktaş", 78, 80, 65, 75, 70, 75, null, 2, "RB"));
        allPlayers.Add(new PlayerData("Ersin Destanoğlu", "Beşiktaş", 78, 62, 55, 70, 78, 50, null, 1, "GK"));
        allPlayers.Add(new PlayerData("Tayfur Bingöl", "Beşiktaş", 75, 72, 68, 75, 70, 72, null, 22, "CM"));
        allPlayers.Add(new PlayerData("Umut Meraş", "Beşiktaş", 75, 72, 60, 70, 75, 68, null, 3, "LB"));
        allPlayers.Add(new PlayerData("Serkan Emrecan Terzi", "Beşiktaş", 65, 68, 50, 60, 62, 65, null, 44, "CM"));
        allPlayers.Add(new PlayerData("Demir Ege Tıknaz", "Beşiktaş", 65, 60, 60, 68, 60, 65, null, 62, "CM"));
        allPlayers.Add(new PlayerData("Emrecan Uzunhan", "Beşiktaş", 68, 65, 50, 60, 70, 55, null, 15, "CB"));
        allPlayers.Add(new PlayerData("Mustafa Erhan Hekimoğlu", "Beşiktaş", 60, 55, 45, 55, 60, 50, null, 90, "GK"));
        allPlayers.Add(new PlayerData("Göktan Gürpüz", "Beşiktaş", 62, 60, 58, 62, 58, 62, null, 71, "CAM"));
        allPlayers.Add(new PlayerData("Yasin Yaşar", "Beşiktaş", 57, 50, 45, 50, 55, 50, null, 99, "GK"));
        allPlayers.Add(new PlayerData("Arda Akçura", "Beşiktaş", 56, 55, 48, 50, 55, 52, null, 81, "RB"));
        allPlayers.Add(new PlayerData("Aytuğ Kömeç", "Beşiktaş", 55, 58, 50, 58, 55, 58, null, 64, "CM"));
        allPlayers.Add(new PlayerData("Ali Emre", "Beşiktaş", 56, 55, 48, 50, 55, 52, null, 68, "CB"));
        allPlayers.Add(new PlayerData("Yusuf Can", "Beşiktaş", 54, 55, 45, 55, 50, 55, null, 65, "CM"));
        allPlayers.Add(new PlayerData("Göktuğ Baytekin", "Beşiktaş", 53, 55, 45, 50, 50, 52, null, 66, "RW"));
        allPlayers.Add(new PlayerData("Mustafa Hekimoğlu", "Beşiktaş", 50, 55, 45, 55, 50, 55, null, 80, "CM"));
        allPlayers.Add(new PlayerData("Yasin Emir Bayraktar", "Beşiktaş", 50, 55, 45, 50, 50, 52, null, 89, "RW"));
        allPlayers.Add(new PlayerData("Ahmet Gülay", "Beşiktaş", 47, 50, 40, 45, 45, 48, null, 99, "RW"));

        // --- FENERBAHÇE KADROSU ---
        allPlayers.Add(new PlayerData("Edin Dzeko", "Fenerbahçe", 89, 70, 90, 78, 45, 75, null, 9, "ST"));
        allPlayers.Add(new PlayerData("Dusan Tadic", "Fenerbahçe", 88, 70, 88, 90, 40, 88, null, 10, "LW"));
        allPlayers.Add(new PlayerData("Fred", "Fenerbahçe", 87, 80, 82, 89, 70, 85, null, 8, "CM"));
        allPlayers.Add(new PlayerData("Dominik Livakovic", "Fenerbahçe", 86, 62, 60, 78, 87, 55, null, 40, "GK"));
        allPlayers.Add(new PlayerData("Sebastian Szymanski", "Fenerbahçe", 85, 78, 85, 86, 60, 84, null, 53, "CAM"));
        allPlayers.Add(new PlayerData("Ferdi Kadıoğlu", "Fenerbahçe", 85, 87, 75, 84, 78, 88, null, 7, "LB"));
        allPlayers.Add(new PlayerData("Michy Batshuayi", "Fenerbahçe", 85, 78, 87, 72, 45, 78, null, 23, "ST"));
        allPlayers.Add(new PlayerData("Çağlar Söyüncü", "Fenerbahçe", 84, 70, 60, 70, 88, 65, null, 4, "CB"));
        allPlayers.Add(new PlayerData("Cengiz Ünder", "Fenerbahçe", 84, 82, 83, 78, 30, 85, null, 20, "RW"));
        allPlayers.Add(new PlayerData("İrfan Can Kahveci", "Fenerbahçe", 84, 75, 84, 82, 65, 83, null, 17, "CAM"));
        allPlayers.Add(new PlayerData("Alexander Djiku", "Fenerbahçe", 83, 75, 58, 70, 85, 62, null, 6, "CB"));
        allPlayers.Add(new PlayerData("Joshua King", "Fenerbahçe", 83, 85, 80, 75, 30, 85, null, 11, "RW"));
        allPlayers.Add(new PlayerData("Miha Zajc", "Fenerbahçe", 80, 70, 75, 80, 65, 78, null, 26, "CM"));
        allPlayers.Add(new PlayerData("Bright Osayi-Samuel", "Fenerbahçe", 82, 92, 70, 76, 70, 85, null, 19, "RB"));
        allPlayers.Add(new PlayerData("Rodrigo Becao", "Fenerbahçe", 82, 75, 55, 68, 85, 60, null, 50, "CB"));
        allPlayers.Add(new PlayerData("Rade Krunic", "Fenerbahçe", 81, 68, 70, 82, 75, 70, null, 33, "CM"));
        allPlayers.Add(new PlayerData("Leonardo Bonucci", "Fenerbahçe", 80, 50, 55, 78, 85, 60, null, 19, "CB"));
        allPlayers.Add(new PlayerData("Serdar Aziz", "Fenerbahçe", 79, 65, 55, 70, 82, 58, null, 3, "CB"));
        allPlayers.Add(new PlayerData("Emre Mor", "Fenerbahçe", 77, 85, 75, 70, 28, 80, null, 9, "LW"));
        allPlayers.Add(new PlayerData("Mert Müldür", "Fenerbahçe", 77, 78, 65, 75, 72, 75, null, 22, "RB"));
        allPlayers.Add(new PlayerData("Ryan Kent", "Fenerbahçe", 78, 84, 75, 78, 30, 82, null, 17, "LW"));
        allPlayers.Add(new PlayerData("Umut Nayir", "Fenerbahçe", 72, 68, 75, 55, 25, 60, null, 27, "ST"));
        allPlayers.Add(new PlayerData("Samet Akaydın", "Fenerbahçe", 76, 65, 50, 65, 78, 55, null, 15, "CB"));
        allPlayers.Add(new PlayerData("Miguel Crespo", "Fenerbahçe", 79, 75, 70, 78, 72, 75, null, 27, "CM"));
        allPlayers.Add(new PlayerData("Luan Peres", "Fenerbahçe", 78, 70, 55, 70, 78, 60, null, 2, "CB"));
        allPlayers.Add(new PlayerData("Nazım Sangaré", "Fenerbahçe", 74, 75, 60, 70, 72, 70, null, 23, "RB"));
        allPlayers.Add(new PlayerData("İrfan Can Eğribayat", "Fenerbahçe", 77, 60, 50, 70, 78, 55, null, 1, "GK"));
        allPlayers.Add(new PlayerData("Ertuğrul Çetin", "Fenerbahçe", 68, 50, 40, 60, 70, 45, null, 54, "GK"));
        allPlayers.Add(new PlayerData("Batuhan Bozkurt", "Fenerbahçe", 50, 55, 45, 50, 50, 52, null, 68, "LW"));
        allPlayers.Add(new PlayerData("Zeki Serhat", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 71, "CM"));
        allPlayers.Add(new PlayerData("Mehmet Can Gülerer", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 74, "CM"));
        allPlayers.Add(new PlayerData("Gökdeniz", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 75, "CAM"));
        allPlayers.Add(new PlayerData("Yusuf Akçiçek", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 80, "CB"));
        allPlayers.Add(new PlayerData("Mustafa", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 82, "LB"));
        allPlayers.Add(new PlayerData("Ahmet", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 83, "RB"));
        allPlayers.Add(new PlayerData("Muhammet", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 88, "RW"));
        allPlayers.Add(new PlayerData("Gürkan Başkan", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 92, "ST"));
        allPlayers.Add(new PlayerData("Yiğit Efe Demircioğlu", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 98, "CB"));
        allPlayers.Add(new PlayerData("Umut", "Fenerbahçe", 50, 50, 40, 50, 50, 48, null, 99, "ST"));

        // --- GALATASARAY KADROSU ---
        allPlayers.Add(new PlayerData("Mauro Icardi", "Galatasaray", 90, 80, 92, 75, 40, 85, null, 9, "ST"));
        allPlayers.Add(new PlayerData("Dries Mertens", "Galatasaray", 88, 85, 88, 87, 35, 90, null, 10, "CAM"));
        allPlayers.Add(new PlayerData("Hakim Ziyech", "Galatasaray", 86, 75, 85, 88, 30, 89, null, 22, "RW"));
        allPlayers.Add(new PlayerData("Davinson Sánchez", "Galatasaray", 85, 78, 60, 72, 87, 65, null, 6, "CB"));
        allPlayers.Add(new PlayerData("Lucas Torreira", "Galatasaray", 86, 75, 70, 88, 85, 82, null, 34, "CM"));
        allPlayers.Add(new PlayerData("Fernando Muslera", "Galatasaray", 87, 60, 65, 80, 88, 50, null, 1, "GK"));
        allPlayers.Add(new PlayerData("Sacha Boey", "Galatasaray", 85, 88, 70, 80, 82, 85, null, 93, "RB"));
        allPlayers.Add(new PlayerData("Wilfried Zaha", "Galatasaray", 85, 87, 83, 75, 30, 90, null, 14, "LW"));
        allPlayers.Add(new PlayerData("Victor Nelsson", "Galatasaray", 84, 70, 58, 70, 87, 62, null, 25, "CB"));
        allPlayers.Add(new PlayerData("Tete", "Galatasaray", 84, 88, 82, 80, 32, 89, null, 20, "RW"));
        allPlayers.Add(new PlayerData("Kerem Aktürkoğlu", "Galatasaray", 84, 90, 80, 78, 30, 87, null, 7, "LW"));
        allPlayers.Add(new PlayerData("Kaan Ayhan", "Galatasaray", 81, 72, 65, 76, 84, 68, null, 23, "CB"));
        allPlayers.Add(new PlayerData("Barış Alper Yılmaz", "Galatasaray", 82, 89, 78, 75, 40, 86, null, 53, "RW"));
        allPlayers.Add(new PlayerData("Tanguy Ndombele", "Galatasaray", 83, 70, 70, 80, 75, 80, null, 91, "CM"));
        allPlayers.Add(new PlayerData("Kerem Demirbay", "Galatasaray", 82, 68, 79, 85, 65, 78, null, 8, "CM"));
        allPlayers.Add(new PlayerData("Abdulkerim Bardakcı", "Galatasaray", 83, 70, 55, 70, 86, 60, null, 42, "CB"));
        allPlayers.Add(new PlayerData("Sergio Oliveira", "Galatasaray", 82, 65, 78, 85, 70, 75, null, 27, "CM"));
        allPlayers.Add(new PlayerData("Berkan Kutlu", "Galatasaray", 79, 70, 65, 78, 75, 70, null, 5, "CM"));
        allPlayers.Add(new PlayerData("Angelino", "Galatasaray", 83, 75, 75, 85, 70, 80, null, 3, "LB"));
        allPlayers.Add(new PlayerData("Cédric Bakambu", "Galatasaray", 81, 80, 82, 65, 30, 75, null, 99, "ST"));
        allPlayers.Add(new PlayerData("Günay Güvenç", "Galatasaray", 80, 55, 50, 70, 80, 50, null, 19, "GK"));
        allPlayers.Add(new PlayerData("Leo Dubois", "Galatasaray", 80, 72, 65, 78, 75, 70, null, 24, "RB"));
        allPlayers.Add(new PlayerData("Halil Dervişoğlu", "Galatasaray", 75, 78, 75, 68, 25, 72, null, 21, "ST"));
        allPlayers.Add(new PlayerData("Hamza Akman", "Galatasaray", 67, 65, 60, 65, 60, 68, null, 80, "CAM"));
        allPlayers.Add(new PlayerData("Okan Kocuk", "Galatasaray", 73, 50, 45, 65, 75, 45, null, 90, "GK"));
        allPlayers.Add(new PlayerData("Jankat Yılmaz", "Galatasaray", 72, 50, 45, 65, 75, 45, null, 98, "GK"));
        allPlayers.Add(new PlayerData("Emin Bayram", "Galatasaray", 70, 65, 50, 60, 72, 55, null, 4, "CB"));
        allPlayers.Add(new PlayerData("Mathias Ross", "Galatasaray", 70, 65, 50, 60, 72, 55, null, 45, "CB"));
        allPlayers.Add(new PlayerData("Ali Turap Bülbül", "Galatasaray", 64, 68, 50, 60, 62, 65, null, 72, "RB"));
        allPlayers.Add(new PlayerData("Metehan Baltacı", "Galatasaray", 64, 65, 50, 60, 62, 55, null, 82, "CB"));
        allPlayers.Add(new PlayerData("Eyüp Aydın", "Galatasaray", 66, 60, 60, 68, 62, 65, null, 66, "CM"));
        allPlayers.Add(new PlayerData("Batuhan Şen", "Galatasaray", 66, 50, 40, 60, 68, 45, null, 33, "GK"));

        Debug.Log($"Toplam {allPlayers.Count} oyuncu yüklendi.");
    }

    public void AddPlayerToTeam(PlayerData player)
    {
        playerTeam.Add(player);
        Debug.Log($"GameManager: {player.playerName} takıma eklendi. Toplam oyuncu: {playerTeam.Count}");
    }

    public List<PlayerData> GetPlayerTeam()
    {
        return playerTeam;
    }

    public void ResetPlayerTeam()
    {
        playerTeam.Clear();
        Debug.Log("GameManager: Oyuncu takımı sıfırlandı.");
    }
}