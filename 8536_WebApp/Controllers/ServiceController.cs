using _8536_WebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _8536_WebApp.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        //Hosted web API REST Service base url
        private readonly string Baseurl = "http://dcss8536api-dev.us-east-2.elasticbeanstalk.com/";

        // GET: Service
        public async Task<ActionResult> Index()
        {
            List<Service> ServiceInfo = new List<Service>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource Service using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Service");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Service list
                    ServiceInfo = JsonConvert.DeserializeObject<List<Service>>(PrResponse);
                }
                //returning the Service list to view
                return View(ServiceInfo);
            }
        }

        // GET: Service/Details/5
        public async Task<ActionResult> Details(int id)
        {
            

            Service service = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/service/{id}");

                if (result.IsSuccessStatusCode)
                {
                    service = await result.Content.ReadAsAsync<Service>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Service/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Description,Price,ServiceCategory")] Service service)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    var response = await client.PostAsJsonAsync("api/Service", service);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View(service);
        }


        // GET: Service/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Service serv = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                HttpResponseMessage Res = await client.GetAsync(string.Format("api/Service/{0}", id));

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Service list  
                    serv = JsonConvert.DeserializeObject<Service>(PrResponse);

                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(serv);
        }

        // POST: Service/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Service service)
        {
            try
            {
                // TODO: Add update logic here
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpResponseMessage Res = await client.GetAsync(string.Format("api/Service/{0}", id));
                    Service serverProd = null;
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var PrResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Service list  
                        serverProd = JsonConvert.DeserializeObject<Service>(PrResponse);
                    }
                    
                    //HTTP POST
                    var postTask = client.PutAsJsonAsync<Service>(string.Format("api/Service/{0}", service.ID), service);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View();
            }
        }




        // GET: Service/Delete/5
        public async Task<ActionResult> Delete(int id)
        {

            //Checking the response is successful or not which is sent using HttpClient 
            Service service = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/service/{id}");

                if (result.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    service = await result.Content.ReadAsAsync<Service>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }


        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var response = await client.DeleteAsync($"api/service/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View();
        }
    }
}
