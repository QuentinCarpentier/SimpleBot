using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace SimpleBot.Dialogs
{
    [Serializable]
    public class SimpleDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi, I'm just the most simple bot ever !");
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            // IAwaitable > variable can be awaited
            var message = await result;
            // Username is empty at the beginning
            var userName = String.Empty;
            // Flag to know if the name is cached
            var getName = false;

            context.UserData.TryGetValue("Name", out userName);
            context.UserData.TryGetValue("GetName", out getName);

            // If getName is true, it means we have to get the name by the text value and put it in the UserData bag
            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue("Name", userName);
                // Set the flag to false
                context.UserData.SetValue("GetName", false);
            }

            // If the name is not cached yet
            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name?");
                // Set the flag to true, we need the name
                context.UserData.SetValue("GetName", true);
            }
            // If the name is cached
            else
            {
                await context.PostAsync(String.Format("How are you today, {0}", userName));
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}