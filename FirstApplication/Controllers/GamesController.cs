using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstApplication.Models;

namespace FirstApplication.Controllers
{
    public class GamesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Games
        public ActionResult Index()
        {
            var games = db.Games;
            return View(games.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);

            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            Game model = new Game();
            model.Name = String.Format("Game - {0}", DateTime.Now.Ticks);

            ViewBag.Genres = new MultiSelectList(db.Genres.ToList(), "GenreId", "Name", model.Genres.Select(x=>x.GenreId).ToArray());

            return View(model);
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,IsMultiplayer,GenreIds")] Game model, string[] GenreIds)
        {
            if (ModelState.IsValid)
            {
                Game checkmodel = db.Games.SingleOrDefault(x => x.Name == model.Name && x.IsMultiplayer == model.IsMultiplayer);

                if (checkmodel == null)
                {
                    //model.GameId = Guid.NewGuid().ToString();
                    //model.CreateDate = DateTime.Now;
                    //model.EditDate = model.CreateDate;
                    db.Games.Add(model);
                    db.SaveChanges();

                    if (GenreIds != null)
                    {
                        foreach (string genreId in GenreIds)
                        {
                            GameGenre gameGenre = new GameGenre();

                            //gameGenre.GameGenreId = Guid.NewGuid().ToString();
                            //gameGenre.CreateDate = DateTime.Now;
                            //gameGenre.EditDate = gameGenre.CreateDate;

                            gameGenre.GameId = model.GameId;
                            gameGenre.GenreId = genreId;
                            model.Genres.Add(gameGenre);
                        }

                        db.Entry(model).State = EntityState.Modified;

                        db.SaveChanges();
                    }


                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicated Game detected.");
                }
            }

            ViewBag.Genres = new MultiSelectList(db.Genres.ToList(), "GenreId", "Name", GenreIds);

            return View(model);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game model = db.Games.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.Genres = new MultiSelectList(db.Genres.ToList(), "GenreId", "Name", model.Genres.Select(x => x.GenreId).ToArray());

            return View(model);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameId,Name,IsMultiplayer,GenreIds")] Game model, string[] GenreIds)
        {
            if (ModelState.IsValid)
            {
                Game tmpmodel = db.Games.Find(model.GameId);
                if(tmpmodel != null)
                {
                    Game checkmodel = db.Games.SingleOrDefault(
                                        x=>x.Name == model.Name && 
                                        x.IsMultiplayer == model.IsMultiplayer &&
                                        x.GameId != model.GameId);

                    if (checkmodel == null)
                    {
                        tmpmodel.Name = model.Name;
                        tmpmodel.IsMultiplayer = model.IsMultiplayer;
                        tmpmodel.EditDate = DateTime.Now;

                        db.Entry(tmpmodel).State = EntityState.Modified;

                        //Items to remove
                        var removeItems = tmpmodel.Genres.Where(x => !GenreIds.Contains(x.GenreId)).ToList();

                        foreach (var removeItem in removeItems)
                        {
                            db.Entry(removeItem).State = EntityState.Deleted;
                        }

                        if (GenreIds != null)
                        {
                            var addedItems = GenreIds.Where(x => !tmpmodel.Genres.Select(y => y.GenreId).Contains(x));

                            //Items to add
                            foreach (string addedItem in addedItems)
                            {
                                GameGenre gameGenre = new GameGenre();

                                gameGenre.GameGenreId = Guid.NewGuid().ToString();
                                gameGenre.CreateDate = DateTime.Now;
                                gameGenre.EditDate = gameGenre.CreateDate;

                                gameGenre.GameId = tmpmodel.GameId;
                                gameGenre.GenreId = addedItem;
                                db.GameGenres.Add(gameGenre);
                            }
                        }

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Duplicated Game detected.");
                    }
                }
            }

            ViewBag.Genres = new MultiSelectList(db.Genres.ToList(), "GenreId", "Name", GenreIds);

            return View(model);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Game model = db.Games.Find(id);

            if(model == null)
            {
                return HttpNotFound();
            }

            foreach (var item in model.Genres.ToList())
            {
                db.GameGenres.Remove(item);
            }

            db.Games.Remove(model);

            var deleted = db.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
