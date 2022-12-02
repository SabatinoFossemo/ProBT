
namespace ProBT
{
    public abstract class Strategy
    {
        // CLASS VARIABLE  ................................................................................................................................................
        internal List<DateTime> Date {get; set;} = new List<DateTime>();
        internal List<double> Open {get; set;} = new List<double>();
        internal List<double> High {get; set;} = new List<double>();
        internal List<double> Low {get; set;} = new List<double>();
        internal List<double> Close {get; set;} = new List<double>();
        internal decimal BigPointValue {get; set;} 
        internal double TickSize {get; set;} 

        internal ListOrders Orders {get; set;} = new ListOrders();
        internal ListTrades Trades {get; set;} = new ListTrades();
        internal Position Position {get; set;} = new Position();

        // CONSTRUCTOR ................................................................................................................................................
        public Strategy(){
            this.Date = new List<DateTime>();
            this.Open = new List<double>();
            this.High = new List<double>();
            this.Low = new List<double>();
            this.Close = new List<double>();
            this.Orders = new ListOrders();
            this.Trades = new ListTrades();
            this.Position = new Position();
        }

        // PROPERTIES ................................................................................................................................................
        public List<DateTime> D {get => this.Date;}
        public List<double> O {get => this.Open;}
        public List<double> H {get => this.High;}
        public List<double> L {get => this.Low;}
        public List<double> C {get => this.Close;}
        public decimal BPV {get => this.BigPointValue;}
        public double TS {get => this.TickSize;}
        public MARKETPOSITION MP {get => this.MarketPosition;}
        public bool MP_LONG { get => MarketPosition == MARKETPOSITION.LONG; }
        public bool MP_SHORT { get => MarketPosition == MARKETPOSITION.SHORT; }
        public bool MP_FLAT { get => MarketPosition == MARKETPOSITION.FLAT; }
        public bool AtMarket { get => MarketPosition == MARKETPOSITION.LONG || MarketPosition == MARKETPOSITION.SHORT; }
        public string current_bar {get => string.Concat(D[0]," - ", O[0]," - ", H[0]," - ", L[0]," - ", C[0]);}

        public MARKETPOSITION MarketPosition{
            get {
                if(Position.Type == ORDER_TYPE.BUY)  
                    return MARKETPOSITION.LONG;
                else if(Position.Type == ORDER_TYPE.SELLSHORT)
                    return MARKETPOSITION.SHORT;
                else
                    return MARKETPOSITION.FLAT;
            }
        }

        // abstract methods for implementation of custom strategy ................................................................................................
        public abstract void Iniialize();
        public abstract void OnBarUpdate();
        public abstract void Deinitialize();

        // METHODS ................................................................................................................................................
        public Strategy Clean(){
            this.Date = new List<DateTime>();
            this.Open = new List<double>();
            this.High = new List<double>();
            this.Low = new List<double>();
            this.Close = new List<double>();
            ListOrders Orders = new ListOrders();
            ListTrades Trades = new ListTrades();
            Position Position = new Position();
            return this;
        }

        public void DeleteOrders(){
            this.Orders = Orders.Delete();
        }

        public void update_quote(List<DateTime> _date, List<double> _open, List<double> _high, List<double> _low, List<double> _close){
            this.Date = _date;
            this.Open = _open;
            this.High = _high;
            this.Low = _low;
            this.Close = _close;
        }

        public void send_attribute(decimal bpv, double ts){
            this.BigPointValue = bpv;
            this.TickSize = ts;
        }

        public void Buy(string price, string entry_name){
            if(price == "open") {
                IOrder order = new Order(ORDER_TYPE.BUY, ORDER_PRICE.OPEN, entry_name);
                this.Orders.Append(order);
            }

            else if(price == "close") {
                Order order = new Order(ORDER_TYPE.BUY, ORDER_PRICE.CLOSE, entry_name);
                this.Orders.Append(order);
            }
            else 
                throw new ArgumentException(String.Format("{0} is not an valid price", price), "price");        
        }
    
        public void Buy(double price, string entry_name){
            Order order = new Order(ORDER_TYPE.BUY, price, entry_name);
            this.Orders.Append(order);
        }

        public void Sell(string price, string entry_name){
            if(price == "open") {
                Order order = new Order(ORDER_TYPE.SELL, ORDER_PRICE.OPEN, entry_name);
                this.Orders.Append(order);
            }
            else if(price == "close") {
                Order order = new Order(ORDER_TYPE.SELL, ORDER_PRICE.CLOSE, entry_name);
                this.Orders.Append(order);
            }
        }

        public void Sell(double price, string entry_name){
            Order order = new Order(ORDER_TYPE.SELL, price, entry_name);
            this.Orders.Append(order);
        }

        public void SellShort(string price, string entry_name){
            if(price == "open") {
                Order order = new Order(ORDER_TYPE.SELLSHORT, ORDER_PRICE.OPEN, entry_name);
                this.Orders.Append(order);
            }
            else if(price == "close") {
                Order order = new Order(ORDER_TYPE.SELLSHORT, ORDER_PRICE.CLOSE, entry_name);
                this.Orders.Append(order);
            }
        }

        public void SellShort(double price, string entry_name){
            Order order = new Order(ORDER_TYPE.SELLSHORT, price, entry_name);
            this.Orders.Append(order);
        }
        
        public void BuyToCover(string price, string entry_name){
            if(price == "open") {
                Order order = new Order(ORDER_TYPE.BUYTOCOVER, ORDER_PRICE.OPEN, entry_name);
                this.Orders.Append(order);
            }
            else if(price == "close") {
                Order order = new Order(ORDER_TYPE.BUYTOCOVER, ORDER_PRICE.CLOSE, entry_name);
                this.Orders.Append(order);
            }
        }

        public void BuyToCover(double price, string entry_name){
            Order order = new Order(ORDER_TYPE.BUYTOCOVER, price, entry_name);
            this.Orders.Append(order);
        }

        public void SetStopLossDollar(decimal amount){
            foreach (var order in this.Orders)
                order.SetStopLoss(Convert.ToDouble(amount/BigPointValue).RoundTicks(TS));
        }

        public void SetTakeProfitDollar(decimal amount){
            foreach (var order in this.Orders)
                order.SetTakeProfit(Convert.ToDouble(amount/BigPointValue).RoundTicks(TS));
        }

        public void SetStopLossPoint(double point){
            foreach (var order in this.Orders)
                order.SetStopLoss(point.RoundTicks(TS));
        }

        public void SetTakeProfitPoint(double point){
            foreach (var order in this.Orders)
                order.SetTakeProfit(point.RoundTicks(TS));
        }


        public bool StopOrProfitIsHit(double high_value, double low_value, double price_stop, double price_profit){
            bool result = false;
            if(this.Position.StopLoss>0){
                if((this.Position.Type == ORDER_TYPE.BUY && low_value<=this.Position.StopLoss) || (this.Position.Type == ORDER_TYPE.SELLSHORT && high_value>=this.Position.StopLoss)){
                    result = true;
                    if(this.Position.Type == ORDER_TYPE.BUY)
                        this.Position = this.Position.Update(high_value, price_stop, price_stop, BPV);
                    if(this.Position.Type == ORDER_TYPE.SELLSHORT)
                        this.Position = this.Position.Update(price_stop, low_value, price_stop, BPV);
                    // # close this.Position
                    Trade trade = this.Position.Close(Date[0], price_stop, "SL");
                    this.Position = this.Position.Reset();
                    // # append trade to list
                    this.Trades.Append(trade);
                }
            }
            if(this.Position.TakeProfit>0){
                if((this.Position.Type == ORDER_TYPE.BUY && high_value>=this.Position.TakeProfit) || (this.Position.Type == ORDER_TYPE.SELLSHORT && low_value<=this.Position.TakeProfit)){
                    result = true;
                    if(this.Position.Type == ORDER_TYPE.BUY)
                        this.Position = this.Position.Update(price_profit, low_value, price_profit, BPV);
                    if(this.Position.Type == ORDER_TYPE.SELLSHORT)
                        this.Position = this.Position.Update(high_value, price_profit, price_profit, BPV);
                    // # close position
                    Trade trade = this.Position.Close(Date[0], price_profit, "TP");
                    this.Position = this.Position.Reset();
                    // # append trade to list
                    this.Trades.Append(trade);
                }
            }

            return result;
        }


        public void execute_orders(CHECK_AT _at){
            bool re_execute = false;
            // Console.WriteLine(_at);
            // # define price to update position already at market
            foreach (var ord in Orders){
                bool buy_on_buy = MP_LONG && (ord.Type == ORDER_TYPE.BUY || ord.Type == ORDER_TYPE.BUYTOCOVER);
                bool sell_on_sell = MP_SHORT && (ord.Type == ORDER_TYPE.SELL || ord.Type == ORDER_TYPE.SELLSHORT);
                bool nothing_to_close = MP_FLAT && (ord.Type == ORDER_TYPE.SELL || ord.Type == ORDER_TYPE.BUYTOCOVER);
                bool is_not_close = _at != CHECK_AT.CLOSE && ord.PriceS == ORDER_PRICE.CLOSE;
                bool is_not_open = _at != CHECK_AT.OPEN && ord.PriceS == ORDER_PRICE.OPEN;
                bool session_not_float = _at == CHECK_AT.SESSION && ord.PriceIsString;

                if(buy_on_buy || sell_on_sell || (is_not_close && is_not_open) || session_not_float)
                    continue;

                // # fill at open
                if( _at == CHECK_AT.OPEN){
                  // # define gap
                    bool gap = (Open[0] >= ord.PriceD && High[1] < ord.PriceD) || (Open[0] <= ord.PriceD && Low[1] > ord.PriceD);
                    gap = gap && ord.PriceD > 0;
                    // # if price is Open or there is an opening gap
                    if(ord.PriceS == ORDER_PRICE.OPEN || gap){   
                        // # if already at market check to close
                        if(AtMarket){
                            // # close position
                            Trade trade = this.Position.Close(Date[0], Open[0], "ORD");
                            this.Position = this.Position.Reset();
                
                            // # append trade to list
                            this.Trades.Append(trade);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                        // # check entry ord
                        if(ord.Type == ORDER_TYPE.BUY || ord.Type == ORDER_TYPE.SELLSHORT){
                            this.Position = new Position(Trades.NumTrades+1, ord, Date[0], Open[0]);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                    }
                }
                // # fill during next session
                else if( _at == CHECK_AT.SESSION && ord.PriceD >0){
                    // # if price is float
                    if(High[0] >= ord.PriceD  && ord.PriceD >= Low[0]){
                        // # if already at market check to close
                        if(AtMarket){
                            // # update before close
                            this.Position = this.Position.Update(High[0], Low[0], ord.PriceD, BPV);

                            // # close position
                            Trade trade = this.Position.Close(Date[0], ord.PriceD, "ORD");
                            this.Position = this.Position.Reset();

                            // # append trade to list
                            this.Trades.Append(trade);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                        // # check entry ord
                        if(ord.Type == ORDER_TYPE.BUY || ord.Type == ORDER_TYPE.SELLSHORT){
                            this.Position =  new Position(Trades.NumTrades+1, ord, Date[0], ord.PriceD);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                    }
                }
                // # fill at close
                else if( _at == CHECK_AT.CLOSE){
                    // # if price is close
                    if (ord.PriceS == ORDER_PRICE.CLOSE){
                        // # if already at market check to close
                        if(AtMarket){
                            //# update before close
                            this.Position = this.Position.Update(High[0], Low[0], Close[0], BPV);

                            // # close position
                            Trade trade = this.Position.Close(Date[0], Close[0], "ORD");
                            this.Position = this.Position.Reset();

                            // # append trade to list
                            this.Trades.Append(trade);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                        // # check entry ord
                        if(ord.Type == ORDER_TYPE.BUY || ord.Type == ORDER_TYPE.SELLSHORT){
                            this.Position = new Position(Trades.NumTrades+1, ord, Date[0], Close[0]);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                    }
                }
                
                if(re_execute) this.execute_orders(_at);
            }

            // Update Open Position Profit
            if (AtMarket){
                if(_at == CHECK_AT.OPEN){
                    if(!StopOrProfitIsHit(O[0],O[0],O[0],O[0]))
                        this.Position = this.Position.Update(O[0], O[0], O[0], BPV);
                }
                else if(_at == CHECK_AT.SESSION) {
                    if(!StopOrProfitIsHit(H[0],L[0],this.Position.StopLoss, this.Position.TakeProfit))   
                        this.Position = this.Position.Update(H[0], L[0], C[0], BPV);
                }  
                else if(_at == CHECK_AT.CLOSE) {
                    this.Position = this.Position.Update(C[0], C[0], C[0], BPV);
                }  
            }


            // Console.WriteLine("...................");
            // Console.WriteLine(_at);
            // Console.WriteLine(current_bar);
            // Console.WriteLine(Orders);
            // Console.WriteLine(Position);
            // if(Trades.NumTrades>0)Console.WriteLine(Trades.Trades[Trades.NumTrades-1]);

            // Console.ReadLine();                

   
        }

        public Point GetPoint(){
            int id = 0;
            ORDER_TYPE type = ORDER_TYPE.NULL;
            DateTime date = Date[0];
            decimal peak = 0;
            decimal valley = 0;
            decimal point = 0;
            decimal balance = Trades.Balance;

            if(MP_LONG){
                id = Position.ID;
                type = ORDER_TYPE.BUY;
                peak = (decimal)(High[0] - Position.EntryPrice);
                valley = (decimal)(Low[0] - Position.EntryPrice);
                point = Position.Profit;
            }
            if(MP_SHORT){
                id = Position.ID;
                type = ORDER_TYPE.SELLSHORT;
                peak = (decimal)(Position.EntryPrice - Low[0]);
                valley = (decimal)(Position.EntryPrice - High[0]);
                point = Position.Profit;
            }

            peak *= BPV;
            valley *= BPV;

            peak += balance;
            valley += balance;
            point += balance;
            
            Point result = new Point(id, type, date, peak,valley, point);
            
            return result;
        }
    }
}