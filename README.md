
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
- [x] WeaponManager, WeaponController 구현
- [x] PlayerManager 구현
- [x] 테스트 맵 구성 (큐브 기반)
- [x] 플레이어 에셋 연결
- [x] 무기 에셋 연결
- [x] 무기 조준시 줌인

---

## 폴더 구조

```
Assets/
├── _Project/
│   ├── Art/
│   │   ├── Model/
│   ├── Data/
│   │   ├── Weapon/
│   ├── Materials/
│   ├── Prefabs/
│   ├── Scenes/
│   └── Scripts/
│   │   ├── Enemy/
│   │   ├── Player/
│   │   │   ├── Weapon/
│   │   ├── Systems/
│   │   └── UI/ 
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

## 2025.06.19 (수) 작업 기록

### 주요 작업
- WeaponController 추상 클래스 설계 및 무기 시스템 기반 구조 완성
- MeleeWeaponController 구현 (CapsuleCast 기반 궤적 판정 + 중복 타격 방지)
- GunWeaponController 구현 (Raycast 기반 공격 + 탄약 소비/재장전 처리)
- WeaponManager 구현 및 무기 장착/해제, 마우스 휠 스위칭 기능 구현
- InputManager에서 숫자 키 및 마우스 휠 입력 처리 추가
- WeaponData(ScriptableObject) 설계 및 근접/총기 무기 데이터 분리
- 무기 스위치 관련 버그 수정
   - 동일한 무기 키 재입력 시 무기 해제
   - 무기 미장착 상태에서 마우스 휠 무기 스위치 제한
   -게임 시작 시 무기 인덱스 초기화 문제 해결
- Debug 로그 및 Gizmos 디버깅 도구 추가

### 메모
- 무기 공격 방식은 WeaponController의 파생 클래스(Melee, Gun)에서 처리
- 공격 판정은 근접 무기는 OverlapCapsule, 원거리는 Raycast 방식 사용
- WeaponData의 coolTime 필드는 제거하고 각 무기 타입에 맞는 방식으로 대체
  - 근접: 애니메이션 클립 길이
  - 원거리: 1 / fireRate 계산으로 쿨타임 대체
- Gizmos는 구형(WireSphere) 두 개로 궤적 시각화 처리

---

## 2025.06.20 (목) 작업 기록

### 주요 작업
- GunWeaponController 기능 확장
  - Raycast 기반 공격 구현 완료
  - 샷건 산탄 처리 (pelletCount, spreadAngle) 적용
  - 조준 상태 구현 (isAiming) 및 조준 시 반동 감소
  - 무기별 반동 적용 (recoilX, recoilY)
  - 반동 보정 계수(aimRecoilMultiplier)에 따른 반동 감쇠
  - 재장전 입력 처리 및 장착된 무기만 재장전 적용
- CameraController 개선
  - Singleton 패턴 적용
  - 조준 상태일 때 카메라 FOV 전환 (aimFOV)
  - 반동 적용 (ApplyRecoil) 및 Lerp 기반 반동 복구
  - 무기별 반동 회복 속도(recoilRecoverySpeed) 반영
  - 줌인 중에는 반동 감소 효과 적용
- WeaponData(ScriptableObject) 확장
  - aimFOV, recoilX, recoilY, aimRecoilMultiplier, recoilRecoverySpeed 필드 추가
  - 무기별 반동 수치 설정 및 FOV 값 지정

---

## 2025.06.21 (금) 작업 기록

### 주요 작업
- PlayerManager 구현  
  - 체력, 마나, 스태미나, 골드 상태 값 관리 및 변경 시 UI 연동
- UIManager, PlayerHUD 구현  
  - 체력/마나/스태미나/탄약/골드 등 상태 수치 HUD에 반영
  - 슬라이더는 비율 기반으로 갱신
  - 탄약은 현재 탄약 / 전체 탄약 형태로 표시
- GunWeaponController에 조준 기능(ADS: Aim Down Sight) 구현  
  - 조준 시 무기 위치를 adsPosition으로 이동하며 부드럽게 전환  
  - originPosition, aimMoveSpeed 등을 활용하여 Lerp 기반 이동 처리
- 카메라 컨트롤러에서 시야각 제한 구현  
  - 무기 미장착 상태: ±clampAngle  
  - 무기 장착 시: ±30도  
  - 조준 중일 때: ±10도로 추가 제한
- 총기 공격 및 쿨타임, 탄약 소비 동작 점검 및 디버깅  
  - fireRate 기반 쿨타임 적용, 사격 후 탄약 감소  
  - 조준 상태에서도 자연스러운 공격 가능하게 조정
- 플레이어 에셋(fbx) 및 총기류 에셋 연결  
  - 적절한 위치 및 회전값 적용을 통한 시각적 자연스러움 확보
- 검 무기 에셋 추가 및 WeaponManager에 연결 완료

### 메모
- 시야각 제한은 무기 장착 → 조준 순으로 우선순위가 정해지며, 조준 상태가 더 강하게 제한됨
- 무기 조준 시 카메라 중심에 무기 모델이 가려지는 현상을 Lerp 이동으로 완화
- 총기 초기화 시 탄약 UI도 초기화되도록 Start()에서 UIManager 호출 추가
- 무기 장착 시만 조준이 가능하도록 InputManager에서 제한 적용됨

---

## 관련 문서

- [Input_Structure_Design.md](./Docs/Input_Structure_Design.md) - 입력 구조 설계 문서