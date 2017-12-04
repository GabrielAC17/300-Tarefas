using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _300_Tarefas.Models;
using System.Web.Security;

namespace _300_Tarefas.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Usuarios
        public ActionResult Index()
        {
            return View(db.UsuarioSet.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.UsuarioSet.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        [AllowAnonymous]
        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create([Bind(Include = "Id,Login,Senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.UsuarioSet.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit()
        {
            Usuario usuario = RetornarLogado();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Login,Senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.UsuarioSet.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.UsuarioSet.Find(id);
            db.UsuarioSet.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login([Bind(Include = "Login,Senha")] Usuario usuario)
        {
            //Cria o objeto que vai guardar o possível usuario
            Usuario c = null;
            //Se foi enviado corretamente
            if (usuario != null)
            {
                //Se login e senha forem diferente de nulos
                if (usuario.Login != "" && usuario.Senha != "")
                {
                    //Procura no banco um login e senha igual e retorna o usuario
                    c = db.UsuarioSet.Where(l => l.Login == usuario.Login && l.Senha == usuario.Senha).FirstOrDefault();

                    //Se o usuario for encontrado
                    if (c != null)
                    {
                        //Cria cookie de autenticação
                        FormsAuthentication.SetAuthCookie(usuario.Login, false);
                        
                        return RedirectToAction("Index", "Tarefas");
                    }
                }
                //Em caso de algum erro
                TempData["Error"] = "Usuário ou senha incorretos!";
                ModelState.AddModelError("", "Usuário ou senha incorretos!");
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Usuarios");
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
