using Cameras;
using System;
using UnityEditor.Rendering;
using UnityEngine;

public class CursorTracker : MonoBehaviour
{
    // UdpReceiver 스크립트 참조 (Inspector에서 연결)
    public UdpReceiver udpReceiver;

    // MediaPipe 랜드마크 인덱스 (검지 끝을 사용할 경우: 8)
    // 랜드마크 좌표는 0부터 시작: Index 8은 8번째 랜드마크
    public int landmarkIndex = 5;

    // 화면 해상도에 맞게 좌표를 조정하기 위한 배율
    public float sensitivity = 10.0f;

    // 화면 영역 제한을 위한 변수 (선택 사항)
    private float screenWidth;
    private float screenHeight;
    public bool isrock = false;
    private SpriteRenderer Cusor;
    public Vector2 vectorPos;
    public bool dedected_bool = false;
    private DateTime dedected_time = DateTime.Now;

    void OnEnable()
    {
        // 현재 화면의 너비와 높이를 가져옴
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        Cusor = GetComponentInChildren<SpriteRenderer>();
        if (Cusor == null)
        {
            Debug.LogError("Cusor가 연결되지 않았습니다.");
        }
        if (udpReceiver == null)
        {
            Debug.LogError("UdpReceiver가 연결되지 않았습니다.");
        }

    }

    void Update()
    {
        if (udpReceiver == null) return;
        string data = udpReceiver.lastReceivedData;

        if (!string.IsNullOrEmpty(data))
        {
            string[] rawFloats = data.Split(',');

            // 데이터 길이가 (21 * 3 = 63)인지 확인
            if (rawFloats.Length == 64)
            {
                // 원하는 랜드마크의 X, Y 좌표 인덱스 계산
                // landmarkIndex의 X 위치 = landmarkIndex * 3
                // landmarkIndex의 Y 위치 = landmarkIndex * 3 + 1
                int xIndex = landmarkIndex * 3;
                int yIndex = landmarkIndex * 3 + 1;

                if (float.TryParse(rawFloats[xIndex], out float rawX) &&
                    float.TryParse(rawFloats[yIndex], out float rawY))
                {
                    // 1. Raw Data (0~1 범위)를 픽셀 좌표로 변환
                    // MediaPipe Y축은 위에서 아래로 증가할 수 있으므로 Y좌표를 반전시켜야 할 수 있습니다.
                    float targetX = rawX * screenWidth;
                    float targetY = (1f - rawY) * screenHeight; // Y축 반전 (유니티 화면 좌표계에 맞춤
                    int.TryParse(rawFloats[rawFloats.Length-1], out int rawGesture);
                    TimeSpan delta = DateTime.Now - dedected_time;
                    if (dedected_bool) if (delta.Milliseconds > 500) dedected_bool = false;
                    if (isrock)
                    {
                        if (rawGesture == 2)
                        {
                            isrock = false;
                            Cusor.color = Color.green;
                            dedected_time = DateTime.Now;
                            dedected_bool = true;
                        }
                    }
                    else
                    {
                        if (rawGesture == 1)
                        {
                            isrock = true;
                            Cusor.color = Color.red;
                            dedected_time = DateTime.Now;
                            dedected_bool = true;
                        }
                    }
                    // 2. 3D 월드 좌표로 변환 (World Space Cursor의 경우)
                    // 커서가 캔버스 UI가 아닌 3D 월드에서 움직여야 한다면:
                    vectorPos = new Vector3(targetX, targetY);
                    Vector3 worldPos = CameraDict.Curcameras.ScreenToWorldPoint(new Vector3(targetX, targetY, sensitivity));
                    // Z축을 카메라 앞에 고정 (sensitivity 값은 깊이 값)
                    worldPos.z = transform.position.z;

                    // 커서 오브젝트 이동
                    transform.position = worldPos;

                    // Debug.Log($"Cursor X: {targetX}, Y: {targetY}");
                }
            }
        }
    }
}