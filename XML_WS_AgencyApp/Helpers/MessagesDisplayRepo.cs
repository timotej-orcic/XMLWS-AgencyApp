using System.Collections.Generic;
using System.Linq;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class MessagesDisplayRepo
    {
        public List<MessageViewModel> GetMessages(long currentUserId)
        {
            using (var ctx = new ApplicationDbContext())
            {                
                List<Message> messages = ctx.Messages
                    .Include("RegisteredUserInfo")
                        .Where(x => x.AgentUserId == currentUserId)
                            .OrderBy(x => x.RegisteredUserInfo.UserName)
                                .ToList();

                List<MessageViewModel> retList = new List<MessageViewModel>();
                if(messages.Count > 0)
                {
                    foreach (var m in messages)
                    {
                        retList.Add(
                            new MessageViewModel
                            {
                                Id = m.Id,
                                SenderUserName = m.RegisteredUserInfo.UserName,
                                IsRead = m.IsRead
                            }
                        );
                    }
                }

                return retList;
            }
        }

        public OpenedMessageViewModel GetOpennedMessage(long messageId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                Message msg = ctx.Messages
                    .Include("RegisteredUserInfo")
                        .Include("ResponseMessage")
                            .FirstOrDefault(x => x.Id == messageId);

                if(!msg.IsRead)
                {
                    msg.IsRead = true;
                    ctx.SaveChanges();
                }

                OpenedMessageViewModel retVM = null;
                if (msg.ResponseMessage == null)
                {
                    retVM = new OpenedMessageViewModel
                    {
                        Id = msg.Id,
                        SenderId = msg.RegisteredUserInfo.Id,
                        Content = msg.Content,
                        HasResponse = msg.HasResponse,
                        SenderUserName = msg.RegisteredUserInfo.UserName
                    };
                }
                else
                {
                    retVM = new OpenedMessageViewModel
                    {
                        Content = msg.Content,
                        HasResponse = msg.HasResponse,
                        SenderUserName = msg.RegisteredUserInfo.UserName,
                        ResponseContent = msg.ResponseMessage.Content
                    };
                }

                return retVM;
            }
        }
    }
}