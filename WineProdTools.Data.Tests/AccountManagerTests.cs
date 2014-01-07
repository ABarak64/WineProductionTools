using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WineProdTools.Data.Tests.Mocks;
using WineProdTools.Data.Managers;

namespace WineProdTools.Data.Tests
{
    [TestClass]
    public class AccountManagerTests
    {
        [TestMethod]
        public void EnsureThatAccountsAreAddedToTheSetWhenTheyAreCreated()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new AccountManager(() => { return context; });
            var username = "My Username";
            var user = new UserProfile { UserName = username, AccountId = 99 };
            context.UserProfiles.Add(user);

            mgr.Create(username);
            var acct = context.Accounts.SingleOrDefault(a => a.Id == 0);
            Assert.AreEqual(true, acct != null);
            Assert.AreEqual(true, context.SaveChangesCalled);
        }
    }
}
