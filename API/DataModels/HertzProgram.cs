﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.DataModels
{
    public class HertzLoyalty
    {
        public static IEnumerable<IHertzProgram> Programs { 
            get { 
                yield return new GoldPointsRewards();
                yield return new ThriftyBlueChip();
                yield return new DollarExpressRenters();
            } 
        }
        public static GoldPointsRewards GoldPointsRewards => new GoldPointsRewards();
        public static ThriftyBlueChip ThriftyBlueChip => new ThriftyBlueChip();
        public static DollarExpressRenters DollarExpressRenters => new DollarExpressRenters();
    }

    public sealed class GoldPointsRewards : IHertzProgram
    {
        public string Name => "Gold Points Rewards";
        public string EarningPreference => "N1";

        public string A_FTPTNRNUM => "ZE1";

        public string A_RASRCCD => "1";

        public string A_TRANSTYPE => "G";

        public decimal CARDTYPE => 0M;

        public IEnumerable<IHertzTier> Tiers { 
            get 
            { 
                yield return RegularGold;
                yield return FiveStar;
                yield return PresidentsCircle;
                yield return Platinum;
                yield return PlatinumSelect;
                yield return PlatinumVIP;
            } 
        }

        public IHertzTier RegularGold { get { return new Tier(this, "Regular Gold", "RG", 0M, 12, 2400,1); } }
        public IHertzTier FiveStar { get { return new Tier( this, "5-Star", "FG", 0.25M, 20, 4000,2); } }
        public IHertzTier PresidentsCircle { get { return new Tier(this, "Presidents Circle", "PC", 0.5M, 0, 0,3); } }
        public IHertzTier Platinum { get { return new Tier(this, "Platinum", "PL", 0.5M, 0, 0,4); } }
        public IHertzTier PlatinumSelect {  get { return new Tier(this, "Platinum Select", "PS", 0.5M, 0, 0,5); } }
        public IHertzTier PlatinumVIP { get { return new Tier(this, "Platinum VIP", "VP", 0M, 0, 0,6); } }


        public GoldPointsRewards()
        {
            
        }
    }

    public sealed class ThriftyBlueChip:IHertzProgram
    {
        public string Name => "Thrifty Blue Chip";

        public string EarningPreference => "BC";

        public string A_FTPTNRNUM => "BC1";

        public string A_RASRCCD => "2";

        public string A_TRANSTYPE => "T";

        public decimal CARDTYPE => 2M;

        public IEnumerable<IHertzTier> Tiers { get { yield return DefaultTier; } }

        public IHertzTier DefaultTier { get { return new Tier(this, "No Tier", null, 0M, 0, 0); } }

        public ThriftyBlueChip()
        {
        }
    }

    public sealed class DollarExpressRenters : IHertzProgram
    {
        public string Name => "Dollar Express Renters";
        public string EarningPreference => "DX";

        public string A_FTPTNRNUM => "RR1";

        public string A_RASRCCD => "3";

        public string A_TRANSTYPE => "D";

        public decimal CARDTYPE => 3M;

        public IEnumerable<IHertzTier> Tiers { get { yield return DefaultTier; } }

        public IHertzTier DefaultTier { get { return new Tier(this, "No Tier", null, 0M, 0, 0); } }

        public DollarExpressRenters()
        {

        }
    }

    public class Tier:IHertzTier
    {
        public IHertzProgram ParentProgram { get; private set; }
        public string Name { get; private set; }

        public string Code { get; private set; }

        public decimal EarningRateModifier { get; private set; }

        public int RentalsToNextTier { get; private set; }

        public int RevenueToNextTier { get; private set; }

        public int Rank { get; set; }

        public Tier(IHertzProgram parent, string Name, string Code, decimal EarningRateModifier , int rentalsToNextTier, int revenueToNextTier)
        {
            this.ParentProgram = parent;
            this.Name = Name;
            this.Code = Code;
            this.EarningRateModifier = EarningRateModifier;
            this.RentalsToNextTier = rentalsToNextTier;
            this.RevenueToNextTier = revenueToNextTier;
            this.Rank = 1;
        }

        public Tier(IHertzProgram parent, string Name, string Code, decimal EarningRateModifier,
            int rentalsToNextTier, int revenueToNextTier, int rank)
        {
            this.ParentProgram = parent;
            this.Name = Name;
            this.Code = Code;
            this.EarningRateModifier = EarningRateModifier;
            this.RentalsToNextTier = rentalsToNextTier;
            this.RevenueToNextTier = revenueToNextTier;
            this.Rank = rank;
        }
    }

    public interface IHertzProgram
    {
        string Name { get; }
        string EarningPreference { get; }
        string A_FTPTNRNUM { get; }
        string A_RASRCCD { get; }
        string A_TRANSTYPE { get; }
        decimal CARDTYPE { get; }
        IEnumerable<IHertzTier> Tiers { get; }
    }

    public interface IHertzTier
    {
        IHertzProgram ParentProgram { get; }
        string Name { get; }
        string Code { get; }
        decimal EarningRateModifier { get; }
        int RentalsToNextTier { get; }
        int RevenueToNextTier { get; }
        /// <summary>
        /// Used to order the tiers for tiering testing
        /// </summary>
        int Rank { get; }
    }
}
