using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using prjMusicBetter.Models;

namespace CoreMVC_SignalR_Chat.Hubs
{
    
    public class ChatHub : Hub
    {
        private readonly dbSoundBetterContext _context;

        public ChatHub(dbSoundBetterContext context)
        {
            _context = context;
        }

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

        public async Task SaveMessageToDatabase(string senderId, string receiverId, string message)
        {
            // 這裡假設您有一個服務或存儲庫來處理數據庫操作
            var chatMessage = new TChatMessage
            {
                FSendMemberId = int.Parse(senderId),
                FReceiveMemberId = int.Parse(receiverId),
                FContent = message,
                FTime = DateTime.UtcNow // 請使用適當的時間戳
            };
            try
            {
                _context.TChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // 處理錯誤，例如記錄到日誌檔
                Console.WriteLine(ex.Message);
                throw; // 或者處理例外情況，不要再拋出
            }

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

            // 如果 sendToID 為空，發送給所有連線的客戶端
            if (string.IsNullOrEmpty(sendToID))
            {
                await Clients.All.SendAsync("UpdContent", selfID + " 說: " + message);
            }

            else // 否則是私訊
            {
                if(memberToConnectionMap.TryGetValue(sendToID, out var sendToConnectionId))
                {               
                  // 接收人
                  await Clients.Client(sendToConnectionId).SendAsync("UpdContent", selfID + " 私訊向你說: " + message);

                  // 發送人
                  await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "你向 " + sendToID + " 私訊說: " + message);

                    // 儲存訊息到資料庫
                   await SaveMessageToDatabase(selfID, sendToID, message);

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