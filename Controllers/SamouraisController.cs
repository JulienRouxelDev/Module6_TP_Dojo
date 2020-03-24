using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using Dojo.Data;
using Dojo.Models;

namespace Dojo.Controllers
{
    public class SamouraisController : Controller
    {
        private Context db = new Context();

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            samourai.Potentiel = (samourai.Force + samourai.Arme.Degats) * (samourai.ArtMartial.Count() + 1);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            SamouraiVM samouraiVM = new SamouraiVM();
           
            samouraiVM.ArtMatiauxDisponible = db.ArtMartials.ToList();

            samouraiVM.ArmesDisponibles = ListeArmesDisponibles();

            return View(samouraiVM);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                //Samourai samourai = new Samourai();

                Arme arme = db.Armes.Find(samouraiVM.IdArme);

                //samourai.Force = samouraiVM.Samourai.Force;
                //samourai.Nom = samouraiVM.Samourai.Nom;
                samouraiVM.Samourai.Arme = arme;

                db.Samourais.Add(samouraiVM.Samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(samouraiVM);
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            
            if (samourai == null)
            {
                return HttpNotFound();
            }

            SamouraiVM samouraiVM = new SamouraiVM();
            samouraiVM.ArmesDisponibles = ListeArmesDisponibles();

            //si le samourai possede une arme on la rajoute à la liste
            if (samourai.Arme!=null)
            {
                samouraiVM.ArmesDisponibles.Add(samourai.Arme);
            }

            samouraiVM.ArtMatiauxDisponible = db.ArtMartials.ToList();

            samouraiVM.Samourai = samourai;
            if (samourai.Arme!=null)
            {
                samouraiVM.IdArme = samourai.Arme.Id;
            }

            if (samourai.ArtMartial.Count>0)
            {
                samouraiVM.IdsArtMartiaux = samourai.ArtMartial.Select(am => am.Id).ToList();
            }


            return View(samouraiVM);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                Samourai samouraiBd = db.Samourais.Find(samouraiVM.Samourai.Id);
                Arme arme = db.Armes.Find(samouraiVM.IdArme);

                samouraiBd.Force = samouraiVM.Samourai.Force;
                samouraiBd.Nom = samouraiVM.Samourai.Nom;
                samouraiBd.Arme = arme;
                samouraiBd.ArtMartial = db.ArtMartials.Where(am => samouraiVM.IdsArtMartiaux.Contains(am.Id)).ToList();

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(samouraiVM);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            db.Samourais.Remove(samourai);
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

        private List<Arme> ListeArmesDisponibles()
        {
            //Liste des armes existantes
            List<Arme> armesExistantes = db.Armes.ToList();

            //Liste des armes disponibles
            List<Arme> armesDisponibles = new List<Arme>();

            //Liste des armes utilisées
            List<Arme> armesUtilisees = db.Samourais.Select(s => s.Arme).ToList();

            foreach (var arme in armesExistantes)
            {
                if (arme!=null && !armesUtilisees.Contains(arme))
                {
                    armesDisponibles.Add(arme);
                }
            }

            return armesDisponibles;
        }
    }
}
