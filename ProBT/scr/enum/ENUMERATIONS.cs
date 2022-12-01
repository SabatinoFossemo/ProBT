namespace ProBT
{
    public enum ORDER_TYPE
    {
        NULL = -1,
        BUY = 0,
        SELL = 1,
        SELLSHORT = 2,
        BUYTOCOVER = 3,
        STOPLOSS = 4
    };

    public enum ORDER_PRICE
    {
        NULL=0,
        OPEN = 1,
        CLOSE = 2,
    };

    public enum CHECK_AT
    {
        OPEN = 0,
        SESSION = 1,
        CLOSE = 2,
    };
    public enum MARKETPOSITION
    {
        FLAT = 0,
        LONG = 1,
        SHORT = 2,
    };
}