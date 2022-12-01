

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


        internal void Append(Order order)
        {
            this.orders.Add(order);
        }

        internal ListOrders Delete()
        {
            return new ListOrders();
        }

        internal void Remove(Order order)
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
}