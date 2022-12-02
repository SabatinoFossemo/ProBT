namespace ProBT
{
    public class Order: IOrder
    {

        public ORDER_TYPE Type { get; }
        public ORDER_PRICE PriceS { get; }
        public double PriceD { get; }
        public double StopLoss { get; }
        public double TakeProfit { get; }
        public string Name { get; }
        public double SL { get; set; }
        public double TP { get; set; }
        public bool PriceIsString { get => this.PriceS ==  ORDER_PRICE.OPEN || this.PriceS ==  ORDER_PRICE.CLOSE;}


        public Order(ORDER_TYPE order_type, ORDER_PRICE order_price, string name) {
            this.Type = order_type;
            this.PriceS = order_price;
            this.Name = name;
        }

        public Order(ORDER_TYPE order_type, double order_price, string name) {
            this.Type = order_type;
            this.PriceD = order_price;
            this.Name = name;
        }

        public void SetStopLoss(double point) {
            this.SL = point;
        }

        public void SetTakeProfit(double point) {
            this.TP = point;
        }


        public override string ToString() {
            string price = this.PriceD.ToString();
            if(PriceIsString)
                price = this.PriceS.ToString();
            
            string output = $"ORD: {this.Type.ToString()} - {this.Name} - {price} - {this.SL} - {this.TP}";

            return output;
        }


    }
}