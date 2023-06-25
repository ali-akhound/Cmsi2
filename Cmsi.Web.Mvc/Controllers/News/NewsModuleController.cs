using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AVA.Core.Entities;
using System.Data.Entity;
using AVA.Web.Mvc.Models.Admin;
using AVA.UI.Helpers.Common;

namespace AVA.Web.Mvc.Controllers
{
    public class NewsModuleController : Controller
    {
        const int pageSize = 5;
        [HttpGet]
        public ActionResult NewsList(int pageNumber, string year)
        {
            return PartialView("~/Views/News/NewsList.cshtml", NewsListFeedGenerator(pageNumber, year));
        }
        [HttpGet]
        public ActionResult NewsListFeed(int pageNumber, string year)
        {
            return Json(NewsListFeedGenerator(pageNumber, year), JsonRequestBehavior.AllowGet);
        }
        public NewsListViewModel NewsListFeedGenerator(int pageNumber, string year)
        {
            string CurrentLang = Request.Cookies["Lang"].Value;
            var contex = new ApplicationDbContext();
            //int totalRecords = contex.Newses.Count();
            int skipRows = (pageNumber - 1) * pageSize;
            var News = contex.Newses
                .Include(news => news.Language)
                .Where(news => (bool)news.Enable && news.Language.Value == CurrentLang)
                .OrderByDescending(news => news.CreateDate)
                .Select(news => new { news.ID, news.Subject, news.ShortImageUrl, news.Date, news.Summery })
                .ToList();

            var newsList = new List<NewsViewModel>();
            foreach (var item in News)
            {
                newsList.Add(
                    new NewsViewModel()
                    {
                        ID = item.ID,
                        Subject = item.Subject,
                        Image = !string.IsNullOrEmpty(item.ShortImageUrl) ? Url.Content(item.ShortImageUrl) : "",
                        Summery = item.Summery,
                        DateConverted = CommonHelper.DateAndTimes.GetPersianDate(item.Date),
                        MonthName = CommonHelper.DateAndTimes.GetMonthName(item.Date),
                        Day = CommonHelper.DateAndTimes.GetPersianDate(item.Date).Split('/')[2],
                        Year = CommonHelper.DateAndTimes.GetPersianDate(item.Date).Split('/')[0]
                    }
                );
            }
            var NewsGroups = newsList
                .GroupBy(news => news.Year)
                .Select(news => new { news.Key, Count = news.Count() })
                .OrderByDescending(news => news.Key);
            var newsGroupList = new List<NewsGroupListViewModel>();
            foreach (var item in NewsGroups)
            {
                newsGroupList.Add(new NewsGroupListViewModel()
                {
                    Year = item.Key,
                    Count = item.Count
                });
            }
            return new NewsListViewModel()
            {
                DataModel = newsList.Where(news => news.Year == year || string.IsNullOrEmpty(year) || year == "undefined").Skip(skipRows).Take(pageSize).ToList(),
                pageNumber = pageNumber,
                pageSize = pageSize,
                totalRecords = newsList.Count(news => news.Year == year || string.IsNullOrEmpty(year) || year == "undefined"),
                GroupList = newsGroupList,
            };
        }
        public ActionResult NewsDetails(int id)
        {
            var contex = new ApplicationDbContext();
            var selectedItem = contex.Newses.Where(news => news.ID == id).SingleOrDefault();
            if (selectedItem != null)
            {
                selectedItem.Visit = selectedItem.Visit == null ? 0 : selectedItem.Visit + 1;
                contex.SaveChanges();
                return PartialView("~/Views/News/NewsDetails.cshtml", new NewsViewModel()
                {
                    ID = selectedItem.ID,
                    Subject = selectedItem.Subject,
                    Explain = selectedItem.Explain,
                    Image = selectedItem.LongImageUrl != null ? Url.Content(selectedItem.LongImageUrl) : "",
                    DateConverted = CommonHelper.DateAndTimes.GetPersianDate(selectedItem.Date),
                    Keywords = selectedItem.Keywords == null ? "" : selectedItem.Keywords,
                    Description = selectedItem.Description,
                    MonthName = CommonHelper.DateAndTimes.GetMonthName(selectedItem.Date),
                    Day = CommonHelper.DateAndTimes.GetPersianDate(selectedItem.Date).Split('/')[2],
                });
            }
            else
            {
                return PartialView("~/Views/News/NewsDetails.cshtml", new NewsViewModel());
            }
        }

    }
}