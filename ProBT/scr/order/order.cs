namespace ProBT
{
    public class Order
    {
        ORDER_TYPE type;
        ORDER_PRICE price_s = ORDER_PRICE.NULL;
        double price_d = 0;
        double sl = 0;
        double tp = 0;
        string name;


        public Order(ORDER_TYPE order_type, ORDER_PRICE order_price, string name)
        {

            this.type = order_type;
            this.price_s = order_price;
            this.name = name;

        }

        public Order(ORDER_TYPE order_type, double order_price, string name)
        {
            this.type = order_type;
            this.price_d = order_price;
            this.name = name;
        }

        internal ORDER_TYPE Type {get => this.type;}
        internal ORDER_PRICE PriceS {get => this.price_s;}
        internal double PriceD {get => this.price_d;}
        internal double StopLoss {get => this.sl;}
        internal double TakeProfit {get => this.tp;}
        internal string Name {get => this.name;}
        
        internal void SetStopLoss(double point)
        {
            this.sl = point;
        }
        internal void SetTakeProfit(double point)
        {
            this.tp = point;
        }
        

        public override string ToString()
        {
            if(PriceIsString)
                return $"ORD: {type.ToString()} - {name} - {price_s} - {sl} - {tp}";
            else
                return $"ORD: {type.ToString()} - {name} - {price_d} - {sl} - {tp}";
        }

        internal bool PriceIsString
        {
            get
            {
            if (price_s ==  ORDER_PRICE.OPEN || price_s ==  ORDER_PRICE.CLOSE)
                return true;
            else
                return false;
            }
        } 

        private bool TypeIsCorrect(ORDER_TYPE order_type)
        {
            if (order_type == ORDER_TYPE.BUY || order_type == ORDER_TYPE.SELL || order_type == ORDER_TYPE.SELLSHORT || order_type == ORDER_TYPE.BUYTOCOVER)
                return true;
            else
                return false;
        }
    }

}