using System;
using System.Collections.Generic;
using System.Text;

namespace Enemies.Parameters
{
    class MessageManager
    {
        private static List<Message> messages;

        public static void SendMessage(int sender, int receiver, string message, object attachment)
        {
            if (sender == receiver) return;

            if(messages == null)
            {
                messages = new List<Message>();
            }

            messages.Add(new Message(sender, receiver, message, attachment));
        }

        public static void ClearMessages()
        {
            messages = new List<Message>();
        }

        public static void ProcessMessages(Action<Message> action)
        {
            while(messages.Count != 0)
            {
                Message item = messages[0];
                action.Invoke(item);
                messages.Remove(item);
            }
        }
    }
}
