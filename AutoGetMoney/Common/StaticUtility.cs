using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGetMoney.Common
{
    public static class StaticUtility
    {
        // log남기기
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        public static void WriteLog(string message)
        {
            try
            {
                // 로그 포맷: 시간 + 메시지
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                string logEntry = $"&[{timestamp}] {message}";
                Debug.WriteLine(logEntry);

                // 폴더 없으면 생성
                if (!Directory.Exists(LogDirectory))
                    Directory.CreateDirectory(LogDirectory);

                // 날짜별 파일명 (예: 2025-03-30.txt)
                string logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                string logFilePath = Path.Combine(LogDirectory, logFileName);

                // 파일에 한 줄 추가
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // 로그 실패 시 콘솔 출력 (원하는 방식으로 변경 가능)
                Console.WriteLine("로그 쓰기 실패: " + ex.Message);
            }
        }

        // 퍼센트 계싼 함수
        public static string  GetDropPercent(decimal? from, decimal to)
        {
            if (from == 0) return "-0%";
            decimal? percent = ((from - to) / from) * 100;
            return $"-{percent:0.##}%";
        }

        // RSI계산
        #region
        public static decimal? CalculateRsi(List<decimal> prices, int period = 14)
        {
            if (prices == null || prices.Count < period + 1)
                return null;

            decimal gains = 0;
            decimal losses = 0;

            // 초기 평균 Gain/Loss 계산
            for (int i = 1; i <= period; i++)
            {
                decimal diff = prices[i] - prices[i - 1];
                if (diff > 0)
                    gains += diff;
                else
                    losses += Math.Abs(diff);
            }

            decimal avgGain = gains / period;
            decimal avgLoss = losses / period;

            // 지수이동평균 방식으로 RSI 계산
            for (int i = period + 1; i < prices.Count; i++)
            {
                decimal diff = prices[i] - prices[i - 1];

                decimal gain = diff > 0 ? diff : 0;
                decimal loss = diff < 0 ? Math.Abs(diff) : 0;

                avgGain = (avgGain * (period - 1) + gain) / period;
                avgLoss = (avgLoss * (period - 1) + loss) / period;
            }

            if (avgLoss == 0)
                return 100;

            decimal rs = avgGain / avgLoss;
            decimal rsi = 100 - (100 / (1 + rs));

            return rsi;
        }

        #endregion

        public static double? FindTargetPriceForRsi(List<decimal> prices, decimal targetRsi, int period = 14)
        {
            if (prices == null || prices.Count < period + 1)
                return null;

            // 기존 가격 리스트 복사
            var priceList = new List<decimal>(prices);

            // 현재 마지막 가격
            decimal lastPrice = prices.Last();

            // 가격을 점진적으로 줄여가며 목표 RSI 이하가 될 때까지 탐색
            decimal step = lastPrice * 0.005m; // 0.5% 단위로 조절
            decimal testPrice = lastPrice;

            for (int i = 0; i < 5000; i++) // 최대 5000번 시도 (필요시 늘릴 수 있음)
            {
                priceList[priceList.Count - 1] = testPrice;

                decimal? rsi = StaticUtility.CalculateRsi(priceList, period);
                if (rsi.HasValue && rsi.Value <= targetRsi)
                {
                    return (double?)Math.Round(testPrice, 3);  // 소수 3자리까지 리턴
                }

                testPrice -= step;
            }

            return null; // 조건을 만족하는 가격을 찾지 못함
        }
    }
}
