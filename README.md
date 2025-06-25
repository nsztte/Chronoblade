
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

## 2주차 목표

- [ ] Enemy FSM 구조 설계 및 상태 분리 (Base + 유형별)
- [ ] Enemy 에셋 연결 및 애니메이터 구성
- [ ] Enemy NavMeshAgent 설정 및 장애물 회피 테스트
- [ ] Enemy AI 동작 테스트 (이동, 공격, 피격 반응 포함)
- [ ] Player 애니메이션 연동 및 상태 전환 처리
- [ ] ItemManager 및 회복 아이템 효과 설계
- [ ] 스태미너 시스템 구현 및 소모 처리
- [ ] InventoryManager 구조 설계 및 아이템 연동
- [ ] 시간 슬로우 기능 구현 (시간 정지 전 단계)
- [ ] 리듬 판정 시스템 기초 설계
- [ ] 슬로우 + 리듬 공격 연동 테스트
- [ ] UI 레이아웃 구성 및 퀵슬롯/무기정보 UI 설계

---

## 폴더 구조

```
Assets/
├── _Project/
│   ├── Animations/
│   │   ├── Enemies/
│   │   ├── Player/
│   │   ├── Weapon/
│   ├── Art/
│   │   ├── Model/
│   ├── Data/
│   │   ├── Weapon/
│   ├── Materials/
│   ├── Prefabs/
│   ├── Scenes/
│   └── Scripts/
│   │   ├── Enemy/
│   │   │   ├── FSM/
│   │   ├── Player/
│   │   │   ├── Weapon/
│   │   ├── Systems/
│   │   └── UI/
```

---

## 참고/이슈 기록
- [해결됨] CharacterController와 Rigidbody 중 선택 테스트
- [보류] Damage 입었을때 event 함수 등록 및 호출 도입할지 고민
- [진행중] 플레이어 애니메이터 Generic vs Humanoid

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

## 2025.06.23 (월) 작업 기록

### 주요 작업
Enemy FSM(상태머신) 시스템 구현
 - EnemyStateMachine과 상태 인터페이스(EnemyState) 설계
 - 상태별 클래스 작성: IdleState, ChaseState, AttackState, DeadState
 - Enemy 스크립트에서 상태 전환, 체력, 공격력, 탐지 범위 등 관리
 - 공격 쿨타임(cooldown), 범위에 따른 타격 로직 처리
 - Enemy.Die() 함수 작성 → EnemyDeadState에서 호출하여 사망 처리
- 피스톨 재장전 애니메이션 클립 간소화 및 자연스러운 연출로 교체
- 샷건, 라이플에도 동일한 구조로 재장전 애니메이션 클립 적용
- GunWeaponController에서 IsReloading 트리거로 애니메이션 작동 처리
- 검 들고 있을 때의 Idle 및 공격 애니메이션 개선 (보다 자연스럽게)
- 상체 전용 애니메이션을 위해 하반신 키프레임 제거 → UpperBody 레이어에 Override로 설정
- IsAttacking 트리거로 검 휘두르기 애니메이션 작동
- PlayerManager에서 애니메이터 제어 메서드 추가
  - SetAnimatorTrigger(string)
  - SetAnimatorBool(string, bool)
  - SetAnimatorFloat(string, float)
  - SetAnimatorFloat(string, float, float, float) ← 오버로드 버전
- 외부 스크립트에서 애니메이션을 직접 접근하지 않고 PlayerManager를 통해 제어 가능하도록 구조화

### 메모
- 총기 무기는 손 오브젝트에 애니메이터가 따로 붙어 있으며, 트리거로 동작
- 근접 무기는 전체 플레이어 애니메이터를 사용하며 상반신 레이어를 통해 애니메이션 재생
- 적 FSM은 추후 보스 AI나 패턴에도 그대로 확장 가능하도록 설계됨

---

## 2025.06.24 (화) 작업 기록

### 주요 작업
- Chrono Monk, Mirror Duelist FSM 공격 상태 구현
  - ChronoAttackState: 일정 거리 이상일 때 순간이동, 가까우면 공격 실행
  - MirrorAttackState: 클론 생성 → 본체와 위치 교환 → 쿨다운마다 반복 공격
- FakeClone 스크립트 작성
  - 피격되면 파괴, 일정 시간 이후 자동 소멸
- Enemy 클래스에 근접/범위 공격 메서드 분리 구현
  - DealDamageWithCapsule() → Watcher, Mirror
  - DealDamageWithSphere() → Chrono Monk (슬로우 디버프 적용)
- 플레이어에게 IDamageable 인터페이스 적용
  - 공격 피격 시 데미지 적용 및 로그 출력
- 클론 스폰 위치 랜덤화(CloneSpread) 및 라이프타임 변수 관리
- 적 공격 애니메이션을 애니메이션 이벤트 기반으로 트리거
  - 공격 실행 시 Enemy 내부의 해당 공격 메서드 실행
- FSM 구조 개선
  - EnemyAttackState를 상속받아 몬스터 특성에 맞는 커스텀 공격 구현
- 레이어/태그 구조 정리
  - 플레이어는 루트 오브젝트에만 Player 레이어 및 태그 부여
- 애니메이션 리깅 방식 롤백
  - 플레이어 애니메이션 리깅을 Humanoid → Generic으로 되돌림
  - 기존 리타게팅 문제로 인한 포즈 깨짐, 본 축 오류 등 개선

### 메모
- 몽크의 순간이동은 공격 전 범위 진입용이며, 이후 일정 쿨다운마다 재사용
- Mirror Duelist의 클론은 본체와 혼동 유도 목적이며 현재는 단순 시각 효과
- 향후 클론도 공격을 하거나, 클론 생성 타이밍을 조건 기반으로 설정 가능
- Patrol 상태는 보류 중이며, 필요 시 FSM에 자연스럽게 확장 가능
- 블렌드 트리 vs 파라미터 분기 방식은 적보단 플레이어 애니메이션에 더 적합
- 플레이어 애니메이션을 Humanoid에서 Generic으로 되돌려 커브 보정 및 커스텀 제어가 용이해짐

---
 
## 2025.06.25 (수) 작업 기록

### 주요 작업
- Watcher FSM 및 애니메이션 연동 완료
 - Idle ↔ Run 블렌드 트리 구성
 - Attack 애니메이션 클립 적용
 - PerformAttack 함수 애니메이션 이벤트로 연결
 - FSM Attack 상태에서 항상 플레이어를 바라보도록 LookAtPlayer() 호출
- 애니메이션 클립 위치/방향 문제 수정
 - Apply Root Motion 비활성화
 - RootTransformPosition/Rotation 설정으로 땅에 박히거나 사선 공격되는 문제 해결
- 이동 문제 해결
 - 적이 NavMesh 위에 있음에도 불구하고 움직이지 않던 문제의 원인이 애니메이션 클립임을 확인
 - 루트 모션 제거 후 정상 이동 확인
- 충돌 중복 문제 해결
 - 공격이 두 번 발생하는 문제를 플레이어의 CapsuleCollider 제거로 해결
- 공격 판정 문제 개선
 - 플레이어가 정지 시 첫 타 이후 공격이 무시되는 현상을 radius 값 증가로 해결

### 추가 개선 작업
- Head 렌더러를 ShadowsOnly로 설정하여 1인칭 시야에서 머리 겹침 문제 해결
- 카메라 위치 및 시야각(ClampAngle), Near Clip Plane 등 세부 설정 조정
 - 벽이나 적 통과 현상 최소화
 - 공격 시 시점 튐 방지
- 공격 중 카메라 위치 변경 로직 주석 처리
- 캐릭터 컨트롤러의 Radius 수치 확대
 - 벽이나 오브젝트에 더 자연스럽게 충돌
 - 공격/웅크림 시 상체 돌출로 인한 벽 뚫림 방지 강화
- 플레이어 애니메이터를 Humanoid 타입으로 전환
 - 리타게팅 및 상반신/하반신 분리 애니메이션 준비 기반 마련
- 크로노몽크 캐릭터 에셋 프로젝트에 추가
- 애니메이션 리그 타입을 Humanoid → Generic으로 설정
- Generic 리깅 기반으로 idle 애니메이션 클립 직접 제작
  - 위치 및 포즈 수동 조정으로 공중 부양 느낌 구현

### 메모
- 애니메이션 클립의 RootTransform 설정을 통해 시각적, 물리적 위치 불일치 문제를 효과적으로 해결
- FSM 설계와 애니메이션 타이밍 동기화를 통해 공격 판정의 정확도 향상
- 사소한 요소(중복 Collider 등)도 FSM과 충돌 판정에 큰 영향을 줄 수 있음을 재확인함

- Head 메시를 ShadowsOnly로 설정하면 카메라 내부 충돌 문제를 해결하면서도 그림자는 유지 가능
- 카메라 위치를 정위치로 복원하면서 시점 흔들림 및 벽 통과 문제도 대부분 해소됨
- CharacterController의 충돌 범위를 넓히면서 공격 애니메이션 시 벽 간섭 현상이 크게 줄어듦
- Humanoid 전환은 앞으로 무기/스킬마다 상체 레이어를 분기 처리할 때 매우 유용
- 크로노몽크처럼 부양, 텔레포트 등 위치 중심 연출이 많은 캐릭터는 Generic 리깅이 훨씬 유리함
- 이후 공격, 순간이동 등도 Generic 기준으로 애니메이션 제작 예정

---

## 관련 문서

- [Input_Structure_Design.md](./Docs/Input_Structure_Design.md) - 입력 구조 설계 문서