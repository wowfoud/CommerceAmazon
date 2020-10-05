using Commerce.Amazon.Web.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commerce.Amazon.Web.Managers
{
    public class TestProcess : ITestProcess
    {
        private readonly IAccountManager _accountManager;
        private readonly IOperationManager _operationManager;

        public TestProcess(IAccountManager accountManager, IOperationManager operationManager)
        {
            _accountManager = accountManager;
            _operationManager = operationManager;
        }


    }
}
