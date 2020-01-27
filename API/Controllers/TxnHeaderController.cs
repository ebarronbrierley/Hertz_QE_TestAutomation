using Brierley.TestAutomation.Core.Database;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.Controllers
{
    public class TxnHeaderController
    {
        private IDatabase dbContext;
        private IStepManager stepContext;
        public TxnHeaderController(IDatabase dbContext = null, IStepManager stepContext = null)
        {
            this.dbContext = dbContext;
            this.stepContext = stepContext;
        }

        public static decimal? CalculateQualifyingPurchaseAmount(TxnHeaderModel expectedTransaction)
        {
            decimal? sumOfValues = Sum(expectedTransaction.A_SBTOTAMT, expectedTransaction.A_LDWCDWCHRGAMT, expectedTransaction.A_ADDLSRVCCHRGAMT, 
                                                expectedTransaction.A_AGEDIFFCHRGAMT, expectedTransaction.A_ADDLAUTHDRVRCHRGAMT, expectedTransaction.A_CHILDSEATTOTAMT, 
                                                expectedTransaction.A_MISCGRPAMT, expectedTransaction.A_GARSPECLEQMNTAMT, expectedTransaction.A_TOTCHRGAMT, 
                                                expectedTransaction.A_NVGTNSYSTOTAMT, expectedTransaction.A_SATLTRADIOTOTAMT, expectedTransaction.A_REFUELINGCHRGAMT)
                                            * expectedTransaction.A_RNTINGCTRYCRNCYUSDEXCHRT??1M;

            decimal? total = Multiply(sumOfValues, expectedTransaction.A_RNTINGCTRYCRNCYUSDEXCHRT);

            return total;
        }

        public static TxnHeaderModel GenerateTransaction(string loyaltyId,
            DateTime? checkInDate = null, DateTime? checkOutDate = null, DateTime? bookDate = null,
            decimal? CDP = null, IHertzProgram program = null, short? HODIndicator = 0, string RSDNCTRYCD = "US",
            decimal? rentalCharges = 0M, string contractTypeCode = null, decimal? contractNumber = null, string sacCode = null,
            string checkoutWorldWideISO = null, string promNum = null, string chkoutlocnum = "06", string chkoutareanum = "01474", string chkoutlocid = null)
        {

            if (program == null) program = HertzLoyalty.GoldPointsRewards;

            TxnHeaderModel txn = new TxnHeaderModel()
            {
                A_LYLTYMEMBERNUM = loyaltyId,
                A_CHKINDT = checkInDate,
                A_RANUM = RANUMController.Generate(),
                A_RESVID = null,
                A_CHKINLOCNUM = "06",
                A_PROMNUM = promNum,
                A_VCHRNUM = null,
                A_CHKINAREANUM = "01474",
                A_CHKOUTDT = checkOutDate,
                A_CHKOUTLOCNUM = chkoutlocnum,
                A_CHKOUTAREANUM = chkoutareanum,
                A_FTPTNRNUM = program.A_FTPTNRNUM,
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
                A_GRSREVNAMT = 0M,
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
                A_HODINDICATOR = HODIndicator,
                A_CHKOUTLOCATIONID = chkoutlocid
            };

            return txn;
        }

        public IEnumerable<TxnHeaderModel> GetFromDB(decimal vckey, string RANUM = null)
        {
            StringBuilder query = new StringBuilder();
            query.Append($"select * from {TxnHeaderModel.TableName} where A_VCKEY = {vckey}");
            if (!String.IsNullOrEmpty(RANUM)) query.Append($" and A_RANUM = '{RANUM}'");

            return dbContext.Query<TxnHeaderModel>(query.ToString());
        }

        private static decimal? Sum(params decimal?[] values)
        {
            decimal? output = null;
            foreach (var value in values)
            {
                if (value.HasValue) output += value.Value;
            }
            return output;
        }
        private static decimal? Multiply(params decimal?[] values)
        {
            decimal? output = null;
            if (values.Any(x => x == null)) return null;
            foreach (var value in values)
            {
                output += value.Value;
            }
            return output;
        }
    }

   
}
