using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HertzNetFramework.DataModels
{
    public class HertzProgram
    {
        public static IHertzProgram GoldPointsRewards
        {
            get { return new GPR(); }
        }
        public static IHertzProgram ThriftyBlueChip
        {
            get { return new Thrifty(); }
        }
        public static IHertzProgram DollarExpressRenters
        {
            get { return new Dollar(); }
        }
    }
    public class GPR:IHertzProgram
    {
        public string SpecificTier { get; set; }
        public IEnumerable<IHertzTier> TierCodes { get { return new List<HertzTier>()
        {
            new HertzTier("Regular Gold", TierCode.RegularGold, 0),
            new HertzTier("5-Star", TierCode.FiveStar, 0.25),
            new HertzTier("Presidents Circle", TierCode.PresidentsCircle, 0.5),
            new HertzTier("Platinum",TierCode.Platinum,0.5)
        }; } }
        public string A_TRANSTYPE { get { return "G"; } }
        public string EarningPreference { get { return "N1"; } }
        public string A_FTPTNRNUM { get { return "ZE1"; } }
        public string A_RASRCCD { get { return "1"; } }

        public class TierCode
        {
            public static string RegularGold = "RG";
            public static string FiveStar = "FG";
            public static string PresidentsCircle = "PC";
            public static string Platinum = "PL";
        }
    }
    public class Thrifty:IHertzProgram
    {
        public string SpecificTier { get { return null; } }
        public IEnumerable<IHertzTier> TierCodes { get { return new List<IHertzTier>(); } }
        public string A_TRANSTYPE { get { return "T"; } }
        public string EarningPreference { get { return "BC"; } }
        public string A_FTPTNRNUM { get { return "BC1"; } }
        public string A_RASRCCD { get { return "2"; } }
    }
    public class Dollar:IHertzProgram
    {
        public string SpecificTier { get { return null; } }
        public IEnumerable<IHertzTier> TierCodes { get { return new List<IHertzTier>(); } }
        public string A_TRANSTYPE { get { return "D"; } }
        public string EarningPreference { get { return "DX"; } }
        public string A_FTPTNRNUM { get { return "RR1"; } }
        public string A_RASRCCD { get { return "3"; } }
    }
    public class HertzTier : IHertzTier
    {
        public string TierName { get; private set; }
        public string TierCode { get; private set; } 
        public double EarningRateModifier { get; private set; }

        public HertzTier(string tierName, string tierCode, double earningRateModifier)
        {
            this.TierName = tierName;
            this.TierCode = tierCode;
            this.EarningRateModifier = earningRateModifier;
        }
    }

    public interface IHertzProgram
    {
        IEnumerable<IHertzTier> TierCodes { get; }
        string SpecificTier { get; }
        string EarningPreference { get; }
        string A_FTPTNRNUM { get; }
        string A_RASRCCD { get; }
        string A_TRANSTYPE { get; }
    }
    public interface IHertzTier
    {
        string TierName { get; }
        string TierCode { get;  }
        double EarningRateModifier { get;}
    }

}
