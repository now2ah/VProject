using UnityEngine;
using UnityEngine.SceneManagement;
using VProject.Utils;

public class MainScene : MonoBehaviour
{
    private void OnEnable()
    {
        InputHandler.OnClickAction += InputHandler_OnClickAction;
    }

    private void OnDisable()
    {
        InputHandler.OnClickAction -= InputHandler_OnClickAction;
    }

    private void InputHandler_OnClickAction()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
