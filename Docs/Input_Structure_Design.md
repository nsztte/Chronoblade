
# Chronoblade Input System 설계 문서

이 문서는 Chronoblade 프로젝트에서 사용하는 입력 구조를 정의한 문서입니다.  
현재까지 구현된 내용 기준으로 구조적 설계를 정리합니다.

---

## 전체 구조 요약

| 컴포넌트            | 역할                                    |
|--------------------|-------------------------------------|
| InputManager     | 입력을 감지하고 이벤트로 발행     |
| PlayerController   | 이동, 점프 처리                       |
| CameraController | 마우스 시점 회전 처리               |

---

## InputManager.cs

"싱글톤 기반 입력 이벤트 발행자"로, 모든 입력은 이 클래스에서 처리되며, 각 시스템은 이벤트를 구독해서 반응합니다.

### 입력 항목 및 이벤트 목록

| 입력 | 이벤트 이름 | 설명 |
|------|-------------|------|
| WASD 이동 | `OnMoveInput(Vector2)` | `x`: 좌/우, `y`: 전/후 이동 |
| 마우스 이동 | `OnLookInput(Vector2)` | `x`: 좌우 회전, `y`: 상하 회전 |
| 스페이스바 | `OnJumpPressed()` | 점프 트리거 |
| 좌클릭 | `OnAttackPressed()` | 기본 공격 발동 (검, 피스톨 등) |
| 우클릭 | `OnSkillPressed()` | 스킬 발동 예정 |
| 숫자키 1~2 | `OnWeaponSwitch(int index)` | 무기 전환 (예: 검 ⇄ 총) |

---

## PlayerController.cs

- `OnMoveInput` → `moveInput` 변수에 저장 → 매 프레임 이동 처리
- `OnJumpPressed` → `velocity.y`를 통해 점프
- `Update()`에서 `Move() + ApplyGravity()` 호출

---

## CameraController.cs

- `OnLookInput`을 통해 마우스 이동값을 받아 회전 처리
- X축은 플레이어 회전, Y축은 카메라 자체 회전으로 분리

---

## 입력 확장 계획

| 기능 | 이벤트 확장 계획 |
|------|------------------|
| 상호작용 | `OnInteractPressed()` 추가 예정 |
| 대화 시스템 | `OnDialogueAdvance()` 등 입력 제한 포함 |
| 시간 조작 | 전투와 분리된 `TimeSlowPressed`, `TimeRewindPressed` 추가 고려 |

---

## 구조적 장점

- 시스템 간 "입력 독립성 확보" → 모듈 테스트 용이
- "이벤트 기반"이라 구독/해제만으로 다양한 시스템 연동 가능
- "확장성/유지보수 용이" (새 키 추가 시 InputManager에만 추가하면 됨)

---

## InputManager 역할 요약

- 입력 관리의 단일 진입점
- 입력을 직접 처리하지 않음 → "해석"은 각 수신자가 담당
- `Start()`에서 구독, `OnDisable()`에서 해제하는 방식 사용

---

향후 무기 시스템, UI 시스템, 시간 조작 시스템과도 유연하게 연결할 수 있도록 설계되어 있습니다.
