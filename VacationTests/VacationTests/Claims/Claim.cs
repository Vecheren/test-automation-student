using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VacationTests.Data;

namespace VacationTests.Claims
{
    // Об enum https://ulearn.me/course/basicprogramming/Konstanty_i_enum_y_f1740706-b8e2-4bd4-ab87-3cc710a52449

    public record Claim(
        string Id,
        [property: JsonConverter(typeof(StringEnumConverter))]
        ClaimType Type,
        ClaimStatus Status,
        Director Director,
        DateTime StartDate,
        DateTime EndDate,
        int? ChildAgeInMonths,
        string UserId,
        bool PaidNow)
    {
        public static Claim CreateDefault()
        {
            var random = new Random();
            var randomClaimId = random.Next(1, 101).ToString();

            return new Claim(
                randomClaimId,
                ClaimType.Paid,
                ClaimStatus.NonHandled,
                Directors.Default,
                DateTime.Now.Date.AddDays(7),
                DateTime.Now.Date.AddDays(12),
                null,
                "1",
                false
            );
        }

        public static Claim CreateChildType()
        {
            var random = new Random();
            var childAgeInMonths = random.Next(1, 101);
            return CreateDefault() with
            {
                Type = ClaimType.Child,
                ChildAgeInMonths = childAgeInMonths
            };
        }
    }
}