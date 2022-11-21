

namespace ProBT
{
    public class PerformanceReport
    {
        ListTrades trades;
        Equity equity;
        StrategyPerformanceSummary strategy_performance_summary;
        // TradeAnalysis trade_analysis;
        // PeriodicalAnalysis periodical_analysis;

        public PerformanceReport(ListTrades trades, Equity equity)
        {
            this.trades = trades;
            this.equity = equity;
            this.strategy_performance_summary = new StrategyPerformanceSummary(trades, equity);
        }

        public StrategyPerformanceSummary Summary { get => this.strategy_performance_summary;}


    }

    public class StrategyPerformanceSummary
    {        
        public Dictionary<string, object> StratSum {get;}

        int num_trades;
        int num_trades_long;
        int num_trades_short;

        int num_profit_trades;
        int num_profit_trades_long;
        int num_profit_trades_short;

        int num_loss_trades;
        int num_loss_trades_long;
        int num_loss_trades_short;

        double gross_profit;
        double gross_profit_long;
        double gross_profit_short;

        double gross_loss;
        double gross_loss_long;
        double gross_loss_short;

        double net_profit;
        double net_profit_long;
        double net_profit_short;

        double avg_profit;
        double avg_profit_long;
        double avg_profit_short;

        double avg_loss;
        double avg_loss_long;
        double avg_loss_short;

        double avg_trade;
        double avg_trade_long;
        double avg_trade_short;

        double profit_factor;
        double profit_factor_long;
        double profit_factor_short;

        double percent_profitable;
        double percent_profitable_long;
        double percent_profitable_short;

        double risk_reward;
        double risk_reward_long;
        double risk_reward_short;

        double max_drawdown_close_to_close;
        double max_drawdown_close_to_close_long;
        double max_drawdown_close_to_close_short;

        double max_drawdown_open_equity;
        double max_drawdown_open_equity_long;
        double max_drawdown_open_equity_short;

        public StrategyPerformanceSummary(ListTrades trades, Equity equity)
        {
            StratSum = new Dictionary<string, object>();
            var trd = trades.Trades;

            num_trades = trd.Count();
            StratSum.Add("NumTrades", num_trades);
            num_trades_long = trd.Where(a => a.Type == ORDER_TYPE.BUY).Count();
            StratSum.Add("NumTradesLong", num_trades_long);
            num_trades_short = trd.Where(a => a.Type == ORDER_TYPE.SELLSHORT).Count();
            StratSum.Add("NumTradesShort", num_trades_short);

            num_profit_trades = trd.Where(a => a.Profit > 0).Count();
            StratSum.Add("NumProfitTrades", num_profit_trades);
            num_profit_trades_long = trd.Where(a => a.Profit > 0 && a.Type == ORDER_TYPE.BUY).Count();
            StratSum.Add("NumProfitTradesLong", num_profit_trades_long);
            num_profit_trades_short = trd.Where(a => a.Profit > 0 && a.Type == ORDER_TYPE.SELLSHORT).Count();
            StratSum.Add("NumProfitTradesShort", num_profit_trades_short);

            num_loss_trades = trd.Where(a => a.Profit <= 0).Count();
            StratSum.Add("NumLossTrades", num_loss_trades);
            num_loss_trades_long = trd.Where(a => a.Profit <= 0 && a.Type == ORDER_TYPE.BUY).Count();
            StratSum.Add("NumLossTradesLong", num_loss_trades_long);
            num_loss_trades_short = trd.Where(a => a.Profit <= 0 && a.Type == ORDER_TYPE.SELLSHORT).Count();
            StratSum.Add("NumLossTradesShort", num_loss_trades_short);

            gross_profit = trd.Where(a => a.Profit > 0).Sum(a => a.Profit);
            StratSum.Add("GrossProfit", Math.Round(gross_profit,2));
            gross_profit_long = trd.Where(a => a.Profit > 0 && a.Type == ORDER_TYPE.BUY).Sum(a => a.Profit);
            StratSum.Add("GrossProfitLong", Math.Round(gross_profit_long,2));
            gross_profit_short = trd.Where(a => a.Profit > 0 && a.Type == ORDER_TYPE.SELLSHORT).Sum(a => a.Profit);
            StratSum.Add("GrossProfitShort", Math.Round(gross_profit_short,2));

            gross_loss = trd.Where(a => a.Profit <= 0).Sum(a => a.Profit);
            StratSum.Add("GrossLoss", Math.Round(gross_loss,2));
            gross_loss_long = trd.Where(a => a.Profit <= 0 && a.Type == ORDER_TYPE.BUY).Sum(a => a.Profit);
            StratSum.Add("GrossLossLong", Math.Round(gross_loss_long,2));
            gross_loss_short = trd.Where(a => a.Profit <= 0 && a.Type == ORDER_TYPE.SELLSHORT).Sum(a => a.Profit);
            StratSum.Add("GrossLossShort", Math.Round(gross_loss_short,2));

            net_profit = gross_profit + gross_loss;
            StratSum.Add("NetProfit", Math.Round(net_profit,2));
            net_profit_long = gross_profit_long + gross_loss_long;
            StratSum.Add("NetProfitLong", Math.Round(net_profit_long,2));
            net_profit_short = gross_profit_short + gross_loss_short;
            StratSum.Add("NetProfitShort", Math.Round(net_profit_short,2));

            avg_profit = (num_profit_trades != 0) ? gross_profit / num_profit_trades : 0;
            StratSum.Add("AvgProfit", Math.Round(avg_profit,2));
            avg_profit_long = (num_profit_trades_long != 0) ? gross_profit_long / num_profit_trades_long : 0;
            StratSum.Add("AvgProfitLong", Math.Round(avg_profit_long,2));
            avg_profit_short = (num_profit_trades_short != 0) ? gross_profit_short / num_profit_trades_short : 0;
            StratSum.Add("AvgProfitShort", Math.Round(avg_profit_short,2));

            avg_loss = (num_loss_trades != 0) ? gross_loss / num_loss_trades : 0;
            StratSum.Add("AvgLoss", Math.Round(avg_loss,2));
            avg_loss_long = (num_loss_trades_long != 0) ? gross_loss_long / num_loss_trades_long : 0;
            StratSum.Add("AvgLossLong", Math.Round(avg_loss_long,2));
            avg_loss_short = (num_loss_trades_short != 0) ? gross_loss_short / num_loss_trades_short : 0;
            StratSum.Add("AvgLossShirt", Math.Round(avg_loss_short,2));

            avg_trade = (num_trades != 0) ? net_profit / num_trades : 0;
            StratSum.Add("AvgTrade", Math.Round(avg_trade,2));
            avg_trade_long = (num_trades_long != 0) ? net_profit_long / num_trades_long : 0;
            StratSum.Add("AvgTradeLong", Math.Round(avg_trade_long,2));
            avg_trade_short = (num_trades_short != 0) ? net_profit_short / num_trades_short : 0;
            StratSum.Add("AvgTradeShort", Math.Round(avg_trade_short,2));

            profit_factor = (gross_loss != 0) ? gross_profit / -gross_loss : 0;
            StratSum.Add("ProfitFactor", Math.Round(profit_factor,2));
            profit_factor_long = (gross_loss_long != 0) ? gross_profit_long / -gross_loss_long : 0;
            StratSum.Add("ProfitFactorLong", Math.Round(profit_factor_long,2));
            profit_factor_short = (gross_loss_short != 0) ? gross_profit_short / -gross_loss_short : 0;
            StratSum.Add("ProfitFactorShort", Math.Round(profit_factor_short,2));

            percent_profitable = (num_trades != 0) ? (double)num_profit_trades / (double)num_trades : 0;
            StratSum.Add("PercentProfitable", Math.Round(percent_profitable,2));
            percent_profitable_long = (num_trades_long != 0) ?  (double)num_profit_trades_long /  (double)num_trades_long : 0;
            StratSum.Add("PercentProfitableLong", Math.Round(percent_profitable_long,2));
            percent_profitable_short = (num_trades_short != 0) ?  (double)num_profit_trades_short /  (double)num_trades_short : 0;
            StratSum.Add("PercentProfitableShort", Math.Round(percent_profitable_short,2));

            risk_reward = (avg_loss != 0) ? avg_profit / -avg_loss : 0;
            StratSum.Add("RiskReward", Math.Round(risk_reward,2));
            risk_reward_long = (avg_loss_long != 0) ? avg_profit_long / -avg_loss_long : 0;
            StratSum.Add("RiskRewardLong", Math.Round(risk_reward_long,2));
            risk_reward_short = (avg_loss_short != 0) ? avg_profit_short / -avg_loss_short : 0;
            StratSum.Add("RiskRewardShort", Math.Round(risk_reward_short,2));

            max_drawdown_open_equity = (equity.DrawDown.Count() > 0) ? equity.DrawDown.Min() : 0; 
            StratSum.Add("MaxDrawDownOpenEquity", Math.Round(max_drawdown_open_equity,2));
            max_drawdown_open_equity_long = (equity.EquityLong.DrawDown.Count() > 0) ? equity.EquityLong.DrawDown.Min() : 0; 
            StratSum.Add("MaxDrawDownOpenEquityLong", Math.Round(max_drawdown_open_equity_long,2));
            max_drawdown_open_equity_short = (equity.EquityShort.DrawDown.Count() > 0) ? equity.EquityShort.DrawDown.Min() : 0; 
            StratSum.Add("MaxDrawDownOpenEquityShort", Math.Round(max_drawdown_open_equity_short,2));

            max_drawdown_close_to_close = (trades.DrawDown.Count() > 0) ? trades.DrawDown.Min() : 0;
            StratSum.Add("MaxDrawDownCloseToClose", Math.Round(max_drawdown_close_to_close,2));
            max_drawdown_close_to_close_long = (trades.TradesLong.DrawDown.Count() > 0) ? trades.TradesLong.DrawDown.Min() : 0;
            StratSum.Add("MaxDrawDownCloseToCloseLong", Math.Round(max_drawdown_close_to_close_long,2));
            max_drawdown_close_to_close_short =(trades.TradesShort.DrawDown.Count() > 0) ? trades.TradesShort.DrawDown.Min() : 0;
            StratSum.Add("MaxDrawDownCloseToCloseShort", Math.Round(max_drawdown_close_to_close_short,2));
        }

        public override string ToString()
        {
            string result = "";
            foreach (var kvp in this.StratSum) 
            {
                result += kvp.Key.PadRight(40,'.');
                string value = " " +  kvp.Value; 
                result += value.ToString().PadLeft(20,'.');
                result += "\n";
            }
            return result;
        }
    }

    public class TradeAnalysis
    {
        public TradeAnalysis()
        {

        }
    }

    public class PeriodicalAnalysis
    {
        public PeriodicalAnalysis()
        {

        }
    }

}