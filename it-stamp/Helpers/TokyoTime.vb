Public NotInheritable Class TokyoTime

    Shared Function Now() As DateTime
        Return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
    End Function

    Shared Function ToLocalTime(dt As DateTime) As DateTime
        Return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt.ToUniversalTime, "Tokyo Standard Time")
    End Function

End Class
