using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using prjBlog.Models;
using prjBlog.ViewModel;
using SlnBlogAndShop;
using SlnBlogAndShop.Models;
using SlnBlogAndShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Text.Json;


namespace prjBlog.Controllers
{
    public class BlogController : Controller
    {
        happyEntities db = new happyEntities();
        // GET: Home
        public ActionResult happyLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult happyLogin(CLoginViewModel login)
        {
            login.txtAccount = Request.Form["txtUsername"];
            login.txtPassword = Request.Form["txtPassword"];
            //CCustomer cust = (new CCustomerFactory()).isAuthticated(login.txtAccount, login.txtPassword); 原始範例碼(改成下面那行)
            TMember cust = (new MemberAuthtication()).isAuthticated(login.txtAccount, login.txtPassword);

            if (cust != null)
            {
                Session[CDictionary.SK_LOGINED_USER] = cust;
                TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
                int fid = member.fMemberId;

                return RedirectToAction("bloggerIndex", "Blog");
            }

            return View();
        }
        public ActionResult log_out()
        {
            Session[CDictionary.SK_LOGINED_USER] = null;
            return RedirectToAction("happyLogin", "Blog");
        }


        public ActionResult personalPage()
        {
            if (Session[CDictionary.SK_LOGINED_USER] == null)
            {
                return RedirectToAction("happyLogin", "Blog");
            }
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int count = db.tNotifies.Where(n => n.fHasread == false && n.fMemberId == member.fMemberId).Count();
            ViewBag.count = count;

            return View();
        }


        [HttpPost]
        public ActionResult personalPage(string changePersonalphoto)
        {
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            var base64Str = changePersonalphoto.Split(',')[1];
            string photoName = null;

            byte[] bytes = Convert.FromBase64String(base64Str);

            photoName = Guid.NewGuid().ToString() + ".jpg";
            var path = Path.Combine(Server.MapPath("~/images/IdPhoto"), photoName);
            using (Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                image.Save(path, ImageFormat.Jpeg);  // Or Png
            }

            tMember q = db.tMembers.Where(m => m.fEmail == member.fEmail && m.fPassword == member.fPassword).FirstOrDefault();
            q.fIdPhoto = photoName;
            db.SaveChanges();
            member.fIdPhoto = photoName;
            return RedirectToAction("personalPage");

        }

        public void editCoverphoto(string base64)
        {
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            var base64Str = base64.Split(',')[1];
            string photoName = null;

            byte[] bytes = Convert.FromBase64String(base64Str);

            photoName = Guid.NewGuid().ToString() + ".jpg";
            var path = Path.Combine(Server.MapPath("~/images/IdPhoto"), photoName);
            using (Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                image.Save(path, ImageFormat.Jpeg);  // Or Png
            }

            tMember q = db.tMembers.Where(m => m.fEmail == member.fEmail && m.fPassword == member.fPassword).FirstOrDefault();
            q.fCoverPhoto = photoName;
            db.SaveChanges();
            member.fCoverPhoto = photoName;
        }



        public ActionResult createMember()
        {
            return View();
        }
        [HttpPost]
        public ActionResult createMember(TMember m)
        {
            var base64Str = m.file_image.Split(',')[1];
            string photoName = null;
            if (m.file_image != null)
            {
                //base_64image = m.file_image;
                byte[] bytes = Convert.FromBase64String(base64Str);

                photoName = Guid.NewGuid().ToString() + ".jpg";
                var path = Path.Combine(Server.MapPath("~/images/IdPhoto"), photoName);
                using (Image image = Image.FromStream(new MemoryStream(bytes)))
                {
                    image.Save(path, ImageFormat.Jpeg);  // Or Png
                }
                m.fIdPhoto = "../images/IdPhoto/" + photoName;

            }

            tMember member = new tMember();
            member.fEmail = m.fEmail;
            member.fName = m.fName;
            member.fPhone = m.fPhone;
            member.fPassword = m.fPassword;
            if (m.fIdPhoto != null)
            {
                member.fIdPhoto = photoName;
            }
            else
            {
                member.fIdPhoto = "personnel_boy.png";
            }

            // 路徑由cshtml裡面做提供
            db.tMembers.Add(member);
            db.SaveChanges();
            return RedirectToAction("happyLogin", "Blog");
        }

        public ActionResult createSupplier()
        {
            return View();
        }
        [HttpPost]
        public ActionResult createSupplier(TSupplier s)
        {

            int index = s.file_photo.FileName.IndexOf(".");
            string extention = s.file_photo.FileName.Substring(index, s.file_photo.FileName.Length - index);
            string photoName = Guid.NewGuid().ToString() + extention;
            s.tMember.fIdPhoto = "../images/IdPhoto/" + photoName;
            string str = Server.MapPath("../images/IdPhoto/") + photoName;
            s.file_photo.SaveAs(Server.MapPath("../images/IdPhoto/") + photoName);
            //happyEntities db = new happyEntities();

            tSupplier supplier = new tSupplier();
            tMember member = new tMember();

            member.fIdPhoto = photoName; // 路徑由cshtml裡面做提供
            member.fEmail = s.tMember.fEmail;
            member.fName = s.tMember.fName;
            member.fPhone = s.tMember.fPhone;
            member.fPassword = s.tMember.fPassword;
            db.tMembers.Add(member);
            db.SaveChanges();

            supplier.fName = s.fName;
            supplier.fMemberId = member.fMemberId;
            db.tSuppliers.Add(supplier);
            db.SaveChanges();
            return RedirectToAction("happyLogin", "Blog");
        }

        public ActionResult bloggerIndex()
        {
            if (Session[CDictionary.SK_LOGINED_USER] == null)
            {
                return RedirectToAction("happyLogin", "Blog");
            }
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            var notify = db.tNotifies.Where(n => n.fMemberId == member.fMemberId).ToList().Take(10);
            int count = db.tNotifies.Where(n => n.fHasread == false && n.fMemberId == member.fMemberId).Count();
            ViewBag.count = count;
            List<Cnotify> Cnotify = new List<Cnotify>();
            foreach (var item in notify)
            {
                Cnotify c = new Cnotify();
                c.fdescription = item.fDescription;
                c.fnotifytime = item.fNotifytime.ToString();
                if (item.fNotificate == "交友通知")
                {
                    var mem = db.tMembers.Where(n => n.fMemberId == item.fNotifyfromId).FirstOrDefault();
                    c.fidphoto = mem.fIdPhoto;
                    c.fname = mem.fName;

                }
                Cnotify.Add(c);
            }
            List<Cnotify> CnotifySort = Cnotify.OrderByDescending(c => c.fnotifytime).ToList();
            Session[CDictionary.comfirm] = CnotifySort;
            //朋友列表
            var beinvite = db.tFriendinvites.Where(i => i.fBefriendId == member.fMemberId&&i.fStatus==false).ToList();
            List<tMember> invite = new List<tMember>();
            foreach (var item in beinvite)
            {
                var inviter = db.tMembers.Where(m => m.fMemberId == item.fFriendId).FirstOrDefault();
                invite.Add(inviter);
            }
            ViewBag.friendtList = (new signalRModels()).getFriendListById((Session[CDictionary.SK_LOGINED_USER] as TMember).fMemberId);
            return View(invite);
        }

        public string loadpost()
        {
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            string ch_btn = "";
            var q = db.tPosts.OrderByDescending(p => p.fPosttime).ToList().Take(10);
            var sq = q.OrderBy(p => p.fPosttime);
            List<fawenInformation> info = new List<fawenInformation>();
            foreach (var item in sq)
            {
                fawenInformation m = new fawenInformation();
                m.fPostId = item.fPostId;
                m.fPosttime = item.fPosttime;
                m.fTitle = item.fTitle;
                m.fMemberId = item.fMemberId;
                m.fDescription = item.fDescription;
                m.fEarn = item.fEarn;
                m.fPersonal = item.fPersonal;
                m.fHasImage = item.fHasImage;
                var mem = db.tMembers.Where(t => t.fMemberId == item.fMemberId).FirstOrDefault();
                m.fName = mem.fName;
                m.fIdPhoto = mem.fIdPhoto;
                m.fCoverPhoto = mem.fCoverPhoto;
                info.Add(m);

            }
            var fri = db.tFriendinvites.Where(f => f.fFriendId == member.fMemberId && f.fStatus == false).ToList();
            var befri = db.tFriendinvites.Where(f => f.fBefriendId == member.fMemberId && f.fStatus == false).ToList();

            string str ="";
            
            List<string> pp = new List<string>();
            foreach (var item in info)
            {
                string spcomment = "";
                string cpcomment = "";
                string allcomment = "";
                if (item.fMemberId != member.fMemberId)
                {
                    var friend = fri.Where(f => f.fBefriendId == item.fMemberId).FirstOrDefault();
                    if (friend == null)
                    {
                        ch_btn = "加入好友";
                    }
                    else
                    {
                        ch_btn = "等待確認";

                    }
                    var befriend = befri.Where(f => f.fFriendId == item.fMemberId).FirstOrDefault();
                    if (befriend != null)
                    {
                        ch_btn = "答覆邀請";
                    }

                }

                var pcomment = db.tParentcomments.Where(f => f.fPostId == item.fPostId).ToList();
                
                if (pcomment.Count!=0)
                {

                    foreach (var parentcomment in pcomment)
                    {
                        var ccomment = db.tChildcomments.Where(f => f.fParentcommentId == parentcomment.fCommentId).ToList();
                        cpcomment = "";
                        if (ccomment.Count != 0)
                        {
                            spcomment = $"<li class='parrentcomment'><div class='comet-avatar'><img src='/images/IdPhoto/{parentcomment.tMember.fIdPhoto}' alt='' style='border-radius:50%;width:52px'></div><div class='we-comment'><div class='coment-head'><h5><a href='time-line.html' title=''>{parentcomment.tMember.fName}</a></h5><span>${parentcomment.fTime}</span><button id='firstclassreply' class='we-reply' title='Reply' commentid='{parentcomment.fCommentId}'><i class='fa fa-reply'></i></button></div><p>{parentcomment.fDescription}</p></div></li>";
                            foreach (var childcomment in ccomment)
                            {
                                cpcomment += $"<ul><li class='childcomment'><div class='comet-avatar'><img src='/images/IdPhoto/{childcomment.tMember.fIdPhoto}' alt='' style='border-radius:50%;width:52px'></div><div class='we-comment'><div class='coment-head'><h5><a href='time-line.html' title=''>{childcomment.tMember.fName}</a></h5><span>${childcomment.fChildtime}</span><button id='secondclassreply' class='we-reply' title='Reply' commentid='{childcomment.fChildcommentId}'><i class='fa fa-reply'></i></button></div><p>{childcomment.fChilddescription}</p></div></li></ul>";
                            }
                            allcomment += (spcomment + cpcomment);

                        }
                        else
                        {
                            allcomment += $"<li class='parrentcomment'><div class='comet-avatar'><img src='/images/IdPhoto/{parentcomment.tMember.fIdPhoto}' alt='' style='border-radius:50%;width:52px'></div><div class='we-comment'><div class='coment-head'><h5><a href='time-line.html' title=''>{parentcomment.tMember.fName}</a></h5><span>${parentcomment.fTime}</span><button id='firstclassreply' class='we-reply' title='Reply' commentid='{parentcomment.fCommentId}'><i class='fa fa-reply'></i></button></div><p>{parentcomment.fDescription}</p></div></li>";
                        }

                    }
                    str = $"<div class='central-meta item'>" +
                                        $"<div class='user-post'>" +
                                        $"<div class='friend-info'>" +
                                        $"<figure><!--image popover event-->" +
                                        $"<img creatorid='{item.fMemberId}' tabindex='0' src='/images/IdPhoto/{item.fIdPhoto}' style='cursor:pointer;' data-toggle='popover' data-trigger='focus' data-html='true' title=\"<div id='aaa' class='feature-photo'><figure><img src='/images/resources/timeline-1.jpg' alt=''></figure></div><div class='row'><div class='ml-3'><figure><img src='/images/IdPhoto/{item.fIdPhoto}' style='border-radius:50%;width:60px' /><strong>  {item.fName}</strong></figure></div><div class='d-flex align-items-center'></div></div><div class='text-right' creatorid='{item.fMemberId}'><button class='m-1 btn btn-secondary btn-sm ' id='friend_hover'>{ch_btn}</button><button class='m-1 btn btn-secondary btn-sm ' id='follower_hover'>加入追蹤</button></div>\" />" +
                                        $"</figure><div class='friend-name'><ins><a href = 'time-line.html' title=''>{item.fName}</a></ins>" +
                                        $"<span>發布時間: {item.fPosttime}</span> </div>" +
                                        $"<div class='post-meta'><div class='description'> {item.fDescription};</div>" +
                                        $"<div class='we-video-info'>" +
                                        "<ul> <li>" +
                                        "<span class='like' data-toggle='tooltip' title='讚'>" +
                                        "<i class='fas fa-heart'></i>" +
                                        "<ins>2.2k</ins></span></li>" +
                                        "<li><a href='#a'><span class='comment' data-toggle='tooltip' title='留言'>" +
                                        "<i class='far fa-comment'></i> <ins>52</ins> </span></a> </li>" +
                                        "<li><span class='' data-toggle='tooltip' title='分享'><i class='fa fa-share-square'></i><ins>1.2k</ins></span>" +
                                        "</li><li><span class='' data-toggle='tooltip' title='檢舉'> <i class='fas fa-flag'></i></span>" +
                                        "</li></ul></div></div></div>" +
                                        $"<div class='coment - area'><ul class='we-comet'>"+allcomment+/*母評論區*/
                                        $"<li class='post-comment'><div class='comet-avatar'><img src ='/images/IdPhoto/{member.fIdPhoto}' alt=''></div>" +
                                        $"<div class='post-comt-box'><textarea placeholder='回覆'></textarea><button id='post_comment' class='btn btn-danger float-right' type = 'button' postid='{item.fPostId}'>送出留言</button>" +
                                        $"</div></li></ul></div>" +/*回覆框*/
                                        "</div></div>";
                }
                
                else
                {
                    str = $"<div class='central-meta item'>" +
                                        $"<div class='user-post'>" +
                                        $"<div class='friend-info'>" +
                                        $"<figure><!--image popover event-->" +
                                        $"<img creatorid='{item.fMemberId}' tabindex='0' src='/images/IdPhoto/{item.fIdPhoto}' style='cursor:pointer;' data-toggle='popover' data-trigger='focus' data-html='true' title=\"<div id='aaa' class='feature-photo'><figure><img src='/images/resources/timeline-1.jpg' alt=''></figure></div><div class='row'><div class='ml-3'><figure><img src='/images/IdPhoto/{item.fIdPhoto}' style='border-radius:50%;width:60px' /><strong>  {item.fName}</strong></figure></div><div class='d-flex align-items-center'></div></div><div class='text-right' creatorid='{item.fMemberId}'><button class='m-1 btn btn-secondary btn-sm ' id='friend_hover'>{ch_btn}</button><button class='m-1 btn btn-secondary btn-sm ' id='follower_hover'>加入追蹤</button></div>\" />" +
                                        $"</figure><div class='friend-name'><ins><a href = 'time-line.html' title=''>{item.fName}</a></ins>" +
                                        $"<span>發布時間: {item.fPosttime}</span> </div>" +
                                        $"<div class='post-meta'><div class='description'> {item.fDescription};</div>" +
                                        $"<div class='we-video-info'>" +
                                        "<ul> <li>" +
                                        "<span class='like' data-toggle='tooltip' title='讚'>" +
                                        "<i class='fas fa-heart'></i>" +
                                        "<ins>2.2k</ins></span></li>" +
                                        "<li><a href='#a'><span class='comment' data-toggle='tooltip' title='留言'>" +
                                        "<i class='far fa-comment'></i> <ins>52</ins> </span></a> </li>" +
                                        "<li><span class='' data-toggle='tooltip' title='分享'><i class='fa fa-share-square'></i><ins>1.2k</ins></span>" +
                                        "</li><li><span class='' data-toggle='tooltip' title='檢舉'> <i class='fas fa-flag'></i></span>" +
                                        "</li></ul></div></div></div>" +
                                        $"<div class='coment - area'><ul class='we-comet'>" +/*母評論區*/
                                        $"<li class='post-comment'><div class='comet-avatar'><img src ='/images/IdPhoto/{member.fIdPhoto}' alt=''></div>" +
                                        $"<div class='post-comt-box'><textarea placeholder='回覆'></textarea><button id='post_comment' class='btn btn-danger float-right' type = 'button' postid='{item.fPostId}'>送出留言</button>" +
                                        $"</div></li></ul></div>" +/*回覆框*/
                                        "</div></div>";
                }
                
                pp.Add(str);
            }
            string strpost = System.Text.Json.JsonSerializer.Serialize(pp);


            return strpost;
        }
        public ActionResult groupPage()
        {
            if (Session[CDictionary.SK_LOGINED_USER] == null)
            {
                return RedirectToAction("happyLogin", "Blog");
            }
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int count = db.tNotifies.Where(n => n.fHasread == false && n.fMemberId == member.fMemberId).Count();
            ViewBag.count = count;
            return View();
        }
        public ActionResult activityPage()
        {
            if (Session[CDictionary.SK_LOGINED_USER] == null)
            {
                return RedirectToAction("happyLogin", "Blog");
            }
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int count = db.tNotifies.Where(n => n.fHasread == false && n.fMemberId == member.fMemberId).Count();
            ViewBag.count = count;
            return View();
        }
        public ActionResult albumPage()
        {
            if (Session[CDictionary.SK_LOGINED_USER] == null)
            {
                return RedirectToAction("happyLogin", "Blog");
            }
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int count = db.tNotifies.Where(n => n.fHasread == false && n.fMemberId == member.fMemberId).Count();
            ViewBag.count = count;
            return View();
        }
        public readonly string[] names = {"Anna","Brittany","Cinderella",
            "Diana","Eva","Fiona","Gunda","Hege","Inga","Johanna","Kitty",
            "Linda","Nina","Ophelia","Petunia","Amanda","Raquel","Cindy",
            "Doris","Eve","Evita","Sunniva","Tove","Unni","Violet","Liza",
            "Elizabeth","Ellen","Wenche","Vicky"};
        public string getAutoCompleteFriendList()
        {
            return string.Join(",", names);
        }
        public string simplePost(simplePostJson json)
        {
            var jj = json;
            //string articletext = jj.text+"_motherfucker_"+jj.img;
            //string articletext = jj.text + "_matherfucker_" + jj.img;
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int fid = member.fMemberId;

            //切字串測試
            string str = jj.img;

            string[] sArray = str.Split(new string[] { "<img", "width:50px" }, StringSplitOptions.RemoveEmptyEntries);

            string img = "";
            string tempimg = "";
            string articletext = "<p>" + jj.text + "</p>";
            string simplearticle = jj.text;
            for (int i = 1; i < sArray.Length; i++)
            {
                if ((i % 2) == 0)
                    continue;
                else
                {
                    int index = sArray[i].IndexOf("style");
                    img += $"<img class='mb-2' {sArray[i].Substring(0, index)}></br>";

                    tempimg = sArray[i].Substring(0, index);
                    var tempbase64Str = tempimg.Split(',')[1];
                    var base64Str = tempbase64Str.Substring(0, tempbase64Str.Length - 2);
                    string photoName = null;

                    //base_64image = m.file_image;
                    byte[] bytes = Convert.FromBase64String(base64Str);

                    photoName = Guid.NewGuid().ToString() + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/images/PostPhoto"), photoName);
                    using (Image image = Image.FromStream(new MemoryStream(bytes)))
                    {
                        image.Save(path, ImageFormat.Jpeg);  // Or Png
                    }
                    articletext += $"<img src='/images/PostPhoto/{photoName}'/>";
                }
                simplearticle += img;
            }

            //結尾

            happyEntities db = new happyEntities();
            tPost post = new tPost();
            post.fMemberId = fid;
            post.fDescription = articletext;
            post.fEarn = false;
            if (jj.img != null)
            {
                post.fHasImage = true;
            }
            else
            {
                post.fHasImage = false;
            }
            post.fPersonal = true;
            post.fPosttime = DateTime.Now;
            db.tPosts.Add(post);
            db.SaveChanges();
            Session[CDictionary.postdetails] = post;
            articleContextJson article = new articleContextJson();
            article.fName = member.fName;
            article.fTitle = "沒有title";
            article.img = img;
            article.text = jj.text;
            article.fPosttime = post.fPosttime;
            article.fIdPhoto = member.fIdPhoto;
            string jasonpost = System.Text.Json.JsonSerializer.Serialize(article);
            return jasonpost;
        }
        public string detailpost(detailPostJson json)
        {
            var jj = json;
            //string articletext = jj.text+"_motherfucker_"+jj.img;
            //string articletext = jj.text + "_matherfucker_" + jj.img;

            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;

            int fid = member.fMemberId;

            //切字串測試
            string str = jj.detailtext;

            //string[] sArray = str.Split(new string[] { "<img", "width:50px" }, StringSplitOptions.RemoveEmptyEntries);

            //string img = "";
            //for (int i = 1; i < sArray.Length; i++)
            //{
            //    if ((i % 2) == 0)
            //        continue;
            //    else
            //    {
            //        int index = sArray[i].IndexOf("style");
            //        img += $"<img class='mb-2' {sArray[i].Substring(0, index)}><br>";
            //    }
            //}
            //string articletext = jj.text + "_matherfucker_" + img;
            //結尾

            happyEntities db = new happyEntities();
            tPost post = new tPost();
            post.fMemberId = fid;
            post.fDescription = jj.detailtext;
            post.fEarn = false;
            if (jj.detailtext.Contains("img"))
            {
                post.fHasImage = true;
            }
            else
            {
                post.fHasImage = false;
            }
            post.fPersonal = true;
            post.fPosttime = DateTime.Now;
            db.tPosts.Add(post);
            db.SaveChanges();
            Session[CDictionary.postdetails] = post;
            articleContextJson article = new articleContextJson();
            article.fName = member.fName;
            article.fTitle = "沒有title";

            article.text = jj.detailtext;
            article.fPosttime = post.fPosttime;
            article.fIdPhoto = member.fIdPhoto;
            string jasonpost = System.Text.Json.JsonSerializer.Serialize(article);
            return jasonpost;
        }

        public ActionResult messagePage()
        {
            if (Session[CDictionary.SK_LOGINED_USER] == null)
            {
                return RedirectToAction("happyLogin", "Blog");
            }
            return View();
        }
        public void addFriend(AddFriendJson add)
        {
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int id1 = member.fMemberId;
            int id2 = int.Parse(add.memberid);
            tFriendinvite friend = new tFriendinvite();
            friend.fFriendId = id1;
            friend.fBefriendId = id2;
            friend.fStatus = false;
            db.tFriendinvites.Add(friend);
            tNotify notify = new tNotify();
            //var inviter = db.tMembers.Where(t => t.fMemberId == id1).FirstOrDefault();
            notify.fMemberId = id2;
            notify.fNotifyfromId = id1;
            notify.fNotifytime = DateTime.Now;
            notify.fDescription = "對你發出交友邀請";
            notify.fHasread = false;
            notify.fNotificate = "交友通知";
            db.tNotifies.Add(notify);
            db.SaveChanges();
        }

        public void notifyRead()
        {
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            var n = db.tNotifies.Where(m => m.fMemberId == member.fMemberId).ToList();
            foreach (var item in n)
            {
                item.fHasread = true;
            }
            db.SaveChanges();
        }

        public JsonResult GetEvents()
        {
            using (happyEntities dc = new happyEntities())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var events = dc.tActivities.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Tcalendar_event e)
        {
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int fid = member.fMemberId;

            var status = false;
            using (happyEntities dc = new happyEntities())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.tActivities.Where(a => a.fActivityId == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.fTitle = e.Subject;
                        v.fStarttime = e.Start;
                        v.fEndtime = e.End;
                        v.fDescription = e.Description;
                        v.fCreatorId = member.fMemberId;
                        v.fCreatetime = DateTime.Now;
                    }
                }
                else
                {
                    tActivity act = new tActivity();

                    act.fTitle = e.Subject;
                    act.fStarttime = e.Start;
                    act.fEndtime = e.End;
                    act.fDescription = e.Description;
                    act.fCreatorId = member.fMemberId;
                    act.fCreatetime = DateTime.Now;

                    dc.tActivities.Add(act);
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (happyEntities dc = new happyEntities())
            {
                var v = dc.tActivities.Where(a => a.fActivityId == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.tActivities.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult otherPerson(int memberid)
        {
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int count = db.tNotifies.Where(n => n.fHasread == false && n.fMemberId == member.fMemberId).Count();
            ViewBag.count = count;
            tMember t = db.tMembers.Where(m => m.fMemberId == memberid).FirstOrDefault();
            return View(t);
        }
        
        public string postcomment(PostCommentJason post)
        {
            //回覆者就是登入者，postid是回覆文章id或評論id，comment是評論內容
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int postid=int.Parse(post.postid);
            string comment = post.comment;
            //存入db
            tParentcomment parentcomment = new tParentcomment();
            parentcomment.fMemberId = member.fMemberId;
            parentcomment.fPostId = postid;
            parentcomment.fDescription = comment;
            parentcomment.fTime = DateTime.Now;
            db.tParentcomments.Add(parentcomment);
            db.SaveChanges();
            //做成jason往前送
            post.commentid = parentcomment.fCommentId.ToString();
            post.time = parentcomment.fTime.ToString();
            post.Name = member.fName;
            post.photo = member.fIdPhoto;

            string jasonpostcomment = System.Text.Json.JsonSerializer.Serialize(post);
            return (jasonpostcomment);
        }
        public string postchildcomment(PostCommentJason post)
        {
            //回覆者就是登入者，postid是回覆文章id或評論id，comment是評論內容
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            int postid = int.Parse(post.postid);
            string comment = post.comment;
            //存入db
            tChildcomment childcomment = new tChildcomment();
            childcomment.fChilduserId= member.fMemberId;
            childcomment.fParentcommentId= postid;
            childcomment.fChilddescription= comment;
            childcomment.fChildtime= DateTime.Now;
            db.tChildcomments.Add(childcomment);
            db.SaveChanges();
            //做成jason往前送
            post.commentid = childcomment.fChildcommentId.ToString();
            post.time = childcomment.fChildtime.ToString();
            post.Name = member.fName;
            post.photo = member.fIdPhoto;

            string jasonpostcomment = System.Text.Json.JsonSerializer.Serialize(post);
            return (jasonpostcomment);
        }

        public string replyInvite(int mem)
        {
            //int n = mem;
            TMember member = Session[CDictionary.SK_LOGINED_USER] as TMember;
            tFriendinvite friend = new tFriendinvite();
            friend.fBefriendId = mem;
            friend.fFriendId = member.fMemberId;
            friend.fStatus = true;
            db.tFriendinvites.Add(friend);
            tFriendinvite t = db.tFriendinvites.Where(f => f.fFriendId == mem && f.fBefriendId == member.fMemberId).FirstOrDefault();
            t.fStatus = true;
            tNotify notify = new tNotify();
            notify.fMemberId = mem;
            notify.fNotifyfromId = member.fMemberId;
            notify.fNotifytime = DateTime.Now;
            notify.fDescription = "同意交友邀請";
            notify.fHasread = false;
            notify.fNotificate = "交友通知";
            db.tNotifies.Add(notify);
            db.SaveChanges();
            tMember tmem = db.tMembers.Where(p => p.fMemberId == mem).FirstOrDefault();
            CnewFriend cnewFriend = new CnewFriend();
            cnewFriend.fidphoto = tmem.fIdPhoto;
            cnewFriend.fname = tmem.fName;
            string jasonpost = System.Text.Json.JsonSerializer.Serialize(cnewFriend);
            return jasonpost;
        }

    }

}