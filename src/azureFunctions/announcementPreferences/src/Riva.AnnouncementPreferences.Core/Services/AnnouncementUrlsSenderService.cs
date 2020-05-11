using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Cosmonaut;
using Cosmonaut.Extensions;
using Microsoft.Azure.CosmosDB.BulkExecutor;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Riva.AnnouncementPreferences.Core.Constants;
using Riva.AnnouncementPreferences.Core.Entities;
using Riva.AnnouncementPreferences.Core.Enums;
using Riva.AnnouncementPreferences.Core.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public class AnnouncementUrlsSenderService : IAnnouncementUrlsSenderService
    {
        private readonly ICosmosStore<FlatForRentAnnouncementPreference> _flatForRentAnnouncementPreferenceCosmosStore;
        private readonly ICosmosStore<RoomForRentAnnouncementPreference> _roomForRentAnnouncementPreferenceCosmosStore;
        private readonly IBulkExecutor _bulkExecutor;
        private readonly ISendGridClient _sendGridClient;
        private readonly int _maxDegreeOfParallelism;
        private readonly ExecutionDataflowBlockOptions _executionDataflowBlockOptions;
        private readonly ParallelOptions _parallelOptions;

        public AnnouncementUrlsSenderService(ICosmosStore<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferenceCosmosStore, 
            ICosmosStore<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferenceCosmosStore, IBulkExecutorInitializer bulkExecutorInitializer,
            ISendGridClient sendGridClient, IOptions<AppSettings> options)
        {
            _flatForRentAnnouncementPreferenceCosmosStore = flatForRentAnnouncementPreferenceCosmosStore;
            _roomForRentAnnouncementPreferenceCosmosStore = roomForRentAnnouncementPreferenceCosmosStore;
            _bulkExecutor = Task.Run(bulkExecutorInitializer.InitializeBulkExecutorAsync).Result;
            _sendGridClient = sendGridClient;
            _maxDegreeOfParallelism = Convert.ToInt32(options.Value.MaxDegreeOfParallelism);
            _executionDataflowBlockOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism };
            _parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism };
        }

        public async Task SendAnnouncementUrlsAsync(AnnouncementSendingFrequency announcementSendingFrequency)
        {
            var groupedAnnouncementPreferences = await GetGroupedAnnouncementPreferencesAsync(announcementSendingFrequency);
            var actionBlock = ConstructActionBlock();
            var tasks = new List<Task>();

            foreach (var groupedAnnouncementPreference in groupedAnnouncementPreferences)
            {
                tasks.Add(actionBlock.SendAsync(groupedAnnouncementPreference));
            }

            await Task.WhenAll(tasks);
            actionBlock.Complete();
            await ClearSentAnnouncementUrlsAsync(groupedAnnouncementPreferences);
        }

        private async Task<List<GroupedAnnouncementPreferences>> GetGroupedAnnouncementPreferencesAsync(AnnouncementSendingFrequency announcementSendingFrequency)
        {
            var groupedFlatFoRentAnnouncementPreferencesTask = _flatForRentAnnouncementPreferenceCosmosStore.Query()
                .Where(x => x.ServiceActive && x.AnnouncementSendingFrequency == announcementSendingFrequency)
                .GroupBy(x => x.UserId)
                .Select(x => new GroupedFlatForRentAnnouncementPreferences(x.Key, x.ToList()))
                .ToListAsync();
            var groupedRoomFoRentAnnouncementPreferencesTask = _roomForRentAnnouncementPreferenceCosmosStore.Query()
                .Where(x => x.ServiceActive && x.AnnouncementSendingFrequency == announcementSendingFrequency)
                .GroupBy(x => x.UserId)
                .Select(x => new GroupedRoomForRentAnnouncementPreferences(x.Key, x.ToList()))
                .ToListAsync();
            await Task.WhenAll(groupedFlatFoRentAnnouncementPreferencesTask, groupedRoomFoRentAnnouncementPreferencesTask);
            var groupedFlatFoRentAnnouncementPreferences = groupedFlatFoRentAnnouncementPreferencesTask.Result;
            var groupedRoomFoRentAnnouncementPreferences = groupedRoomFoRentAnnouncementPreferencesTask.Result;

            return
                (from groupedFlatFoRentAnnouncementPreference in groupedFlatFoRentAnnouncementPreferences
                    join groupedRoomForRentAnnouncementPreference in groupedRoomFoRentAnnouncementPreferences on
                        groupedFlatFoRentAnnouncementPreference.UserId equals
                        groupedRoomForRentAnnouncementPreference.UserId into groupedAnnouncementPreferences
                    from sub in groupedAnnouncementPreferences
                    select new GroupedAnnouncementPreferences(
                        groupedFlatFoRentAnnouncementPreference.FlatForRentAnnouncementPreferences.First().UserEmail,
                        groupedFlatFoRentAnnouncementPreference.FlatForRentAnnouncementPreferences,
                        sub.RoomForRentAnnouncementPreferences)) as List<GroupedAnnouncementPreferences>;
        }

        private ActionBlock<GroupedAnnouncementPreferences> ConstructActionBlock()
        {
            return new ActionBlock<GroupedAnnouncementPreferences>(async groupedAnnouncementPreferences =>
            {
                var sb = FormAnnouncementUrlStringBuilder(groupedAnnouncementPreferences);
                if (sb.Capacity > 0)
                {
                    var sendGridMessage = MailHelper.CreateSingleEmail(new EmailAddress(ConstantVariables.RivaEmailAddress),
                        new EmailAddress(groupedAnnouncementPreferences.UserEmail), ConstantVariables.EmailSubject, string.Empty,
                        sb.ToString());
                    await _sendGridClient.SendEmailAsync(sendGridMessage);
                }

            }, _executionDataflowBlockOptions);
        }

        private static StringBuilder FormAnnouncementUrlStringBuilder(GroupedAnnouncementPreferences groupedAnnouncementPreferences)
        {
            var sb = new StringBuilder();
            if (groupedAnnouncementPreferences.FlatForRentAnnouncementPreferences.Any())
            {
                sb.Append("Flat fo rent announcement urls:").AppendLine();

                foreach (var announcementUrlsToSend in groupedAnnouncementPreferences.FlatForRentAnnouncementPreferences.SelectMany(x => x.AnnouncementUrlsToSend))
                {
                    sb.Append(announcementUrlsToSend).AppendLine();
                }
                sb.AppendLine().AppendLine();
            }
            if (groupedAnnouncementPreferences.RoomForRentAnnouncementPreferences.Any())
            {
                sb.Append("Room for rent announcement urls:").AppendLine();

                foreach (var announcementUrlsToSend in groupedAnnouncementPreferences.RoomForRentAnnouncementPreferences.SelectMany(x => x.AnnouncementUrlsToSend))
                {
                    sb.Append(announcementUrlsToSend).AppendLine();
                }
                sb.AppendLine().AppendLine();
            }

            return sb;
        }

        private async Task ClearSentAnnouncementUrlsAsync(ICollection<GroupedAnnouncementPreferences> groupedAnnouncementPreferences)
        {
            var processedAnnouncementPreferenceIds = new HashSet<Guid>();
            Parallel.ForEach(
                groupedAnnouncementPreferences.SelectMany(x => x.FlatForRentAnnouncementPreferences),
                _parallelOptions,
                flatForRentAnnouncementPreference => ProcessAnnouncementPreference(flatForRentAnnouncementPreference, processedAnnouncementPreferenceIds));
            Parallel.ForEach(
                groupedAnnouncementPreferences.SelectMany(x => x.RoomForRentAnnouncementPreferences),
                _parallelOptions,
                roomForRentAnnouncementPreference => ProcessAnnouncementPreference(roomForRentAnnouncementPreference, processedAnnouncementPreferenceIds));

            var documents = groupedAnnouncementPreferences.SelectMany(x => x.FlatForRentAnnouncementPreferences).Select(JsonConvert.SerializeObject).ToList();
            documents.AddRange(groupedAnnouncementPreferences.SelectMany(x => x.RoomForRentAnnouncementPreferences).Select(JsonConvert.SerializeObject));
            await _bulkExecutor.BulkImportAsync(documents, true, true, _maxDegreeOfParallelism);
        }

        private static void ProcessAnnouncementPreference(IAnnouncementPreference announcementPreference, HashSet<Guid> processedAnnouncementPreferenceIds)
        {
            lock (processedAnnouncementPreferenceIds)
            {
                if (!processedAnnouncementPreferenceIds.Add(announcementPreference.Id))
                    return;
            }

            announcementPreference.AnnouncementUrlsToSend = new List<string>();
        }

        private class GroupedFlatForRentAnnouncementPreferences
        {
            public Guid UserId { get; }
            public IEnumerable<FlatForRentAnnouncementPreference> FlatForRentAnnouncementPreferences { get; }

            public GroupedFlatForRentAnnouncementPreferences(Guid userId, IEnumerable<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferences)
            {
                UserId = userId;
                FlatForRentAnnouncementPreferences = flatForRentAnnouncementPreferences;
            }
        }

        private class GroupedRoomForRentAnnouncementPreferences
        {
            public Guid UserId { get; }
            public IEnumerable<RoomForRentAnnouncementPreference> RoomForRentAnnouncementPreferences { get; }

            public GroupedRoomForRentAnnouncementPreferences(Guid userId, IEnumerable<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferences)
            {
                UserId = userId;
                RoomForRentAnnouncementPreferences = roomForRentAnnouncementPreferences;
            }
        }

        private class GroupedAnnouncementPreferences
        {
            public string UserEmail { get; }
            public ICollection<FlatForRentAnnouncementPreference> FlatForRentAnnouncementPreferences { get; }
            public ICollection<RoomForRentAnnouncementPreference> RoomForRentAnnouncementPreferences { get; }

            public GroupedAnnouncementPreferences(string userEmail, 
                IEnumerable<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferences, 
                IEnumerable<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferences)
            {
                UserEmail = userEmail;
                FlatForRentAnnouncementPreferences = flatForRentAnnouncementPreferences.ToList();
                RoomForRentAnnouncementPreferences = roomForRentAnnouncementPreferences.ToList();
            }
        }
    }
}