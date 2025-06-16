using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum RequestStatus
{
    Pending = 1,
    Approved,
    Denied,
}

public static class RequestStatusExtensions
{
    public static string ParseStatus(this RequestStatus requestStatus)
        => requestStatus switch
        {
            RequestStatus.Pending => "Pending", 
            RequestStatus.Approved => "Approved",
            RequestStatus.Denied => "Denied",
            _ => throw new ValidationException($"Unknown request status '{requestStatus}'.")
        };
}