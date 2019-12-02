using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Utilities;

namespace HertzNetFramework.DataModels
{
    public class TxnHeader
    {
        public static List<string> UsedRANUMs = new List<string>();
        public static readonly string TableName = "ats_txnheader";
        public const string dbUser = "bp_htz";

        #region Public Properties
        public decimal A_ROWKEY { get; set; }
        public decimal A_VCKEY { get; set; }
        public decimal? A_PARENTROWKEY { get; set; }
        [ModelAttribute("Key", check: EqualityCheck.Skip)]
        public string A_KEY { get; set; }
        [ModelAttribute("QUALTOTAMT")]
        public decimal? A_QUALTOTAMT { get; set; }
        [ModelAttribute("LYLTYMEMBERNUM")]
        public string A_LYLTYMEMBERNUM { get; set; }
        [ModelAttribute("CUSTID")]
        public long? A_CUSTID { get; set; }
        [ModelAttribute("REFSRCNUM")]
        public string A_REFSRCNUM { get; set; }
        [ModelAttribute("CHKINDT")]
        public DateTime? A_CHKINDT { get; set; }
        [ModelAttribute("RANUM", ReportOption.Print)]
        public string A_RANUM { get; set; }
        [ModelAttribute("RESVID")]
        public string A_RESVID { get; set; }
        [ModelAttribute("CHKINLOCNUM", ReportOption.Print)]
        public string A_CHKINLOCNUM { get; set; }
        [ModelAttribute("PROMNUM")]
        public string A_PROMNUM { get; set; }
        [ModelAttribute("VCHRNUM")]
        public string A_VCHRNUM { get; set; }
        [ModelAttribute("CHKINAREANUM", ReportOption.Print)]
        public string A_CHKINAREANUM { get; set; }
        [ModelAttribute("CHKOUTDT", ReportOption.Print, check: EqualityCheck.Skip)]
        public DateTime? A_CHKOUTDT { get; set; }
        [ModelAttribute("CHKOUTLOCNUM", ReportOption.Print)]
        public string A_CHKOUTLOCNUM { get; set; }
        [ModelAttribute("CHKOUTAREANUM", ReportOption.Print)]
        public string A_CHKOUTAREANUM { get; set; }
        [ModelAttribute("FTPTNRNUM", ReportOption.Print)]
        public string A_FTPTNRNUM { get; set; }
        [ModelAttribute("CHKOUTCITYCD")]
        public string A_CHKOUTCITYCD { get; set; }
        [ModelAttribute("CHKOUTWORLDWIDERGNCTRYISO")]
        public string A_CHKOUTWORLDWIDERGNCTRYISO { get; set; }
        [ModelAttribute("CHKOUTWWDSTPROVCD")]
        public string A_CHKOUTWWDSTPROVCD { get; set; }
        [ModelAttribute("ORIGBOOKDT", ReportOption.Print)]
        public DateTime? A_ORIGBOOKDT { get; set; }
        [ModelAttribute("CHRGVEHCLSCD")]
        public string A_CHRGVEHCLSCD { get; set; }
        [ModelAttribute("CRCARDTYPECD")]
        public string A_CRCARDTYPECD { get; set; }
        [ModelAttribute("RQSTSIPPCD")]
        public string A_RQSTSIPPCD { get; set; }
        [ModelAttribute("GEOLOCTYPECD")]
        public string A_GEOLOCTYPECD { get; set; }
        [ModelAttribute("RASRCCD", ReportOption.Print)]
        public string A_RASRCCD { get; set; }
        [ModelAttribute("INTRNLNETRTGCD")]
        public string A_INTRNLNETRTGCD { get; set; }
        [ModelAttribute("TRAVLPRPSTYPECD")]
        public string A_TRAVLPRPSTYPECD { get; set; }
        [ModelAttribute("MKTGRTANALCD")]
        public string A_MKTGRTANALCD { get; set; }
        [ModelAttribute("CRNCYISOCD")]
        public string A_CRNCYISOCD { get; set; }
        [ModelAttribute("RENTALTYPE")]
        public string A_RENTALTYPE { get; set; }
        [ModelAttribute("DAYSCHRGQTY")]
        public long? A_DAYSCHRGQTY { get; set; }
        [ModelAttribute("LDWCDWCHRGAMT")]
        public decimal? A_LDWCDWCHRGAMT { get; set; }
        [ModelAttribute("DISCAMT")]
        public decimal? A_DISCAMT { get; set; }
        [ModelAttribute("NWEXECSAMT")]
        public decimal? A_NWEXECSAMT { get; set; }
        [ModelAttribute("PAITOTCHRGAMT")]
        public decimal? A_PAITOTCHRGAMT { get; set; }
        [ModelAttribute("ADDLAUTHDRVRCHRGAMT")]
        public decimal? A_ADDLAUTHDRVRCHRGAMT { get; set; }
        [ModelAttribute("AGEDIFFCHRGAMT")]
        public decimal? A_AGEDIFFCHRGAMT { get; set; }
        [ModelAttribute("ADDLSRVCCHRGAMT")]
        public decimal? A_ADDLSRVCCHRGAMT { get; set; }
        [ModelAttribute("SBTOTAMT")]
        public decimal? A_SBTOTAMT { get; set; }
        [ModelAttribute("TOTCHRGAMT")]
        public decimal? A_TOTCHRGAMT { get; set; }
        [ModelAttribute("LISTOTCHRGAMT")]
        public decimal? A_LISTOTCHRGAMT { get; set; }
        [ModelAttribute("CHILDSEATTOTAMT")]
        public decimal? A_CHILDSEATTOTAMT { get; set; }
        [ModelAttribute("ITVALLFEETOTAMT")]
        public decimal? A_ITVALLFEETOTAMT { get; set; }
        [ModelAttribute("GARSPECLEQMNTAMT")]
        public decimal? A_GARSPECLEQMNTAMT { get; set; }
        [ModelAttribute("GRSREVNAMT")]
        public decimal? A_GRSREVNAMT { get; set; }
        [ModelAttribute("MISCGRPAMT")]
        public decimal? A_MISCGRPAMT { get; set; }
        [ModelAttribute("NVGTNSYSTOTAMT")]
        public decimal? A_NVGTNSYSTOTAMT { get; set; }
        [ModelAttribute("SATLTRADIOTOTAMT")]
        public decimal? A_SATLTRADIOTOTAMT { get; set; }
        [ModelAttribute("REFUELINGOPTCD")]
        public string A_REFUELINGOPTCD { get; set; }
        [ModelAttribute("REFUELINGCHRGAMT")]
        public decimal? A_REFUELINGCHRGAMT { get; set; }
        [ModelAttribute("TPTOTCHRGAMT")]
        public decimal? A_TPTOTCHRGAMT { get; set; }
        [ModelAttribute("TRANSACTIONSTATE")]
        public string A_TRANSACTIONSTATE { get; set; }
        [ModelAttribute("RESCHANNEL")]
        public string A_RESCHANNEL { get; set; }
        [ModelAttribute("TOTEXPIRINGEUPGRADES")]
        public long? A_TOTEXPIRINGEUPGRADES { get; set; }
        [ModelAttribute("TOTEUPGRADES")]
        public long? A_TOTEUPGRADES { get; set; }
        [ModelAttribute("RARTTYPECD")]
        public string A_RARTTYPECD { get; set; }
        [ModelAttribute("RESVRTTYPECD")]
        public string A_RESVRTTYPECD { get; set; }
        [ModelAttribute("RTCATCD")]
        public string A_RTCATCD { get; set; }
        [ModelAttribute("RTCLSNCD")]
        public string A_RTCLSNCD { get; set; }
        [ModelAttribute("RTFMLYCD")]
        public string A_RTFMLYCD { get; set; }
        [ModelAttribute("SACCD")]
        public string A_SACCD { get; set; }
        [ModelAttribute("RSDNCTRYCD")]
        public string A_RSDNCTRYCD { get; set; }
        [ModelAttribute("GOLDRNTALIND")]
        public string A_GOLDRNTALIND { get; set; }
        [ModelAttribute("TxnDate", check: EqualityCheck.Skip)]
        public DateTime A_TXNDATE { get; set; }
        [ModelAttribute("TxnHeaderId", check:EqualityCheck.Skip)]
        public string A_TXNHEADERID { get; set; }
        [ModelAttribute("CHKOUTTM")]
        public string A_CHKOUTTM { get; set; }
        [ModelAttribute("TransType")]
        public string A_TRANSTYPE { get; set; }
        [ModelAttribute("RNTINGCTRYCRNCYUSDEXCHRT")]
        public decimal? A_RNTINGCTRYCRNCYUSDEXCHRT { get; set; }
        [ModelAttribute("CORPDISCPRGID", ReportOption.Print)]
        public decimal? A_CORPDISCPRGID { get; set; }
        [ModelAttribute("CONTRACTTYPECD")]
        public string A_CONTRACTTYPECD { get; set; }
        [ModelAttribute("CONTRACTNUM")]
        public decimal? A_CONTRACTNUM { get; set; }
        [ModelAttribute("BrandId")]
        public string A_BRANDID { get; set; }
        [ModelAttribute("CreditCardId")]
        public long? A_CREDITCARDID { get; set; }
        [ModelAttribute("TxnMaskId")]
        public string A_TXNMASKID { get; set; }
        [ModelAttribute("TxnNumber", ReportOption.Print)]
        public string A_TXNNUMBER { get; set; }
        [ModelAttribute("TxnRegisterNumber", check: EqualityCheck.Skip)]
        public string A_TXNREGISTERNUMBER { get; set; }
        [ModelAttribute("TxnStoreId", check: EqualityCheck.Skip)]
        public long? A_TXNSTOREID { get; set; }
        [ModelAttribute("TxnTypeId", check: EqualityCheck.Skip)]
        public long? A_TXNTYPEID { get; set; }
        [ModelAttribute("TxnAmount", check: EqualityCheck.Skip)]
        public decimal? A_TXNAMOUNT { get; set; }
        [ModelAttribute("TxnQualPurchaseAmt", ReportOption.Print)]
        public decimal? A_TXNQUALPURCHASEAMT { get; set; }
        [ModelAttribute("TxnDiscountAmount")]
        public decimal? A_TXNDISCOUNTAMOUNT { get; set; }
        [ModelAttribute("TxnEmployeeId", check: EqualityCheck.Skip)]
        public string A_TXNEMPLOYEEID { get; set; }
        [ModelAttribute("TxnChannel", check: EqualityCheck.Skip)]
        public string A_TXNCHANNEL { get; set; }
        [ModelAttribute("TxnOriginalTxnRowKey")]
        public decimal? A_TXNORIGINALTXNROWKEY { get; set; }
        [ModelAttribute("TxnCreditsUsed", check: EqualityCheck.Skip)]
        public long? A_TXNCREDITSUSED { get; set; }
        public decimal? STATUSCODE { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public decimal? LASTDMLID { get; set; }
        public decimal? LAST_DML_ID { get; set; }
        [ModelAttribute("HODIndicator", ReportOption.Print)]
        public short? A_HODINDICATOR { get; set; }
        [ModelAttribute("CHKINROWKEY", check:EqualityCheck.Skip)]
        public decimal? A_CHKINROWKEY { get; set; }
        [ModelAttribute("CHKOUTROWKEY", check: EqualityCheck.Skip)]
        public decimal? A_CHKOUTROWKEY { get; set; }
        [ModelAttribute("CHKINLOCATIONID")]
        public string A_CHKINLOCATIONID { get; set; }
        [ModelAttribute("CHKOUTLOCATIONID")]
        public string A_CHKOUTLOCATIONID { get; set; }
        #endregion

        public static TxnHeader Generate(string loyaltyId,  
                                            DateTime? checkInDate = null, DateTime? checkOutDate = null, DateTime? bookDate = null, 
                                            decimal? CDP = null, IHertzProgram program = null, short? HODIndicator = 0, string RSDNCTRYCD = "US",
                                            decimal? rentalCharges = 0M, string contractTypeCode = null, decimal? contractNumber = null, string sacCode = null,
                                            string checkoutWorldWideISO = null, string promNum = null)
        {
            if (program == null) program = HertzProgram.GoldPointsRewards;

            TxnHeader output = new TxnHeader()
            {
                A_LYLTYMEMBERNUM = loyaltyId,
                A_CHKINDT = checkInDate,
                A_RANUM = RANUM.Generate(),
                A_RESVID = null,
                A_CHKINLOCNUM = "06",
                A_PROMNUM = promNum,
                A_VCHRNUM = null,
                A_CHKINAREANUM = "01474",
                A_CHKOUTDT = checkOutDate,
                A_CHKOUTLOCNUM = "06",
                A_CHKOUTAREANUM = "01474",
                //A_FTPTNRNUM = program.A_FTPTNRNUM,
                A_FTPTNRNUM = "ZE%",
                A_CHKOUTCITYCD = null,
                A_CHKOUTWORLDWIDERGNCTRYISO = checkoutWorldWideISO ?? RSDNCTRYCD,
                A_CHKOUTWWDSTPROVCD = StrongRandom.NumericString(2),
                A_ORIGBOOKDT = bookDate,
                A_CHRGVEHCLSCD = StrongRandom.NumericString(1),
                A_CRCARDTYPECD = "0",
                A_RQSTSIPPCD = null,
                A_GEOLOCTYPECD = null,
                A_RASRCCD = program.A_RASRCCD,
                A_INTRNLNETRTGCD = null,
                A_TRAVLPRPSTYPECD = null,
                A_MKTGRTANALCD = null,
                A_CRNCYISOCD = null,
                A_RENTALTYPE = null,
                A_DAYSCHRGQTY = 1,
                A_LDWCDWCHRGAMT = rentalCharges,
                A_DISCAMT = 0M,
                A_NWEXECSAMT = 0M,
                A_PAITOTCHRGAMT = 0M,
                A_ADDLAUTHDRVRCHRGAMT = 0M,
                A_AGEDIFFCHRGAMT = 0M,
                A_ADDLSRVCCHRGAMT = 0M,
                A_SBTOTAMT = 0M,
                A_TOTCHRGAMT = 0M,
                A_LISTOTCHRGAMT = 0M,
                A_CHILDSEATTOTAMT = 0M,
                A_ITVALLFEETOTAMT = 0M,
                A_GARSPECLEQMNTAMT = 0M,
                A_GRSREVNAMT = rentalCharges,
                A_MISCGRPAMT = 0M,
                A_NVGTNSYSTOTAMT = 0M,
                A_SATLTRADIOTOTAMT = 0M,
                A_REFUELINGOPTCD = StrongRandom.AlphaString(1),
                A_REFUELINGCHRGAMT = 0M,
                A_TPTOTCHRGAMT = 0M,
                A_TRANSACTIONSTATE = "MX",
                A_RESCHANNEL = StrongRandom.AlphaString(2),
                A_TOTEXPIRINGEUPGRADES = null,
                A_TOTEUPGRADES = null,
                A_RARTTYPECD = null,
                A_RESVRTTYPECD = null,
                A_RTCATCD = null,
                A_RTCLSNCD = null,
                A_RTFMLYCD = null,
                A_SACCD = sacCode ?? "N",
                A_RSDNCTRYCD = RSDNCTRYCD,
                A_GOLDRNTALIND = "Y",
                A_TXNDATE = checkInDate ?? DateTime.Now.Comparable(),
                A_TXNHEADERID = null,
                A_CHKOUTTM = null,
                A_TRANSTYPE = program.A_TRANSTYPE,
                A_RNTINGCTRYCRNCYUSDEXCHRT = 1M,
                A_CORPDISCPRGID = CDP ?? Convert.ToDecimal(StrongRandom.Next(0, 9)),
                A_CONTRACTTYPECD = contractTypeCode ?? StrongRandom.NumericString(3),
                A_CONTRACTNUM = contractNumber ?? Convert.ToDecimal(StrongRandom.Next(1, 999999)),
                A_BRANDID = null,
                A_CREDITCARDID = null,
                A_TXNMASKID = null,
                A_TXNNUMBER = null,
                A_TXNREGISTERNUMBER = StrongRandom.NumericString(1),
                A_TXNSTOREID = StrongRandom.Next(1, 9),
                A_TXNTYPEID = StrongRandom.Next(1, 9),
                A_TXNAMOUNT = 0M,
                A_TXNQUALPURCHASEAMT = 0M,
                A_QUALTOTAMT = 0M,
                A_TXNDISCOUNTAMOUNT = 0M,
                A_TXNEMPLOYEEID = StrongRandom.NumericString(2),
                A_TXNCHANNEL = StrongRandom.NumericString(2),
                A_TXNORIGINALTXNROWKEY = null,
                A_TXNCREDITSUSED = null,
                A_HODINDICATOR = HODIndicator              
        };
        //output.A_TXNQUALPURCHASEAMT = (output.A_SBTOTAMT+output.A_LDWCDWCHRGAMT + output.A_ADDLSRVCCHRGAMT + output.A_AGEDIFFCHRGAMT + output.A_ADDLAUTHDRVRCHRGAMT + output.A_CHILDSEATTOTAMT + output.A_MISCGRPAMT + output.A_GARSPECLEQMNTAMT + output.A_TOTCHRGAMT + output.A_NVGTNSYSTOTAMT + output.A_SATLTRADIOTOTAMT + output.A_REFUELINGCHRGAMT) * output.A_RNTINGCTRYCRNCYUSDEXCHRT;
        //output.A_QUALTOTAMT = (output.A_SBTOTAMT + output.A_LDWCDWCHRGAMT + output.A_ADDLSRVCCHRGAMT + output.A_AGEDIFFCHRGAMT + output.A_ADDLAUTHDRVRCHRGAMT + output.A_CHILDSEATTOTAMT + output.A_MISCGRPAMT + output.A_GARSPECLEQMNTAMT + output.A_TOTCHRGAMT + output.A_NVGTNSYSTOTAMT + output.A_SATLTRADIOTOTAMT + output.A_REFUELINGCHRGAMT) * output.A_RNTINGCTRYCRNCYUSDEXCHRT;

            return output;
        }
        public static IEnumerable<TxnHeader> GetFromDB(IDatabase db, decimal vckey, string RANUM = null)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {dbUser}.{TableName} where a_vckey = {vckey}");
            if (!String.IsNullOrEmpty(RANUM)) query.Append($" and a_ranum = '{RANUM}'");

            return db.Query<TxnHeader>(query.ToString());
        }
        
    }
    public class RANUM
    {
        private const int section1Max = 9999;
        private const int section2Max = 26;
        private const int section2MaxSize = 3;
        private const int section3Max = 99;

        private static RANUM current = null;
        private int section1;
        private int[] section2 = new int[section2MaxSize];
        private int section3;


        public RANUM()
        {
            section1 = StrongRandom.Next(0, 9000);
            for(int i = section2MaxSize-1; i >= 0; i--)
                section2[i] = StrongRandom.Next(0, 25);
            section3 = StrongRandom.Next(0, 99);
        }

        public static string Generate()
        {
            current = RANUM.current??new RANUM();
            string output = String.Format("{0:D4}{1}{2:D2}",current.section1, section2String(), current.section3);
            RANUM.incrementCurrent();
            return output;
        }
        private static void incrementCurrent()
        {
            if (current.section3 < 99) current.section3 += 1;
            else
            {
                current.section3 = 0;
                rollSection2();
            }
        }
        private static void rollSection2()
        {
            if (current.section2[section2MaxSize-1] < section2Max) current.section2[2] += 1;
            else
            {
                current.section2[2] = 0;
                if (current.section2[1] < section2Max) current.section2[1] += 1;
                else
                {
                    if (current.section2[0] < section2Max) current.section2[0] += 1;
                    else rollSection1();
                }
            }
        }
        private static void rollSection1()
        {
            if (current.section1 < section1Max) current.section1 += 1;
            else throw new Exception("RANUM overflow");
        }
        private static string section2String()
        {
            return String.Format("{0}{1}{2}", Convert.ToChar(65 + current.section2[0]), Convert.ToChar(65 + current.section2[1]), Convert.ToChar(65 + current.section2[2]));
        }
    }
}
