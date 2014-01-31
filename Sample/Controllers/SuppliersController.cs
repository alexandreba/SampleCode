using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Sample;
using Sample.Models;
using WebApi.OutputCache.V2;
using WebApi.OutputCache.V2.TimeAttributes;

namespace Sample.Controllers
{
    [AutoInvalidateCacheOutput]
    public class SuppliersController : ApiController
    {
        private OBS_RECETTEEntities db = new OBS_RECETTEEntities();

        // GET api/Suppliers
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        [CacheOutputUntil(2020, 01, 25, 17, 00)]
        public IQueryable<Supplier> GetSuppliers(){
            return db.Suppliers;
            Product p = new Product();
            p.Distance = 100;            
        }        

         [CacheOutputUntil(2020, 01, 25, 17, 00)]
         public String GetDate()
         {
             return DateTime.Now.Ticks.ToString();
         }

        // GET api/Suppliers/5
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult GetSupplier(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier);
        }

        // PUT api/Suppliers/5
        public IHttpActionResult PutSupplier(int id, Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplier.SupplierId)
            {
                return BadRequest();
            }

            db.Entry(supplier).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Suppliers
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult PostSupplier(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Suppliers.Add(supplier);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = supplier.SupplierId }, supplier);
        }

        // DELETE api/Suppliers/5
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult DeleteSupplier(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            db.Suppliers.Remove(supplier);
            db.SaveChanges();

            return Ok(supplier);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupplierExists(int id)
        {
            return db.Suppliers.Count(e => e.SupplierId == id) > 0;
        }

        public void Post()
        {
            //reset the post data
        }
    }
}