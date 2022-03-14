using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ASPNetForAndroid092021.Controllers
{
    public class ProductController : ApiController
    {


        [System.Web.Http.HttpGet]
        [Route("api/product/find/{moduleId}")]
        public HttpResponseMessage find(string moduleId)
        {
            C_SQL_Connect c_SQL = new C_SQL_Connect();
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(c_SQL.M_Select_Product(moduleId)));

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway);
            }
        }


        [System.Web.Http.HttpGet]
        [Route("api/product/getHistory/{moduleId}")]
        public HttpResponseMessage getHistory(string moduleId)
        {
            SQLGetHistoryProduct c_SQL = new SQLGetHistoryProduct();
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(c_SQL.GetHistory(moduleId)));

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway);
            }
        }

 

        [System.Web.Http.HttpGet]
        [Route("api/product/put/{moduleId}/{lokalizacja}/{ean}")]

        public HttpResponseMessage Put(string moduleId, string lokalizacja, string ean)
        {

            try
            {
                C_SQL_Connect c_SQL = new C_SQL_Connect();





                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(c_SQL.M_Update_Location(moduleId, lokalizacja, ean)));

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway);
            }

        }

      

        [System.Web.Http.HttpGet]
        [Route("api/product/putRemament/{moduleId}/{stan}/{pracownik}")]

        public HttpResponseMessage PutRemamanet(string moduleId, string stan, string pracownik)
        {

            try
            {
                C_SQL_Remament c_SQL = new C_SQL_Remament();





                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(c_SQL.M_Update_Location(moduleId, stan, pracownik)));

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway);
            }

        }

        [System.Web.Http.HttpGet]
        [Route("api/product/findRemament/{moduleId}")]
        public HttpResponseMessage findReamament(string moduleId)
        {
            C_SQL_Remament c_SQL = new C_SQL_Remament();
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(c_SQL.M_Select_Product(moduleId)));

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway);
            }
        }

        [System.Web.Http.HttpGet]
        [Route("api/product/getRemament/{moduleId}")]
        public HttpResponseMessage getRemament(string moduleId)
        {
            C_SQL_Remament c_SQL = new C_SQL_Remament();
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(c_SQL.M_Select_Product(moduleId)));

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return response;

            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway);
            }
        }

    }
}