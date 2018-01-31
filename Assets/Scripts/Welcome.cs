  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Welcome : MonoBehaviour
{
    [SerializeField] private Button m_startButton;
    [SerializeField] private Image m_fadeImage;
    [SerializeField] private AudioSource audioSource;
    private bool flag;

    private void Awake()
    {
        m_fadeImage.color = new Color(0, 0, 0, 0);
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!audioSource.isPlaying)
                audioSource.Play();
            m_startButton.image.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_startButton.image.color = new Color(1, 1, 1, 1);
            EnterGame();
        }

        if (flag)
        {
            m_fadeImage.color = Color.Lerp(m_fadeImage.color, Color.black, 0.1f);
        }
    }

    void EnterGame()
    {
        flag = true;
        Invoke("LoadScene", 1f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("Tutor");
        //SceneManager.LoadScene("Game");
    }
}