# -*- coding: utf-8 -*-
import cv2
import mediapipe as mp
import socket
import numpy as np
import os

# --- UDP 소켓 설정 (Unity UdpReceiver.cs의 포트와 일치해야 합니다) ---
UDP_IP = "127.0.0.1"  # 루프백 주소 (같은 컴퓨터에서 통신)
UDP_PORT = 5005
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)  # UDP 소켓 생성

# --- MediaPipe Hands 초기화 ---
BaseOptions = mp.tasks.BaseOptions
GestureRecognizer = mp.tasks.vision.GestureRecognizer
GestureRecognizerOptions = mp.tasks.vision.GestureRecognizerOptions
VisionRunningMode = mp.tasks.vision.RunningMode
mp_hands = mp.solutions.hands

gesture = {
    "None": 0,
    "Closed_Fist": 1,
    "Open_Palm": 2,
    "Pointing_Up": 3,
    "Thumb_Down": 4,
    "Thumb_Up": 5,
    "Victory": 6,
    "ILoveYou": 7,
}

model_path = os.path.join(os.path.dirname(__file__), "gesture_recognizer.task")
base_options = BaseOptions(model_asset_path=model_path)
options = GestureRecognizerOptions(
    base_options=base_options,
    running_mode=VisionRunningMode.IMAGE,
    num_hands=1,
)
# --- 웹캠 켜기 ---
cap = cv2.VideoCapture(0)  # 0번 카메라 (기본 웹캠)

print(f"UDP 서버 시작: {UDP_IP}:{UDP_PORT}")

while cap.isOpened():
    success, image = cap.read()
    if not success:
        continue

    # BGR 이미지를 RGB로 변환하고 좌우 반전
    image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)
    image.flags.writeable = False

    # 손 랜드마크 처리
    with GestureRecognizer.create_from_options(options) as recognizer:
        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=image)
        results = recognizer.recognize(mp_image)
        if results.hand_landmarks:
            for i, hand_landmarks in enumerate(results.hand_landmarks):
                data = []
                # 21개 랜드마크의 X, Y, Z 좌표를 리스트에 추가
                for landmark in hand_landmarks:
                    # X, Y, Z 순서로 데이터를 추가합니다.
                    data.extend([landmark.x, landmark.y, landmark.z])
                if results.gestures and results.gestures[i]:
                    gesture_label = gesture[results.gestures[i][0].category_name]
                    data.extend([gesture_label])

                # 데이터를 쉼표로 구분된 문자열로 변환하여 전송
                data_string = ",".join(map(str, data))
                sock.sendto(data_string.encode("utf-8"), (UDP_IP, UDP_PORT))
    if cv2.waitKey(5) & 0xFF == 27:  # ESC 키를 누르면 종료
        break

cap.release()
cv2.destroyAllWindows()
sock.close()
