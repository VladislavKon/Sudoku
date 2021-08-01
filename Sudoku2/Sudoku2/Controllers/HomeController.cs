
using Newtonsoft.Json;
using Sudoku2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sudoku2.Controllers
{
    public class HomeController : Controller
    {
        SudokuContext db = new SudokuContext();        
        public ActionResult Index()
        {
            Board board = new Board();           
            Session["ArraySquares"] = board.ArraySquares;            
            ViewBag.Board = board;
            var dbResults = db.Results.OrderBy(a => a.Time);
            ViewBag.Top = dbResults;
            return View();
        }
        public ActionResult LoadBoard(int id)
        {
            List<int> des;
            var dbResults = db.Results.OrderBy(a => a.Time);
            foreach (var item in dbResults)
            {
                if (item.Id==id)
                {
                    des = JsonConvert.DeserializeObject<List<int>>(item.Squares);
                    ViewBag.SolvedBoard = des;
                    ViewBag.Name = item.Name;
                    break;
                }                
            }     
            return View();
        }

        [HttpGet]
        public ActionResult WriteName()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult End(List<int> Squares)
        {
            string str=Squares.ToString();            
            Session["Result"] = DateTime.Now - (DateTime)Session["Time"];
            if (Squares.SequenceEqual((List<int>)Session["ArraySquares"]))
            {
                Session["message"] = "";
                return Redirect("/Home/WriteName");                
            }
            Session["message"] = "Не верное решение";            
            return Redirect("/Home/Index");
        }        
        public ActionResult WriteName(string name)
        {
            string stringSquares= JsonConvert.SerializeObject((List<int>)Session["ArraySquares"]);            
            GameResult gameResult = new GameResult() { Name = name, Squares = stringSquares, Time = (TimeSpan)Session["Result"] };
            Session["Name"] = name;
            db.Results.Add(gameResult);
            db.SaveChanges();
            return Redirect("Index");
        }
        
              
    }
}