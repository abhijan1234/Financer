using System;
namespace Financer.DataAccess.Entities.Finance
{
    public class FinanceData
    {
        public int MonthlyIncome { get; set; }
        public int? FixedSavings { get; set; }
        public Expenditure? Expenditure { get; set; }
        public InvestmentData? Investment { get; set; }
    }
}
