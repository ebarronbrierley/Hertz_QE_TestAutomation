using Hertz.API.Controllers;
using Hertz.API.DataModels;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Text;

namespace Hertz.API.TestData
{
    public class HertzTransferPointsTestData
    {
        public static decimal points = 100;

        public static IEnumerable PositiveScenarios
        {
            get
            {
                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    IHertzTier tier = program.Tiers.First();

                    MemberModel memberS1 = MemberController.GenerateRandomMember(tier);
                    memberS1.MemberDetails.A_COUNTRY = "US";

                    MemberModel memberD1 = MemberController.GenerateRandomMember(tier);
                    memberD1.MemberDetails.A_COUNTRY = "US";
                    bool useRanum = true;
                    yield return new TestCaseData(memberS1, memberD1, points, tier, useRanum)
                        .SetName($"HertzTransferPoints Positive - Program:[{program.Name}] Tier:[{tier.Name}] Points:[{points}] With Ranum {useRanum}");
                }
            }

        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                StringBuilder errorMessage = new StringBuilder();

                foreach (IHertzProgram program in HertzLoyalty.Programs)
                {
                    IHertzTier tier = program.Tiers.First();
                    MemberModel member = MemberController.GenerateRandomMember(tier);
                    MemberModel memberD = MemberController.GenerateRandomMember(tier);                                       

                    int errorCode = 708;
                    errorMessage.Clear().Append("Sending Loyalty Member Id must be populated");
                    yield return new TestCaseData(null, memberD, errorCode, errorMessage.ToString(), "100", "csadmin", "automationNeg")
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] Sending Loyalty Member Id must be populated");

                    errorCode = 704;
                    errorMessage.Clear().Append("Receiving Loyalty Member Id must be populated");
                    yield return new TestCaseData(member, null, errorCode, errorMessage.ToString(), "100", "csadmin", "automationNeg")
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] Receiving Loyalty Member Id must be populated");

                    errorCode = 200;
                    errorMessage.Clear().Append("Agent Username must be populated");
                    yield return new TestCaseData(member, memberD, errorCode, errorMessage.ToString(), "100", null, "automationNeg")
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] Agent Username must be populated");

                    errorCode = 600;
                    errorMessage.Clear().Append("Points must be populated");
                    yield return new TestCaseData(member, memberD, errorCode, errorMessage.ToString(), null, "csadmin", "automationNeg")
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] Points must be populated");

                    errorCode = 614;
                    errorMessage.Clear().Append("Numeric Data Required");
                    yield return new TestCaseData(member, memberD, errorCode, errorMessage.ToString(), "cat", "csadmin", "automationNeg")
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] Numeric Data Required");

                    errorCode = 502;
                    errorMessage.Clear().Append("Reason Code must be populated");
                    yield return new TestCaseData(member, memberD, errorCode, errorMessage.ToString(), "100", "csadmin", null)
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] Reason Code must be populated");

                    errorCode = 201;
                    errorMessage.Clear().Append("Invalid Agent Username");
                    yield return new TestCaseData(member, memberD, errorCode, errorMessage.ToString(), "100", "auttestagent", "automationNeg")
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] Invalid Agent Username");

                    errorCode = 202;
                    errorMessage.Clear().Append("CS Agent does not have the permissions to issue this number of points");
                    yield return new TestCaseData(member, memberD, errorCode, errorMessage.ToString(), "1000", "devres", "automationNeg")
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] CS Agent does not have the permissions to issue this number of points");
                    
                    errorCode = 707;
                    errorMessage.Clear().Append("Sending Loyalty Member has insufficient points");
                    yield return new TestCaseData(member, memberD, errorCode, errorMessage.ToString(), "100", "csadmin", "automationNeg")
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] Sending Loyalty Member has insufficient points");


                    IHertzTier anotherBrandTier = HertzLoyalty.Programs
                                                    .Where(x=>x.Name!= program.Name)
                                                    .First()
                                                    .Tiers.First();
                    memberD = MemberController.GenerateRandomMember(anotherBrandTier);
                    errorCode = 706;
                    errorMessage.Clear().Append("Sending loyalty member and receiving loyalty member must be in the same brand");
                    yield return new TestCaseData(member, memberD, errorCode, errorMessage.ToString(), "100", "csadmin", "automationNeg")
                        .SetName($"HertzTransferPoints  Negative - Program:[{program.Name}] Sending loyalty member and receiving loyalty member must be in the same brand");

                }

            }
        }

    }
}
