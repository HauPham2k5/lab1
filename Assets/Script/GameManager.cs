using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cubePrefab;

    public float xMin = -10f, xMax = 10f;
    public float yMin = 0f, yMax = 5f;
    public float zMin = -10f, zMax = 10f;

    // Start được gọi khi khởi tạo
    void Start()
    {
        StartCoroutine(TaoCube());
    }

    IEnumerator TaoCube()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);

            float randomX = Random.Range(xMin, xMax);
            float randomY = Random.Range(yMin, yMax);
            float randomZ = Random.Range(zMin, zMax);

            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

            GameObject CubeClone = Instantiate(cubePrefab, randomPosition, Quaternion.identity);
            StartCoroutine(MoveCube(CubeClone));
            StartCoroutine(Facube(CubeClone)); // Gọi để xử lý sự kiện khi nhấn phím Space
        }
    }

    IEnumerator MoveCube(GameObject CubeClone)
    {
        float elapsedtime = 0f;
        float moveDuration = 3f;  // Đảm bảo cube mất 3 giây để di chuyển đến gốc
        Vector3 startPosition = CubeClone.transform.position;

        while (elapsedtime < moveDuration)
        {
            CubeClone.transform.position = Vector3.Lerp(startPosition, Vector3.zero, elapsedtime / moveDuration);
            elapsedtime += Time.deltaTime;
            yield return null;
        }

        CubeClone.transform.position = Vector3.zero;  // Đảm bảo cube kết thúc đúng tại gốc
    }

    IEnumerator Facube(GameObject CubeClone)
    {
        // Chờ đợi cho đến khi nhấn phím Space
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        // Lấy Renderer của cube để thay đổi độ mờ
        Renderer cubeRenderer = CubeClone.GetComponent<Renderer>();
        Color originalColor = cubeRenderer.material.color; // Lưu lại màu ban đầu của cube

        // Đảm bảo vật liệu sử dụng chế độ Transparent
        cubeRenderer.material.SetFloat("_Mode", 3); // Chế độ Transparent
        cubeRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        cubeRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        cubeRenderer.material.SetInt("_ZWrite", 0);
        cubeRenderer.material.DisableKeyword("_ALPHATEST_ON");
        cubeRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        cubeRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        cubeRenderer.material.renderQueue = 3000;

        float fadeDuration = 2f; // Thời gian làm mờ dần
        float elapsedTime = 0f;

        // Làm mờ dần cube trong khoảng thời gian fadeDuration
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration); // Lerp từ alpha = 1 đến 0
            cubeRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo rằng alpha là 0 khi kết thúc
        cubeRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
