using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WineProdTools.Data.Tests.Mocks;
using WineProdTools.Data.Managers;
using WineProdTools.Data.EntityModels;
using WineProdTools.Data.DtoModels;

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

        [TestMethod]
        public void EnsureThatANewlyCreatedAccountIsSetToActive()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new AccountManager(() => { return context; });
            var username = "My Username";
            var user = new UserProfile { UserName = username, AccountId = 99 };
            context.UserProfiles.Add(user);

            mgr.Create(username);
            var acct = context.Accounts.SingleOrDefault(a => a.Id == 0);
            Assert.AreEqual(true, acct.Active);
        }

        [TestMethod]
        public void EnsureThatTheUserProfileAssociatedWithTheNewAccountHasItsAccountIdUpdated()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new AccountManager(() => { return context; });
            var username = "My Username";
            var user = new UserProfile { UserName = username, AccountId = 99 };
            context.UserProfiles.Add(user);

            mgr.Create(username);
            Assert.AreEqual(0, user.AccountId);
        }

        [TestMethod]
        public void EnsureThatUpdatingAnAccountChangesItsName()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new AccountManager(() => { return context; });
            var acct = new Account { Id = 0, Name = "somejunk" };
            context.Accounts.Add(acct);

            var newName = "mynewname";
            mgr.UpdateAccount(new AccountDto { Id = 0, Name = newName });
            Assert.AreEqual(newName, acct.Name);
            Assert.AreEqual(true, context.SaveChangesCalled);
        }
    }
}
