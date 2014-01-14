using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.Tests.Mocks;
using WineProdTools.Data.Managers;
using WineProdTools.Data.EntityModels;
using WineProdTools.Data.DtoModels;

namespace WineProdTools.Data.Tests
{
    [TestClass]
    public class NoteManagerTests
    {
        [TestMethod]
        public void EnsureThatNotesAreAddedToTheSet()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });
            var comment = "this is my note comment";
            var accountId = 5;

            mgr.AddNoteForAccount(new NoteDto { Comment = comment }, accountId);
            var note = context.Notes.SingleOrDefault(a => a.Id == 0);
            Assert.AreEqual(true, note != null);
            Assert.AreEqual(true, context.SaveChangesCalled);
        }

        [TestMethod]
        public void EnsureThatAnAddedNoteHasThePassedComment()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });
            var comment = "this is my note comment";
            var accountId = 5;

            mgr.AddNoteForAccount(new NoteDto { Comment = comment }, accountId);
            var note = context.Notes.SingleOrDefault(a => a.Id == 0);
            Assert.AreEqual(comment, note.Comment);
        }

        [TestMethod]
        public void EnsureThatAnAddedNoteHasTheCorrectAssignedAccount()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });
            var comment = "this is my note comment";
            var accountId = 5;

            mgr.AddNoteForAccount(new NoteDto { Comment = comment }, accountId);
            var note = context.Notes.SingleOrDefault(a => a.Id == 0);
            Assert.AreEqual(accountId, note.AccountId);
        }

        [TestMethod]
        public void EnsureThatAnAddedNoteHasItsCreateDateSetForNow()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });
            var comment = "this is my note comment";
            var accountId = 5;

            var beforeNoteCreated = DateTime.Now;
            mgr.AddNoteForAccount(new NoteDto { Comment = comment }, accountId);
            var note = context.Notes.SingleOrDefault(a => a.Id == 0);
            Assert.AreEqual(true, beforeNoteCreated <= note.DateCreated);
        }

        [TestMethod]
        public void EnsureThatGettingSomeNotesOnlyGrabsNotesForTheSpecifiedAccount()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });
            var accountId = 5;
            var note1 = new Note { AccountId = accountId, Comment = "blah" };
            var note2 = new Note { AccountId = accountId, Comment = "blah" };
            var noteOther = new Note { AccountId = 7, Comment = "blah" };
            context.Notes.Add(note1);
            context.Notes.Add(note2);
            context.Notes.Add(noteOther);

            var actual = mgr.GetSomeNotesAfterThisManyForAccount(0, accountId);
            Assert.AreEqual(2, actual.Count());
        }

        [TestMethod]
        public void EnsureThatGettingSomeNotesOnlyGrabsNotesDescendingNumericallyAfterTheNumberToSkip()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });
            var accountId = 5;
            var note1 = new Note { Id = 1, AccountId = accountId, Comment = "blah" };
            var note2 = new Note { Id = 2, AccountId = accountId, Comment = "blah" };
            var note3 = new Note { Id = 3, AccountId = accountId, Comment = "blah" };
            var note4 = new Note { Id = 4, AccountId = accountId, Comment = "blah" };
            var note5 = new Note { Id = 5, AccountId = accountId, Comment = "blah" };
            context.Notes.Add(note1);
            context.Notes.Add(note2);
            context.Notes.Add(note3);
            context.Notes.Add(note4);
            context.Notes.Add(note5);

            var actual = mgr.GetSomeNotesAfterThisManyForAccount(3, accountId).ToList();
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(2, actual[0].Id);
            Assert.AreEqual(1, actual[1].Id);
        }

        [TestMethod]
        public void EnsureDateTimeSinceNowReportsSecondsProperly()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });

            var actual = mgr.TimeSince(DateTime.Now.AddSeconds(-1), DateTime.Now);
            Assert.AreEqual("1 second", actual);
            actual = mgr.TimeSince(DateTime.Now.AddSeconds(-59), DateTime.Now);
            Assert.AreEqual("59 seconds", actual);
            actual = mgr.TimeSince(DateTime.Now.AddSeconds(-60), DateTime.Now);
            Assert.AreEqual("1 minute", actual);
        }

        [TestMethod]
        public void EnsureDateTimeSinceNowReportsMinutesProperly()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });

            var actual = mgr.TimeSince(DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.AreEqual("1 minute", actual);
            actual = mgr.TimeSince(DateTime.Now.AddMinutes(-59), DateTime.Now);
            Assert.AreEqual("59 minutes", actual);
            actual = mgr.TimeSince(DateTime.Now.AddMinutes(-60), DateTime.Now);
            Assert.AreEqual("1 hour", actual);
        }

        [TestMethod]
        public void EnsureDateTimeSinceNowReportsHoursProperly()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });

            var actual = mgr.TimeSince(DateTime.Now.AddHours(-1), DateTime.Now);
            Assert.AreEqual("1 hour", actual);
            actual = mgr.TimeSince(DateTime.Now.AddHours(-23), DateTime.Now);
            Assert.AreEqual("23 hours", actual);
            actual = mgr.TimeSince(DateTime.Now.AddHours(-24), DateTime.Now);
            Assert.AreEqual("1 day", actual);
        }

        [TestMethod]
        public void EnsureDateTimeSinceNowReportsDaysProperly()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });

            var actual = mgr.TimeSince(DateTime.Now.AddDays(-1), DateTime.Now);
            Assert.AreEqual("1 day", actual);
            actual = mgr.TimeSince(DateTime.Now.AddDays(-29), DateTime.Now);
            Assert.AreEqual("29 days", actual);
            actual = mgr.TimeSince(DateTime.Now.AddDays(-30), DateTime.Now);
            Assert.AreEqual("1 month", actual);
        }

        [TestMethod]
        public void EnsureDateTimeSinceNowReportsMonthsProperly()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });

            var actual = mgr.TimeSince(DateTime.Now.AddMonths(-1), DateTime.Now);
            Assert.AreEqual("1 month", actual);
            actual = mgr.TimeSince(DateTime.Now.AddMonths(-11), DateTime.Now);
            Assert.AreEqual("11 months", actual);
            actual = mgr.TimeSince(DateTime.Now.AddMonths(-12), DateTime.Now);
            Assert.AreEqual("1 year", actual);
        }

        [TestMethod]
        public void EnsureDateTimeSinceNowReportsYearsProperly()
        {
            var context = new FakeWineProdToolsContext();
            var mgr = new NoteManager(() => { return context; });

            var actual = mgr.TimeSince(DateTime.Now.AddYears(-1), DateTime.Now);
            Assert.AreEqual("1 year", actual);
            actual = mgr.TimeSince(DateTime.Now.AddMonths(-18), DateTime.Now);
            Assert.AreEqual("1 year", actual);
            actual = mgr.TimeSince(DateTime.Now.AddYears(-2), DateTime.Now);
            Assert.AreEqual("2 years", actual);
        }
    }
}
