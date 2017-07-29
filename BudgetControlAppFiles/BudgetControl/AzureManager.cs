using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace BudgetControl
{
    class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<BC001> BudgetControlTable;
        

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://budgetcontrol001.azurewebsites.net");
            this.BudgetControlTable = this.client.GetTable<BC001>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }
        public async Task<List<BC001>> GetPrevInformation()
        {
            return await this.BudgetControlTable.ToListAsync();
        }
        public async Task PostBudgetInformation(BC001 BCModel)
        {
            await this.BudgetControlTable.InsertAsync(BCModel);
        }
    }
}
