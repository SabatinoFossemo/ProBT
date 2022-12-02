namespace ProBT
{
    public interface IOrder
    {
        ORDER_TYPE Type { get; }
        ORDER_PRICE PriceS { get; }
        double PriceD { get; }
        double StopLoss { get; }
        double TakeProfit { get; }
        string Name { get; }
        double SL { get; set; }
        double TP { get; set; }
        bool PriceIsString{ get; }

        void SetStopLoss(double point);
        void SetTakeProfit(double point);
    }
}