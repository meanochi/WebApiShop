using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    public class OrderedSeatController : Controller
    {

        IOrderedSeatsService _service;

        public OrderedSeatController(IOrderedSeatsService service)
        {
            _service = service;
        }
        // GET: OrderedSeatController
        [HttpGet("showid/{showId}")]
        public Task<ActionResult<List<OrderedSeatReadDTO>>> getOrderedSeatsForShow(int showId)
        {
            List<OrderedSeatReadDTO> orderedSeats = 
        }

        // GET: OrderedSeatController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderedSeatController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderedSeatController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderedSeatController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderedSeatController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderedSeatController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderedSeatController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
