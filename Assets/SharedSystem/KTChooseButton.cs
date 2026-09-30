using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KTChooseButton : MonoBehaviour
{
    [SerializeField] public string sceneName;
    public void ChooseThis()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OnValidate()
    {
        gameObject.name = $"KTBtn_({sceneName})";
    }
}
