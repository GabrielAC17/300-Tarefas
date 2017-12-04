using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _300_Tarefas.Models;

namespace _300_Tarefas.Controllers
{
    [Authorize]
    public class TarefasController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Tarefas
        public ActionResult Index(int? id)
        {
            if (id == 0 || id == null)
                return View(db.TarefaSet.Where(x => x.Usuario.Login == User.Identity.Name).ToList());
            else if (id == 1)
                return View(db.TarefaSet.Where(x => x.Usuario.Login == User.Identity.Name && x.Concluido == true).ToList());
            else if (id == 2)
                return View(db.TarefaSet.Where(x => x.Usuario.Login == User.Identity.Name && x.Concluido == false).ToList());
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Tarefas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = db.TarefaSet.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            return View(tarefa);
        }

        // GET: Tarefas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tarefas/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Concluido")] Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                tarefa.Usuario = RetornarLogado();
                db.TarefaSet.Add(tarefa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tarefa);
        }

        // GET: Tarefas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = db.TarefaSet.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            return View(tarefa);
        }

        // POST: Tarefas/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Concluido")] Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarefa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tarefa);
        }

        // GET: Tarefas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = db.TarefaSet.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            return View(tarefa);
        }

        // POST: Tarefas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tarefa tarefa = db.TarefaSet.Find(id);
            db.TarefaSet.Remove(tarefa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public Usuario RetornarLogado()
        {
            Usuario c = null;
            c = db.UsuarioSet.Where(x => x.Login == User.Identity.Name).FirstOrDefault();
            return c;
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
