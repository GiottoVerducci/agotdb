using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

using GenericDB.DataAccess;
using GenericDB.OCTGN;

using NRADB.BusinessObjects;

namespace NRADB.OCTGN
{
    public class NraOctgnLoader: OctgnLoader<NraOctgnCard>
    {
        public static Regex ProvidesRecurringCreditsRegex = new Regex(@"atTurnStart:Refill([\dX])Credits", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        public static Regex ProvidesLinkRegex = new Regex(@"whileRezzed:Gain([\dX])Base Link", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        public static Regex ProvidesMURegex = new Regex(@"whileRezzed:Gain([\dX])MU", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        public static Regex ProvidesCreditsRegex = new Regex(@"Gain([\dX])Credits", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        public static Regex TransferCreditsRegex = new Regex(@"Transfer([\dX])Credits", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private static List<string> GetIceTypes()
        {
            var rawData = ApplicationSettings.Instance.DatabaseManager.GetIceTypes();
            var result = new List<string>();
            foreach (DataRow dataRow in rawData.Rows)
            {
                result.Add(dataRow["Key"].ToString());
                result.Add(dataRow["Icebreaker"].ToString());
            }
            return result;
        }
        
        public static ImportResult ImportCards(Dictionary<OctgnSetData, NraOctgnCard[]> octgnSets, BackgroundWorker backgroundWorker)
        {
            var setTable = ApplicationSettings.Instance.DatabaseManager.GetExpansionSets();
            var iceTypes = GetIceTypes();

            var setInformations = new Dictionary<int, SetInformation>();

            var progress = 0;
            foreach (DataRow row in setTable.Rows)
            {
                if (backgroundWorker.CancellationPending)
                    return new ImportResult { IsSuccessful = false };
                backgroundWorker.ReportProgress(progress * 100 / setTable.Rows.Count, OctgnLoaderTask.ImportSet);
                var setId = (int)row["Id"];
                if (setId >= 0)
                {
                    var si = new SetInformation
                        {
                            OriginalName = row["OriginalName"].ToString(),
                            ShortName = row["ShortName"].ToString(),
                            ByChapter = (bool)row["ByChapter"],
                            SetId = (byte)row["SetId"],
                            ChaptersNames = row["ChaptersNames"].ToString().Split(',').Select(s => s.Trim()).ToArray()
                        };
                    //var name = Strip(si.OriginalName);
                    //var octgnName = octgnSets.Keys.FirstOrDefault(k => string.Equals(name, Strip(k.Name), StringComparison.InvariantCultureIgnoreCase));
                    //si.OctgnName = octgnName.Name;
                    setInformations.Add(Convert.ToInt32(row["SetId"]), si);
                }
                ++progress;
            }
            backgroundWorker.ReportProgress(100, OctgnLoaderTask.ImportSet); // match set

            var missingSets = octgnSets.Keys.Where(setData =>
                {
                    var setInfo = setInformations.FirstOrDefault(si => si.Key == setData.Id);
                    return setInfo.Value == null || (setInfo.Value.ByChapter && setInfo.Value.ChaptersNames.All(name => !string.Equals(name, setData.Name, StringComparison.InvariantCultureIgnoreCase)));
                }).OrderBy(setData => setData.Id).ThenBy(setData => setData.MinCardId).ToArray();

            if (missingSets.Length > 0)
            {
                var setUpdateOperations = missingSets
                    .Select(missingSet =>
                        {
                            var setInfo = setInformations.FirstOrDefault(si => si.Key == missingSet.Id);
                            if (setInfo.Value == null)
                                return new Action<DataRowCollection>(rows => rows.Add(missingSet.Id - 1, missingSet.Name, missingSet.Id, true, true, missingSet.Name, missingSet.Name));
                            return new Action<DataRowCollection>(rows =>
                                {
                                    foreach (DataRow dataRow in rows)
                                    {
                                        if ((Int32)dataRow["Id"] == -1 || (byte)dataRow["SetId"] != missingSet.Id)
                                            continue;
                                        var chapterNames = dataRow["ChaptersNames"].ToString().Split(',').Select(n => n.Trim()).ToList();
                                        chapterNames.Add(missingSet.Name);
                                        dataRow["ChaptersNames"] = string.Join(", ", chapterNames);
                                        setInfo.Value.ChaptersNames = chapterNames.ToArray();
                                    }
                                });
                        })
                    .ToArray();
                ApplicationSettings.Instance.DatabaseManager.UpdateSets(setUpdateOperations);
            }

            var allCards = new List<KeyValuePair<OctgnSetData, NraOctgnCard>>();
            foreach (var kvp in octgnSets)
                allCards.AddRange(kvp.Value.Select((card => new KeyValuePair<OctgnSetData, NraOctgnCard>(kvp.Key, card))));
            allCards = allCards.OrderBy(kvp =>
                {
                    int setId, cardId;
                    ExtractSetIdAndCardIdFromOctgnId(kvp.Value.Id, out setId, out cardId);
                    return GetUniversalId(setId, cardId);
                }).ToList();

            var cursor = (object)0;
            object errorObject = null;

            ApplicationSettings.Instance.DatabaseManager.ResetAndImportCards(destinationRows =>
                {
                    if (backgroundWorker.CancellationPending)
                        return DatabaseManager.OperationResult.Abort;

                    var index = (int)cursor;
                    if (index >= allCards.Count)
                    {
                        backgroundWorker.ReportProgress(100, OctgnLoaderTask.ImportCard);
                        backgroundWorker.ReportProgress(66, OctgnLoaderTask.UpdateDatabase); // arbitrary, we don't get progress notification when the dataset is updated 
                        // nb: we could using OnRowUpdated according to http://stackoverflow.com/questions/15889694/dataadapter-update-method-progress-status
                        return DatabaseManager.OperationResult.Done;
                    }

                    var card = allCards[index].Value;
                    cursor = index + 1;

                    var keywords = card.Keywords.Split('-').Where(k => !string.IsNullOrEmpty(k)).ToList();
                    bool isUnique = keywords.Contains("Unique");

                    int setId, cardId;
                    ExtractSetIdAndCardIdFromOctgnId(card.Id, out setId, out cardId);

                    var stat = card.Stat == "-" ? string.Empty : card.Stat;
                    var strength = string.Equals(card.Type, "ICE", StringComparison.InvariantCultureIgnoreCase)
                                   || string.Equals(card.Type, "Program", StringComparison.InvariantCultureIgnoreCase)
                                       ? (card.Stat == "-" ? string.Empty : (string.IsNullOrEmpty(card.Stat) ? "0" : card.Stat))
                                       : string.Empty;
                    var agendaPoints = string.Equals(card.Type, "Agenda", StringComparison.InvariantCultureIgnoreCase)
                                           ? stat
                                           : string.Empty;
                    var link = string.Equals(card.Type, "Identity", StringComparison.InvariantCultureIgnoreCase)
                               && string.Equals(card.Side, "Runner", StringComparison.InvariantCultureIgnoreCase)
                               && !string.Equals(card.Cost.Trim(), "0")
                                   ? card.Cost
                                   : string.Empty;
                    var trashCost = string.Equals(card.Type, "Asset", StringComparison.InvariantCultureIgnoreCase)
                                        ? stat
                                        : string.Empty;

                    var influence = card.Influence == "-" ? string.Empty : card.Influence;
                    if (string.Equals(card.Type, "Identity", StringComparison.InvariantCultureIgnoreCase))
                        influence = stat;

                    var mu = string.Equals(card.Type, "Program", StringComparison.InvariantCultureIgnoreCase)
                                 ? card.Requirement
                                 : string.Empty;
                    var deckSize = string.Equals(card.Type, "Identity", StringComparison.InvariantCultureIgnoreCase)
                                       ? card.Requirement
                                       : string.Empty;

                    var iceType = string.Join("/", iceTypes.Where(keywords.Contains));

                    if (!setInformations.ContainsKey(allCards[index].Key.Id))
                    {
                        errorObject = allCards[index].Key;
                        return DatabaseManager.OperationResult.Abort;
                    }

                    var setInformation = setInformations[allCards[index].Key.Id];
                    var set = setInformation.ByChapter
                                  ? string.Format("{0}-{1}({2})", setInformation.ShortName, setInformation.GetChapterId(allCards[index].Key.Name), cardId)
                                  : string.Format("{0}({1})", setInformation.ShortName, cardId);

                    var recurringMatches = ProvidesRecurringCreditsRegex.Matches(card.AutoScript);
                    string recurringCredits = recurringMatches.Count > 0
                                                  ? recurringMatches[0].Groups[1].ToString()
                                                  : null;

                    if (link.Length == 0)
                    {
                        var linkMatches = ProvidesLinkRegex.Matches(card.AutoScript);
                        link = linkMatches.Count > 0
                                   ? linkMatches[0].Groups[1].ToString()
                                   : null;
                    }

                    var muMatches = ProvidesMURegex.Matches(card.AutoScript);
                    string providesMU = muMatches.Count > 0
                                            ? muMatches[0].Groups[1].ToString()
                                            : null;

                    var creditMatches = ProvidesCreditsRegex.Matches(card.AutoScript);
                    string creditsIncome = creditMatches.Count > 0
                                               ? creditMatches[0].Groups[1].ToString()
                                               : null;

                    if (creditsIncome == null)
                    {
                        creditMatches = TransferCreditsRegex.Matches(card.AutoScript);
                        creditsIncome = creditMatches.Count > 0
                                            ? creditMatches[0].Groups[1].ToString()
                                            : null;
                    }

                    //var cleanName = new string(card.Name.Where(c => char.IsLetterOrDigit(c) || char.IsSeparator(c) || char.IsPunctuation(c)).ToArray());
                    //if (!string.Equals(cleanName, card.Name))
                    //{
                    //    Debugger.Break();
                    //}

                    destinationRows.Add(
                        index,
                        card.Name,
                        card.Subtitle,
                        card.Side,
                        card.Type,
                        card.Faction,
                        isUnique ? "Yes" : "No",
                        string.Join(". ", keywords) + (keywords.Count > 0 ? "." : null),
                        card.Text,
                        card.Instructions,
                        card.Cost,
                        stat,
                        strength,
                        agendaPoints,
                        link,
                        trashCost,
                        influence,
                        card.Requirement,
                        mu,
                        deckSize,
                        set,
                        card.Name,
                        (GetUniversalId(setId, cardId)).ToString(),
                        "",//banned
                        "",//restricted
                        card.Id,
                        card.Flavor,
                        recurringCredits,
                        creditsIncome,
                        providesMU,
                        iceType
                        );

                    backgroundWorker.ReportProgress((index * 100) / allCards.Count, OctgnLoaderTask.ImportCard);
                    return DatabaseManager.OperationResult.Ok;
                });
            if (backgroundWorker.CancellationPending)
                return new ImportResult { IsSuccessful = false };

            if (errorObject is OctgnSetData)
                return new ImportResult { IsSuccessful = false, SetNotFound = errorObject as OctgnSetData };

            backgroundWorker.ReportProgress(100, OctgnLoaderTask.UpdateDatabase);
            return new ImportResult { IsSuccessful = true };
        }
    }
}