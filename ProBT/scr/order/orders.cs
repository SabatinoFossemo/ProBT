namespace ProBT
{
    public class ListOrders
    {
        public List<IOrder> Orders { get; }

        public ListOrders(){
            this.Orders = new List<IOrder>();
        }


        internal void Append(IOrder order){
            this.Orders.Add(order);
        }

        internal ListOrders Delete(){
            return new ListOrders();
        }

        internal void Remove(IOrder order){
            this.Orders.Remove(order);
        }

        public override string ToString(){
            string result = "";

            foreach (var item in this.Orders){
                result+=item.ToString() + "\n";
            }
            return result;
        }    


        public MyEnumerator GetEnumerator(){  
            return new MyEnumerator(this);  
        }  

        // Declare the enumerator class:  
        public class MyEnumerator{  
            int nIndex;  
            ListOrders collection;  
            public MyEnumerator(ListOrders coll){  
                collection = coll;  
                nIndex = -1;  
            }  
    
            public bool MoveNext(){  
                nIndex++;  
                return (nIndex < collection.Orders.Count());  
            }  
  
            public IOrder Current => collection.Orders[nIndex];
        }  
    }
}