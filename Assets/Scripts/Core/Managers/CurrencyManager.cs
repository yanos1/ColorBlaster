namespace Core.Managers
{
    public class CurrencyManager
    {
        private int currency;

        public CurrencyManager()
        {
            LoadCurrency();
        }

        private void LoadCurrency()
        {
            CoreManager.instance.SaveManager.Load<CurrencySaver>(savedData =>
            {
                if (savedData != null)
                {
                    currency = savedData.SavedCurrency;
                }
                else
                {
                    currency = 0; // Default starting currency
                }
            });
        }

        public void AddCurrency(int amount)
        {
            currency += amount;
            SaveCurrency();
        }

        public void SaveCurrency()
        {
            var currencySaver = new CurrencySaver { SavedCurrency = currency };
            CoreManager.instance.SaveManager.Save(currencySaver);
        }

        public int GetCurrency()
        {
            return currency;
        }
    }

    public class CurrencySaver : ISaveData
    {
        public int SavedCurrency { get; set; }
    }

}