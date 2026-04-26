using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoGetMoney.Model
{
    public class Section : INotifyPropertyChanged
    {
        public Section()
        {
            SelectedExchange = "나스닥 (NAS)";  // 초기 선택값 설정!
            StartRateLimitTimer();
        }

        // 계좌 순서
        private int _inSectionNo;
        public int InSectionNo
        {
            get { return _inSectionNo; }
            set
            {
                if (_inSectionNo != value)
                {
                    _inSectionNo = value;
                    Notify();
                }
            }
        }

        // 통신url
        private string? _strCommunicationUrl;
        public string? StrCommunicationUrl
        {
            get { return _strCommunicationUrl; }
            set
            {
                if (_strCommunicationUrl != value)
                {
                    _strCommunicationUrl = value;
                    Notify();

                    // 통신 URL에 따라 호출 제한 자동 설정
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (value.Contains("https://openapivts.koreainvestment.com:29443")) // 모의투자
                        {
                            MaxApiCallPerSecond = 2;
                        }
                        else if (value.Contains("https://openapi.koreainvestment.com:9443")) // 실전투자
                        {
                            MaxApiCallPerSecond = 20;
                        }
                    }
                }
            }
        }

        // 계좌번호
        private string? _strAccountNumber;
        public string? StrAccountNumber
        {
            get { return _strAccountNumber; }
            set
            {
                if (_strAccountNumber != value)
                {
                    _strAccountNumber = value;
                    Notify();
                }
            }
        }

        // 앱키
        private string? _strAppKey;
        public string? StrAppKey
        {
            get { return _strAppKey; }
            set
            {
                if (_strAppKey != value)
                {
                    _strAppKey = value;
                    Notify();
                }
            }
        }

        // 앱시크릿
        private string? _strAppSecret;
        public string? StrAppSecret
        {
            get { return _strAppSecret; }
            set
            {
                if (_strAppSecret != value)
                {
                    _strAppSecret = value;
                    Notify();
                }
            }
        }

        // 웹접속키
        private string? _strWebKey;
        public string? StrWebKey
        {
            get { return _strWebKey; }
            set
            {
                if (_strWebKey != value)
                {
                    _strWebKey = value;
                    Notify();
                }
            }
        }

        // 토큰
        private string? _strToken;
        public string? StrToken
        {
            get { return _strToken; }
            set
            {
                if (_strToken != value)
                {
                    _strToken = value;
                    Notify();
                }
            }
        }

        // 종목들
        private ObservableCollection<Stock> _obcStockList = new ObservableCollection<Stock>();
        public ObservableCollection<Stock> ObcStockList
        {
            get { return _obcStockList; }
            set
            {
                _obcStockList = value;
                Notify();
            }
        }

        // 방법론
        private ObservableCollection<StockActionPlan> _obcStockActionPlanList = new ObservableCollection<StockActionPlan>();
        public ObservableCollection<StockActionPlan> ObcStockActionPlanList
        {
            get { return _obcStockActionPlanList; }
            set
            {
                _obcStockActionPlanList = value;
                Notify();
            }
        }

        // 종목검색 방법(수동 종목 추가시 사용)
        private string? _strSearchMethod_Add;
        public string? StrSearchMethod_Add
        {
            get { return _strSearchMethod_Add; }
            set
            {
                if (_strSearchMethod_Add != value)
                {
                    _strSearchMethod_Add = value;
                    Notify();
                }
            }
        }

        // 종목매수방법(수동 종목 추가시 사용)
        private string? _strBuyMethod_Add;
        public string? StrBuyMethod_Add
        {
            get { return _strBuyMethod_Add; }
            set
            {
                if (_strBuyMethod_Add != value)
                {
                    _strBuyMethod_Add = value;
                    Notify();
                }
            }
        }

        // 메뉴얼 방법(수동 종목 추가시 사용)
        private string? _strManualMethod_Add;
        public string? StrManualMethod_Add
        {
            get { return _strManualMethod_Add; }
            set
            {
                if (_strManualMethod_Add != value)
                {
                    _strManualMethod_Add = value;
                    Notify();
                }
            }
        }


        // 거래소(수동 종목 추가시 사용)
        private ObservableCollection<string> _obcStockExcdComboBoxItem_Add = new ObservableCollection<string>
        {
            "나스닥 (NAS)",
            "뉴욕 (NYS)",
            "아멕스 (AMS)",
            "홍콩 (HKS)",
            "중국상해 (SHS)",
            "중국심천 (SZS)",
            "도쿄 (TSE)",
            "베트남 하노이 (HNX)",
            "베트남 호치민 (HSX)"
        };
        public ObservableCollection<string> ObcStockExcdComboBoxItem_Add
        {
            get { return _obcStockExcdComboBoxItem_Add; }
            set
            {
                _obcStockExcdComboBoxItem_Add = value;
                Notify();
            }
        }

        private string? _selectedExchange;
        public string? SelectedExchange
        {
            get { return _selectedExchange; }
            set
            {
                _selectedExchange = value;
                Notify();
                // 여기서 코드 추출
                if (!string.IsNullOrEmpty(value))
                {
                    // 예시: "나스닥 (NAS)" → "NAS"
                    var code = value.Split('(', ')');
                    StrStockExcd_Add = code.Length >= 2 ? code[1] : null;
                }
            }
        }

        // Tag값
        private string? _strStockExcd_Add;
        public string? StrStockExcd_Add
        {
            get { return _strStockExcd_Add; }
            set
            {
                _strStockExcd_Add = value;
                Notify();
            }
        }

        // 종목코드 (수동 종목 추가시 사용)
        private string? _strStockSymb_Add;
        public string? StrStockSymb_Add
        {
            get { return _strStockSymb_Add; }
            set
            {
                if (_strStockSymb_Add != value)
                {
                    _strStockSymb_Add = value;
                    Notify();
                }
            }
        }

        // 종목명(수동 종목 추가시 사용)
        private string? _strStockName_Add;
        public string? StrStockName_Add
        {
            get { return _strStockName_Add; }
            set
            {
                if (_strStockName_Add != value)
                {
                    _strStockName_Add = value;
                    Notify();
                }
            }
        }

        // API제한 때문에 추가 실전 1초에 20회, 모의 1초에 2회
        private int _inApiCallCount = 0;
        public int InApiCallCount
        {
            get { return _inApiCallCount; }
            set
            {
                if (_inApiCallCount != value)
                {
                    _inApiCallCount = value;
                    Notify();
                }
            }
        }

        private System.Timers.Timer? _rateResetTimer;
        public int MaxApiCallPerSecond { get; set; } = 2;
        private readonly object _lock = new object();

        private void StartRateLimitTimer()
        {
            _rateResetTimer = new System.Timers.Timer(1000); // 1초
            _rateResetTimer.Elapsed += (s, e) =>
            {
                InApiCallCount = 0;
            };
            _rateResetTimer.AutoReset = true;
            _rateResetTimer.Start();
        }

        public async Task WaitIfApiLimitExceededAsync()
        {
            while (InApiCallCount >= MaxApiCallPerSecond)
            {
                await Task.Delay(1000); // 1초씩 기다림
            }

            InApiCallCount++;
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
