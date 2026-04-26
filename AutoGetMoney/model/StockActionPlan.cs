using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace AutoGetMoney.Model
{
    public class StockActionPlan : INotifyPropertyChanged
    {
        // 순서
        private int _inStockActionPlanNo;
        public int InStockActionPlanNo
        {
            get { return _inStockActionPlanNo; }
            set
            {
                if (_inStockActionPlanNo != value)
                {
                    _inStockActionPlanNo = value;
                    Notify();
                }
            }
        }

        // 종목 검색 방법
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

                    RaiseFilteredListChanged();
                }
            }
        }

        // 종목 매수 방법
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

                    RaiseFilteredListChanged();
                }
            }
        }

        // 메뉴얼 방법
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

                    RaiseFilteredListChanged();
                }
            }
        }

        // 플랜 동작중/정지중
        private bool? _blIsActive;
        public bool? BlIsActive
        {
            get { return _blIsActive; }
            set
            {
                if (_blIsActive != value)
                {
                    _blIsActive = value;
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

        private CancellationTokenSource? _ctsSearch;
        [JsonIgnore] // 직렬화 시 순환참조 방지
        public CancellationTokenSource? CtsSearch
        {
            get { return _ctsSearch; }
            set
            {
                _ctsSearch = value;
                Notify();
            }
        }

        private CancellationTokenSource? _ctsBuy;
        [JsonIgnore] // 직렬화 시 순환참조 방지
        public CancellationTokenSource? CtsBuy
        {
            get { return _ctsBuy; }
            set
            {
                _ctsBuy = value;
                Notify();
            }
        }

        private Task? _taskSearch;
        [JsonIgnore] // 직렬화 시 순환참조 방지
        public Task? TaskSearch
        {
            get { return _taskSearch; }
            set
            {
                _taskSearch = value;
                Notify();
            }
        }

        private Task? _taskBuy;
        [JsonIgnore] // 직렬화 시 순환참조 방지
        public Task? TaskBuy
        {
            get { return _taskBuy; }
            set
            {
                _taskBuy = value;
                Notify();
            }
        }

        // 특정한 매수법에 대한 종목 list 추출하기
        public IEnumerable<Stock> ObFilteredStockList
        {
            get
            {
                if (ClsParentSection == null) return Enumerable.Empty<Stock>();

                return ClsParentSection.ObcStockList.Where(s =>
                    s.StrSearchMethod == this.StrSearchMethod &&
                    s.StrBuyMethod == this.StrBuyMethod &&
                    s.StrManualMethod == this.StrManualMethod);
            }
        }

        public void RaiseFilteredListChanged()
        {
            Notify(nameof(ObFilteredStockList));
        }

        // 메세지 박스
        private string? _strMessage;
        public string? StrMessage
        {
            get { return _strMessage; }
            set
            {
                if (_strMessage != value)
                {
                    _strMessage = value;
                    Notify();
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
