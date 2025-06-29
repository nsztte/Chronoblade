
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

- [x] Enemy FSM 구조 설계 및 상태 분리 (Base + 유형별)
- [ ] Enemy 에셋 연결 및 애니메이터 구성
- [x] Enemy NavMeshAgent 설정 및 장애물 회피 테스트
- [x] Enemy AI 동작 테스트 (이동, 공격, 피격 반응 포함)
- [x] Player 애니메이션 연동 및 상태 전환 처리
- [x] ItemManager 및 회복 아이템 효과 설계
- [x] 스태미너 시스템 구현 및 소모 처리
- [x] InventoryManager 구조 설계 및 아이템 연동
- [x] 시간 슬로우 기능 구현 (시간 정지 전 단계)
- [x] 리듬 판정 시스템 기초 설계

---

## 폴더 구조

```
Assets/
├── _Project/
│   ├── Animations/
│   │   ├── Enemies/
│   │   ├── Player/
│   │   ├── Weapons/
│   ├── Art/
│   │   ├── Model/
│   ├── Data/
│   │   ├── Combat/
│   │   │   ├── ComboAttack/
│   │   │   ├── ComboSequence/
│   │   ├── Enemy/
│   │   ├── Item/
│   │   ├── Weapon/
│   ├── Materials/
│   ├── Prefabs/
│   │   ├── Enemy/
│   ├── Scenes/
│   └── Scripts/
│   │   ├── Enemy/
│   │   │   ├── FSM/
│   │   ├── Item/
│   │   ├── Player/
│   │   │   ├── Weapon/
│   │   ├── Systems/
│   │   │   ├── Combat/
│   │   │   ├── Interaction/
│   │   │   ├── Time/
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

## 2025.06.26 (목) 작업 기록

### 주요 작업
- 아이템 시스템 전반 구현
  - ItemData ScriptableObject 설계 (itemName, icon, itemType, value 등)
  - isAutoPickup 필드 추가로 자동 습득 여부 설정 가능
  - ItemManager 구현: 아이템 효과 사용 처리 및 탄약 충전 분기 처리
- 인벤토리 시스템 구축
  - InventoryManager 구현: 아이템 추가, 스택 처리, 사용 처리 기능 완비
  - enum(ItemType, ConsumableItemEffectType, AmmoType) 정의 및 연동
- PlayerManager 기능 확장
  - HP, MP, Stamina 자연 회복 시스템 추가
    - MP: 사용 후 2.5초 지연 뒤 (최대 MP 3% + 고정 1.5)/초 회복
    - Stamina: 0.5초 지연 뒤 초당 25 회복
  - 자원 변수 타입 int → float 전환, UI 갱신 시 Mathf.RoundToInt 처리
  - 접근 제한을 위한 Property 기반 변수 노출 방식 도입
- 스태미너 기반 소비 시스템 도입
  - MeleeWeaponController: 근접 공격 시 staminaCost 소모 (기본값 25)
  - PlayerController: 달리기 중 초당 15 스태미너 소모, 0일 경우 자동 중지
- 상호작용 시스템 도입
  - InputManager: F키 입력 시 OnInteract 이벤트 발생
  - IInteractable 인터페이스 도입: Interact() 메서드 기반 상호작용 구조 정립
  - ItemPickup:
    - IInteractable 구현
    - 자동 습득 / F키 상호작용 통합 관리
  - PlayerManager:
    - F키 입력 시 주변 IInteractable 중 가장 가까운 대상과 상호작용 수행

### 메모
- MP 회복은 하이브리드 방식 (비율 + 고정량)
- 자연 회복 중 RestoreMP/RecoverStamina 호출 시 타이머 꼬임 없이 정상 작동
- 상호작용 구조가 통일되어 다양한 오브젝트 확장에 유리함
- 플레이어 자원 변수는 float로 세밀 제어, 외부 노출 최소화

---

## 2025.06.27 (금) 작업 기록

### 주요 작업
- 시간 시스템 구조 구현
  - TimeInputHandler 구현: Q/E키 입력 방식에 따라 시간 조작 이벤트 분리 (탭/홀드)
  - TimeManager 구현: ITimeControllable 등록 시스템 도입 및 시간 슬로우 적용 처리
  - EnemyTimeController 구현: NavMeshAgent 및 Animator에 TimeScale 반영
  - Enemy에서 GetAdjustedDeltaTime()을 사용하여 FSM, 사망 지연 시 시간 반영
  - 시간 관련 처리 주기를 OnEnable → Start()로 옮겨 사이클 꼬임 해결

- 리듬 판정 시스템 구조 설계
  - TimingComboManager 구현
    - 일정 주기의 박자(beatInterval) 기준으로 입력 판정
    - Perfect / Good / Miss 구간 설정 및 유효 입력 시간 제한 처리
    - 입력을 리스트 형태로 관리하여 빠른 연속 입력 대응
    - 판정 결과를 OnTimingJudged 이벤트로 송출
    - InputManager.OnAttackPressed와 연동하여 공격 입력 버퍼링
    - 박자 계산은 startTime + beatInterval * N 형태로 정밀하게 보정
  - 리듬 판정에 따른 데미지 보정 배율 (Perfect/Good 배율) Inspector에서 조정 가능

- 연계 공격 데이터 구조 설계
  - ComboAttackData ScriptableObject 생성
    - 개별 공격 애니메이션, 데미지, 넉백, 사운드, VFX, 입력 윈도우 및 연결 정보 포함
    - 타이밍 판정 여부(useTimingJudgement) 설정 가능
  - ComboSequence ScriptableObject 생성
    - 콤보 단위 그룹 구성 (아이콘 + 연속 공격 데이터 리스트)

### 추가 개선사항
- FSM(PlayerStateMachine) 구조 도입으로 상태별 입력 처리 및 동작 분리
  - `PlayerBaseState`: 공통 상태 인터페이스 (Enter, Update, Exit 정의)
  - `PlayerStateMachine`: 현재 상태를 관리하며 상태 전이(ChangeState) 수행
- `PlayerLocomotionState`에서 InputManager 이벤트 구독/해제 처리
  - 이동, 점프, 웅크리기, 달리기 입력을 상태 내부에서 직접 처리
- `PlayerController`는 상태에 따라 호출되는 실행 메서드만 담당
  - 이동, 점프, 중력 적용, 애니메이터 값 변경 등을 내부 메서드로 캡슐화
- 입력 → FSM → PlayerController → 애니메이션/이동 실행 구조로 역할 분리 확립

### 메모
- 시간 슬로우, FSM, 애니메이션, 사망 처리 모두 정상 작동 확인
- 타이밍 판정은 매 박자마다 이루어지며, 입력은 유효 시간 내에만 판정
- TimingComboManager는 전투 시스템에 연결될 준비가 완료되었으며, 다음 주 FSM 및 콤보 실행 로직과 통합 예정
- ComboAttackData와 ComboSequence는 확장성과 관리 편의성을 고려해 ScriptableObject로 구성
- 기존 PlayerController 내부에서 처리하던 입력 로직을 FSM 상태별로 분산시킴으로써 역할 분리가 명확해짐
- 추후 `AttackState`, `JumpState`, `RewindState` 등 추가 시에도 상태별 입력과 동작을 독립적으로 구현 가능
- 플레이어 FSM과 에너미 FSM은 입력 구조가 다르므로, 구조 통일보다는 각자의 책임에 맞춘 방식으로 유지
  - 플레이어 FSM은 입력 기반 설계, 에너미 FSM은 AI 기반 자율 설계
- InputManager 이벤트 구독/해제를 FSM에서 담당함으로써 상태 전이에 따른 입력 제한 제어가 용이해짐

---

## 2025.06.28 (토) 작업 기록

### 주요 작업
- `ChronoMonk` 전용 스크립트 분리
  - `Enemy` → 추상 클래스화
  - `ChronoMonk`, `Watcher`, `MirrorDuelist` 각각 별도 클래스로 상속 구성
  - `OnPerformAttack()`을 통해 타입별 공격 로직 분리
- 크로노몽크 전투 로직 개선
  - 근접 시 텔레포트, 중거리 발사체, 원거리 기습으로 패턴 구분
  - `AttackState` 진입 시 바로 공격 가능하도록 `lastAttackTime = 0` 처리
  - 10% 확률로 랜덤 텔레포트 기능 추가
- 크로노몽크 텔레포트 기능 분리
  - `TryTeleport()` 로직 Enemy가 아닌 ChronoMonk에서 관리
  - `OnChronoTeleport()` / `OnChronoTeleportParticle()` 애니메이션 이벤트 함수 구현
  - `ParticleSystem.Play()` 및 `Stop()` 연동 처리
- 애니메이션 클립 정리 및 제작
  - 공격, 피격, 텔레포트, 체이스 클립 제작 및 애니메이터 구성 완료
  - Death 애니메이션 대신 피격 클립 + 연기 이펙트로 대체 연출 구현
- 파티클 연동 및 제어
  - 텔레포트 연기 파티클 효과 적용
  - Death 시에도 연기 파티클만 재생되도록 분리 설계

### 메모
- 추상 `Enemy` 기반 구조로 확장성과 유지보수성이 크게 향상됨
- 크로노몽크가 지능적인 거리 기반 전투를 구현하게 됨
- 애니메이션 이벤트 기반의 공격/텔레포트 시스템이 안정적으로 작동
- 피로 누적으로 Death 애니메이션 제작은 보류 (Hit 애니메이션 + 이펙트로 대체)
- `GetComponentInChildren` 사용 시 중복 파티클 제어 이슈가 있었고, 해결 완료

---

## 2025.06.29 (일) 작업 기록

### 주요 작업
- Enemy 스크립트 구조 리팩토링
  - `Enemy.cs`에서 공격 관련 변수(attackStartPosition, attackEndPosition 등) 제거
  - Watcher, MirrorDuelist 등 각 적 클래스에서 전용 변수와 공격 로직 개별 정의
  - 기즈모 관련 `OnDrawGizmosSelected()`도 Enemy에서 제거하고 개별 클래스에서 정의
  - 적 타입별 역할 분리를 통한 책임 명확화 및 확장성 향상
- 플레이어 공격 방식 개선
  - 기존: 애니메이션 클립 길이를 기준으로 공격 지속 시간 계산
  - 개선: 애니메이션 이벤트를 활용하여 공격 타이밍 정확히 조절
  - 불필요한 시간 계산 제거로 직관적이고 신뢰도 높은 타격 판정 구현
- MirrorDuelist 애니메이션 클립 연결
  - Idle / Walk / Attack / Spawn / Hit / Death 애니메이션 클립 적용 및 연결
  - 클립 전환과 상태 반영이 매끄럽도록 Animator 설정 조정
- 클론 프리팹 추가 및 Mirror 공격 로직 개선
  - MirrorDuelist용 클론 프리팹 제작 및 적용
  - 클론 생성 로직을 FSM(MirrorAttackState)에서 본체(MirrorDuelist) 스크립트로 이동
  - 애니메이션 클립에서 이벤트로 클론 생성 실행
  - 공격 도중에는 피격 및 상태 전환이 되지 않도록 로직 보완
    - 공격 중 피격 시 HitState로 전환 금지
    - 공격 중 ChaseState로 전환 금지
  - 전투 중 안정성 향상 및 의도된 공격 흐름 유지

### 추가 개선 사항
- 타입 캐스팅 안전성 개선
  - `ChronoAttackState`에서 `Enemy` → `ChronoMonk` 캐스팅 시 `as` 연산자와 null 체크 적용
  - 잘못된 캐스팅 시 에러 메시지 출력으로 디버깅 용이성 향상
  - 중복된 캐스팅 제거로 성능 및 가독성 개선
- 상태 전환 구조 개선
  - `EnemyStateMachine` 내 `switch` 문 기반 상태 전환 → `Dictionary<EnemyStateType, EnemyBaseState>`로 대체
  - 신규 상태 추가 시 `InitStateDictionary()`만 수정하면 자동 연동
  - 확장성과 유지보수성 향상
- Strategy 패턴 도입: EnemyHitState
  - 각 적 타입의 피격 반응을 `IHitBehavior` 인터페이스로 분리
  - 예: ChronoMonk는 피격 후 일정 시간 뒤 텔레포트하도록 별도 클래스 처리
  - `EnemyHitState`는 더 이상 적별 분기처리 없이 `IHitBehavior.Execute()`만 호출
- 성능 최적화
  - 매 프레임 수행되던 `Vector3.Distance()` 계산 → `0.1초 간격`으로 캐싱
  - 적용 대상: 
    - `EnemyAttackState`
    - `ChronoAttackState`
    - `MirrorAttackState`
    - `EnemyChaseState`
  - 반복 연산 최소화로 FPS 안정성과 CPU 부담 감소

### 메모
- MirrorDuelist의 클론 생성 방식은 향후 개수 조절 및 속도 튜닝 필요
- Enemy 구조 분리는 다른 적 타입 확장(예: 보스) 시 유용하게 작용할 것
- 애니메이션 이벤트 기반 공격 처리는 FSM과 자연스럽게 결합되어 앞으로도 사용할 수 있음
- FSM 및 전투 관련 코드가 확장에 유리한 구조로 재편됨
- Enemy 스크립트의 SOLID 원칙 준수도 향상됨
- 후속 작업에서 BossEnemy 전용 FSM 확장 시 활용 가능성 높음

---

## 관련 문서

- [Input_Structure_Design.md](./Docs/Input_Structure_Design.md) - 입력 구조 설계 문서