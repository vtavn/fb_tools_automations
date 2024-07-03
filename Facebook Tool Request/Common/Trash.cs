using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Common
{
    internal class Trash
    {

        //get token page post bài mới
        //string tokenPage = Helpers.CommonRequest.getTokenPageAndFindUid(uidPage, token, cookie, proxy, typeProxy);
        //if(tokenPage != null || tokenPage != "")
        //{
        //    DelayThaoTacNho(1);
        //    bool changeInfo = Helpers.CommonRequest.updateInfoPageByToken(cookie, tokenPage, proxy, userAgent, typeProxy, uidPage, linkAvt, linkCover, txtPhoneNumber, txtWebsite);
        //    if(changeInfo)
        //    {
        //        DelayThaoTacNho(1);
        //        //first post
        //        if (ckbFirstPost)
        //        {
        //            bool post = false;
        //            SetStatusAccount(indexRow, text2 + $"Đang nghỉ, chuẩn bị đăng bài đầu tiên: {namePage}/{uidPage} !");
        //            Helpers.Common.DelayTime(rd.Next(delayPostform, delayPostto + 1));
        //            SetStatusAccount(indexRow, text2 + $"Đăng bài viết đầu tiên lên trang: {namePage}/{uidPage} !");

        //            if (ckbPostImg)
        //            {
        //                if (pathPostImg.Count == 0)
        //                {
        //                    pathPostImg = CloneList(pathPostImgs);
        //                }
        //                linkImgPost = pathPostImg[rd.Next(0, pathPostImg.Count)];
        //                pathCover.Remove(linkImgPost);
        //                SetStatusAccount(indexRow, text2 + ("Đang chuẩn bị ảnh bài viết..."));
        //                linkImgPost = CommonRequest.UploadImgToServer(linkImgPost);
        //                DelayThaoTacNho(1);

        //                SetStatusAccount(indexRow, text2 + ("Đang chuẩn bị nội dung bài viết..."));
        //                txtContentPost = Helpers.Common.SpinText(txtContentPost, rd);
        //                txtContentPost = GetIconFacebook.ProcessString(txtContentPost, rd);
        //                //txtContentPost = HttpUtility.UrlEncode(txtContentPost);
        //                DelayThaoTacNho(1);
        //                if (linkImgPost != "" || linkImgPost != null)
        //                {
        //                    SetStatusAccount(indexRow, text2 + ("Đang đăng bài viết mới..."));
        //                    post = Helpers.CommonRequest.postNewPost(cookie, tokenPage, proxy, userAgent, typeProxy, uidPage, linkImgPost, txtContentPost, 1);
        //                }
        //            }
        //            else
        //            {
        //                post = Helpers.CommonRequest.postNewPost(cookie, tokenPage, proxy, userAgent, typeProxy, uidPage, linkImgPost, txtContentPost, 2);
        //            }

        //            if (post)
        //            {
        //                SetStatusAccount(indexRow, text2 + $"Đăng bài viết thành công : {namePage}/{uidPage} !");
        //                chrome.GotoURL("https://www.facebook.com/" + uidPage);
        //                chrome.DelayTime(1.0);
        //                goto checkDone;
        //            }
        //            else
        //            {
        //                SetStatusAccount(indexRow, text2 + ("Không đăng được bài mới."));
        //                return 0;
        //            }
        //        }
        //        else
        //        {
        //            SetStatusAccount(indexRow, text2 + $"Cập nhật thành công thông tin trang: {namePage}/{uidPage} !");
        //            chrome.GotoURL("https://www.facebook.com/" + uidPage);
        //            chrome.DelayTime(1.0);
        //            goto checkDone;
        //        }
        //    }
        //    else
        //    {
        //        SetStatusAccount(indexRow, text2 + $"Không cập nhật được thông tin trang!");
        //        chrome.GotoURL("https://www.facebook.com/" + uidPage);
        //        chrome.DelayTime(1.0);
        //        goto checkDone;
        //    }
        //}
        //else
        //{
        //    SetStatusAccount(indexRow, text2 + $"Không có token Page!");
        //    chrome.GotoURL("https://www.facebook.com/" + uidPage);
        //    chrome.DelayTime(1.0);
        //    goto checkDone;
        //}

    }
}
