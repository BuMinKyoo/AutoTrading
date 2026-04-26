using AutoGetMoney.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace AutoGetMoney.Converter
{
    public class MultiFibonacciColorConverterSecond : IMultiValueConverter
    {
        // ViewModel -> View
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            string? StockSymb = "";
            // 값을 안전하게 파싱
            try
            {
                StockSymb = values[0].ToString();
                string? raw = values[1]?.ToString();
                decimal stockLast = decimal.Parse(!string.IsNullOrWhiteSpace(raw) ? raw : "0");
                decimal? red = (decimal?)values[2];
                decimal? orange = (decimal?)values[3];
                decimal? green = (decimal?)values[4];
                decimal? blue = (decimal?)values[5];
                decimal? purple = (decimal?)values[6];

                // 비교 로직 (작을수록 아래로 내려감)
                if (stockLast != 0 && red != null && orange != null && green != null && blue != null && purple != null)
                {

                    if (stockLast > red)
                        return Brushes.Red;
                    else if (stockLast > orange)
                        return Brushes.Orange;
                    else if (stockLast > green)
                        return Brushes.Green;
                    else if (stockLast > blue)
                        return Brushes.Blue;
                    else if (stockLast > purple)
                        return Brushes.Purple;

                    return Brushes.Transparent; // 또는 예외 상황에서 보여줄 기본 색상
                }
                else
                {
                    return Brushes.Transparent; // 또는 예외 상황에서 보여줄 기본 색상
                }

            }
            catch (Exception ex)
            {
                string StrLog = $" FibonacciColorConverter 오류, 종목: {StockSymb}, 예외: {ex.Message}, 스택: {ex.StackTrace}";
                StaticUtility.WriteLog(StrLog);
                return Brushes.Transparent; // 또는 예외 상황에서 보여줄 기본 색상
            }

        }

        // View -> ViewModel
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
