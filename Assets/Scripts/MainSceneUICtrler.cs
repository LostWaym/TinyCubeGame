using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneUICtrler : MonoBehaviour
{
    public Text txtHealth, txtScore, txtKill, txtTime, txtEnerge;
    public Entity playerEntity;
    private PlayerLogic playerLogic;
    public Transform ResultUIRoot;
    public Button btnSubmit;

    // Start is called before the first frame update
    void Start()
    {
        playerLogic = playerEntity.GetComponent<PlayerLogic>();
        GameManager.Instance.gameStateChanged += OnGameStateChanged;

        btnSubmit.onClick.AddListener(() =>
        {
            ResultUIRoot.gameObject.SetActive(false);
            SceneManager.LoadScene("GameScene");
        });
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Playing)
        {
            ResultUIRoot.gameObject.SetActive(false);
        }
        else
        {
            ResultUIRoot.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEntity != null)
        {
            txtHealth.text = $"生命值：{playerEntity.hp:F1}";
            txtEnerge.text = $"能量：{playerLogic.energy:F1}";
        }
        else
        {
            txtHealth.text = $"生命值：0.0";
            txtEnerge.text = $"能量：0.0";
        }
        txtScore.text = $"分数：{GameManager.Instance.score}/{GameManager.Instance.winScore}";
        txtKill.text = $"击杀数：{GameManager.Instance.killAmount}/{GameManager.Instance.winKillAmount}";
        txtTime.text = TimeFormat(GameManager.Instance.remainTime);
    }

    private string TimeFormat(float time)
    {
        int iTime = (int)Mathf.Ceil(time);
        int minute = iTime / 60;
        int second = iTime % 60;
        return $"{minute:D2}:{second:D2}";
    }
}
