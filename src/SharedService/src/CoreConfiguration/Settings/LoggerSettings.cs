namespace ConfigurationShare.Settings;

public class LoggerSettings
{
    public string Level { get; set; } = string.Empty;
    public bool WriteToConsole { get; set; } = true;
    public bool WriteToFile { get; set; } = false;
    public string FileRollingInterval { get; set; } = string.Empty;
    public string FileRollingSize { get; set; } = string.Empty;
}

public enum LogLevels
{
    //
    // Summary:
    //     Anything and everything you might want to know about a running block of code.
    Verbose,

    //
    // Summary:
    //     Internal system events that aren't necessarily observable from the outside.
    Debug,

    //
    // Summary:
    //     The lifeblood of operational intelligence - things happen.
    Information,

    //
    // Summary:
    //     Service is degraded or endangered.
    Warning,

    //
    // Summary:
    //     Functionality is unavailable, invariants are broken or data is lost.
    Error,

    //
    // Summary:
    //     If you have a pager, it goes off when one of these occurs.
    Fatal
}

public enum LogRollingInterval
{
    //
    // Summary:
    //     The log file will never roll; no time period information will be appended to
    //     the log file name.
    Infinite,

    //
    // Summary:
    //     Roll every year. Filenames will have a four-digit year appended in the pattern
    //     yyyy
    //     .
    Year,

    //
    // Summary:
    //     Roll every calendar month. Filenames will have
    //     yyyyMM
    //     appended.
    Month,

    //
    // Summary:
    //     Roll every day. Filenames will have
    //     yyyyMMdd
    //     appended.
    Day,

    //
    // Summary:
    //     Roll every hour. Filenames will have
    //     yyyyMMddHH
    //     appended.
    Hour,

    //
    // Summary:
    //     Roll every minute. Filenames will have
    //     yyyyMMddHHmm
    //     appended.
    Minute
}