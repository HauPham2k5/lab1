using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện quản lý Scene

public class SceneTransition : MonoBehaviour
{
    public string Level2; // Tên Scene muốn chuyển đến

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu nhân vật chạm cửa
        {
            SceneManager.LoadScene(Level2); // Chuyển cảnh
        }
    }
}