﻿// Copyright © 2018 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InterfaceFactory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Framework;
using VirtualRadar.Interface;
using VirtualRadar.Interface.Database;

namespace Test.VirtualRadar.Database
{
    public class TrackHistoryDatabaseTests<T>
        where T : ITrackHistoryDatabase
    {
        public TestContext TestContext { get; set; }

        protected T _Database;
        protected Func<IDbConnection> _CreateConnection;
        protected Action<T> _InitialiseImplementation;
        protected string _SqlReturnNewIdentity;
        protected ClockMock _Clock;

        protected IClassFactory _Snapshot;

        protected void CommonTestInitialise(Action initialiseDatabase, Func<IDbConnection> createConnection, Action<T> initialiseImplementation, string sqlReturnNewIdentity)
        {
            _Snapshot = Factory.TakeSnapshot();

            _Clock = new ClockMock();
            Factory.RegisterInstance<IClock>(_Clock.Object);

            initialiseDatabase?.Invoke();
            _CreateConnection = createConnection;
            _SqlReturnNewIdentity = sqlReturnNewIdentity;

            _InitialiseImplementation = initialiseImplementation;
            EstablishDatabaseImplementation();
        }

        protected void EstablishDatabaseImplementation()
        {
            var implementation = (T)Factory.Resolve(typeof(T));
            _Database = implementation;
            _InitialiseImplementation?.Invoke(implementation);
        }

        protected void CommonTestCleanup()
        {
            if(_Database != null) {
                _Database = default(T);
            }

            Factory.RestoreSnapshot(_Snapshot);
        }

        private MethodInfo _TestInitialise;
        private void RunTestInitialise()
        {
            if(_TestInitialise == null) {
                _TestInitialise = GetType().GetMethods().Single(r => r.GetCustomAttributes(typeof(TestInitializeAttribute), inherit: false).Length != 0);
            }
            _TestInitialise.Invoke(this, new object[0]);
        }

        private MethodInfo _TestCleanup;
        private void RunTestCleanup()
        {
            if(_TestCleanup == null) {
                _TestCleanup = GetType().GetMethods().Single(r => r.GetCustomAttributes(typeof(TestCleanupAttribute), inherit: false).Length != 0);
            }
            _TestCleanup.Invoke(this, new object[0]);
        }

        #region Aircraft
        protected void Aircraft_Save_Creates_New_Aircraft_Correctly()
        {
            var created = DateTime.UtcNow;
            var updated = created.AddMilliseconds(7);
            foreach(var aircraft in SampleAircraft(true, created, updated)) {
                _Database.Aircraft_Save(aircraft);

                Assert.AreNotEqual(0, aircraft.AircraftID);

                var readBack = _Database.Aircraft_GetByID(aircraft.AircraftID);
                AssertAircraftAreEqual(aircraft, readBack);
            }
        }

        protected void Aircraft_Save_Updates_Existing_Records_Correctly()
        {
            var created = DateTime.UtcNow;
            var updated = created.AddSeconds(9);

            var allSavedAircraft =   SampleAircraft(generateForCreate: true,  createdUtc: created, updatedUtc: created);
            var allUpdatedAircraft = SampleAircraft(generateForCreate: false, createdUtc: created, updatedUtc: updated);

            foreach(var aircraft in allSavedAircraft) {
                _Database.Aircraft_Save(aircraft);
            }

            for(var i = 0;i < allSavedAircraft.Count;++i) {
                var savedAircraft = allSavedAircraft[i];
                var updatedAircraft = allUpdatedAircraft[i];

                updatedAircraft.AircraftID = savedAircraft.AircraftID;
                _Database.Aircraft_Save(updatedAircraft);

                var readBack = _Database.Aircraft_GetByID(savedAircraft.AircraftID);
                AssertAircraftAreEqual(updatedAircraft, readBack);
            }
        }

        protected void Aircraft_GetByIcao_Returns_Aircraft_For_Icao()
        {
            foreach(var aircraft in SampleAircraft(true)) {
                _Database.Aircraft_Save(aircraft);

                var readBack = _Database.Aircraft_GetByIcao(aircraft.Icao);
                AssertAircraftAreEqual(aircraft, readBack);
            }
        }

        protected void Aircraft_GetByIcao_Is_Case_Insensitive()
        {
            foreach(var aircraft in SampleAircraft(true)) {
                _Database.Aircraft_Save(aircraft);

                var flippedIcao = TestUtilities.FlipCase(aircraft.Icao);

                var readBack = _Database.Aircraft_GetByIcao(flippedIcao);
                AssertAircraftAreEqual(aircraft, readBack);
            }
        }

        private List<TrackHistoryAircraft> SampleAircraft(bool generateForCreate, DateTime? createdUtc = null, DateTime? updatedUtc = null)
        {
            var created = createdUtc ?? DateTime.UtcNow;
            var updated = updatedUtc ?? DateTime.UtcNow;

            var result = new List<TrackHistoryAircraft>();

            var previousIcao = 0;
            foreach(var property in typeof(TrackHistoryAircraft).GetProperties()) {
                var aircraft = new TrackHistoryAircraft() {
                    Icao =          (++previousIcao).ToString("X6"),
                    CreatedUtc =    created,
                    UpdatedUtc =    updated,
                };

                var propertyValue = GenerateAircraftPropertyValue(property, generateForCreate);
                if(propertyValue != null) {
                    property.SetValue(aircraft, propertyValue);
                    result.Add(aircraft);
                }
            }

            return result;
        }

        private object GenerateAircraftPropertyValue(PropertyInfo property, bool generateForCreate)
        {
            object value = null;

            switch(property.Name) {
                case nameof(TrackHistoryAircraft.Notes):        value = generateForCreate ? new String('Ă', 2000) : new string('b', 2000); break;
                case nameof(TrackHistoryAircraft.Registration): value = generateForCreate ? new String('A', 20)   : new string('b', 20); break;
                case nameof(TrackHistoryAircraft.Serial):       value = generateForCreate ? new String('Ă', 200)  : new string('b', 200); break;

                case nameof(TrackHistoryAircraft.LastLookupUtc):
                    value = generateForCreate ? new DateTime(2019, 2, 1, 17, 16, 15, 143) : new DateTime(2009, 8, 7, 6, 5, 4, 321);
                    break;
                case nameof(TrackHistoryAircraft.YearBuilt):
                    value = generateForCreate ? 25 : 50;
                    break;
                case nameof(TrackHistoryAircraft.IsInteresting):
                case nameof(TrackHistoryAircraft.IsMissingFromLookup):
                case nameof(TrackHistoryAircraft.SuppressAutoUpdates):
                    value = generateForCreate;
                    break;

                case nameof(TrackHistoryAircraft.AircraftID):
                case nameof(TrackHistoryAircraft.CreatedUtc):
                case nameof(TrackHistoryAircraft.Icao):
                case nameof(TrackHistoryAircraft.UpdatedUtc):
                    break;

                default:
                    throw new NotImplementedException($"Need code for {nameof(TrackHistoryAircraft)}.{property.Name}");
            }

            return value;
        }

        private void AssertAircraftAreEqual(TrackHistoryAircraft expected, TrackHistoryAircraft actual)
        {
            TestUtilities.TestObjectPropertiesAreEqual(expected, actual);
        }
        #endregion

        #region Receiver
        protected void Receiver_Save_Creates_New_Records_Correctly()
        {
            var created = DateTime.UtcNow;
            var updated = created.AddMilliseconds(7);
            foreach(var receiver in SampleReceivers(created, updated)) {
                _Database.Receiver_Save(receiver);

                Assert.AreNotEqual(0, receiver.ReceiverID);

                var readBack = _Database.Receiver_GetByID(receiver.ReceiverID);
                AssertReceiversAreEqual(receiver, readBack);
            }
        }

        protected void Receiver_Save_Updates_Existing_Records_Correctly()
        {
            var now = DateTime.UtcNow;

            var createReceivers = SampleReceivers(now, now);
            foreach(var receiver in createReceivers) {
                _Database.Receiver_Save(receiver);
            }

            var updatedTime = now.AddSeconds(7);
            var updateReceivers = SampleReceivers(now, updatedTime);
            for(var i = 0;i < updateReceivers.Length;++i) {
                var createReceiver = createReceivers[i];
                var updateReceiver = updateReceivers[i];

                var expectedName = new String(createReceiver.Name.Reverse().ToArray());

                updateReceiver.ReceiverID = createReceiver.ReceiverID;
                updateReceiver.Name = expectedName;

                _Database.Receiver_Save(updateReceiver);

                Assert.AreEqual(createReceiver.ReceiverID, updateReceiver.ReceiverID);
                Assert.AreEqual(expectedName,              updateReceiver.Name);
                Assert.AreEqual(createReceiver.CreatedUtc, updateReceiver.CreatedUtc);
                Assert.AreEqual(updatedTime,               updateReceiver.UpdatedUtc);

                var readBack = _Database.Receiver_GetByID(updateReceiver.ReceiverID);
                AssertReceiversAreEqual(updateReceiver, readBack);
            }
        }

        protected void Receiver_GetByName_Fetches_By_Case_Insensitive_Name()
        {
            foreach(var receiver in SampleReceivers()) {
                _Database.Receiver_Save(receiver);

                var sameCaseReadBack = _Database.Receiver_GetByName(receiver.Name);
                AssertReceiversAreEqual(receiver, sameCaseReadBack);

                var flippedName = TestUtilities.FlipCase(receiver.Name);
                var flippedCaseReadBack = _Database.Receiver_GetByName(flippedName);
                AssertReceiversAreEqual(receiver, flippedCaseReadBack);
            }
        }

        protected void Receiver_GetOrCreateByName_Creates_New_Records_Correctly()
        {
            var now = DateTime.UtcNow.AddDays(-7);
            _Clock.UtcNowValue = now;

            foreach(var name in SampleReceivers().Select(r => r.Name)) {
                var saved = _Database.Receiver_GetOrCreateByName(name);

                Assert.AreNotEqual(0, saved.ReceiverID);
                Assert.AreEqual(now, saved.CreatedUtc);
                Assert.AreEqual(now, saved.UpdatedUtc);

                var readBack = _Database.Receiver_GetByID(saved.ReceiverID);
                AssertReceiversAreEqual(saved, readBack);
            }
        }

        protected void Receiver_GetOrCreateByName_Fetches_Existing_Records_Correctly()
        {
            foreach(var receiver in SampleReceivers()) {
                _Database.Receiver_Save(receiver);

                var sameCaseReadBack = _Database.Receiver_GetOrCreateByName(receiver.Name);
                AssertReceiversAreEqual(receiver, sameCaseReadBack);

                var flippedName = TestUtilities.FlipCase(receiver.Name);
                var flippedCaseReadBack = _Database.Receiver_GetOrCreateByName(flippedName);
                AssertReceiversAreEqual(receiver, flippedCaseReadBack);
            }
        }

        protected void Receiver_Delete_Deletes_Receivers()
        {
            foreach(var receiver in SampleReceivers()) {
                _Database.Receiver_Save(receiver);

                var id = receiver.ReceiverID;
                _Database.Receiver_Delete(receiver);

                var readBack = _Database.Receiver_GetByID(id);
                Assert.IsNull(readBack);
            }
        }

        protected void Receiver_Delete_Nulls_Out_References_To_Receiver()
        {
            var trackHistory = SampleTrackHistoryForHistoryStates();
            var state = SampleTrackHistoryStates(trackHistory, generateForCreate: true).First(r => r.ReceiverID != null);
            _Database.TrackHistoryState_Save(state);

            var receiver = _Database.Receiver_GetByID(state.ReceiverID.Value);
            Assert.IsNotNull(receiver);

            _Database.Receiver_Delete(receiver);

            var readBackState = _Database.TrackHistoryState_GetByID(state.TrackHistoryStateID);
            Assert.IsNull(readBackState.ReceiverID);
        }

        protected void Receiver_Delete_Ignores_Deleted_Receivers()
        {
            var doesNotExist = new TrackHistoryReceiver() {
                ReceiverID = 1,
                Name =       "sqrt(-1)",
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
            };

            // This just needs to not throw an exception
            _Database.Receiver_Delete(doesNotExist);
        }

        private TrackHistoryReceiver[] SampleReceivers(DateTime? createdUtc = null, DateTime? updatedUtc = null)
        {
            var created = createdUtc ?? DateTime.UtcNow;
            var updated = updatedUtc ?? DateTime.UtcNow;

            return new TrackHistoryReceiver[] {
                new TrackHistoryReceiver() { Name = "Strix", CreatedUtc = created, UpdatedUtc = updated, },
                new TrackHistoryReceiver() { Name = "Diath", CreatedUtc = created, UpdatedUtc = updated, },
            };
        }

        private void AssertReceiversAreEqual(TrackHistoryReceiver expected, TrackHistoryReceiver actual)
        {
            TestUtilities.TestObjectPropertiesAreEqual(expected, actual);
        }
        #endregion

        #region TrackHistory
        protected void TrackHistory_Save_Creates_New_Records_Correctly()
        {
            foreach(var trackHistory in SampleTrackHistories()) {
                _Database.TrackHistory_Save(trackHistory);

                Assert.AreNotEqual(0, trackHistory.TrackHistoryID);

                var readBack = _Database.TrackHistory_GetByID(trackHistory.TrackHistoryID);
                AssertTrackHistoriesAreEqual(trackHistory, readBack);
            }
        }

        protected void TrackHistory_Save_Updates_Existing_Records_Correctly()
        {
            var now = DateTime.UtcNow;

            var createTrackHistories = SampleTrackHistories(now, now);
            foreach(var trackHistory in createTrackHistories) {
                _Database.TrackHistory_Save(trackHistory);
            }

            var updatedTime = now.AddSeconds(7);
            var updateTrackHistories = SampleTrackHistories(now, updatedTime);
            for(var i = 0;i < updateTrackHistories.Length;++i) {
                var createHistory = createTrackHistories[i];
                var updateHistory = updateTrackHistories[i];

                var originalAircraft = _Database.Aircraft_GetByID(createHistory.AircraftID);

                var expectedAircraftID = AircraftIDForIcao(new String(originalAircraft.Icao.Reverse().ToArray()));
                var expectedIsPreserved = !createHistory.IsPreserved;

                updateHistory.TrackHistoryID = createHistory.TrackHistoryID;
                updateHistory.AircraftID = expectedAircraftID;
                updateHistory.IsPreserved = expectedIsPreserved;

                _Database.TrackHistory_Save(updateHistory);

                Assert.AreEqual(createHistory.TrackHistoryID, updateHistory.TrackHistoryID);
                Assert.AreEqual(expectedAircraftID,           updateHistory.AircraftID);
                Assert.AreEqual(expectedIsPreserved,          updateHistory.IsPreserved);
                Assert.AreEqual(createHistory.CreatedUtc,     updateHistory.CreatedUtc);
                Assert.AreEqual(updatedTime,                  updateHistory.UpdatedUtc);

                var readBack = _Database.TrackHistory_GetByID(updateHistory.TrackHistoryID);
                AssertTrackHistoriesAreEqual(updateHistory, readBack);
            }
        }

        protected void TrackHistory_GetByAircraftID_With_No_Criteria_Returns_Correct_Records()
        {
            var today = DateTime.UtcNow;
            var tomorrow = today.AddDays(1);
            var savedHistories = SaveTrackHistoriesForTimes(new DateTime[] { today, tomorrow });
            var todayHistories = savedHistories.Where(r => r.CreatedUtc == today).ToArray();
            var tomorrowHistories = savedHistories.Where(r => r.CreatedUtc == tomorrow).ToArray();

            for(var historyIdx = 0;historyIdx < todayHistories.Length;++historyIdx) {
                var expected = new TrackHistory[] {
                    todayHistories[historyIdx],
                    tomorrowHistories[historyIdx],
                };

                var aircraftID = expected[0].AircraftID;
                var readBack = _Database.TrackHistory_GetByAircraftID(aircraftID, null, null).ToArray();

                AssertTrackHistoriesAreEqual(expected, readBack);
            }
        }

        private List<TrackHistory> SaveTrackHistoriesForTimes(DateTime[] utcTimes)
        {
            var result = new List<TrackHistory>();

            foreach(var utcTime in utcTimes) {
                foreach(var trackHistory in SampleTrackHistories(utcTime, utcTime)) {
                    _Database.TrackHistory_Save(trackHistory);
                    result.Add(trackHistory);
                }
            }

            return result;
        }

        protected void TrackHistory_GetByAircraftID_With_Criteria_Returns_Correct_Records()
        {
            var today = DateTime.UtcNow;
            var yesterday = today.AddDays(-1);
            var tomorrow = today.AddDays(1);
            var savedHistories = SaveTrackHistoriesForTimes(new DateTime[] { today, yesterday, tomorrow });

            var aircraftIDs = savedHistories.Select(r => r.AircraftID).Distinct().ToArray();

            foreach(var dateRange in SampleTrackHistoryDateRanges(yesterday, today, tomorrow)) {
                foreach(var aircraftID in aircraftIDs) {
                    var from = dateRange.Item1;
                    var to =   dateRange.Item2;
                    var expected = ExtractExpectedTrackHistoriesForDateTime(savedHistories, from, to, filterToAircraftID: true, aircraftID: aircraftID);

                    var readbackHistories = _Database.TrackHistory_GetByAircraftID(aircraftID, from, to);
                    AssertTrackHistoriesAreEqual(expected, readbackHistories);
                }
            }
        }

        protected void TrackHistory_GetByDateRange_Returns_Correct_Records()
        {
            var today = DateTime.UtcNow;
            var yesterday = today.AddDays(-1);
            var tomorrow = today.AddDays(1);
            var savedHistories = SaveTrackHistoriesForTimes(new DateTime[] { today, yesterday, tomorrow });

            var aircraftIDs = savedHistories.Select(r => r.AircraftID).Distinct().ToArray();

            foreach(var dateRange in SampleTrackHistoryDateRanges(yesterday, today, tomorrow)) {
                foreach(var aircraftID in aircraftIDs) {
                    var from = dateRange.Item1;
                    var to =   dateRange.Item2;
                    var expected = ExtractExpectedTrackHistoriesForDateTime(savedHistories, from, to);

                    var actual = _Database.TrackHistory_GetByDateRange(from, to);
                    AssertTrackHistoriesAreEqual(expected, actual);
                }
            }
        }

        protected void TrackHistory_Delete_Removes_TrackHistory_Records()
        {
            foreach(var isPreserved in new bool[] { false, true }) {
                var trackHistory = new TrackHistory() {
                    AircraftID =  AircraftIDForIcao("123456"),
                    IsPreserved = isPreserved,
                };
                _Database.TrackHistory_Save(trackHistory);

                var deleted = _Database.TrackHistory_Delete(trackHistory);

                Assert.AreEqual(1, deleted.CountTrackHistories);
                Assert.AreEqual(0, deleted.CountTrackHistoryStates);
                Assert.AreEqual(trackHistory.CreatedUtc, deleted.EarliestHistoryUtc);
                Assert.AreEqual(trackHistory.CreatedUtc, deleted.LatestHistoryUtc);

                var readBack = _Database.TrackHistory_GetByID(trackHistory.TrackHistoryID);
                Assert.IsNull(readBack);
            }
        }

        protected void TrackHistory_Delete_Can_Be_Called_Within_A_Transaction()
        {
            var record = new TrackHistory() {
                AircraftID = AircraftIDForIcao("123456"),
            };
            _Database.TrackHistory_Save(record);

            _Database.PerformInTransaction(() => {
                _Database.TrackHistory_Delete(record);

                var inTransReadBack = _Database.TrackHistory_GetByID(record.TrackHistoryID);
                Assert.IsNull(inTransReadBack);

                return false;
            });

            var readBack = _Database.TrackHistory_GetByID(record.TrackHistoryID);
            AssertTrackHistoriesAreEqual(record, readBack);
        }

        protected void TrackHistory_DeleteExpired_Deletes_Appropriate_Transactions()
        {
            var today = DateTime.UtcNow;
            var yesterday = today.AddDays(-1);
            var dayBeforeYesterday = yesterday.AddDays(-1);
            var savedHistories = SaveTrackHistoriesForTimes(new DateTime[] { yesterday, today, dayBeforeYesterday });

            var expectDeleted = savedHistories.Where(r => r.CreatedUtc <= yesterday && !r.IsPreserved).ToArray();

            var deleted = _Database.TrackHistory_DeleteExpired(yesterday);
            Assert.AreEqual(expectDeleted.Length, deleted.CountTrackHistories);
            Assert.AreEqual(0, deleted.CountTrackHistoryStates);
            Assert.AreEqual(expectDeleted.Select(r => r.CreatedUtc).Min(), deleted.EarliestHistoryUtc);
            Assert.AreEqual(expectDeleted.Select(r => r.CreatedUtc).Max(), deleted.LatestHistoryUtc);
        }

        protected void TrackHistory_Truncate_Removes_All_But_First_And_Last_States()
        {
            var trackHistory = SampleTrackHistoryForHistoryStates();
            var allStates = SampleTrackHistoryStates(trackHistory, generateForCreate: true);
            _Database.TrackHistoryState_SaveMany(allStates);

            var expected1st = allStates[0];
            var expected2nd = TrackHistoryState.MergeStates(allStates);
            var expectedStates = new TrackHistoryState[] { expected1st, expected2nd };

            var now = trackHistory.UpdatedUtc.AddSeconds(19);

            var truncateResult = _Database.TrackHistory_Truncate(trackHistory, now);
            Assert.AreEqual(1,                          truncateResult.CountTrackHistories);
            Assert.AreEqual(allStates.Count - 2,        truncateResult.CountTrackHistoryStates);
            Assert.AreEqual(trackHistory.CreatedUtc,    truncateResult.EarliestHistoryUtc);
            Assert.AreEqual(trackHistory.UpdatedUtc,    truncateResult.LatestHistoryUtc);

            var readBackStates = _Database.TrackHistoryState_GetByTrackHistory(trackHistory).ToArray();
            Assert.AreEqual(2, readBackStates.Length);

            // Bit of a kludge, we don't know the ID of the merged record
            Assert.AreNotEqual(0, readBackStates[1].TrackHistoryStateID);
            expected2nd.TrackHistoryStateID = readBackStates[1].TrackHistoryStateID;

            AssertTrackHistoryStatesAreEqual(expectedStates, readBackStates);
        }

        protected void TrackHistory_Truncate_Ignores_Preserved_Histories()
        {
            var trackHistory = SampleTrackHistoryForHistoryStates(isPreserved: true);
            var allStates = SampleTrackHistoryStates(trackHistory, generateForCreate: true);
            _Database.TrackHistoryState_SaveMany(allStates);

            var now = trackHistory.UpdatedUtc.AddSeconds(19);

            var truncateResult = _Database.TrackHistory_Truncate(trackHistory, now);
            Assert.AreEqual(0,                  truncateResult.CountTrackHistories);
            Assert.AreEqual(0,                  truncateResult.CountTrackHistoryStates);
            Assert.AreEqual(default(DateTime),  truncateResult.EarliestHistoryUtc);
            Assert.AreEqual(default(DateTime),  truncateResult.LatestHistoryUtc);

            var readBackStates = _Database.TrackHistoryState_GetByTrackHistory(trackHistory).ToArray();
            AssertTrackHistoryStatesAreEqual(allStates, readBackStates);
        }

        protected void TrackHistory_TruncateExpired_Truncates_Histories()
        {
            var now = DateTime.UtcNow;
            var threshold = now.AddDays(-7);

            var truncatable1 =  SampleTrackHistoryForHistoryStates(utcNow: threshold);
            var preserved =     SampleTrackHistoryForHistoryStates(utcNow: threshold, isPreserved: true);
            var pastThreshold = SampleTrackHistoryForHistoryStates(utcNow: threshold.AddSeconds(1));
            var truncatable2 =  SampleTrackHistoryForHistoryStates(utcNow: threshold.AddSeconds(-1));

            var truncatable1States =  SampleTrackHistoryStates(truncatable1, generateForCreate: true);
            var preservedStates =     SampleTrackHistoryStates(preserved, generateForCreate: true);
            var pastThresholdStates = SampleTrackHistoryStates(pastThreshold, generateForCreate: true);
            var truncatable2States =  SampleTrackHistoryStates(truncatable2, generateForCreate: true);

            _Database.TrackHistoryState_SaveMany(truncatable1States);
            _Database.TrackHistoryState_SaveMany(preservedStates);
            _Database.TrackHistoryState_SaveMany(pastThresholdStates);
            _Database.TrackHistoryState_SaveMany(truncatable2States);

            var truncateResult = _Database.TrackHistory_TruncateExpired(threshold, now);
            Assert.AreEqual(2,                                                               truncateResult.CountTrackHistories);
            Assert.AreEqual((truncatable1States.Count - 2) + (truncatable2States.Count - 2), truncateResult.CountTrackHistoryStates);
            Assert.AreEqual(truncatable2.CreatedUtc,                                         truncateResult.EarliestHistoryUtc);
            Assert.AreEqual(truncatable1.CreatedUtc,                                         truncateResult.LatestHistoryUtc);

            Assert.AreEqual(2, _Database.TrackHistoryState_GetByTrackHistory(truncatable1).Count());
            Assert.AreEqual(2, _Database.TrackHistoryState_GetByTrackHistory(truncatable2).Count());
            Assert.AreEqual(preservedStates.Count, _Database.TrackHistoryState_GetByTrackHistory(preserved).Count());
            Assert.AreEqual(pastThresholdStates.Count, _Database.TrackHistoryState_GetByTrackHistory(pastThreshold).Count());
        }

        private Tuple<DateTime?, DateTime?>[] SampleTrackHistoryDateRanges(DateTime yesterday, DateTime today, DateTime tomorrow)
        {
            return new Tuple<DateTime?, DateTime?>[] {
                new Tuple<DateTime?, DateTime?>(null, today),
                new Tuple<DateTime?, DateTime?>(today, null),
                new Tuple<DateTime?, DateTime?>(today, tomorrow),

                new Tuple<DateTime?, DateTime?>(null, today.AddMilliseconds(-1)),
                new Tuple<DateTime?, DateTime?>(today.AddMilliseconds(-1), null),
                new Tuple<DateTime?, DateTime?>(today, tomorrow.AddMilliseconds(-1)),

                new Tuple<DateTime?, DateTime?>(null, today.AddMilliseconds(1)),
                new Tuple<DateTime?, DateTime?>(today.AddMilliseconds(1), null),
                new Tuple<DateTime?, DateTime?>(today, tomorrow.AddMilliseconds(1)),

                new Tuple<DateTime?, DateTime?>(today, yesterday),
            };
        }

        private TrackHistory[] SampleTrackHistories(DateTime? createdUtc = null, DateTime? updatedUtc = null)
        {
            var coalescedCreated = createdUtc ?? DateTime.UtcNow;
            var coalescedUpdated = updatedUtc ?? DateTime.UtcNow;

            return new TrackHistory[] {
                new TrackHistory() { AircraftID = AircraftIDForIcao("ABC123"), IsPreserved = true,  CreatedUtc = coalescedCreated, UpdatedUtc = coalescedUpdated, },
                new TrackHistory() { AircraftID = AircraftIDForIcao("987654"), IsPreserved = false, CreatedUtc = coalescedCreated, UpdatedUtc = coalescedUpdated, },
            };
        }

        private long AircraftIDForIcao(string icao)
        {
            var result = _Database.Aircraft_GetByIcao(icao);
            if(result == null) {
                result = new TrackHistoryAircraft() {
                    Icao =          icao,
                    CreatedUtc =    DateTime.UtcNow,
                    UpdatedUtc =    DateTime.UtcNow,
                };
                _Database.Aircraft_Save(result);
            }

            return result.AircraftID;
        }

        private void AssertTrackHistoriesAreEqual(TrackHistory expected, TrackHistory actual)
        {
            TestUtilities.TestObjectPropertiesAreEqual(expected, actual);
        }

        private void AssertTrackHistoriesAreEqual(IEnumerable<TrackHistory> expected, IEnumerable<TrackHistory> actual)
        {
            var expectedArray = expected?.ToArray() ?? new TrackHistory[0];
            var actualArray =   actual?.ToArray() ?? new TrackHistory[0];

            Assert.AreEqual(expectedArray.Length, actualArray.Length);
            for(var i = 0;i < expectedArray.Length;++i) {
                AssertTrackHistoriesAreEqual(expectedArray[i], actualArray[i]);
            }
        }

        private TrackHistory[] ExtractExpectedTrackHistoriesForDateTime(IEnumerable<TrackHistory> histories, DateTime? from, DateTime? to, bool filterToAircraftID = false, long? aircraftID = null)
        {
            return histories.Where(r =>
                   r.CreatedUtc >= from.GetValueOrDefault()
                && r.CreatedUtc <= (to ?? new DateTime(9999, 12, 31))
                && (!filterToAircraftID || r.AircraftID == aircraftID)
            )
            .OrderBy(r => r.CreatedUtc)
            .ThenBy(r => r.AircraftID)
            .ToArray();
        }
        #endregion

        #region TrackHistoryState
        protected void TrackHistoryState_Save_Creates_New_Records_Correctly()
        {
            var trackHistory = SampleTrackHistoryForHistoryStates();

            var seenStateIDs = new HashSet<long>();

            foreach(var state in SampleTrackHistoryStates(trackHistory, generateForCreate: true)) {
                _Database.TrackHistoryState_Save(state);

                Assert.AreNotEqual(0, state.TrackHistoryStateID);

                Assert.IsFalse(seenStateIDs.Contains(state.TrackHistoryStateID));
                seenStateIDs.Add(state.TrackHistoryStateID);

                var readBack = _Database.TrackHistoryState_GetByID(state.TrackHistoryStateID);
                AssertTrackHistoryStatesAreEqual(state, readBack);
            }
        }

        protected void TrackHistoryState_Save_Updates_Existing_Records_Correctly()
        {
            var trackHistory = SampleTrackHistoryForHistoryStates();
            var created = DateTime.UtcNow;
            var updated = created.AddSeconds(9);

            var savedStates =   SampleTrackHistoryStates(trackHistory, generateForCreate: true,  timestampUtc: created);
            var updatedStates = SampleTrackHistoryStates(trackHistory, generateForCreate: false, timestampUtc: updated);

            foreach(var state in savedStates) {
                _Database.TrackHistoryState_Save(state);
            }

            for(var i = 0;i < savedStates.Count;++i) {
                var savedState = savedStates[i];
                var updatedState = updatedStates[i];

                updatedState.TrackHistoryStateID = savedState.TrackHistoryStateID;
                _Database.TrackHistoryState_Save(updatedState);

                var readBack = _Database.TrackHistoryState_GetByID(savedState.TrackHistoryStateID);
                AssertTrackHistoryStatesAreEqual(updatedState, readBack);
            }
        }

        protected void TrackHistoryState_Save_Can_Move_To_Another_TrackHistory()
        {
            var now = DateTime.UtcNow;
            var originalHistory = SampleTrackHistoryForHistoryStates(utcNow: now);
            var newHistory      = SampleTrackHistoryForHistoryStates(utcNow: now.AddSeconds(1));

            var state = new TrackHistoryState() {
                TrackHistoryID = originalHistory.TrackHistoryID,
                SequenceNumber = 1,
                TimestampUtc =   now.AddSeconds(2),
            };
            _Database.TrackHistoryState_Save(state);

            state.TrackHistoryID = newHistory.TrackHistoryID;
            _Database.TrackHistoryState_Save(state);

            var readBack = _Database.TrackHistoryState_GetByID(state.TrackHistoryStateID);
            Assert.AreEqual(newHistory.TrackHistoryID, readBack.TrackHistoryID);
        }

        protected void TrackHistoryState_Save_Throws_If_TrackHistoryID_Is_Zero()
        {
            _Database.TrackHistoryState_Save(new TrackHistoryState() { TrackHistoryID = 0, SequenceNumber = 1, TimestampUtc = DateTime.UtcNow, });
        }

        protected void TrackHistoryState_Save_Throws_If_SequenceNumber_Is_Zero()
        {
            _Database.TrackHistoryState_Save(new TrackHistoryState() { TrackHistoryID = 1, SequenceNumber = 0, TimestampUtc = DateTime.UtcNow, });
        }

        protected void TrackHistoryState_Save_Throws_If_Timestamp_Is_Default()
        {
            _Database.TrackHistoryState_Save(new TrackHistoryState() { TrackHistoryID = 1, SequenceNumber = 1, });
        }

        protected void TrackHistoryState_SaveMany_Creates_New_Records_Correctly()
        {
            var trackHistory = SampleTrackHistoryForHistoryStates();

            var seenStateIDs = new HashSet<long>();
            var allStates = SampleTrackHistoryStates(trackHistory, generateForCreate: true);
            _Database.TrackHistoryState_SaveMany(allStates);

            foreach(var state in allStates) {
                Assert.AreNotEqual(0, state.TrackHistoryStateID);

                Assert.IsFalse(seenStateIDs.Contains(state.TrackHistoryStateID));
                seenStateIDs.Add(state.TrackHistoryStateID);

                var readBack = _Database.TrackHistoryState_GetByID(state.TrackHistoryStateID);
                AssertTrackHistoryStatesAreEqual(state, readBack);
            }
        }

        protected void TrackHistoryState_SaveMany_Updates_Existing_Records_Correctly()
        {
            var trackHistory = SampleTrackHistoryForHistoryStates();
            var created = DateTime.UtcNow;
            var updated = created.AddSeconds(9);

            var savedStates =   SampleTrackHistoryStates(trackHistory, generateForCreate: true,  timestampUtc: created);
            var updatedStates = SampleTrackHistoryStates(trackHistory, generateForCreate: false, timestampUtc: updated);

            for(var i = 0;i < savedStates.Count;++i) {
                var saveState = savedStates[i];
                _Database.TrackHistoryState_Save(saveState);

                var updateState = updatedStates[i];
                updateState.TrackHistoryStateID = saveState.TrackHistoryStateID;
            }

            _Database.TrackHistoryState_SaveMany(updatedStates);

            for(var i = 0;i < savedStates.Count;++i) {
                var savedState = savedStates[i];
                var updatedState = updatedStates[i];

                var readBack = _Database.TrackHistoryState_GetByID(savedState.TrackHistoryStateID);
                AssertTrackHistoryStatesAreEqual(updatedState, readBack);
            }
        }

        protected void TrackHistoryState_GetByTrackHistory_Returns_Saved_IDs_In_Correct_Order()
        {
            var trackHistory = SampleTrackHistoryForHistoryStates();
            var now = DateTime.UtcNow;

            foreach(var state in new TrackHistoryState[] {
                new TrackHistoryState() { TrackHistoryID = trackHistory.TrackHistoryID, SequenceNumber = 2, TimestampUtc = now.AddSeconds(2) },
                new TrackHistoryState() { TrackHistoryID = trackHistory.TrackHistoryID, SequenceNumber = 1, TimestampUtc = now.AddSeconds(3) },
                new TrackHistoryState() { TrackHistoryID = trackHistory.TrackHistoryID, SequenceNumber = 4, TimestampUtc = now.AddSeconds(1) },
                new TrackHistoryState() { TrackHistoryID = trackHistory.TrackHistoryID, SequenceNumber = 3, TimestampUtc = now.AddSeconds(4) },
            }) {
                _Database.TrackHistoryState_Save(state);
            }

            var states = _Database.TrackHistoryState_GetByTrackHistory(trackHistory);

            // Expected wrong orders:
            // SequenceNumbers  Issue
            // ---------------  -----
            // 2, 1, 4, 3       No sorting, result is in insert order
            // 4, 2, 1, 3       TimestampUtc ascending order
            // 3, 1, 2, 4       TimestampUtc descending order
            // 4, 3, 2, 1       SequenceNumber descending order

            var expected = "1, 2, 3, 4";
            var actual = String.Join(", ", states.Select(r => r.SequenceNumber.ToString()));
            Assert.AreEqual(expected, actual, $"Was expecting order of sequence numbers to be {expected}, was actually {actual}");
        }

        private TrackHistory SampleTrackHistoryForHistoryStates(string icao = "ABC123", bool isPreserved = false, DateTime? utcNow = null)
        {
            var now = utcNow ?? DateTime.UtcNow;

            var result = new TrackHistory() {
                AircraftID = AircraftIDForIcao(icao),
                IsPreserved = isPreserved,
                CreatedUtc = now,
                UpdatedUtc = now,
            };
            _Database.TrackHistory_Save(result);

            return result;
        }

        private List<TrackHistoryState> SampleTrackHistoryStates(TrackHistory parent, bool generateForCreate, DateTime? timestampUtc = null)
        {
            var result = new List<TrackHistoryState>();

            foreach(var property in typeof(TrackHistoryState).GetProperties()) {
                var state = new TrackHistoryState() {
                    TrackHistoryID = parent.TrackHistoryID,
                };

                object value = GenerateTrackHistoryStatePropertyValue(property, generateForCreate: true);

                if(value != null) {
                    property.SetValue(state, value);

                    if(result.Count == 0) {
                        state.SequenceNumber = 1;
                    } else {
                        state.SequenceNumber = result[result.Count - 1].SequenceNumber + 1;
                    }
                    state.TimestampUtc = timestampUtc ?? DateTime.UtcNow;

                    result.Add(state);
                }
            }

            return result;
        }

        private object GenerateTrackHistoryStatePropertyValue(PropertyInfo property, bool generateForCreate)
        {
            object value = null;

            var receiver = _Database.Receiver_GetOrCreateByName(generateForCreate ? "Evelyn" : "Paulton");

            switch(property.Name) {
                case nameof(TrackHistoryState.Callsign):    value = generateForCreate ? "BAW1" : "VIR25"; break;
                case nameof(TrackHistoryState.SpeedType):   value = generateForCreate ? SpeedType.GroundSpeedReversing : SpeedType.IndicatedAirSpeed; break;
                case nameof(TrackHistoryState.ReceiverID):  value = receiver.ReceiverID; break;

                case nameof(TrackHistoryState.Latitude):
                case nameof(TrackHistoryState.Longitude):
                    value = generateForCreate ? 1.2 : 7.4;
                    break;
                case nameof(TrackHistoryState.AirPressureInHg):
                case nameof(TrackHistoryState.GroundSpeedKnots):
                case nameof(TrackHistoryState.TargetTrack):
                case nameof(TrackHistoryState.TrackDegrees):
                    value = generateForCreate ? 1.2F : 7.4F;
                    break;
                case nameof(TrackHistoryState.AltitudeFeet):
                case nameof(TrackHistoryState.SignalLevel):
                case nameof(TrackHistoryState.SquawkOctal):
                case nameof(TrackHistoryState.TargetAltitudeFeet):
                case nameof(TrackHistoryState.VerticalRateFeetMin):
                    value = generateForCreate ? 25 : 50;
                    break;
                case nameof(TrackHistoryState.IdentActive):
                case nameof(TrackHistoryState.IsCallsignSuspect):
                case nameof(TrackHistoryState.IsMlat):
                case nameof(TrackHistoryState.IsTisb):
                case nameof(TrackHistoryState.TrackIsHeading):
                    value = generateForCreate;
                    break;
                case nameof(TrackHistoryState.AltitudeType):
                case nameof(TrackHistoryState.VerticalRateType):
                    value = generateForCreate ? AltitudeType.Geometric : AltitudeType.Barometric;
                    break;

                case nameof(TrackHistoryState.SequenceNumber):
                case nameof(TrackHistoryState.TimestampUtc):
                case nameof(TrackHistoryState.TrackHistoryID):
                case nameof(TrackHistoryState.TrackHistoryStateID):
                    break;

                default:
                    throw new NotImplementedException($"Need code for {nameof(TrackHistoryState)}.{property.Name}");
            }

            return value;
        }

        private void AssertTrackHistoryStatesAreEqual(TrackHistoryState expected, TrackHistoryState actual)
        {
            TestUtilities.TestObjectPropertiesAreEqual(expected, actual);
        }

        private void AssertTrackHistoryStatesAreEqual(IEnumerable<TrackHistoryState> expected, IEnumerable<TrackHistoryState> actual)
        {
            var expectedArray = expected?.ToArray() ?? new TrackHistoryState[0];
            var actualArray =   actual?.ToArray() ?? new TrackHistoryState[0];

            Assert.AreEqual(expectedArray.Length, actualArray.Length);
            for(var i = 0;i < expectedArray.Length;++i) {
                AssertTrackHistoryStatesAreEqual(expectedArray[i], actualArray[i]);
            }
        }
        #endregion
    }
}
