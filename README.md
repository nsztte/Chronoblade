
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

## 4주차 목표
- [x] 콤보 파이널 스킬 구조화 및 상태이상 시스템 완성
- [x] AOE / 다단히트 / 리듬 기반 데미지 적용 통합
- [x] Player 피격/사망 FSM 상태 처리
- [x] 상점 시스템 + 인벤토리 연동 및 구매/판매 UI
- [x] 시간 시스템 완성 (정지/되감기/빨리감기)
- [x] 리와인드 보간 연출 기반 퍼즐 연계 테스트 준비

## 5주차 목표
- [ ] 보스 FSM 및 전투 패턴 구현
- [ ] 리듬 연동 보스 기믹 설계
- [ ] 시간 퍼즐 기초 2종 구현
- [ ] 컷씬 트리거 및 시네머신 연출
- [x] 게임 상태머신 시스템 구축
- [x] GameManager 및 상태 흐름 통합
- [x] TimeManager와 상태 동기화
- [ ] 핵심 UI 및 전투 HUD 구성

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
│   │   ├── GameStates/
│   │   ├── UI/
│   ├── Scenes/
│   └── Scripts/
│   │   ├── Enemy/
│   │   │   ├── FSM/
│   │   ├── Item/
│   │   ├── Player/
│   │   │   ├── FSM/
│   │   │   ├── Weapon/
│   │   ├── Systems/
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
  - Combat, Puzzle 상태 종료 시 자동으로 Exploration으로 복귀

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


## 관련 문서

- [Input_Structure_Design.md](./Docs/Input_Structure_Design.md) - 입력 구조 설계 문서