using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class OyunYonetim : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform player;
    public Transform coin;
    public TextMeshProUGUI skorText;
    Rigidbody rb;  
    [Header("Ayarlar")]
    public float playerHiz = 5f;
    public float coinDonmeHizi = 300f;
    int skor = 0;
    float zamanSayaci = 5f;
    [Header("Game Over Sistemi")]
    public GameObject gameOverPanel;
    public Button yenidenBaslaButton;
    public Button oyunuBitirButton;
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        skorText.text = "Skor: 0";
        gameOverPanel.SetActive(false);
        yenidenBaslaButton.onClick.AddListener(YenidenBasla);
        oyunuBitirButton.onClick.AddListener(OyunuBitir);
    }
    void Update()
    {
        Hareket();
        CoiniDondur();
        CoinOtomatikYerDegistir();
        if (player.position.y < -1f)
        {
            OyunBitti();
        }
    }
    void Hareket()
    {
        float yatay = Input.GetAxis("Horizontal");
        float dikey = Input.GetAxis("Vertical");
        Vector3 hareket = new Vector3(yatay, 0, dikey);
        player.Translate(hareket * playerHiz * Time.deltaTime, Space.World);
    }
    void CoiniDondur()
    {
        coin.Rotate(Vector3.up * coinDonmeHizi * Time.deltaTime, Space.World);
    }
    void CoinOtomatikYerDegistir()
    {
        zamanSayaci -= Time.deltaTime;
        if (zamanSayaci <= 0)
        {
            CoiniRastgeleYerlestir();
            zamanSayaci = 5f;
        }
    }
    void CoiniRastgeleYerlestir()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        coin.position = new Vector3(x, coin.position.y, z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            skor++;
            skorText.text = "Skor: " + skor;
            CoiniRastgeleYerlestir();
            zamanSayaci = 5f;
        }
    }
    void OyunBitti()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }
    void YenidenBasla()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        player.position = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        skor = 0;
        skorText.text = "Skor: 0";
        zamanSayaci = 5f;
    }
    void OyunuBitir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();   // BUILD ALINCA ÇALIŞIR
#endif
    }
}