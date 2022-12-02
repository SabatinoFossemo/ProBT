namespace ProBT
{
    public interface IQuote
    {
        List<DateTime> Date {get; }
        List<double> Open {get; }
        List<double> High {get; }
        List<double> Low {get; }
        List<double> Close {get; }

        string Symbol {get; }
        string Category {get; }
        string Sector {get; }
        double TickSize {get; }
        decimal BigPointValue {get; }
    }
}