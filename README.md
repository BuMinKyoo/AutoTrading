
# AutoGetMoney (자동매매 WPF 프로그램)

## 프로젝트 소개
WPF 기반 해외주식 자동매매 프로그램입니다.  
- MVVM 패턴 적용
- Microsoft.Extensions.DependencyInjection 으로 의존성 주입(DI) 구조
- REST API + Task 동시 실행, 유량제어 등 자동매매에 필요한 구조 설계
- 피보나치, RSI, High Drop Range 분석 로직 적용

## 기술 스택
- C#, WPF, MVVM
- CommunityToolkit.Mvvm
- Microsoft.Extensions.DependencyInjection
- Newtonsoft.Json
- 한국투자증권 OPEN API (REST JSON)

## 주요 기능
- 계좌별 다중 Plan 관리 (종목 검색 + 매수 전략 자동 실행)
- 종목별 RSI, 피보나치 구간, High Drop Range 실시간 계산
- MultiValueConverter로 가격대별 색상 표시
- 한국투자 OPEN API 유량제어(모의 1초 2건, 실전 1초 20건) 구조
- CancellationTokenSource + Task로 종목검색, 매수 병렬 처리
- JSON 로그 저장으로 예외 상황 분석 가능


![image](https://github.com/user-attachments/assets/9d6de293-4c25-44b4-82d4-a9e42ae8a90c)

<br/>
<br/>
<br/>

https://github.com/user-attachments/assets/5323ac25-b06b-4193-af0d-0d508188ab99

