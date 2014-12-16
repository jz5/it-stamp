Public NotInheritable Class TokyoTime

    Shared Function Now() As DateTime
        Return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time")
    End Function

End Class
