
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
            Solver solver = new Solver();
            ViewBag.index = solver.GetRandom();
            ViewBag.board = solver.GetBoard();
            Session["ArraySquares"] = solver.GetBoard();
            var dbResults = db.Results.OrderBy(a => a.Time);
            ViewBag.Top = dbResults;
            return View();
        }
        public ActionResult LoadBoard(int id)
        {
            int[] des;
            var dbResults = db.Results.OrderBy(a => a.Time);
            foreach (var item in dbResults)
            {
                if (item.Id==id)
                {
                    des = JsonConvert.DeserializeObject<int[]>(item.Squares);
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
        public ActionResult End(int[] Squares)
        {
            Session["Result"] = DateTime.Now - (DateTime)Session["Time"];
            string str=Squares.ToString();
            SolveChecker solveChecker = new SolveChecker();            
            if (solveChecker.Check(Squares))
            {
                Session["message"] = "";
                return Redirect("/Home/WriteName");                
            }
            Session["message"] = "Не верное решение";            
            return Redirect("/Home/Index");
        }        
        public ActionResult WriteName(string name)
        {
            string stringSquares= JsonConvert.SerializeObject((int[])Session["ArraySquares"]);            
            GameResult gameResult = new GameResult() { Name = name, Squares = stringSquares, Time = (TimeSpan)Session["Result"] };
            Session["Name"] = name;
            db.Results.Add(gameResult);
            db.SaveChanges();
            return Redirect("Index");
        }
        
              
    }
}