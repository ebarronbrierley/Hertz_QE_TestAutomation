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
        
        public string A_TRANSTYPE { get { return "G"; } }
        public string EarningPreference { get { return "N1"; } }
        public string A_FTPTNRNUM { get { return "ZE1"; } }
        public string A_RASRCCD { get { return "1"; } }

        public class Tier
        {
            public static HertzTier RegularGold { get { return new HertzTier("Regular Gold", "RG", 0); } }
            public static HertzTier FiveStar { get { return new HertzTier("5-Star", "FG", 0.25M); } }
            public static HertzTier PresidentsCircle { get { return new HertzTier("Presidents Circle", "PC", 0.5M); } }
            public static HertzTier Platinum { get { return new HertzTier("Platinum", "PL", 0.5M); } }
            public static HertzTier PlatinumSelect { get { return new HertzTier("Platinum Select", "PS", 0.5M); } }
            public static HertzTier PlatinumVIP { get { return new HertzTier("Platinum VIP", "VP", 0.5M); } }
        }
    }
    public class Thrifty:IHertzProgram
    {
        public string SpecificTier { get { return null; } }
        public string A_TRANSTYPE { get { return "T"; } }
        public string EarningPreference { get { return "BC"; } }
        public string A_FTPTNRNUM { get { return "BC1"; } }
        public string A_RASRCCD { get { return "2"; } }
    }
    public class Dollar:IHertzProgram
    {
        public string SpecificTier { get { return null; } }
        public string A_TRANSTYPE { get { return "D"; } }
        public string EarningPreference { get { return "DX"; } }
        public string A_FTPTNRNUM { get { return "RR1"; } }
        public string A_RASRCCD { get { return "3"; } }
    }
    public class HertzTier : IHertzTier
    {
        public string Name { get; private set; }
        public string Code { get; private set; } 
        public decimal EarningRateModifier { get; private set; }

        public HertzTier(string tierName, string tierCode, decimal earningRateModifier)
        {
            this.Name = tierName;
            this.Code = tierCode;
            this.EarningRateModifier = earningRateModifier;
        }
    }

    public interface IHertzProgram
    {
        string SpecificTier { get; }
        string EarningPreference { get; }
        string A_FTPTNRNUM { get; }
        string A_RASRCCD { get; }
        string A_TRANSTYPE { get; }
    }
    public interface IHertzTier
    {
        string Name { get; }
        string Code { get;  }
        decimal EarningRateModifier { get;}
    }

}
