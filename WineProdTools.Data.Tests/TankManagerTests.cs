using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WineProdTools.Data.Tests.Mocks;
using WineProdTools.Data.Managers;
using WineProdTools.Data.EntityModels;
using WineProdTools.Data.DtoModels;
using System.Security.Authentication;

namespace WineProdTools.Data.Tests
{
    [TestClass]
    public class TankManagerTests
    {
        [TestMethod]
        public void EnsureThatOnlyTanksForTheSpecifiedAccountAreRetrieved()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var badAccountId = 6;
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 1 });
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 2 });
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 3 });
            context.Tanks.Add(new Tank { AccountId = badAccountId, Id = 4 });

            var actual = mgr.GetTanksForAccount(accountId);
            Assert.AreEqual(false, actual.Any(t => t.Id == badAccountId));
        }

        [TestMethod]
        public void EnsureThatAllUndeletedTanksForTheSpecifiedAccountAreRetrieved()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 1, DateDeleted = null });
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 2, DateDeleted = null });
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 3, DateDeleted = null });

            var actual = mgr.GetTanksForAccount(accountId);
            Assert.AreEqual(3, actual.Count());
        }

        [TestMethod]
        public void EnsureThatNoDeletedTanksForTheSpecifiedAccountAreRetrieved()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 1, DateDeleted = null });
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 2, DateDeleted = null });
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 3, DateDeleted = DateTime.Now });

            var actual = mgr.GetTanksForAccount(accountId);
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(false, actual.Any(t => t.Id == 3));
        }

        [TestMethod]
        public void EnsureThatOnlyATankForTheSpecifiedAccountIsRetrieved()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var badAccountId = 6;
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 1 });
            context.Tanks.Add(new Tank { AccountId = badAccountId, Id = 4 });

            var actual = mgr.GetTankForAccount(4, accountId);
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void EnsureThatADeletedTankIsNotRetrieved()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 1, DateDeleted = DateTime.Now });

            var actual = mgr.GetTankForAccount(1, accountId);
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void EnsureThatTheCorrectTankIsRetrieved()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 1, DateDeleted = null });

            var actual = mgr.GetTankForAccount(1, accountId);
            Assert.AreEqual(1, actual.Id);
        }

        [TestMethod]
        public void EnsureThatATankBeingPartiallyFilledWillNotOverflow()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank { AccountId = accountId, Id = 1, DateDeleted = null, Gallons = 1000,
                Contents = new TankContents{ Gallons = 500 }
            });

            var actual = mgr.TankWillOverflow(100, 1);
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void EnsureThatATankBeingCompletelyFilledWillNotOverflow()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                DateDeleted = null,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500.5M }
            });

            var actual = mgr.TankWillOverflow(499.5M, 1);
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void EnsureThatATankBeingOverFilledWillOverflow()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                DateDeleted = null,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500.5M }
            });

            var actual = mgr.TankWillOverflow(600, 1);
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void EnsureThatATankCanRemoveLessThanItsTotalContentInGallons()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                DateDeleted = null,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500 }
            });

            var actual = mgr.TankContainsAtLeastThisManyGallons(100, 1);
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void EnsureThatATankCanRemoveItsTotalContentInGallons()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                DateDeleted = null,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500 }
            });

            var actual = mgr.TankContainsAtLeastThisManyGallons(500, 1);
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void EnsureThatATankCannotRemoveMoreThanItsTotalContentInGallons()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                DateDeleted = null,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500 }
            });

            var actual = mgr.TankContainsAtLeastThisManyGallons(600, 1);
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void EnsureThatATankWithNoContentsCannotRemoveAnything()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                DateDeleted = null,
                Gallons = 1000,
                Contents = null
            });

            var actual = mgr.TankContainsAtLeastThisManyGallons(500, 1);
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void EnsureThatAddingATankAddsItInTheSet()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var tankDto = new TankDto { Gallons = 100, Name = "Blah" };

            var actual = mgr.AddTankForAccount(tankDto, accountId);
            Assert.AreEqual(true, context.SaveChangesCalled);
            Assert.AreEqual(1, context.Tanks.Count());
        }

        [TestMethod]
        public void EnsureThatAddingATankSavesTheCorrectAccountId()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var tankDto = new TankDto { Gallons = 100, Name = "Blah" };

            var actual = mgr.AddTankForAccount(tankDto, accountId);
            Assert.AreEqual(accountId, context.Tanks.Single(t => t.Id == 0).AccountId);
        }

        [TestMethod]
        public void EnsureThatAddingATankReturnsTheNewlyAddedTanksId()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var tankDto = new TankDto { Gallons = 100, Name = "Blah" };

            var actual = mgr.AddTankForAccount(tankDto, accountId);
            Assert.AreEqual(actual, context.Tanks.First().Id);
        }

        [TestMethod]
        public void EnsureThatAddingATankDefaultsToUndeleted()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var tankDto = new TankDto { Gallons = 100, Name = "Blah" };

            var actual = mgr.AddTankForAccount(tankDto, accountId);
            Assert.AreEqual(null, context.Tanks.First().DateDeleted);
        }

        [TestMethod]
        public void EnsureThatUpdatingATankSetsAllRelevantProperties()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var tankDto = new TankDto { Id = 1, Gallons = 100, Name = "Blah", XPosition = 100, YPosition = 100 };
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                Name = "Meh",
                XPosition = 333,
                YPosition = 333,
                DateDeleted = null,
                Gallons = 1000,
                Contents = null
            });

            mgr.UpdateTankForAccount(tankDto, accountId);
            Assert.AreEqual(true, context.SaveChangesCalled);
            Assert.AreEqual(tankDto.Name, context.Tanks.First().Name);
            Assert.AreEqual(tankDto.Gallons, context.Tanks.First().Gallons);
            Assert.AreEqual(tankDto.XPosition, context.Tanks.First().XPosition);
            Assert.AreEqual(tankDto.YPosition, context.Tanks.First().YPosition);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void EnsureThatUpdatingATankThatDoesntBelongToTheAccountThrowsAnException()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var tankDto = new TankDto { Id = 1, Gallons = 100, Name = "Blah", XPosition = 100, YPosition = 100 };
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                Name = "Meh",
                XPosition = 333,
                YPosition = 333,
                DateDeleted = null,
                Gallons = 1000,
                Contents = null
            });

            mgr.UpdateTankForAccount(tankDto, 6);
        }

        [TestMethod]
        public void EnsureThatUpdatingATanksContentsSetsAllRelevantProperties()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var contentDto = new TankContentsDto
            {
                TankId = 1,
                Alcohol = 2,
                MA = 3,
                Name = "Meh",
                Ph = 4,
                RS = 5,
                So2 = 6,
                TA = 7,
                VA = 8,
                State = new TankContentsState { Id = 9 },
            };
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                Name = "Meh",
                XPosition = 333,
                YPosition = 333,
                DateDeleted = null,
                Gallons = 1000,
                Contents = new TankContents { Alcohol = 1, MA = 1, Name = "Blah", Ph = 1, RS = 1, So2 = 1, TA = 1, VA = 1, TankContentsStateId = 1 }
            });

            mgr.UpdateTankContentsForAccount(contentDto, accountId);
            Assert.AreEqual(true, context.SaveChangesCalled);
            Assert.AreEqual(contentDto.Alcohol, context.Tanks.First().Contents.Alcohol);
            Assert.AreEqual(contentDto.MA, context.Tanks.First().Contents.MA);
            Assert.AreEqual(contentDto.Name, context.Tanks.First().Contents.Name);
            Assert.AreEqual(contentDto.Ph, context.Tanks.First().Contents.Ph);
            Assert.AreEqual(contentDto.RS, context.Tanks.First().Contents.RS);
            Assert.AreEqual(contentDto.So2, context.Tanks.First().Contents.So2);
            Assert.AreEqual(contentDto.VA, context.Tanks.First().Contents.VA);
            Assert.AreEqual(contentDto.State.Id, context.Tanks.First().Contents.TankContentsStateId);
            Assert.AreEqual(contentDto.TA, context.Tanks.First().Contents.TA);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void EnsureThatUpdatingATanksContentsThatDoesntBelongToTheAccountThrowsAnException()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var contentDto = new TankContentsDto
            {
                TankId = 1,
                Alcohol = 2,
                MA = 3,
                Name = "Meh",
                Ph = 4,
                RS = 5,
                So2 = 6,
                TA = 7,
                VA = 8,
                State = new TankContentsState { Id = 9 },
            };
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                Name = "Meh",
                XPosition = 333,
                YPosition = 333,
                DateDeleted = null,
                Gallons = 1000,
                Contents = new TankContents { Alcohol = 1, MA = 1, Name = "Blah", Ph = 1, RS = 1, So2 = 1, TA = 1, VA = 1, TankContentsStateId = 1 }
            });

            mgr.UpdateTankContentsForAccount(contentDto, 6);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EnsureThatUpdatingTheContentsOfAnEmptyTankThrowsAnException()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var contentDto = new TankContentsDto
            {
                TankId = 1,
                Alcohol = 2,
                MA = 3,
                Name = "Meh",
                Ph = 4,
                RS = 5,
                So2 = 6,
                TA = 7,
                VA = 8,
                State = new TankContentsState { Id = 9 },
            };
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                Name = "Meh",
                XPosition = 333,
                YPosition = 333,
                DateDeleted = null,
                Gallons = 1000,
                Contents = null
            });

            mgr.UpdateTankContentsForAccount(contentDto, accountId);
        }

        [TestMethod]
        public void EnsureThatDeletingATankSetsAllRelevantProperties()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                Name = "Meh",
                XPosition = 333,
                YPosition = 333,
                DateDeleted = null,
                Gallons = 1000,
                Contents = null
            });

            mgr.DeleteTankForAccount(1, accountId);
            Assert.AreEqual(true, context.SaveChangesCalled);
            Assert.AreNotEqual(null, context.Tanks.First().DateDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void EnsureThatDeletingATankThatDoesntBelongToTheAccountThrowsAnException()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                Name = "Meh",
                XPosition = 333,
                YPosition = 333,
                DateDeleted = null,
                Gallons = 1000,
                Contents = null
            });

            mgr.DeleteTankForAccount(1, 6);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void EnsureThatATankTransferBetweenAnyTankThatDoesntBelongToTheAccountThrowsAnException()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                Gallons = 1000
            });
            context.Tanks.Add(new Tank
            {
                AccountId = 6,
                Id = 2,
                Gallons = 1000
            });
            var transfer = new TankTransferDto { FromId = 1, ToId = 2, Gallons = 100 };

            mgr.TankContentsTransferForAccount(transfer, accountId);
        }

        [TestMethod]
        public void EnsureThatATankTransferChangesAllTheRelevantPropertiesOnTheTankToFill()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            context.Tanks.Add(new Tank
            {
                AccountId = accountId,
                Id = 1,
                Gallons = 1000
            });
            var toTank = new Tank
            {
                AccountId = accountId,
                Id = 2,
                Gallons = 1000
            };
            context.Tanks.Add(toTank);
            var transfer = new TankTransferDto
            {
                FromId = 1,
                ToId = 2,
                Gallons = 100,
                Alcohol = 2,
                MA = 3,
                Name = "Meh",
                Ph = 4,
                RS = 5,
                So2 = 6,
                TA = 7,
                VA = 8,
                State = new TankContentsState { Id = 9 },
            };

            mgr.TankContentsTransferForAccount(transfer, accountId);
            Assert.AreEqual(true, context.SaveChangesCalled);
            Assert.AreEqual(transfer.Alcohol, toTank.Contents.Alcohol);
            Assert.AreEqual(transfer.MA, toTank.Contents.MA);
            Assert.AreEqual(transfer.Name, toTank.Contents.Name);
            Assert.AreEqual(transfer.Ph, toTank.Contents.Ph);
            Assert.AreEqual(transfer.RS, toTank.Contents.RS);
            Assert.AreEqual(transfer.So2, toTank.Contents.So2);
            Assert.AreEqual(transfer.VA, toTank.Contents.VA);
            Assert.AreEqual(transfer.State.Id, toTank.Contents.TankContentsStateId);
            Assert.AreEqual(transfer.TA, toTank.Contents.TA);
        }

        [TestMethod]
        public void EnsureThatATankTransferChangesTheNumberOfGallonsBetweenTheTwoTanksCorrectly()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var fromTank = new Tank
            {
                AccountId = accountId,
                Id = 1,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500 }
            };
            var toTank = new Tank
            {
                AccountId = accountId,
                Id = 2,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500 }
            };
            context.Tanks.Add(toTank);
            context.Tanks.Add(fromTank);
            var transfer = new TankTransferDto
            {
                FromId = 1,
                ToId = 2,
                Gallons = 100,
                Alcohol = 2,
                MA = 3,
                Name = "Meh",
                Ph = 4,
                RS = 5,
                So2 = 6,
                TA = 7,
                VA = 8,
                State = new TankContentsState { Id = 9 },
            };

            mgr.TankContentsTransferForAccount(transfer, accountId);
            Assert.AreEqual(500 - transfer.Gallons, fromTank.Contents.Gallons);
            Assert.AreEqual(500 + transfer.Gallons, toTank.Contents.Gallons);
        }

        [TestMethod]
        public void EnsureThatATankTransferToATankWithNoContentsSuccessfullyCreatesContentsForThatTank()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var fromTank = new Tank
            {
                AccountId = accountId,
                Id = 1,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500 }
            };
            var toTank = new Tank
            {
                AccountId = accountId,
                Id = 2,
                Gallons = 1000,
                Contents = null
            };
            context.Tanks.Add(toTank);
            context.Tanks.Add(fromTank);
            var transfer = new TankTransferDto
            {
                FromId = 1,
                ToId = 2,
                Gallons = 100,
                Alcohol = 2,
                MA = 3,
                Name = "Meh",
                Ph = 4,
                RS = 5,
                So2 = 6,
                TA = 7,
                VA = 8,
                State = new TankContentsState { Id = 9 },
            };

            mgr.TankContentsTransferForAccount(transfer, accountId);
            Assert.AreNotEqual(null, toTank.Contents);
            Assert.AreEqual(null, toTank.Contents.DateDeleted);
            Assert.AreEqual(transfer.Gallons, toTank.Contents.Gallons);
        }

        [TestMethod]
        public void EnsureThatCompletelyEmptyingATankDuringATransferSoftDeletesTheAttachedTankContents()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new TankManager(() => { return context; });
            var accountId = 5;
            var fromTank = new Tank
            {
                AccountId = accountId,
                Id = 1,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500 }
            };
            var toTank = new Tank
            {
                AccountId = accountId,
                Id = 2,
                Gallons = 1000,
                Contents = new TankContents { Gallons = 500 }
            };
            context.Tanks.Add(toTank);
            context.Tanks.Add(fromTank);
            var transfer = new TankTransferDto
            {
                FromId = 1,
                ToId = 2,
                Gallons = 500,
                Alcohol = 2,
                MA = 3,
                Name = "Meh",
                Ph = 4,
                RS = 5,
                So2 = 6,
                TA = 7,
                VA = 8,
                State = new TankContentsState { Id = 9 },
            };

            mgr.TankContentsTransferForAccount(transfer, accountId);
            Assert.AreEqual(null, fromTank.TankContentsId);
            Assert.AreNotEqual(null, fromTank.Contents.DateDeleted);
        }
    }
}
