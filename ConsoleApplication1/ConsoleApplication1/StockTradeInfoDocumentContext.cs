using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using Leopard.Data;
using RestSharp;
using Stock.Core.Domain.Models;

namespace ConsoleApplication1
{

    public class StockTradeInfoDocumentContext : JsonDocumentContext<Stock.Core.Domain.StockTradeInfo>
    {
        protected List<Stock.Core.Domain.StockTradeDetail> _stockTradeDetailBufferList = new List<Stock.Core.Domain.StockTradeDetail>();

        /// <summary>
        /// 超過明細總量數量檢查點，則會取得不重複的明細項目，
        /// </summary>
        public const int CheckPointCount = 200000;
        /// <summary>
        /// 若不重複的明細項目超過該儲存數量，則儲存明細為json檔，若尚未超過則增加回 buffer 清單中。
        /// </summary>
        public const int MaxSaveCount = 150000;

        public override string GetSource()
        {
            var req = new RestRequest("http://mis.twse.com.tw/stock/api/getStockInfo.jsp");
            req.AddParameter("delay", "0");
            var ex_ch = GetExChParameter();
            req.AddParameter("ex_ch", ex_ch);
            var res = Client.Post(req);
            return res.Content;
        }

        private string GetExChParameter()
        {
            return "tse_1101.tw|tse_1102.tw|tse_1103.tw|tse_1104.tw|tse_1108.tw|tse_1109.tw|tse_1110.tw|tse_1201.tw|tse_1203.tw|tse_1210.tw|tse_1213.tw|tse_1215.tw|tse_1216.tw|tse_1217.tw|tse_1218.tw|tse_1219.tw|tse_1220.tw|tse_1225.tw|tse_1227.tw|tse_1229.tw|tse_1231.tw|tse_1232.tw|tse_1233.tw|tse_1234.tw|tse_1235.tw|tse_1236.tw|tse_1262.tw|tse_1301.tw|tse_1303.tw|tse_1304.tw|tse_1305.tw|tse_1307.tw|tse_1308.tw|tse_1309.tw|tse_1310.tw|tse_1312.tw|tse_1313.tw|tse_1314.tw|tse_1315.tw|tse_1316.tw|tse_1319.tw|tse_1321.tw|tse_1323.tw|tse_1324.tw|tse_1325.tw|tse_1326.tw|tse_1337.tw|tse_1338.tw|tse_1339.tw|tse_1340.tw|tse_1402.tw|tse_1409.tw|tse_1410.tw|tse_1413.tw|tse_1414.tw|tse_1416.tw|tse_1417.tw|tse_1418.tw|tse_1419.tw|tse_1423.tw|tse_1432.tw|tse_1434.tw|tse_1435.tw|tse_1436.tw|tse_1437.tw|tse_1438.tw|tse_1439.tw|tse_1440.tw|tse_1441.tw|tse_1442.tw|tse_1443.tw|tse_1444.tw|tse_1445.tw|tse_1446.tw|tse_1447.tw|tse_1449.tw|tse_1451.tw|tse_1452.tw|tse_1453.tw|tse_1454.tw|tse_1455.tw|tse_1456.tw|tse_1457.tw|tse_1459.tw|tse_1460.tw|tse_1463.tw|tse_1464.tw|tse_1465.tw|tse_1466.tw|tse_1467.tw|tse_1468.tw|tse_1469.tw|tse_1470.tw|tse_1471.tw|tse_1472.tw|tse_1473.tw|tse_1474.tw|tse_1475.tw|tse_1476.tw|tse_1477.tw|tse_1503.tw|tse_1504.tw|tse_1506.tw|tse_1507.tw|tse_1512.tw|tse_1513.tw|tse_1514.tw|tse_1515.tw|tse_1516.tw|tse_1517.tw|tse_1519.tw|tse_1521.tw|tse_1522.tw|tse_1524.tw|tse_1525.tw|tse_1526.tw|tse_1527.tw|tse_1528.tw|tse_1529.tw|tse_1530.tw|tse_1531.tw|tse_1532.tw|tse_1533.tw|tse_1535.tw|tse_1536.tw|tse_1537.tw|tse_1538.tw|tse_1539.tw|tse_1540.tw|tse_1541.tw|tse_1560.tw|tse_1568.tw|tse_1582.tw|tse_1583.tw|tse_1589.tw|tse_1590.tw|tse_1603.tw|tse_1604.tw|tse_1605.tw|tse_1608.tw|tse_1609.tw|tse_1611.tw|tse_1612.tw|tse_1613.tw|tse_1614.tw|tse_1615.tw|tse_1616.tw|tse_1617.tw|tse_1618.tw|tse_1626.tw|tse_1701.tw|tse_1702.tw|tse_1704.tw|tse_1707.tw|tse_1708.tw|tse_1709.tw|tse_1710.tw|tse_1711.tw|tse_1712.tw|tse_1713.tw|tse_1714.tw|tse_1715.tw|tse_1717.tw|tse_1718.tw|tse_1720.tw|tse_1721.tw|tse_1722.tw|tse_1723.tw|tse_1724.tw|tse_1725.tw|tse_1726.tw|tse_1727.tw|tse_1729.tw|tse_1730.tw|tse_1731.tw|tse_1732.tw|tse_1733.tw|tse_1734.tw|tse_1735.tw|tse_1736.tw|tse_1737.tw|tse_1762.tw|tse_1773.tw|tse_1783.tw|tse_1786.tw|tse_1789.tw|tse_1802.tw|tse_1805.tw|tse_1806.tw|tse_1808.tw|tse_1809.tw|tse_1810.tw|tse_1817.tw|tse_1902.tw|tse_1903.tw|tse_1904.tw|tse_1905.tw|tse_1906.tw|tse_1907.tw|tse_1909.tw|tse_2002.tw|tse_2006.tw|tse_2007.tw|tse_2008.tw|tse_2009.tw|tse_2010.tw|tse_2012.tw|tse_2013.tw|tse_2014.tw|tse_2015.tw|tse_2017.tw|tse_2020.tw|tse_2022.tw|tse_2023.tw|tse_2024.tw|tse_2025.tw|tse_2027.tw|tse_2028.tw|tse_2029.tw|tse_2030.tw|tse_2031.tw|tse_2032.tw|tse_2033.tw|tse_2034.tw|tse_2038.tw|tse_2049.tw|tse_2059.tw|tse_2062.tw|tse_2101.tw|tse_2102.tw|tse_2103.tw|tse_2104.tw|tse_2105.tw|tse_2106.tw|tse_2107.tw|tse_2108.tw|tse_2109.tw|tse_2114.tw|tse_2115.tw|tse_2201.tw|tse_2204.tw|tse_2206.tw|tse_2207.tw|tse_2208.tw|tse_2227.tw|tse_2228.tw|tse_2231.tw|tse_2301.tw|tse_2302.tw|tse_2303.tw|tse_2305.tw|tse_2308.tw|tse_2311.tw|tse_2312.tw|tse_2313.tw|tse_2314.tw|tse_2316.tw|tse_2317.tw|tse_2321.tw|tse_2323.tw|tse_2324.tw|tse_2325.tw|tse_2327.tw|tse_2328.tw|tse_2329.tw|tse_2330.tw|tse_2331.tw|tse_2332.tw|tse_2337.tw|tse_2338.tw|tse_2340.tw|tse_2342.tw|tse_2344.tw|tse_2345.tw|tse_2347.tw|tse_2348.tw|tse_2349.tw|tse_2351.tw|tse_2352.tw|tse_2353.tw|tse_2354.tw|tse_2355.tw|tse_2356.tw|tse_2357.tw|tse_2358.tw|tse_2359.tw|tse_2360.tw|tse_2361.tw|tse_2362.tw|tse_2363.tw|tse_2364.tw|tse_2365.tw|tse_2367.tw|tse_2368.tw|tse_2369.tw|tse_2371.tw|tse_2373.tw|tse_2374.tw|tse_2375.tw|tse_2376.tw|tse_2377.tw|tse_2379.tw|tse_2380.tw|tse_2382.tw|tse_2383.tw|tse_2384.tw|tse_2385.tw|tse_2387.tw|tse_2388.tw|tse_2390.tw|tse_2392.tw|tse_2393.tw|tse_2395.tw|tse_2397.tw|tse_2399.tw|tse_2401.tw|tse_2402.tw|tse_2404.tw|tse_2405.tw|tse_2406.tw|tse_2408.tw|tse_2409.tw|tse_2412.tw|tse_2413.tw|tse_2414.tw|tse_2415.tw|tse_2417.tw|tse_2419.tw|tse_2420.tw|tse_2421.tw|tse_2423.tw|tse_2424.tw|tse_2425.tw|tse_2426.tw|tse_2427.tw|tse_2428.tw|tse_2429.tw|tse_2430.tw|tse_2431.tw|tse_2433.tw|tse_2434.tw|tse_2436.tw|tse_2437.tw|tse_2438.tw|tse_2439.tw|tse_2440.tw|tse_2441.tw|tse_2442.tw|tse_2443.tw|tse_2444.tw|tse_2448.tw|tse_2449.tw|tse_2450.tw|tse_2451.tw|tse_2453.tw|tse_2454.tw|tse_2455.tw|tse_2456.tw|tse_2457.tw|tse_2458.tw|tse_2459.tw|tse_2460.tw|tse_2461.tw|tse_2462.tw|tse_2464.tw|tse_2465.tw|tse_2466.tw|tse_2467.tw|tse_2468.tw|tse_2471.tw|tse_2472.tw|tse_2474.tw|tse_2475.tw|tse_2476.tw|tse_2477.tw|tse_2478.tw|tse_2480.tw|tse_2481.tw|tse_2482.tw|tse_2483.tw|tse_2484.tw|tse_2485.tw|tse_2486.tw|tse_2488.tw|tse_2489.tw|tse_2491.tw|tse_2492.tw|tse_2493.tw|tse_2495.tw|tse_2496.tw|tse_2497.tw|tse_2498.tw|tse_2499.tw|tse_2501.tw|tse_2504.tw|tse_2505.tw|tse_2506.tw|tse_2509.tw|tse_2511.tw|tse_2514.tw|tse_2515.tw|tse_2516.tw|tse_2520.tw|tse_2524.tw|tse_2527.tw|tse_2528.tw|tse_2530.tw|tse_2534.tw|tse_2535.tw|tse_2536.tw|tse_2537.tw|tse_2538.tw|tse_2539.tw|tse_2540.tw|tse_2542.tw|tse_2543.tw|tse_2545.tw|tse_2546.tw|tse_2547.tw|tse_2548.tw|tse_2597.tw|tse_2601.tw|tse_2603.tw|tse_2605.tw|tse_2606.tw|tse_2607.tw|tse_2608.tw|tse_2609.tw|tse_2610.tw|tse_2611.tw|tse_2612.tw|tse_2613.tw|tse_2614.tw|tse_2615.tw|tse_2616.tw|tse_2617.tw|tse_2618.tw|tse_2634.tw|tse_2637.tw|tse_2642.tw|tse_2701.tw|tse_2702.tw|tse_2704.tw|tse_2705.tw|tse_2706.tw|tse_2707.tw|tse_2712.tw|tse_2722.tw|tse_2723.tw|tse_2727.tw|tse_2731.tw|tse_2801.tw|tse_2809.tw|tse_2812.tw|tse_2816.tw|tse_2820.tw|tse_2823.tw|tse_2832.tw|tse_2833.tw|tse_2834.tw|tse_2836.tw|tse_2837.tw|tse_2838.tw|tse_2841.tw|tse_2845.tw|tse_2847.tw|tse_2849.tw|tse_2850.tw|tse_2851.tw|tse_2852.tw|tse_2855.tw|tse_2856.tw|tse_2867.tw|tse_2880.tw|tse_2881.tw|tse_2882.tw|tse_2883.tw|tse_2884.tw|tse_2885.tw|tse_2886.tw|tse_2887.tw|tse_2888.tw|tse_2889.tw|tse_2890.tw|tse_2891.tw|tse_2892.tw|tse_2901.tw|tse_2903.tw|tse_2904.tw|tse_2905.tw|tse_2906.tw|tse_2908.tw|tse_2910.tw|tse_2911.tw|tse_2912.tw|tse_2913.tw|tse_2915.tw|tse_2923.tw|tse_2929.tw|tse_3002.tw|tse_3003.tw|tse_3004.tw|tse_3005.tw|tse_3006.tw|tse_3008.tw|tse_3010.tw|tse_3011.tw|tse_3013.tw|tse_3014.tw|tse_3015.tw|tse_3016.tw|tse_3017.tw|tse_3018.tw|tse_3019.tw|tse_3021.tw|tse_3022.tw|tse_3023.tw|tse_3024.tw|tse_3025.tw|tse_3026.tw|tse_3027.tw|tse_3028.tw|tse_3029.tw|tse_3030.tw|tse_3031.tw|tse_3032.tw|tse_3033.tw|tse_3034.tw|tse_3035.tw|tse_3036.tw|tse_3037.tw|tse_3038.tw|tse_3040.tw|tse_3041.tw|tse_3042.tw|tse_3043.tw|tse_3044.tw|tse_3045.tw|tse_3046.tw|tse_3047.tw|tse_3048.tw|tse_3049.tw|tse_3050.tw|tse_3051.tw|tse_3052.tw|tse_3054.tw|tse_3055.tw|tse_3056.tw|tse_3057.tw|tse_3058.tw|tse_3059.tw|tse_3060.tw|tse_3061.tw|tse_3062.tw|tse_3090.tw|tse_3094.tw|tse_3130.tw|tse_3149.tw|tse_3164.tw|tse_3167.tw|tse_3189.tw|tse_3209.tw|tse_3229.tw|tse_3231.tw|tse_3257.tw|tse_3296.tw|tse_3305.tw|tse_3308.tw|tse_3311.tw|tse_3312.tw|tse_3315.tw|tse_3338.tw|tse_3356.tw|tse_3376.tw|tse_3380.tw|tse_3383.tw|tse_3406.tw|tse_3419.tw|tse_3432.tw|tse_3437.tw|tse_3443.tw|tse_3450.tw|tse_3454.tw|tse_3474.tw|tse_3481.tw|tse_3494.tw|tse_3501.tw|tse_3504.tw|tse_3514.tw|tse_3515.tw|tse_3518.tw|tse_3519.tw|tse_3532.tw|tse_3533.tw|tse_3535.tw|tse_3536.tw|tse_3545.tw|tse_3550.tw|tse_3557.tw|tse_3559.tw|tse_3561.tw|tse_3573.tw|tse_3576.tw|tse_3579.tw|tse_3583.tw|tse_3584.tw|tse_3588.tw|tse_3591.tw|tse_3593.tw|tse_3596.tw|tse_3598.tw|tse_3605.tw|tse_3607.tw|tse_3617.tw|tse_3622.tw|tse_3638.tw|tse_3645.tw|tse_3653.tw|tse_3665.tw|tse_3669.tw|tse_3673.tw|tse_3679.tw|tse_3682.tw|tse_3686.tw|tse_3694.tw|tse_3698.tw|tse_3701.tw|tse_3702.tw|tse_3703.tw|tse_3704.tw|tse_3705.tw|tse_3706.tw|tse_4104.tw|tse_4106.tw|tse_4108.tw|tse_4119.tw|tse_4133.tw|tse_4137.tw|tse_4141.tw|tse_4142.tw|tse_4144.tw|tse_4164.tw|tse_4306.tw|tse_4414.tw|tse_4426.tw|tse_4526.tw|tse_4532.tw|tse_4536.tw|tse_4722.tw|tse_4725.tw|tse_4733.tw|tse_4737.tw|tse_4746.tw|tse_4755.tw|tse_4904.tw|tse_4906.tw|tse_4915.tw|tse_4916.tw|tse_4919.tw|tse_4930.tw|tse_4934.tw|tse_4935.tw|tse_4938.tw|tse_4942.tw|tse_4952.tw|tse_4956.tw|tse_4958.tw|tse_4960.tw|tse_4976.tw|tse_4977.tw|tse_4984.tw|tse_4994.tw|tse_4999.tw|tse_5007.tw|tse_5203.tw|tse_5215.tw|tse_5225.tw|tse_5234.tw|tse_5243.tw|tse_5259.tw|tse_5264.tw|tse_5269.tw|tse_5280.tw|tse_5285.tw|tse_5305.tw|tse_5388.tw|tse_5434.tw|tse_5469.tw|tse_5471.tw|tse_5484.tw|tse_5515.tw|tse_5519.tw|tse_5521.tw|tse_5522.tw|tse_5525.tw|tse_5531.tw|tse_5533.tw|tse_5534.tw|tse_5538.tw|tse_5607.tw|tse_5608.tw|tse_5706.tw|tse_5871.tw|tse_5880.tw|tse_5906.tw|tse_5907.tw|tse_6005.tw|tse_6108.tw|tse_6112.tw|tse_6115.tw|tse_6116.tw|tse_6117.tw|tse_6120.tw|tse_6128.tw|tse_6131.tw|tse_6133.tw|tse_6136.tw|tse_6139.tw|tse_6141.tw|tse_6142.tw|tse_6145.tw|tse_6152.tw|tse_6153.tw|tse_6155.tw|tse_6164.tw|tse_6165.tw|tse_6166.tw|tse_6168.tw|tse_6172.tw|tse_6176.tw|tse_6177.tw|tse_6183.tw|tse_6184.tw|tse_6189.tw|tse_6191.tw|tse_6192.tw|tse_6196.tw|tse_6197.tw|tse_6201.tw|tse_6202.tw|tse_6205.tw|tse_6206.tw|tse_6209.tw|tse_6213.tw|tse_6214.tw|tse_6215.tw|tse_6216.tw|tse_6224.tw|tse_6225.tw|tse_6226.tw|tse_6230.tw|tse_6235.tw|tse_6239.tw|tse_6243.tw|tse_6251.tw|tse_6257.tw|tse_6269.tw|tse_6271.tw|tse_6277.tw|tse_6278.tw|tse_6281.tw|tse_6282.tw|tse_6283.tw|tse_6285.tw|tse_6286.tw|tse_6289.tw|tse_6405.tw|tse_6409.tw|tse_6412.tw|tse_6414.tw|tse_6415.tw|tse_6504.tw|tse_6505.tw|tse_6605.tw|tse_6702.tw|tse_8011.tw|tse_8016.tw|tse_8021.tw|tse_8033.tw|tse_8039.tw|tse_8046.tw|tse_8070.tw|tse_8072.tw|tse_8081.tw|tse_8101.tw|tse_8103.tw|tse_8105.tw|tse_8110.tw|tse_8112.tw|tse_8114.tw|tse_8131.tw|tse_8150.tw|tse_8163.tw|tse_8201.tw|tse_8210.tw|tse_8213.tw|tse_8215.tw|tse_8249.tw|tse_8261.tw|tse_8271.tw|tse_8374.tw|tse_8404.tw|tse_8411.tw|tse_8422.tw|tse_8427.tw|tse_8429.tw|tse_8926.tw|tse_8940.tw|tse_8996.tw|tse_9802.tw|tse_9902.tw|tse_9904.tw|tse_9905.tw|tse_9906.tw|tse_9907.tw|tse_9908.tw|tse_9910.tw|tse_9911.tw|tse_9912.tw|tse_9914.tw|tse_9917.tw|tse_9918.tw|tse_9919.tw|tse_9921.tw|tse_9924.tw|tse_9925.tw|tse_9926.tw|tse_9927.tw|tse_9928.tw|tse_9929.tw|tse_9930.tw|tse_9931.tw|tse_9933.tw|tse_9934.tw|tse_9935.tw|tse_9937.tw|tse_9938.tw|tse_9939.tw|tse_9940.tw|tse_9941.tw|tse_9942.tw|tse_9943.tw|tse_9944.tw|tse_9945.tw|tse_9946.tw|tse_9955.tw|tse_9958.tw|";
        }

        /// <summary>
        /// 更新股票明細資訊
        /// </summary>
        public void UpdateStockInfo()
        {
            if (!LoadDocument())
            {
                // TODO: 請求到反序列化過程中有錯誤處理
                Console.WriteLine("{0} Load Document Failed.", DateTime.Now);
                return;
            }

            if (Document.msgArray == null)
            {
                // TODO: 股票資料錯誤訊息處理
                Console.WriteLine(Document.rtmessage);
                return;
            }

            this._stockTradeDetailBufferList.AddRange(this.Document.msgArray);
        }

        /// <summary>
        /// 檢查明細buffer是否已達到可儲存的數量，並儲存為 json 檔案
        /// </summary>
        /// <returns></returns>
        public int SaveCheck()
        {
            if (IsTradeTime(DateTime.Now))
            {
                if (this._stockTradeDetailBufferList.Count >= CheckPointCount)
                {
                    return SaveDistinctItems(DateTime.Now.AddMinutes(-1));
                }
            }
            else if (this._stockTradeDetailBufferList.Any())
            {
                return  SaveDistinctItems(DateTime.Now.AddHours(1));
            }

            return 0;
        }

        private int SaveDistinctItems(DateTime beforeOneMinutes)
        {
            var distinctItems = GetDistinctItems(beforeOneMinutes);

            // 移除已經以儲存的項目，並且進行記憶體回收
            this._stockTradeDetailBufferList.RemoveAll(item => beforeOneMinutes >= item.TradeTime);

            // 當超過指定儲存數量或不在交易時間內時，將明細儲存為 json.
            if (distinctItems.Count >= MaxSaveCount || !IsTradeTime(DateTime.Now))
            {
                Save(distinctItems);    
            }
            else
            {
                this._stockTradeDetailBufferList.AddRange(distinctItems);
            }

            return distinctItems.Count;
        }

        private List<Stock.Core.Domain.StockTradeDetail> GetDistinctItems(DateTime beforeMinutes)
        {
            // 取得前幾分鐘所有按照時間排序且不重複的交易紀錄
            var query = from item in this._stockTradeDetailBufferList
                        where beforeMinutes >= item.TradeTime
                        orderby item.TradeTime
                        select item;

            var list = query.Distinct(new StockIdAndTradeTimeEqualityComparer()).ToList();
            return list;
        }

        private void Save(List<Stock.Core.Domain.StockTradeDetail> distinctItems)
        {
            if (distinctItems.Any())
            {
                var dict = distinctItems.GroupBy(item => item.StockID).ToDictionary(g => g.Key, g => g.ToList());
                var content = JsonConvert.SerializeObject(dict);
                File.WriteAllText(string.Format("{0}.json", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")), content, Encoding.UTF8);
            }
        }

        public class StockIdAndTradeTimeEqualityComparer : IEqualityComparer<Stock.Core.Domain.StockTradeDetail>
        {
            bool IEqualityComparer<Stock.Core.Domain.StockTradeDetail>.Equals(Stock.Core.Domain.StockTradeDetail x, Stock.Core.Domain.StockTradeDetail y)
            {
                return x.StockID == y.StockID && x.TradeTime == y.TradeTime;
            }

            int IEqualityComparer<Stock.Core.Domain.StockTradeDetail>.GetHashCode(Stock.Core.Domain.StockTradeDetail obj)
            {
                return obj.StockID.GetHashCode() ^ obj.TradeTime.GetHashCode();
            }
        }

        /// <summary>
        /// 只在星期一到五且早上8:59到下午2:31交易
        /// </summary>
        public static bool IsTradeTime(DateTime datetime)
        {
            var timeOfDay = datetime.TimeOfDay;
            var dayOfWeek = datetime.DayOfWeek;
            return IsTradeTime(timeOfDay) && DayOfWeek.Sunday < dayOfWeek && dayOfWeek < DayOfWeek.Saturday;
        }

        private static bool IsTradeTime(TimeSpan timeOfDay)
        {
            return new TimeSpan(8, 59, 0) <= timeOfDay && timeOfDay <= new TimeSpan(13, 36, 0);
        }
    }
}
