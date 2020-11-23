using prjBlog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlnBlogAndShop.Models
{
    public class signalRModels
    {
        happyEntities dbContext = new happyEntities();
        //Goto db and use memberId to get tMemebr.
        public tMember getMemberInfoById(string memberId)
        {
            var q = (from m in this.dbContext.tMembers
                     where m.fMemberId.ToString() == memberId
                     select m).FirstOrDefault();
            return q;
        }
        //Get hub userList and connectionId to get userNo in list.
        public TMember getSignalrUserById(List<TMember> list,string connectionId)
        {
            var q = from u in list
                     where u.fSignalRUserId == connectionId
                     select u;
            return q.FirstOrDefault();
        }
        //get朋友清單(因頁面載入後才登入hub，故無法一開始就取得上線狀態)
        public List<TMember> getFriendListById(int fMemberId)
        {
            List<TMember> list = new List<TMember>();
            //tMember跟tFriendinvite inner join get fMemberId的朋友
            var q = from m in this.dbContext.tMembers
                     join f in this.dbContext.tFriendinvites
                     on m.fMemberId equals f.fBefriendId
                     where f.fFriendId == fMemberId && f.fStatus == true
                     select m;
            //轉換tMember to TMember
            foreach (var item in q)
            {
                TMember mem = new TMember();
                mem.fMemberId = item.fMemberId;
                mem.fName = item.fName;
                mem.fIdPhoto = item.fIdPhoto;
                list.Add(mem);
            }
            return list;
        }

        internal int getMemberIdByConnectionId(string connectionId)
        {
            var q = (from l in BlogHub.UserList
                    where l.fSignalRUserId == connectionId
                    select l).FirstOrDefault();
            return q.fMemberId;
        }

        //get朋友清單,且加入上線狀態
        public List<TMember> getFriendListWithUserListById(int fMemberId)
        {
            List<TMember> list = new List<TMember>();
            //tMember跟tFriendinvite inner join get fMemberId的朋友
            var q = from m in this.dbContext.tMembers
                    join f in this.dbContext.tFriendinvites
                    on m.fMemberId equals f.fBefriendId
                    where f.fFriendId == fMemberId && f.fStatus == true
                    select m;
            //轉換tMember to TMember,並加入hub中的fSignalRUserId 以便view中顯示好友上線狀態
            foreach (var item in q)
            {
                TMember mem = new TMember();
                mem.fMemberId = item.fMemberId;
                mem.fName = item.fName;
                mem.fIdPhoto = item.fIdPhoto;
                //引入hub資料UserList
                foreach (var hub in BlogHub.UserList)
                {
                    if (item.fMemberId == hub.fMemberId)
                        mem.fSignalRUserId = hub.fSignalRUserId;
                }
                list.Add(mem);
            }
            return list;
        }
        //更新User登入資訊 fMemberId和userName
        public void putUserInfoByConnectionId(string connectionId,string memberId)
        {
            var tMem = getMemberInfoById(memberId);
            if (!string.IsNullOrEmpty(tMem.fName))
            {
                var TMem = getSignalrUserById(BlogHub.UserList, connectionId);
                if (!string.IsNullOrEmpty(TMem.fSignalRUserId))
                {
                    TMem.fMemberId = tMem.fMemberId;
                    TMem.fName = tMem.fName;
                    TMem.fIdPhoto = tMem.fIdPhoto;
                }
            }
        }

        public List<string> postJsonlistByFriendlist(List<TMember> list)
        {
            List<string> jsonlist = new List<string>();
            string fSignalRUserId;
            foreach (var item in list)
            {
                string str = "";
                if (item.fSignalRUserId == null)
                {
                    fSignalRUserId = "0";
                }
                else
                {
                    fSignalRUserId = item.fSignalRUserId;
                }
                str += "{fMemberId:" + item.fMemberId + ",fName:" + item.fName + ",fSignalRUserId:" + fSignalRUserId + "}";
                jsonlist.Add(str);
            }
            return jsonlist;
        }
        //已有onConnected暫不使用
        //public void postUserToUserList(string memberId, string connectedId)
        //{
        //    var tMem = getMemberInfoById(memberId);
        //    TMember TMem = new TMember();
        //    TMem.fMemberId = tMem.fMemberId;
        //    TMem.fName = tMem.fName;
        //    TMem.fSignalRUserId = connectedId;
        //    BlogHub.UserList.Add(TMem);
        //}
        //已有disConnected暫不使用
        //public void deleteUserToUserList(string memberId)
        //{
        //    BlogHub.UserList.RemoveAt(getIndexOfUserList(memberId));
        //}
        //public int getIndexOfUserList(string memberId)
        //{
        //    for (var i = 0; i < BlogHub.UserList.Count; i++)
        //    {
        //        if (memberId == BlogHub.UserList[i].fMemberId.ToString())
        //        {
        //            return i;
        //        }
        //    }
        //    return 0;
        //}


    }
}