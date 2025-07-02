// using System;
// using System.Collections.Generic;
// using UnityEngine;

// public class ComboEvaluator_ex : MonoBehaviour
// {
//     #region Singleton
//     public static ComboEvaluator Instance { get; private set; }
//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
//     #endregion

//     [Header("콤보 설정")]
//     [SerializeField] private int maxComboBeats = 8; // 콤보 최대 비트 수

//     public event Action<ComboSequence> OnComboMatched;
//     public event Action<string> OnComboProgress;
//     public event Action<ComboSequence, int, ComboAttackData> OnComboAttackExecuted; // 콤보 공격 실행
//     public event Action<ComboSequence> OnComboCompleted; // 콤보 완성
//     public event Action<ComboSequence> OnComboFailed; // 콤보 실패
//     public event Action<AttackType> OnNormalAttackExecuted; // 일반 공격 실행

//     // 비트별 입력 버퍼
//     private Queue<AttackType> beatInputBuffer = new Queue<AttackType>();
//     private int currentBeatIndex = 0;
//     private float lastComboTime = 0f;
//     private bool inputRegisteredThisBeat = false;
//     private bool isComboExecuting = false; // 콤보 실행 중 플래그
    
//     // 진행형 콤보 관련 변수
//     private bool isComboInProgress = false; // 콤보 진행 중 플래그
//     private ComboSequence currentCombo = null; // 현재 진행 중인 콤보
//     private int currentComboStep = 0; // 현재 콤보 단계

//     private void Start()
//     {
//         // InputManager에서 입력 발생 시 현재 비트에 기록
//         if (InputManager.Instance != null)
//         {
//             InputManager.Instance.OnLightAttackPressed += OnLightAttackPressed;
//             InputManager.Instance.OnHeavyAttackPressed += OnHeavyAttackPressed;
//         }

//         // TimingComboManager의 비트 루프에 맞춰 평가
//         if (TimingComboManager.Instance != null)
//         {
//             TimingComboManager.Instance.OnBeat += OnBeat; // OnBeat는 각 비트마다 호출되는 이벤트라고 가정
//         }
//     }

//     private void OnDestroy()
//     {
//         // 이벤트 구독 해제
//         if (InputManager.Instance != null)
//         {
//             InputManager.Instance.OnLightAttackPressed -= OnLightAttackPressed;
//             InputManager.Instance.OnHeavyAttackPressed -= OnHeavyAttackPressed;
//         }

//         if (TimingComboManager.Instance != null)
//         {
//             TimingComboManager.Instance.OnBeat -= OnBeat;
//         }
//     }

//     private void OnLightAttackPressed()
//     {
//         RegisterInput(AttackType.Light);
//     }

//     private void OnHeavyAttackPressed()
//     {
//         RegisterInput(AttackType.Heavy);
//     }

//     /// <summary>
//     /// 비트마다 호출: 입력이 없으면 Rest로 기록, 입력이 있으면 해당 입력 기록
//     /// </summary>
//     private void OnBeat()
//     {
//         Debug.Log("OnBeat!!!");
        
//         if (isComboExecuting)
//         {
//             return;
//         }

//         // 콤보 진행 중이면 타이밍 체크
//         if (isComboInProgress)
//         {
//             CheckComboTiming();
//         }

//         if (!inputRegisteredThisBeat)
//         {
//             beatInputBuffer.Enqueue(AttackType.Rest);
//         }
//         inputRegisteredThisBeat = false;

//         if (Time.time - lastComboTime > TimingComboManager.Instance.GetComboWindow())
//         {
//             // 콤보 윈도우 초과 시 콤보 실패
//             if (isComboInProgress)
//             {
//                 OnComboFailed?.Invoke(currentCombo);
//                 ResetCombo();
//             }
//             else
//             {
//                 beatInputBuffer.Clear();
//                 currentBeatIndex = 0;
//                 NotifyComboProgress();
//             }
//             return;
//         }

//         currentBeatIndex++;
//         lastComboTime = Time.time;
//         NotifyComboProgress();

//         if (beatInputBuffer.Count > maxComboBeats)
//         {
//             beatInputBuffer.Dequeue();
//         }
//     }

//     /// <summary>
//     /// 입력 발생 시 현재 비트에 기록
//     /// </summary>
//     public void RegisterInput(AttackType input)
//     {
//         if (isComboExecuting || inputRegisteredThisBeat) return;
        
//         beatInputBuffer.Enqueue(input);
//         inputRegisteredThisBeat = true;
//         lastComboTime = Time.time;

//         // 콤보 진행 중이 아니면 콤보 시작 시도
//         if (!isComboInProgress)
//         {
//             TryStartCombo(input);
//         }
//         else
//         {
//             // 콤보 진행 중이면 다음 단계 확인
//             TryContinueCombo(input);
//         }
//     }

//     /// <summary>
//     /// 특정 콤보의 해당 스텝까지 입력이 유효한지 검사
//     /// </summary>
//     public bool IsValidStep(ComboSequence combo, int step)
//     {
//         if (beatInputBuffer.Count < step + 1)
//             return false;

//         var bufferArray = beatInputBuffer.ToArray();
//         for (int i = 0; i <= step; i++)
//         {
//             if (combo.attackSequence[i].attackType != bufferArray[i])
//             {
//                 return false;
//             }
//         }
//         return true;
//     }

//     /// <summary>
//     /// 콤보 매칭 시도
//     /// </summary>
//     private void TryMatchCombo()
//     {
//         // Debug.Log($"TryMatchCombo 실행 - Instance: {GetInstanceID()}, Time: {Time.time:F3}");
//         var weapon = WeaponManager.Instance?.CurrentWeapon;
//         if (weapon == null || weapon.weaponData.weaponType != WeaponType.Sword)
//             return;
//         var availableCombos = weapon.weaponData.swordCombos;
//         if (availableCombos == null || availableCombos.Count == 0)
//             return;
//         foreach (var combo in availableCombos)
//         {
//             if (IsComboMatch(combo))
//             {
//                 // Debug.Log($"[ComboEvaluator] 콤보 매칭 성공: {combo.comboName}");
//                 OnComboMatched?.Invoke(combo);
//                 beatInputBuffer.Clear();
//                 currentBeatIndex = 0;
//                 return;
//             }
//         }
//         // Debug.Log("[ComboEvaluator] 매칭되는 콤보 없음");
//     }

//     /// <summary>
//     /// 입력 버퍼와 콤보 패턴이 일치하는지 확인
//     /// </summary>
//     private bool IsComboMatch(ComboSequence combo)
//     {
//         if (beatInputBuffer.Count < combo.attackSequence.Count)
//             return false;

//         var bufferArray = beatInputBuffer.ToArray();
//         int startIdx = bufferArray.Length - combo.attackSequence.Count;

//         for (int i = 0; i < combo.attackSequence.Count; i++)
//         {
//             if (combo.attackSequence[i].attackType != bufferArray[startIdx + i])
//                 return false;
//         }
//         return true;
//     }

//     /// <summary>
//     /// 콤보 성공 시 타이밍 판정
//     /// </summary>
//     // private void EvaluateComboTiming()
//     // {
//     //     // 마지막 입력의 타이밍을 기준으로 판정
//     //     if (beatInputBuffer.Count == 0) return;
        
//     //     float currentTime = Time.time;
//     //     float lastInputTime = currentTime - 0.1f; // 마지막 입력 시간 추정
        
//     //     // TimingComboManager의 판정 로직 사용
//     //     float beatsPassed = Mathf.Round((lastInputTime - TimingComboManager.Instance.StartTime) / TimingComboManager.Instance.BeatInterval);
//     //     float nearestBeatTime = TimingComboManager.Instance.StartTime + beatsPassed * TimingComboManager.Instance.BeatInterval;
//     //     float offset = Mathf.Abs(lastInputTime - nearestBeatTime);
        
//     //     TimingComboManager.TimingResult result;
//     //     if (offset <= 0.1f) // Perfect 윈도우
//     //         result = TimingComboManager.TimingResult.Perfect;
//     //     else if (offset <= 0.25f) // Good 윈도우
//     //         result = TimingComboManager.TimingResult.Good;
//     //     else
//     //         result = TimingComboManager.TimingResult.Miss;
            
//     //     float damageMultiplier = result == TimingComboManager.TimingResult.Perfect ? 1.5f : 1.2f;
        
//     //     Debug.Log($"[ComboEvaluator] 콤보 타이밍 판정: {result}, 배율: {damageMultiplier}");
//     // }

//     /// <summary>
//     /// 콤보 진행 상황 알림
//     /// </summary>
//     private void NotifyComboProgress()
//     {
//         if (beatInputBuffer.Count > 0)
//         {
//             var bufferArray = beatInputBuffer.ToArray();
//             string progress = string.Join(" → ", Array.ConvertAll(bufferArray, GetAttackTypeString));
//             OnComboProgress?.Invoke(progress);
//         }
//     }

//     private string GetAttackTypeString(AttackType type)
//     {
//         switch (type)
//         {
//             case AttackType.Light: return "약";
//             case AttackType.Heavy: return "강";
//             case AttackType.Rest: return "쉬기";
//             default: return "?";
//         }
//     }

//     public void ClearInputBuffer()
//     {
//         beatInputBuffer.Clear();
//         currentBeatIndex = 0;
//         inputRegisteredThisBeat = false;
//     }

//     /// <summary>
//     /// 콤보 실행 시작 - 입력 버퍼 업데이트 중단
//     /// </summary>
//     public void StartComboExecution()
//     {
//         isComboExecuting = true;
//         Debug.Log("[ComboEvaluator] 콤보 실행 시작 - 입력 버퍼 업데이트 중단");
//     }

//     /// <summary>
//     /// 콤보 실행 종료 - 입력 버퍼 업데이트 재개
//     /// </summary>
//     public void EndComboExecution()
//     {
//         isComboExecuting = false;
//         ClearInputBuffer();
//         Debug.Log("[ComboEvaluator] 콤보 실행 종료 - 입력 버퍼 클리어 및 업데이트 재개");
//     }

//     private void TryStartCombo(AttackType input)
//     {
//         var weapon = WeaponManager.Instance?.CurrentWeapon;
//         if (weapon == null || weapon.weaponData.weaponType != WeaponType.Sword)
//             return;
            
//         foreach (var combo in weapon.weaponData.swordCombos)
//         {
//             if (combo.attackSequence[0].attackType == input)
//             {
//                 // 콤보 시작!
//                 isComboInProgress = true;
//                 currentCombo = combo;
//                 currentComboStep = 0;
                
//                 Debug.Log($"[ComboEvaluator] 콤보 시작: {combo.comboName}");
                
//                 // 즉시 첫 번째 공격 실행
//                 ExecuteComboAttack(combo, 0);
//                 return;
//             }
//         }
        
//         // 콤보가 아니면 일반 공격으로 처리
//         OnNormalAttackExecuted?.Invoke(input);
//     }

//     private void TryContinueCombo(AttackType input)
//     {
//         if (currentCombo == null) return;
        
//         currentComboStep++;
//         if (currentComboStep < currentCombo.attackSequence.Count)
//         {
//             if (currentCombo.attackSequence[currentComboStep].attackType == input)
//             {
//                 // 콤보 계속 진행 - 즉시 공격 실행
//                 Debug.Log($"[ComboEvaluator] 콤보 단계 진행: {currentComboStep + 1}/{currentCombo.attackSequence.Count}");
//                 ExecuteComboAttack(currentCombo, currentComboStep);
//             }
//             else
//             {
//                 // 콤보 실패 - 잘못된 입력
//                 Debug.Log("[ComboEvaluator] 콤보 실패 - 잘못된 입력");
//                 OnComboFailed?.Invoke(currentCombo);
//                 ResetCombo();
//             }
//         }
//         else
//         {
//             // 콤보 완성!
//             Debug.Log($"[ComboEvaluator] 콤보 완성: {currentCombo.comboName}");
//             OnComboCompleted?.Invoke(currentCombo);
//             ResetCombo();
//         }
//     }

//     private void ExecuteComboAttack(ComboSequence combo, int step)
//     {
//         var attackData = combo.attackSequence[step];
//         // 즉시 공격 실행 (애니메이션, 데미지 등)
//         OnComboAttackExecuted?.Invoke(combo, step, attackData);
//     }

//     private void CheckComboTiming()
//     {
//         // 콤보 진행 중 타이밍 체크
//         if (Time.time - lastComboTime > TimingComboManager.Instance.GetComboWindow())
//         {
//             Debug.Log("[ComboEvaluator] 콤보 실패 - 타이밍 초과");
//             OnComboFailed?.Invoke(currentCombo);
//             ResetCombo();
//         }
//     }

//     private void ResetCombo()
//     {
//         isComboInProgress = false;
//         currentCombo = null;
//         currentComboStep = 0;
//         ClearInputBuffer();
//     }
// } 