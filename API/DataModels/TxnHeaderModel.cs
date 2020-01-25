using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;

namespace Hertz.API.DataModels
{
    public class TxnHeaderModel
    {
        public decimal A_ROWKEY { get; set; }
        public decimal A_VCKEY { get; set; }
        public decimal? A_PARENTROWKEY { get; set; }
        [ModelAttribute("Key", equalityCheck: EqualityCheck.Skip)]
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
        [ModelAttribute("CHKOUTDT", ReportOption.Print, equalityCheck:EqualityCheck.Skip)]
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
        [ModelAttribute("TxnDate", equalityCheck: EqualityCheck.Skip)]
        public DateTime A_TXNDATE { get; set; }
        [ModelAttribute("TxnHeaderId", equalityCheck: EqualityCheck.Skip)]
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
        [ModelAttribute("TxnRegisterNumber", equalityCheck: EqualityCheck.Skip)]
        public string A_TXNREGISTERNUMBER { get; set; }
        [ModelAttribute("TxnStoreId", equalityCheck: EqualityCheck.Skip)]
        public long? A_TXNSTOREID { get; set; }
        [ModelAttribute("TxnTypeId", equalityCheck: EqualityCheck.Skip)]
        public long? A_TXNTYPEID { get; set; }
        [ModelAttribute("TxnAmount", equalityCheck: EqualityCheck.Skip)]
        public decimal? A_TXNAMOUNT { get; set; }
        [ModelAttribute("TxnQualPurchaseAmt", ReportOption.Print)]
        public decimal? A_TXNQUALPURCHASEAMT { get; set; }
        [ModelAttribute("TxnDiscountAmount")]
        public decimal? A_TXNDISCOUNTAMOUNT { get; set; }
        [ModelAttribute("TxnEmployeeId", equalityCheck: EqualityCheck.Skip)]
        public string A_TXNEMPLOYEEID { get; set; }
        [ModelAttribute("TxnChannel", equalityCheck: EqualityCheck.Skip)]
        public string A_TXNCHANNEL { get; set; }
        [ModelAttribute("TxnOriginalTxnRowKey")]
        public decimal? A_TXNORIGINALTXNROWKEY { get; set; }
        [ModelAttribute("TxnCreditsUsed", equalityCheck: EqualityCheck.Skip)]
        public long? A_TXNCREDITSUSED { get; set; }
        public decimal? STATUSCODE { get; set; }
        public DateTime CREATEDATE { get; set; }
        public DateTime? UPDATEDATE { get; set; }
        public decimal? LASTDMLID { get; set; }
        public decimal? LAST_DML_ID { get; set; }
        [ModelAttribute("HODIndicator", ReportOption.Print)]
        public short? A_HODINDICATOR { get; set; }
        [ModelAttribute("CHKINROWKEY", equalityCheck: EqualityCheck.Skip)]
        public decimal? A_CHKINROWKEY { get; set; }
        [ModelAttribute("CHKOUTROWKEY", equalityCheck: EqualityCheck.Skip)]
        public decimal? A_CHKOUTROWKEY { get; set; }
        [ModelAttribute("CHKINLOCATIONID")]
        public string A_CHKINLOCATIONID { get; set; }
        [ModelAttribute("CHKOUTLOCATIONID", ReportOption.Print)]
        public string A_CHKOUTLOCATIONID { get; set; }
    }
}
