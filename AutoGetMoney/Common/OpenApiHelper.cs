using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoGetMoney.Common
{
    public static class OpenApiHelper
    {
        // 웹접속키 생성 api
        #region
        public static async Task<string?> GenerateWebKeyAsync(string? communicationUrl, string appKey, string appSecret)
        {
            var result = await HttpHelper.SendAsync(null, $"{communicationUrl}/oauth2/Approval", new
            {
                grant_type = "client_credentials",
                appkey = appKey,
                secretkey = appSecret
            }, HttpMethodType.Post, ContentType.Json);

            return result;
        }
        #endregion

        // 토큰생성 api
        #region
        public static async Task<string?> GenerateTokenAsync(string? communicationUrl, string appKey, string appSecret)
        {
            var result = await HttpHelper.SendAsync(null, $"{communicationUrl}/oauth2/tokenP", new
            {
                grant_type = "client_credentials",
                appkey = appKey,
                appsecret = appSecret

            }, HttpMethodType.Post, ContentType.Json);

            return result;
        }
        #endregion

        // 분봉 조회 API
        #region
        public static async Task<string> GetMinuteCandlestickAsync(
       Model.Section? ClickClsSection,
       string? StrSectionCommunicationUrl,
       string? token,
       string? appKey,
       string? appSecret,
       string auth,
       string excd,
       string symb,
       string nmin,
       string pinc,
       string next,
       string nrec,
       string fill,
       string keyb
       )
        {
            string url = $"{StrSectionCommunicationUrl}/uapi/overseas-price/v1/quotations/inquire-time-itemchartprice" +
                         $"?AUTH={auth}" +
                         $"&EXCD={excd}" +
                         $"&SYMB={symb}" +
                         $"&NMIN={nmin}" +
                         $"&PINC={pinc}" +
                         $"&NEXT={next}" +
                         $"&NREC={nrec}" +
                         $"&FILL={fill}" +
                         $"&KEYB={keyb}";

            var headers = new Dictionary<string, string>
            {
                { "content-type", "application/json" },
                { "authorization", $"Bearer {token}" },
                { "appkey", appKey! },
                { "appsecret", appSecret! },
                { "tr_id", "HHDFS76950200" },
                { "custtype", "P" }
            };

            if (ClickClsSection == null)
            {
                return "부모 Section이 null입니다";
            }
            string result = await HttpHelper.SendAsync(ClickClsSection, url, null, HttpMethodType.Get, ContentType.Json, headers);

            return result;
        }
        #endregion

        // 종목 매수 api
        #region
        public static async Task<string> SendBuyOrderAsync(
            Model.Section ClickClsSection,
            string acnt_prdt_cd,
            string ovrs_excg_cd,
            string pdno,
            string ord_qty,
            string ovrs_ord_unpr,
            string sll_type,
            string ord_svr_dvsn_cd,
            string ord_dvsn
            )
        {
            if (ClickClsSection == null)
            {
                return "부모 Section이 null입니다";
            }

            string StrTr_id = "";
            if (ClickClsSection.StrCommunicationUrl == "https://openapivts.koreainvestment.com:29443") // 모의투자
            {
                StrTr_id = "VTTT1002U";
            }
            else if (ClickClsSection.StrCommunicationUrl == "https://openapi.koreainvestment.com:9443") // 실전투자
            {
                StrTr_id = "TTTT1002U";
            }

            var headers = new Dictionary<string, string>
            {
                { "content-type", "application/json" },
                { "authorization", $"Bearer {ClickClsSection.StrToken}" },
                { "appkey", ClickClsSection.StrAppKey! },
                { "appsecret", ClickClsSection.StrAppSecret! },
                { "tr_id", StrTr_id }


                //[실전투자]
                //TTTT1002U : 미국 매수 주문
                //TTTT1006U : 미국 매도 주문
                //TTTS0308U : 일본 매수 주문
                //TTTS0307U : 일본 매도 주문
                //TTTS0202U : 상해 매수 주문
                //TTTS1005U : 상해 매도 주문
                //TTTS1002U : 홍콩 매수 주문
                //TTTS1001U : 홍콩 매도 주문
                //TTTS0305U : 심천 매수 주문
                //TTTS0304U : 심천 매도 주문
                //TTTS0311U : 베트남 매수 주문
                //TTTS0310U : 베트남 매도 주문
                //[모의투자]
                //VTTT1002U: 미국 매수 주문
                //VTTT1001U : 미국 매도 주문
                //VTTS0308U : 일본 매수 주문
                //VTTS0307U : 일본 매도 주문
                //VTTS0202U : 상해 매수 주문
                //VTTS1005U : 상해 매도 주문
                //VTTS1002U : 홍콩 매수 주문
                //VTTS1001U : 홍콩 매도 주문
                //VTTS0305U : 심천 매수 주문
                //VTTS0304U : 심천 매도 주문
                //VTTS0311U : 베트남 매수 주문
                //VTTS0310U : 베트남 매도 주문
            };

            var result = await HttpHelper.SendAsync(ClickClsSection, $"{ClickClsSection.StrCommunicationUrl}/uapi/overseas-stock/v1/trading/order", new
            {
                CANO = ClickClsSection.StrAccountNumber,
                ACNT_PRDT_CD = acnt_prdt_cd,
                OVRS_EXCG_CD = ovrs_excg_cd,
                PDNO = pdno,
                ORD_QTY = ord_qty,
                OVRS_ORD_UNPR = ovrs_ord_unpr,
                ORD_SVR_DVSN_CD = ord_svr_dvsn_cd,
                ORD_DVSN = ord_dvsn

            }, HttpMethodType.Post, ContentType.Json, headers);

            return result;
        }
        #endregion

        // 종목 조회 API
        #region
        public static async Task<string> SearchStockForAllStrategyAsync(
        Model.Section? ClickClsSection,
        string? StrSectionCommunicationUrl,
        string? token,
        string? appKey,
        string? appSecret,
        string auth,
        string excd,
        string coYnPriceCur,
        string coStPriceCur,
        string coEnPriceCur,
        string coYnRate,
        string coStRate,
        string coEnRate,
        string coYnValx,
        string coStValx,
        string coEnValx,
        string coYnShar,
        string coStShar,
        string coEnShar,
        string coYnVolume,
        string coStVolume,
        string coEnVolume,
        string coYnAmt,
        string coStAmt,
        string coEnAmt,
        string coYnEps,
        string coStEps,
        string coEnEps,
        string coYnPer,
        string coStPer,
        string coEnPer
        )
        {
            string url = $"{StrSectionCommunicationUrl}/uapi/overseas-price/v1/quotations/inquire-search" +
                         $"?AUTH={auth}" +
                         $"&EXCD={excd}" +
                         $"&CO_YN_PRICECUR={coYnPriceCur}" +
                         $"&CO_ST_PRICECUR={coStPriceCur}" +
                         $"&CO_EN_PRICECUR={coEnPriceCur}" +
                         $"&CO_YN_RATE={coYnRate}" +
                         $"&CO_ST_RATE={coStRate}" +
                         $"&CO_EN_RATE={coEnRate}" +
                         $"&CO_YN_VALX={coYnValx}" +
                         $"&CO_ST_VALX={coStValx}" +
                         $"&CO_EN_VALX={coEnValx}" +
                         $"&CO_YN_SHAR={coYnShar}" +
                         $"&CO_ST_SHAR={coStShar}" +
                         $"&CO_EN_SHAR={coEnShar}" +
                         $"&CO_YN_VOLUME={coYnVolume}" +
                         $"&CO_ST_VOLUME={coStVolume}" +
                         $"&CO_EN_VOLUME={coEnVolume}" +
                         $"&CO_YN_AMT={coYnAmt}" +
                         $"&CO_ST_AMT={coStAmt}" +
                         $"&CO_EN_AMT={coEnAmt}" +
                         $"&CO_YN_EPS={coYnEps}" +
                         $"&CO_ST_EPS={coStEps}" +
                         $"&CO_EN_EPS={coEnEps}" +
                         $"&CO_YN_PER={coYnPer}" +
                         $"&CO_ST_PER={coStPer}" +
                         $"&CO_EN_PER={coEnPer}";

            var headers = new Dictionary<string, string>
            {
                { "content-type", "application/json" },
                { "authorization", $"Bearer {token}" },
                { "appkey", appKey! },
                { "appsecret", appSecret! },
                { "tr_id", "HHDFS76410000" }
            };

            if (ClickClsSection == null)
            {
                return "부모 Section이 null입니다";
            }
            string result = await HttpHelper.SendAsync(ClickClsSection, url, null, HttpMethodType.Get, ContentType.Json, headers);

            return result;
        }
        #endregion


        // 거래량 순위 API
        #region
        public static async Task<string> SearchStockByVolumeRankAsync(
            Model.Section ClickClsSection,
            string vol_rang,
            string prc2,
            string prc1,
            string nday,
            string excd
            )
        {
            string url = $"{ClickClsSection.StrCommunicationUrl}/uapi/overseas-price/v1/quotations/inquire-search" +
                         $"?KEYB=" +
                         $"&VOL_RANG={vol_rang}" +
                         $"&PRC2={prc2}" +
                         $"&PRC1={prc1}" +
                         $"&NDAY={nday}" +
                         $"&EXCD={excd}" +
                         $"&AUTH=";

            var headers = new Dictionary<string, string>
            {
                { "content-type", "application/json" },
                { "authorization", $"Bearer {ClickClsSection.StrToken}" },
                { "appkey", ClickClsSection.StrAppKey! },
                { "appsecret", ClickClsSection.StrAppSecret! },
                { "tr_id", "HHDFS76310010" }
            };

            if (ClickClsSection == null)
            {
                return "부모 Section이 null입니다";
            }
            string result = await HttpHelper.SendAsync(ClickClsSection, url, null, HttpMethodType.Get, ContentType.Json, headers);

            return result;
        }
        #endregion
    }
}
