using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using prjBlog.Models;
using prjBlog.ViewModel;
using SlnBlogAndShop.Models;

namespace SlnBlogAndShop
{
    public class BlogHub : Hub
    {
        public static List<TMember> UserList { get; private set; }
        happyEntities dbContext = new happyEntities();
        private object flag = new object();
        public BlogHub()
        {
            if (BlogHub.UserList == null)
                BlogHub.UserList = new List<TMember>();
        }
        //登入時自動被呼叫
        public override Task OnConnected()
        {
            lock (flag)
            {
                if (!BlogHub.UserList.Where(c => c.fSignalRUserId == this.Context.ConnectionId).Any())
                    BlogHub.UserList.Add(new TMember()
                    {
                        fSignalRUserId = this.Context.ConnectionId,
                    });
            }
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            signalRModels sm = new signalRModels();
            BlogHub.UserList.Remove(
                BlogHub.UserList.Where(c => c.fSignalRUserId == this.Context.ConnectionId).First());

            //User login change login status to all.
            
            List<TMember> list = sm.getFriendListWithUserListById(sm.getMemberIdByConnectionId(this.Context.ConnectionId));
            string friendList = System.Text.Json.JsonSerializer.Serialize(sm.postJsonlistByFriendlist(list));
            Clients.All.putFriendStatusToClient(friendList);
            return base.OnDisconnected(stopCalled);
        }
        //變更使用者登入hub的資訊
        public void putUserInfo(string memberId)
        {
            // Call the changeLogInStatus method to update clients.
            signalRModels sm = new signalRModels();
            //Already used onConnected(), sm.addUserToUserList(memberId,this.Context.ConnectionId);
            //Update userInfo for fMemberId and fUserName.
            sm.putUserInfoByConnectionId(this.Context.ConnectionId, memberId);

            //User login change login status to all.
            List<TMember> list = sm.getFriendListWithUserListById(int.Parse(memberId));
            string friendList = System.Text.Json.JsonSerializer.Serialize(sm.postJsonlistByFriendlist(list));
            Clients.All.putFriendStatusToClient(friendList);
        }
        //提供朋友清單給前端，前端使用fSignalRUserId判斷使用者是否上線
        public void getFriendStatusByfMemberId(string memberId)
        {
            signalRModels sm = new signalRModels();
            List<TMember> list = sm.getFriendListWithUserListById(int.Parse(memberId));
            string friendList = System.Text.Json.JsonSerializer.Serialize(sm.postJsonlistByFriendlist(list));
            //在原使用者端呼叫方法並傳jason格式string
            Clients.Client(this.Context.ConnectionId).putFriendStatusToClient(friendList); 
        }

        //TODO:group
        public Task JoinRoom(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }
        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }


        //暫時用不到
        
    }
}