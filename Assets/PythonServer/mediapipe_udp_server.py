# -*- coding: utf-8 -*-
import cv2
import mediapipe as mp
import socket
import numpy as np

# --- UDP 소켓 설정 (Unity UdpReceiver.cs의 포트와 일치해야 합니다) ---
UDP_IP = "127.0.0.1"  # 루프백 주소 (같은 컴퓨터에서 통신)
UDP_PORT = 5005      
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM) # UDP 소켓 생성

# --- MediaPipe Hands 초기화 ---
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(
    static_image_mode=False,
    max_num_hands=1,
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5)

# --- 웹캠 켜기 ---
cap = cv2.VideoCapture(0) # 0번 카메라 (기본 웹캠)

print(f"UDP 서버 시작: {UDP_IP}:{UDP_PORT}")

while cap.isOpened():
    success, image = cap.read()
    if not success:
        continue

    # BGR 이미지를 RGB로 변환하고 좌우 반전
    image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)
    image.flags.writeable = False
    
    # 손 랜드마크 처리
    results = hands.process(image)
    
    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            data = []
            # 21개 랜드마크의 X, Y, Z 좌표를 리스트에 추가
            for landmark in hand_landmarks.landmark:
                # X, Y, Z 순서로 데이터를 추가합니다.
                data.extend([landmark.x, landmark.y, landmark.z])
            
            # 데이터를 쉼표로 구분된 문자열로 변환하여 전송
            data_string = ",".join(map(str, data))
            sock.sendto(data_string.encode('utf-8'), (UDP_IP, UDP_PORT))
            
            # (선택 사항) 화면에 랜드마크 그리기
            mp.solutions.drawing_utils.draw_landmarks(
                image, hand_landmarks, mp_hands.HAND_CONNECTIONS)

    # 화면 표시
    cv2.imshow('MediaPipe Hands', image)
    if cv2.waitKey(5) & 0xFF == 27: # ESC 키를 누르면 종료
        break

cap.release()
cv2.destroyAllWindows()
sock.close()