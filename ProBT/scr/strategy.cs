
namespace ProBT
{
    public abstract class Strategy
    {
        double bpv;
        double ts;
        List<DateTime> date;
        List<double> open;
        List<double> high;
        List<double> low;
        List<double> close;

        ListOrders orders;
        ListTrades trades;
        Position position;

        public Strategy()
        {
            this.position = new Position();
            this.orders = new ListOrders();
            this.trades = new ListTrades();
            this.date = new List<DateTime>();
            this.open = new List<double>();
            this.high = new List<double>();
            this.low = new List<double>();
            this.close = new List<double>();
        }

        public double BPV {get => this.bpv;}
        public double BigPointValue {get => this.bpv;}
        public double TS {get => this.ts;}
        public double TickSize {get => this.ts;}
        public ListOrders Orders {get => this.orders;}
        public ListTrades Trades {get => this.trades;}
        public Position Position {get => this.position;}

        // abstract methods for implementation of custom strategy
        public abstract void Iniialize();
        public abstract void OnBarUpdate();
        public abstract void Deinitialize();

        // Properties

        public List<DateTime> D {get => this.date;}
        public List<DateTime> Date {get => this.date;}
        public List<double> O {get => this.open;}
        public List<double> Open {get => this.open;}
        public List<double> H {get => this.high;}
        public List<double> High {get => this.high;}
        public List<double> L {get => this.low;}
        public List<double> Low {get => this.low;}
        public List<double> C {get => this.close;}
        public List<double> Close {get => this.close;}
        public string current_bar {get => string.Concat(D[0]," - ", O[0]," - ", H[0]," - ", L[0]," - ", C[0]);}

        public MARKETPOSITION MarketPosition
        {
            get 
            {
                if(Position.Type == ORDER_TYPE.BUY)  
                    return MARKETPOSITION.LONG;
                else if(Position.Type == ORDER_TYPE.SELLSHORT)
                    return MARKETPOSITION.SHORT;
                else
                    return MARKETPOSITION.FLAT;
            }
        }

        public MARKETPOSITION MP {get => this.MarketPosition;}

        public bool MP_LONG
        {
            get{if(MarketPosition == MARKETPOSITION.LONG) return true;
            else return false;}
        }
        public bool MP_SHORT
        {
            get{if(MarketPosition == MARKETPOSITION.SHORT) return true;
            else return false;}
        }

        public bool MP_FLAT
        {
            get{if(MarketPosition == MARKETPOSITION.FLAT) return true;
            else return false;}
        }

        public bool AtMarket
        {
            get{if(MarketPosition == MARKETPOSITION.LONG || MarketPosition == MARKETPOSITION.SHORT) return true;
            else return false;}
        }


        public void DeleteOrders()
        {
            this.orders = orders.Delete();
        }

        public void update_quote(List<DateTime> _date, List<double> _open, List<double> _high, List<double> _low, List<double> _close)
        {
            this.date = _date;
            this.open = _open;
            this.high = _high;
            this.low = _low;
            this.close = _close;
        }

        public void send_attribute(double bpv, double ts)
        {
            this.bpv = bpv;
            this.ts = ts;
        }

        public void Buy(string price, string entry_name)
        {
            if(price == "open") 
            {
                Order order = new Order(ORDER_TYPE.BUY, ORDER_PRICE.OPEN, entry_name);
                this.orders.Append(order);
            }

            else if(price == "close") 
            {
                Order order = new Order(ORDER_TYPE.BUY, ORDER_PRICE.CLOSE, entry_name);
                this.orders.Append(order);
            }
        }
        public void Buy(double price, string entry_name)
        {
            Order order = new Order(ORDER_TYPE.BUY, price, entry_name);
            this.orders.Append(order);
        }

        public void Sell(string price, string entry_name)
        {
            if(price == "open") 
            {
                Order order = new Order(ORDER_TYPE.SELL, ORDER_PRICE.OPEN, entry_name);
                this.orders.Append(order);
            }

            else if(price == "close") 
            {
                Order order = new Order(ORDER_TYPE.SELL, ORDER_PRICE.CLOSE, entry_name);
                this.orders.Append(order);
            }
        }
        public void Sell(double price, string entry_name)
        {
            Order order = new Order(ORDER_TYPE.SELL, price, entry_name);
            this.orders.Append(order);
        }

        public void SellShort(string price, string entry_name)
        {
            if(price == "open") 
            {
                Order order = new Order(ORDER_TYPE.SELLSHORT, ORDER_PRICE.OPEN, entry_name);
                this.orders.Append(order);
            }

            else if(price == "close") 
            {
                Order order = new Order(ORDER_TYPE.SELLSHORT, ORDER_PRICE.CLOSE, entry_name);
                this.orders.Append(order);
            }
        }
        public void SellShort(double price, string entry_name)
        {
            Order order = new Order(ORDER_TYPE.SELLSHORT, price, entry_name);
            this.orders.Append(order);
        }
        
        public void BuyToCover(string price, string entry_name)
        {
            if(price == "open") 
            {
                Order order = new Order(ORDER_TYPE.BUYTOCOVER, ORDER_PRICE.OPEN, entry_name);
                this.orders.Append(order);
            }

            else if(price == "close") 
            {
                Order order = new Order(ORDER_TYPE.BUYTOCOVER, ORDER_PRICE.CLOSE, entry_name);
                this.orders.Append(order);
            }
        }
        public void BuyToCover(double price, string entry_name)
        {
            Order order = new Order(ORDER_TYPE.BUYTOCOVER, price, entry_name);
            this.orders.Append(order);
        }

        public void SetStopLossDollar(double amount, string custom_name="")
        {
            foreach (var order in orders)
                order.SetStopLoss(amount/BigPointValue);
        }

        public void SetTakeProfitDollar(double amount, string custom_name="")
        {
            foreach (var order in orders)
                order.SetTakeProfit(amount/BigPointValue);
        }

        public void SetStopLossPoint(double point, string custom_name="")
        {
            foreach (var order in orders)
                order.SetStopLoss(point);
        }

        public void SetTakeProfitPoint(double point, string custom_name="")
        {
            foreach (var order in orders)
                order.SetTakeProfit(point);
        }


        public bool StopOrProfitIsHit(double high_value, double low_value, double price_stop, double price_profit)
        {
            bool result = false;
            if(position.StopLoss>0){
                if((position.Type == ORDER_TYPE.BUY && low_value<=position.StopLoss) || (position.Type == ORDER_TYPE.SELLSHORT && high_value>=position.StopLoss)){
                    result = true;
                    if(position.Type == ORDER_TYPE.BUY)
                        position = position.Update(high_value, price_stop, price_stop, BPV);
                    if(position.Type == ORDER_TYPE.SELLSHORT)
                        position = position.Update(price_stop, low_value, price_stop, BPV);
                    // # close position
                    Trade trade = position.Close(Date[0], price_stop, "SL");
                    position = position.Reset();
                    // # append trade to list
                    trades.Append(trade);
                }
            }

            if(position.TakeProfit>0){
                if((position.Type == ORDER_TYPE.BUY && high_value>=position.TakeProfit) || (position.Type == ORDER_TYPE.SELLSHORT && low_value<=position.TakeProfit)){
                    result = true;
                    if(position.Type == ORDER_TYPE.BUY)
                        position = position.Update(price_profit, low_value, price_profit, BPV);
                    if(position.Type == ORDER_TYPE.SELLSHORT)
                        position = position.Update(high_value, price_profit, price_profit, BPV);

                    // # close position
                    Trade trade = position.Close(Date[0], price_profit, "TP");
                    position = position.Reset();
                    // # append trade to list
                    trades.Append(trade);
                }
            }

            return result;
        }


        public void execute_orders(CHECK_AT _at)
        {


            bool re_execute = false;
            // Console.WriteLine(_at);
            // # define price to update position already at market
            foreach (var ord in Orders)
            {
                bool buy_on_buy = MP_LONG && (ord.Type == ORDER_TYPE.BUY || ord.Type == ORDER_TYPE.BUYTOCOVER);
                bool sell_on_sell = MP_SHORT && (ord.Type == ORDER_TYPE.SELL || ord.Type == ORDER_TYPE.SELLSHORT);
                bool nothing_to_close = MP_FLAT && (ord.Type == ORDER_TYPE.SELL || ord.Type == ORDER_TYPE.BUYTOCOVER);
                bool is_not_close = _at != CHECK_AT.CLOSE && ord.PriceS == ORDER_PRICE.CLOSE;
                bool is_not_open = _at != CHECK_AT.OPEN && ord.PriceS == ORDER_PRICE.OPEN;
                bool session_not_float = _at == CHECK_AT.SESSION && ord.PriceIsString;

                if(buy_on_buy || sell_on_sell || (is_not_close && is_not_open) || session_not_float)
                    continue;

                // # fill at open
                if( _at == CHECK_AT.OPEN)
                {
                  // # define gap
                    bool gap = (open[0] >= ord.PriceD && high[1] < ord.PriceD) || (open[0] <= ord.PriceD && low[1] > ord.PriceD);
                    gap = gap && ord.PriceD > 0;
                    // # if price is open or there is an opening gap
                    if(ord.PriceS == ORDER_PRICE.OPEN || gap)
                    {   
                        // # if already at market check to close
                        if(AtMarket)
                        {
                            // # close position
                            Trade trade = position.Close(Date[0], Open[0], "ORD");
                            position = position.Reset();
                
                            // # append trade to list
                            trades.Append(trade);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                        // # check entry ord
                        if(ord.Type == ORDER_TYPE.BUY || ord.Type == ORDER_TYPE.SELLSHORT)
                        {
                            position = new Position(Trades.NumTrades+1, ord, Date[0], Open[0]);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                    }
                }
                // # fill during next session
                else if( _at == CHECK_AT.SESSION && ord.PriceD >0)
                {
                    // # if price is float
                    if(High[0] >= ord.PriceD  && ord.PriceD >= Low[0])
                    {
                        // # if already at market check to close
                        if(AtMarket)
                        {
                            // # update before close
                            position = position.Update(High[0], Low[0], ord.PriceD, BPV);

                            // # close position
                            Trade trade = position.Close(Date[0], ord.PriceD, "ORD");
                            position = position.Reset();

                            // # append trade to list
                            trades.Append(trade);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                        // # check entry ord
                        if(ord.Type == ORDER_TYPE.BUY || ord.Type == ORDER_TYPE.SELLSHORT)
                        {
                            position =  new Position(Trades.NumTrades+1, ord, Date[0], ord.PriceD);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                    }
                }
                // # fill at close
                else if( _at == CHECK_AT.CLOSE)
                {
                    // # if price is close
                    if (ord.PriceS == ORDER_PRICE.CLOSE)
                    {
                        // # if already at market check to close
                        if(AtMarket)
                        {
                            // # update before close
                            // position = position.Update(High[0], Low[0], Close[0], BPV);

                            // # close position
                            Trade trade = position.Close(Date[0], Close[0], "ORD");
                            position = position.Reset();

                            // # append trade to list
                            trades.Append(trade);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                        // # check entry ord
                        if(ord.Type == ORDER_TYPE.BUY || ord.Type == ORDER_TYPE.SELLSHORT)
                        {
                            position = new Position(Trades.NumTrades+1, ord, Date[0], Close[0]);

                            //remove order for recalculation
                            this.Orders.Remove(ord);
                            re_execute = true;
                        }
                    }
                }
                
                if(re_execute) this.execute_orders(_at);
            }

            // Update Open Position Profit
            if (AtMarket)
            {
                if(_at == CHECK_AT.OPEN){
                    if(!StopOrProfitIsHit(O[0],O[0],O[0],O[0]))
                        position = position.Update(O[0], O[0], O[0], BPV);
                }
                else if(_at == CHECK_AT.SESSION) {
                    if(!StopOrProfitIsHit(H[0],L[0],position.StopLoss, position.TakeProfit))   
                        position = position.Update(H[0], L[0], C[0], BPV);
                }  
                else if(_at == CHECK_AT.CLOSE) {
                    position = position.Update(C[0], C[0], C[0], BPV);
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

        public Point GetPoint()
        {
            int id = 0;
            ORDER_TYPE type = ORDER_TYPE.NULL;
            DateTime date = Date[0];
            double peak = 0;
            double valley = 0;
            double point = 0;
            double balance = Trades.Balance;

            if(MP_LONG)
            {
                id = Position.ID;
                type = ORDER_TYPE.BUY;
                peak = High[0] - Position.EntryPrice;
                valley = Low[0] - Position.EntryPrice;
                point = Position.Profit;
            }
            if(MP_SHORT)
            {
                id = Position.ID;
                type = ORDER_TYPE.SELLSHORT;
                peak = Position.EntryPrice - Low[0];
                valley = Position.EntryPrice - High[0];
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