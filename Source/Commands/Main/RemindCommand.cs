using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

namespace HBot.Commands.Main {
    public class RemindCommand : BaseCommandModule {
        private static ConcurrentDictionary<ulong, List<Tuple<string, DateTime>>> reminders = new ConcurrentDictionary<ulong, List<Tuple<string, DateTime>>>();

        [Command("remind")]
        [Description("Remind you about something or list your reminders")]
        [Usage("[list] or [Time] [Time Unit (minutes/m, hours/h, days/d, years/y)] [Message] (Note that long timespans are likely unreliable due to bot restarts)]")]
        [Category(Category.Main)]
        public async Task Remind(CommandContext Context, string timeStr, [RemainingText] string message = "") {
            // List reminders
            if (timeStr.ToLower() == "list") {
                if (reminders.TryGetValue(Context.User.Id, out var userReminders) && userReminders.Count > 0) {
                    var reminderList = string.Join("\n", userReminders.Select((reminder, index) => $"{index + 1}. {reminder.Item2}: {reminder.Item1}"));
                    await Context.RespondAsync($"Here are your reminders:\n{reminderList}");
                }
                else {
                    await Context.RespondAsync("You have no reminders.");
                }

                return;
            }
			
			TimeSpan timeSpan;
			TimeSpan maxDelay = TimeSpan.FromDays(24); //workaround for Task.Delay limitations

            if (timeStr.EndsWith("s")) {
                if (!int.TryParse(timeStr.TrimEnd('s'), out int seconds)) {
                    await Context.RespondAsync("Invalid format for seconds. Please provide a number.");
                    return;
                }

                timeSpan = TimeSpan.FromSeconds(seconds);
            }
            else if (timeStr.EndsWith("m")) {
                if (!int.TryParse(timeStr.TrimEnd('m'), out int minutes)) {
                    await Context.RespondAsync("Invalid format for minutes. Please provide a number.");
                    return;
                }

                timeSpan = TimeSpan.FromMinutes(minutes);
            }
            else if (timeStr.EndsWith("h")) {
                if (!int.TryParse(timeStr.TrimEnd('h'), out int hours)) {
                    await Context.RespondAsync("Invalid format for hours. Please provide a number.");
                    return;
                }

                timeSpan = TimeSpan.FromHours(hours);
            }
            else if (timeStr.EndsWith("d")) {
                if (!int.TryParse(timeStr.TrimEnd('d'), out int days)) {
                    await Context.RespondAsync("Invalid format for days. Please provide a number.");
                    return;
                }

                timeSpan = TimeSpan.FromDays(days);
            }
            else if (timeStr.EndsWith("y")) {
                if (!int.TryParse(timeStr.TrimEnd('y'), out int years)) {
                    await Context.RespondAsync("Invalid format for years. Please provide a number.");
                    return;
                }

                timeSpan = TimeSpan.FromDays(years * 365);
            }
            else {
                await Context.RespondAsync("Invalid time format. Please use m for minutes, h for hours, d for days, y for years.");
                return;
            }

            await Context.RespondAsync($"Reminder set for {timeStr} from now.");

            
            var reminderTime = DateTime.Now + timeSpan;
            var reminder = Tuple.Create(message, reminderTime);
            reminders.AddOrUpdate(Context.User.Id, new List<Tuple<string, DateTime>> { reminder }, (key, oldList) => { oldList.Add(reminder); return oldList; });

			// workaround for Task.Delay limitations
            while (timeSpan > maxDelay) {
				await Task.Delay(maxDelay);
				timeSpan -= maxDelay;
			}
			await Task.Delay(timeSpan);

            // Send the reminder
            await Context.RespondAsync($"{Context.User.Mention}: {message}");

            // Remove the reminder from the list
            if (reminders.TryGetValue(Context.User.Id, out var updatedUserReminders)) {
                updatedUserReminders.Remove(reminder);
                if (updatedUserReminders.Count == 0) {
                    reminders.TryRemove(Context.User.Id, out _);
                }
            }
        }
    }
}
