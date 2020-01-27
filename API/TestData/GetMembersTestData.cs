using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brierley.TestAutomation.Core.Utilities;
using Hertz.API.Controllers;
using Hertz.API.DataModels;
using NUnit.Framework;

namespace Hertz.API.TestData
{
    public class GetMembersTestData
    {
        public static IEnumerable PositiveScenarios
        {
            get
            {
                yield return new TestCaseData();
            }
        }

        public static IEnumerable NegativeScenarios
        {
            get
            {
                string[] searchTypes = { "" };
                string[] searchValues = { "" };
                int? startIndex = null;
                int? batchSize = null;
                bool valideSearch = false;
                int errorCode = 3317;
                StringBuilder errorMessage = new StringBuilder("No search type provided for member search.");
                yield return new TestCaseData(searchTypes,searchValues,startIndex,batchSize,valideSearch,errorCode,errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",",searchTypes)}], SearchValue:[{String.Join(", ",searchValues)}]");


                searchTypes = new string[]{ "NOTREAL" };
                searchValues = new string[]{ "" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3375;
                errorMessage.Clear().Append("Invalid search type NOTREAL provided. Valid search values are: MemberID, CardID, EmailAddress, PhoneNumber, AlternateID, LastName, Username, PostalCode.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = new string[] { "AlternateID" };
                searchValues = new string[] { "" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3320;
                errorMessage.Clear().Append("No alternate id provided for member search.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = new string[] { "MemberID" };
                searchValues = new string[] { "" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3301;
                errorMessage.Clear().Append("No member id provided for member search.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = new string[] { "CardID" };
                searchValues = new string[] { "" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3304;
                errorMessage.Clear().Append("No card id provided for member search.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = new string[] { "EmailAddress" };
                searchValues = new string[] { "" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3318;
                errorMessage.Clear().Append("No email address provided for member search.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = new string[] { "PhoneNumber" };
                searchValues = new string[] { "" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3319;
                errorMessage.Clear().Append("No phone number provided for member search.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = new string[] { "LastName" };
                searchValues = new string[] { "" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3359;
                errorMessage.Clear().Append("Empty parameters provided for member search by name.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = new string[] { "Username" };
                searchValues = new string[] { "" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3321;
                errorMessage.Clear().Append("No username provided for member search.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = new string[] { "PostalCode" };
                searchValues = new string[] { "" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3322;
                errorMessage.Clear().Append("No postal code provided for member search.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = new string[] { "AlternateID" };
                searchValues = new string[] { "345678" };
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 3323;
                errorMessage.Clear().Append("No members found.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [{String.Join(",", searchTypes)}], SearchValue:[{String.Join(", ", searchValues)}]");


                searchTypes = null;
                searchValues = null;
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 2003;
                errorMessage.Clear().Append("MemberSearchType of GetMembersIn is a required property.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [null], SearchValue:[null]");


                searchTypes = null;
                searchValues = null;
                startIndex = null;
                batchSize = null;
                valideSearch = true;
                errorCode = 2003;
                errorMessage.Clear().Append("MemberSearchType of GetMembersIn is a required property.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [null], SearchValue:[Valid LID from DB]");


                searchTypes = new string[] { "" };
                searchValues = null;
                startIndex = null;
                batchSize = null;
                valideSearch = false;
                errorCode = 2003;
                errorMessage.Clear().Append("SearchValue of GetMembersIn is a required property.  Please provide a valid value.");
                yield return new TestCaseData(searchTypes, searchValues, startIndex, batchSize, valideSearch, errorCode, errorMessage.ToString()).
                    SetName($"GetMembers Negative - SearchType: [empty string], SearchValue:[null]");
            }
        }
    }
}
