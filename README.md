
# Chronoblade

1인칭 시점의 시간 조작 액션 어드벤처 게임

---

## 프로젝트 개요

- 개발 기간: 2025.06.16 ~ 2025.07.14
- 플랫폼: PC (Windows)
- 개발 엔진: Unity 6000.0.51 (URP)
- 주요 기능: 
  - 1인칭 시점 컨트롤
  - 시간 조작 및 리듬 전투 시스템
  - 퍼즐과 선택지 기반 분기

---

## 1주차 목표

- [x] Unity 프로젝트 생성
- [x] 기본 폴더 구조 구성
- [x] 테스트용 씬 생성 및 저장
- [x] InputManager 구조 설계
- [x] PlayerController 이동 구현
- [x] CameraController 회전 구현
- [ ] WeaponController 틀 생성
- [ ] 테스트 맵 구성 (큐브 기반)
- [ ] 크로스헤어 UI 구현

---

## 폴더 구조

```
Assets/
├── _Project/
│   ├── Art/
│   ├── Materials/
│   ├── Prefabs/
│   ├── Scenes/
│   └── Scripts/
│   │   ├── Player/
│   │   └── Systems/ 
```

---

## 참고/이슈 기록
- [해결됨] CharacterController와 Rigidbody 중 선택 테스트

---

## 2025.06.17 (월) 작업 기록

### 주요 작업
- Unity 프로젝트 생성 (URP 3D 템플릿, 이름: Chronoblade)
- Assets/_Project 폴더 구조 정리 완료
- Main.unity 씬 생성 및 저장
- Plane + Directional Light 배치로 테스트 씬 뼈대 구성
- Player용 Capsule 오브젝트 배치, 카메라 자식으로 연결
- README.md 작성 시작

### 메모
- MainCamera는 Player의 자식으로 배치하여 1인칭 시점 구현 준비
- Skybox 밝기 기본값은 추후 조정 예정

---

## 2025.06.18 (화) 작업 기록

### 주요 작업
- 이벤트 기반 InputManager 구조 구현 및 모든 입력 처리 완료
- PlayerController에서 WASD 이동, 점프, 중력 구현 (fallMultiplier 적용)
- 마우스 회전 기반 CameraController 구현 (X: 본체 회전, Y: 상하 회전 제한)
- Cursor 상태 고정 처리 (`Lock`, `Invisible')
- 입력 구조 설계 문서 작성 및 프로젝트 내 Docs 폴더에 포함

### 메모
- 이동 중에도 중력 적용을 누락하지 않도록 Vector3.y 포함 필수
- `OnMoveInput(Vector2.zero)`를 전달하지 않으면 이동이 멈추지 않음
- 캐릭터 이동시 1인칭 액션 어드벤처 (정확한 컨트롤, 중력/점프 포함)에는 물리 기반인 Rigidbody보다 CharacterController 이용이 적절

---

## 관련 문서

- [Input_Structure_Design.md](./Docs/Input_Structure_Design.md) - 입력 구조 설계 문서