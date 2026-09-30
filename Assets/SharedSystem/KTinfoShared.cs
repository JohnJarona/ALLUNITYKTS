using UnityEngine;
using UnityEngine.SceneManagement;

public class KTinfoShared : MonoBehaviour
{
    public void ReturnBack()
    {
        SceneManager.LoadScene($"SharedSystem/Manager");
    }
}
