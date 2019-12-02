﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Brierley.TestAutomation.Core.Reporting;
using Brierley.TestAutomation.Core.Utilities;
using HertzNetFramework.DataModels;
using System.Collections;
using Brierley.TestAutomation.Core.Database;

namespace HertzNetFramework.Tests.SOAP
{
    [TestFixture]
    class ExclusionListTests : BrierleyTestFixture
    {
        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("ExclusionList")]
        [Category("ExclusionList_Positive")]
        [Test]
        public void ExclusionList_Positive()
        {
            try
            {

                BPTest.Start<TestStep>($"Step 0: Create an RG Member");
                decimal[] RemovedCDPs = new decimal[] { 552641M , 552642M , 603849M , 610205M , 663884M , 663888M , 779693M ,
                                                        792763M , 793627M , 881706M , 965871M , 538987M , 525191M  , 517402M };

                decimal[] HTZ18536RemovedCDPs = new decimal[] { 67447M, 66442M,64747M, 358685M };

                decimal[] HTZ18536_NonEarningCDPs = new decimal[] {69042M,
                    67198M,
                    67045M,
                    67046M,
                    67047M,
                    66824M,
                    68779M,
                    68826M,
                    68827M,
                    68834M,
                    68836M,
                    68895M,
                    68902M,
                    68952M,
                    68985M,
                    69022M,
                    25036M,
                    68977M,
                    68981M,
                    67049M,
                    67048M,
                    67051M,
                    66825M,
                    180334M,
                    371153M,
                    480700M,
                    431493M,
                    433369M,
                    331990M,
                    310543M,
                    265480M,
                    67406M,
                    67405M,
                    67404M,
                    67403M,
                    67355M,
                    68832M,
                    68982M,
                    69038M,
                    69107M,
                    69121M,};
                
                Member testMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("RG", "MemberDetails.A_TIERCODE");
                Member memberOut = Member.AddMember(testMember);
                string loyaltyid = testMember.GetLoyaltyID();

                string query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{loyaltyid}'";
                Hashtable ht = Database.QuerySingleRow(query);
                decimal vckey = Convert.ToDecimal(ht["VCKEY"]);

                DateTime checkInDt = new DateTime(2019, 10, 2);
                DateTime checkOutDt = new DateTime(2019, 10, 1);
                DateTime origBkDt = new DateTime(2019, 10, 1);
                int count = 1;
                BPTest.Pass<TestStep>($"RG Member Created");
                
                foreach (decimal cdp in HTZ18536_NonEarningCDPs)
                {
                    BPTest.Start<TestStep>($"Step {count}: Test CDP {cdp}");
                    TxnHeader txnHeader = TxnHeader.Generate(testMember.GetLoyaltyID(), checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 100, "AAAA", 246095, "N", "US", null);
                    testMember.AddTransaction(txnHeader);
                    Member.UpdateMember(testMember);
                    testMember.RemoveTransaction();
                    BPTest.Pass<TestStep>($"Step {count++} Passed");
                }
                BPTest.Start<TestStep>($"Step {count}: Validate DB");
                string query2 = $@"select * from bp_htz.lw_pointtransaction where vckey = '{vckey}'";
                QueryResult qr = Database.Query(query2);
                int dbCount = qr.Rows.Count;
                if ( dbCount == count-1)
                {
                    BPTest.Pass<TestStep>($"Step {count} Passed");
                }
                else
                {
                    BPTest.Fail<TestStep>($"Step {count} Failed");
                }

            }
            catch(Exception ex)
            {
               string x = ex.Message;
            }
        }

        [Category("Api_Smoke")]
        [Category("Api_Positive")]
        [Category("ExclusionList")]
        [Category("ExclusionList_Positive")]
        [Test]
        public void ExclusionList_Negative()
        {
            try
            {
                BPTest.Start<TestStep>($"Step 0: Create an RG Member");
                decimal[] RemovedCDPs = new decimal[] { 552641M , 552642M , 603849M , 610205M , 663884M , 663888M , 779693M ,
                                                        792763M , 793627M , 881706M , 965871M , 538987M , 525191M  , 517402M };
                decimal[] HTZ18536_NewlyNonEarningCDPs = new decimal[] {69042M,
                    67198M,
                    67045M,
                    67046M,
                    67047M,
                    66824M,
                    68779M,
                    68826M,
                    68827M,
                    68834M,
                    68836M,
                    68895M,
                    68902M,
                    68952M,
                    68985M,
                    69022M,
                    25036M,
                    68977M,
                    68981M,
                    67049M,
                    67048M,
                    67051M,
                    66825M,
                    180334M,
                    371153M,
                    480700M,
                    431493M,
                    433369M,
                    331990M,
                    310543M,
                    265480M,
                    67406M,
                    67405M,
                    67404M,
                    67403M,
                    67355M,
                    68832M,
                    68982M,
                    69038M,
                    69107M,
                    69121M};
                
                decimal[] HTZ18536_AlreadyNonEarningCDPs = new decimal[] {
                    102219M,
                    437438M,
                    449882M,
                    383561M,
                    448960M,
                    239071M,
                    315583M,
                    67012M,
                    67011M,
                    308379M,
                    50993M,
                    34385M,
                    309585M,
                    12363M,
                    17376M,
                    451105M,
                    414437M,
                    39446M,
                    65477M,
                    267623M,
                    304106M,
                    67745M,
                    334611M,
                    65059M,
                    65058M,
                    65137M,
                    65139M,
                    35093M,
                    67876M,
                    68358M,
                    68455M,
                    68691M,
                    68726M,
                    50673M,
                    434844M,
                    64524M,
                    463389M,
                    463281M,
                    27895M,
                    65687M,
                    325583M,
                    67817M,
                    55124M,
                    312043M,
                    52712M,
                    67449M,
                    67450M,
                    67453M,
                    443262M,
                    67452M,
                    413263M,
                    66087M,
                    49584M,
                    49585M,
                    57598M,
                    67455M,
                    65922M,
                    45922M,
                    65919M,
                    67148M,
                    59958M,
                    61065M,
                    68927M,
                    68755M,
                    68725M,
                    68635M,
                    68609M,
                    68540M,
                    68539M,
                    68481M,
                    68480M,
                    68456M,
                    68413M,
                    68391M,
                    68390M,
                    67653M,
                    68290M,
                    68276M,
                    68274M,
                    68273M,
                    68199M,
                    354783M,
                    68182M,
                    68157M,
                    67942M,
                    67942M,
                    67879M,
                    67878M,
                    67877M,
                    67867M,
                    67680M,
                    67478M,
                    49250M,
                    67476M,
                    67477M,
                    67526M,
                    66197M,
                    66818M,
                    444043M,
                    442078M,
                    37851M,
                    66203M,
                    65444M,
                    13619M,
                    102495M,
                    52314M,
                    64874M,
                    269851M,
                    324261M,
                    49251M,
                    64676M,
                    343932M,
                    52315M,
                    281733M,
                    321616M,
                    356176M,
                    322431M,
                    337698M,
                    65651M,
                    322632M,
                    453094M,
                    67073M,
                    65686M,
                    48497M,
                    52504M,
                    324491M,
                    40796M,
                    65910M,
                    67419M,
                    67407M,
                    67375M,
                    67281M,
                    67284M,
                    67283M,
                    67282M,
                    67280M,
                    67249M,
                    67248M,
                    67230M,
                    67165M,
                    67205M,
                    66903M,
                    66892M,
                    66887M,
                    66886M,
                    66885M,
                    66839M,
                    66822M,
                    68389M,
                    66810M,
                    66774M,
                    66775M,
                    66655M,
                    66597M,
                    66356M,
                    66361M,
                    66367M,
                    66382M,
                    66395M,
                    66443M,
                    66464M,
                    66457M,
                    20322M,
                    20323M,
                    53174M,
                    53173M,
                    57130M,
                    38717M,
                    52753M,
                    36668M,
                    37969M,
                    36450M,
                    52826M,
                    67880M,
                    7138M,
                    55247M,
                    320828M,
                    422415M,
                    64852M,
                    61066M,
                    324493M,
                    325270M,
                    450630M,
                    64557M,
                    276988M,
                    338608M,
                    64378M,
                    440430M,
                    64748M,
                    66937M,
                    48874M,
                    143518M,
                    263972M,
                    494603M,
                    46243M,
                    271135M,
                    65684M,
                    64257M,
                    331734M,
                    46242M,
                    284023M,
                    306017M,
                    65660M,
                    311505M,
                    446098M,
                    211631M,
                    270395M,
                    67616M,
                    474095M,
                    353212M,
                    40000M,
                    65752M,
                    259632M,
                    390228M,
                    353574M,
                    141012M,
                    240261M,
                    481206M,
                    311919M,
                    229876M,
                    139708M,
                    457783M,
                    51628M,
                    254212M,
                    54141M,
                    48875M,
                    67615M,
                    319138M,
                    2186M,
                    325584M,
                    497543M,
                    329479M,
                    64684M,
                    427341M,
                    37574M,
                    242957M,
                    155937M,
                    55184M,
                    2189M,
                    11850M,
                    55185M,
                    450294M,
                    427020M,
                    140998M,
                    65740M,
                    56127M,
                    201453M,
                    135391M,
                    211771M,
                    491689M,
                    218904M,
                    260066M,
                    52165M,
                    425001M,
                    63046M,
                    67069M,
                    64715M,
                    325678M,
                    67542M,
                    448307M,
                    208573M,
                    489211M,
                    407742M,
                    60983M,
                    20038M,
                    324613M,
                    36916M,
                    320059M,
                    284640M,
                    467536M,
                    66024M,
                    4912M,
                    495177M,
                    54288M,
                    312516M,
                    64643M,
                    362236M,
                    64791M,
                    64286M,
                    312258M,
                    65138M,
                    284710M,
                    65126M,
                    22910M,
                    277272M,
                    515M,
                    12155M,
                    56174M,
                    471916M,
                    374181M,
                    66031M,
                    276579M,
                    48432M,
                    64340M,
                    450296M,
                    67057M,
                    265211M,
                    458477M,
                    58646M,
                    64644M,
                    47516M,
                    463391M,
                    862M,
                    863M,
                    55325M,
                    47517M,
                    47518M,
                    51355M,
                    282141M,
                    63949M,
                    391079M,
                    499678M,
                    57711M,
                    48393M,
                    66848M,
                    278175M,
                    407743M,
                    56555M,
                    51399M,
                    4732M,
                    289888M,
                    66849M,
                    451669M,
                    61063M,
                    24491M,
                    64950M,
                    326631M,
                    289787M,
                    494150M,
                    411241M,
                    411242M,
                    496541M,
                    363114M,
                    466509M,
                    66753M,
                    14066M,
                    303136M,
                    386109M,
                    308143M,
                    50992M,
                    66961M,
                    63083M,
                    50327M,
                    59168M,
                    67750M,
                    450781M,
                    64525M,
                    64523M,
                    448427M,
                    50675M,
                    53380M,
                    331826M,
                    65685M,
                    211774M,
                    358686M,
                    53377M,
                    228039M,
                    67536M,
                    311855M,
                    269138M,
                    63950M,
                    25471M,
                    29387M,
                    349868M,
                    265210M,
                    61031M,
                    53002M,
                    407575M,
                    61064M,
                    413083M,
                    413084M,
                    64526M,
                    415407M,
                    64851M,
                    64565M,
                    17382M,
                    66959M,
                    35097M,
                    54139M,
                    237086M,
                    56552M,
                    64645M,
                    66566M,
                    401997M,
                    22610M,
                    421766M,
                    53783M,
                    237331M,
                    425589M,
                    443467M,
                    317279M,
                    65011M,
                    64459M,
                    436546M,
                    238530M,
                    474541M,
                    281100M,
                    65019M,
                    327600M,
                    57710M,
                    323304M,
                    478607M,
                    299575M,
                    66653M,
                    63048M,
                    460700M,
                    66751M,
                    315703M,
                    240203M,
                    249023M,
                    66744M,
                    306161M,
                    64342M,
                    282399M,
                    446408M,
                    422834M,
                    67741M,
                    437633M,
                    450080M,
                    451785M,
                    60231M,
                    60232M,
                    461647M,
                    492291M,
                    457022M,
                    65209M,
                    66713M,
                    314774M,
                    419084M,
                    67092M,
                    301117M,
                    316871M,
                    230554M,
                    326934M,
                    64187M,
                    66958M,
                    24316M,
                    368867M,
                    389004M,
                    301017M,
                    2438M,
                    33091M,
                    66985M,
                    64873M,
                    62365M,
                    47530M,
                    306999M,
                    394279M,
                    64793M,
                    552704M,
                    344609M,
                    54893M,
                    41333M,
                    199651M,
                    34053M,
                    247699M,
                    299749M,
                    475595M,
                    350313M,
                    7817M,
                    40631M,
                    248414M,
                    66329M,
                    352448M,
                    252294M,
                    447063M,
                    308157M,
                    370757M,
                    42937M,
                    65098M,
                    65109M,
                    261554M,
                    50991M,
                    442465M,
                    322260M,
                    323533M,
                    66462M,
                    66463M,
                    65683M,
                    442562M,
                    62588M,
                    63226M,
                    63486M,
                    66106M,
                    65829M,
                    488045M,
                    483419M,
                    429433M,
                    310114M,
                    65476M,
                    310020M,
                    309997M,
                    47513M,
                    450629M,
                    65909M,
                    432928M,
                    36096M,
                    64258M,
                    50258M,
                    438600M,
                    47514M,
                    334611M,
                    66462M,
                    68278M,
                    66821M,
                    260833M,
                    67439M,
                    67440M,
                    68246M,
                    68247M,
                    68248M,
                    68322M,
                    68342M,
                    68366M,
                    68388M,
                    67129M,
                    67127M,
                    68440M,
                    68506M,
                    68717M,
                    67130M,
                    260357M,
                    260357M,
                    68445M,
                    68490M,
                    68579M,
                    68560M,
                    68727M,
                    754382M,
                    68541M,
                    63761M,
                    262705M,
                    4412M,
                    66529M,
                    379362M,
                    46186M,
                    68446M,
                    68464M,
                    16441M,
                    16442M,
                    16440M,
                    16300M,
                    16299M,
                    16293M,
                    16292M,
                    16307M,
                    16301M,
                    16302M,
                    16303M,
                    16304M,
                    16306M,
                    16305M,
                    16308M,
                    14565M,
                    14566M,
                    14567M,
                    14568M,
                    14569M,
                    14564M,
                    14571M,
                    14572M,
                    14573M,
                    14577M,
                    14576M,
                    14575M,
                    14574M,
                    15162M,
                    13395M,
                    15243M,
                    15242M,
                    15241M,
                    13861M,
                    13862M,
                    13863M,
                    13864M,
                    13859M,
                    15774M,
                    15776M,
                    15775M,
                    14250M,
                    14246M,
                    14249M,
                    14245M,
                    14248M,
                    14263M,
                    14262M,
                    14259M,
                    14258M,
                    14257M,
                    14256M,
                    14255M,
                    14254M,
                    14261M,
                    14260M,
                    14252M,
                    14251M,
                    14270M,
                    14269M,
                    14268M,
                    14267M,
                    14266M,
                    14265M,
                    14264M,
                    14280M,
                    14279M,
                    14278M,
                    14277M,
                    14276M,
                    14274M,
                    14273M,
                    14281M,
                    14272M,
                    14271M,
                    14286M,
                    14284M,
                    14285M,
                    14283M,
                    14282M,
                    14287M,
                    15879M,
                    15878M,
                    15880M,
                    15881M,
                    15883M,
                    15882M,
                    15886M,
                    15885M,
                    15884M,
                    15888M,
                    15887M,
                    15889M,
                    15890M,
                    17436M,
                    16291M,
                    16292M,
                    16293M,
                    16299M,
                    16300M,
                    16301M,
                    16302M,
                    16303M,
                    16304M,
                    16305M,
                    16306M,
                    16307M,
                    16440M,
                    16441M,
                    16442M,
                    17436M,
                    19910M,
                    19911M,
                    19912M,
                    19913M,
                    19914M,
                    19915M,
                    19916M,
                    19917M,
                    19918M,
                    19919M,
                    19920M,
                    19921M,
                    19922M,
                    19923M,
                    22294M,
                    25745M,
                    31388M,
                    31389M,
                    31390M,
                    31391M,
                    31392M,
                    31393M,
                    31394M,
                    31395M,
                    31396M,
                    31397M,
                    31398M,
                    31399M,
                    31400M,
                    31402M,
                    31403M,
                    31404M,
                    31405M,
                    31406M,
                    31407M,
                    31408M,
                    31409M,
                    31410M,
                    31411M,
                    31412M,
                    31413M,
                    31414M,
                    31415M,
                    31416M,
                    31417M,
                    31418M,
                    31419M,
                    31420M,
                    31421M,
                    31422M,
                    31423M,
                    31424M,
                    31425M,
                    31426M,
                    31427M,
                    31428M,
                    31429M,
                    31430M,
                    31431M,
                    31432M,
                    31433M,
                    31434M,
                    31435M,
                    31436M,
                    31437M,
                    31438M,
                    31439M,
                    31440M,
                    31441M,
                    31442M,
                    31443M,
                    31444M,
                    31445M,
                    31446M,
                    31447M,
                    31448M,
                    31449M,
                    31450M,
                    31451M,
                    31452M,
                    31453M,
                    31454M,
                    31455M,
                    31456M,
                    31457M,
                    31458M,
                    31459M,
                    34387M,
                    35172M,
                    35173M,
                    35174M,
                    35175M,
                    35176M,
                    35177M,
                    35178M,
                    35179M,
                    35181M,
                    35182M,
                    35811M,
                    36618M,
                    37420M,
                    45019M,
                    47282M,
                    51030M,
                    52344M,
                    53825M,
                    55123M,
                    61236M,
                    65014M,
                    65720M,
                    65757M,
                    66606M,
                    67772M,
                    67773M,
                    67774M,
                    67775M,
                    67778M,
                    67779M,
                    67781M,
                    67782M,
                    67783M,
                    67784M,
                    67785M,
                    67786M,
                    67787M,
                    67788M,
                    67789M,
                    67790M,
                    67791M,
                    67792M,
                    67793M,
                    67794M,
                    67795M,
                    67796M,
                    67797M,
                    67798M,
                    67799M,
                    67801M,
                    67802M,
                    13435M,
                    67947M,
                    67974M,
                    67976M,
                    67977M,
                    67978M,
                    67979M,
                    67981M,
                    67982M,
                    67983M,
                    67984M,
                    67985M,
                    67986M,
                    67987M,
                    67988M,
                    67989M,
                    67992M,
                    67993M,
                    67994M,
                    67995M,
                    67996M,
                    67997M,
                    67998M,
                    68001M,
                    68002M,
                    68003M,
                    68004M,
                    68005M,
                    68007M,
                    68008M,
                    68010M,
                    68011M,
                    68013M,
                    68014M,
                    68016M,
                    68017M,
                    68018M,
                    68019M,
                    68023M,
                    68024M,
                    68025M,
                    68026M,
                    68027M,
                    68028M,
                    68031M,
                    68032M,
                    68033M,
                    68035M,
                    68036M,
                    68037M,
                    68038M,
                    68056M,
                    68058M,
                    68060M,
                    68061M,
                    68062M,
                    68063M,
                    68064M,
                    68065M,
                    68067M,
                    68068M,
                    68069M,
                    68071M,
                    68073M,
                    68074M,
                    68077M,
                    68078M,
                    68080M,
                    68082M,
                    68083M,
                    68084M,
                    68085M,
                    68086M,
                    68087M,
                    68088M,
                    68089M,
                    68090M,
                    68091M,
                    68092M,
                    68093M,
                    68094M,
                    68095M,
                    68096M,
                    68097M,
                    68098M,
                    68099M,
                    68101M,
                    68102M,
                    68103M,
                    68104M,
                    68105M,
                    68106M,
                    68107M,
                    68108M,
                    68109M,
                    68110M,
                    68111M,
                    68198M,
                    68298M,
                    67991M,
                    68006M,
                    68009M,
                    68015M,
                    68020M,
                    68021M,
                    68022M,
                    68029M,
                    68030M,
                    68034M,
                    68057M,
                    68059M,
                    68066M,
                    68070M,
                    68072M,
                    68079M,
                    68100M,
                    68754M,
                    68762M

                };

                decimal[] HTZ18527_NewlyNonEarningCDPs = new decimal[] {
                    785507M,
                    817301M,
                    785505M,
                    809778M,
                    887128M,
                    869312M,
                    785520M,
                    878605M,
                    888188M,
                    785521M,
                    856882M,
                    785527M,
                    880158M,
                    785529M,
                    825329M,
                    795157M,
                    885145M,
                    832233M,
                    785544M,
                    785548M,
                    785546M,
                    785545M,
                    785549M,
                    785550M,
                    785553M,
                    785556M,
                    785032M,
                    814124M,
                    785568M,
                    785569M,
                    785571M,
                    785570M,
                    839592M,
                    880097M,
                    881422M,
                    839518M,
                    856931M,
                    839187M,
                    785574M,
                    805148M,
                    856878M,
                    785578M,
                    820492M,
                    888716M,
                    805147M,
                    785597M,
                    870228M,
                    785602M,
                    785604M,
                    785605M,
                    785608M,
                    785610M,
                    876826M,
                    880098M,
                    881420M,
                    870256M,
                    785704M,
                    785717M,
                    785711M,
                    785636M,
                    785710M,
                    785713M,
                    785716M,
                    785643M,
                    785714M,
                    785715M,
                    884937M,
                    785614M,
                    885157M,
                    785021M,
                    785624M,
                    785022M,
                    817471M,
                    785718M,
                    785626M,
                    693800M,
                    785641M,
                    785707M,
                    828477M,
                    828478M,
                    828479M,
                    828480M,
                    828481M,
                    828482M,
                    828483M,
                    828484M,
                    828486M,
                    828487M,
                    828488M,
                    828489M,
                    828490M,
                    828491M,
                    828492M,
                    828493M,
                    828494M,
                    828495M,
                    828496M,
                    828497M,
                    828499M,
                    828500M,
                    828501M,
                    828502M,
                    850896M,
                    828504M,
                    828505M,
                    828506M,
                    828508M,
                    828510M,
                    828511M,
                    828512M,
                    828513M,
                    828514M,
                    828515M,
                    828516M,
                    828517M,
                    828518M,
                    828519M,
                    828520M,
                    828521M,
                    828522M,
                    828523M,
                    828524M,
                    828525M,
                    828526M,
                    828527M,
                    828528M,
                    828529M,
                    828530M,
                    828531M,
                    828533M,
                    828534M,
                    828535M,
                    828536M,
                    828538M,
                    828537M,
                    828539M,
                    828540M,
                    828541M,
                };

                Member testMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("PL", "MemberDetails.A_TIERCODE");
                Member memberOut = Member.AddMember(testMember);
                string loyaltyid = testMember.GetLoyaltyID();

                string query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{loyaltyid}'";
                //Hashtable ht = Database.QuerySingleRow(query);
                //decimal vckey = Convert.ToDecimal(ht["VCKEY"]);
                //string query2 = $@"select * from bp_htz.lw_pointtransaction where vckey = '{vckey}'";

                DateTime checkInDt = new DateTime(2019, 10, 2);
                DateTime checkOutDt = new DateTime(2019, 10, 1);
                DateTime origBkDt = new DateTime(2019, 10, 1);
                int count = 1;
                BPTest.Pass<TestStep>($"Step 0 Passed: RG Member Created");

                foreach (decimal cdp in HTZ18527_NewlyNonEarningCDPs)
                {
                    BPTest.Start<TestStep>($"Step {count}: Test CDP {cdp}");
                    TxnHeader txnHeader = TxnHeader.Generate(testMember.GetLoyaltyID(), checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, null, "US", 100, "AAAA", 246095, "N", "US", null);
                    testMember.AddTransaction(txnHeader);
                    Member.UpdateMember(testMember);
                    testMember.RemoveTransaction();
                    BPTest.Pass<TestStep>($"Step {count++} Passed");
                }
                count = 0;
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        [Test]
        public void TestOneMember()
        {
            try
            {
                BPTest.Start<TestStep>($"Step 1: Create Member and Execute Transaction");
                Member testMember = Member.GenerateRandom(MemberStyle.ProjectOne).Set("N1", "MemberDetails.A_EARNINGPREFERENCE").Set("RG", "MemberDetails.A_TIERCODE");
                Member.AddMember(testMember);
                string loyaltyid = testMember.GetLoyaltyID();
                string alternateid = testMember.ALTERNATEID;
                decimal? cdp = 334505;
                decimal points = 150;
                decimal[] cdps = new decimal[] { 102495, 515, 11850, 135391, 281733, 40000 };

                DateTime checkInDt = new DateTime(2019, 11, 2);
                DateTime checkOutDt = new DateTime(2019, 11, 1);
                DateTime origBkDt = new DateTime(2019, 11, 1);
                BPTest.Pass<TestStep>($"Step 1 Passed");

                BPTest.Start<TestStep>($"Step 2: Add Reward");
                for(int x = 0; x < 3; x++)
                {
                    TxnHeader txnHeader = TxnHeader.Generate(testMember.GetLoyaltyID(), checkInDt, checkOutDt, origBkDt, cdp, HertzProgram.GoldPointsRewards, 0, "US", points, null, null, "N", "US", null);
                    testMember.AddTransaction(txnHeader);
                    Member.UpdateMember(testMember);
                    testMember.RemoveTransaction();
                    //points += 150;
                }
                

                //string query = $@"select vckey from bp_htz.lw_virtualcard where loyaltyidnumber = '{testMember.GetLoyaltyID()}'";
                //Hashtable ht = Database.QuerySingleRow(query);
                //decimal vckey = Convert.ToDecimal(ht["VCKEY"]);
                //string string_vckey = Convert.ToString(ht["VCKEY"]);

                //Member.AddReward(alternateid, "182346", string_vckey);
                BPTest.Pass<TestStep>($"Step 2 Passed");

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            
        }
    }
}
