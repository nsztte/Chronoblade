
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
- [ ] InputManager 구조 설계
- [ ] PlayerController 이동 구현
- [ ] CameraController 회전 구현
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