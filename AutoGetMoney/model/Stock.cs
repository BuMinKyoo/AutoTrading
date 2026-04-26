using AutoGetMoney.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace AutoGetMoney.Model
{
    public class Stock : INotifyPropertyChanged
    {

        // 거래소코드
        private string? _strStockExcd;
        public string? StrStockExcd
        {
            get { return _strStockExcd; }
            set
            {
                if (_strStockExcd != value)
                {
                    _strStockExcd = value;
                    Notify();
                }
            }
        }

        // 종목명
        private string? _strStockName;
        public string? StrStockName
        {
            get { return _strStockName; }
            set
            {
                if (_strStockName != value)
                {
                    _strStockName = value;
                    Notify();
                }
            }
        }

        // 종목코드
        private string? _strStockSymb;
        public string? StrStockSymb
        {
            get { return _strStockSymb; }
            set
            {
                if (_strStockSymb != value)
                {
                    _strStockSymb = value;
                    Notify();
                }
            }
        }

        // 현재가
        private decimal? _dmDayLast;
        public decimal? DmDayLast
        {
            get { return _dmDayLast; }
            set
            {
                if (_dmDayLast != value)
                {
                    _dmDayLast = value;
                    Notify();
                }
            }
        }

        // 저가
        private decimal? _dmDayLow;
        public decimal? DmDayLow
        {
            get { return _dmDayLow; }
            set
            {
                if (_dmDayLow != value)
                {
                    _dmDayLow = value;
                    Notify();
                }
            }
        }

        // 시가
        private decimal? _dmDayOpen;
        public decimal? DmDayOpen
        {
            get { return _dmDayOpen; }
            set
            {
                if (_dmDayOpen != value)
                {
                    _dmDayOpen = value;
                    Notify();
                }
            }
        }


        // 등락율 (%)
        private decimal? _dmDayRate;
        public decimal? DmDayRate
        {
            get { return _dmDayRate; }
            set
            {
                if (_dmDayRate != value)
                {
                    _dmDayRate = value;
                    Notify();
                }
            }
        }

        // 거래대금 (단위: 천)
        private decimal? _dmDayEamt;
        public decimal? DmDayEamt
        {
            get { return _dmDayEamt; }
            set
            {
                if (_dmDayEamt != value)
                {
                    _dmDayEamt = value;
                    Notify();
                }
            }
        }

        // 거래량
        private int? _inTvol;
        public int? InTvol
        {
            get { return _inTvol; }
            set
            {
                if (_inTvol != value)
                {
                    _inTvol = value;
                    Notify();
                }
            }
        }


        // 매매가능 여부
        private string? _strStockEordyn;
        public string? StrStockEordyn
        {
            get { return _strStockEordyn; }
            set
            {
                if (_strStockEordyn != value)
                {
                    _strStockEordyn = value;
                    Notify();
                }
            }
        }

        // 어떤 StockActionPlan인지 확인하기 위함
        private string? _strSearchMethod;
        public string? StrSearchMethod
        {
            get { return _strSearchMethod; }
            set
            {
                if (_strSearchMethod != value)
                {
                    _strSearchMethod = value;
                    Notify();
                }
            }
        }

        // 어떤 StockActionPlan인지 확인하기 위함
        private string? _strBuyMethod;
        public string? StrBuyMethod
        {
            get { return _strBuyMethod; }
            set
            {
                if (_strBuyMethod != value)
                {
                    _strBuyMethod = value;
                    Notify();
                }
            }
        }

        // 어떤 StockActionPlan인지 확인하기 위함
        private string? _strManualMethod;
        public string? StrManualMethod
        {
            get { return _strManualMethod; }
            set
            {
                if (_strManualMethod != value)
                {
                    _strManualMethod = value;
                    Notify();
                }
            }
        }

        // RSI 조회 시간
        private DateTime? _dtRsiCheckedAt;
        public DateTime? DtRsiCheckedAt
        {
            get { return _dtRsiCheckedAt; }
            set
            {
                if (_dtRsiCheckedAt != value)
                {
                    _dtRsiCheckedAt = value;
                    Notify();
                }
            }
        }

        // 현재 RSI값
        private decimal? _dmDayRsi;
        public decimal? DmDayRsi
        {
            get { return _dmDayRsi; }
            set
            {
                if (_dmDayRsi != value)
                {
                    _dmDayRsi = value;
                    Notify();
                }
            }
        }


        // RSI값
        private string? _strRsiValue;
        public string? StrRsiValue
        {
            get { return _strRsiValue; }
            set
            {
                if (_strRsiValue != value)
                {
                    _strRsiValue = value;
                    Notify();
                }
            }
        }

        // RSI값 시뮬레이터
        private string? _strSimulatedRsi;
        public string? StrSimulatedRsi
        {
            get { return _strSimulatedRsi; }
            set
            {
                if (_strSimulatedRsi != value)
                {
                    _strSimulatedRsi = value;
                    Notify();
                }
            }
        }

        // 일일 데이터는 1분에 1번만 조회하도록 하기 위한 시간타임 확인
        private DateTime? _dtLastDailyRequestTime;
        public DateTime? DtLastDailyRequestTime
        {
            get { return _dtLastDailyRequestTime; }
            set
            {
                if (_dtLastDailyRequestTime != value)
                {
                    _dtLastDailyRequestTime = value;
                    Notify();
                }
            }
        }

        // 부모 Section 참조 추가
        private Section? _clsParentSection;
        [JsonIgnore] // 직렬화 시 순환참조 방지
        public Section? ClsParentSection
        {
            get { return _clsParentSection; }
            set
            {
                _clsParentSection = value;
                Notify();
            }
        }

        // 하루동안 고가
        private decimal? _dmDayHigh;
        public decimal? DmDayHigh
        {
            get { return _dmDayHigh; }
            set
            {
                if (_dmDayHigh != value)
                {
                    _dmDayHigh = value;
                    Notify();
                }
            }
        }


        // 3일동안의 고가
        private decimal? _dmThreeDayHigh;
        public decimal? DmThreeDayHigh
        {
            get { return _dmThreeDayHigh; }
            set
            {
                if (_dmThreeDayHigh != value)
                {
                    _dmThreeDayHigh = value;
                    Notify();
                }
            }
        }

        // 4일동안의 저가
        private decimal? _dmFourDayLow;
        public decimal? DmFourDayLow
        {
            get { return _dmFourDayLow; }
            set
            {
                if (_dmFourDayLow != value)
                {
                    _dmFourDayLow = value;
                    Notify();
                }
            }
        }

        // 피보나치 1선(빨)
        private decimal? _dmFibonacci_Red;
        public decimal? DmFibonacci_Red
        {
            get { return _dmFibonacci_Red; }
            set
            {
                if (_dmFibonacci_Red != value)
                {
                    _dmFibonacci_Red = value;
                    Notify();
                }
            }
        }

        // 피보나치 2선(주)
        private decimal? _dmFibonacci_Orange;
        public decimal? DmFibonacci_Orange
        {
            get { return _dmFibonacci_Orange; }
            set
            {
                if (_dmFibonacci_Orange != value)
                {
                    _dmFibonacci_Orange = value;
                    Notify();
                }
            }
        }

        // 피보나치 3선(초)
        private decimal? _dmFibonacci_Green;
        public decimal? DmFibonacci_Green
        {
            get { return _dmFibonacci_Green; }
            set
            {
                if (_dmFibonacci_Green != value)
                {
                    _dmFibonacci_Green = value;
                    Notify();
                }
            }
        }

        // 피보나치 4선(파)
        private decimal? _dmFibonacci_Blue;
        public decimal? DmFibonacci_Blue
        {
            get { return _dmFibonacci_Blue; }
            set
            {
                if (_dmFibonacci_Blue != value)
                {
                    _dmFibonacci_Blue = value;
                    Notify();
                }
            }
        }

        // 피보나치 5선(보)
        private decimal? _dmFibonacci_Purple;
        public decimal? DmFibonacci_Purple
        {
            get { return _dmFibonacci_Purple; }
            set
            {
                if (_dmFibonacci_Purple != value)
                {
                    _dmFibonacci_Purple = value;
                    Notify();
                }
            }
        }

        // 피보나치 비교
        private string? _strFibonacciComparison;
        public string? StrFibonacciComparison
        {
            get { return _strFibonacciComparison; }
            set
            {
                if (_strFibonacciComparison != value)
                {
                    _strFibonacciComparison = value;
                    Notify();
                }
            }
        }

        // 최고점 대비 현재 위치 구간 설명 (예: "13%~18%: -15%") 
        private string? _strHighDropPercentRange;
        public string? StrHighDropPercentRange
        {
            get { return _strHighDropPercentRange; }
            set
            {
                if (_strHighDropPercentRange != value)
                {
                    _strHighDropPercentRange = value;
                    Notify();
                }
            }
        }

        // 현 위치에서 구간 양쪽으로 도달하는 데 필요한 퍼센트 및 가격(예: "13%: 13750(+2.23%), 18%: 13200(-1.45%)")
        private string? _strHighDropPercentTargetInfo;
        public string? StrHighDropPercentTargetInfo
        {
            get { return _strHighDropPercentTargetInfo; }
            set
            {
                if (_strHighDropPercentTargetInfo != value)
                {
                    _strHighDropPercentTargetInfo = value;
                    Notify();
                }
            }
        }

        // RSI 예측 및 시뮬레이션 정보 업데이트 함수
        public void UpdateSimulatedRsiInfo(List<decimal> prices)
        {
            if (DmDayLast == null || DmDayLast <= 0 || DmDayRsi == null)
                return;

            decimal rsiValue = DmDayRsi.Value;
            decimal simulatedRsi = 0;

            if (rsiValue > 30) simulatedRsi = 30;
            else if (rsiValue > 25) simulatedRsi = 25;
            else if (rsiValue > 20) simulatedRsi = 20;
            else if (rsiValue > 15) simulatedRsi = 15;

            decimal simulatedRsi2 = simulatedRsi - 5;

            // 라벨 구간
            string RsiLabel(decimal val)
            {
                if (val <= 15) return "과15";
                if (val <= 20) return "과20";
                if (val <= 25) return "과25";
                if (val <= 30) return "과30";
                return "없음";
            }

            string StrRsiV = RsiLabel(rsiValue);
            string StrSimulatedRsiV = RsiLabel(simulatedRsi);
            string StrSimulatedRsiV2 = RsiLabel(simulatedRsi2);

            decimal? price1 = (decimal?)StaticUtility.FindTargetPriceForRsi(prices, simulatedRsi);
            decimal? price2 = (decimal?)StaticUtility.FindTargetPriceForRsi(prices, simulatedRsi2);

            string drop1 = "";
            string drop2 = "";
            if (price1 != null && price2 != null)
            {
                drop1 = StaticUtility.GetDropPercent(DmDayLast.Value, price1.Value);
                drop2 = StaticUtility.GetDropPercent(DmDayLast.Value, price2.Value);
            }

            StrRsiValue = $"현재가: {DmDayLast:0.####}({rsiValue:0.##},{StrRsiV})";
            StrSimulatedRsi = $"{StrSimulatedRsiV}: {price1:0.####}({drop1}), {StrSimulatedRsiV2}: {price2:0.####}({drop2})";
        }


        public void UpdateFibonacciComparison()
        {
            if (DmDayLast == 0)
                return;

            string DmFibonacciDate = "";
            string DmFibonacciDateSecond = "";

            var levels = new[]
            {
                DmFibonacci_Red,
                DmFibonacci_Orange,
                DmFibonacci_Green,
                DmFibonacci_Blue,
                DmFibonacci_Purple
            };

            string[] colorNames = { "Rd", "Og", "Gn", "Bl", "Pr" };

            // 상단 방향 비교: stockLast < 기준선 → 상승 목표
            for (int i = levels.Length - 1; i >= 0; i--)
            {
                var level = levels[i];
                if (level.HasValue && DmDayLast < level.Value)
                {
                    decimal? percentUp = ((level.Value - DmDayLast) / DmDayLast) * 100;
                    DmFibonacciDate = $"{colorNames[i]}: {level.Value:0.##} (+{percentUp:0.##}%)";
                    break;
                }
            }

            // 하단 방향 비교: stockLast > 기준선 → 하락 목표
            for (int i = 0; i < levels.Length; i++)
            {
                var level = levels[i];
                if (level.HasValue && DmDayLast > level.Value)
                {
                    decimal? percentDown = ((DmDayLast - level.Value) / DmDayLast) * 100;
                    DmFibonacciDateSecond = $"{colorNames[i]}: {level.Value:0.##} (-{percentDown:0.##}%)";
                    break;
                }
            }

            // 문자열 조합
            StrFibonacciComparison = $"{DmFibonacciDate}, {DmFibonacciDateSecond}";
        }

        public void UpdateHighDropAnalysis()
        {
            // 현재가와 최고가를 문자열에서 decimal로 파싱
            if (DmDayHigh <= 0 || DmDayLast <= 0) return;

            // 최고점 대비 몇 % 떨어졌는지 계산
            decimal? dropPercent = ((DmDayHigh - DmDayLast) / DmDayHigh) * 100;

            // 기준 퍼센트 라인 정의
            decimal[] percentLines = { 4, 7, 9, 13, 18, 23, 28, 33, 38, 43, 48, 53, 56 };

            // 포함된 구간 찾기
            for (int i = 0; i < percentLines.Length - 1; i++)
            {
                decimal lower = percentLines[i];
                decimal upper = percentLines[i + 1];

                if (dropPercent >= lower && dropPercent <= upper)
                {
                    // 예: "13%~18%: -15.22%"
                    StrHighDropPercentRange = $"{lower}%~{upper}%: -{dropPercent:0.##}%";

                    // 각 퍼센트 기준 가격
                    decimal? priceAtLower = DmDayHigh * (1 - (lower / 100));
                    decimal? priceAtUpper = DmDayHigh * (1 - (upper / 100));

                    // 현재가 대비 퍼센트 변화 계산
                    decimal? percentToLower = ((priceAtLower - DmDayLast) / DmDayLast) * 100;
                    decimal? percentToUpper = ((priceAtUpper - DmDayLast) / DmDayLast) * 100;

                    // 부호 표시
                    string signLower = percentToLower >= 0 ? "+" : "";
                    string signUpper = percentToUpper >= 0 ? "+" : "";

                    // 예: "13%: 13750(+2.3%), 18%: 13200(-1.5%)"
                    StrHighDropPercentTargetInfo = $"{lower}%: {priceAtLower:0.####}( {signLower}{percentToLower:0.##}%), " +
                                                   $"{upper}%: {priceAtUpper:0.####}( {signUpper}{percentToUpper:0.##}%)";

                    break;
                }
            }
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
