
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

## 관련 문서

- [Input_Structure_Design.md](./Docs/Input_Structure_Design.md) - 입력 구조 설계 문서

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
- [x] Enemy 에셋 연결 및 애니메이터 구성
- [x] Enemy NavMeshAgent 설정 및 장애물 회피 테스트
- [x] Enemy AI 동작 테스트 (이동, 공격, 피격 반응 포함)
- [x] Player 애니메이션 연동 및 상태 전환 처리
- [x] ItemManager 및 회복 아이템 효과 설계
- [x] 스태미너 시스템 구현 및 소모 처리
- [x] InventoryManager 구조 설계 및 아이템 연동
- [x] 시간 슬로우 기능 구현 (시간 정지 전 단계)
- [x] 리듬 판정 시스템 기초 설계

---

## 3주차 목표

- [x] 리듬 판정 시스템 완전 구현 (Perfect/Good/Miss 처리 및 타이밍 윈도우 조정)
- [x] 플레이어 FSM 이주 완료 (기본 이동, 점프, 공격 등 상태 분리 및 전환 처리)
- [x] 리듬 판정 결과에 따른 콤보 어택 흐름 연동 (Miss 시 중단, Good/Perfect 시 연계)
- [x] 콤보 어택 전용 데이터(ComboAttackData, ComboSequence) 구조 설계 및 구축
- [x] 콤보 어택 애니메이션 연결 (애니메이션 클립 + 상체 레이어 재생)
- [x] 콤보 어택 데미지 처리 로직 구현 (퍼펙트/굿에 따른 배율 적용)
- [x] 콤보 공격 시 넉백 효과 적용 (IDamageable 확장 + Enemy 이동 처리)
- [x] 콤보 어택 시 플레이어 스태미너 차감 로직 반영

---

## 4주차 목표
- [x] 콤보 파이널 스킬 구조화 및 상태이상 시스템 완성
- [x] AOE / 다단히트 / 리듬 기반 데미지 적용 통합
- [x] Player 피격/사망 FSM 상태 처리
- [x] 상점 시스템 + 인벤토리 연동 및 구매/판매 UI
- [x] 시간 시스템 완성 (정지/되감기/빨리감기)
- [x] 리와인드 보간 연출 기반 퍼즐 연계 테스트 준비

---

## 5주차 목표
- [x] 게임 상태머신 시스템 구축
- [x] GameManager 및 상태 흐름 통합
- [x] TimeManager와 상태 동기화
- [x] 플레이어 회피 및 패링 시스템 구현
- [x] 보스 FSM 구조 설계 및 기초 구현
- [x] 보스 전투 패턴 1종 구현 (기초)

---

## 폴더 구조

```
Assets/
├── _Project/
│   ├── Animations/
│   │   ├── Boss/
│   │   ├── Enemies/
│   │   ├── Environment/
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
│   │   ├── Environment/
│   │   ├── GameStates/
│   │   ├── UI/
│   ├── Scenes/
│   └── Scripts/
│   │   ├── Boss/
│   │   │   ├── FSM/
│   │   │   │   ├── Phase1/
│   │   │   │   ├── Puzzle/
│   │   ├── Enemy/
│   │   │   ├── FSM/
│   │   ├── Item/
│   │   ├── Player/
│   │   │   ├── FSM/
│   │   │   ├── Weapon/
│   │   ├── Systems/
│   │   │   ├── Camera/
│   │   │   ├── Combat/
│   │   │   ├── Core/
│   │   │   ├── GameStates/
│   │   │   ├── Input/
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

## 2025.06.30 (월) 작업 기록

### 주요 작업
- PlayerJumpState 구현
  - 점프 입력 시 FSM 전환 구조 완성
  - 점프 시작, 공중 상태, 하강 상태를 하나의 JumpState 내에서 관리
  - 공중 상태 추후 AirborneState로 확장 고려
- PlayerAttackState 구현 및 무기 타입별 입력 처리
  - 근접 무기(Sword): 경직된 공격 → 콤보 진입
  - 원거리 무기(Pistol, Shotgun): 단발 공격
  - 연사 무기(Rifle): 공격 홀드 시 연속 사격
- ComboState 기본 구조 구현
  - PlayerComboState 클래스 생성
  - TimingComboManager 연동 준비
- WeaponController 통합 공격 함수 추가
  - PerformLightAttack, PerformHeavyAttack, PerformWeaponAttack 함수 정리
- FSM 기반 무기 입력 시스템 완성
  - InputManager에서 공격 입력 시 무기 타입에 따라 공격 이벤트 분기
  - 공격 입력 지속 시간에 따라 근접 무기는 경직/강공격 판정
- 무기 교체 로직 방어 처리 추가
  - 무기 교체 시 FSM 상태가 AttackState 또는 ComboState일 경우 무기 변경 불가
  - 공격 중(IsAttacking=true) 무기 전환 금지

### 추가 개선사항
- 공격 지속 시간 대신 `WeaponController.IsAttacking` 기준으로 상태 유지
- AttackState에서 `wasAttacking`을 통해 상태 전환 타이밍 제어
- 연사 무기(Rifle)는 버튼을 누르고 있을 때만 공격 유지되도록 수정
- 무기 타입별 이벤트 등록 및 해제 로직 분기 처리 개선

### 메모
- 현재 구조는 무기 FSM 통합 구조 기반으로 잘 정리됨
- 콤보 시스템과 애니메이션 이벤트 연동은 다음 단계에서 확장 가능
- TimingComboManager 연동 및 애니메이션 타이밍 기반 콤보 설계 필요

---

## 2025.07.01 (화) 작업 기록

### 주요 작업
- 리듬 기반 콤보 시스템 구현
  - `TimingComboManager`에 `StartBeatRoutine()` / `StopBeatRoutine()` 도입
    - 공격 시작 시 비트 루프 시작
    - 콤보 종료 및 이동/점프 등 상태 전환 시 루프 중단
  - `ComboEvaluator` 개선
    - 입력 버퍼에 `Rest` 포함하여 "쉬기" 개념 반영
    - `IsValidStep()` 함수로 콤보 진행 중간 검증 기능 추가
    - `TryMatchCombo()` 내부에서 매칭 성공 여부 판단 및 콤보 발동
- `PlayerComboState` 개선
  - `beatInterval` 기준으로 콤보 공격 단계별 실행
  - 애니메이션 클립 길이를 비트 타이밍에 맞게 속도 조절 (`AttackSpeed` 파라미터 사용)
  - `CrossFadeInFixedTime()` 활용해 상체 애니메이션만 자연스럽게 재생
  - 콤보 입력이 틀릴 경우 즉시 상태 종료
- 애니메이터 구조 간소화
  - 오버라이드 애니메이터 사용 제거
  - 애니메이터 내부에서 상태 전이 없이 직접 애니메이션 클립 재생

### 메모
- 현재 입력 타이밍이 맞아도 **콤보 2타 이상이 정상적으로 이어지지 않는 문제** 존재
  - `IsValidStep()` 판정 또는 비트 시점과 입력 간격 간 오차 가능성 있음
  - 콤보 3타를 입력해도 1타까지만 출력되는 현상 디버깅 필요
- 테스트를 위해 다양한 콤보 시퀀스 데이터 등록 필요 (`Light + Rest + Light` 등)

---

## 2025.07.02 (수) 작업 기록

### 주요 작업
- ComboEvaluator 구조 전면 리팩토링
  - 입력 버퍼 기반 로직 제거, 단일 입력 기반 실시간 평가 방식 도입
  - `RegisterInput()`에서 타이밍 판정 후 콤보 시작/진행/실패 처리
  - `OnComboAttackExecuted`, `OnComboCompleted`, `OnComboFailed`, `OnNormalAttackExecuted` 이벤트로 상태 분리
  - Miss 판정 시 자동 콤보 실패 처리

- TimingComboManager 구조 정리
  - `EvaluateInputs()` 제거
  - `JudgeTiming(float inputTime)` 함수로 타이밍 판정 및 데미지 배율 반환 구조 정리

- PlayerAttackState 리팩토링
  - 무기 타입에 따라 입력 구독 분기 처리 (Sword, Rifle, 일반)
  - ComboEvaluator 이벤트 처리 방식으로 상태 전이
    - 콤보 성공 시 `PlayerComboState`로 전환
    - 콤보 실패 또는 일반 입력 시 Locomotion 상태로 복귀
  - `isComboTriggered` 플래그로 중복 전이 방지

- PlayerComboState 구조 개편
  - ComboEvaluator에서 콤보가 확정되면 상태 진입
  - 단계별 입력 시 타이밍 판정 후 애니메이션 실행 및 데미지 계산
  - Miss 판정, 잘못된 입력, 시간 초과 시 즉시 `PlayerLocomotionState`로 전환
  - 상체 레이어에서 공격 애니메이션 재생 및 애니메이션 속도 조절 반영

- PlayerLocomotionState 개선
  - 공격 입력 시 `JudgeTiming()` 기반으로 콤보 시도
  - 시작 가능한 콤보가 존재하고 타이밍이 맞으면 바로 `PlayerComboState` 진입
  - 실패 시 `PlayerAttackState`로 분기

### 메모
- 기존 버퍼 기반 입력 시스템이 타이밍 오차에 민감하고 관리가 어려워 실시간 입력 기반 구조로 재설계
- 모든 입력 및 판정 흐름이 ComboEvaluator에서 일관되게 처리되도록 통합
- 애니메이션, 데미지, 판정, 상태 전이가 명확히 구분되어 디버깅 및 유지보수 용이

---

## 2025.07.03 (목) 작업 기록

### 주요 작업

- ComboEvaluator 완전 리팩토링
  - 입력 시점마다 후보군 필터링 방식으로 개선
  - PlayerComboState와 직접 연동되도록 구조 변경
  - 첫 입력 실패 및 타이밍 Miss 시 콤보 실패 처리

- PlayerComboState 완전 리팩토링
  - 입력마다 후보군을 줄여가며 단계별로 애니메이션/데미지 적용
  - 막타 입력 시 `GetComboWindow()`만큼 유지 후 상태 전환
  - 입력 실패 시 즉시 콤보 종료
  - 타이밍 판정 결과(Perfect/Good/Miss)에 따라 데미지 배율 및 연출 반영

- ComboEvaluator 후보군 필터링 방식으로 전환
  - 기존 `TryStartCombo`, `GetStartableCombo` 제거
  - `GetMatchingCombos(List<AttackType>)` 기반 필터링 로직으로 일원화

- PlayerAttackState에서 불필요한 이벤트 제거
  - `OnComboAttackExecuted`, `OnComboCompleted`, `OnComboFailed` 삭제
  - 콤보 흐름은 전적으로 `PlayerComboState`에서 관리

- 타이밍 판정 통합 및 연동
  - JudgeTiming 호출 위치를 통일
  - 퍼펙트/굿/미스 결과에 따라 데미지 및 효과 차등 적용

- 마지막 콤보 타 후 상태 전환 개선
  - 즉시 전환이 아닌 `ComboWindow` 시간만큼 유지 후 전환
  - `EndComboAfterDelay(float waitTime)` 코루틴으로 처리

- IDamageable 인터페이스 확장
  - `ApplyKnockback(Vector3 direction, float power)` 메서드 추가

- Enemy 클래스 넉백 처리 구현
  - NavMesh 기반 위치 보정 포함

- PlayerManager에 스태미너 체크 함수 추가
  - `UseStaminaIfAvailable(float amount)` 함수로 조건부 소모 처리

- PlayerController → MeleeWeaponController로 데미지 위임
  - `ApplyComboDamage(damage, knockbackPower)` 함수 추가

- 애니메이션 이벤트 OnComboAttackHit 연동
  - PlayerManager에서 임시 데미지 캐싱 후 처리

- ComboSequence ScriptableObject 확장 준비
  - 방어무시, 상태이상 등 추가 스킬 효과 설계 반영 논의 완료


### 추가 개선

- 총기류 공격 실행시 지연되는 버그 해결
  - PlayerLocomotionState에서 무기 타입에 따라 Input 이벤트 분리
  - 원거리 무기일 경우 OnAttackPressed/OnAttackHeld 시 즉시 발동

- 콤보 첫타 입력 시 타이밍이 맞으면 바로 실행되도록 개선
  - `PlayerComboState.Enter()`에서 첫 공격 실행 + `isWaitingForInput = true` 설정

### 메모

- 리듬 기반 타이밍 콤보 시스템 안정화
- FSM 책임 분리가 명확해짐 (PlayerComboState가 콤보 전담)
- 타격감, 연출, 입력 타이밍 피드백 향상
- 이후 파이널 스킬 효과, 추가 특수 효과 적용 예정

---

## 2025.07.04 (금) 작업 기록

### 주요 작업
- 에너미 넉백 로직 개선
  - 넉백 방향을 에너미의 로컬 기준 뒤쪽으로 설정
  - `SmoothKnockback()` 코루틴 도입 → 자연스러운 넉백 이동 연출

- MeleeWeaponController 개선
  - `ClearHitTargets()` 함수 추가
  - 콤보 타격 시 중복 타격 방지 처리

- PlayerComboState 개선
  - 마지막 콤보 애니메이션은 클립 전체 길이로 재생되도록 조정
    - `ComboSequence`에 `lastAttackAnimSpeed` 필드 추가
    - 마지막 공격 시 해당 속도로 애니메이션 속도 적용
  - 마지막 타 직후 콤보 종료 딜레이 적용
    - `EndComboAfterDelay(float waitTime)` 코루틴 도입
    - 콤보 종료 연출과 타격감을 향상

- 애니메이션 클립 연결
  - `약약약` 콤보 애니메이션 정상 연동

### 메모
- 콤보 마지막 타의 연출이 강화되어 타격감 개선
- 콤보 효과는 추후 연출 스크립트 분리 또는 이펙트 연동 예정
- 다음 작업은 파이널 스킬 이펙트 및 UI 피드백 강화 방향 고려

---

## 2025.07.05 (토) 작업 기록

### 주요 작업
- Repeat(약약약), Break(약약강), Crash(약강강) 콤보 공격 데이터 구성
  - 각 콤보에 대응하는 `ComboAttackData` 및 `ComboSequence` ScriptableObject 생성 및 입력
  - 콤보별 후반부로 갈수록 데미지 증가하는 고정 수치 반영
- 애니메이션 클립 연결 및 이벤트 설정
  - 각 `ComboAttackData`에 맞는 애니메이션 클립 지정
  - 애니메이션 타격 타이밍에 `OnComboAttackHit` 이벤트 등록 처리 완료
- 스태미너 소모 로직 구조 정리
  - `ComboAttackData`에서 `staminaCost` 필드 제거 (공통 수치 사용 방식으로 전환)
  - 스태미너 소모 수치(`Light: 12`, `Heavy: 24`)를 `PlayerManager`에 통합 관리
  - `MeleeAttackController`에서는 더 이상 개별 스태미너 수치를 갖지 않도록 구조 개선

### 메모
- 넉백 수치는 테스트를 통해 추후 직접 조정 예정
- 향후 `ComboAttackData`에 특수효과 필드나 연출 전용 플래그 등을 확장할 여지 있음
- 다음 작업 시 `ComboSequence` 기반으로 입력 리듬 흐름 제어 여부 확인 필요
- Rewind(강약강), Finale(약강약강) 데이터 작성 및 애니메이션 연결 진행 예정

---

## 2025.07.07 (월) 작업 기록

### 주요 작업
- 콤보 공격 데이터 및 애니메이션 클립 추가
  - Repeat(약약약), Break(약약강), Crash(약강강), Rewind(강약강) 콤보 시퀀스 데이터 추가
  - 각 시퀀스에 대응하는 애니메이션 클립 연결
  - `ComboAttackData` 및 `ComboSequence` ScriptableObject 구성 완료

- 콤보 애니메이션 클립 수정
  - 약공격 애니메이션 타이밍 오류 수정
    - 애니메이션 이벤트보다 실제 클립 종료가 더 빨라 발생하는 문제 해결
    - 클립 길이 조정으로 이벤트 발동 타이밍 보정

- 플레이어 피격 및 사망 FSM 상태 구현
  - `PlayerHitState` 구현
    - 피격 시 0.3초간 무적 상태 적용
    - 향후 효과 구현을 위한 화면 효과, 카메라 흔들림, 피격 사운드 구조 마련
    - FSM 기반 자동 복귀 처리
  - `PlayerDeathState` 구현
    - 연출 구조 마련
    - 입력 차단 및 `Time.timeScale = 0` 처리
    - 상태 전환 로직을 `PlayerManager`에서 상태 클래스로 위임하여 책임 분리

### 메모
- 콤보 리듬 시스템은 4가지 패턴까지 확장됨
  - 타이밍 보정 기반 스킬 연계 설계 기초 완료
- 피격/사망 상태는 기본 틀만 구성되어 있고, 연출 관련 로직은 추후 시네머신, UI 시스템과 연계 예정
- `PlayerManager`의 무적 처리와 FSM 전환 흐름이 명확해져 유지보수성이 향상됨

---

## 2025.07.08 (화) 작업 기록

### 주요 작업
- 콤보 막타 상태이상 적용 시스템 구축
  - `ComboAttackData`에 `isFinalHit`, `statusEffectType`, `statusDuration` 필드 추가
  - 막타일 경우 상태이상 효과 발동 가능하도록 설정

- 상태이상 처리 인터페이스 및 구조 설계
  - `IStatusEffectable` 인터페이스 정의  
    - `ApplyStatus(StatusEffectType type, float duration)` 메서드 제공
  - `FinalComboController`에서 인터페이스 구현  
    - 적의 FSM, NavMeshAgent, Animator를 제어하여 `Freeze`, `Slow` 상태 처리

- 콤보 공격 정보 구조화
  - `ComboAttackInfo` 구조체 정의 → `PlayerManager`에 저장
  - 기존 개별 필드(`currentComboDamage`, `currentComboKnockback`, `isFinalHit`) 제거
  - `PlayerComboState.ExecuteCurrentAttack()`에서 구조체 생성 후 전달

- OnComboAttackHit 리팩토링
  - 전달받은 `ComboAttackInfo`를 활용해 데미지, 넉백, 상태이상 효과 적용
  - 상태이상 효과는 `FinalComboController.ApplyStatus(ComboAttackData)` 호출 방식 유지

- 스킬 특성 필드 추가 및 구조 정리
  - `ComboAttackData`에 다음 필드 추가:  
    - `isAOE`: 범위 공격 여부  
    - `aoeRadius`: 범위 반경  
    - `aoeHitCount`: 다단히트 수
  - `StatusEffectType`은 `None`, `Freeze`, `Slow`로 단순화

### 메모
- 파이널 콤보 스킬 구조 분리 및 통합 방향 정립
  - 상태이상은 적 개별 객체(`FinalComboController`)에서 관리
  - AOE/다단히트는 추후 `MeleeWeaponController`에서 별도 처리 예정
- `Repeat`(3연속 히트)는 상태이상이 아닌 별도 특수효과로 `MeleeWeaponController`에서 구현 예정

---

## 2025.07.09 (수) 작업 기록

### 주요 작업

- 상점 시스템 및 인벤토리 연동 구현
  - ShopData / Shop / ShopManager 설계 및 구현
    - ScriptableObject 기반 판매 아이템 관리
    - IInteractable 인터페이스 연동으로 상점 자판기 상호작용 처리
    - ShopManager에서 구매/판매 로직 분리, 감가율 반영
  - ShopUI 상호작용 기능 구현
    - 아이템 버튼 동적 생성 및 선택/해제 로직 구현
    - 선택된 아이템 강조 색상 적용 (노란색)
    - 선택된 아이템 정보 TextMeshPro로 표시
    - Buy/Sell 버튼의 클릭 이벤트를 Start()에서 동적 연결
    - 선택 상태에 따라 버튼 활성/비활성 처리
  - 커서 표시 개선
    - UI 열릴 때 Cursor.lockState 및 Cursor.visible 처리 통합
    - UIManager에서 SetCursorLockState 함수로 일괄 관리
  - Grid Layout Group 설정 변경
    - Fixed Column Count = 3, UpperLeft 정렬
    - Padding, Spacing 조정으로 좌측 상단부터 고정 배치

- InventoryManager 핵심 로직 개선
  - TryAddItem() → 잔여 수량 반환하도록 수정
    - 탄약일 경우 item.value 단위로 계산하여 CeilToInt 반환
    - 과잉 반환으로 인한 무한 획득 버그 수정
  - AddAmmo() → 최대 탄약 수량 초과 시 남는 수량 정확히 계산
  - TryPickup()에서 실패 시 남은 수량 정확히 유지되도록 연동
  - RemoveItem() 및 DropAmmo() 통합 고려 기반 분리 유지

- 콤보 스킬 AOE 및 다단 히트 로직 구현
  - ComboAttackData에 다음 필드 추가
    - isAOE, aoeRadius, aoeHitCount
    - isMultiHit, multiHitCount, multiHitInterval
  - MeleeWeaponController.OnComboAttackHit 확장
    - AOE 여부에 따라 범위 내 적 타격 처리
    - 단일 대상은 ProcessComboAttack() 통해 처리
    - 다단 히트 시 ApplyMultiHit() 코루틴으로 처리
  - 상태이상 효과는 파이널 콤보일 경우에만 FinalComboController를 통해 적용
  - AOE 중복 타격 방지를 위해 hitTargets 리스트 사용
  - 데미지는 TimingComboManager에서 판정된 값으로 전달
  - OnComboAttackHit 내부 로직 분리: ProcessComboAttack(), ApplyAOE(), ApplyMultiHit()

- 슬로우 상태 애니메이션 속도 동기화 및 파이널 콤보 테스트
  - FinalComboController.HandleSlow() 수정
    - agent.speed뿐만 아니라 animator.speed도 함께 감소 적용
    - duration 종료 시 원래 속도로 복구
  - 파이널 콤보 전체 테스트 완료
    - 단일 타격, AOE, 다단 히트, Freeze/Slow 정상 작동 확인
    - TimingComboManager와의 연동도 확인

### 메모
- 콤보 공격에서 다양한 타격 조건을 구조적으로 처리할 수 있도록 통합 리팩토링 진행
- 상점 UI/UX 흐름을 데이터 기반으로 구성하고 커서 처리까지 통합 완료
- 탄약 수량 처리와 아이템 수량 제한 문제 해결됨
- 구매/판매 및 상태이상 공격 테스트 정상 완료

---

## 2025.07.11 (금) 작업 기록

### 주요 작업

- 시간 시스템 전체 구현 완료 (정지, 빨리감기, 되감기)
  - TimeManager 설계 및 TimeState 열거형 정의
    - Normal, Stop, FastForward, Rewind 상태에 따른 시간 제어
    - ITimeControllable 등록 대상에 SetTimeScale() 적용

- TimeInputHandler 구현
  - E 키 탭: 시간 정지 → OnTimeStop 이벤트
  - E 키 홀드: 빨리감기 → OnTimeFastForwardStart 이벤트
  - Q 키 홀드: 되감기 → OnTimeRewindStart 이벤트

- ITimeControllable 인터페이스 정의
  - SetTimeScale(float timeScale), GetTimeScale() 구현 강제화

- 되감기 시스템 구현 (RewindRecorder.cs 기반)
  - RewindSnapshot 구조체로 위치/회전 저장 (속도는 제거됨)
  - Rigidbody 포함 오브젝트에 RewindRecorder 부착하여 상태 저장
  - Q 키 홀드 중 역순으로 snapshot 재생 → 되감기 처리
  - snapshot 소진 시 → isKinematic = true로 정지
  - Q 키 해제 시 → StopRewind() 호출 및 isKinematic = false

- 점진적 되감기 개선 및 보간 연출 추가
  - snapshot 간 위치 보간 시 lerpSpeed가 점점 증가하도록 구성
  - 되감기 속도(rewindInterval)도 점점 짧아지도록 적용
  - 목표 위치에 충분히 근접하면 → 정확히 고정 (snap threshold = 0.01f)
  - hasTarget 플래그로 snapshot 적용 중복 방지
  - 결과적으로 점점 빨라지며 자연스럽게 과거로 되감기되는 연출 완성

### 테스트
- 테스트용 박스(prefab)에 Rigidbody, RewindRecorder 적용하여 낙하 → 되감기 테스트 진행
- Q 키 홀드 시 되감기 시각화 정상 작동 확인
- 시각적으로 자연스럽고, 플레이어 조작 타이밍 파악 가능한 퍼즐용 연출 확보

### 메모
- snapshot 시작 위치와 현재 위치가 크게 차이날 경우 "튀는 현상" 발생할 수 있음
  → 퍼즐 구현 단계에서 별도 보완 예정
- 퍼즐 오브젝트는 보간 기반 연출이 자연스럽고, 플레이어 리와인드는 지양할 예정
- rewindInterval과 lerpSpeed를 동시에 증가시키는 방식이 현재 가장 자연스러움
- 되감기의 전반적인 로직은 퍼즐 구현 후 테스트하면서 보완할 예정

---

## 2025.07.14 (월) 작업 기록

### 주요 작업
- **게임 상태머신 시스템 전체 구현**
  - GameStateMachine, GameManager, GameBaseState 설계 및 스크립트 구현
  - 상태 전환 주체는 GameManager, 실제 전환은 GameStateMachine이 수행
  - Current/Previous 상태를 관리하여 상태 흐름 안정화

- **각 GameState 스크립트 구현 및 시간 흐름 처리**
  - Exploration, Combat, Puzzle, Pause, GameOver, Cutscene, Loading 등 구현 완료
  - Cutscene, Pause 상태에서 Time.timeScale 제어 및 복원 처리
  - PuzzleState는 PreviousState에 따라 시간 초기화 여부 분기 처리

- **시간 시스템 연동**
  - TimeManager에 SetTimeState, InitializeTimeState 함수 보완
  - 타임스케일과 시간 스킬 상태(TimeState.Normal, Stop, Slow 등)의 일관성 유지

- **GameManager를 통한 상태 복귀 및 Exploration 일원화 처리**
  - Cutscene → PreviousState 자동 복귀 구조 도입
  - Combat 상태 종료 시 EnemyManager에서 적 전멸 여부를 판단하여 Exploration으로 전환

- **InputManager에 Pause 키(Esc) 입력 처리 구현**
  - Esc 키 입력 시 GameManager에서 Pause 상태로 진입하거나 복귀
  - MainMenu, Loading, GameOver 상태에서는 일시정지 불가능하도록 예외 처리

- **전투 진입/종료 흐름 구현**
  - Enemy에서 OnCombatStarted 이벤트 호출 → GameManager에서 구독 후 EnterCombat()
  - EnemyManager에서 활성 적 등록/해제 및 전투 종료 판단
  - 적이 모두 사망하면 Exploration 상태로 자동 전환

- **EnemyManager 구조 설계 및 연동**
  - Enemy의 Start에서 자동 등록, 사망 시 Unregister 처리
  - CombatState와 연동되어 상태 종료 흐름에 연결됨

### 메모
- Exploration 상태에서만 저장이 가능하다는 전제를 기반으로 상태 전환 구조를 설계함
- EnemyManager는 Systems/Combat 폴더에 위치, 전투 흐름 제어 전담
- GameState마다 Exit 시 상태 전환 여부를 명시함으로써 흐름을 명확히 함
- Enemy 플레이어 거리 기반 활성화 구조는 향후 최적화 단계에서 도입 예정

---

## 2025.07.15 (화) 작업 기록

### 주요 작업
- CCTV 감지 시스템 구현
  - 시야 각도, 거리, Raycast 기반 감지 로직 완성
  - ViewCone을 3D 원뿔 에셋으로 교체하여 직관적 시각화 구현
  - 시야 범위 조정 및 시각 효과 개선

### 기획 정리
- 시간 퍼즐 구현 보류 결정
  - 퍼즐 기획 아이디어 미정 상태로, 맵 연결 이후 기획 재진행 예정
  - 당분간 퍼즐 구현은 연기하고 보스부터 구현하기로 결정

- 보스 기획: 시간의 수호자 오르렐(Orrel)
  - 시간/기억을 수집하는 고대 시계탑 수호자
  - 전투 중 시간 조작 패턴(슬로우존, 정지) 및 되감기 퍼즐 삽입
  - 감정·연출이 강조된 퍼즐/패턴 연계 설계

### 보스 전투 구조 요약
- Phase1 (100%~70%): 슬래시, 슬로우존, 시간 정지 패턴 중심
- Puzzle1: 되감기 기반 회전 퍼즐 (정확히 맞추면 Phase2 진입)
- Phase2 (70%~5%): 연속 공격, 약점 노출, 강화된 시간 기믹
- FinalPuzzle: 되감기/빨리감기 기반 최종 퍼즐 → 성공 시 피니시

### 보스 FSM 흐름 설계
- 각 Phase는 Idle → 공격 상태 순환 구조
- 체력 조건에 따라 PuzzlePhase1State, FinalPuzzlePhaseState 진입
- 성공/실패에 따라 상태 전이 또는 보스 체력 회복/반격 처리

---

## 2025.07.16 (수) 작업 기록

### 주요 작업

- **플레이어 대쉬 시스템 구현**
  - `LeftAlt` 키 입력 시 대쉬 발동
  - `InputManager`에 `OnDashPressed` 이벤트 추가
  - `PlayerLocomotionState`에서 입력 감지 후 `PlayerDashState`로 전이
  - 대쉬 중 방향 기반 빠른 이동 및 무적 시간 적용
  - 일정 시간 후 `LocomotionState`로 복귀
  - `PlayerController`에 `lastMoveInput` 저장 및 직접 이동 함수 구현

- **플레이어 방어 시스템 구현 (BlockState)**
  - 마우스 우클릭 입력 시 `PlayerBlockState` 진입
  - `InputManager`에 `OnBlockStarted`, `OnBlockCanceled` 이벤트 추가
  - 근접 무기일 경우에만 진입
  - 방어 해제 시 자동으로 `LocomotionState`로 복귀
  - 방어 중에는 스태미너 자연 회복 차단
  - 애니메이터 파라미터 `IsBlocking`과 FSM 연동
  - 방어 중 피격 시 `PlayerManager.TakeDamage()`에서 스태미너를 소비해 데미지 감소 처리
  - 방어 관련 수치 `PlayerManager`에서 통합 관리 (`blockHitCost`, `blockDamageReduction` 등)

- **타이밍 기반 패링 시스템 구현**
  - 방어 해제 시각과 적 공격 시각의 차이를 비교해 패링 성공 여부 판정
  - `PlayerManager.TryParry(float attackTime)` 함수 추가
  - 패링 성공 시 무적 상태 부여 및 데미지 무효 처리
  - 패링 판정은 `Enemy`의 공격 함수(`DealDamagedWithCapsule`, `DealDamagedWithSphere`) 내에서 선검사
  - `BlockState`는 패링 판정 로직과 분리되어 상태 유지만 수행

- **패링 성공 시 에너미 스턴 및 FSM 정지 처리**
  - `Enemy.OnParried()`에서 FSM 정지 및 `agent` 정지 처리
  - 넉백 연출 포함 (`ApplyKnockback`)
  - `Animator.SetBool("IsStunned", true)`로 스턴 애니메이션 적용
  - 일정 시간 후 FSM 복구 및 애니메이션 초기화 (`RecoverFromStun()` 코루틴)
  - `EnemyChaseState`, `EnemyAttackState` 내에서 `isStopped` 체크 추가
  - 스턴 상태 전용 FSM 상태는 도입하지 않고 FSM 자체를 일시 정지하는 방식으로 처리

### 메모

- 스턴 중에도 FSM이 전이되면서 `agent.isStopped`가 해제되는 문제가 있었으며, 이를 해결하기 위해 FSM 자체를 `enabled = false`로 비활성화하는 구조를 채택
- 방어, 패링, 스턴 각각의 로직은 분리하되 상호 연계성 있게 동작하도록 설계
- 연출 요소(이펙트, 사운드, 카메라 흔들림, 애니메이션 등)는 추후 마무리 단계에서 일괄 추가 예정

---

## 2025.07.17 (목) 작업 기록

### 주요 작업
- **보스 FSM 시스템 구조 설계 및 초기 구성**
  - `BossStateMachine` 클래스 설계 및 상태 등록/전이/갱신 흐름 구현
  - `BaseBossState` 추상 클래스 정의: Enter/Update/Exit 구조, BossController 참조 주입

- **Phase 전이 구조 구현 (BossPhaseManager)**
  - 체력 기반 Phase 자동 전이 (Phase1 → Puzzle1 → Phase2 → FinalPuzzle → Ending)
  - 퍼즐 페이즈 전환 조건 0.5로 조정
  - 수동 전이를 위한 `SetPhase()` 함수 작성

- **보스 컨트롤러 설계 (`BossController`)**
  - FSM 및 PhaseManager 초기화 및 연동
  - FSM 상태 업데이트 및 애니메이션 트리거 실행 구조 구성
  - `TakeDamage()` 내에서 체력 감소 + Phase 업데이트 처리
  - `IDamageable` 인터페이스 구현 (넉백 추후 구현)
  - 애니메이션 유틸 함수: `GetCurrentAnimationLength`, `GetAnimationClipLengthFromState`

- **Phase1 공격 FSM 구현**
  - `BossIdleState` 구현: 5가지 공격 패턴 랜덤 선택
  - `HorizontalSlashState`, `VerticalSmashState`, `EnergyBoltState` 구현
  - 공통 상속 `BaseBossAttackState`를 통해 애니메이션 길이 기반 상태 복귀 처리
  - 특수 공격 상태: `SpawnSlowZoneState`, `TimeStopAttackState` 구현
    - 연출 스텁 함수만 정의, 실제 효과는 추후 연동

- **Phase1 종료 및 Puzzle1 전이 FSM 구성**
  - `CheckPhase1EndState` 구현: 체력 ≤ 50% 조건일 때 퍼즐 상태 진입
  - 모든 공격 상태에서 종료 시 `CheckPhase1EndState`를 거쳐 전이 구조 통일
  - `BossIdleState`에서도 체력 조건 만족 시 퍼즐로 진입 가능하도록 처리
  - `PuzzlePhase1State` 생성 (아직 퍼즐 입력/연출 미구현)

- **애니메이션/이펙트 연출 스텁 정의**
  - `BossController` 내 `SpawnSlowZoneAtPosition()`, `StartTimeStopEffect()` 등 연출용 자리 확보
  - 각 상태 내에서 스텁 호출까지 연동 완료

### 메모
- Animator 파라미터 트리거는 모두 등록 완료 (애니메이션 클립은 아직 미연결 상태)
- `bool` 타입 애니메이션 파라미터는 현재 구조에선 불필요, Phase2에서는 필요할 가능성 있음

---

## 2025.07.18 (금) 작업 기록

### 주요 작업
- **보스 공격 시 회전 및 예고 처리 로직 개선**
  - `BaseBossAttackState`에 `boss.LookAtPlayer()` 호출 추가
  - `isWindingUp` 플래그 기반으로 예고 시간 동안 플레이어 방향 회전 유지
  - 예고 종료 후 방향 고정되도록 구현
  - `windingUpDelay`만 설정하면 자동 적용되도록 설계

- **슬로우존 생성 및 플레이어 디버프 시스템 구현**
  - `SpawnSlowZoneState`에서 보스 애니메이션 → 애니메이션 이벤트 통해 `BossController.SpawnSlowZoneAtPosition()` 호출
  - 보스가 직접 슬로우존 생성 책임을 가지도록 구조 설계
  - `PlayerController`에 슬로우 처리 로직 추가
    - 이동 속도 및 애니메이션 속도 감소 / 복원
    - 중복 적용 방지
  - `SlowZone` 트리거 영역 진입/이탈로 디버프 적용/해제
  - `IStatusEffectable` 인터페이스에 `ApplyStatus(StatusEffectType)` 및 `RemoveStatus(StatusEffectType)` 오버로드 추가

- **보스 타임스탑(TimeStop) 패턴 전체 구현**
  - `TimeStopAttackState` 구성 및 애니메이션 연동
  - `StartTimeStopEffect()` → `TimeManager.SetTimeState(Stop)` 호출, 플레이어 Freeze 처리
  - `EndTimeStopEffect()` → `Normal`로 복원 및 상태 해제
  - `PlayerController`에서 Freeze 상태 적용 및 복원 처리
  - `BossController.TriggerTimeStopAttack()` 함수 구현
    - 애니메이션 이벤트에서 호출, 연출/히트박스 분리 가능
  - 타이밍은 애니메이션 이벤트로 처리하여 유지보수성 향상

- **FSM 제어 방식 개선 리팩토링**
  - 모든 공격 스테이트에 `isWindingUp / hasHandled` 구조 적용
  - `Update()`에서 조건 만족 시 `HandleAttack()` 실행
  - 기존 `WaitForSeconds` 제거 → 이벤트 기반 FSM 흐름 전환
  - 시간 스킬 사용 시에도 안정적으로 FSM 흐름 유지

### 추가 작업
- **프리즈 상태에서 입력 전체 차단 처리**
  - `InputManager.Update()`에 `PlayerManager.Instance.IsFrozen` 분기 추가
    - 이동, 무기 교체, 공격 등 모든 플레이어 입력 차단
  - `PlayerController`에서 프리즈 상태 시 `moveSpeed = 0` 처리하던 기존 로직 제거

- **`PlayerManager` → `IsFrozen` 속성 정의**
  - `PlayerController.isFrozen`을 참조하여 외부에 상태 전달
  - 상태 분기 시 한 곳에서 제어 가능하도록 정리

- 결과적으로 프리즈 상태는 이동 불가 뿐 아니라 모든 조작이 완전히 정지되도록 동작
  - 시간 정지 패턴 등에서도 안정적인 조작 차단 가능
  - 향후 상태이상(Stun, Sleep 등) 확장 가능성 고려한 구조

- **TimeManager.cs**
  - `SetTimeStop(bool isTimeStop)` 함수 추가
    - TimeState를 변경하지 않고, 등록된 `ITimeControllable` 대상에게만 `SetTimeScale(0/1)` 적용
    - 플레이어는 포함되지 않으므로 시간 정지 중에도 Freeze로 제어 필요
  - 기존 `SetTimeState()`는 플레이어의 시간 조작(정지/재생)에만 사용

- **BossController.cs**
  - `StartTimeStopEffect()` / `EndTimeStopEffect()` 구조 변경
    - 보스가 시간 정지를 시작/해제할 때는 TimeManager.SetTimeStop(bool)만 호출
    - 동시에 `PlayerController.ApplyStatus(StatusEffectType.Freeze)` 호출로 조작 제한

- **PlayerController.cs**
  - Freeze 상태 처리 개선
    - 이동속도 설정 제거 (입력 자체 차단되므로 중복 처리 방지)
    - 애니메이션 정지(0f), 복원(1f)은 그대로 유지
    - 중복 Freeze 방지를 위한 플래그 포함

### 메모
- 보스는 시간 스킬 면역이지만, 콤보 디버프는 추후 별도 논의
- 이벤트 기반 구조로 변경하면서 연출 타이밍 연동이 수월해짐
- 슬로우는 PlayerController에서 이동/애니메이션만 느려지는 상태로 유지
- 프리즈는 FSM/입력 레벨에서 전반적 차단을 통해 완전한 정지 상태 구성
- 플레이어가 시간 정지 해제 키를 눌러도 currentTimeState는 Normal로 유지됨
- 하지만 Freeze 상태가 유지되므로 보스가 설정한 타임스탑은 플레이어에 의해 해제 불가
- 보스의 시간 지배력과 패턴 연출 신뢰도 확보
- 다음 작업: FSM 순환 흐름 테스트, 히트박스 임시 적용, BossModel 연결 (주말 예정)

---

## 2025.07.18 (일) 작업 기록

### 보스 공격 애니메이션 클립 연결 및 회전 처리 개선

- **공격 스테이트별 애니메이션 클립 연결**
  - `HorizontalSlashState`, `VerticalSmashState`, `EnergyBoltState`, `TimeStopAttackState`, 'SpawnSlowZone' 에 Animator 트리거 연결
  - Animator 상태 머신 구성 및 공격 트리거 정상 작동 확인
  - 각 공격에 대응되는 애니메이션 클립에 공격 타이밍 이벤트 등록

- **공격 중 회전 뚝 끊김 현상 개선**
  - `BaseBossAttackState`에 `LookAtPlayer(float rotationSpeed)` 함수 도입
  - 회전 속도를 상황에 따라 분리 적용:
    - **예고 중**(isWindingUp = true): 빠른 회전 (rotationSpeed = 12)
    - **공격 중/후**: 느린 회전 (rotationSpeed = 6)
  - 공격 중에도 Slerp 기반 부드러운 회전 유지로 끊김 없는 전투 연출 확보

- **Update() 내 회전 처리 구조 변경**
  - 각 스테이트의 `Update()`에서 상황에 맞는 `LookAtPlayer()` 호출
  - 이전처럼 예고 후 회전 정지하는 방식 대신, 항상 회전 유지하되 속도만 분리

- **공격 판정 타이밍 이벤트 등록**
  - 애니메이션 클립 내에 등 이벤트 등록
  - 판정 타이밍은 애니메이션 기준으로 직접 조정 가능하여, 리듬감 있는 타격 타이밍 연출 가능

### 💡 메모
- 플레이어 위치에 따라 보스가 자연스럽게 시선 유지
- 공격 직전에 빠른 회전으로 시선 고정, 공격 중엔 뚝 끊기지 않고 부드럽게 회전 유지
- 애니메이션 이벤트 기반 타이밍 분기 구조는 향후 이펙트/사운드에도 활용 가능

---

## 2025.07.21 (월) 작업 기록

### 주요 작업
- **보스 공격 히트박스 시스템 구현**
  - 히트박스 오브젝트를 **오른손 자식으로 배치**하여 공격 위치 정밀 추적
    - 검 대신 손에 부착하여 애니메이션과 정확히 동기화
    - Box 형태의 히트박스를 마커 기준 위치/크기/회전으로 계산
  - `FollowSlashHitbox` 코루틴 구현
    - 일정 시간 동안 마커를 따라가며 `Physics.OverlapBox`로 충돌 감지
    - `IDamageable` 구현 대상에게 데미지 적용
  - `TriggerHorizontalSlash()`, `TriggerVerticalSlash()` 등에서 애니메이션 이벤트로 히트박스 트리거
    - `isWindingUp` 해제 후 히트박스 생성
    - `TriggerTimeStopAttackHitbox()`에서는 정지 공격 전용 히트박스 생성
  - `OnDrawGizmos()`를 활용해 마커 기준 히트박스 실시간 시각화 지원

- **퍼즐 바늘 회전 로직 및 시간 시스템 연동**
  - `PuzzleHand.cs` 작성
    - z축 기준 일정 속도로 회전하는 시계 바늘 구현
    - `isRight` 설정으로 시계방향/반시계방향 선택 가능
    - `ITimeControllable` 인터페이스 구현 → `SetTimeScale()`로 회전 속도 제어 (빨리감기 대응)
    - `IRewindable` 인터페이스 구현 → `isRewinding` 상태에서 회전 방향 반전 (되감기 대응)
  - `TimeManager`에 자동 등록/해제 처리 (`Start()`, `OnDisable()`)
  - 시계 에셋에 `PuzzleHand` 연결 완료
    - 거리 기반 퍼즐 상태 전환 로직을 `Update()` 내 임시 테스트용으로 구현

### 메모
- 보스 공격의 **정확한 타이밍/위치 판정**을 위해 히트박스를 따로 추적 방식으로 처리함
- 퍼즐 바늘은 Snapshot 기록 없이 회전 방향 반전만으로 간단하게 되감기 처리함
- 이후 퍼즐 목표 각도 정답 판정, 퍼즐 매니저와 FSM 연동 예정

---

## 2025.07.22 (화) 작업 기록

### 주요 작업

- **퍼즐 바늘 정답 판정 및 시계 애니메이션 제어 개선**
  - `PuzzleHand.cs`에 `IsAligned()` 함수 구현: targetAngle과의 z축 회전 차이 계산 → 허용 오차 이내 여부 판정
  - Animator 연동 제거: 전체 애니메이션 속도 제어는 `PuzzleClockManager`로 일원화

- **`PuzzleClockManager.cs` 기능 확장**
  - 퍼즐 전체 흐름 및 판정 로직 통합 관리
  - 현재 `TimeManager.TimeState`에 따라 Animator 속도 자동 갱신
  - 퍼즐 성공 조건 `IsPuzzleCleared()`: 바늘 3개가 정렬 상태이면서 시간 정지 중일 때
  - 퍼즐 성공 시 `OnPuzzleSuccess`, 실패 시 `OnPuzzleFail` 이벤트 발생
  - 성공/실패 후 TimeManager 초기화 및 GameState 전환 처리

- **퍼즐 FSM 상태 처리 구조 정리**
  - `PuzzlePhase1State.cs`에서 `PuzzleClockManager` 이벤트 구독
  - 퍼즐 시작 시 `GameManager.EnterPuzzle()` 호출
  - 성공 시 약점 노출, 실패 시 보스에게 clockParts 발사 후 Phase2 진입
  - `PuzzleSuccess()` / `PuzzleFail()`에 후속 연출/패널티 위치 TODO로 명시

- **보스 약점 노출 시스템 구현**
  - `BossController.cs`에 `ExposeWeakPoint(float duration, Action onComplete)` 함수 구현
    - Animator의 `"IsWeakExposed"` bool 파라미터 기반 약점 애니메이션 전환
    - 노출 종료 시 FSM Phase2 전이 콜백 연결
  - `WeaknessHitbox.cs`: 보스 약점용 IDamageable → BossController로 데미지 위임, 배율 적용

- **시계 부품 발사 및 충돌 처리 로직 구현**
  - `ClockPart.cs`에 `Launch()` 함수 구현: 랜덤 방향 + Force + 일정 시간 후 타겟 추적
  - `OnTriggerEnter()`에서 Player/Boss에 닿을 경우 도착 처리 + 데미지 적용
    - 중복 처리 방지를 위한 `hasArrived` 체크 도입
  - `PuzzleClockManager.cs` → `AreAllPartsArrived()` 검사 후 완료 콜백 처리
  - `BossController.cs`에서 `WaitClockPartsArrival(Action onComplete)` 코루틴 구현
  - `PuzzlePhase1State.cs`에서 성공/실패 결과에 따라 타겟 지정 후 부품 발사 및 Phase2 전환

### 메모

- 퍼즐 로직과 보스 FSM/게임 FSM 전이 처리를 깔끔하게 분리하여 확장성과 유지보수성 향상
- 약점 노출 방식은 스턴 상태가 아닌 별도 애니메이션 + 배율 데미지로 차별화
- ClockPart의 충돌 처리 안정성을 위해 `hasArrived` 체크는 반드시 필요
- 추후 FinalPuzzle에도 재활용 가능하도록 PuzzleClockManager와 ClockPart는 확장성 고려함

---

## 2025.07.23 (수) 작업 기록

### 주요 작업
- **지뢰 생성 패턴 구현 (DelayedMineState)**
  - 플레이어 주변에 지뢰 3개 생성 후 일정 시간 뒤 폭발
  - 애니메이션 이벤트를 통해 생성 타이밍 제어
  - 폭발 전 경고 애니메이션 실행 및 범위 내 데미지 처리

- **에너지 볼트 패턴 구현 (RapidEnergyShotState)**
  - 보스 기준 원형 위치에 에너지 볼트 3개 생성
  - 플레이어 방향으로 0.3초 간격 발사
  - EnergyBoltProjectile.cs에서 직선 이동 및 마비 효과 적용

- **플레이어 마비(Paralysis) 상태 구현**
  - 일정 시간 동안 입력 차단 (시점 회전 제외)
  - InputManager에서 마비 상태 처리 및 차단
  - 상태 해제는 자동 코루틴으로 처리
  - 추후 연출 추가(TODO) 주석 기입

- **히트박스 트리거 구조 개선**
  - TriggerAttackHitbox() 함수 개선
    - 각 FSM에서 개별 데미지를 전달하도록 구조 변경
  - duration과 damage를 분리하여 확장성 강화

- **보스 대시 상태(BossDashState) 구현**
  - 지정 방향으로 고속 이동 후 후속 공격 상태로 전이
  - DoubleSlashComboState에서 거리 조건 판단 → 대시 후 공격 처리
  - StaggerCheckState 초안 작성 (향후 연동 예정)

- **Phase2 패턴 결정 코루틴 구현**
  - DecideNextPatternPhase2(): Phase2 진입 시 랜덤으로 공격 패턴 선택
    - DoubleSlashCombo, DelayedMine, RapidEnergyShot, StaggerCheck 중 택1

- **패턴별 애니메이션 클립 제작 및 이벤트 등록**
  - DoubleSlashCombo: 두 번 베기 타이밍에 히트박스 이벤트
  - DelayedMine: 지뢰 생성 타이밍에 이벤트 등록
  - RapidEnergyShot: 3연속 투사체 발사 타이밍에 이벤트 등록

### 추가작업
- **보스 퍼즐 진입 조건 상태 통합 (CheckPhaseEndState)**
  - 기존 CheckPhase1EndState, CheckPhase2EndState 클래스를 하나의 CheckPhaseEndState로 리팩토링
  - 보스 페이즈(PhaseManager.CurrentPhase)에 따라 Puzzle1 또는 FinalPuzzle 상태로 분기
  - IdleState에서 페이즈별 공격 패턴 결정이 이미 분리되어 있어 상태 통합이 자연스러움

- **테스트용 프리팹 적용 및 전투 패턴 동작 검증**
  - Mine(지뢰) 프리팹: 간단한 메시 및 Collider 기반 폭발 테스트 완료
  - EnergyBolt 프리팹: 보스 기준 원형 생성 및 플레이어 방향 발사 테스트 완료
  - 향후 연출/이펙트 적용 전까지 기능 테스트용으로 유지

### 메모
- 보스 FSM 전반의 흐름이 정교화되어 실시간 패턴 전이가 자연스러움
- 마비/스턴 등 상태이상에 따른 연출은 추후 시각 효과 및 사운드 연동 필요
- 현재까지 FSM 흐름 및 공격 애니메이션 이벤트 연계는 매우 안정적이며 구조적 통일성 유지됨

---

## 2025.07.24 (목) 작업 기록

### 주요 작업

- **LeapSmashState 구현 (도약 후 내려찍기 공격)**
  - `LeapSmashState.cs` 추가: `BaseBossAttackState` 상속
  - 애니메이션 기반 도약 후 내려찍기 FSM 구성
  - 무적 처리 및 히트박스 생성 타이밍 제어 (0.05초 유지)
  - `BossController.TriggerLeapSmash()`에서 히트박스 발동

- **보스 패링 히트박스 분리 및 스턴 상태 연동**
  - `TriggerParryHitbox()`, `ParrySlashHitbox()`로 분리
  - 플레이어가 패링 성공 시 `StaggerCheckState`로 전이
  - `yield break`로 히트박스 중복 타격 방지

- **공격 FSM 전환 로직 정비**
  - `BaseBossAttackState.Exit()`에서 상태 전이용 코루틴 중지
  - `LeapSmashState`, `TimeStopAttackState` 등에서 `base.Exit()` 호출
  - `TimeStopAttackState` 구조 변경 → `BaseBossAttackState` 상속으로 패링 대응 가능화

- **수치/연출 조정**
  - `Mine` 딜레이, 범위 조정
  - `BossDashState` 회전/스톱 거리 수정 → 과도한 밀착 방지
  - `LeapSmashState` 히트박스 유지 시간 조정 (`0.05초`)
  - `Dash` 애니메이션 클립 추가 및 FSM 연결 (`PlayAnimation(string, bool)` 오버로드 사용)

- **에너지볼트 추적 로직 및 프리팹 연결**
  - `RapidEnergyShotState`에서 3발 중 첫 번째만 추적형
  - `SpawnEnergyBolt()`에 `isHoming` 인자 추가 → 추적 분기 처리
  - `EnergyBoltProjectile.cs`
    - `SetTarget()` 도입 → 플레이어 위치 유도
    - 일정 거리(`homingEndDistance`) 이내 접근 시 추적 해제
    - `target.GetComponent<Collider>().bounds.center.y`를 기준으로 수직 조준 정확도 개선
  - 프리팹에 스크립트 및 이펙트 연결 완료

### 메모

- 추적 해제 타이밍 덕분에 플레이어는 반응하거나 회피할 기회 확보 가능
- FSM 구조 전반이 일관되게 통합되어 추후 유지보수 및 확장 용이
- 연출적 예고(도약, 대시 방향 고정 등)가 잘 녹아들어 보스 패턴의 읽기성이 향상됨

---

## 날짜: 2025.07.25 (금) 작업 기록

### 주요 작업
- **CutsceneCameraManager 도입 및 퍼즐1 연출 흐름 구현**
  - `CutsceneCameraManager.cs` 추가
    - 퍼즐 진입 시 플레이어 시네머신 카메라 → 퍼즐용 시네머신 카메라 순차 활성화
    - 블렌딩 후 자동 비활성화 로직 구현
    - 메인카메라에 부착 (시네머신 브레인 포함)

- **카메라 구조 정리 및 배치**
  - 메인카메라: `CameraController`, `CinemachineBrain` 유지
  - 플레이어 시네머신 카메라: 플레이어 자식으로 배치 (퍼즐 연출 진입 시 시작 카메라)
  - 퍼즐 시네머신 카메라: 시계 오브젝트 자식으로 배치 (퍼즐 클로즈업용)

- **PuzzlePhase1State 카메라 연출 흐름 연결**
  - `StartPuzzle()` 진입 시 CutsceneCameraManager 통해 카메라 연출 시작
  - `EndPuzzle()` 시점에서 연출 종료 및 메인카메라 복귀
  - 시계 부품 발사/도착 로직 완료 후 Phase2 전환 처리

- **BossPhaseManager 개선**
  - `isPuzzle1Cleared` 플래그 추가 → 퍼즐1 클리어 이후 중복 진입 방지

### 메모
- **컷씬 전환 시 1프레임 딜레이 필수**
  - 시네머신 카메라 전환 시 `yield return null`을 통해 1프레임 쉰 다음 카메라를 활성화해야 정상 작동
  - 블렌딩 종료 후 카메라 비활성화도 `yield return new WaitUntil(() => !IsBlending())`으로 처리
  - 이 방식은 **모든 컷씬/연출 전환 시 공통 적용** 필요하므로 반드시 기억해둘 것

---

## 2025.07.26 (토) 작업 기록

### 주요 작업

- **보스 파이널 퍼즐 상태 구현 및 보스 상태/체력 제어 로직 개선**
  - `FinalPuzzleState.cs`:
    - 보스 FSM에 최종 상태 `FinalPuzzleState` 구현
    - 퍼즐 성공 시 `boss.SetHPWithPercent(0)` → 체력 0 처리 및 엔딩 상태 전이
    - 퍼즐 실패 시 `boss.SetHPWithPercent(20)` → 체력 20% 회복 및 Phase2 전이
  - `BossController.cs`:
    - `SetHPWithPercent(int percent)` 함수 추가
    - 체력 변경 시 자동으로 `BossPhaseManager.UpdatePhase()` 호출
  - `BossPhaseManager.cs`:
    - `UpdatePhase()` 조건 분기 로직 명확화
    - 체력과 퍼즐 클리어 여부 기반으로 보스 페이즈 자동 전이 구조 완성

- **파이널 퍼즐 순차 진행 및 퍼즐1/파이널 분기 로직 구현**
  - `PuzzleClockManager.cs`:
    - `BossPhase`에 따라 퍼즐1(동시 조작)과 파이널 퍼즐(순차 조작) 분기
    - 파이널 퍼즐 시작 시 `currentProgress = HandProgress.Hour` 초기화
    - 퍼즐 성공 판정 방식 분리 (동시 or 순차)
  - `PuzzleHand.cs`:
    - 파이널 퍼즐 중 현재 단계(HandProgress)에 해당하는 바늘만 조작 가능
    - 바늘별 각도 판정에 `finalPuzzleTargetAngle` 추가 도입

- **퍼즐 성공/실패 연출을 카메라 연출 이후로 순서 조정**
  - `FinalPuzzleState.cs` / `PuzzlePhase1State.cs`:
    - `CutsceneCameraManager.EndPuzzle()` 완료 후 퍼즐 성공/실패 처리 시작
    - 이후 clock 파츠 발사 및 연출 로직 순차적으로 실행되도록 변경
  - **연출 안정성 향상**
    - 카메라 블렌딩 완료 후 상태 전이 → 갑작스러운 시점 전환 방지

### 메모

- 퍼즐1은 **동시 정렬**, 파이널 퍼즐은 **시 → 분 → 초 순차 정렬** 구조로 확실히 분리됨
- 퍼즐 연출 타이밍(카메라 → clock 파츠 → 상태전이)이 명확하게 분리되어 안정적
- `CutsceneCameraManager` 구조 덕분에 이후 퍼즐 외 다른 컷씬 연출에도 동일 방식으로 재사용 가능

---

## 2025.07.30 (화) 작업 기록

### 주요 작업
- 최종장 맵 에셋 탐색 및 테스트 전용 프로젝트 구성
  - 새로운 프로젝트를 생성하여 다양한 맵 에셋을 비교 테스트
  - 시네머신, 전투 연출, 퍼즐 구성에 적합한 구조를 기준으로 최종 후보 선정
  - 현재 맵은 계단형 회랑 구조 + 중앙 건축물 배치가 인상적인 에셋으로 결정

- 라이팅 및 포스트프로세싱 설정 적용
  - Directional Light: 따뜻한 석양빛 (#FFD6A5), Intensity 2.5
  - Ambient Color: 연한 노란색 (#FFF0C8), Skybox 제거
  - Volume Profile 구성:
    - Bloom: 1.5, Exposure: 0, Tonemapping: ACES
    - White Balance: Temperature +30
    - SSAO: Renderer Feature에서 Intensity 1.2 설정

- Roughness 텍스처 → Smoothness 텍스처 변환 에디터 툴 구현
  - Tools > Texture Tools > Invert Roughness Map 메뉴 생성
  - 지정한 Roughness 텍스처를 반전시켜 PNG 저장
  - 저장 경로와 파일명 자동 지정, 덮어쓰기 방지 확인창 포함

- 맵 충돌 테스트
  - 구조물에 BoxCollider, MeshCollider 배치
    - 정적 구조물은 Convex 미사용
  - 플레이어 프리팹을 배치해 맵 전체를 직접 이동하며 충돌 및 시야 테스트 완료

### 메모
- URP 최신 버전에서는 Ambient Occlusion이 Volume Profile이 아닌 Renderer Feature에서 직접 조정됨
- Smoothness 적용 시 Roughness 텍스처는 반드시 반전한 후 사용해야 효과가 명확히 반영됨
- 최종장 맵은 보스전, 컷씬, 시간 퍼즐, 감정 연출 등 다양한 기능을 복합적으로 설계하기에 적절한 구조로 판단됨

---

## 2025.07.29 (화) 작업 기록

### 주요 작업
- **챕터1 맵 구성 및 테스트**
  - 연구소 맵 에셋 배치 완료
  - 플레이어 프리팹 배치 및 전 구역 콜라이더 수동 설정
  - 플레이어가 자유롭게 이동 가능한지 직접 플레이로 전 구간 테스트

- **라이팅 및 포스트프로세싱 설정**
  - Environment Reflection Source를 Skybox로 변경하여 어두운 톤 개선
  - Ambient Mode를 Gradient로 설정
    - Sky: `#B2C5D6`
    - Equator: `#96A7B8`
    - Ground: `#6A7B88`
  - Skybox 머테리얼 교체로 우주 느낌 강조
  - Directional Light 및 Environment Light 밝기 조정
  - 실수로 Baked 설정되어 있었던 조명 Realtime으로 전환하여 어두운 문제 해결

- **챕터별 맵 구성 기획 정리**
  - 최종 구성 확정: **연구소 → 미래 구역 → 신전 (보스)**
  - 각 챕터별 콘셉트, 분위기, 구조 및 배치 이유 정리 완료

### 메모
- 연구소 맵은 SF 실험실 분위기로 챕터1 튜토리얼 맵에 적합
- 포스트프로세싱을 통해 부드럽고 자연스러운 화면 연출 확보
- Lightmap Baking 없이도 기능적 구현 문제 없음 → 현재는 Realtime 조명만 사용
- 플레이어 시점에서 동선/시야/충돌 모두 이상 없음

---

## 날짜: 2025.08.04 (월) 작업 기록

### 주요 작업
- 보스 연출용 라이팅 및 포스트프로세싱 설정
  - Directional Light를 추가하여 어두운 보스 연출 개선
    - 기존 Spot Light 단독 사용 시 캐릭터 실루엣이 너무 어두워짐
    - 노란 계열 조명을 사용하고 Intensity를 2.5로 설정하여 시인성 확보
    - 그림자 비활성화(Shadow Type: No Shadows) 처리
  - 포스트 프로세싱 설정 변경
    - Color Adjustments: Post Exposure=1.8, Contrast=20, Hue Shift = -5, Saturation=0
    - White Balance: Temperature/Tint 모두 -10으로 설정하여 차가운 느낌 연출
    - Vignette 및 Bloom 값을 미세 조정하여 집중도 있는 분위기 형성
  - Directional Light 및 PostProcess Volume을 **보스 오브젝트의 자식으로 등록**
    - 보스 활성화 시 함께 켜지도록 구성
    - 전투 연출 시점에만 효과 적용되도록 제어

### 메모
- 챕터2 맵 구성 관련 고민
  - 기존에 3개 맵 구성(연구소, 미래구역, 신전)을 고려했으나 실제 배치와 설계 시간, 분위기 연출 효율 등을 종합적으로 판단하여 **2개 맵 구성으로 확정**
  - **Watcher, ChronoMonk**: 챕터1 맵(연구소)에서 주로 배치
  - **MirrorDuelist**: 챕터2 맵(신전)에서 제한적으로 등장
    - 신전은 따뜻한 조명과 건축양식으로 차분한 분위기이므로 MirrorDuelist는 **특별 이벤트**에서만 등장
    - 등장 시에는 기존 분위기를 **차갑고 전투적인 느낌으로 전환**하기 위해 포스트프로세싱 변경 처리

---

## 2025.08.05 (화) 작업 기록

### 주요 작업

- **트리거 기반 자동문 시스템 구현**
  - `DoorController.cs` 작성
    - `OnTriggerEnter/Exit`로 플레이어 접근 감지
    - Animator 트리거(`Open`, `Close`)를 통해 문 열림/닫힘 제어
    - `isOpen` 플래그로 중복 호출 방지
  - 문 오브젝트에 애니메이션 클립 적용 및 트리거 콜라이더 범위 조정

- **기계장치 키카드 퍼즐 시스템 구축**
  - `KeycardMover.cs` 작성
    - 일정 시간 간격으로 키카드가 `KeyCardPoints` 중 랜덤 위치로 이동
    - `rewindIndices`를 통해 과거 위치 저장 → 되감기 시 이전 위치로 되돌아감
    - `timeScale == 0`일 경우 `WaitUntil`로 처리해 시간 정지 대응
  - `PuzzleStateTrigger.cs`
    - 퍼즐방 입구에 트리거 배치
    - GameManager의 상태를 `PuzzleState`로 전환
    - 방 이탈 시 자동으로 `ExplorationState` 복귀 처리

- **CCTV 리와인드 및 탐지 연출 개선**
  - `CCTVPlayerDetector.cs` 수정
    - `isRewinding`일 때 애니메이터 `Speed` 파라미터를 -1로 설정 → 역재생 시도
    - `Start()`에서 `Invoke("StartAnimation", Random.Range(0, 5))`으로 CCTV 시작 타이밍 분산
    - 탐지 범위 표시용 콘(빨간색) 시각적 조정 완료

### 메모

- 퍼즐방 트리거 진입/퇴장 상태 전환은 GameManager와 구조적으로 안정적으로 연동됨
- 자동문 시스템은 각 문에 쉽게 적용 가능하도록 재사용성 확보됨

---

## 2025.08.06 (수) 작업 기록

### 주요 작업

- **CCTV 감지 시스템 고도화**
  - 감지 시 애니메이션 정지 (`animator.speed = 0`)
  - 감지 상태에서 플레이어 방향으로 회전 (수평 방향만)
    - `Slerp`를 사용한 자연스러운 회전 처리
  - 감지 해제 시 애니메이션 속도 복원
  - `TriggerAlarm()` 기반 와쳐 스폰 연동

- **와쳐 스폰 시스템 구현**
  - 일정 시간 이상 감지 시 문 개방 + 와쳐 소환
  - `EnemySpawner`에서 `spawnPoint` 기준 `spawnRange` 내 다수 소환
  - `spawnCooldown` 기반 반복 소환 방지

- **연구소 맵 연출 강화**
  - 실험실 수술대 3개 배치
    - 크로노몽크 2체 연출 배치
    - 플레이어 기상용 빈 수술대 구성
  - 튜브 안에 연출용 에너미 배치로 몰입도 강화

- **무기 획득 및 관리 시스템 구현**
  - `ItemData`에 `WeaponData` 연결 필드 추가
  - `ItemPickup`에서 장비 아이템 획득 시 `InventoryManager.RegisterWeapon()` 호출
  - 획득 무기만 장착 가능하도록 `WeaponManager`, `InputManager` 로직 개선
  - 피스톨은 기본 무기로 `Start()` 시 자동 획득 처리

- **키카드 도어 시스템 구현**
  - `KeycardDoor.cs`에서 `IInteractable` 구현
  - `Interact()` 시 해당 키카드 소지 여부 검사
  - 보유 시 `Animator.SetTrigger("Open")`로 문 열림 처리
  - 도어마다 `requiredKeycard` 지정 가능
  - 키카드 문 열림 애니메이션(슬라이딩 방식) 제작 및 연결 완료

### 메모

- CCTV 감지 시 화면 연출(빨간색 효과)은 폴리싱 단계에서 처리 예정
- 무기 획득 위치는 앞으로 챕터1 초반에 자연스럽게 배치할 계획
- 키카드 UI 표시, 획득 연출 등은 나중에 UI 정비 시 추가 가능

---

## 2025.08.07 (목) 작업 기록

### 주요 작업
- **파이널 챕터 최적화**
  - 일부 MeshCollider 삭제 및 BoxCollider로 대체하여 물리 연산 최적화
  - Convex 처리로 충돌 메시 단순화
  - 사용하지 않는 베이크된 라이팅 제거 및 LightingData 삭제
  - 맵 전체 Static으로 설정하여 조명/렌더링/내비메시 성능 향상
  - Skybox를 변경하여 시각적 완성도 향상

- **3번 퍼즐방 전용 NavMesh 설정 및 결정체 에이전트 적용**
  - PuzzleNavMeshSurface 오브젝트 구성
    - Agent Type: `PuzzleOrb`
    - Area: `PuzzleOnly`
    - Layer: `PuzzleRoom`
    - Collect Objects: Volume
  - Crystal_red 프리팹에 NavMeshAgent 추가 및 속성 조정
  - 퍼즐룸 관련 오브젝트 계층 정리

- **퍼즐룸3 PuzzleStateTrigger 적용 및 상태 전환 처리**
  - 챕터1에서 사용하던 트리거 프리팹 재활용
  - 퍼즐룸 진입 시 GameStateMachine이 `PuzzleState`로 전환되도록 구성
  - 퍼즐 상태에서 시간 스킬 사용 정상화

- **CrystalFollowPlayer 개선**
  - NavMeshAgent 속도 및 회전 속도에 timeScale 반영
  - 되감기(positionHistory 기반) 부드럽게 작동
  - 자식 파티클에도 simulationSpeed 반영
  - 되감기 후 positionHistory 유지

- **CrystalMover 리팩토링 및 이동/되감기 시스템 통합**
  - PingPong, Bounce 이동 로직을 하나의 구조로 통합
    - isBouncing 여부에 따라 방향 랜덤 offset 적용
  - TimeManager timeScale 반영
  - 일정 간격으로 위치 저장 및 MoveTowards 기반 되감기 구현
  - Crystal_blue에 Rigidbody, BoxCollider, CrystalMover 부착

- **시간 스킬 시 자식 파티클까지 일괄 적용**
  - `GetComponentsInChildren<ParticleSystem>()` 활용
  - 모든 자식 파티클의 simulationSpeed에 timeScale 반영

- **Crystal_red 데미지 및 넉백 기능 구현**
  - 플레이어 충돌 시 IDamageable 통해 데미지 적용
  - 충돌 방향 반대쪽으로 수평 넉백 (dir.y = 0)
  - MoveTowards 기반 넉백 코루틴 구현
  - 넉백 중 NavMeshAgent 일시 정지

- **상태이상 적용 및 결정체 배치**
  - CrystalMover 충돌 시 슬로우/프리즈 상태 적용
  - IStatusEffectable 인터페이스로 상태이상 전달
  - PlayerController 상태이상 중첩 방지 및 자동 회복 구조 개선
  - 결정체 배치:
    - 레드 1개
    - 블루 3개
    - 그린 2개

- **퍼즐룸 진입 시 문과 퍼즐 오브젝트 활성화 처리**
  - PuzzleStateTrigger에서 `Invoke(nameof(...), 0.5f)`로 문과 오브젝트 활성화
  - `GetComponentsInChildren(true)`로 비활성 오브젝트까지 포함 활성화

- **기타 개선 사항**
  - CrystalMover, CrystalFollowPlayer의 TimeManager 등록 위치를 Start → OnEnable/OnDisable로 변경
  - PlayerController에서 originalMoveSpeed 및 originalAnimSpeed를 Start에서 1회만 저장하도록 수정

### 메모
- 결정체 상태이상, 시간 되감기, 충돌 및 넉백 기능이 정상적으로 연동됨
- 퍼즐 난이도는 테스트를 거쳐 지속적으로 밸런싱 예정
- PuzzleStateTrigger 구조는 FinalPuzzle에도 재활용 가능

---

## 2025.08.08 (금) 작업 기록

### 주요 작업
- 퍼즐룸5 보초병 조각상 퍼즐 오브젝트 구현
  - Rigidbody 기반 파편 오브젝트 분해 구조 설계
  - Lerp 기반 복원 시스템 구축 (시간 정지 중 복원 정지 가능)
  - IRewindable, ITimeControllable 인터페이스 연동
  - 퍼즐룸 입장 시 자동 복원 시작 / 완료 후 충돌체 활성화 처리

- 석판 회전 퍼즐(PuzzlePlate) 시스템 구현
  - IInteractable 인터페이스 상호작용 구조 도입
  - 정답 방향(localRotation) 저장 후 무작위 회전
  - 90도 회전 기능 구현 및 부드러운 회전 연출 적용
  - 3x4 석판 배치 및 정답 맞추기 퍼즐 구성

- 퍼즐 매니저 공통 구조 정비
  - PuzzleRoomManager 추상 클래스 도입
    - isCleared 및 CheckPuzzle(), OnPuzzleSolved() 공통화
  - PuzzleRoom5Manager 구현
    - 퍼즐판 자동 수집 및 정답 일괄 체크 구조 확립
    - 정답 시 clearedPortal, clearedReward 오브젝트 활성화 처리
  - PuzzleStateTrigger에서 PuzzleRoomManager 상속 구조 연동

- TeleportPortal 기능 구현
  - 퍼즐 완료 시 생성되는 포탈을 통해 보스룸 전방으로 순간이동
  - 지정 위치 + 회전으로 이동
  - Collider 자동 트리거 설정 처리

### 추가 작업

- Metallic + Roughness 통합용 에디터 유틸리티 구현
  - `Tools/Texture Tools/Metallic Smoothness Combiner` 메뉴 추가
  - 메탈릭 텍스처(R)와 러프니스 텍스처를 선택하여 하나의 마스크(R + A)로 통합 저장
  - 저장 후 자동 임포트 설정 및 프로젝트 내 TextureType 적용 처리
  - 출력된 텍스처는 마테리얼의 MetallicSmoothness Map으로 바로 사용 가능

- 무기 전환 실패 시에도 조준이 취소되는 문제 해결
  - 다른 무기를 보유하지 않은 상태에서 무기 변경 키를 눌러도 조준이 유지되도록 개선

### 메모
- PuzzlePlate의 정답 체크는 현재 Update 기반으로 유지 (단순성 및 참조 최소화 목적)
- 퍼즐 클리어 → 리워드 획득 → 포탈 생성 → 보스룸 이동까지 흐름이 자연스럽게 연결됨
- 이후 리워드를 수집해 보스룸 앞 석판에 끼우는 구조로 확장 예정

---

## 2025.08.09 (토) 작업 기록

### 주요 작업
- 기존 라이팅 세팅 및 라이트맵 초기화
  - 불필요한 베이크 데이터 제거 후, 새로운 공통 라이팅 환경 구축
- 공통 Directional Light 프리팹 제작
  - 색상, 강도, 그림자, 캐스케이드 설정 적용
  - 맵 전역에 동일 프리팹 배치로 일관성 확보
- Global Post-process Volume(공통 프로파일) 제작
  - Tonemapping, Bloom, Contrast, Saturation, Vignette, White Balance 등 기본값 세팅
- Reflection Probe 전역 적용
  - Box Projection 활성화로 반사 왜곡 최소화
  - 중심부 배치 및 사이즈 여유 확보로 전체 커버리지 보장
- 상황별 볼륨 스냅샷 3종 제작
  - Exploration / Combat / TimeStop 각 프로파일 독립 생성
  - 상황별 색감, 대비, 포스트프로세싱 값 초기 설정
- VolumeSnapshotController 스크립트 구현 및 테스트
  - Snapshot enum 기반 전환 기능
  - SmoothStep 보간 + unscaledDeltaTime 사용
  - 디버그 입력(Alpha8/9/0)으로 전환 테스트 완료

### 메모
- Reflection Probe 베이크 시 중심 위치와 주변 환경에 따라 전반적인 색감이 크게 변함을 확인  
  → 톤 연출용 꼼수로 쓰일 수 있으나, 최종 라이팅 확정 후 적용하는 것이 안전
- Box Projection 적용 시 방 단위 프로브보다 전역 프로브 하나로도 충분한 품질 확보 가능
- Volume Profile 복사 후 개별 수정해야 서로 영향을 주지 않음 (공유 상태에서 수정하면 모든 볼륨이 변함)
- 포스트프로세싱은 프로브 베이크 결과를 기반으로 후처리하는 편이 자연스러움
- 디렉셔널 라이트와 Reflection Probe 범위가 겹칠 때 광원 톤과 반사 톤이 따로 노는 현상 주의

---

## 2025.08.10 (일) 작업 기록

### 주요 작업
- **챕터1(연구소) 안개 및 포스트프로세싱 변주 적용**
  - 안개(Fog) 활성화 및 거리 범위 설정 (색상: #B3CFEA, Start: 20, End: 60)
  - 포스트프로세싱 수치 조정
    - Post Exposure: -0.05
    - Saturation: -5
    - Color Filter: RGB(126, 156, 191)
    - White Balance Temperature: -5

- **챕터1(연구소) 글로벌 볼륨 스냅샷 구현**
  - 기존 Exploration 스냅샷을 챕터1 전용 스냅샷으로 대체
  - Combat, TimeStop 스냅샷에 챕터1 변주 값 반영
  - 전반적으로 어둡고 차가운 톤 유지로 연구소 분위기 강화

- **파이널 챕터(신전) 글로벌 볼륨 변주 및 스냅샷 3종 적용**
  - 기본(Global) 볼륨 색감 조정
    - 기존 프리팹 기반으로 톤 수정하여 챕터1과의 통일성 유지
    - 채도·화이트밸런스 조정으로 따뜻하고 몽환적인 분위기 구현
  - Combat 스냅샷
    - 콘트라스트 강화 및 채도 조정으로 전투 긴장감 연출
  - TimeStop 스냅샷
    - 채도 감소 및 밝기 조정으로 시간 정지 특유의 이질감 표현
  - VolumeSnapshotController를 통해 각 상황별 부드러운 전환 테스트 완료

- **보스전 전용 포스트프로세싱 및 조명 세팅**
  - 보스전 전용 포스트프로세싱 프로파일 적용
    - Contrast 조정으로 보스 집중도 향상
    - Bloom 조정으로 무기 및 장식 빛 반사 강조
    - Vignette로 화면 가장자리 암부 강화
  - 스팟라이트 세팅
    - Color Temperature로 뜨거운 톤 부여
    - Intensity·Angle 조정으로 보스 실루엣 부각

### 메모
- 신전 Reflection Probe는 시각적 차이가 미미하여 유지 여부 추후 판단 예정
- Occlusion Culling 및 LODGroup 적용은 프로젝트 최적화 단계에서 진행 예정

---

## 2025.08.11 (월) 작업 기록

### 주요 작업
- **수도꼭지 및 물통 상호작용 시스템 구현**
  - `WateringCan`
    - 물 수위에 따라 시각적으로 변하는 워터 메쉬 적용
    - 플레이어가 들고 있는 상태, 놓여 있는 상태, 수도꼭지·식물 근처 여부 등 상태 관리
    - 물 한 방울 단위의 수위 증감(AddDrop) 로직 구현 (가득 참/비었음 상태 검사 포함)
    - F키 상호작용 시:
      - 손에 없으면 플레이어 손 위치로 픽업
      - 손에 있고 수도꼭지 근처면 `OnPlaced` 이벤트 호출
      - 손에 있고 식물 근처면 `OnWatered` 이벤트 호출
      - 그 외엔 시작 위치로 되돌려 놓기
    - Rigidbody 활성/비활성 처리로 물리 충돌 관리
  - `WaterFaucet`
    - `WateringCan.OnPlaced` 이벤트 수신 후 지정 위치(`canPosition`)에 캔 장착
    - 애니메이션 이벤트(`AnimDrip`)로 물방울 떨어질 때마다 물통 수위 변화
    - 플레이어가 캔을 들고 수도꼭지 범위에 들어오면 `IsNearFaucet` 플래그 설정
    - `ITimeControllable`, `IRewindable` 구현으로 시간 가속/되감기 시 애니메이션 속도 및 물 수위 역변화 지원

- **4번방 물받기/물주기 퍼즐 기본 구현**
  - `WateringCan`
    - 식물 근처(`IsNearPlant`)에서 상호작용 시 `OnWatered` 이벤트 트리거
  - `GrowingPlant`
    - `WateringCan.OnWatered` 구독 → 가득 찬 캔일 때만 성장 시작
    - 애니메이터 트리거(`Growing`) 및 속도 제어(`SetTimeScale` / `StartRewind` / `StopRewind`) 연동
    - 트리거 범위 내에서 플레이어가 캔을 들고 있으면 `IsNearPlant` 플래그 갱신

### 메모
- 물주기 로직은 현재 한 번만 동작하도록 `isWaterd` 플래그로 중복 방지
- 향후 성장 애니메이션 속도는 `timeScale`을 반영하도록 조정 가능

---

## 2025.08.12 (화) 작업 기록

### 주요 작업
- 물 주는 파티클 이펙트 구현
  - WaterPourVFX 프리팹 제작 (Cone Shape 기반 연속 물줄기)
  - Stretched Billboard + Soft Particle 적용
  - Color over Lifetime, Noise, Collision 모듈 활용
  - Simulation Space: World, 빠른 재생 속도 대응 구조 설계

- 물 주는 전체 시스템 구현 및 연출 개선
  - 쥐는 애니메이션 / 물 붓는 애니메이션 클립 제작 및 연동
  - 손에 들었을 때 위치 오프셋 적용
  - Animator 파라미터(IsPour)로 애니메이션 제어
  - PlayPourAnimation()에서 파티클 재생 및 자연스러운 물 수위 감소
  - Interact() 상호작용으로 식물에 물 주기 가능
  - DropToStart() 함수로 손에 들지 않을 때 자동 위치 복귀 처리

- 4번 퍼즐방 힌트 벽화 설치
  - 벽화 3종(물방울/새싹/꽃) 배치하여 퍼즐 구조 직관적으로 유도
  - 신전 벽과 자연스럽게 어울리는 붉은 갈색 석재 질감 사용

- 4번 퍼즐방 환경 디테일 강화
  - 말라있는 나무 화분 4개 배치 (퍼즐 대상 오브젝트)
  - 벽면에 덩굴, 담쟁이, 고사 식물 등 다양한 벽 장식 추가
  - 식물의 생장/고사를 시각적으로 표현해 퍼즐 분위기 보완
  - 전반적인 공간 밀도 향상 및 몰입감 개선

### 메모
- 물 주는 연출이 퍼즐의 힌트 이미지와 유기적으로 연결되도록 구성
- 공간이 휑하게 보이지 않도록, 천장/바닥/벽 모두에 오브젝트 분산 배치

---

## 2025.08.13 (수) 작업 기록

### 주요 작업
- 6번방 조각상 미로 퍼즐 기획 및 구조 설계
  - 시간 조작 능력을 활용한 **전략 퍼즐**로 설계
  - 플레이어는 미로 안에서 순찰하는 조각상들을 피하며 탈출해야 함
  - 조각상은 일정 경로를 순찰하며, 닿을 경우 플레이어는 페널티를 받음 (ex. 초기화)
  - 조각상은 시간 정지, 슬로우, 되감기, 빨리감기 능력의 영향을 받아 경로를 뚫을 수 있도록 구성
  - 플레이어는 특정 조각상에 빙의하여 전용 구간을 통과하거나 스위치를 조작 가능
  - 빙의 해제 지점은 퍼즐 흐름에 맞춰 미리 지정된 안전지대에서만 가능
  - 2층에서 전체 미로 구조와 조각상 경로를 관찰할 수 있도록 설계하여 **관찰 → 계획 → 실행** 구조 유도

- 미로 구조 및 텍스처 작업
  - 미로 규모 조정 및 동선 확보
  - 벽 높이, 통로 폭 등을 플레이어 시야와 난이도에 맞게 조정
  - 텍스처 및 머티리얼 대비 강화로 시각적 가독성 개선

- 2층 시네머신 관찰 카메라 구조 세팅
  - 퍼즐 시작 전 미로 전체를 관찰할 수 있도록 상단 뷰 시네머신 카메라 배치
  - 향후 컷씬 전환 연출을 위한 구조 기반 마련

- 조각상 이동 시스템 (MazeStatue.cs) 구현
  - NavMeshAgent 기반 이동 구조 폐기
  - 한 칸씩 이동하는 MazeStatue 시스템 직접 구현
    - PingPong 방식으로 웨이포인트 리스트 왕복
    - 회전 방향은 이동 벡터를 기준으로 90/180/270도 자동 계산
    - 코루틴 기반으로 회전 → 이동 → 대기 순서로 자연스럽게 전환
  - 이동 속도, 간격은 moveSpeed, moveInterval로 조절 가능

- 조각상 웨이포인트 구성
  - 각 조각상 별로 커스터마이징된 경로 설정
  - 경로 밀도는 플레이어의 시간 능력 활용 타이밍을 고려해 최적화
  - 현재 총 3개의 조각상 경로 설정 및 테스트 완료

### 메모
- 한 칸씩 이동하는 구조는 시간 능력 적용과 시각적 연출을 위한 최적 해법
- 빙의 조각상 또한 동일한 방식으로 이동 시스템을 공유할 수 있음

---

## 2025.08.16 (토) 작업 기록

### 주요 작업
- Waypoint 자동 생성 에디터툴 구현
  - `WaypointLineGenerator.cs` 에디터툴 생성
    - 시작점과 끝점 지정 후 원하는 개수만큼 웨이포인트 자동 생성
    - 생성된 웨이포인트는 Maze 오브젝트의 자식으로 배치됨
    - 반복적인 포지션 작업을 간편화

- 플레이어 이동 경로 웨이포인트 배치
  - 벽을 넘지 않도록 전체 미로 경로에 수동 배치
  - 플레이어 기준으로 거리 조정하여 자연스러운 이동 경로 구성

- 조각상 전용 웨이포인트 설정
  - 각 미로 조각상에 대해 독립 경로 리스트 작성
  - Inspector에서 순서대로 웨이포인트 연결 가능하게 설정

- 웨이포인트 y축 정규화
  - 모든 웨이포인트와 조각상 이동 위치의 y값 통일
  - 이동 시 발생하던 바닥 뚫림 문제 해결

- 플레이어 미로 조작 시스템 (`PlayerMazeController.cs`) 구현
  - W/A/S/D로 한 칸씩 입력 이동 (로컬 기준 방향 반영)
  - LeftControl 입력 시 90도씩 부드러운 회전 (SmoothRotate 코루틴)
  - 이동 시 Raycast로 이동 가능 위치(`waypointLayer`) 탐색
    - 벽(`wallLayer`) 감지 및 충돌 방지 포함
  - `startPosition` 기준으로 위치 초기화 가능

- 충돌 시 위치 초기화 로직 구현 (`MazeStatue.cs`, `PlayerMazeController.cs`)
  - OnTriggerEnter에서 `Reset()` 호출
    - 모든 코루틴 정지
    - `isMoving`, `isRotating` 플래그 초기화
    - 시작 위치와 회전으로 복귀
  - 초기화 이후 다시 정상 조작 가능하게 상태 정리 완료

### 메모
- 이동이 너무 자연스럽지 않으면 입력 딜레이 혹은 타이밍 조절도 고려할 수 있음
- 추후 시네머신 카메라 연동 여부는 별도 테스트 후 결정
- 회전 시 사라지는 방향 혼동 방지를 위해 카메라 각도 유지 or UI 요소 고려 가능

---

## 날짜: 2025.08.17 (일) 작업 기록

### 주요 작업
- MazeStatue.cs 시간 스킬 전체 구현
  - `ITimeControllable`, `IRewindable` 인터페이스 구현
  - 시간 정지/슬로우/빨리감기/되감기 모두 통합 대응
    - `moveSpeed`, `moveInterval`에 `timeScale` 적용
    - 정지 상태일 경우 이동 루틴 일시 중단
    - 되감기 시 `direction` 반전 및 `currentIndex` 역순 추적
    - 이동 중 되감기 전환 시도 시, 즉시 반응하도록 Lerp 보간 방식 수정
  - 회전은 되감기 중에는 생략하여 자연스럽고 즉각적인 방향 반전 처리
  - 되감기 시작/종료 시 이동 루틴 중단 후 재시작 처리 (`RestartRoutine`)

### 메모
- 되감기 중에도 부드럽게 이동되도록 `MoveToNextPoint` 내 Lerp 개선
- timeScale이 0일 때(정지) 모든 루틴 일시 정지 → 퍼즐 연출 다양성 확보
- 각 조각상마다 독립 루틴으로 작동 → 개별 시간 스킬 연동 가능

---

## 2025.08.18 (월) 작업 기록

### 주요 작업
- 컷씬 카메라 시스템 리팩토링
  - `StartPuzzle()`, `EndPuzzle()` 제거
  - `StartCutscene(GameObject)`, `EndCutscene(GameObject, Action)` 범용 함수로 통합
  - PlayerCinemachine → 대상 컷씬 카메라 순으로 자연스러운 블렌딩 처리 구현

- 퍼즐6 빙의 시스템 구현
  - `PossessionPortal.cs` 추가: F키 상호작용 기반 빙의 전환 처리
  - `PlayerMazeController.cs`: `SetPossessed(bool)` 구현 및 카메라/입력 제어 분기 추가
  - `CutsceneCameraManager`와 연동하여 자연스러운 시야 전환 구현

- 입력 제어 시스템 개선
  - `InputManager.cs`에서 빙의 상태일 경우 상호작용(F키)만 허용
  - 입력 처리 흐름을 `IsPossessed > IsFrozen > IsParalyzed` 순으로 재정렬
  - 조각상 회전 입력을 마우스 클릭(좌/우) 기반으로 변경

- 빙의 해제 시스템 구현
  - `UnpossessPortal.cs`: 조각상 트리거 진입 감지 및 해제 조건 전달
  - `PossessionPortal.cs`: 빙의 상태일 경우 F키 입력 시 해제 처리
  - 양방향 빙의-해제 흐름을 트리거 + 인터랙션 기반으로 구현

- 퍼즐6 매니저 구현 (`PuzzleRoom6Manager.cs`)
  - 조각상이 endPoint 근처에 도달했는지 감지하여 퍼즐 해금 처리
  - `clearedPortal`, `clearedReward` 오브젝트 활성화
  - 기존 `PuzzleRoomManager` 구조와 동일한 방식으로 구현하여 일관성 유지

### 메모
- 컷씬/카메라/입력 구조가 일관적으로 정비되어 퍼즐6 외 다른 퍼즐이나 컷씬 연출에 재활용 가능
- 빙의 상태 시 플레이어 입력 차단을 InputManager 중심으로 처리해 확장성 확보
- 조각상 태그/레이어는 추후 폴리싱 단계에서 정리할 예정
- 퍼즐 해금 조건을 퍼즐매니저 쪽에서 전담하도록 분리해 유지보수에 유리한 구조 완성

---

## 2025.08.19 (화) 작업 기록

### 주요 작업
- PuzzleProgressManager 구현 및 퍼즐 해금 체인 설계
  - 중앙 퍼즐 진행 매니저(PuzzleProgressManager) 신규 구현
  - 이벤트 시스템(OnRoomUnlocked / OnRoomCleared / OnKeyInserted) 구축
  - 퍼즐 해금 체인: 3번 → 5번 → 4번 → 6번
  - 초기 해금 방: 3번
  - Dictionary 기반 unlockMap으로 해금 순서 간결화
  - IsCleared, GetKeyCount 함수로 클리어 및 키 삽입 여부 체크 가능

- PuzzleGate 컴포넌트 구현 및 각 퍼즐방 도어에 연동
  - 각 퍼즐 도어에 PuzzleGate 부착
  - PuzzleProgressManager의 해금 이벤트 구독하여 해금 시 자동 비활성화 처리
  - Start 시 초기 해금 상태 동기화 적용

- BossKeyPickup 구현 (보스 키 상호작용 오브젝트)
  - IInteractable 구현, F키 상호작용 가능
  - 플레이어 손 위치(HeldPosition)에 부착/해제 가능
  - 들고 있을 때 isKinematic 활성화, 콜라이더 비활성화
  - InsertToSocket() 구현: 소켓에 삽입 시 상태 변경 및 비주얼 처리
  - insert 델리게이트 이벤트 구조로 삽입 요청 전달

- BossAltar 구현 (보스 키 소켓 구조)
  - OnTriggerStay로 플레이어가 들고 있는 BossKeyPickup의 CanInsert 설정
  - InsertKey()로 유효한 소켓에 키 삽입 처리
    - childCount 기준 중복 방지
    - 삽입 시 inserted 카운트 증가
    - PuzzleProgressManager.ReportKeyInserted(inserted) 호출
    - 모든 슬롯 삽입 완료 시 bossGateToOpen 오브젝트 활성화

- 구조 개선 및 방어 코드 추가
  - BossKeyPickup의 중복 삽입 방지
  - BossAltar의 슬롯 인덱스 유효성 및 상태 점검
  - 이벤트 해제 시점 명확화(OnDisable 등 정리)

### 메모
- 향후 보스 키 삽입 연출(회전 구조물, 빛 이펙트 등)은 에셋 확보 이후 폴리싱 단계에서 구현 예정
- 퍼즐 실패 → 부활 처리, PlayerPrefs 저장 로직 등은 UI 작업 이후 진행
- 보스방 개방 컷씬은 폴리싱 단계로 이관 예정

---

## 2025.08.20 (수) 작업 기록

### 주요 작업
- 퍼즐 진행도 연동 및 보스 제단 개선
  - PuzzleProgressManager에 OnAllCleared 이벤트 및 중복 방지 플래그 추가
  - 보스 제단(BossAltar)에서 socket 활성화 방식으로 전환
  - BossKeyPickup → ActivateSocket() 구조로 변경, 삽입 후 self 파괴
  - key 삽입 → manager에 ReportKeyInserted(total, max) 보고
  - 각 클래스 널 가드, 상태 초기화 처리 보강

- 퍼즐룸 3 클리어 조건 구현
  - PuzzleRoom3Manager: 플레이어가 특정 위치에 도달하면 클리어 처리
  - Physics.OverlapSphere로 영역 감지, layerMask 기반 필터링
  - 클리어 시 포탈/보상 오브젝트 자동 활성화

- 퍼즐룸 매니저 공통 처리 구조 개선
  - PuzzleRoomManager: OnPuzzleSolved 함수 내부 공통화
  - roomId 기반으로 PuzzleProgressManager에 클리어 보고
  - clearedPortal, clearedReward 자동 처리 → 자식 클래스에서 중복 제거

- 퍼즐 전체 연동 테스트 및 마무리 작업
  - PuzzleRoom4Manager, PuzzleRoom6Manager 구현 마무리
    - GrowingPlant 성장 완료 시 퍼즐 클리어 처리
    - 퍼즐6은 빙의 해제(controller.SetPossessed(false)) 포함
  - PossessionPortal: OnDisable로 상태 초기화 대응
  - PuzzleStateTrigger: 퍼즐 진입 시 플레이어를 앞으로 밀기 로직 추가
  - TeleportPortal: CharacterController 일시 비활성화로 순간이동 오류 해결
  - PuzzleRoom3Manager: FillUpHP()로 입장 시 체력 회복

### 메모
- PuzzleProgressManager의 디버깅 편의를 위해 상태 갱신 함수 및 디버그 변수 추가함
- 각 퍼즐 클리어 조건은 추상 메서드 대신 공통 구현 구조로 변경하여 관리 편의성 확보
- 보스 퍼즐 파트는 key 삽입 → 소켓 활성화 → 최종 조합으로 단순화 및 시연 최적화됨
- 퍼즐 전체 흐름은 완전하게 구성 및 테스트 완료
- 연출적인 요소는 향후 폴리싱 단계에서 진행

---

## 2025.08.21 (목) 작업 기록

### 주요 작업
- 폰트 에셋 추가 및 TMP 기본 설정 완료
  - HUD용 텍스트 폰트 에셋(SDF) 생성 및 연결
- BarWidget 프리팹 제작 및 슬라이더 기반 게이지 구조 구현
  - Slider + Fill 방식으로 HP/STA/Time 공용 게이지 구조 구성
  - Fill 색상 차이만으로 Variant 생성 가능하도록 설계
- PlayerHUD 구현 및 UIManager 연동
  - HP/STA/Time 슬라이더 값 연동 처리
  - 저체력 시 깜빡임 처리 (LowHpBlink Coroutine)
  - Gold/Ammo 텍스트 연결 및 표시 처리
- HUD 배치 및 레이아웃 구성
  - TopLeft 패널: HP/STA/Time BarWidget 배치
  - TopRight 패널: AmmoPanel, GoldPanel 배치
  - 각 패널 Anchor 및 Position 통일
- WeaponManager → UIManager 연동
  - 무기 장착/해제 시 AmmoPanel 자동 활성화/비활성화 처리
  - 총기류 장착 시만 탄약 패널 노출되도록 구현
- PlayerManager → UIManager → PlayerHUD 흐름 테스트 완료
  - HP/STA/Gold 값 반영 정상 확인
  - MP는 시간 자원 로직 미연결로 테스트 보류

### 메모
- HUD UI 전반은 디자인 적용 없이 기능 우선 구현
- MP(Time) 바는 구조는 완성되어 있으나 TimeManager 연동 필요
- 크로스헤어 및 퀵슬롯 UI는 마무리 단계에서 통일성 고려해 적용 예정
- TMP 폰트는 Pretendard 기반으로 작업, 한글 포함 전용 폰트로 나중에 교체 가능
- 모든 바(HP/STA/Time)는 BarWidget 하나로 통일해 관리되며, 재사용성 높음

---

## 날짜: 2025.08.22 (금) 작업 기록

### 주요 작업
- **시간 스킬 MP 소모 및 HUD 연동**
  - TimeManager에 초당 MP 소모량 설정 (rewind, stop, slow, fastForward)
  - MP 고갈 시 시간 스킬 자동 종료 및 상태 Normal로 전환
  - PlayerManager의 UseMP() 재활용, HUD MP바 실시간 갱신

- **시간 스킬 아이콘 표시 및 깜빡임 구현**
  - PlayerHUD에서 시간 상태에 따른 아이콘 표시/숨김 처리
    - 활성 상태는 알파값 1 + 깜빡임(Blink), 나머지는 알파 0
  - TimeManager에서 시간 상태 변경 시 UIManager를 통해 HUD 동기화

- **시간 스킬 HUD 레이아웃 정비**
  - TopRight_Panel 내부에 Time_Panel 구성
  - 아이콘은 겹쳐 배치되도록 정렬, 레이아웃 컴포넌트 제거
  - 위치 고정: TopRight 기준 Anchor / Time_Panel Pos = (0,0)

- **무기 아이콘 자동 캡처 및 전용 스크린샷 씬 구성**
  - WeaponIconCapture 스크립트 구현 (512x512, 투명 PNG 캡처)
  - ScreenshotScene 구성: 전용 배경, 조명, 무기 프리팹 배치
  - 검, 권총, 소총, 샷건 아이콘 각각 캡처 완료

- **무기 아이콘 HUD 연동**
  - 무기 장착 시 weaponData.iconSprite → UIManager 전달
  - PlayerHUD.SetWeaponImage()로 이미지 자동 갱신
  - 근접 무기(Sword)일 경우 탄약 패널 자동 숨김 처리

- **크로스헤어 시스템 구현**
  - 무기 장착 시 크로스헤어 활성화, 해제 시 비활성화
  - 무기 타입별 크로스헤어 이미지 자동 변경
  - 발사 시 크로스헤어 일시 확대, 줌인 시 축소
  - 사정거리 내 적 조준 시 크로스헤어 색상 빨간색으로 전환

- **WeaponHolder 카메라 자식으로 이동**
  - 사격 정확도 확보를 위해 WeaponHolder를 MainCamera 자식으로 배치
  - FirePoint를 카메라 중심 기준으로 전환하여 탄착 정확도 개선

### 메모
- 시간 스킬의 연출과 조작 피드백이 HUD를 통해 자연스럽게 전달되도록 구현
- 무기 아이콘 및 크로스헤어 시각적 일관성 확보 완료
- 총기 사격 방향과 HUD 크로스헤어가 완전히 일치하여 조준 신뢰도 향상
- 조준 시 무기 이동 연출은 유지하여 몰입감 유지

---

## 2025.08.25 (월) 작업 기록

### 주요 작업
- 상호작용 프롬프트 시스템 완성
  - InteractionHandler에서 주기적 탐색(0.2초 간격)으로 IInteractable 인식
  - 상황별 프롬프트 문구 제공 (`GetPrompt()` 분기 로직 구현)
  - UIManager → PlayerHUD 연동 구조 구성
  - PlayerHUD.ShowPrompt() 기반으로 UI 표시

- 다양한 IInteractable 오브젝트에 프롬프트 적용
  - KeycardDoor: 키카드 보유 여부에 따른 문구 표시
  - ItemPickup: 아이템 이름 기반 획득 문구
  - Shop: 상점 열기
  - BossKeyPickup: 들기 / 삽입 분기 대응
  - PossessionPortal: 빙의 / 되돌아가기 조건별 문구 처리
  - PuzzlePlate: 회전하기
  - WateringCan: 들기 / 채우기 / 물주기 상태에 따라 문구 변경

- 프롬프트 상태 갱신 개선
  - InteractionHandler에서 currentTarget가 동일해도 매번 `GetPrompt()` 호출
  - PossessionPortal 등 상태 변화 시 문구 즉시 반영되도록 수정
- 시간 UI 초기화 처리
  - 퍼즐 종료 또는 탐험 상태 진입 시 UIManager.ClearTimeState() 호출
  - 시간 아이콘 및 깜빡임 상태 완전 리셋

- 토스트 메시지 시스템 기본 틀 구현
  - ToastPrefab 구성 (Text + Background + Layout 조절 + Fade 애니메이션)
  - ToastController에서 Show(message) 호출로 토스트 출력
  - LayoutRebuilder 적용으로 정렬 문제 해결
  - 코루틴 기반 FadeIn → 유지 → FadeOut → 자동 제거 구조 구성

- ConfirmModal UI 구현 및 중계 구조 구성
  - ConfirmModalUI: 제목/내용/확인/취소 처리 및 콜백 등록 구조 구현
  - UIManager.ShowConfirm(title, msg, onConfirm, onCancel) 중계 방식으로 호출 통일
  - ToastUI, PlayerHUD와 동일한 UIManager 통합 방식 유지

- ConfirmModal 커서 락 제어 및 입력 차단 연동
  - ConfirmModalUI.Show() → 커서 락 해제 및 커서 표시
  - Hide() → 락 복구 및 커서 숨김
  - InputManager: Cursor.lockState != Locked일 경우 Update 입력 차단 처리

### 메모
- InteractionHandler의 프롬프트 시스템은 HUD 연동 상태만 남겨둔 상태
- ConfirmModal은 추후 ESC 닫기, 애니메이션 처리 등 개선 여지 있음

---

## 2025.08.26 (화) 작업 기록

### 주요 작업
- **보스 HP 패널 및 Overlay 캔버스 구조 개선**
  - HUD_Canvas 하위에 `TopCenter_Panel` 추가 후 `BossHP_Panel` 구성
    - `BossNameText`, `BossHP_Bar` 하위 요소 배치
    - 플레이어 HUD와 일관된 구조로 보스 HUD 관리 가능
  - Overlay_Canvas 신설 → ConfirmModal, Pause, Option 등 오버레이 UI를 통합 관리

- **BossHUD 구현 및 UIManager 리팩토링**
  - BossHUD
    - CanvasGroup 의존성 보장 및 중복 방지 속성 추가
    - Show(cur,max), Hide() 단순화 및 게이지 갱신 안전화
  - UIManager
    - 참조 직렬화(PlayerHUD, BossHUD, Toast, ConfirmModal 등)
    - 메서드 전반을 람다식/Null 전파 연산자로 간결화

- **보스 HP HUD 연동 및 표시/숨김 로직 적용**
  - BossController에서 UIManager 연동
    - ShowBossHUD / UpdateBossHUD / HideBossHUD 호출로 일원화
  - 전투 흐름에 맞춰 표시/숨김 타이밍 정리
    - Intro 진입 → HUD 표시
    - Puzzle 구간 진입 → HUD 숨김
    - Puzzle 종료 → HUD 재표시
    - Ending 상태 → HUD 숨김
  - PhaseManager UpdatePhase 호출 위치 정리 → Update()에서 제거, 이벤트 지점에서만 호출

- **슬로우존 파괴 시 버그 수정**
  - 파괴될 때 플레이어 슬로우 상태가 해제되지 않는 문제 해결
  - 내부 진입 대상 추적 및 OnDisable 시 일괄 상태 해제 보장
  - OnTriggerStay 의존 제거로 안정성 강화

- **일시정지 시스템 및 UI 통합 구현**
  - PauseUI
    - Resume / Options / Quit 버튼 구성
    - CanvasGroup 기반 Show/Hide 처리
  - UIManager
    - PauseUI 관련 함수 (ShowPause, ClosePause) 추가
    - OverlayBackground Show/Hide 함수 추가
  - ConfirmModalUI에 OverlayBackground 처리 통합 적용
  - InputManager
    - Pause 입력(Esc) 최상단에서 처리
    - TriggerPause() 함수 추가
  - PausedState
    - Enter: UIManager.ShowPause(), Time.timeScale = 0
    - Exit: UIManager.ClosePause(), Time.timeScale = 1

- **옵션 UI 레이아웃 구성**
  - AudioGroup
    - "오디오 설정" 라벨 + Master/BGM/SFX 슬라이더 구성
  - ControlGroup
    - "컨트롤 설정" 라벨 + 마우스 감도 슬라이더 구성
  - ScreenGroup
    - "스크린 설정" 라벨 + 해상도 드롭다운, 전체화면 토글 구성
  - SaveGroup
    - "저장" 탭에 저장하기/불러오기 버튼 배치
    - 현재는 틀만 구성, 기능은 추후 구현 예정
  - 탭 전환 버튼(오디오/컨트롤/스크린/저장) 추가 → 선택 시 해당 그룹만 표시되도록 설계

### 메모
- 보스 HUD/페이즈 로직과 UI 연동이 안정화됨
- OverlayCanvas 신설로 Pause/ConfirmModal/Option 같은 오버레이 UI의 관리가 단순화됨
- Pause 상태는 다른 모든 게임 입력보다 우선되도록 설계 → TimeState와 충돌 없이 동작 보장
- 옵션 UI는 오디오/컨트롤/스크린/저장 4개 그룹 틀을 완성, 기능은 추후 구현 예정

---

## 2025.08.27 (수) 작업 기록

### 주요 작업
- **InventoryShop 레이아웃 구축**
  - 기존 `Shop_Canvas` → `InventoryShop_Canvas`로 리네임
  - Canvas Scaler 설정: Screen Space Overlay, 1920×1080 기준, Match 0.5
  - Body 컨테이너에 Horizontal Layout Group 적용
    - Padding 16, Spacing 16
    - 인벤토리 패널 960 / 상점 패널 720 (Preferred Width)으로 비율 조정
  - 좌측 인벤토리 패널
    - Header(Height 56) 구성: Title("인벤토리") 좌측 정렬
    - 골드 표시 그룹(GoldGroup: 아이콘 24×24 + GoldText) 우측 고정
    - InventoryGrid: Stretch + Offsets(Left 16 / Right 16 / Top 56 / Bottom 16)
    - Grid Layout Group: CellSize 160×160, Spacing 16, FixedColumn 5
  - 우측 상점 패널
    - Header(Height 56): Title("상점")
    - ShopGrid: CellSize 160×160, Spacing 16, FixedColumn 3
  - 중앙 Divider(Width 2, Alpha ~0.3) 추가로 시각적 구분
  - 인벤토리 단독 모드 대응: ShopPanel_Right 비활성화 시 레이아웃 자동 리플로우

- **슬롯 프리팹 제작 및 적용**
  - `InventorySlot` 프리팹
    - 슬롯 크기 160×160
    - 구성: Icon(96×96), NameText, CountBadge(xN), SelectionFrame
    - CountBadge는 보유 수량에 따라 활성/비활성
  - `ShopSlot` 프리팹
    - 슬롯 크기 160×160, 상점 Grid 전용
    - 구성: Icon, NameText, PriceText, SelectionFrame
  - InventoryGrid와 ShopGrid에 프리팹 배치 테스트 완료
    - 인벤토리: 5열 배치, 상점: 3열 배치
    - 스크롤 지원 정상 동작 확인

### 메모
- 인벤토리는 그리드, 상점은 현재는 그리드로 두었으나 아이템 설명이 필요하다면 리스트 + 상세창 구조로 전환 고려 가능
- 골드 표시를 인벤토리 헤더에 배치하여 인벤토리 단독 모드에서도 정보 완결성 확보
- 다음 단계: Confirm/Toast 구매 플로우 연동, 이벤트 기반 인벤토리/골드 동기화

---

## 2025.08.28 (목) 작업 기록

### 주요 작업
- InputManager 입력 차단 구조 개선
  - HandleUIBlockingInput() 함수 구현
    - PauseUI: ESC만 허용
    - InventoryUI: ESC, I만 허용
    - ShopUI: ESC만 허용
  - ESC/I 핫키는 UI 비활성 시에도 처리되도록 분리 처리
- UIManager 입력 연동 개선
  - UI 상태 확인용 프로퍼티 추가 (IsPauseOpen, IsInventoryOpen, IsShopOpen, IsAnyUIOpen)
  - ToggleInventoryUI()에서 상점 열림 중일 경우 I키 입력 무시
- InventoryUI / ShopUI 구조 개선
  - OnEnable / OnDisable에서 InputManager.OnPause 등록 및 해제
  - OnEnable 시 커서 LockState = None, OnDisable 시 LockState = Locked
  - 기존 InputManager.TriggerPause() 호출 제거
- ItemDetailPanel UI 신규 구성
  - ShopPanel_Right 구조 개편 (좌측: 스크롤뷰, 우측: 디테일 패널 고정)
  - 디테일 구성 요소:
    - 아이콘 배경, 이름 텍스트, 타입 태그, 설명 텍스트, 효과 텍스트, 가격 표시, 버튼 그룹 등
  - TextMeshPro의 멀티라인 및 워드랩 설정 적용
- 상점 UI 동작 완성
  - 슬롯 선택 시 ItemDetailPanel 연동
  - Buy / Sell 버튼 클릭 시 ShopManager 연동 처리 완료
- 인벤토리 시스템 리팩토링
  - 아이템 관리 기준을 itemName → itemID로 전면 전환
  - InventoryManager, ItemDatabase, ItemManager, InventoryUI 전반 수정
  - 중복 ID 경고 및 Dictionary 수동 초기화 방식 도입
  - 아이템 장착 전용 메서드 (Equip/Unequip) 정리

### 메모
- ESC 키 기반 UI 닫기 흐름이 명확하게 정리되어 UX 개선
- ItemDetailPanel 기반의 아이템 정보 제공 및 연동 로직 완성
- itemID 기반 구조로 아이템 시스템의 안정성과 확장성 향상됨

---

## 2025.08.29 (금) 작업 기록

### 주요 작업
- 무기 시스템 구조 통합 및 ItemData 연동
  - WeaponData → ItemData 기반으로 무기 정의 및 장착 연동 구조 일원화
  - InventoryManager에서 무기 등록 및 소유 체크 함수 통합
  - WeaponController, WeaponManager, InputManager 등 연동 구조 정비

- 총기 UI 및 획득 연동 개선
  - GunWeaponController에 UpdateAmmoCount() 함수 추가
    - 무기 장착 시 자동 호출되도록 설정
    - 동일 AmmoType의 총알 획득 시 즉시 UI 반영
  - InventoryManager에서 총알 획득 후 장착 무기와 매칭 시 UI 갱신 로직 추가

- 인벤토리/상점 슬롯 UI 개선
  - InventorySlot.Set()에서 isShopSlot 플래그 기반으로 CountBadge 표시 여부 분기
    - 상점 슬롯: 개수 미표시
    - 인벤토리 슬롯: 개수 표시

- 인벤토리 UI 오픈 방식 개선 및 디테일 패널 분기 처리
  - InventoryUI.Open() 함수에 overrideDetailPanel 인자 추가
    - 단독 인벤토리: 내부 패널 사용
    - 상점 호출: 상점 전용 패널 연동
  - OnDisable() 시 디테일 패널이 켜져 있으면 자동 종료 처리
  - UIManager에서 ToggleInventoryUI() 시 Open()을 호출하도록 변경

- 인벤토리 컨텍스트 시스템 도입
  - InventoryOpenContext 열거형 추가 (Standalone / Shop)
  - context에 따라 그리드 패딩 반응형 적용
  - AddSlot(), UpdateOrAddSlot() 함수로 슬롯 추가 및 갱신 로직 모듈화
    - 구매 시: 슬롯 생성 또는 수량 증가
    - 판매 시: 수량이 0이면 슬롯 제거
  - ShopUI에서 구매/판매 시 inventoryPanel.UpdateOrAddSlot() 호출로 자동 동기화

### 메모
- UIManager와 InventoryUI의 연결 구조를 명확히 정리해두었기 때문에 UI 흐름이 훨씬 일관되고 관리하기 쉬워짐
- 무기 시스템이 ItemData 기반으로 통합되면서 상점 → 인벤토리 → 무기 → HUD까지 흐름이 정돈됨
- 디테일 패널 override 구조 덕분에 재사용성과 기능 분리도가 올라감

---

## 날짜: 2025.09.01 (월) 작업 기록

### 주요 작업
- **퀵슬롯 시스템 1차 구현 및 아이템 사용 통합 처리**
  - `QuickSlotManager.cs`, `QuickSlotSlot.cs` 신규 제작
  - 1~4번 슬롯에 무기/소비 아이템 등록 및 사용 가능
  - 슬롯 아이콘 표시 및 선택 강조 처리
  - 소비형 아이템 수량 0일 시 회색 처리 로직 적용

- **무기 스위칭 시스템 퀵슬롯 기반 구조로 전환**
  - 기존 `SwitchWeapon()` 방식 제거
  - `InputManager.HandleQuickSlotActivation()` 신규 추가
    - 1~4번 숫자키로 퀵슬롯 사용
    - 마우스 휠로 퀵슬롯 내 무기만 순환
  - `WeaponManager.CanSwitchWeapon()` 도입하여 전투 중 장착 제한 로직 이관

- **UI 레이아웃 및 시각적 개선**
  - `QuickSlotSlot` 프리팹 제작 (64x64, 아이콘/숫자 힌트/하이라이트 포함)
  - `HUD_Canvas > BottomRight_Panel > QuickSlots_Panel` 구성
    - `HorizontalLayoutGroup`, `ContentSizeFitter`로 자동 정렬
    - 슬롯 간 Spacing 10px, Padding 8px 설정
  - 숫자 힌트 위치/스타일 정리

- **선택 아이템 동기화 시스템 구축**
  - `SelectedItemContext.cs` 신규 생성
    - 인벤토리/상점 간 선택된 아이템 정보 동기화
    - `OnSelectedItemChanged` 이벤트 기반 구조로 통합 관리
  - `InventoryUI`, `ShopUI` 모두 이벤트 등록 및 하이라이트 동기화 처리
  - `InventoryUI.Close()` 시 선택 초기화 처리 (`SelectedItemContext.Clear()`)

- **퀵슬롯 등록/해제 및 자동 갱신 처리**
  - 이미 등록된 아이템을 다시 클릭하면 슬롯 해제
  - `QuickSlotSlot.OnPointerClick()`에서 아이템 등록/해제 처리
  - `QuickSlotManager.RefreshAllSlotVisuals()` 도입
    - 소비형 아이템 수량 변화 시 색상 자동 반영
    - `InventoryManager.AddItem()`, `RemoveItem()`에서 호출

- **아이템 사용 로직 개선**
  - `ItemManager.ApplyConsumableItemEffect()` 내 수량 0일 시 효과 중단
  - 소비형 아이템은 사용 후에도 퀵슬롯에 남도록 유지

### 메모
- 퀵슬롯 시스템은 실질적 사용이 가능한 1차 완성 상태
- 향후 툴팁 표시, 아이템 우클릭 등록 기능은 필요 시 후순위로 구현 예정
- 전체 시스템은 무기/소비형 아이템 공용 구조로 설계되어 유연성 확보
- UI는 가독성과 접근성을 우선으로 배치 완료 (중앙 하단 → 오른쪽 하단)

---

## 날짜: 2025.09.02 (화) 작업 기록

### 주요 작업
- 인벤토리 장비 전용 툴팁 구현
  - InfoTooltipTrigger 컴포넌트로 i 아이콘 호버 시 TooltipUI 표시
  - TooltipUI는 장비형 아이템(ItemType.Equipment)에만 활성화
  - 공통 정보(타입, 공격력) + 총기 타입 전용 정보(fireRate, range 등) 분기 표시
  - 툴팁 위치는 슬롯 우측 중앙 기준으로 자동 배치되도록 RectTransform 기반 위치 계산

- 아이템 상세 패널 버튼 기능 연동 (장착/해제/사용/버리기)
  - ItemManager에서 HandleItemAction() 내부에 장착 중 자동 해제 로직 추가
  - DropItem() 함수 추가 → 아이템 수량 차감 및 장착 중 장비 자동 해제 후 제거
  - ItemDetailPanel에서 버튼 클릭 시 ItemManager 함수만 호출되도록 구조 단순화
  - 버튼 리스너 중복 방지 (RemoveAllListeners 적용)
  - 아이템 수량이 0이 되면 슬롯 제거 및 상세 패널 자동 닫힘 처리

- 장착 마커 표시 기능 구현
  - InventorySlot에 equippedMarker 오브젝트 연결
  - InventoryManager.IsEquipped() 기준으로 장착 여부 판단
  - 장착된 경우 슬롯 좌상단에 '장착중' 텍스트 마커 표시

### 메모
- 장착 마커는✔︎ 아이콘보다는 ‘장착중’ 텍스트가 더 직관적이라 판단되어 적용
- i 아이콘, 장착 마커 크기가 작다는 피드백 반영 → 사이즈 확대 및 상단 정렬 통일
- TooltipUI는 확장성이 좋아서 이후 방어구/액세서리 등의 정보도 자연스럽게 추가 가능
- ItemDetailPanel은 각 버튼마다 별도 로직을 가지지 않고 ItemManager에 위임하는 방식으로 구조 단순화

---

## 날짜: 2025.09.03 (수) 작업 기록

### 주요 작업
- Save 시스템 전반 구축 및 테스트
  - ISaveable 인터페이스 설계
    - CaptureStateJson() / RestoreStateJson(string json) 구조 정의
  - SaveId 컴포넌트 구현
    - GUID 자동 생성 및 수동 재설정 (ContextMenu)
  - SaveableBehaviour 추상 클래스 도입
    - SaveId 캐싱 및 ISaveable 기본 구현 포함
- SaveManager 저장/로드 시스템 구현
  - JSON 기반 단일 슬롯 저장
  - 씬 이름 및 저장 시각 메타 정보 포함
  - FindObjectsByType → ISaveable 필터링 → 중복 SaveId 검출 로그 처리
  - 다른 씬 로딩 후 상태 복원 처리 구조 포함
- PlayerSaveProxy 구현 및 PlayerManager 연동
  - 위치, 회전(yaw), HP, MP 저장 및 복원 처리
  - CharacterController 위치 강제 변경 시 비활성화 후 복원 → 튐 현상 방지
  - PlayerManager에 SetHP / SetMP 추가로 복원 시 UI 동기화 포함
- Core 프리팹 계층 구조 재정비 및 프리팹화
  - Systems / Gameplay / Presentation / Player 오브젝트로 분리 정리
  - UI 매니저는 별도로 UI 루트에서 관리
- CoreBootstrap 도입 및 매니저 초기화 통일
  - DontDestroyOnLoad 처리
  - 각 매니저 Initialize() 함수 분리 및 중복 인스턴스 방지 구조 구축

### 메모
- 플레이어 위치 저장이 적용되지 않았던 문제는 CharacterController로 인해 발생했으며, `.enabled = false` 처리로 해결함
- Player는 Core 하위에 있기 때문에 씬 전환 시에도 살아남으며, 위치 복원은 강제적으로 동작시켜야 함
- UIManager는 씬마다 존재하므로 CoreBootstrap 초기화 대상에서 제외함
- 초기화 순서를 명확히 정리해두어 다른 매니저 간 의존 관계로 인한 오류 방지에 용이함
- 포트폴리오 문서화 시 SaveableBehaviour 기반 저장 구조 설계 이유(중복 제거, 일관성 확보, 확장성 우위 등)를 반드시 포함해야 함

---

## 날짜 2025.09.04 (목) 작업 기록

### 주요 작업
- **플레이어 골드 저장/복원 연동**
  - `PlayerSaveProxy`에 골드 필드 추가
  - `PlayerManager.AddGold()` 호출 방식으로 상태 복원
  - HUD(Gold 텍스트)는 자동 동기화됨

- **인벤토리 저장/복원 구현**
  - `InventoryManager`에 `DumpItemsAndAmmo()`, `RestoreItemsAndAmmo()` 함수 추가
    - 아이템 및 탄약 상태를 `itemId`, `ammoType` 기준으로 직렬화
    - `TryAddItem()`, `AddAmmo()` 경유로 HUD/퀵슬롯 자동 동기화
  - `InventorySaveProxy` 신규 생성 및 SaveManager 연동

- **무기 저장/복원 연동**
  - `WeaponSaveProxy` 신규 생성
    - 장착된 무기 슬롯 인덱스 저장 (`equippedIndex`)
    - 각 무기 탄창 수 (`currentAmmo`) 저장
    - 복원 시 `EquipWeapon()` → 탄약 수 복원 → `UpdateAmmoCount()` 호출
  - `GunWeaponController`에 `SetCurrentAmmo()` 추가
  - 무기 해제 시 UI(크로스헤어/탄약 패널 등) 비활성화 일관 처리

- **무기 복원 안정성 개선**
  - 무기 비장착 상태에서 저장한 경우, 로드 시 손에 무기 남는 문제 수정
  - `WeaponManager.Start()`에서 `currentAmmo` 초기화 분리
    - `Start()`에서 `CurrentAmmo < 0`일 때만 초기화
    - 세이브 데이터가 덮어씌워지지 않도록 보완

- **퀵슬롯 구조 개선 및 저장 시스템 연동**
  - `QuickSlotManager`를 `CoreBootstrap` 하위로 이동
    - 저장/복원 흐름에 맞게 구조 통합
    - 슬롯 오브젝트 바인딩은 `UIManager.Start()`에서 처리
  - `QuickSlotSaveProxy` 신규 생성
    - 슬롯별 `itemId`, 선택 슬롯 인덱스 저장/복원
    - `AssignItemToSlot()` 경유로 UI 자동 반영

- **퀵슬롯 하이라이트 자동화**
  - `QuickSlotManager.RefreshHighlight()` 함수 추가
    - 현재 장착 무기를 기준으로 하이라이트 처리
  - `EquipWeapon()`, `UnEquipWeapon()`, `AssignItemToSlot()`에서 자동 호출
  - `ActivateSlot()` 중복 호출 제거로 구조 정리

### 메모
- 저장 시스템 구조가 거의 완성 단계에 도달했음
- 무기/탄약 관련 초기화 순서에 주의 필요
- 퀵슬롯 하이라이트는 이제 모든 흐름에서 자동으로 반영되므로 추가 호출 불필요
- 다음은 저장 슬롯 UI 또는 자동 저장 로직을 설계해도 좋을 시점

---

## 날짜: 2025.09.05 (금) 작업 기록

### 주요 작업
- 세이브 가드 시스템 도입
  - SaveGuard.cs 구현: 태그 기반 저장 차단 구조
  - SaveBlockTag enum 도입으로 퍼즐/컷씬/전투 등 독립 제어 가능
  - 참조 카운트 기반 Block/Unblock 처리 및 이벤트 발생 구조 구축

- 저장 함수 구조 개편 및 저장 의도 구분
  - SaveIntent enum 도입 (Manual / Auto / Checkpoint)
  - SaveManager.Save()에 intent 매개변수 추가
  - Manual일 경우에만 SaveGuard로 차단하고, Auto는 항상 허용

- 퍼즐 상태와 저장 정책 통합
  - 퍼즐 진입 시 자동 저장 + 저장 차단 (PuzzleStateTrigger → PuzzleState로 책임 이전)
  - 퍼즐 클리어 시 저장 가능 상태 복귀 및 자동 저장

- 플레이어 위치 이동 로직 통합
  - PlayerController.SetPositionAndRotation(pos, rot) 함수 추가
  - 위치 이동 시 CharacterController 잠금 해제/재활성화 포함
  - 기존 위치 설정 코드 통일 및 중복 제거

- 퍼즐 로드시 상태 초기화 구현
  - PuzzleRoomManager에 TransformSnapshot 구조체 도입
    - 위치, 회전, 활성화 상태를 통합 저장 및 복원
  - 퍼즐 입장 시 CacheInitialStates() 호출로 초기 상태 저장
  - 퍼즐 미클리어 로드시 ResetToInitialIfUncleared()를 통해 상태 초기화 및 입구 리스폰

- 로드 이벤트 시스템 도입 및 퍼즐 트리거 연동
  - SaveManager에 OnAfterLoad 이벤트 추가
  - RestoreState() 이후 퍼즐이 직접 초기화되도록 PuzzleStateTrigger에서 이벤트 구독
  - 퍼즐 클리어 시 및 비활성화 시 구독 해제

- 퍼즐 진입 흐름 안정화
  - 퍼즐 오브젝트 활성화 → 스냅샷 저장 → 오토세이브 순서를 코루틴으로 명확하게 처리
  - 퍼즐 클리어 후 isCleared 플래그 세팅 추가로 상태 일관성 보장

### 메모
- 퍼즐 저장/로드 정책이 확정적으로 안정화됨
- 수동 저장 차단 → 클리어 후 오토세이브 → 로드시 자동 초기화 흐름이 자연스럽게 연결됨
- 추후 다른 시스템(컷씬, 전투 등)에도 SaveGuard 태그 연동으로 확장 가능

---

## 날짜: 2025.09.08 (월) 작업 기록

### 주요 작업
- 저장 시스템 안정화 및 경량 저장 루틴 구축
  - `DefaultSave(slot)` 함수로 저장 구조 통일
  - `AutoSave(reason)`으로 자동저장 트리거 구성
  - `isSaving` 플래그로 중복 저장 방지
  - `SaveGuard.Block()`/`Unblock()` 구조 정립
  - 저장 완료/실패 시 토스트 메시지 출력
  - `WriteAtomic()`으로 저장 중 파일 깨짐 방지
  - `JsonUtility.ToJson(..., false)`로 용량 최적화

- 외부 저장 호출부 정비 및 통일
  - `PuzzleRoomManager`, `PuzzleStateTrigger`, `SaveDebug`의 저장 호출 `DefaultSave()`로 통일
  - 저장 흐름을 일관되게 `BackgroundSaveRoutine()` 기반으로 전환

- 보스 페이즈 진입 시 자동 저장 연동
  - `BossPhaseManager.SetPhase()`에 자동 저장 트리거 삽입
  - `BossIntroState`, `PuzzlePhase1State`, `FinalPuzzleState`에서 호출
  - 보스 시작 시 저장 차단(Block), 종료 시 해제(Unblock)

- 블랙페이드 연출용 `FadeUI` 컴포넌트 구현
  - `Show(duration)`, `Hide(duration)` 코루틴 제공
  - `CanvasGroup` 기반 페이드 + 입력 차단 처리
  - `UIManager`에 `FadeUI` 참조 연결

- 입력 차단 기능 도입
  - `InputManager.SetInputEnabled(bool)` 함수 추가
  - `isInputEnabled` 체크로 `Update()` 내 전체 입력 차단 가능
  - 저장/로드 중 오작동 방지용으로 설계

### 추가 개선 작업
- **세이브 버전 관리 1차 도입**
  - `SaveFile.version` 필드 및 `CURRENT_VERSION = 1` 선언
  - 저장 시 현재 버전 값을 기록하고, 로드시 버전 불일치 시 `TryMigrate()` 경로로 업그레이드 처리
  - 현재는 v1 기준 통과만 하고, 구조 변경 시 단계적 마이그레이션 방식으로 확장 가능
  - 구버전 세이브 파일도 v1로 간주되어 호환성 유지

- **.bak 백업 및 자동 복구 로직 적용**
  - 저장 시 기존 세이브 파일을 `.bak` 확장자로 자동 백업 (`File.Replace(...)`)
  - `Load()`에서 메인 세이브가 없거나 파싱에 실패한 경우 `.bak` 파일로 자동 폴백
    - 백업 복구 성공 시 `"백업 세이브로 복구했습니다"` 토스트 출력
  - 파싱/버전 체크/씬 전환 등의 공통 처리를 `TryLoadFromJson()`으로 분리하여 구조 정리

### 메모
- 저장 중 시각 연출 및 입력 차단 구조 정비 완료
- 세이브/로드 공통 래퍼(SaveFlow)는 필요 시점에 도입 예정
- 현재 구조는 페이드/입력잠금/저장 안정성 모두 확보된 상태
- `.json.bak` 파일은 항상 **이전 세이브본 1회분만** 보관되며, 저장 실패 또는 파싱 오류 발생 시 자동으로 복구된다
- `TryMigrate()`는 앞으로 구조 변경이 생겼을 때 `case` 단위로 단계별 보정 처리를 넣을 수 있는 확장 포인트다

---

## 2025.09.09 (화) 작업 기록

### 주요 작업
- **보스 상태 저장 프록시 도입**
  - `BossControllerSaveProxy` 작성: `phase`, `hpPercent` 저장/복원
  - 복원 순서: `SetHPWithPercent()` → `SetPhaseFromSave()`로 안전 처리
  - `BossController`, `BossPhaseManager`에 복원 전용 메서드 추가
  - 정책: 보스전 수동 저장 차단 (`Phase1` 진입 시 Block, 엔딩 후 Unblock 유지)

- **세이브 가드 UX 개선 및 토스트 메시지 공통화**
  - `SaveGuard`: `GetCurrentMainBlock()` 추가 (우선순위: Boss > Puzzle > Cutscene)
  - `SaveManager`: 차단 사유에 따라 수동 저장 시 토스트 출력
  - 세이브 중 중복 실행 방지 및 페이드/토스트 UX 정비
  - `SaveBlockTag` 메시지 Dictionary로 관리, `static readonly`로 GC 최적화

- **GenericInteractionSaveProxy 및 IInteractableSavable 설계**
  - 인터페이스: `isActivated`, `isHeld`, `TryGetWorldPose()` 제공
  - 프록시: `activated`, `held`, `hasPose` 저장 및 조건부 복원 처리
  - 위치/회전은 필요 시만 복원되도록 설계
  - 기존 `InteractionHandler`, `IInteractable`와 충돌 없이 연동 가능

- **상호작용 오브젝트에 상태 저장 적용**
  - `BossKeyPickup`, `WateringCan`:
    - `IInteractableSavable` 직접 구현
    - `held` 및 `pose` 저장/복원 적용
    - 삽입(소모)은 `SetActive(false)` 방식으로 변경
  - `KeycardDoor`:
    - 열림 여부(activated)만 저장
    - 복원 시 Animator 상태 이름 기반 스냅 처리

- **플레이어 손 오브젝트 저장 구조 도입**
  - `PlayerSaveProxy`에 `heldObject(string)` 필드 추가
  - `CaptureStateJson`: `CurrentHeldObject.name` 저장
  - `RestoreStateJson`: 해당 이름으로 `Find → SetHeldObject()` 복원
  - 상체 애니메이션(`Empty`, `Held`, `Sword`, `Gun`) 정확히 연동

### 메모
- 보스전 수동 저장 차단 정책 확정: Phase1 진입 시 차단, 엔딩 종료 후 해제
- `GameObject` 저장이 안되는 문제로 이름 기반(`string`) 저장 방식 도입
- `GenericInteractionSaveProxy` 구조는 재사용성 높고 퍼즐/오브젝트 확장 용이
- 내일은 저장 슬롯 메타 정보 확장 및 테스트 매트릭스 실행 예정

---

## 2025.09.10 (수) 작업 기록

### 주요 작업
- 세션 기반 누적 플레이타임 저장 시스템 구현
  - SaveMeta.playtimeSeconds 필드 추가 (누적 시간 초 단위 저장)
  - 저장 시점에 prevPlaytime + 세션 경과 시간으로 누적 플레이타임 자동 계산
  - 로드 시 저장파일의 누적값을 세션 시작점으로 반영
  - SaveMeta.FormatPlaytime()으로 UI 표기용 hh:mm:ss 변환 지원

- 저장 타입 필드(saveType) 도입 및 저장 구조 개선
  - "Quick", "Auto", "Manual" 타입을 SaveIntent.ToString()으로 자동 기록
  - 저장 타입 기반으로 UI 필터링 및 정렬 가능
  - 기존 세이브 파일과의 호환성 유지

- 퀵세이브 / 자동저장 슬롯 구조 정리
  - FirstSlotIndex = 1, LastSlotIndex = 9로 슬롯 범위 상수화
  - 퀵세이브: 슬롯 1번 고정  
  - 자동저장: 슬롯 2~4번 순환  
  - 수동 저장: 슬롯 5~9 (UI에서 선택)

- SaveSlot 프리팹 및 바인딩 구조 구현
  - 루트에 Button 적용하여 전체 클릭 가능 영역 처리
  - Init(index, meta)로 슬롯 초기화
  - SetMeta(), SetSelected(), SetInteractable() 함수로 상태 제어
  - 저장/로드 시 슬롯 클릭 이벤트로 SaveUI에 위임 처리

- 세이브 메타 읽기 전용 클래스(MetaWrapper) 및 파싱 함수 구현
  - 저장 전체 파일 순회 → 메타 정보만 로딩
  - SaveManager.GetAllMeta() 헬퍼 함수로 외부 사용 가능

- SaveUI.cs 구현 및 슬롯 동작 구조 정립
  - SaveUIMode 열거형(SaveOnly / LoadOnly)으로 모드 분리
  - 저장 모드: 수동 슬롯(5~9)만 표시, 빈 슬롯 저장 가능
  - 불러오기 모드: Quick + 최신 4개만 정렬 표시
  - RefreshSlots(), GetManualSlotsOnly(), GetDisplaySlots()로 구조 분리

### 메모
- 레이아웃 구조는 아직 확정 전 → 내일 Slot 리스트 UI 레이아웃 구성 및 시각 스타일링 예정
- 현재 구조 기준으로 저장 정책/입력 흐름은 잘 정리되어 있음
  - QuickSave(), AutoSave()는 코드에서 직접 호출
  - 수동 저장은 UI에서 슬롯 클릭 기반으로 진행

---

## 날짜: 2025.09.11 (목) 작업 기록

### 주요 작업
- 옵션용 SaveLoadPanel 및 SaveSlot 프리팹 구성 완료
  - SaveLoadPanel을 옵션 패널 하위에 임베디드 구조로 배치
  - 저장/불러오기 버튼 그룹과 슬롯 리스트를 분리해 구조적 명확성 확보
  - SaveUI는 `SaveOnly`, `LoadOnly` 모드에 따라 슬롯 표현 방식 분기 처리

- SaveSlot 레이아웃 디자인 및 초기 구현
  - 사이버펑크2077 스타일 참고 레이아웃 적용
    - 좌측: 썸네일 (RawImage)
    - 중앙 상단: 퀘스트 타이틀 + 세이브 타입
    - 중앙 하단: 씬 이름
    - 우측 상단: 플레이타임
    - 우측 하단: 저장 시각

- SaveUI를 고정 슬롯 방식으로 리팩토링
  - 동적 생성 대신 `slotList` 기반 5개 고정 슬롯으로 재설계
  - `RefreshSlots()`는 **실제 슬롯 인덱스(slotIndex)** 를 기준으로 슬롯과 데이터를 매핑
    - 저장 모드(=SaveOnly): 수동 저장 슬롯(5~9번)만 표시
    - 불러오기 모드(=LoadOnly): 퀵 저장 1개 + 최신 자동/수동 저장 4개 혼합 노출
  - 빈 슬롯은 `frameRoot` 비활성화 및 버튼 비인터랙티브 처리

- SaveSlot 동작 개선
  - `Init()`에서 `SaveMeta`를 바탕으로 퀘스트/세이브타입/씬/플레이타임/저장시각을 UI에 연동
  - 슬롯 클릭 시 저장/불러오기 분기 처리 및 ConfirmModal 연동

- 세이브 슬롯 정렬·매핑 로직 개선 (버그 픽스 반영)
  - `GetDisplaySlots()`는 `(int slotIndex, SaveMeta meta)` 쌍을 반환하도록 변경 — UI 정렬과 실제 파일(슬롯) 매핑 분리
  - 퀵 슬롯이 존재하면 퀵 1 + 최신 저장 4개, 없으면 최신 저장 5개 방식 적용
  - 부족 시 5~9번 수동 슬롯 기준으로 `(-1, null)` 패딩하여 UI 정렬 유지
  - 이 변경으로 UI에 보이는 순서와 실제 로드 대상이 불일치해 엉뚱한 슬롯을 로드하던 버그 해결

- UI 초기화/호출 흐름 안전화 (버그 픽스 반영)
  - `OnEnable()`에서 즉시 `RefreshSlots()` 호출하던 부분 제거 — 슬롯/매니저 초기화 순서(race)로 인한 NRE 방지
  - `SaveUI.Open()`에서만 `RefreshSlots()`를 호출하도록 통일하여 UI가 실제로 열릴 때만 초기화 수행
  - `OnEnable/OnDisable`에서는 `SaveManager.OnSaved` 구독(및 해제)만 처리하여 저장 직후 갱신은 유지

- 파일 안정성 및 UX 개선
  - 저장 시 `.json` → `.bak` 전환 로그 명확화 및 토스트 메시지 처리 개선
  - Toast 메시지에 `WaitForSecondsRealtime` 적용하여 시간 정지 상태에서도 자연스러운 페이드아웃 보장

### 메모
- 문제 발생 원인: UI가 보여주는 정렬 순서와 실제 파일(슬롯 인덱스) 매핑이 분리되어 있었음. `RefreshSlots()`에서 표시용 정렬 결과만 넣고 슬롯에 실제 인덱스를 주입하지 않아 클릭 시 잘못된 슬롯이 로드됨.  
  → 해결: `GetDisplaySlots()`에서 `(slotIndex, meta)` 쌍을 반환하고 `SaveSlot.Init(realIndex, meta)`로 실제 인덱스를 주입하도록 변경.
- `OnEnable()`에서 바로 `RefreshSlots()`를 호출하면 SaveSlot/SaveManager 초기화가 끝나기 전에 접근해 NRE가 날 수 있으므로, UI 활성화 진입점은 `Open()`으로 표준화하여 초기화 순서 문제를 회피함.
- 퀘스트 시스템 미구현 상태라 퀘스트 타이틀은 당분간 메타 텍스트로 대체 표시함.

---

## 날짜: 2025.09.12 (금) 작업 기록

### 주요 작업
- 썸네일 캡처 시스템 추가 (ThumbnailCapture)
  - `ThumbnailCapture.cs` 구현
    - `Awake()`에서 `previewCamera` 초기화, 메인 카메라 설정 복사(UI 레이어 제외), `RenderTexture(rt)` 생성 및 `rt.Create()` 호출
    - `CaptureToFile(string fullPath)`에서 수동 렌더링: `previewCamera.targetTexture = rt` → `previewCamera.Render()` → `RenderTexture.active`로 `ReadPixels` → `Texture2D`로 인코딩(`EncodeToPNG`) → 파일 쓰기 → `finally`에서 `RenderTexture.active` 및 `previewCamera.targetTexture` 복구
    - `OnDisable()`에서 `rt.Release()` 및 `Destroy(rt)` 처리
  - 인스펙터/설정
    - `previewCamera`는 메인 카메라의 자식으로 배치(Transform 동기화), `enabled=false`로 수동 렌더링 제어
    - 캡처 해상도: `width/height` 필드(기본값 512×288)

- SaveManager 연동 (썸네일 생성/메타 기록)
  - `SaveManager`에 `ThumbnailCapture thumbnailCapture` 필드 추가 (인스펙터 할당)
  - `GetPreviewPath(int slot)` 추가: 슬롯에 대응하는 전체 썸네일 경로 반환 (예: `<persistentDataPath>/Previews/slot_5.png`)
  - `ToRelativePreviewPath(string fullPath)` 추가: 메타에 저장할 상대경로 형태로 변환(예: `Previews/slot_5.png`)
  - `Save(int slot, SaveIntent intent = ..., string thumbnailRelPath = null)` 오버로드: `SaveFile.meta.thumbnail`에 상대경로 기록 지원
  - `BackgroundSaveRoutine(int slot, SaveIntent intent)` 수정
    - `WaitForEndOfFrame()` 후 `thumbnailCapture` 유효 시 `GetPreviewPath(slot)`로 전체 경로 생성 → `thumbnailCapture.CaptureToFile(previewFullPath)` 호출
    - 캡처 성공 시 `ToRelativePreviewPath(previewFullPath)` 값을 `Save()` 호출에 전달해 메타에 기록

- ThumbnailCapture 안정성 개선 (depth / 텍스처 정리)
  - `Awake()`에서 `rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32)`로 depth 24 반영
  - `CaptureToFile()`에서 `Texture2D tex = null`을 try/catch 외부에 선언하고 `finally`에서 `if (tex != null) Destroy(tex);`로 정리 보장
  - `finally`에서 `RenderTexture.active` 및 `previewCamera.targetTexture` 복구 유지

- SaveSlot 썸네일 로드/표시 기능 추가
  - `SaveSlot.cs`에 `RawImage preview`, `Color deactivatePreview`, `Color activatePreview` 등 SerializedField 추가
  - `previewTexture` 필드로 로드된 텍스처 참조 보관
  - `SetMeta(SaveManager.SaveMeta)` 변경: 메타 존재 시 `LoadPreview(meta.thumbnail)` 호출, 미존재 시 `UnloadPreview()` 호출
  - `LoadPreview(string relativePath)` 구현: `Application.persistentDataPath`와 결합한 전체 경로에서 PNG 읽어 `Texture2D.LoadImage()`로 디코드 → `preview.texture`에 할당. 실패 시 `preview.texture = null` 처리
  - `UnloadPreview()` 구현: 기존 `previewTexture` `Destroy()` 후 `preview.texture = null`, `preview.color`를 비활성 색으로 리셋

- QuickSave 입력 추가 및 연동
  - `InputManager.cs`에 `public event Action OnQuickSave;` 추가
  - `Update()`에 `if (Input.GetKeyDown(KeyCode.F5)) OnQuickSave?.Invoke();` 추가
  - `SaveManager`에서 `OnEnable()`에 `InputManager.Instance.OnQuickSave += QuickSave;` 구독 및 `OnDisable()`에서 해제

- 게임 상태 ↔ SaveGuard ↔ UI 연동 및 EnterPreviousState 안정화
  - `GameManager.cs`
    - `EnterPreviousState()` 구현: 이전 상태 유효성 검사 → 탐색 상태 보정 → 중복 전환 방지 → `ChangeState` 호출
    - `OnPausePressed()`에서 일시정지 복귀 흐름에 `EnterPreviousState()` 사용
  - `CutsceneCameraManager.cs`
    - `StartCutscene()`/`EndCutscene()` 수정: SaveGuard 직접 블락/언블락 로직 제거
    - 컷씬 시작 시 `GameManager.EnterCutscene()` 호출, 종료 시 `GameManager.EnterPreviousState()` 호출
  - `SaveGuard.cs`
    - `SaveBlockTag` enum에 `Combat`, `Pause`, `UI`, `GameOver` 등 추가
    - 블락 우선순위(예: Boss, Combat, Puzzle, Cutscene, Pause, GameOver, UI, Default) 기반 대표 태그 선택 로직 제공
  - `UIManager`, `PlayerHUD`
    - `PlayerHUD.SetPlayerHud(bool enabled)` 추가
    - `UIManager.UpdatePlayerHud(bool enabled)` 래퍼 추가
  - 상태들(CutsceneState, CombatState, PausedState 등)에서 HUD/SaveGuard 블락/해제 호출 일관성 확보
    - `Exit()`에서 `GameManager.EnterPreviousState()` 호출 제거(상태 전환 책임 분리)

- UI / SaveGuard 연동 및 상태별 UI 토글
  - `UIManager.UpdateUI(bool)` 추가 (전체 UI 토글)
  - `SaveGuard` 우선순위 기반 대표 태그 선택 및 블락 로직 유지
  - `MainMenuState`, `LoadingState`, `GameOverState`, `ExplorationState` 등 각 state의 `Enter/Exit`에 `UIManager` 및 `SaveGuard` 호출 반영

- 초기화 순서 및 이벤트 등록 안정화 (CoreBootstrap)
  - 초기화 순서 명시: `InputManager` → `SaveManager` → `GameManager`
  - `GameManager.Initialize()`에서 `OnAfterLoad` 이벤트 등록
  - `SaveManager.Initialize()`에서 `OnQuickSave` 이벤트 등록
  - `OnEnable()`에서 즉시 등록하는 방식으로 인한 NRE 위험 제거

- 카메라 및 UI 상태 복원 흐름 구현
  - `CameraController.ResetToPlayer()` 구현: 위치/회전/FOV/줌/커서 상태 초기화
  - `CutsceneCameraManager.EndCutscene()`에서 `CameraController.ResetToPlayer()` 호출
  - `GameManager.PostLoadRoutine()` 구현: 로드 후 카메라 복원, `InputManager.SetInputEnabled(true)`, `UIManager.UpdatePlayerHud(true)`, 커서 잠금 복원

- 저장 중 입력 차단 및 복원
  - `SaveManager.BackgroundSaveRoutine()` 시작 직전에 `InputManager.SetInputEnabled(false)` 호출
  - 저장 완료(`finally`)에서 `InputManager.SetInputEnabled(true)` 호출 보장 (예외 상황 포함)
  - 의도: 저장 중 상태 꼬임, 무기 스위칭, HUD 중복 입력 방지 및 로드 시 입력 잠금 흐름과의 일관성 확보

### 메모
- 썸네일 파이프라인
  - 캡처는 `WaitForEndOfFrame()` 이후에 수행하되, 캡처 실패(파일 쓰기/IO 문제 등) 시에는 로그만 남기고 저장은 메타 없이 진행하도록 처리(사용자 UX를 저해하지 않기 위함).
  - 썸네일은 `SaveMeta.thumbnail`에 상대경로(`Previews/slot_N.png`)로 기록하며, 로드 시 `SaveSlot`이 해당 파일을 읽어 UI에 표시.
  - 썸네일 파일 관리는 저장/삭제 시 일관되게 처리 필요(예: 슬롯 덮어쓰기 시 기존 프리뷰 파일 삭제 또는 덮어쓰기).

- SaveGuard / 상태 전이
  - SaveGuard 우선순위가 낮은 태그가 남아 있더라도 우선순위가 높은 태그가 블락 중이면 저장이 차단됨 — 대표 태그 정책으로 일관성 유지.
  - 일부 상태에서 `EnterPreviousState()` 호출 흐름을 정리하여 중복 전환/무한 루프 가능성 제거.

- 입력/초기화 안정성
  - Input → Save → Game 순 초기화 보장으로 런타임 NullReference 위험을 최대한 제거.
  - QuickSave(F5)는 게임플레이 중 빠른 저장을 허용하되, SaveGuard에 의해 차단될 수 있음.

---

## 날짜: 2025.09.18 (목) 작업 기록

### 주요 작업
- 오디오 시스템 전반 구현 (풀링 + 매니저 + 믹서 + 캐시)
  - AudioMixer 에셋 구성 (Master/BGM/SFX 그룹 및 파라미터 설정)
  - AudioManager.cs 전체 구현 (UI, 3D, BGM 사운드 관리 및 믹서 연동)
  - Master/BGM/SFX 볼륨 PlayerPrefs 저장/불러오기 및 dB 변환 적용
  - 마스터 뮤트 기능 구현 (슬라이더 조정과 무관하게 음소거 처리)

- 범용 풀 시스템 (PoolBase<T> / Pool<T>) 구현
  - AudioSource 풀링을 포함한 인스턴스 재사용 구조 마련
  - 자동 반환 / 수동 반환 / 루프용 재생 분리 및 충돌 방지 설계

- SfxPool 도입 및 3D/루프/수동 사운드 지원
  - PlayAt, PlayLoop, StopLoop, ReleaseAfter 등 재생 API 구성
  - 어드레서블 Address 기반 프리팹 로드 및 재생 기능 구축
  - 자동 반환 예약 중복 방지 및 루프 충돌 안전 해제 구현

- AudioManager의 전반적 구조 설계 및 최적화
  - AudioClip 캐시(clipCache), 핸들 관리(handle/label별) 설계
  - UI용 SFX는 사전 Preload 방식, 3D 사운드는 OnDemand 비동기 로드
  - bgmSourceA/B 듀얼 오디오소스를 통한 크로스페이드 구현

- Audio 관련 프리팹/오브젝트 인스펙터 설정 완료
  - SfxPool용 AudioSource 프리팹 (3D, Output=SFX) 구성
  - uiSfxSource / bgmSourceA / bgmSourceB 설정 및 연결

### 메모
- Addressables 로드/해제 구조를 AudioManager에서 통합 관리하도록 설계
- 오디오 믹서 그룹을 통해 전체 볼륨 제어 및 뮤트 연동 가능
- SfxPool은 향후 이펙트 풀 등으로도 확장 가능한 범용 풀 시스템 기반으로 구현
- UI 및 옵션 패널 연동은 내일 진행 예정 (옵션 UI 탭 구조는 이미 완성된 상태)

---

## 날짜 (예: 2025.09.19 (금)) 작업 기록

### 주요 작업
- OptionUI 전체 구현 및 탭 구조 정비
  - OptionOpenMode(Title, InGame) 분기처리로 저장 탭 조건부 표시
  - 탭 구조: 저장 / 스크린 / 오디오 / 컨트롤
  - 각 탭은 IOptionsTab 인터페이스 기반으로 독립 동작
  - 닫기 버튼 액션 델리게이트 처리(SetCloseButtonAction)로 상황에 따라 핸들링 전환

- 저장 탭(SaveTabController) 리팩토링
  - 기존 SaveUI → SaveTabController로 스크립트명 변경
  - 저장/불러오기 버튼 → 패널 오픈 연결
  - 닫기 버튼 → 패널 닫힘과 OptionUI 닫힘 사이 핸들링 분리
  - IOptionsTab 구현 및 그룹/패널 분리 구조 반영

- 스크린 탭(ScreenTabController) 구현
  - 해상도 / 화면모드 / 주사율 / VSync 드롭다운 자동 설정
  - 선택한 옵션 PlayerPrefs 저장 및 적용
  - 주사율 필터링 및 드롭다운 연동 로직 포함

- 오디오 탭(AudioTabController) 구현
  - 마스터 / BGM / SFX 볼륨 슬라이더 구현
  - 음소거 토글 추가
  - AudioManager에 볼륨 연동 및 저장 처리

- 컨트롤 탭(ControlTabController) 구현
  - 마우스 감도 X/Y 슬라이더 + 실시간 값 표시
  - Y축 반전 / 달리기 토글 / 웅크리기 토글 설정 구현
  - PlayerPrefs 저장 및 InputManager 연동

- InputManager 확장
  - 감도/반전/토글 설정 로드 및 저장 함수 추가
  - Toggle 방식 웅크리기/달리기 처리 방식 분리 구현
  - 입력 처리 로직 정리 및 감도 반영

- UIManager 오버레이 시스템 개선
  - ShowOverlayBackground / HideOverlayBackground 내에서 GameManager 상태 변경 통합
  - 일시정지 / 상점 / 인벤토리 / 옵션 UI의 일관된 흐름 정리

- PauseUI 연동
  - PauseUI → OptionUI 열기 동작 처리(OpenOption)
  - 옵션 진입 시 PauseUI 비활성화, 종료 시 복귀

### 메모
- 각 탭은 추후에도 독립적으로 확장 가능하도록 IOptionsTab 구조를 도입함
- 오버레이 UI 진입 시 일시정지 상태로 자동 전환되도록 흐름을 단일화하여 PauseUI/OptionUI/ShopUI 간 충돌 최소화
- 마우스 감도는 0.1 ~ 2.0 범위로 제한하며 슬라이더 step 및 숫자 표시 추가
- 향후 사운드도 슬라이더 step 및 숫자 표시 추가 고려
- 저장탭 내 세이브/로드 패널 오픈 시 닫기 버튼 핸들링 전환 구조 확정 (패널 → 그룹 → 옵션 UI 계층적 닫힘 처리)

---

## 2025.09.24 (수) 작업 기록

### 주요 작업
- **타이틀 UI 레이아웃 최종 구성**
  - CHRONOBLADE 타이틀 및 태그라인(시간을 베다) 중앙 배치
  - Press Any Key 텍스트를 하단으로 이동, 크기 축소 및 여백 조정
  - 시네마틱한 구도를 위해 분리선으로 상·하 구역 구분
  - 룬, 이펙트 등 추가 디자인 요소는 폴리싱 단계에서 반영 예정

- **타이틀 → 메인메뉴 전환 로직 안정화**
  - TitleUI: 아무 키 입력 시 페이드아웃/페이드인 전환
  - InputManager: OnPressAnyKey 이벤트 기반 트리거 연동
  - CanvasGroup 제어(interactable / blocksRaycasts)로 충돌 방지
  - Time.unscaledTime 기반으로 깜빡임 구현 (TimeScale=0에서도 동작)

- **MainMenuState 및 UI 리팩토링**
  - 메인메뉴 UI 기본 구조 배치 (텍스트 버튼, 좌측 패널)
  - New Game / Continue / Options / Exit 버튼 코드 및 이벤트 연결
  - Continue 시 로드 패널, Options 시 옵션 패널 토글 처리
  - Exit 시 빌드 환경/에디터 환경 대응 종료 처리
  - MainMenuState에서 입력/커서/TimeScale/SaveGuard 제어를 상태 진입/이탈 시점으로 이동

- **씬 구조 정리**
  - Title 씬 분리 및 빌드 세팅에 등록
  - Core 및 UI 프리팹을 타이틀 씬에 배치
  - TitleUIManager를 별도 구성해 타이틀/메인메뉴 전용으로 경량화

- **FSM 리팩토링 (ScriptableObject 기반)**
  - GameBaseState: MonoBehaviour → ScriptableObject로 전환
  - 상태별 Enter/Exit 로직 그대로 유지하되 GameManager 주입 방식 Init 도입
  - MainMenu, Loading, Exploration, Combat, Cutscene, Puzzle, Paused, GameOver 전부 SO 자산화
  - GameManager만 DontDestroyOnLoad 유지 → 상태 전환 안정성 확보

- **로드 플로우 개선**
  - SaveTabController: 슬롯 클릭 시 SaveManager.Load 직접 호출 제거
  - LoadingState를 통해 슬롯 인덱스를 받아 DefaultLoad 실행
  - OnAfterLoad 이벤트 수신 후 ExplorationState 전환
  - LoadingState.Enter/Exit에서 UI, 입력, TimeScale 제어로 일관성 확보

- **카메라 위치 초기화 버그 수정**
  - 씬 로드시 MainCamera 위치가 틀어지는 문제 수정
  - CameraController ResetToPlayer에서 cameraPosition에 따라 위치 및 회전 초기화

### 메모
- 씬 전환 흐름은 이제 FSM 규칙에 맞게 **MainMenu → Loading → Exploration**으로 일관되게 동작  
- CameraController 보정으로 로드시 “카메라 지 맘대로” 현상 해결  
- 타이틀/메인메뉴 UI는 레이아웃 안정화 단계까지 마무리, 폴리싱은 추후  
- ScriptableObject 기반 전환으로 상태 생명주기/전환 안정성이 크게 향상됨

---

## 2025.09.25 (목) 작업 기록

### 주요 작업
- **메인메뉴 버튼 연결 및 초기화**
  - MainMenuUI: New Game / Continue / Options / Exit 버튼 이벤트 연결
  - OnDestroy에서 이벤트 해제 처리로 메모리 누수 방지
  - NewGame → 지정된 STARTSCENE 로드
  - Continue → LoadPanel 토글 및 OpenPanel 호출
  - Options → OptionUI.Open/Close 토글
  - Exit → 에디터/빌드 환경 분기하여 종료 처리
  - 초기화 시 LoadPanel, OptionUI는 비활성화 상태로 시작되도록 구성
  - OptionUI.Awake()의 중복 SetActive(false) 제거 → 첫 클릭 시 정상 Open 호출 가능

- **Continue / Options 패널 토글 개선**
  - Continue 클릭 시 열린 Option 패널 자동 닫기
  - Continue는 LoadPanel 토글, 활성화 시에만 OpenPanel 호출
  - Options 클릭 시 열린 LoadPanel 자동 닫기
  - Options는 현재 상태에 따라 Open/Close 동작
  - 결과: 서로 다른 서브패널 동시 오픈 방지, 같은 버튼 재클릭 시 닫히는 직관적 토글 동작 보장

- **OptionUI 탭 우선순위 반영 버그 수정**
  - ShowTab 조건 수정: `index == currentTab` 조건 제거
  - 같은 탭이라도 초기화/우선순위 로직이 정상 반영되도록 개선
  - 결과: 옵션 UI 진입 시 기본 우선 탭(Screen/Save)이 정상적으로 표시됨

- **저장 데이터 유무 기반 Continue/Load 버튼 활성화**
  - SaveManager: HasAnySave() 함수 추가
  - MainMenuUI: UpdateContinueButton() 도입, OnSaved 이벤트 연동으로 자동 상태 갱신
  - SaveTabController: UpdateLoadButtonState() 추가, Start/RefreshSlots/OnSaved 이벤트 기반으로 Load 버튼 상태 자동 갱신
  - 결과: 저장 데이터가 없을 때 Continue/Load 버튼이 비활성화되어 UX 혼란 감소

### 메모
- 메인메뉴 전반의 동작 플로우(타이틀 → 메인메뉴, 버튼 이벤트, 옵션/로드 패널 토글)가 안정화됨  
- Continue/Load 버튼이 저장 데이터 존재 여부에 따라 동적으로 활성/비활성 처리되어 사용자 경험 개선  
- OptionUI 탭 우선순위가 정상적으로 반영되어 인게임/타이틀 모드에 맞는 기본 탭이 열림  

---

## 2025.09.26 (금) 작업 기록

### 주요 작업
- **State**
  - `LoadingState` 구조 확장 및 `NewGame/Load` 흐름 통합
    - `LoadingMode enum { None, NewGame, LoadSave, SceneTransition }` 추가
    - 각 모드에 따른 씬 로딩 분기 처리
    - `MainMenuUI`에서 NewGame 클릭 시 `NextLoadingMode` 지정 후 `GameManager.EnterLoading()` 호출
    - `SaveTabController`에서 Load 슬롯 클릭 시 `LoadingMode.LoadSave` 설정 및 슬롯 인덱스 지정 후 로딩 진입
    - 모든 씬 전환 흐름을 `LoadingState`로 일관되게 관리 (로딩 연출, 입력 차단, 상태 복원 포함)

- **Input**
  - ESC 입력 우선순위 기반 UI 정리 로직 구현
    - 우선순위: ConfirmModal → OptionUI → PauseUI → ShopUI → InventoryUI
    - `InputManager.TryCloseTopUI()` 함수로 ESC 입력 흐름 통합
    - UI 열림 상태에 따라 해당 UI만 닫고, 없으면 `OnPause()`로 Pause 상태 진입
    - 기존 `HandleUIBlockingInput()` 구조 제거

- **UI**
  - 상태이상 아이콘 UI(`StatusIconUI`) 구현 및 PlayerController 연동
    - `StatusIconUI.cs` 신규 구현: 상태 타입별 아이콘 표시 및 지속시간 연출
    - 지속시간 1초 전 알파 깜빡임, 무기한 지속 상태 처리
    - `PlayerController.ApplyStatus()` → `StatusIconUI.Show()`
    - `PlayerController.RemoveStatus()` → `StatusIconUI.Hide()`
    - HUD와 `UIManager.Instance.StatusIconUI`로 연결

  - Heartbeat UI 레이아웃 및 기본 스크립트 구현
    - `CenterPos`, `HearbeatImages` 부모 오브젝트 구성
    - `HeartbeatLineImage`: `PulseOriginPos` 및 RectTransform 접근자 정의
    - `HeartbeatLinePool`: `Pool<RectTransform>` 상속 및 `OnBeforeRelease()` 오버라이드
    - `HeartbeatScroller`: 기본 스크롤러 구조 정의 및 UI 연동 준비

### 메모
- 로딩/씬 전환, ESC 입력 우선순위, HUD 연동까지 UI/State/Input 흐름이 하나의 구조로 정리됨  
- Heartbeat UI는 기본 구조와 풀링까지 구현 완료, 세부 이동 로직은 다음 주에 다시 진행 예정

---

## 2025.09.29 (월) 작업 기록

### 주요 작업
- 하트비트 라인 스크롤러 구현 (HeartbeatScroller)
  - 일정 간격으로 배치된 이미지가 오른쪽으로 스크롤되며 순환되도록 구성
  - 이미지 중심이 패널 중앙을 정확히 beatInterval 주기로 통과하도록 scrollSpeed 계산
  - 가장 오른쪽으로 벗어난 이미지는 가장 왼쪽으로 이동시켜 재사용
  - OnBeat 이벤트 발생 시, 중앙에 가장 가까운 이미지에 깜빡임 연출 적용

- 하트비트 이미지 깜빡임 연출 (HeartbeatLineImage)
  - CanvasGroup을 활용해 alpha 페이드 방식으로 연출
  - Flash(): alpha = 1 → 지정된 targetAlpha까지 자연스럽게 감소하는 코루틴 실행

- HeartbeatScroller 구조 리팩토링
  - Start() → OnEnable() 구조로 전환
    - 하트비트 이미지 초기화 및 초기 위치 배치를 OnEnable()에서 처리
  - OnDisable()에서 리스트 및 리스너 정리
  - Show()/Hide()로 스크롤링 상태만 제어하고, GameObject 활성화는 Unity에서 직접 제어하도록 변경

- 하트비트 UI 마스크 및 배경 적용
  - Mask 오브젝트
    - Chamfered Rectangle 형태의 마스크 이미지 적용
    - 하트비트 라인(HeartbeatImages)이 마스크 영역 내에서만 렌더링되도록 처리
  - Background 오브젝트
    - 어두운 톤의 HUD 스타일 배경 + 사이버틱한 그리드 이미지 적용
    - 시각적 몰입감 및 전체 UI 일체감 강화

- 전체 UI 계층 구조 정리
  - `HeartbeatUI_Panel`
    - `Background` (배경 이미지)
    - `Mask` (Chamfered Rectangle)
      - `HeartbeatImages` (스크롤되는 이미지들)

### 메모
- 스크롤러의 중심 통과 위치 보정을 위해 이미지 크기 및 RectTransform 기준 위치 신경써야 함
- Flash 타이밍은 OnBeat 트리거 타이밍과 일치시켜야 자연스러운 연출 가능
- 마스크 처리 이후 UI 몰입도 향상됨. HUD 내 일체감 유지하도록 추후 다른 UI도 동일한 배경/마스크 구조 적용 고려

---

## 2025.09.30 (화) 작업 기록

### 주요 작업
- **TimingComboManager 리팩토링**
  - 판정 수치 현실적 기준으로 조정 (Perfect ±60ms / Good ±120ms)
  - `emitEvents` 매개변수 추가로 UI 출력 제어 가능
  - `Unavailable` 판정 도입 → 비트 루틴 미시작 시 일반 공격 처리

- **PlayerLocomotionState & PlayerComboState 연동**
  - 콤보 진입 여부 판단 시 `emitEvents: false` 적용하여 UI 중복 출력 방지
  - 파이널 콤보 종료 시점까지 애니메이션 끝나도록 처리
    - `AnimatorStateInfo.normalizedTime`로 마지막 타이밍 감지
    - `EndComboAfterDelay()` 제거하고 실시간 모니터링 방식으로 전환

- **ComboResult UI 시스템 구현**
  - `ComboResult_Area` → `ComboStack` → `ComboResultEntry` 구조로 HUD 상단에 표시되는 판정 텍스트 UI 구성
  - `ComboResultUIController`를 통해 스택처럼 위로 떠오르는 애니메이션 연출
  - `ComboResultEntry`: TextMeshPro + CanvasGroup 기반 / DOTween 연동
  - `ComboResultPool`: Pool<ComboResultEntry> 구조로 풀링 최적화 (초기 5개, 최대 6개)

- **UIManager → TimingComboManager 이벤트 연동**
  - OnPerfect / OnGood / OnMissed 이벤트 발생 시 `ComboResultUIController.ShowResult()` 호출
  - 람다 대신 명시적 핸들러 방식으로 이벤트 연결/해제 처리

- **외부 플러그인 적용**
  - **DOTween** 설치 및 Setup 완료 (Tools > Demigiant > DOTween Utility Panel)
  - 콤보 UI 연출 전반에 DOTween 사용

### 메모
- 콤보 결과 텍스트는 일반 타격마다 float-up 방식으로 최대 2~3개 정도 쌓여도 문제없도록 설계됨
- 파이널 콤보 연출은 ComboResultUI와 분리 예정 → 별도 Finish UI 구조 필요
- Miss 판정은 ComboState 내에서만 UI 출력되도록 구조 통제함
- DOTween 연출 구조는 앞으로 다른 UI 연출에도 재활용 가능 (ex. 토스트, 퀵슬롯, 피니시 등)

---

## 2025.10.01 (수) 작업 기록

### 주요 작업
- **EnemyHPUI UI 구성 및 연동**
  - World Space Canvas 기반 Slider UI 구성
  - 카메라 빌보딩 + 거리 기반 알파 적용
  - `SetHP()`, `SetFollowTarget()`, `SetOffset()` 기능 구현

- **Enemy.cs 내 HP UI 자동 연결 로직 구현**
  - 휴머노이드 본 또는 `"head"` 이름 포함 자식 트랜스폼 자동 탐색
  - `Resources.Load` 방식으로 프리팹 로드하여 연결

- **Transform 하위 조건 탐색 유틸 함수 작성**
  - `TransformUtils.FindChildRecursive()` 정적 함수 구현

- **Enemy 풀링 시스템 도입**
  - `Pool<Enemy>` 상속 구조 및 `EnemyPool.cs` 생성
  - `Enemy.ResetState()`, `Die()` → `Release()` 변경
  - FSM 상태 초기화용 `ResetToIdle()` 함수 도입

- **EnemyType 기반 다중 풀링 구조로 개선**
  - `EnemyPool` Singleton 제거
  - `EnemyManager`에서 `EnemyType` → `Pool` 매핑 Dictionary 구성
  - `SpawnEnemy()`, `ReleaseEnemy()` 함수로 스폰/반환 일원화

- **EnemySpawnPoint 스크립트 기본 구현**
  - `enemyType`, `spawnOnStart` 기반 적 스폰
  - `EnemyManager` 연동으로 타입 기반 적 생성

- **EnemySpawnPoint 기능 확장**
  - `spawnRadius`, `min/max count` 기반 랜덤 다중 스폰
  - `NavMesh.SamplePosition()` 기반 유효 위치 보정
  - 에디터 시각화(`OnDrawGizmosSelected`) 및 `UNITY_EDITOR` 전처리 처리

- **스폰 초기화 흐름 개선**
  - `EnemyManager.SpawnEnemy()`에서 `enemy.enabled = true → ResetState()` 순서로 초기화 흐름 명시
  - `Enemy.cs`의 `ResetState()` 내부에서는 `enabled = true` 호출 제거
  - 스크립트 활성화는 외부(EnemyManager)에서만 제어하도록 책임 분리

### 메모
- `Enemy.cs` → `SetupHPUI()`는 `Start()`에서 한 번만 호출하도록 유지
- `Enemy.cs` → `Release()`는 `EnemyManager`를 통해 수행 (직접 참조 제거)
- `EnemySpawnPoint`는 한 타입 고정 구조 유지 → 범위 겹치기로 혼합 연출 가능
- 웨이브 구조 확장 시 `SpawnGroup`, `WaveSpawner` 스크립트로 분리 예정

---

## 2025.10.02 (목) 작업 기록

### 주요 작업
- MirrorDuelist 클론 무적 처리 및 FSM 구조 개선
  - 클론 존재 중 본체 데미지 무효화 (HasActiveClones)
  - 클론 리스트 등록/해제 함수 도입
  - AttackState에서 소환 조건 검사 및 쿨타임 분기 처리

- FakeClone 풀링 도입 및 폭발 시스템 완성
  - FakeClonePool.cs 추가 (Pool<FakeClone>)
  - Instantiate 제거 후 풀에서 가져오는 방식으로 수정
  - FakeClone:
    - 수명 만료 시 자동 폭발
    - 폭발 시 주변 클론 연쇄 폭발 처리
    - 피격 시에도 즉시 폭발
    - NavMeshAgent 기반 추적 전환
    - Animator의 IsRunning 파라미터로 이동 애니메이션 제어
    - EnemyTimeController 연동 구조 반영

- ChronoMonk 발사체 풀링 적용
  - ChronoProjectilePool.cs 생성
  - Instantiate 제거, 풀에서 발사체 획득 후 Initialize로 세팅
  - 수명 초과 또는 충돌 시 풀로 반환

- EnemyTimeController 구조 개선
  - SetSpeed(float speed): baseSpeed 1회 등록 방식 도입
  - currentTimeScale에 따라 NavMeshAgent 및 Animator 속도 자동 조정

- Enemy.cs 전체 구조 정리
  - ResetState()에서 EnemyTimeController.SetSpeed(MoveSpeed) 호출로 통일
  - 시간 조작과 이동 속도 구조 일관성 확보

### 메모
- FakeClone은 FSM 없이도 시간 조작 + 연쇄 폭발 + 애니메이션까지 자연스럽게 처리되도록 구조화됨
- baseSpeed 구조 통일로 추후 다른 Enemy 계열에도 시간 조작 연동이 쉬워짐
- 연출 작업 시 FakeClone 예열 및 폭발에 이펙트/SFX/카메라 흔들림 추가 예정

---

## 2025.10.03 (금) 작업 기록

### 주요 작업

- MirrorDuelist 클론 소환 FSM 구조 분리 및 개선
  - MirrorAttackState 제거, EnemyAttackState로 공격 FSM 통합
  - 클론 소환은 ChaseState에서만 트리거되도록 변경
  - CanSpawnClone(), MarkCloneSpawned() 등 MirrorDuelist 내부 로직으로 쿨타임/상태 관리 분리
  - 소환 중에는 이동 정지 처리 (isSpawning), 애니메이션 종료 시 이동 재개

- Enemy FSM 통합 정리
  - EnemyAttackState에서 MirrorDuelist도 동일 FSM 사용
  - 공격 중엔 "Attack" 태그 애니메이션 진행 상태일 경우 상태 전이 방지
  - 애니메이션 종료 후에만 ChaseState로 전이되도록 안정성 개선

- 피격 시 강제 감지 유도 시스템 도입
  - Enemy.TakeDamage() 내부에서 DetectPlayer() 호출
  - hasDetectedPlayer 플래그로 중복 감지 방지
  - 상태와 무관하게 피격 시 추적 시작

- 시야 기반 감지 시스템 도입
  - Enemy.CanSeePlayer() 함수 구현 (시야각 + 거리 + 장애물 Raycast)
  - IdleState에서 거리 기반 감지 제거, 시야 기반으로 감지 전환
  - 감지 성공 시 DetectPlayer()로 상태 전이 및 전투 시작

- 시야각 시각화를 위한 Gizmos 표시 추가
  - 시야거리 원 + 부채꼴 시야각 라인 표시
  - MirrorDuelist, Watcher, ChronoMonk에서 공통 Gizmo 표시 적용
  - EnemyBehaviorData에 detectionAngle(기본값 150°) 추가

### 메모

- Enemy FSM 구조가 간결하게 통합되면서도, MirrorDuelist 특수 행동은 분기 처리로 유지됨
- 감지 시스템이 거리/피격/시야 기반으로 확장되며, 몰입감 있는 전투 흐름 구축 완료
- 시야각 Gizmo는 레벨 디자인/디버깅 시 시각적 피드백에 매우 유용
- 다음 작업: PatrolState 도입, 감지 연동, Waypoint 순회 로직 등

---

## 날짜: 2025.10.14 (화) 작업 기록

### 주요 작업
- **스폰 초기화 구조 개선**
  - `Enemy.ResetState()` 강화: 코루틴 정리, FSM/Agent/Animator/HP/UI/Collider/감지 플래그 초기화, `timeController.SetSpeed(MoveSpeed)` 복구
  - `EnemyManager.SpawnEnemy()`: **Warp(+회전 보정) → enabled=true → ResetState()** 순서 일원화로 경로 꼬임/풀 잔상 방지

- **리스폰 이벤트/관리 정비**
  - `Enemy.cs`에 `OnDied`, `OnDespawned`(+중복 가드) 추가
  - `Die()`에서 **OnDied 1회** 발행 후 릴리즈 코루틴, 등록해제는 **OnDisable 경로로 단일화**
  - 풀 반환 직전 **OnDespawned 1회** 발행

- **EnemySpawnPoint 리스폰 로직 보강**
  - 조건: `allowRespawn / respawnCooldown / maxAlive / maxPerCount / minPlayerDistance`
  - 쿨다운: **실제 스폰 발생 시에만** `ScheduleNextRespawn()` 갱신(초기 스폰 시 1회 시작)
  - 위치 샘플링: `NavMesh.SamplePosition` **랜덤 3회 + 중심 폴백**

- **패트롤 시스템 도입 & 주입 구조**
  - `PatrolMode`: `None / RandomInRadius / WaypointsLoop`
  - `Enemy.PatrolConfig` + `ApplyPatrolConfig(in cfg)`로 **스폰포인트→Enemy 런타임 오버라이드**
  - `patrolPointsRoot` 자식 트랜스폼 **자동 수집**으로 웨이포인트 관리 단순화
  - `RandomAround(center, radius)` 내부에 **NavMesh 샘플/폴백** 적용

- **FSM 전환 자연화**
  - **Idle → Patrol**: `PatrolMode != None`이면 **0.3s 지연 후** 자동 진입(플레이어 감지 시 기존 Detect→Chase 유지)
  - **Patrol → Chase**: `CanSeePlayer()` 시 즉시 전환
  - **Chase → Patrol/Idle**: **시야 연속 상실(LOST_SIGHT_TIME)** 로만 복귀(거리 리쉬 제거), `PatrolMode`에 따라 복귀 대상 결정

### 메모
- 동일 스폰포인트 기준 **사망→쿨다운→리스폰**, **Patrol→Chase→(시야 상실)→복귀** 잔상/중복 스폰/전이 튀는 현상 없음 확인  
- 동선 겹침 이슈는 **레벨 단위 운영** 예정
  - 병목 지형: 해당 스폰포인트만 `maxAlive=1`
  - 넓은 공간: 다중 허용
- `LOST_SIGHT_TIME`는 맵에 맞춰 수정 여지 있음

---

## 2025.10.15 (수) 작업 기록

### 주요 작업
- **모델/프리팹**
  - 플레이어 메시 분리: `body_mesh(가슴+다리)` / `arms_mesh(양팔)` FBX 임포트 및 적용
  - `Player` 프리팹 SkinnedMeshRenderer → `body_mesh`로 교체
  - `FP_Arms_Sword` 프리팹 추가(Layer=`FirstPerson`) 및 총기류 무기 레이어도 `FirstPerson`로 정리

- **카메라**
  - `FPCamera` 생성: MainCamera 자식 **Overlay**로 설정, **URP Camera Stack**에 추가
  - CullingMask 분리: Main=월드(`FirstPerson` 제외), FP=`FirstPerson` 전용
  - FPCamera **Near Clip=0.01**, FP 팔/무기 **Cast/Receive Shadows 비활성**
  - `CameraController`에 `fpCamera` 참조 추가 → **Start/Update/Reset에서 Main/FP FOV 동기화**

- **애니메이터/전투**
  - `SwordAnimator` 컨트롤러 생성(기본 틀)
  - `MeleeWeaponController`가 **플레이어 Animator 대신 자체 Animator(SwordAnimator)** 사용하도록 전환
  - (준비) 애니메이션 이벤트 바인딩 계획 수립

- **무기 장착 시 가시성/섀도우 전환**
  - `WeaponManager`에 월드 팔(`arms_mesh`) **ShadowCastingMode 토글** 로직 추가  
    - 장착(drawn): `ShadowsOnly`  
    - 해제: `On`

- **애니메이션 이벤트 책임 이관**
  - 근접 공격용 **애니메이션 이벤트 호출 대상을 PlayerManager → MeleeWeaponController로 재바인딩**(정리/주석 포함)

### 메모
- FP/월드 렌더 분리는 **Main에서 `FirstPerson` 제외**, **FPCamera는 `FirstPerson`만**으로 유지
- FOV는 부모-자식으로 상속되지 않으므로 **양 카메라 동기화**를 계속 보장할 것
- 다음 단계: `SwordAnimator` 상태/트리거(Idle/Light/Heavy/Combo) 구성 및 **클립 이벤트(Impact/HitStop/CameraShake) 바인딩**

---

## 2025.10.16 (목) 작업 기록

### 주요 작업
- **공격 및 콤보 시 카메라 쉐이킹(임팩트) 효과 추가**
  - `CameraController`
    - `PlayImpactShake(float intensity, float duration)` 함수 추가
    - 이동 보블(bob)과 임팩트 쉐이크를 통합 적용하도록 LateUpdate 로직 개선
    - 조준 중엔 임팩트 강도 50% 감쇠 처리
  - `MeleeWeaponController`
    - `OnMeleeAttackHit()` 시 공격 임팩트 쉐이크 호출 (`0.06f / 0.1f`)
    - `OnComboAttackHit()`에서 피니시 히트 시 강도 높은 쉐이크 호출 (`1f / 0.1f`)
    - 인스펙터에서 쉐이크 강도·지속시간 튜닝 가능

- **조준(ADS) 시 카메라·무기 흔들림 및 이동속도 감쇠**
  - `CameraController`: 조준 중 카메라 흔들림 진폭 0.3배 축소
  - `WeaponHolderSway`: 조준 중 무기 흔들림 진폭 0.3배 축소
  - `PlayerController`: 조준 상태 시 이동 속도 0.6배로 감소

- **무기/카메라 이동 쉐이킹(보블) 효과**
  - `CameraController`: 걷기·뛰기 속도에 따라 좌우·상하 보블 적용, 정지 시 복귀 처리
  - `WeaponHolderSway`: 이동 속도 기반 무기 좌우 흔들림 구현, 걷기/뛰기 진폭 차등 적용

- **콤보 시스템 리팩터링**
  - `PlayerComboState`: 플레이어 Animator 직접 제어 제거 → 무기 전용 Animator 제어로 전환
  - `MeleeWeaponController`: 콤보 애니메이션 재생, 속도 제어, 종료 판정 API 일원화
  - Idle 복귀, 속도 초기화, 널 가드 등 종료 루틴 보강

- **검 전용 애니메이터 적용**
  - `SwordAnimator`에 콤보 및 기본 공격 애니메이션 클립 배치
  - 이벤트를 `MeleeWeaponController`로 재바인딩
  - Light/Heavy 공격 및 콤보 트랙 연결, 테스트 완료

### 메모
- 공격 직후에도 쉐이크가 즉시 반영되도록 `LateUpdate()` early return 제거 → 정상 작동 확인  
- 콤보 피니시 시 강한 쉐이크와 일반 공격 시 약한 쉐이크의 강도 차이 뚜렷하게 체감됨  
- 조준 중 감쇠로 시점 안정감 확보, 이동·무기 리듬감 유지됨

---

## 2025.10.17 (금) 작업 기록

### 주요 작업
- **Watcher 전용 Upper Layer 분리 및 피격 리액션 구조 구성**
  - Animator에 Upper Layer 추가 (Spine~Head 중심 AvatarMask 적용)
  - Base Layer: 이동·공격·사망 등 전신 애니메이션 유지
  - Upper Layer: 상체 전용 Hit 리액션 재생 전용 상태머신 구성 (Empty ↔ Hit)
  - Avatar Mask 설정으로 하체 이동 유지 + 상체 플린치 자연스럽게 구현

- **ChronoMonk 스턴 파라미터 및 전이 조건 추가**
  - Bool 파라미터 `IsStunned` 추가
  - `Hit` 진입/유지/복귀 전이에 `IsStunned` 조건 적용
  - `Move`·`AttackSM` 전이 경로에 스턴 가드 반영

- **MirrorDuelist 제너릭 리그용 Upper Layer 및 아바타 마스크 적용**
  - 제너릭 스켈레톤 기반 UpperBodyMask Generic 신규 생성 (Spine~Head만 활성)
  - Animator에 Upper Layer 추가 및 마스크 지정
  - Base Layer: 이동·공격·사망 전신 애니메이션 유지
  - Upper Layer: 상체 전용 Hit 리액션 전담 (Empty ↔ Hit 상태 구성)

- **MirrorDuelist 피격 처리 보완 및 Hit 지속시간 조정**
  - EnemyHitState의 `hitDuration`을 0.20f → 0.22f로 조정하여 ChaseState 전환 텀 보강
  - MirrorDuelist에 누락된 `TakeDamage()` 기능 복원 및 FSM 전환 로직 정상화
    - 클론 존재 시 무적 처리
    - 공격/소환 중 피격 시 HitState 진입 제한
    - HP 0 이하 시 DeadState로 전환

- **VFXManager 도입 및 풀 기반 VFX 스폰/반환 구현**
  - VFXManager: 키→풀 매핑, Spawn API, 자동 반환 지원 (Init/Singleton)
  - VFXPool: Transform 풀에 파티클 정리(OnBeforeRelease) 로직 추가
  - CoreBootstrap: Presentation 단계에서 VFXManager.Initialize() 연동

- **스윙 VFX를 애니메이션 히트 시점에 재생**
  - MeleeWeaponController.OnMeleeAttackHit에서 `vfxSpawnPoint` 기준으로 `"Swing"` 스폰
  - `vfxSpawnPoint` 필드 추가 및 사용 (무기 하위 빈 오브젝트)
  - VFXManager 및 VFXPool 테스트 완료

### 메모
- 보스 전용 파이널콤보컨트롤러 기획은 주말~다음주 초로 보류
  - 잡몹과 달리 풀스턴 없이 마이크로 스태거 + 짧은 슬로우 중심 구조로 구상 중
  - IStatusEffectable 인터페이스를 활용한 상태 적용 설계 검토 예정

※ 테스트 이후 보스 전용 파이널콤보컨트롤러는 넣지 않는 것으로 결정

---

## 2025.10.21 (화) 작업 기록

### 주요 작업
- 스타트 컷씬 연출 완성
  - 눈 깜빡임 → 시스템 대사 → 카메라 애니메이션(기상·둘러보기·일어서기) → 플레이어 복귀 시퀀스 구현
  - Animator를 **UnscaledTime / AlwaysAnimate**로 설정하여 타임스케일 0에서도 정상 재생되도록 수정
  - **CINE_StartCutscene** 카메라를 활성화하여 컷씬 블렌딩 및 페이드 연출 완성
  - 자막 종료 타이밍(`SubtitleUI.IsPlaying`)을 컷씬 진행 조건으로 연결하여 대사 완료 시점에 맞춰 연출 전환

- UI / 페이드 / 컷씬 시스템 통합
  - `BaseCutscene`을 통한 공통 흐름 관리 (입력 차단, HUD 숨김, 컷씬 모드 전환)
  - `FadeUI`를 활용한 블랙스크린 및 눈 깜빡임 연출 추가
  - `UIManager`에 `ShowSubtitleAuto` / `ShowSubtitleHold` 중계 함수 추가로 컷씬 내 자막 호출 간소화

- SubtitleUI 시스템 정비
  - 큐 기반 자막 재생 구조 확립 (`SubtitleMode { Auto, Click }`)
  - CanvasGroup 페이드 인/아웃을 **언스케일드 시간**으로 처리
  - 클릭 입력(좌클릭 / Space) + 디바운스 로직 구현
  - `IsPlaying` 프로퍼티로 재생 상태 감지 가능하도록 개선

- 타임스케일 및 블렌드 관련 수정
  - `CutsceneState` 종료 시 항상 **타임스케일 1f**로 복구되도록 보정
  - Cinemachine **Ignore Time Scale** 옵션 활성화로 컷씬 블렌드 정상 작동

### 메모
- 컷씬용 Animator는 반드시 `UnscaledTime` + `AlwaysAnimate`로 설정해야 함
- CinemachineBrain의 **Ignore Time Scale** 옵션을 기본 활성화하여 timeScale=0에서도 블렌드 유지
- 스타트 컷씬에서 카메라 연출, 페이드, 자막이 모두 연결되어 최종적인 플로우 완성

---

## 2025.10.22 (수) 작업 기록

### 주요 작업
- **보스 각성 컷씬 구현**
  - `BossAwakeningCutscene` 스크립트 추가
    - **OnBeforePlay**: Animator를 `UnscaledTime / AlwaysAnimate`로 강제, 플레이어 비활성화, 컷씬 카메라 전환
    - **RunSequence**: 블렌드 종료 대기 → 보스 심장 이동(`MoveToBoss`) 애니메이션 재생 및 완료 대기 → 보스 `Idle` 트리거 → 컷씬 종료
    - **OnAfterPlay**: 애니메이터 설정 원복
    - **OnComplete**: 플레이어 재활성화 처리
  - 타임스케일 0 상태에서도 정상 재생되도록 언스케일드 시간 기반으로 구성
  - `StartPlay()` 외부 호출 함수 추가 (보스 제단 등에서 컷씬 트리거 가능)
  - `BossAltar`에서 `BossAwakeningCutscene.StartPlay()` 호출 연결

- **보스 심장부 애니메이터 구성**
  - `BossHeartAnimator.controller` 생성 및 **심장 이동/장착 애니메이션 클립(`MoveToBoss`)** 추가
  - 컷씬 내에서 보스 각성 연출과 동기화되도록 설정

- **보스 StartIdle 애니메이션 1차 제작**
  - `Boss_StartIdle.anim` 추가 (무릎 꿇은 대기 자세)
  - 보스 Animator Controller에 `StartIdle` 스테이트 구성
  - 향후 Intro/Idle 전이 및 루프 보정 예정

- **컷씬 공통 구조 개선**
  - `BaseCutscene`에 `try/finally` 적용 → `OnAfterPlay`가 항상 호출되도록 보장
  - 탐험 상태 복귀 시점을 **카메라 블렌드 완료 시점**으로 통일
  - `WaitAnimDone`, `ForceUnscaledAnimators`, `RestoreAnimators` 유틸 함수를 `BaseCutscene`으로 이동
  - `StartCutscene`의 Animator 모드 강제/원복을 `OnBeforePlay/OnAfterPlay`로 이전

- **게임 상태 복귀 로직 정리**
  - `GameManager.EnterPreviousState`에서 이전 상태가 `CutsceneState`일 경우 자동으로 `ExplorationState`로 복귀하도록 변경
  - `StartCutscene`에서 직접 탐험 상태로 복귀하던 코드를 제거하여 중복 로직 해소
  - `LoadingState`의 테스트용 시작 씬을 `Chapter_1`로 설정 → 로딩 → 탐험 → 스타트 컷씬 → 탐험 복귀 흐름 정상화

### 메모
- 보스각성 컷씬에서는 **심장 이동 및 보스 각성 애니메이션**을 전부 Animator 기반으로 제어해 안정적임
- `EnterPreviousState()` 수정 덕분에 모든 컷씬 종료 후 탐험 복귀 흐름이 자동으로 정리됨
- 향후 보스 인트로 컷씬(`BossIntroCutscene`)에서 전투 시작 전 연출만 추가하면 챕터2의 전체 플로우 완성 예정

---

## 2025.10.23 (목) 작업 기록

### 주요 작업
- **보스 인트로 컷씬 구현 및 FSM 연동**
  - `BossIntroCutscene` 신규 구현 (`BaseCutscene` 상속)
  - `OnBeforePlay`에서 카메라 전환 및 플레이어 비활성화 처리
  - `RunSequence`에서 `BossController.StartIntroState()` 호출 후 **애니 길이 기반 실시간 대기**로 전환
  - `OnAfterPlay`에서 애니메이터 복원 및 플레이어 재활성화
  - `CutsceneCameraManager` 블렌드/복귀 루틴 정상 동작 확인

- **플레이어 표시 및 보스 인트로 상태 제어 기능 추가**
  - `PlayerManager.ShowPlayerBody(bool)` 추가: 플레이어 모델 활성/비활성 안전 토글
  - `BossController.StartIntroState()` 추가: FSM 초기화 진입점
  - `BossAwakeningCutscene`, `StartCutscene`에 `ShowPlayerBody()` 반영(컷씬 중 비활성화 → 종료 후 복원)

- **보스 인트로 트리거 및 컷씬 실행 로직**
  - `BossIntroTrigger`: 플레이어 진입 시 `BossIntroCutscene.StartPlay()` 호출, 1회성 실행 위해 트리거 비활성화
  - `BossIntroCutscene`: `WaitAnimDone` 대신 `bossController.GetCurrentAnimationLength()` 기반 대기로 **EndCutscene 미호출 문제 해결**
  - 언스케일드 애니메이터 처리 및 시네머신 전환 유지

- **보스 페이즈 전환 컷씬 + 퍼즐 연동**
  - `BossPhaseTransitionCutscene`: 인트로 구조 재사용, **VFX 재생 + 애니 길이 기반 대기** 후 `EndCutscene()` 처리
  - `BossPhaseTransitionState`: 동일 Intro 클립 재생 후 대기 → Idle 전환, `Exit`에서 Phase2/HUD/Combat 진입
  - `PuzzlePhase1State`: 퍼즐 완료 시 `boss.TC.StartPlay()`로 전환 컷씬 실행
  - `BossController`: `BossPhaseTransitionCutscene` 참조 추가 및 `StartPhaseTransitionState()` 제공

- **유니티 버전 업그레이드**
  - Unity **6000.0.58f2 → 6000.0.60f1**
  - VFX 메모리 누수(`JobTempAlloc`) 경고 해결, 파티클/VFX 생성 시 안정성 개선

### 메모
- `WaitAnimDone()`가 레이어/전이 이슈로 대기 해제되지 않던 문제를 **애니 길이 기반 대기**로 우회하여 안정화
- 인트로/페이즈 전환은 **같은 애니 클립 재사용**, FSM/컷씬 역할 분리로 전투 흐름 유지
- `CutsceneCameraManager`의 블렌드 대기/복귀 구조가 모든 컷씬에서 일관적으로 정상 작동
- 다음 목표: **EndingCutscene** 구현 및 `GameManager` 복귀 흐름 테스트

---

## 2025.10.27 (월) 작업 기록

### 주요 작업
- **보스 엔딩 및 심장부 종료 연출 애니메이션 제작**
  - 보스 애니메이터에 `Ending`, `HeartEnding` 클립 추가
  - `Ending`: 심장부 노출 및 이동 연출 포함, 시네머신 카메라 이동 타이밍 반영
  - `HeartEnding`: 심장부 기능(에너지 코어) 단계적 비활성화 연출
    - 내부 발광 및 회전 모션 순차 정지
    - 각 기능이 하나씩 꺼지며 ‘시간의 심장’이 멈추는 느낌 구현

- **보스 엔딩 컷씬 및 엔딩 전환 로직 구현**
  - `BossEndingCutscene` 전체 구성 완료 (`Ending → HeartEnding → Ending2`)
  - 자막 연출 및 페이드아웃 후 `GameManager.EnterEnding()` 호출로 타이틀 씬 복귀
  - 보스 및 심장부 애니메이터 언스케일드 처리, 시네머신 카메라 블렌드 안정화
  - 엔딩 컷씬 중 대사:
    ```
    "시간의 신전이 멈췄다."
    "그 속에서 모든 소리가 사라졌다."
    "남은 것은, 흐름을 거스른 자의 흔적뿐이었다."
    "나는 이 흐름 속에, 영원히 머물게 될 것을 직감했다."
    ```

- **엔딩 전환 시스템 추가**
  - `GameManager`
    - `EnterEnding()` 함수 추가
    - `LoadingState.NextLoadingMode = Ending` 설정 후 로딩 스테이트 진입
    - 엔딩 종료 시 타이틀 씬 자동 복귀
  - `LoadingState`
    - `LoadingMode`에 `Ending` 항목 추가
    - `Ending` 모드 시 `"Title"` 씬 자동 로드 처리

- **UI 및 이벤트 시스템 정리**
  - `EventSystem`을 **Core 프리팹**으로 이동하여 전역 단일 관리 구조 확립
  - 개별 UI 프리팹 내 EventSystem 제거 → 씬 전환 시 중복 생성 경고(`There are 2 event systems`) 해소

- **보스전 자동 저장 로직 개선**
  - `CutsceneCameraManager.EndCutscene()`에 `autoSave(bool)` 매개변수 추가 (기본값 `false'`)
  - 보스전 컷씬 종료 후 자동 저장 비활성화를 위해 `autoSave=false` 설정 가능
  - 전투 중 컷씬 종료 시 불필요한 저장 호출로 인한 상태 전이 꼬임 문제 해결

### 메모
- 엔딩 컷씬 완성 → 게임 전체 흐름(탐험 → 보스전 → 엔딩 → 타이틀) 정상 순환 확인  
- 보스전 중 컷씬 저장 충돌 제거로 상태머신/세이브 동기화 안정화  
- 다음 목표: **FinalChapterIntroCutscene 구현 및 챕터 진입 자막 연출 추가**

---

## 2025.10.29 (수) 작업 기록

### 주요 작업
- **파이널 챕터 인트로 컷씬 구현**
  - `FinalChapterIntroCutscene` 신규 구성 (BaseCutscene 상속)
  - 씬 전환 직후 자동 실행 구조로 변경 (Start() → LoadingState.OnSceneLoaded() 호출)
  - 블랙 화면 → 페이드 인 → 카메라 연출(Final_Intro 애니메이션) → 자막 출력 순으로 진행
  - 자막을 플레이어 내면 독백 형태로 수정  
    - "…여긴 대체 뭐지?"  
    - "공기가… 멈춰 있는 것 같아."  
    - "일단, 안으로 들어가보자."
  - 카메라 애니메이션 및 Blink 효과로 자연스러운 장면 전환 연출 구현

- **로딩스테이트 기반 컷씬 자동 실행 구조 정비**
  - `LoadingState.OnSceneLoaded()`에서 씬별 컷씬 자동 호출
    - `Chapter_1` → `StartCutscene.StartPlay()`  
    - `Chapter_Final` → `FinalChapterIntroCutscene.StartPlay()`
  - `GameManager.EnterFinalChapter()` 함수 추가  
    - `NextLoadingMode = SceneTransition`, `NextSceneName = "Chapter_Final"` 설정 후 `EnterLoading()` 진입  
    - 로딩 완료 시 파이널 챕터 인트로 컷씬 자동 재생
  - `StartCutscene`의 Start() 자동 실행 제거 → 로딩스테이트 기반으로 일원화

- **씬 시작 시 플레이어 위치 초기화 구조 통합**
  - `StartCutscene` / `FinalChapterIntroCutscene`
    - `sceneStartPoint(Transform)` 기준으로 플레이어 위치 및 회전 세팅
    - `PlayerController.SetPositionAndRotation()` 호출로 정확한 시작 위치 반영
    - 컷씬 카메라 활성화 전에 위치 보정해 화면 튐 방지
  - `PlayerController`
    - 함수명 오타 수정: `SetPositionAndRotaion → SetPositionAndRotation`
    - 기존 안전 이동 로직(Controller 비활성 → 위치 세팅 → 재활성) 유지

- **HUD 표시 및 상태 전환 흐름 개선**
  - `UIManager.UpdatePlayerHud()`  
    - HUD 숨김 시 퀵슬롯 패널도 함께 비활성화되도록 수정  
    - HUD 비활성화 시 퀵슬롯만 남는 문제 해결
  - `GameManager`  
    - Start()에서 불필요하게 `EnterExploration()` 호출하던 로직 제거  
    - 컷씬/로딩 중 HUD가 강제로 표시되던 문제 수정
  - `LoadingState`  
    - OnSceneLoaded()에서 `GameManager.EnterExploration()` 호출 제거  
    - 컷씬 종료 시점에서 탐험 상태로 복귀하도록 흐름 정리  
    - HUD 활성화 시점을 컷씬 종료 시점으로 일원화

### 메모
- 씬 전환 → 컷씬 → 탐험 복귀 전체 루프 정상 작동 확인  
- 플레이어 위치, 카메라 전환, HUD 표시 상태 모두 일관성 확보  
- 불러오기·로딩 시 중복 컷씬 재생이나 HUD 오작동 문제 제거  
- 다음 단계: 챕터1~2 간 **게임 흐름 배치(맵 구성, 카드키·문 퍼즐, 상점 배치 등)** 우선 진행

---

## 2025.11.03 (월) 작업 기록

### 주요 작업
- **상점 시스템 개선**
  - 챕터1 맵에 상점 모델 프리팹 적용
  - 상점 로고 회전 애니메이션 구현 및 연출 보강
  - 기존 상점 스크립트 연결 후 상호작용 동작 검증
  - 포션 상점 1개, 탄약 상점 1개 배치 완료

- **적 스폰 및 내비메쉬 구조 통합**
  - EnemyPools 프리팹 생성으로 에너미 관리 구조 분리
  - EnemySpawnPoint 프리팹화로 스폰 위치 관리 일원화
  - 챕터1 맵 내 몬스터 스폰 포인트 1차 배치 완료
  - 경사로 및 벽 일체형 구간에 BoxCollider 추가하여 이동 경로 보정
  - 챕터1 전체 영역에 NavMeshSurface 구성 및 베이크 완료
  - NavMesh 및 EnemyPools 구조를 통합해 챕터1 적 배치 및 경로 탐색 안정화

- **에너미 AI 시야 로직 개선**
  - Enemy.cs에 `HasClearShotToTarget()` 함수 추가로 공격 중 명확한 시야 판정 분리
  - 기존 `CanSeePlayer()`의 `hasDetectedPlayer` 플래그 영향 제거를 위해 전투용 별도 로직 적용
  - ChronoAttackState에 `HasClearShotToTarget()` 반영하여 장애물 가림 시 추격 상태로 전환
  - 감지(Detect)와 전투(Attack) 시야 로직 분리로 층간 인식 및 오탐 문제 개선

### 메모
- 크로노몽크 시야 로직 수정 후 전투 상태 안정화 확인
- 챕터1 맵 구조 정리 및 내비메쉬 재정비로 이동 경로 오류 해결
- 파이널 챕터 작업은 챕터1 흐름 완성 후 착수 예정

---

## 2025.11.04 (화) 작업 기록

### 주요 작업
- **문 제어 및 컷씬 연동 구조 개선**
  - DoorController에 `isUnlocked` 플래그 추가로 문 해금 상태 제어
  - `OnConditionMet()`에서 다음 문 해금 및 자동 개방 처리
  - 다중 문 해금 지원을 위해 DoorController 배열 구조로 확장
  - 컷씬 종료 시 OnConditionMet() 호출로 자동 문 해금 구현
  - 코드 일관성 향상 (isOpen 상태를 OpenDoor/CloseDoor 내부에서 관리)

- **아이템 및 상호작용 개선**
  - ItemPickup에 `onPickupSuccess` 이벤트 추가  
    → 아이템 완전 획득 시 이벤트 호출로 다음 문 해금 연동  
    → 인스펙터에서 DoorController.OnConditionMet() 다중 바인딩 가능
  - ItemPickup 프롬프트 표시 조건에 콜라이더 활성 상태 반영  
    → 콜라이더가 없거나 비활성화된 경우 프롬프트 비표시로 변경

- **퍼즐 및 키카드 시스템 개선**
  - KeycardMover에 시간 상태 기반 픽업 제어 추가  
    → TimeManager.CurrentTimeState가 Normal일 때 키카드 콜라이더 비활성화  
    → 시간 조작 중에만 픽업 가능하도록 변경  
  - KeycardMover의 이동 루틴을 누적(acc) 기반 타이머 방식으로 변경  
    → 시간 정지 시 즉시 멈추고, 재개 시 자연스럽게 이동 이어짐  
    → "정지 상태에서 다음 위치로 이동 후 멈추는 문제" 수정

- **능력 해금 시스템 도입**
  - TimeManager에 시간 스킬 해금 플래그(`unlockedSlow/Stop/Rewind/FastForward`) 추가  
    → 해금된 스킬만 사용 가능하도록 구조 개선  
    → 허용되지 않는 상태일 경우 자동 Normal 복귀 처리  
  - PlayerManager에 대쉬 해금 플래그(`canDash`) 추가  
    → UnlockDash/LockDash 메서드 제공, LocomotionState에서 입력 게이트로 활용  

- **AbilityUnlockTrigger 구현 및 맵 반영**
  - AbilityUnlockTrigger 스크립트 추가 (트리거 진입 시 능력 자동 해금)
    - TimeSlow, TimeStop, TimeRewind, TimeFastForward, Dash 해금 지원
    - 해금 시 UI 토스트 출력 및 오브젝트 자동 제거
  - 시간실(Time Room): TimeSlow, TimeStop 해금 트리거 배치
  - 감시실(Surveillance Room): TimeRewind, TimeFastForward 해금 트리거 배치
  - 각 방의 키카드와 문 연결  
    → 카드 습득 시 다음 방 문 자동 개방
  - 챕터1 루프 완성: **능력 해금 → 카드 획득 → 문 개방 → 다음 방 진입**  

### 메모
- 시간 능력 및 대쉬 해금 로직이 통합되면서 챕터1 전체 흐름 안정화  
- 키카드 Normal 상태 차단 및 정지 시 이동 문제 해결로 퍼즐 안정성 향상  
- 컷씬, 검 픽업, 카드 획득, 능력 해금이 전부 DoorController 연동 구조로 통합됨

---

## 2025.11.05 (수) 작업 기록

### 주요 작업
- **CCTV 감지 기반 전투 구조 리팩토링**
  - CCTV 감지 시 `EnemySpawner` 의존 제거 후 **`EnemySpawnPoint` 풀링 구조**로 전환  
  - 감시실 CCTV 감지 시 `EnemySpawnPoint.TrySpawnEnemies()` 호출 → **풀링된 Watcher 소환**  
  - 스폰된 적들은 CCTVPlayerDetector에서 **`DetectPlayer()`** 호출로 즉시 플레이어 추적  
  - 스폰/추적 로직을 완전히 분리하여 **유지보수성 및 재사용성 향상**

- **Enemy FSM 안정화**
  - FSM의 `currentState`가 `null`일 때만 IdleState로 전환하도록 조건 보완  
  - IdleState / PatrolState 진입 시 **`ResetDetection()`** 호출로 감지 플래그 초기화  
  - Chase → Idle 복귀 후에도 플레이어 재인식 정상화  
  - 스폰 후 감지 인식 누락 및 초기화 불일치 문제 해결

- **홀로그램 로그 시스템 구현**
  - `SubtitleUI.Open()`에 **텍스트 정렬 매개변수 추가** (중앙 / 왼쪽 정렬 지원)  
  - `UIManager.ShowSubtitleAuto()`, `ShowSubtitleHold()`에서 정렬 인자 전달 가능  
  - **`HologramLogTerminal` 스크립트 구현 및 적용**  
    - 기록실 홀로그램 단말 오브젝트와 연동  
    - 상호작용 시 SubtitleUI 좌측 정렬 로그 출력  
    - 재생 완료 후 문 해금 및 상호작용 종료 처리  
  - 기록실 내 세계관 로그 연출(Subject-09 / ChronoCore 실험 기록) 완성

- **기록실 샷건 보상 및 보급실 문 연동**
  - 기록실에 **샷건 아이템 프리팹** 배치  
  - 샷건 획득 시 **보급실 문 자동 개방** (`DoorController.OnConditionMet()` 호출)  
  - 로그 → 샷건 획득 → 보급실 진입으로 이어지는 전투 준비 흐름 완성  

---

### 메모
- 감시실 CCTV 감지 구조를 풀링 기반으로 완전히 교체하면서 **퍼포먼스와 구조 일관성 확보**  
- Enemy FSM 감지 플래그 초기화 문제 해결로 **AI 추적 루프가 안정화됨**  
- 기록실 홀로그램 로그 및 샷건 보상까지 완성되어 **챕터1 후반부 흐름 정리 완료 단계**  
- 내일 남은 작업:  
  - **보급실 카드키 연동 / 대쉬 해금 트리거 구성**  
  - **전투실 웨이브 클리어 → 신전(파이널 챕터) 전환 트리거 연결**

---

## 2025.11.06 (목) 작업 기록

### 주요 작업
- **보급실 키카드 도어 연동 및 보상 배치**
  - `KeycardDoor`에 `colToActivate` 참조 연결  
    - 블루 도어 → **대쉬 언락 트리거 콜라이더**  
    - 옐로 도어 → **라이플 픽업 콜라이더**  
  - 각 문에 애니메이션 상태(Open/Closed) 매핑 및 키카드 보유 시 개방 처리  
  - 문 뒤에 대쉬 언락 트리거와 라이플 아이템을 배치하여 **보상 루프 완성**
  - `ApplyActivated()`에서 `colToActivate.enabled = activated` 동기화  
    - 세이브/로드 시 문 상태와 보상 콜라이더 상태가 일관되도록 수정

- **전투실 포탈 트리거 구현**
  - `PortalTrigger` 스크립트 추가  
    - 참조된 `EnemySpawnPoint`들의 `ActiveEnemies`가 모두 0일 때 포탈 자동 활성화  
    - `InvokeRepeating()` 기반의 주기적 상태 검사 구조 적용  
    - 포탈 활성화 시 비주얼/콜라이더 동기화 및 반복 검사 중단  
  - 플레이어가 포탈 트리거에 진입 시 **`GameManager.EnterFinalChapter()` 호출**  
  - 전투실 웨이브 종료 → 포탈 등장 → 파이널 챕터 이동의 흐름 완성

---

### 메모
- 보급실의 키카드-보상 루프(블루/옐로 도어)와 전투실 포탈 트리거까지 완성되어  
  **챕터1 전체 진행 루프가 사실상 완성 단계**에 도달함  
- 내일은 폴리싱 전 점검 단계로,  
  - 각 방 전환 시 컷씬/이펙트 보완,  
  - 포탈 활성화 시 연출(빛, 카메라 컷 등) 추가 검토 예정  
- 폴리싱 단계에서는 포탈 활성화 컷씬 → 씬 전환 컷씬으로 자연스럽게 이어지도록 구성할 계획

---

## 2025.11.10 (월) 작업 기록

### 주요 작업
- **GuideLightMover 및 퍼즐 매니저 연동 마무리**
  - `PuzzleProgressManager.OnKeyInserted` 이벤트를 **unlockMap 기반의 다음 방 roomId**로 발행하도록 수정
  - `GuideLightMover`는 해당 roomId가 자기와 일치할 때 자동으로 `StartGuide()` 실행
  - `OnEnable` / `OnDisable` 에서 이벤트 구독 및 해제 처리로 구조 안정화
  - `GuideLightsTrigger`를 통해 초기 팬아웃(4개 빛 동시 이동) 트리거 구현 완료
  - roomId=3 완료 시 1회 추가 이동 옵션 유지 (`repeatOnceIfRoom3`)
  - 전역 제어(static currentGuide) 완전 제거 → **동시 이동 가능**
  - 트리거 및 매니저 이벤트 흐름 정리 완료

- **LightPathController → Light 기반 연출로 전환**
  - 기존 `LineRenderer` 기반의 LightPathController는 테스트 결과 어색하여 제거
  - 대신 **빛(GuideLight) 이동형 에셋**으로 전환
    - 부드러운 이동 및 공간감 표현이 자연스러움
    - 포스트프로세싱 Bloom과 조합 시 시각적 완성도 향상
  - 각 빛 오브젝트는 `GuideLightMover`를 통해 경로(waypoints)를 따라 이동
  - 심장부 트리거 진입 시 4개 빛이 동시에 이동하며 팬아웃 연출
  - 퍼즐 클리어 후 키 삽입 시 unlockMap에 따라 다음 방으로 이동

- **PuzzleProgressManager 리팩터링**
  - `lastClearedRoomId` 필드 추가 및 `MarkCleared()`에서 갱신
  - `ReportKeyInserted()` 호출 시 `unlockMap[lastClearedRoomId]`를 참조해 다음 방 ID 발행
  - `OnAllCleared`는 기존 구조 유지
  - 라이트 시스템 외부 의존 없이 내부 이벤트만으로 가이드 연출 제어

### 메모
- `LightPathController`는 초기 구상(라인 기반 빛 경로)에서는 구현 완전했지만, 실제 씬에서는 3D 공간에서 어색하게 보여 **빛 오브젝트 이동형으로 교체**함
- `GuideLightMover` 구조는 단순하며 확장성 높음 코루틴 기반 이동 + Renderer/Light 제어가 핵심
- `GuideLightsTrigger`와 `PuzzleProgressManager.OnKeyInserted`의 이벤트 흐름이 명확히 분리되어 관리 용이

---

## 2025.11.11 (화) 작업 기록

### 주요 작업
- **파이널 챕터 NavMesh 설정 및 베이크 완료**
  - Temple_Environment에 `NavMeshSurface` 구성 및 베이크
  - Agent Type: Humanoid (Radius 0.5, Height 2.0, Max Slope 45°)
  - Use Geometry: Physics Colliders / Include Layers: Ground
  - Override Voxel Size(0.1)로 계단·단차 구간의 경계 끊김 해결
  - 베이크 후 적 AI 이동 정상 동작 및 계단 이동 테스트 완료

- **MirrorDuelist / FakeClone 지면 침투 문제 수정**
  - `CreateClones()`에서 `NavMesh.SamplePosition()`을 사용해 유효 지면 좌표 샘플링
  - `FakeClone.Initialize()` 내 `agent.Warp()` 적용으로 내부 좌표(nextPosition) 동기화
  - NavMeshAgent의 baseOffset / path / nextPosition 초기화로 Y축 오프셋 문제 해결
  - 클론 생성 시 지면 아래로 파고드는 현상 완전 수정

- **EnemySpawnPoint 기즈모 시각화 및 테스트용 스폰포인트 배치**
  - `OnDrawGizmosSelected()`에 패트롤 반경(파란색 와이어) 표시 추가
  - `overridePatrol && patrolMode == RandomInRadius` 조건에서만 표시
  - MirrorDuelist(웨이포인트 루프/랜덤), Watcher(RandomInRadius) 스폰포인트 각 1개씩 배치
  - 씬 내 반경 시각화 및 적 동작 정상 확인

- **이동형 상점 ShopOrbitMover 구현**
  - 상점이 중심점을 기준으로 일정 반경에서 **원형 공전**하도록 구현
  - 플레이어 접근 시(`stopRange` 이내) **정지 및 상호작용 대기**
  - Animator 파라미터(`IsInteract`)로 이동/정지 상태 전환
  - Gizmos 시각화 추가: 중심점, 궤도 원(오렌지), 정지 반경(파란색) 및 진행 방향 화살표 표시
  - 파이널 챕터 상점 오브젝트에 적용 후 정상 작동 확인

- **상점 정지 시 플레이어 응시 및 수직 보블 모션 추가**
  - `stopRange` 내 플레이어 감지 시 **플레이어 바라보기 로직 추가**
  - 공전 중엔 기존처럼 궤도 접선 방향 회전 유지
  - 사인 곡선 기반 수직 보블(부양) 모션 구현
    - `bobAmplitude`, `bobFrequency`, `bobMinScaleOnStop` 파라미터로 진폭/주기/정지 시 잔여 움직임 조절
  - 이동 중에는 부드럽게 떠오르며 공전, 정지 시엔 자연스럽게 응시하며 호흡하는 연출 완성

### 메모
- NavMesh는 파이널 챕터 전체 구간에서 끊김 없이 매끄럽게 이어지며, 계단/단차 문제 해결됨
- MirrorDuelist 클론의 지면 침투는 NavMeshAgent의 Warp 사용으로 구조적으로 해결됨
- 상점은 공전과 정지를 동시에 수행하며, 플레이어가 접근할 때 자연스럽게 응시하는 시각적 연출 완성
- EnemySpawnPoint 기즈모 시각화로 테스트 및 배치 효율성 향상

---

## 2025.11.12 (수) 작업 기록

### 주요 작업
- **EnemySpawnPoint 저장/복원 시스템 완성**
  - `EnemySpawnPoint`에 세이브 프록시용 브리지 및 유틸 함수 추가  
    - `AllowRespawn` 게터, `SpawnOnStart` 프로퍼티 추가  
    - `DespawnAllEnemies()` / `SpawnOneAt(Vector3 pos, float yaw)` 구현  
    - `NavMeshAgent.Warp()` 기반 정확 위치/회전 복원 및 풀 반환 구조 확립  
  - `EnemySpawnPointSaveProxy` 구현  
    - `allowRespawn == false`인 스폰포인트만 저장/복원  
    - 적의 위치/회전/HP비율을 JSON 스냅샷으로 저장  
    - 로드시 자동 스폰 차단 후 기존 적 제거 및 정확 좌표 재스폰  
    - `Enemy.SetHpRatio()`로 HP 비율 복원 (UI 연동 포함)  
  - `Enemy.cs` 개선  
    - `GetHpRatio()` / `SetHpRatio(float ratio, bool syncUi=true)` 추가  
    - HP UI 자동 동기화 옵션 지원  

- **PuzzleProgressManager 저장/복원 구조 도입**
  - `PuzzleProgressManagerSaveProxy` 구현  
    - `PPMData` 구조체(clearedRooms, unlockedRooms, keyCount 등) 직렬화  
    - `ToData()` / `ApplyDataOnly()` 구현, 이벤트 발행 억제  
    - 로드시 컷씬/사운드 없이 상태만 복원  
  - `PuzzleProgressManager`에 브리지 추가  
    - `ClearedRooms`, `UnlockedRooms` 읽기 전용 프로퍼티  
    - `_suppressEvents` 가드 및 무음 복원 모드 적용  

- **PuzzleRoomManager / PuzzleRoomSaveProxy 추가**
  - `PuzzleRoomManager`  
    - `RoomId`, `IsCleared`, `IsActivated` 게터 및 `ApplyState()` 구현  
    - `CacheInitialStates()` / `ResetToInitialIfUncleared()` 유지  
  - `PuzzleRoomSaveProxy`  
    - `RoomData(roomId, isCleared, isActivated)` 직렬화  
    - `RestoreStateJson()` 시 이벤트 없이 무음 복원  

- **PuzzleStateTrigger 구조 단순화**
  - `subscribed` 변수 완전 제거  
  - `TrySubscribeOrWait()`를 `OnEnable()`에서 호출하도록 변경 (지연 구독 지원)  
  - `Start()`에서 1회 `CacheInitialStates()` 및 초기 `OnAfterLoadCommon()` 실행  
  - `Update()`는 퍼즐 클리어 시 비주얼 정리만 담당  
  - 구독/해제 관리 경로를 `Start()`/`OnDisable()`로 일원화  

- **퍼즐 시작 스냅샷 제거 및 프리셋 재구성 방식으로 변경**
  - `PuzzleRoomManager`의 `startStates`, `CacheStartStates()`, `RestoreForLoad()` 제거  
  - `PuzzleStateTrigger`  
    - 로드시 `IsActivated`면 `ActivePuzzleRoomDoor()` / `ActivePuzzleObjects()` 호출  
    - 비활성 상태면 `ResetToInitialIfUncleared()`만 실행  
    - 퍼즐 입장 시 스냅샷 캐싱 삭제, 자동 저장 유지  
  - 결과적으로 퍼즐 시작→오토세이브→로드 시 동일한 프리셋 복원  

- **RewindableObjects 구조 단순화**
  - `isStarted` 필드 제거, `PuzzleRoomManager.IsActivated` 기반으로 통일  
  - 퍼즐룸 미존재 시 자동 비활성 폴백 처리  
  - 퍼즐 활성/비활성 상태에 따라 복원·콜라이더 제어 단순화  
  - `SnapToOriginalPose()` 추가  
    - `originalPositions` / `originalRotations` 기반 즉시 포즈 복원  
    - `Rigidbody` 속도 초기화 및 `Physics.SyncTransforms()` 호출  
    - 퍼즐 비활성 또는 로드 시점 오브젝트 리셋용으로 사용  

### 메모
- 퍼즐 시작 스냅샷은 유지 부담이 커서 제거, 로드시 퍼즐 활성 상태(`IsActivated`) 기준으로 비주얼 재구성  
- `RewindableObjects`는 퍼즐룸의 상태만 참고해 동작하도록 단순화  
- 전체 퍼즐 세이브/로드 체계가 PuzzleRoomManager의 논리 상태(`IsCleared`, `IsActivated`) 중심으로 통합됨

---

