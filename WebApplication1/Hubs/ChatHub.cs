using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CoreMVC_SignalR_Chat.Hubs
{
    
    public class ChatHub : Hub
    {
        public static Dictionary<string, string> memberToConnectionMap = new Dictionary<string, string>();

        // 用戶連線 ID 列表
        public static List<string> ConnIDList = new List<string>();

        /// <summary>
        /// 連線事件
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var memberId = Context.User.Identity.Name ?? Context.ConnectionId; // 如果會員 ID 為空，則使用連線 ID;

            if (!memberToConnectionMap.ContainsKey(memberId))
            {
                memberToConnectionMap.Add(memberId, Context.ConnectionId);
                ConnIDList.Add(memberId); // 確保將會員 ID 添加到 ConnIDList
            }

           
            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConnIDList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新個人 ID
            await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID",memberId);

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "新連線 ID: " +memberId);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var memberId = Context.User.Identity.Name;

            if (memberToConnectionMap.ContainsKey(memberId))
            {
                memberToConnectionMap.Remove(memberId);
            }
            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(memberToConnectionMap.Keys);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "已離線 ID: " +memberId);

            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SendMessage(string selfID, string message, string sendToID)
        {
            if (selfID == sendToID)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "不能給自己發送消息。");
                return;
            }


            if (string.IsNullOrEmpty(sendToID))
            {
                await Clients.All.SendAsync("UpdContent", selfID + " 說: " + message);
            }
            else
            {
                if(memberToConnectionMap.TryGetValue(sendToID, out var sendToConnectionId))
                {               
                  // 接收人
                  await Clients.Client(sendToConnectionId).SendAsync("UpdContent", selfID + " 私訊向你說: " + message);

                  // 發送人
                  await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "你向 " + sendToID + " 私訊說: " + message);

                }
                else
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "無法找到用戶: " + sendToID);
                }
                //var sendToConnectionId = memberToConnectionMap[sendToID];
                // 儲存訊息到資料庫
             
            }
        }
        public async Task<string> GetMemberId()
        {
            // 假設會員 ID 存儲在用戶的認證中
            var memberId = Context.User.Identity.Name;
            return await Task.FromResult(memberId);
        }
    }
}