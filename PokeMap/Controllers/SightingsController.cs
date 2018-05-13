using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PokeMap.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using PokeMap.Enums;
using PokeMap.Extension_Methods;
using System.IO;
using Newtonsoft.Json;
using PokeMap.Classes;
using System;
using PokeMap.DTOs;

namespace PokeMap.Controllers
{
    public class SightingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<SightingDTO> _existingSightings = new List<SightingDTO>();

        // GET: Sightings
        public ActionResult Index()
        {
            return View(db.Sightings.ToList());
        }



        public void APIAdd(Sighting passedSighting)
        {

            if (!DoesSightingPreExist(passedSighting))
            {
                Sighting sighting = new Sighting();
                sighting.Longitude = passedSighting.Longitude;
                sighting.Latitude = passedSighting.Latitude;

                if (IsItemOnSameLocation(passedSighting))
                    RandomiseLocation(passedSighting);

                sighting.AspNetUserId = "473a991e-b5f9-4c19-b34e-255af548fc90";
                sighting.Type = passedSighting.Type;
                sighting.Rarity = passedSighting.Rarity;
                sighting.PokeMon = passedSighting.PokeMon;
                sighting.TimeOfDay = passedSighting.TimeOfDay;
                sighting.Notes = passedSighting.Notes;

                db.Sightings.Add(sighting);

                db.SaveChanges();

                Vote vote = new Vote();
                vote.SightingId = sighting.SightingId;
                vote.AspNetUserId = "473a991e-b5f9-4c19-b34e-255af548fc90";
                vote.Action = VoteAction.Up;
                db.Votes.Add(vote);

                db.SaveChanges();
            }

        }

        public Sighting RandomiseLocation(Sighting sighting)
        {
            var x0 = sighting.Longitude;
            var y0 = sighting.Latitude;
            // Convert Radius from meters to degrees. - 5 metres
            var rd = 4.492362982929021;

            var u = new Random().NextDouble();
            var v = new Random().NextDouble();

            var w = rd * Math.Sqrt(u);
            var t = 2 * Math.PI * v;
            var x = w * Math.Cos(t);
            var y = w * Math.Sin(t);

            var xp = x / Math.Cos(y0);

            sighting.Latitude = y + y0;
            sighting.Longitude = xp + x0;

            // Resulting point.
            return sighting;
        }

        public bool DoesSightingPreExist(Sighting sighting)
        {
            return db.Sightings.Any(x => x.Longitude == sighting.Longitude && x.Latitude == sighting.Latitude && x.PokeMon == sighting.PokeMon);
        }


        public bool IsItemOnSameLocation(Sighting sighting)
        {
            return db.Sightings.Any(x => x.Longitude == sighting.Longitude && x.Latitude == sighting.Latitude);
        }

        public ActionResult GetSightings()
        {
            var str = SightingType.PokeMon.GetDescription();
            return Json(db.Sightings.Where(xx => xx.Rating > -5).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HasVoted()
        {
            var req = Request.InputStream;
            var json = new StreamReader(req).ReadToEnd();
            var sightingId = JsonConvert.DeserializeObject<int>(json);
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            return Json(!db.Votes.Any(x => x.AspNetUserId == userId && x.SightingId == sightingId), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult DeleteSighting()
        {
            var req = Request.InputStream;
            var json = new StreamReader(req).ReadToEnd();
            var sightingId = JsonConvert.DeserializeObject<int>(json);
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            Sighting sighting = db.Sightings.Find(sightingId);

            if (sighting == null || userId == null)
                return new HttpStatusCodeResult(404, "This sighting no longer exists");

            if (sighting.AspNetUserId == userId || db.Users.Where(xx => xx.Id == userId).Select(x => x.IsAdmin).First())
            {
                db.Sightings.Remove(sighting);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(400, "Bad Request, the data passed up was invalid");


        }


        static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }


        [HttpGet]
        public ActionResult GetPokemon()
        {

            var pokemon = Enum.GetValues(typeof(Pokemon)).Cast<Pokemon>().ToList();

            var mapped = from p in pokemon
                         select new PokemonData
                         {
                             EnumId = (int)p,
                             PokemonName = p.GetDescription()
                         };

            mapped = mapped.ToList();

            var result = Json(mapped, JsonRequestBehavior.AllowGet);

            return result;
        }

        [HttpPost]
        public ActionResult GetUserRating()
        {
            var req = Request.InputStream;
            var json = new StreamReader(req).ReadToEnd();
            var sightingId = JsonConvert.DeserializeObject<int>(json);
            var userId = db.Sightings.Where(xx => xx.SightingId == sightingId).Select(x => x.AspNetUserId).First();

            var selfVotes = db.Votes.Where(xx => xx.AspNetUserId == userId).Count();
            var usersSightings = db.Sightings.Where(xx => xx.AspNetUserId == userId).Select(x => x.SightingId);
            var otherVotes = db.Votes.Where(xx => usersSightings.Contains(xx.SightingId) && xx.AspNetUserId != userId).Count();

            double rating = (selfVotes * 0.25) + otherVotes;
            if (rating >= 20 && rating < 40)
                return Json("Bronze", JsonRequestBehavior.AllowGet);
            else if (rating >= 40 && rating < 60)
                return Json("Silver", JsonRequestBehavior.AllowGet);
            else if (rating >= 60)
                return Json("Top Contributor", JsonRequestBehavior.AllowGet);
            else
                return Json("No Rating", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetUserVoteStatus()
        {
            var req = Request.InputStream;
            var json = new StreamReader(req).ReadToEnd();
            var sightingId = JsonConvert.DeserializeObject<int>(json);
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var hasVoted = db.Votes.Any(x => x.AspNetUserId == userId && x.SightingId == sightingId);

            if (hasVoted)
            {
                var result = db.Votes.Where(x => x.AspNetUserId == userId && x.SightingId == sightingId).Select(x => x.Action).First().ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json("NoVote", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostSightings()
        {
            var req = Request.InputStream;
            var json = new StreamReader(req).ReadToEnd();
            var filters = JsonConvert.DeserializeObject<GetDataFilters>(json);

            var str = SightingType.PokeMon.GetDescription();

            var filteredData = db.Sightings.Where(xx => xx.Rating > -5 && xx.Latitude >= filters.Bounds.bottomLeftLat && xx.Longitude >= filters.Bounds.bottomLeftLong && xx.Latitude <= filters.Bounds.topRightLat && xx.Longitude <= filters.Bounds.topRightLong);

            if (!string.IsNullOrEmpty(filters.Pokemon))
            {
                filteredData = filteredData.Where(xx => xx.PokeMon.ToString().StartsWith(filters.Pokemon));
            }

            var sightingList = new List<SightingDTO>();


            if (_existingSightings.Any())
            {
                var stillWithinBounds = _existingSightings.Where(xx => xx.Latitude >= filters.Bounds.bottomLeftLat && xx.Longitude >= filters.Bounds.bottomLeftLong && xx.Latitude <= filters.Bounds.topRightLat && xx.Longitude <= filters.Bounds.topRightLong);
                var exisitingIds = stillWithinBounds.Select(xx => xx.SightingId);
            
                sightingList.AddRange(stillWithinBounds);
                
                filteredData = filteredData.Where(xx => !exisitingIds.Contains(xx.SightingId));
            }

            var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();

            foreach (var sighting in filteredData.ToList())
            {
                sightingList.Add(new SightingDTO
                {
                    AspNetUserId = sighting.AspNetUserId,
                    Latitude = sighting.Latitude,
                    Longitude = sighting.Longitude,
                    PokeMon = sighting.PokeMon != null ? sighting.PokeMon.GetDescription() : null,
                    Rarity = sighting.Rarity.GetDescription(),
                    Rating = sighting.Rating,
                    SightingId = sighting.SightingId,
                    Type = sighting.Type.GetDescription(),
                    TimeOfDay = sighting.TimeOfDay.ToString(),
                    TimeAdded = sighting.TimeAdded,
                    Notes = sighting.Notes
                });
            }

            _existingSightings = sightingList;

            //var data2 = data.ToList();
            var result = Json(sightingList, JsonRequestBehavior.AllowGet);

            return result;
        }

        // GET: Sightings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sighting sighting = db.Sightings.Find(id);
            if (sighting == null)
            {
                return HttpNotFound();
            }
            return View(sighting);
        }


        public ActionResult UpVote(int id)
        {
            Sighting sighting = db.Sightings.Find(id);
            sighting.Rating = sighting.Rating + 1;

            db.Entry(sighting).State = EntityState.Modified;
            db.SaveChanges();

            return Json(sighting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpVote()
        {
            var req = Request.InputStream;
            var json = new StreamReader(req).ReadToEnd();
            var id = JsonConvert.DeserializeObject<int>(json);

            Sighting sighting = db.Sightings.Find(id);
            sighting.Rating = sighting.Rating + 1;

            db.Entry(sighting).State = EntityState.Modified;

            Vote vote = new Vote();
            vote.SightingId = id;
            vote.AspNetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            vote.Action = VoteAction.Up;

            db.Votes.Add(vote);

            db.SaveChanges();

            return Json(sighting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Vote()
        {
            var req = Request.InputStream;


            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var json = new StreamReader(req).ReadToEnd();
            var data = JsonConvert.DeserializeObject<VoteData>(json);

            Sighting sighting = db.Sightings.Find(data.Id);
            sighting.Rating = data.Increase ? sighting.Rating + 1 : sighting.Rating - 1;
            db.Entry(sighting).State = EntityState.Modified;

            var preVote = db.Votes.Where(xx => xx.AspNetUserId == userId && xx.SightingId == data.Id).FirstOrDefault();
            if (preVote != null)
                db.Votes.Remove(preVote);

            Vote vote = new Vote();
            vote.SightingId = data.Id;
            vote.AspNetUserId = userId;


            if (data.Increase)
            {
                vote.Action = VoteAction.Up;
            }
            else
            {
                vote.Action = VoteAction.Down;
            }

            db.Votes.Add(vote);
            db.SaveChanges();

            var sightingMapped = new SightingDTO();

            sightingMapped.SightingId = sighting.SightingId;
            sightingMapped.AspNetUserId = sighting.AspNetUserId;
            sightingMapped.Latitude = sighting.Latitude;
            sightingMapped.Longitude = sighting.Longitude;
            sightingMapped.PokeMon = sighting.PokeMon != null ? sighting.PokeMon.GetDescription() : null;
            sightingMapped.Rarity = sighting.Rarity.GetDescription();
            sightingMapped.Rating = sighting.Rating;
            sightingMapped.SightingId = sighting.SightingId;
            sightingMapped.Type = sighting.Type.GetDescription();
            sightingMapped.TimeOfDay = sighting.TimeOfDay.ToString();


            return Json(sightingMapped, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DownVote()
        {
            var req = Request.InputStream;
            var json = new StreamReader(req).ReadToEnd();
            var id = JsonConvert.DeserializeObject<int>(json);

            Sighting sighting = db.Sightings.Find(id);
            sighting.Rating = sighting.Rating - 1;

            db.Entry(sighting).State = EntityState.Modified;


            Vote vote = new Vote();
            vote.SightingId = id;
            vote.AspNetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            vote.Action = VoteAction.Down;

            db.Votes.Add(vote);
            db.SaveChanges();

            return Json(sighting, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownVote(int id)
        {
            Sighting sighting = db.Sightings.Find(id);
            sighting.Rating = sighting.Rating - 1;

            db.Entry(sighting).State = EntityState.Modified;
            db.SaveChanges();

            return Json(sighting, JsonRequestBehavior.AllowGet);
        }

        // GET: Sightings/Create
        [HttpPost]
        public ActionResult CreateSighting()
        {
            var req = Request.InputStream;
            var json = new StreamReader(req).ReadToEnd();
            var result = JsonConvert.DeserializeObject<Sighting>(json);
            var bounds = JsonConvert.DeserializeObject<Bounds>(json);

            if (!IsSightingDuplicate(result, bounds))
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var sighting = new Sighting();
                sighting.Longitude = result.Longitude;
                sighting.Latitude = result.Latitude;
                sighting.AspNetUserId = userId;
                sighting.Type = result.Type;
                sighting.Rarity = result.Rarity;
                sighting.PokeMon = result.PokeMon;
                sighting.TimeOfDay = result.TimeOfDay;
                sighting.Notes = result.Notes;

                db.Sightings.Add(sighting);

                db.SaveChanges();

                Vote vote = new Vote();
                vote.SightingId = sighting.SightingId;
                vote.AspNetUserId = userId;
                vote.Action = VoteAction.Up;
                db.Votes.Add(vote);

                db.SaveChanges();

                var sightingMapped = new SightingDTO();

                sightingMapped.SightingId = sighting.SightingId;
                sightingMapped.AspNetUserId = sighting.AspNetUserId;
                sightingMapped.Latitude = sighting.Latitude;
                sightingMapped.Longitude = sighting.Longitude;
                sightingMapped.PokeMon = sighting.PokeMon != null ? sighting.PokeMon.GetDescription() : null;
                sightingMapped.Rarity = sighting.Rarity.GetDescription();
                sightingMapped.Rating = sighting.Rating;
                sightingMapped.SightingId = sighting.SightingId;
                sightingMapped.Type = sighting.Type.GetDescription();
                sightingMapped.TimeOfDay = sighting.TimeOfDay.ToString();
                sightingMapped.Notes = sighting.Notes;


                return Json(sightingMapped, JsonRequestBehavior.AllowGet);
            }
            else
                return new HttpStatusCodeResult(409, "This pokemon has already been added to this area");


        }

        bool IsSightingDuplicate(Sighting sighting, Bounds bounds)
        {
            var closeSimilar = db.Sightings.Where(xx => xx.PokeMon == sighting.PokeMon && xx.TimeOfDay == sighting.TimeOfDay && xx.Rating > -5 && xx.Latitude >= bounds.bottomLeftLat && xx.Longitude >= bounds.bottomLeftLong && xx.Latitude <= bounds.topRightLat && xx.Longitude <= bounds.topRightLong).Select(xx => new { Latitude = xx.Latitude, Longitude = xx.Longitude }).ToList();

            foreach (var closeSighting in closeSimilar)
            {
                var pokeMonCoOrds = new Coordinates() { Latitude = closeSighting.Latitude, Longitude = closeSighting.Longitude };
                var pointCoOrds = new Coordinates() { Latitude = sighting.Latitude, Longitude = sighting.Longitude };
                var proximity = HaversineDistance(pokeMonCoOrds, pointCoOrds, DistanceUnit.Kilometers);
                if (proximity < 0.025)
                    return true;

            }

            return false;

        }

        public double HaversineDistance(Coordinates pos1, Coordinates pos2, DistanceUnit unit)
        {
            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var lat = (pos2.Latitude - pos1.Latitude).ToRadians();
            var lng = (pos2.Longitude - pos1.Longitude).ToRadians();
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(pos1.Latitude.ToRadians()) * Math.Cos(pos2.Latitude.ToRadians()) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }


        // POST: Sightings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SightingId,Longitude,Latitude,Rarity,Type,AspNetUserId,Rating,PokeMon")] Sighting sighting)
        {
            if (ModelState.IsValid)
            {

                sighting.AspNetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                db.Sightings.Add(sighting);
                db.SaveChanges();
                return RedirectToAction("Index", "Home", null);
            }

            return View(sighting);
        }

        // GET: Sightings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sighting sighting = db.Sightings.Find(id);
            if (sighting == null)
            {
                return HttpNotFound();
            }
            return View(sighting);
        }

        // POST: Sightings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SightingId,Longitude,Latitude,Rarity,Type,AspNetUserId,Rating,PokeMon")] Sighting sighting)
        {
            if (ModelState.IsValid)
            {
                sighting.AspNetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

                db.Entry(sighting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sighting);
        }

        // GET: Sightings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sighting sighting = db.Sightings.Find(id);
            if (sighting == null)
            {
                return HttpNotFound();
            }
            return View(sighting);
        }

        // POST: Sightings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sighting sighting = db.Sightings.Find(id);
            if (sighting.AspNetUserId == System.Web.HttpContext.Current.User.Identity.GetUserId())
            {
                db.Sightings.Remove(sighting);
                db.SaveChanges();
                return Redirect("/");
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
