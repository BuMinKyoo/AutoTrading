using AutoGetMoney.Model;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using System.Numerics;
using System.Threading;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;
using AutoGetMoney.Common;

// 유량제한
//(실전투자)1초당 20건
//(모의투자)1초당 2건

namespace AutoGetMoney.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // 메인 class
        #region
        private ObservableCollection<Section> _obcSectionList = new ObservableCollection<Section>();
        public ObservableCollection<Section> ObcSectionList
        {
            get { return _obcSectionList; }
            set
            {
                _obcSectionList = value;
                Notify();
            }
        }

        private Section? _clsSection = new Section();
        public Section? ClsSection
        {
            get { return _clsSection; }
            set
            {
                _clsSection = value;
                Notify();
            }
        }
        #endregion


        // 일반 ui 바인딩 변수
        #region
        // 실전투자, 모의투자 선택
        private string? _uiStrTradeMode = "모의투자"; // 초반세팅
        public string? UiStrTradeMode
        {
            get { return _uiStrTradeMode; }
            set
            {
                _uiStrTradeMode = value;
                Notify();

                // 콤보박스가 변경되면 통신 URL도 자동 갱신
                UpdateCommunicationUrlBasedOnTradeMode(value);
            }
        }

        // URI
        private string? _uiStrCommunicationUrl = "https://openapivts.koreainvestment.com:29443"; // 초반세팅
        public string? UiStrCommunicationUrl
        {
            get { return _uiStrCommunicationUrl; }
            set
            {
                _uiStrCommunicationUrl = value;
                Notify();
            }
        }

        // 계좌번호
        private string? _uiStrAcountNumber = "50131331";
        public string? UiStrAcountNumber
        {
            get { return _uiStrAcountNumber; }
            set
            {
                _uiStrAcountNumber = value;
                Notify();
            }
        }

        // 앱키
        private string? _uiStrAppKey = "PSF9tgpn4hUOAUXMHf6zqMz3J45I6CqpQACt";
        public string? UiStrAppKey
        {
            get { return _uiStrAppKey; }
            set
            {
                _uiStrAppKey = value;
                Notify();
            }
        }

        // 앱시크릿
        private string? _uiStrAppSecret = "ComZh48pNSCMNmLQIILpi9EJOqP/wVldJkRwtist7QCqvSWLiyOkRLJRa1ybtRdNjs5esMaUXw0X1e1xA9GSjDKVsvDZeOY2NPHRIkJ/sq+ynLdyuZcPC9ptBKjuUprkBydo89g2aSvXWpkUzKjv4p3GBpOmUccY6JFAg7plcuoC3unhfgM=";
        public string? UiStrAppSecret
        {
            get { return _uiStrAppSecret; }
            set
            {
                _uiStrAppSecret = value;
                Notify();
            }
        }

        // 웹접속키
        private string? _uiStrWebKey = "7c27e753-9bf0-48f6-b4f3-2627e6a9128d";
        public string? UiStrWebKey
        {
            get { return _uiStrWebKey; }
            set
            {
                _uiStrWebKey = value;
                Notify();
            }
        }

        // 토큰
        private string? _uiStrToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJ0b2tlbiIsImF1ZCI6ImVhN2FlNjkwLWYxMWQtNGQwYS05OTMwLWM4ZWEzMjFlZmM1OCIsInByZHRfY2QiOiIiLCJpc3MiOiJ1bm9ndyIsImV4cCI6MTc1MjA2MjYyNCwiaWF0IjoxNzUxOTc2MjI0LCJqdGkiOiJQU0Y5dGdwbjRoVU9BVVhNSGY2enFNejNKNDVJNkNxcFFBQ3QifQ.hYjk2E221tes1n_n_j3cNQfRPfMXf-QdY5k4augXj9O4zDgYM-U1oG6lfspC2mEfmr1bro56lid7HmNObmvPcA";
        public string? UiStrToken
        {
            get { return _uiStrToken; }
            set
            {
                _uiStrToken = value;
                Notify();
            }
        }

        // 콤보박스에 바인딩 하기 위해 사용
        private ObservableCollection<string> _obcTradeModeList = new ObservableCollection<string> { "모의투자", "실전투자" };
        public ObservableCollection<string> ObcTradeModeList
        {
            get { return _obcTradeModeList; }
            set
            {
                _obcTradeModeList = value;
                Notify();
            }
        }

        #endregion


        // 종목검색, 종목매수 방법 관련 바인딩
        #region

        // 종목검색 방법
        private ObservableCollection<string> _obcStrSearchMethodList = new ObservableCollection<string>
        {
            "ㅡ선택안함ㅡ",
            "16시종목",
            "주도주30%"
        };
        public ObservableCollection<string> ObcStrSearchMethodList
        {
            get { return _obcStrSearchMethodList; }
            set
            {
                _obcStrSearchMethodList = value;
                Notify();
            }
        }

        // 종목매수 방법
        private ObservableCollection<string> _obcStrBuyMethodList = new ObservableCollection<string>
        {
            "ㅡ선택안함ㅡ",
            "과매도30",
            "과매도25",
            "세력선3(초록색)"
        };
        public ObservableCollection<string> ObcStrBuyMethodList
        {
            get { return _obcStrBuyMethodList; }
            set
            {
                _obcStrBuyMethodList = value;
                Notify();
            }
        }


        // 메뉴얼 방법
        private ObservableCollection<string> _obcStrManualMethodList = new ObservableCollection<string>
        {
            "ㅡ선택안함ㅡ",
            "ABC종목매수"
        };
        public ObservableCollection<string> ObcStrManualMethodList
        {
            get { return _obcStrManualMethodList; }
            set
            {
                _obcStrManualMethodList = value;
                Notify();
            }
        }

        #endregion


        // 일반 함수
        #region
        private void UpdateCommunicationUrlBasedOnTradeMode(string? tradeMode)
        {
            // 콤보박스가 변경되면 통신 URL도 자동 갱신

            switch (tradeMode)
            {
                case "모의투자":
                    UiStrCommunicationUrl = "https://openapivts.koreainvestment.com:29443";
                    break;
                case "실전투자":
                    UiStrCommunicationUrl = "https://openapi.koreainvestment.com:9443";
                    break;
                default:
                    UiStrCommunicationUrl = string.Empty;
                    break;
            }
        }
        #endregion




        // 웹접속키생성 버튼
        #region
        private Command? _cmdBtnGenerateWebKey_Click;
        public ICommand BtnGenerateWebKey_Click
        {
            get { return _cmdBtnGenerateWebKey_Click = new Command(OnCmdBtnGenerateWebKey_Click); }
        }

        private async void OnCmdBtnGenerateWebKey_Click(object obj)
        {
            if (UiStrAcountNumber == null || UiStrAppKey == null || UiStrAppSecret == null)
            {
                MessageBox.Show("계좌번호, APP KEY, APP SECRET을 채워주세요");
                return;
            }

            JObject? json = null;

            var result = await OpenApiHelper.GenerateWebKeyAsync(UiStrCommunicationUrl, UiStrAppKey, UiStrAppSecret);

            // JSON 파싱
            try
            {
                json = JObject.Parse(result!);
                UiStrWebKey = json["approval_key"]?.ToString();
            }
            catch (JsonException ex)
            {
                string StrLog = $"웹접속키 생성 JSON 파싱 오류: {ex.Message} // result : {result}";
                StaticUtility.WriteLog(StrLog);
                // JSON 파싱 오류 처리
            }
        }
        #endregion

        // 토큰생성 버튼
        #region
        private Command? _cmdBtnGenerateToken_Click;
        public ICommand BtnGenerateToken_Click
        {
            get { return _cmdBtnGenerateToken_Click = new Command(OnCmdBtnGenerateToken_Click); }
        }

        private async void OnCmdBtnGenerateToken_Click(object obj)
        {
            if (UiStrAcountNumber == null || UiStrAppKey == null || UiStrAppSecret == null)
            {
                MessageBox.Show("계좌번호, APP KEY, APP SECRET을 채워주세요");
                return;
            }

            JObject? json = null;

            var result = await OpenApiHelper.GenerateTokenAsync(UiStrCommunicationUrl, UiStrAppKey, UiStrAppSecret);

            // JSON 파싱
            try
            {
                json = JObject.Parse(result!);
                UiStrToken = json["access_token"]?.ToString();
            }
            catch (JsonException ex)
            {
                string StrLog = $"토큰 생성 JSON 파싱 오류: {ex.Message} // result : {result}";
                StaticUtility.WriteLog(StrLog);
                // JSON 파싱 오류 처리
            }
        }
        #endregion

        // 계좌추가 버튼
        #region
        private Command? _cmdBtnSectionAdd_Click;
        public ICommand BtnSectionAdd_Click
        {
            get { return _cmdBtnSectionAdd_Click = new Command(OnCmdBtnSectionAdd_Click); }
        }

        private void OnCmdBtnSectionAdd_Click(object obj)
        {
            if (UiStrAppKey == null || UiStrAppSecret == null)
            {
                MessageBox.Show("계좌를 만들 데이터가 부족합니다");
                return;
            }

            // 계좌 갯수 확인
            int InAcountCnt = _obcSectionList.Count;

            _obcSectionList.Add(new Section() { InSectionNo = InAcountCnt, StrCommunicationUrl = UiStrCommunicationUrl, StrAccountNumber = UiStrAcountNumber, StrAppKey = UiStrAppKey, StrAppSecret = UiStrAppSecret, StrWebKey = UiStrWebKey, StrToken = UiStrToken, StrSearchMethod_Add = "ㅡ선택안함ㅡ" , StrBuyMethod_Add = "ㅡ선택안함ㅡ" , StrManualMethod_Add = "ㅡ선택안함ㅡ" });
        }
        #endregion

        // 계좌삭제 버튼
        #region
        private Command? _cmdBtnDeleteAccount_Click;
        public ICommand BtnDeleteAccount_Click
        {
            get { return _cmdBtnDeleteAccount_Click = new Command(OnCmdBtnDeleteAccount_Click); }
        }

        private void OnCmdBtnDeleteAccount_Click(object obj)
        {
            // 버튼의 데이터 컨텍스트 가져오기
            Section? ClsClickedSection = obj as Section;

            if (ClsClickedSection != null)
            {
                // 확인 메시지 박스
                var result = MessageBox.Show(
                    $"정말 이 계좌를 삭제하시겠습니까? 계좌번호 : {ClsClickedSection.StrAccountNumber}",
                    "계좌 삭제 확인",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result != MessageBoxResult.Yes)
                {
                    return; // 사용자가 No를 누르면 종료
                }

                ClsClickedSection.ObcStockList.Clear();
                ClsClickedSection.ObcStockActionPlanList.Clear();

                if (ClsClickedSection.InSectionNo + 1 == _obcSectionList.Count()) // 마지막 섹션을 지우면 마지막것만 지움
                {
                    _obcSectionList.RemoveAt(ClsClickedSection.InSectionNo);
                }
                else
                {
                    _obcSectionList.RemoveAt(ClsClickedSection.InSectionNo);

                    for (int i = ClsClickedSection.InSectionNo; i < _obcSectionList.Count(); i++) // 중간 섹션을 지우면 삭제하는 섹션의 다음 섹션부터 섹션번호를 -1로 해줌
                    {
                        _obcSectionList[i].InSectionNo--;
                    }
                }
            }
        }
        #endregion

        // 방법추가 버튼
        #region
        private Command? _cmdBtnTradePlanAdd_Click;
        public ICommand BtnTradePlanAdd_Click
        {
            get { return _cmdBtnTradePlanAdd_Click = new Command(OnCmdBtnTradePlanAdd_Click); }
        }

        private void OnCmdBtnTradePlanAdd_Click(object obj)
        {
            Section? ClsClickedSection = obj as Section;

            if (ClsClickedSection != null)
            {
                int InStockActionPlanCnt = ClsClickedSection.ObcStockActionPlanList.Count;

                ClsClickedSection.ObcStockActionPlanList.Add(new StockActionPlan() {InStockActionPlanNo = InStockActionPlanCnt, BlIsActive = false, ClsParentSection = ClsClickedSection, StrSearchMethod = "ㅡ선택안함ㅡ", StrBuyMethod = "ㅡ선택안함ㅡ", StrManualMethod = "ㅡ선택안함ㅡ"});
            }
        }
        #endregion

        // 종목플랜 Start 버튼
        #region
        private Command? _cmdBtnTradePlanStart_Click;
        public ICommand BtnTradePlanStart_Click
        {
            get { return _cmdBtnTradePlanStart_Click = new Command(OnCmdBtnTradePlanStart_Click); }
        }

        private void OnCmdBtnTradePlanStart_Click(object obj)
        {
            // 버튼의 데이터 컨텍스트 가져오기
            StockActionPlan? ClsClickedStockActionPlan = obj as StockActionPlan;

            if (ClsClickedStockActionPlan == null || ClsClickedStockActionPlan.BlIsActive == true)
            {
                return;
            }

            if (ClsClickedStockActionPlan.StrSearchMethod == "ㅡ선택안함ㅡ" &&
                ClsClickedStockActionPlan.StrBuyMethod == "ㅡ선택안함ㅡ" &&
                ClsClickedStockActionPlan.StrManualMethod == "ㅡ선택안함ㅡ")
            {
                MessageBox.Show("플랜 방법이 지정되지 않았습니다. 시작할 수 없습니다");
                return;
            }

            if ( (ClsClickedStockActionPlan.StrSearchMethod != "ㅡ선택안함ㅡ" &&
                ClsClickedStockActionPlan.StrManualMethod != "ㅡ선택안함ㅡ" ) ||
                (ClsClickedStockActionPlan.StrBuyMethod != "ㅡ선택안함ㅡ" &&
                ClsClickedStockActionPlan.StrManualMethod != "ㅡ선택안함ㅡ") )
            {
                MessageBox.Show("이 플랜에 공통된 방법이 있습니다 방법을 수정해 주세요");
                return;
            }

            if (ClsClickedStockActionPlan.ClsParentSection == null)
            {
                MessageBox.Show("부모데이터가 없습니다");
                return;
            }

            bool BlHasDuplicateSearchMethod = ClsClickedStockActionPlan.ClsParentSection
                .ObcStockActionPlanList
                .Any(plan =>
                    plan != ClsClickedStockActionPlan &&  // 자기 자신 제외
                    plan.StrSearchMethod == ClsClickedStockActionPlan.StrSearchMethod &&
                    plan.BlIsActive == true);

            if (BlHasDuplicateSearchMethod == true)
            {
                MessageBox.Show("다른 플랜에 공통 종목검색 방법이 있습니다. 방법을 수정해 주세요");
                return;
            }

            var result = MessageBox.Show(
               $"이 방법을 '실행'하시겠습니까? 검색방법 : {ClsClickedStockActionPlan.StrSearchMethod}, 매수방법 : {ClsClickedStockActionPlan.StrBuyMethod}, 메뉴얼방법 : {ClsClickedStockActionPlan.StrManualMethod}",
               "계좌 플랜 실행",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning
           );

            if (result != MessageBoxResult.Yes)
            {
                return; // 사용자가 No를 누르면 종료
            }

            ClsClickedStockActionPlan.BlIsActive = true;

            // 검색 Task 실행
            ClsClickedStockActionPlan.CtsSearch = new CancellationTokenSource();
            ClsClickedStockActionPlan.TaskSearch = Task.Run(async () =>
            {
                try
                {
                    while (!ClsClickedStockActionPlan.CtsSearch.Token.IsCancellationRequested)
                    {
                        await DoStockSearchAsync(ClsClickedStockActionPlan);  // 종목 검색 함수
                        await Task.Delay(TimeSpan.FromSeconds(10)); // 주기적 실행
                    }
                    ClsClickedStockActionPlan.CtsSearch = null;
                    ClsClickedStockActionPlan.TaskSearch = null;

                }
                catch (Exception ex)
                {
                    string StrLog = $"[검색 Task 예외] {ex.Message}";
                    StaticUtility.WriteLog(StrLog);
                    ClsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                }
            });

            // 매수 Task 실행
            ClsClickedStockActionPlan.CtsBuy = new CancellationTokenSource();
            ClsClickedStockActionPlan.TaskBuy = Task.Run(async () =>
            {
                try
                {
                    while (!ClsClickedStockActionPlan.CtsBuy.Token.IsCancellationRequested)
                    {
                        await DoBuyStockAsync(ClsClickedStockActionPlan); // 종목 매수 함수
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    ClsClickedStockActionPlan.CtsBuy = null;
                    ClsClickedStockActionPlan.TaskBuy = null;
                }
                catch (Exception ex)
                {
                    string StrLog = $"[매수 Task 예외] {ex.Message}";
                    StaticUtility.WriteLog(StrLog);
                    ClsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                }
            });

        }

        #endregion

        // 종목 플랜 Stop 버튼
        #region
        private Command? _cmdBtnTradePlanStop_Click;
        public ICommand BtnTradePlanStop_Click
        {
            get { return _cmdBtnTradePlanStop_Click = new Command(OnCmdBtnTradePlanStop_Click); }
        }

        private void OnCmdBtnTradePlanStop_Click(object obj)
        {
            // 버튼의 데이터 컨텍스트 가져오기
            StockActionPlan? ClsClickedStockActionPlan = obj as StockActionPlan;

            if (ClsClickedStockActionPlan == null || ClsClickedStockActionPlan.BlIsActive == false)
            {
                return;
            }

            var result = MessageBox.Show(
               $"이 방법을 '정지'하시겠습니까? 검색방법 : {ClsClickedStockActionPlan.StrSearchMethod}, 매수방법 : {ClsClickedStockActionPlan.StrBuyMethod}, 메뉴얼방법 : {ClsClickedStockActionPlan.StrManualMethod}",
               "계좌 플랜 정지",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning
           );

            if (result != MessageBoxResult.Yes)
            {
                return; // 사용자가 No를 누르면 종료
            }

            ClsClickedStockActionPlan.BlIsActive = false;

            // Cancel and null
            ClsClickedStockActionPlan.CtsSearch?.Cancel();
            ClsClickedStockActionPlan.CtsBuy?.Cancel();
        }
        #endregion

        // 종목 플랜 Delete 버튼
        #region
        private Command? _cmdBtnTradePlanDelete_Click;
        public ICommand BtnTradePlanDelete_Click
        {
            get { return _cmdBtnTradePlanDelete_Click = new Command(OnCmdBtnTradePlanDelete_Click); }
        }

        private void OnCmdBtnTradePlanDelete_Click(object obj)
        {
            // 버튼의 데이터 컨텍스트 가져오기
            StockActionPlan? ClsClickedStockActionPlan = obj as StockActionPlan;

            if (ClsClickedStockActionPlan != null)
            {
                var result = MessageBox.Show(
               $"이 방법을 '삭제'하시겠습니까? 검색방법 : {ClsClickedStockActionPlan.StrSearchMethod}, 매수방법 : {ClsClickedStockActionPlan.StrBuyMethod}, 메뉴얼방법 : {ClsClickedStockActionPlan.StrManualMethod}",
               "계좌 플랜 삭제",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning
               );

                if (result != MessageBoxResult.Yes)
                {
                    return; // 사용자가 No를 누르면 종료
                }

                if (ClsClickedStockActionPlan.InStockActionPlanNo + 1 == ClsClickedStockActionPlan.ClsParentSection!.ObcStockActionPlanList.Count()) // 마지막 섹션을 지우면 마지막것만 지움
                {
                    ClsClickedStockActionPlan.ClsParentSection.ObcStockActionPlanList.RemoveAt(ClsClickedStockActionPlan.InStockActionPlanNo);
                }
                else
                {
                    ClsClickedStockActionPlan.ClsParentSection.ObcStockActionPlanList.RemoveAt(ClsClickedStockActionPlan.InStockActionPlanNo);

                    for (int i = ClsClickedStockActionPlan.InStockActionPlanNo; i < ClsClickedStockActionPlan.ClsParentSection.ObcStockActionPlanList.Count(); i++) // 중간 섹션을 지우면 삭제하는 섹션의 다음 섹션부터 섹션번호를 -1로 해줌
                    {
                        ClsClickedStockActionPlan.ClsParentSection.ObcStockActionPlanList[i].InStockActionPlanNo--;
                    }
                }
            }
        }
        #endregion

        // 현재 플랜의 개별 종목 Delete 버튼
        #region
        private Command? _cmdBtnStockDelete_Click;
        public ICommand BtnStockDelete_Click
        {
            get { return _cmdBtnStockDelete_Click = new Command(OnCmdBtnStockDelete_Click); }
        }

        private void OnCmdBtnStockDelete_Click(object obj)
        {
            // 버튼의 데이터 컨텍스트 가져오기
            Stock? ClsClickedStock = obj as Stock;

            if (ClsClickedStock != null)
            {
                var result = MessageBox.Show(
               $"이 종목을 '삭제'하시겠습니까? 종목이름 : {ClsClickedStock.StrStockName}, 티커 : {ClsClickedStock.StrStockSymb}",
               "종목 삭제",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning
               );

                if (result != MessageBoxResult.Yes)
                {
                    return; // 사용자가 No를 누르면 종료
                }

                // 부모 Section의 ObcStockList에서 제거
                Section? ClsParentSection = ClsClickedStock.ClsParentSection;
                if (ClsParentSection != null && ClsParentSection.ObcStockList.Contains(ClsClickedStock))
                {
                    ClsParentSection.ObcStockList.Remove(ClsClickedStock);

                    // 같은 것은 1개 밖에 나오지 않음
                    var actionPlan = ClsParentSection.ObcStockActionPlanList.FirstOrDefault(s =>
                        s.StrSearchMethod == ClsClickedStock.StrSearchMethod &&
                        s.StrBuyMethod == ClsClickedStock.StrBuyMethod &&
                        s.StrManualMethod == ClsClickedStock.StrManualMethod);

                    if (actionPlan != null)
                    {
                        actionPlan.RaiseFilteredListChanged();
                    }
                }
            }
        }
        #endregion

        // 종목 수동 추가 버튼
        #region
        private Command? _cmdBtnManualStockAdd_Click;
        public ICommand BtnManualStockAdd_Click
        {
            get { return _cmdBtnManualStockAdd_Click = new Command(OnCmdBtnManualStockAdd_Click); }
        }

        private void OnCmdBtnManualStockAdd_Click(object obj)
        {
            // 버튼의 데이터 컨텍스트 가져오기
            Section? ClsClickedSection = obj as Section;

            if (ClsClickedSection == null)
            {
                return;
            }

            if (ClsClickedSection.StrStockSymb_Add == null || ClsClickedSection.StrStockExcd_Add == null)
            {
                MessageBox.Show("종목코드 or 종목명이 없습니다");
                return;
            }

            if (ClsClickedSection.StrSearchMethod_Add == "ㅡ선택안함ㅡ" &&
                ClsClickedSection.StrBuyMethod_Add == "ㅡ선택안함ㅡ" &&
                ClsClickedSection.StrManualMethod_Add == "ㅡ선택안함ㅡ")
            {
                MessageBox.Show("종목검색 or 종목매수 방법이 선택되어 있지 않습니다");
                return;
            }

            // 이미 있는 종목이면 기존 객체를 찾아서 덮어쓰기
            string? symb = ClsClickedSection.StrStockSymb_Add;
            var existingStock = ClsClickedSection.ObcStockList
            .FirstOrDefault(s => s.StrStockSymb == symb);

            if (existingStock != null)
            {
                MessageBox.Show("이미 종목이 있습니다");
            }
            else
            {
                var stockModel = new Stock
                {
                    ClsParentSection = ClsClickedSection,
                    StrStockExcd = ClsClickedSection.StrStockExcd_Add,
                    StrStockSymb = ClsClickedSection.StrStockSymb_Add,
                    StrStockName = ClsClickedSection.StrStockName_Add,

                    StrSearchMethod = ClsClickedSection.StrSearchMethod_Add,
                    StrBuyMethod = ClsClickedSection.StrBuyMethod_Add,
                    StrManualMethod = ClsClickedSection.StrManualMethod_Add,
                };

                // Stock 추가
                ClsClickedSection.ObcStockList.Add(stockModel);

                // 여기서 Notify
                foreach (var plan in ClsClickedSection.ObcStockActionPlanList)
                {
                    plan.RaiseFilteredListChanged();
                }
            }
        }
        #endregion

        


        

        // 종목 검색 분기처리
        #region
        private async Task DoStockSearchAsync(StockActionPlan clsClickedStockActionPlan)
        {
            if (clsClickedStockActionPlan == null || clsClickedStockActionPlan.BlIsActive == false)
            {
                return;
            }

            // 종목 검색 방법은 둘중 하나만 선택 해야함
            if (clsClickedStockActionPlan.StrSearchMethod != "ㅡ선택안함ㅡ" &&
                clsClickedStockActionPlan.StrManualMethod != "ㅡ선택안함ㅡ")
            {

                return;
            }

            if (clsClickedStockActionPlan.StrSearchMethod == "ㅡ선택안함ㅡ" &&
                clsClickedStockActionPlan.StrBuyMethod == "ㅡ선택안함ㅡ" &&
                clsClickedStockActionPlan.StrManualMethod == "ABC종목매수")
            {
                // ABC종목은 수동으로 종목을 추가한다
                return;
               
            }
            else if (clsClickedStockActionPlan.StrSearchMethod == "16시종목" &&
                clsClickedStockActionPlan.StrManualMethod == "ㅡ선택안함ㅡ")
            {
                string StrJsonResult = await OpenApiHelper.SearchStockForAllStrategyAsync(
                   clsClickedStockActionPlan.ClsParentSection,
                   clsClickedStockActionPlan.ClsParentSection!.StrCommunicationUrl, // URL
                   clsClickedStockActionPlan.ClsParentSection.StrToken, // 토큰
                   clsClickedStockActionPlan.ClsParentSection.StrAppKey, // 앱키
                   clsClickedStockActionPlan.ClsParentSection.StrAppSecret, // 앱시크릿
                   "", // 사용자권한정보 ""(Null 값 설정)
                   "NAS", // 거래소코드
                   "1", // 현재가선택조건
                   "160", // 현재가시작범위가
                   "170", // 현재가끝범위가
                   "", // 등락율선택조건
                   "", // 등락율시작율
                   "", // 등락율끝율
                   "", // 시가총액선택조건
                   "", // 시가총액시작액
                   "", // 시가총액끝액
                   "", // 발행주식수선택조건
                   "", // 발행주식시작수
                   "", // 발행주식끝수
                   "", // 거래량선택조건
                   "", // 거래량시작량
                   "", // 거래량끝량
                   "", // 거래대금선택조건
                   "", // 거래대금시작금
                   "", // 거래대금끝금
                   "", // EPS선택조건
                   "", // EPS시작
                   "", // EPS끝
                   "", // PER선택조건
                   "", // PER시작
                   ""  // PER끝
                   );

                try
                {
                    var json = JObject.Parse(StrJsonResult);
                    var stockArray = json["output2"];
                    if (stockArray != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            foreach (var stock in stockArray)
                            {
                                string? symb = stock["symb"]?.ToString();
                                if (string.IsNullOrEmpty(symb)) continue;

                                // 이미 있는 종목이면 기존 객체를 찾아서 덮어쓰기
                                var existingStock = clsClickedStockActionPlan.ClsParentSection.ObcStockList
                                .FirstOrDefault(s => s.StrStockSymb == symb);

                                if (existingStock != null)
                                {
                                    existingStock.StrStockExcd = stock["excd"]?.ToString();
                                    existingStock.StrStockSymb = stock["symb"]?.ToString();
                                    existingStock.StrStockName = stock["name"]?.ToString();
                                    existingStock.StrStockEordyn = stock["e_ordyn"]?.ToString();

                                    existingStock.StrSearchMethod = clsClickedStockActionPlan.StrSearchMethod;
                                    existingStock.StrBuyMethod = clsClickedStockActionPlan.StrBuyMethod;
                                    existingStock.StrManualMethod = clsClickedStockActionPlan.StrManualMethod;
                                }
                                else
                                {
                                    // 없는 종목이면 새로 추가
                                    var stockModel = new Stock
                                    {
                                        ClsParentSection = clsClickedStockActionPlan.ClsParentSection,
                                        StrStockExcd = stock["excd"]?.ToString(),
                                        StrStockSymb = stock["symb"]?.ToString(),
                                        StrStockName = stock["name"]?.ToString(),
                                        StrStockEordyn = stock["e_ordyn"]?.ToString(),

                                        StrSearchMethod = clsClickedStockActionPlan.StrSearchMethod,
                                        StrBuyMethod = clsClickedStockActionPlan.StrBuyMethod,
                                        StrManualMethod = clsClickedStockActionPlan.StrManualMethod
                                    };

                                    clsClickedStockActionPlan.ClsParentSection.ObcStockList.Add(stockModel);
                                }

                                // 여기서 Notify
                                clsClickedStockActionPlan.RaiseFilteredListChanged();
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    string StrLog = $" 종목검색 : {clsClickedStockActionPlan.StrSearchMethod}, 종목메뉴얼 : {clsClickedStockActionPlan.StrManualMethod} /// JSON 파싱 예외] {ex.Message} /// JsonData : {StrJsonResult}";
                    StaticUtility.WriteLog(StrLog);
                    clsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                }
            }
            else if (clsClickedStockActionPlan.StrSearchMethod == "주도주30%" &&
                clsClickedStockActionPlan.StrManualMethod == "ㅡ선택안함ㅡ")
            {
                ObservableCollection<Stock> tempStocks = new ObservableCollection<Stock>();

                // 거래량 순위 35를 다시 1일봉을 조회 // !(시가 > 종가) && !(전일종가 > 현재종가) && 거래대금 9백만달러이상 && 전일종가대비 고가 30 % 이상
                #region

                // 거래량 순위 35
                string[] exchanges = { "NAS", "NYS", "AMS" };
                foreach (string excd in exchanges)
                {
                    string StrJsonResult = await OpenApiHelper.SearchStockByVolumeRankAsync(
                    clsClickedStockActionPlan.ClsParentSection,
                    "0", // 거래량조건  0(전체), 1(1백주이상), 2(1천주이상), 3(1만주이상), 4(10만주이상), 5(100만주이상), 6(1000만주이상)
                    "999999999999", // 현재가 필터범위 2   (~가격)
                    "0", // 현재가 필터범위 1 (가격~)
                    "0", // 일자값 N일전 : 0(당일), 1(2일), 2(3일), 3(5일), 4(10일), 5(20일전), 6(30일), 7(60일), 8(120일), 9(1년)
                    excd // 거래소코드
                    );

                    // 위에서 나온 값에서 35순위 까지만 추출해서 1일봉 추출
                    try
                    {
                        var json = JObject.Parse(StrJsonResult);
                        var stockArray = json["output2"];
                        if (stockArray != null)
                        {
                            foreach (var stock in stockArray)
                            {
                                int inRank = int.Parse(stock["rank"]?.ToString() ?? "999");
                                if (inRank > 35)
                                    break;

                                tempStocks.Add(new Stock { StrStockExcd = stock["excd"]?.ToString(), StrStockSymb = stock["symb"]?.ToString(), StrStockName = stock["name"]?.ToString(), StrStockEordyn = stock["e_ordyn"]?.ToString(), InTvol = int.TryParse(stock["tvol"]?.ToString(), out var vol) ? vol : 0});
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string StrLog = $" 종목검색 : {clsClickedStockActionPlan.StrSearchMethod}, 종목메뉴얼 : {clsClickedStockActionPlan.StrManualMethod} /// JSON 파싱 예외] {ex.Message} /// JsonData : {StrJsonResult}";
                        StaticUtility.WriteLog(StrLog);
                        clsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                    }
                }

                tempStocks = new ObservableCollection<Stock>(tempStocks.OrderByDescending(s => s.InTvol)); // 거래량 순으로 정렬

                // 거래량 순위 35를 다시 1일봉을 조회 // !(시가 > 종가) && !(전일종가 > 현재종가) && 거래대금 9백만달러이상 && 전일종가대비 고가 30 % 이상
                int inCount = 0;
                foreach (var stock in tempStocks)
                {
                    // 1일봉 조회
                    inCount++;
                    if (inCount > 35)
                        break;

                    string StrDailyResultJson = "";
                    try
                    {
                        StrDailyResultJson = await OpenApiHelper.GetMinuteCandlestickAsync(
                        clsClickedStockActionPlan.ClsParentSection,
                        clsClickedStockActionPlan.ClsParentSection.StrCommunicationUrl,
                        clsClickedStockActionPlan.ClsParentSection.StrToken,
                        clsClickedStockActionPlan.ClsParentSection.StrAppKey,
                        clsClickedStockActionPlan.ClsParentSection.StrAppSecret,
                        "",           // AUTH, 공백입력
                        stock.StrStockExcd,         // 거래소 코드
                        stock.StrStockSymb,         // 종목 코드
                        "1440",       // NMIN: 분단위(1: 1분봉, 2: 2분봉, ...)
                        "1",          // PINC: 	0:당일 1:전일포함
                        "",           // NEXT: 다음 데이터 키 (초기 조회는 빈값)
                        "4",        // NREC: 조회할 데이터 수
                        "",           // FILL: 공백입력
                        ""            // KEYB: 1분 전 혹은 n분 전의 시간을 입력
                        );

                        // 여기에 일봉 파싱 로직
                        JObject jObj = JObject.Parse(StrDailyResultJson);
                        var output2 = jObj["output2"] as JArray;
                        if (output2 != null)
                        {
                            // 1일 고가
                            decimal latestHigh = decimal.Parse(output2.First()["high"]!.ToString());

                            // 1일 시가
                            decimal latestOpen = decimal.Parse(output2.First()["open"]!.ToString());

                            // 1일 거래대금
                            decimal latestEamt = decimal.Parse(output2.First()["eamt"]!.ToString());

                            // 1일 종가(현재가)
                            decimal latestLast = decimal.Parse(output2.First()["last"]!.ToString());

                            // 전일 종가
                            decimal prevClose = decimal.Parse(output2[1]["last"]!.ToString());

                            // 전일종가대비고가 (%) = ((1일고가 - 전일 종가) / 전일 종가) × 100
                            decimal highRate = ((latestHigh - prevClose) / prevClose) * 100;

                            // !(시가 > 종가) && !(전일종가 > 현재종가) && 거래대금 9백만달러이상 && 전일종가대비 고가 30 % 이상
                            if (latestOpen <= latestLast && prevClose <= latestLast && latestEamt >= 9000000 && highRate > 30)
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    // 이미 있는 종목이면 기존 객체를 찾아서 덮어쓰기
                                    var existingStock = clsClickedStockActionPlan.ClsParentSection.ObcStockList
                                    .FirstOrDefault(s => s.StrStockSymb == stock.StrStockSymb);

                                    if (existingStock != null)
                                    {
                                        existingStock.StrStockExcd = stock.StrStockExcd;
                                        existingStock.StrStockSymb = stock.StrStockSymb;
                                        existingStock.StrStockName = stock.StrStockName;
                                        existingStock.StrStockEordyn = stock.StrStockEordyn;

                                        existingStock.StrSearchMethod = clsClickedStockActionPlan.StrSearchMethod;
                                        existingStock.StrBuyMethod = clsClickedStockActionPlan.StrBuyMethod;
                                        existingStock.StrManualMethod = clsClickedStockActionPlan.StrManualMethod;
                                    }
                                    else
                                    {
                                        // 없는 종목이면 새로 추가
                                        var stockModel = new Stock
                                        {
                                            ClsParentSection = clsClickedStockActionPlan.ClsParentSection,
                                            StrStockExcd = stock.StrStockExcd,
                                            StrStockSymb = stock.StrStockSymb,
                                            StrStockName = stock.StrStockName,
                                            StrStockEordyn = stock.StrStockEordyn,

                                            StrSearchMethod = clsClickedStockActionPlan.StrSearchMethod,
                                            StrBuyMethod = clsClickedStockActionPlan.StrBuyMethod,
                                            StrManualMethod = clsClickedStockActionPlan.StrManualMethod
                                        };

                                        clsClickedStockActionPlan.ClsParentSection.ObcStockList.Add(stockModel);
                                    }

                                    // 여기서 Notify
                                    clsClickedStockActionPlan.RaiseFilteredListChanged();
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string StrLog = $" 종목매수 : {clsClickedStockActionPlan.StrBuyMethod}, 종목메뉴얼 : {clsClickedStockActionPlan.StrManualMethod} /// 1분봉조회 /// 종목 : {stock.StrStockSymb}] {ex.Message} /// JsonData : {StrDailyResultJson}";
                        StaticUtility.WriteLog(StrLog);
                        clsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                    }
                }
                #endregion

                // 위의 조건과 아래의 조건을 합해서 종목 선정

                tempStocks.Clear();

                // 조건검색  등락율 9%이상, 거래대금9백만달러 이상
                string StrJsonResult1 = await OpenApiHelper.SearchStockForAllStrategyAsync(
                   clsClickedStockActionPlan.ClsParentSection,
                   clsClickedStockActionPlan.ClsParentSection!.StrCommunicationUrl, // URL
                   clsClickedStockActionPlan.ClsParentSection.StrToken, // 토큰
                   clsClickedStockActionPlan.ClsParentSection.StrAppKey, // 앱키
                   clsClickedStockActionPlan.ClsParentSection.StrAppSecret, // 앱시크릿
                   "", // 사용자권한정보 ""(Null 값 설정)
                   "NAS", // 거래소코드
                   "", // 현재가선택조건
                   "", // 현재가시작범위가
                   "", // 현재가끝범위가
                   "1", // 등락율선택조건
                   "9", // 등락율시작율
                   "999999999999", // 등락율끝율
                   "", // 시가총액선택조건
                   "", // 시가총액시작액
                   "", // 시가총액끝액
                   "", // 발행주식수선택조건
                   "", // 발행주식시작수
                   "", // 발행주식끝수
                   "", // 거래량선택조건
                   "", // 거래량시작량
                   "", // 거래량끝량
                   "1", // 거래대금선택조건
                   "9000", // 거래대금시작금
                   "999999999999", // 거래대금끝금
                   "", // EPS선택조건
                   "", // EPS시작
                   "", // EPS끝
                   "", // PER선택조건
                   "", // PER시작
                   ""  // PER끝
                   );

                try
                {
                    var json = JObject.Parse(StrJsonResult1);
                    var stockArray = json["output2"];
                    if (stockArray != null)
                    {
                        foreach (var stock in stockArray)
                        {
                            tempStocks.Add(new Stock { StrStockExcd = stock["excd"]?.ToString(), StrStockSymb = stock["symb"]?.ToString(), StrStockName = stock["name"]?.ToString(), StrStockEordyn = stock["e_ordyn"]?.ToString() });
                        }
                    }
                }
                catch (Exception ex)
                {
                    string StrLog = $" 종목검색 : {clsClickedStockActionPlan.StrSearchMethod}, 종목메뉴얼 : {clsClickedStockActionPlan.StrManualMethod} /// JSON 파싱 예외] {ex.Message} /// JsonData : {StrJsonResult1}";
                    StaticUtility.WriteLog(StrLog);
                    clsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                }

                // 위의 조건에서, 전일 종가 대비고가가 30%이상이었던 종목 선정
                foreach (var stock in tempStocks)
                {
                    // 1일봉 조회
                    string StrDailyResultJson = "";
                    try
                    {
                        StrDailyResultJson = await OpenApiHelper.GetMinuteCandlestickAsync(
                        clsClickedStockActionPlan.ClsParentSection,
                        clsClickedStockActionPlan.ClsParentSection.StrCommunicationUrl,
                        clsClickedStockActionPlan.ClsParentSection.StrToken,
                        clsClickedStockActionPlan.ClsParentSection.StrAppKey,
                        clsClickedStockActionPlan.ClsParentSection.StrAppSecret,
                        "",           // AUTH, 공백입력
                        stock.StrStockExcd,         // 거래소 코드
                        stock.StrStockSymb,         // 종목 코드
                        "1440",       // NMIN: 분단위(1: 1분봉, 2: 2분봉, ...)
                        "1",          // PINC: 	0:당일 1:전일포함
                        "",           // NEXT: 다음 데이터 키 (초기 조회는 빈값)
                        "4",        // NREC: 조회할 데이터 수
                        "",           // FILL: 공백입력
                        ""            // KEYB: 1분 전 혹은 n분 전의 시간을 입력
                        );

                        // 여기에 일봉 파싱 로직
                        JObject jObj = JObject.Parse(StrDailyResultJson);
                        var output2 = jObj["output2"] as JArray;
                        if (output2 != null)
                        {
                            // 1일 고가
                            decimal latestHigh = decimal.Parse(output2.First()["high"]!.ToString());

                            // 전일 종가
                            decimal prevClose = decimal.Parse(output2[1]["last"]!.ToString());

                            // 전일종가대비고가 (%) = ((1일고가 - 전일 종가) / 전일 종가) × 100
                            decimal highRate = ((latestHigh - prevClose) / prevClose) * 100;

                            // !(시가 > 종가) && !(전일종가 > 현재종가) && 거래대금 9백만달러이상 && 전일종가대비 고가 30 % 이상
                            if (highRate > 30)
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    // 이미 있는 종목이면 기존 객체를 찾아서 덮어쓰기
                                    var existingStock = clsClickedStockActionPlan.ClsParentSection.ObcStockList
                                    .FirstOrDefault(s => s.StrStockSymb == stock.StrStockSymb);

                                    if (existingStock != null)
                                    {
                                        existingStock.StrStockExcd = stock.StrStockExcd;
                                        existingStock.StrStockSymb = stock.StrStockSymb;
                                        existingStock.StrStockName = stock.StrStockName;
                                        existingStock.StrStockEordyn = stock.StrStockEordyn;

                                        existingStock.StrSearchMethod = clsClickedStockActionPlan.StrSearchMethod;
                                        existingStock.StrBuyMethod = clsClickedStockActionPlan.StrBuyMethod;
                                        existingStock.StrManualMethod = clsClickedStockActionPlan.StrManualMethod;
                                    }
                                    else
                                    {
                                        // 없는 종목이면 새로 추가
                                        var stockModel = new Stock
                                        {
                                            ClsParentSection = clsClickedStockActionPlan.ClsParentSection,
                                            StrStockExcd = stock.StrStockExcd,
                                            StrStockSymb = stock.StrStockSymb,
                                            StrStockName = stock.StrStockName,
                                            StrStockEordyn = stock.StrStockEordyn,

                                            StrSearchMethod = clsClickedStockActionPlan.StrSearchMethod,
                                            StrBuyMethod = clsClickedStockActionPlan.StrBuyMethod,
                                            StrManualMethod = clsClickedStockActionPlan.StrManualMethod
                                        };

                                        clsClickedStockActionPlan.ClsParentSection.ObcStockList.Add(stockModel);
                                    }

                                    // 여기서 Notify
                                    clsClickedStockActionPlan.RaiseFilteredListChanged();
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string StrLog = $" 종목매수 : {clsClickedStockActionPlan.StrBuyMethod}, 종목메뉴얼 : {clsClickedStockActionPlan.StrManualMethod} /// 1분봉조회 /// 종목 : {stock.StrStockSymb}] {ex.Message} /// JsonData : {StrDailyResultJson}";
                        StaticUtility.WriteLog(StrLog);
                        clsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                    }
                }



            }
        }
        #endregion

        // 종목 매수 분기처리
        #region
        private async Task DoBuyStockAsync(StockActionPlan clsClickedStockActionPlan)
        {
            if (clsClickedStockActionPlan == null || clsClickedStockActionPlan.BlIsActive == false)
            {
                return;
            }

            // 여기서 각각의 선택한 플랜에 대한 종목을 가져온다
            var StockList = clsClickedStockActionPlan.ClsParentSection!.ObcStockList.Where(s =>
                    s.StrSearchMethod == clsClickedStockActionPlan.StrSearchMethod &&
                    s.StrBuyMethod == clsClickedStockActionPlan.StrBuyMethod &&
                    s.StrManualMethod == clsClickedStockActionPlan.StrManualMethod).ToList(); // ToList()를 사용해서, 순회 도중에 종목이 삭제가 되었을때 Task예외가 발생되는 것을 방지함, ToList()는 종목을 복사해 가기 때문


            // ABC종목매수 or 과매도30인 경우
            if ( (clsClickedStockActionPlan.StrSearchMethod == "ㅡ선택안함ㅡ" &&
                clsClickedStockActionPlan.StrBuyMethod == "ㅡ선택안함ㅡ" &&
                clsClickedStockActionPlan.StrManualMethod == "ABC종목매수") ||

                (clsClickedStockActionPlan.StrBuyMethod == "과매도30" &&
                clsClickedStockActionPlan.StrManualMethod == "ㅡ선택안함ㅡ") )
            {
                foreach (var stock in StockList)
                {
                    string symb = stock.StrStockSymb ?? "";
                    string excd = stock.StrStockExcd ?? "";

                    if (string.IsNullOrEmpty(symb) || string.IsNullOrEmpty(excd))
                        continue;


                    // 일봉조회(피보나치 만들기)
                    if (stock.DtLastDailyRequestTime == null ||
                        (DateTime.Now - stock.DtLastDailyRequestTime.Value).TotalSeconds >= 60)
                    {
                        string StrDailyResultJson = "";
                        try
                        {
                            StrDailyResultJson = await OpenApiHelper.GetMinuteCandlestickAsync(
                            clsClickedStockActionPlan.ClsParentSection,
                            clsClickedStockActionPlan.ClsParentSection.StrCommunicationUrl,
                            clsClickedStockActionPlan.ClsParentSection.StrToken,
                            clsClickedStockActionPlan.ClsParentSection.StrAppKey,
                            clsClickedStockActionPlan.ClsParentSection.StrAppSecret,
                            "",           // AUTH, 공백입력
                            excd,         // 거래소 코드
                            symb,         // 종목 코드
                            "1440",       // NMIN: 분단위(1: 1분봉, 2: 2분봉, ...)
                            "1",          // PINC: 	0:당일 1:전일포함
                            "",           // NEXT: 다음 데이터 키 (초기 조회는 빈값)
                            "4",        // NREC: 조회할 데이터 수
                            "",           // FILL: 공백입력
                            ""            // KEYB: 1분 전 혹은 n분 전의 시간을 입력
                            );

                            // 여기에 일봉 파싱 로직
                            JObject jObj = JObject.Parse(StrDailyResultJson);
                            var output2 = jObj["output2"] as JArray;

                            if (output2 != null && output2.Count >= 4)
                            {
                                // 1일 고가
                                decimal latestHigh = decimal.Parse(output2.First()["high"]!.ToString());

                                // 1일 저가
                                decimal latestLow = decimal.Parse(output2.First()["low"]!.ToString());

                                // 1일 시가
                                decimal latestOpen = decimal.Parse(output2.First()["open"]!.ToString());

                                // 1일 거래대금
                                decimal latestEamt = decimal.Parse(output2.First()["eamt"]!.ToString());

                                // 1일 종가(현재가)
                                decimal latestLast = decimal.Parse(output2.First()["last"]!.ToString());

                                // 전일 종가
                                decimal prevClose = decimal.Parse(output2[1]["last"]!.ToString());

                                // 등락률 (%) = ((현재 종가 - 전일 종가) / 전일 종가) × 100
                                decimal latesRate = ((latestLast - prevClose) / prevClose) * 100;

                                // 최근 3일 고가
                                var highs = output2
                                    .Take(3)
                                    .Select(x => decimal.Parse(x["high"]!.ToString()))
                                    .ToList();

                                // 최근 4일 저가
                                var lows = output2
                                    .Take(4)
                                    .Select(x => decimal.Parse(x["low"]!.ToString()))
                                    .ToList();

                                stock.DmDayLast = latestLast; // 현재가
                                stock.DmDayHigh = latestHigh; // 하루고가
                                stock.DmDayLow = latestLow; // 하루저가
                                stock.DmDayEamt = latestLow; // 하루거래량
                                stock.DmDayRate = latesRate; // 하루등락률
                                stock.DmThreeDayHigh = highs.Max(); // 최근 3일 고가
                                stock.DmFourDayLow = lows.Min(); // 최근 4일 저가

                                // 피보나치세팅
                                stock.DmFibonacci_Red = Math.Round((decimal)(((stock.DmThreeDayHigh - stock.DmFourDayLow) * (decimal)0.764) + stock.DmFourDayLow), 4);
                                stock.DmFibonacci_Orange = Math.Round((decimal)(((stock.DmThreeDayHigh - stock.DmFourDayLow) * (decimal)0.618) + stock.DmFourDayLow), 4);
                                stock.DmFibonacci_Green = Math.Round((decimal)(((stock.DmThreeDayHigh - stock.DmFourDayLow) * (decimal)0.5) + stock.DmFourDayLow), 4);
                                stock.DmFibonacci_Blue = Math.Round((decimal)(((stock.DmThreeDayHigh - stock.DmFourDayLow) * (decimal)0.375) + stock.DmFourDayLow), 4);
                                stock.DmFibonacci_Purple = Math.Round((decimal)(((stock.DmThreeDayHigh - stock.DmFourDayLow) * (decimal)0.25) + stock.DmFourDayLow), 4);
                            }
                        }
                        catch (Exception ex)
                        {
                            string StrLog = $" 종목매수 : {clsClickedStockActionPlan.StrBuyMethod}, 종목메뉴얼 : {clsClickedStockActionPlan.StrManualMethod} /// 주기별 데이터 파싱 오류 /// 종목 : {symb}] {ex.Message} /// JsonData : {StrDailyResultJson}";
                            StaticUtility.WriteLog(StrLog);
                            clsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                        }
                        finally
                        {
                            stock.DtLastDailyRequestTime = DateTime.Now;
                        }
                    }


                    // 분봉조회
                    string StrMinuteResultJson = "";
                    try
                    {
                        StrMinuteResultJson = await OpenApiHelper.GetMinuteCandlestickAsync(
                        clsClickedStockActionPlan.ClsParentSection,
                        clsClickedStockActionPlan.ClsParentSection.StrCommunicationUrl,
                        clsClickedStockActionPlan.ClsParentSection.StrToken,
                        clsClickedStockActionPlan.ClsParentSection.StrAppKey,
                        clsClickedStockActionPlan.ClsParentSection.StrAppSecret,
                        "",           // AUTH, 공백입력
                        excd,         // 거래소 코드
                        symb,         // 종목 코드
                        "1",          // NMIN: 1분봉, 분갭
                        "1",          // PINC: 	0:당일 1:전일포함
                        "",           // NEXT: 다음 데이터 키 (초기 조회는 빈값)
                        "120",        // NREC: 조회할 데이터 수
                        "",           // FILL: 공백입력
                        ""            // KEYB: 1분 전 혹은 n분 전의 시간을 입력
                        );

                        //string StrLog = $"[분봉 데이터: {symb}] {StrMinuteResultJson}";
                        //StaticUtility.WriteLog(StrLog);

                        var jObj = JObject.Parse(StrMinuteResultJson);
                        var outputArray = jObj["output2"];

                        if (outputArray != null)
                        {
                            // 분봉 종가 리스트 추출 (예: RSI 계산용)
                            List<decimal> closingPrices = outputArray
                                .Select(o => decimal.TryParse(o["last"]?.ToString(), out decimal val) ? val : 0)
                                .Where(v => v > 0)
                                .Reverse()  // 최신이 뒤로 가게
                                .ToList();

                            decimal? rsi = StaticUtility.CalculateRsi(closingPrices);
                            stock.DmDayRsi = rsi;

                            // 현재가대입
                            var firstItem = outputArray.FirstOrDefault();
                            if (firstItem != null && decimal.TryParse(firstItem["last"]?.ToString(), out decimal last))
                            {
                                stock.DmDayLast = last;
                            }
                            if (stock.DmDayLast > stock.DmDayHigh)
                                stock.DmDayHigh = stock.DmDayLast;
                            else if (stock.DmDayLast < stock.DmDayLow)
                                stock.DmDayLow = stock.DmDayLast;


                            // RSI계산과, 예측 RSI계산
                            stock.UpdateSimulatedRsiInfo(closingPrices);

                            // 현재가와 피보나치를 비교하기
                            stock.UpdateFibonacciComparison();

                            // 해당종목의 %선 가격 구하기
                            stock.UpdateHighDropAnalysis();

                            // 시간 최신화
                            stock.DtRsiCheckedAt = DateTime.Now;
                        }
                    }
                    catch (Exception ex)
                    {
                        string StrLog = $" 종목매수 : {clsClickedStockActionPlan.StrBuyMethod}, 종목메뉴얼 : {clsClickedStockActionPlan.StrManualMethod} /// 주기별 데이터 파싱 오류 /// 종목 : {symb}] {ex.Message} /// JsonData : {StrMinuteResultJson}";
                        StaticUtility.WriteLog(StrLog);
                        clsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                    }




                    //매수하기
                    //string StrBuyOrderJson = await OpenApiHelper.SendBuyOrderAsync(
                    //    clsClickedStockActionPlan.ClsParentSection, //종합계좌번호
                    //    "01",   // 계좌상품코드
                    //    "NASD", // 해외거래소코드
                    //    symb,   // 종목코드
                    //    "1",    // 주문수량
                    //    $"{stock.StrStockLast}",     // 해외주문단가
                    //    "",     // 판매유형(제거 : 매수, 00 : 매도)
                    //    "0",    // 주문서버구분코드(고정값)
                    //    "00"    // 주문구분
                    //    );

                    //try
                    //{
                    //    var jObj = JObject.Parse(StrBuyOrderJson);
                    //    var rtCd = jObj["rt_cd"].ToString();

                    //    if (rtCd == "0")
                    //    {
                    //        string StrLog = $"{symb} : 매수완료";
                    //        StaticUtility.WriteLog(StrLog);
                    //        clsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                    //    }
                    //    else
                    //    {
                    //        StaticUtility.WriteLog(StrBuyOrderJson);
                    //        clsClickedStockActionPlan.StrMessage = $"{StrBuyOrderJson} ({DateTime.Now:HH:mm:ss.fff})";
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    string StrLog = $"[종목 매수 파싱 오류: {symb}] {ex.Message} /// json : {StrBuyOrderJson}";
                    //    StaticUtility.WriteLog(StrLog);
                    //    clsClickedStockActionPlan.StrMessage = $"{StrLog} ({DateTime.Now:HH:mm:ss.fff})";
                    //}

                    // 종목마다 대기
                    await Task.Delay(TimeSpan.FromMilliseconds(200));
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string? propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
