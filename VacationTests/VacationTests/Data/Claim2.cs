using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VacationTests.Claims;

public class Claim2
{
    // Конструктор класса
    public Claim2(string id, ClaimType type, ClaimStatus status, Director director, DateTime startDate,
        DateTime endDate, int? childAgeInMonths, string userId, bool paidNow)
    {
        Id = id;
        Type = type;
        Status = status;
        Director = director;
        StartDate = startDate;
        EndDate = endDate;
        ChildAgeInMonths = childAgeInMonths;
        UserId = userId;
        PaidNow = paidNow;
    }
    
    // Свойства класса
    public string Id { get; }
    
    [property: JsonConverter(typeof(StringEnumConverter))]
    public ClaimType Type { get; }
    
    public ClaimStatus Status { get; }
    
    public Director Director { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public int? ChildAgeInMonths { get; }
    public string UserId { get; }
    public bool PaidNow { get; }
}