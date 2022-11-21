

namespace ProBT
{
    public class ListOrders
    {
        List<Order> orders;

        public ListOrders()
        {
            this.orders = new List<Order>();
        }

        public List<Order> Orders {get => this.orders;}


        public MyEnumerator GetEnumerator()
        {  
            return new MyEnumerator(this);  
        }  

        // Declare the enumerator class:  
        public class MyEnumerator
        {  
            int nIndex;  
            ListOrders collection;  
            public MyEnumerator(ListOrders coll)
            {  
                collection = coll;  
                nIndex = -1;  
            }  
    
            public bool MoveNext()
            {  
                nIndex++;  
                return (nIndex < collection.orders.Count());  
            }  
  
            public Order Current => collection.orders[nIndex];
        }  


        public void Append(Order order)
        {
            this.orders.Add(order);
        }

        public ListOrders Delete()
        {
            return new ListOrders();
        }

        public void Remove(Order order)
        {
            this.orders.Remove(order);
        }

        public override string ToString()
        {
            string result = "";

            foreach (var item in orders)
            {
                result+=item.ToString() + "\n";
            }
            return result;
        }    }


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

        public ORDER_TYPE Type {get => this.type;}
        public ORDER_PRICE PriceS {get => this.price_s;}
        public double PriceD {get => this.price_d;}
        public double StopLoss {get => this.sl;}
        public double TakeProfit {get => this.tp;}
        public string Name {get => this.name;}
        
        public void SetStopLoss(double point)
        {
            this.sl = point;
        }
        public void SetTakeProfit(double point)
        {
            this.tp = point;
        }
        

        public override string ToString()
        {
            return "ORD: " + type.ToString() + " - " + 
                             name + " - " + 
                             price_s + " - " + 
                             price_d + " - " + 
                             sl + " - " + 
                             tp;
        }

        public bool PriceIsString
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