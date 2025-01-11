using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cubePrefab;

    public static int totalCubesCreated = 0; // Số Cube đã tạo
    public static int currentCubesActive = 0; // Số Cube còn lại trong cảnh
    // Hàm Start được gọi khi bắt đầu game
    void Start()
    {
        StartCoroutine(TaoCube());
    }

    IEnumerator TaoCube()
    {
        for (int i = 0; i < 5; i++)
        {
            // Đợi 3 giây trước khi tạo cube
            yield return new WaitForSeconds(3);
            float x = Random.Range(-5, 5);
            Vector3 pos = new Vector3(x, x, x);
            GameObject cubeClone = Instantiate(cubePrefab, pos, Quaternion.identity);
            StartCoroutine(MoveCube(cubeClone));
        }
    }

    IEnumerator MoveCube(GameObject cubeClone)
    {
        float elapsedTime = 0f;
        float duration = 3f;

        // Lấy Renderer và Material của cube
        Renderer renderer = cubeClone.GetComponent<Renderer>();
        Material material = renderer.material;

        // Cấu hình vật liệu để hỗ trợ chế độ trong suốt
        material.SetFloat("_Mode", 3); // Chế độ Transparent
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        // Lấy màu ban đầu và giá trị alpha
        Color color = material.color;
        float startAlpha = color.a;

        while (elapsedTime < duration)
        {
            // Di chuyển cube về vị trí (0,0,0)
            cubeClone.transform.position = Vector3.Lerp(cubeClone.transform.position, Vector3.zero, elapsedTime / duration);

            // Làm mờ dần bằng cách giảm giá trị alpha
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
            material.color = new Color(color.r, color.g, color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo cube hoàn toàn mờ sau khi kết thúc
        material.color = new Color(color.r, color.g, color.b, 0f);

        // Xóa cube khi đã mờ hoàn toàn
        Destroy(cubeClone);
    }

    // Update được gọi mỗi frame (không sử dụng trong trường hợp này)
    void Update()
    {

    }
}
