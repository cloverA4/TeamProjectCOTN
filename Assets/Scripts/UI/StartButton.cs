using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    Button stratButton;
    private void Start()
    {
        stratButton = GetComponent<Button>();
        stratButton.onClick.AddListener(ClickStart);
    }
    public void ClickStart()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
